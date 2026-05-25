// SPDX-License-Identifier: MIT
//
// Foundational layout/ABI service. This is intentionally minimal: it answers
// size/alignment questions for scalar LLVM types (and for pointer types,
// driven by the module datalayout when available) so callers can stop
// falling back to "assume i32" for unknown types. Aggregate layout, niche
// optimization, fat pointers, and Rust ABI lowering are roadmapped in
// docs/support-matrix.md and will plug into the same surface.

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rustlyn.Backend;

public readonly record struct TypeLayoutInfo(int SizeInBits, int AlignmentInBits, TypeLayoutCategory Category)
{
    public int SizeInBytes => (SizeInBits + 7) / 8;
    public int AlignmentInBytes => Math.Max(1, (AlignmentInBits + 7) / 8);
}

public enum TypeLayoutCategory
{
    Integer,
    Float,
    Pointer,
    Aggregate,
    Vector,
    Unknown,
}

public sealed class TypeLayoutService
{
    private readonly int _pointerSizeBits;
    private readonly int _pointerAlignBits;

    public TypeLayoutService(LoweredModuleMetadata? metadata = null)
    {
        var (size, align) = ParsePointerLayout(metadata?.DataLayout);
        _pointerSizeBits = size;
        _pointerAlignBits = align;
    }

    public int PointerSizeBits => _pointerSizeBits;
    public int PointerSizeBytes => _pointerSizeBits / 8;

    public bool TryGetLayout(string llvmType, out TypeLayoutInfo info)
    {
        info = default;
        if (string.IsNullOrWhiteSpace(llvmType))
        {
            return false;
        }

        var t = llvmType.Trim();
        if (t.EndsWith("*", StringComparison.Ordinal) || t == "ptr")
        {
            info = new TypeLayoutInfo(_pointerSizeBits, _pointerAlignBits, TypeLayoutCategory.Pointer);
            return true;
        }

        if (t.Length > 1 && (t[0] == 'i' || t[0] == 'I') && int.TryParse(t.AsSpan(1), NumberStyles.Integer, CultureInfo.InvariantCulture, out var bits) && bits > 0)
        {
            // LLVM ABI alignment for integers is min(size, pointer-size) rounded up to a power of two.
            var align = NaturalAlignForBits(bits);
            info = new TypeLayoutInfo(bits, align, TypeLayoutCategory.Integer);
            return true;
        }

        switch (t)
        {
            case "half":   info = new TypeLayoutInfo(16, 16, TypeLayoutCategory.Float); return true;
            case "bfloat": info = new TypeLayoutInfo(16, 16, TypeLayoutCategory.Float); return true;
            case "float":  info = new TypeLayoutInfo(32, 32, TypeLayoutCategory.Float); return true;
            case "double": info = new TypeLayoutInfo(64, 64, TypeLayoutCategory.Float); return true;
            case "fp128":
            case "ppc_fp128":
                info = new TypeLayoutInfo(128, 128, TypeLayoutCategory.Float); return true;
            case "x86_fp80":
                info = new TypeLayoutInfo(80, 128, TypeLayoutCategory.Float); return true;
            case "void":
                info = new TypeLayoutInfo(0, 8, TypeLayoutCategory.Unknown); return true;
        }

        if (TryLayoutAggregate(t, out info)) { return true; }
        if (TryLayoutArrayOrVector(t, out info)) { return true; }

        return false;
    }

    private bool TryLayoutAggregate(string t, out TypeLayoutInfo info)
    {
        info = default;
        // Strip optional "packed" wrapper: <{ ... }>
        var packed = t.StartsWith("<{", StringComparison.Ordinal) && t.EndsWith("}>", StringComparison.Ordinal);
        if (packed) { t = t.Substring(1, t.Length - 2); }
        if (!t.StartsWith("{", StringComparison.Ordinal) || !t.EndsWith("}", StringComparison.Ordinal))
        {
            return false;
        }

        var inner = t.Substring(1, t.Length - 2).Trim();
        if (inner.Length == 0)
        {
            info = new TypeLayoutInfo(0, 8, TypeLayoutCategory.Aggregate);
            return true;
        }

        var members = SplitTopLevel(inner, ',');
        var totalBits = 0;
        var maxAlign = 8;
        foreach (var memberRaw in members)
        {
            var member = memberRaw.Trim();
            if (!TryGetLayout(member, out var m))
            {
                return false;
            }
            var memberAlign = packed ? 8 : m.AlignmentInBits;
            // Pad current offset up to member alignment.
            totalBits = AlignUp(totalBits, memberAlign);
            totalBits += m.SizeInBits;
            if (memberAlign > maxAlign) { maxAlign = memberAlign; }
        }

        var structAlign = packed ? 8 : maxAlign;
        var aligned = AlignUp(totalBits, structAlign);
        info = new TypeLayoutInfo(aligned, structAlign, TypeLayoutCategory.Aggregate);
        return true;
    }

    private bool TryLayoutArrayOrVector(string t, out TypeLayoutInfo info)
    {
        info = default;
        var isVector = t.StartsWith("<", StringComparison.Ordinal) && t.EndsWith(">", StringComparison.Ordinal);
        var isArray = t.StartsWith("[", StringComparison.Ordinal) && t.EndsWith("]", StringComparison.Ordinal);
        if (!isVector && !isArray) { return false; }

        var inner = t.Substring(1, t.Length - 2).Trim();
        var xIdx = inner.IndexOf(" x ", StringComparison.Ordinal);
        if (xIdx <= 0) { return false; }

        var countText = inner.Substring(0, xIdx).Trim();
        var elemText = inner.Substring(xIdx + 3).Trim();
        if (!int.TryParse(countText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var count) || count < 0)
        {
            return false;
        }
        if (!TryGetLayout(elemText, out var elem)) { return false; }

        // For arrays, each element is padded up to element alignment.
        // For vectors, elements are packed but the whole vector takes natural alignment up to its bit-width.
        if (isVector)
        {
            var totalBits = count * elem.SizeInBits;
            var vAlign = NaturalAlignForBits(totalBits);
            info = new TypeLayoutInfo(totalBits, vAlign, TypeLayoutCategory.Vector);
            return true;
        }

        var stride = AlignUp(elem.SizeInBits, elem.AlignmentInBits);
        var arraySize = stride * count;
        info = new TypeLayoutInfo(arraySize, elem.AlignmentInBits, TypeLayoutCategory.Aggregate);
        return true;
    }

    private static int AlignUp(int bits, int alignment)
    {
        if (alignment <= 1) { return bits; }
        var rem = bits % alignment;
        return rem == 0 ? bits : bits + (alignment - rem);
    }

    private static IReadOnlyList<string> SplitTopLevel(string text, char separator)
    {
        var parts = new List<string>();
        var depthAngle = 0;
        var depthBrace = 0;
        var depthBracket = 0;
        var depthParen = 0;
        var start = 0;
        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];
            switch (c)
            {
                case '<': depthAngle++; break;
                case '>': if (depthAngle > 0) depthAngle--; break;
                case '{': depthBrace++; break;
                case '}': if (depthBrace > 0) depthBrace--; break;
                case '[': depthBracket++; break;
                case ']': if (depthBracket > 0) depthBracket--; break;
                case '(': depthParen++; break;
                case ')': if (depthParen > 0) depthParen--; break;
            }
            if (c == separator && depthAngle == 0 && depthBrace == 0 && depthBracket == 0 && depthParen == 0)
            {
                parts.Add(text.Substring(start, i - start));
                start = i + 1;
            }
        }
        if (start <= text.Length)
        {
            parts.Add(text.Substring(start));
        }
        return parts;
    }

    public TypeLayoutInfo GetLayoutOrUnknown(string llvmType)
        => TryGetLayout(llvmType, out var info)
            ? info
            : new TypeLayoutInfo(0, 0, TypeLayoutCategory.Unknown);

    private static int NaturalAlignForBits(int bits)
    {
        // Round bits up to next power of two, capped at 64-bit alignment for very wide ints.
        if (bits <= 8) return 8;
        if (bits <= 16) return 16;
        if (bits <= 32) return 32;
        if (bits <= 64) return 64;
        return 64;
    }

    private static (int Size, int Align) ParsePointerLayout(string? dataLayout)
    {
        // Defaults if no datalayout is available: 64-bit pointers, 64-bit alignment.
        var size = 64;
        var align = 64;
        if (string.IsNullOrWhiteSpace(dataLayout))
        {
            return (size, align);
        }

        foreach (var segment in dataLayout.Split('-', StringSplitOptions.RemoveEmptyEntries))
        {
            // Pointer entries look like "p:64:64" or "p0:64:64:64" — read first two ":" numbers.
            if (segment.Length > 0 && (segment[0] == 'p' || segment[0] == 'P'))
            {
                var colon = segment.IndexOf(':');
                if (colon < 0) continue;
                var rest = segment.Substring(colon + 1);
                var parts = rest.Split(':');
                if (parts.Length >= 1 && int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var s)) size = s;
                if (parts.Length >= 2 && int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var a)) align = a;
                return (size, align);
            }
        }

        return (size, align);
    }
}
