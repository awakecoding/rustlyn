// SPDX-License-Identifier: MIT
//
// Helper that answers "does this Rust enum variant qualify for niche-optimized
// layout?" without requiring callers to know the full Rust ABI. The helper is
// deliberately scoped to the common cases that surface in fixtures today:
//
//   - Option<&T> / Option<&mut T>   -> null pointer encodes None
//   - Option<NonZero*>              -> zero encodes None
//   - Option<bool>                  -> reserved bit pattern encodes None
//   - Option<u32>                   -> no niche, needs discriminant
//
// Aggregate niche optimization (e.g., Option<Option<&T>> with two niches) and
// arbitrary user enums with `#[repr]` annotations are still roadmapped. The
// emitter does not consume this helper yet — Tier 3 will wire it through the
// aggregate layout path. Adding it now gives the layout engine an explicit
// API surface and a regression base.

using System;

namespace Rustlyn.Backend;

public enum NicheCarrierKind
{
    None,
    NullablePointer,
    NonZeroInteger,
    NarrowBool,
    Custom,
}

public readonly record struct NicheLayoutDecision(
    bool NeedsDiscriminant,
    NicheCarrierKind Carrier,
    string Description)
{
    public static NicheLayoutDecision NeedsDiscriminantSlot(string reason)
        => new(true, NicheCarrierKind.None, reason);

    public static NicheLayoutDecision NicheCarrier(NicheCarrierKind carrier, string description)
        => new(false, carrier, description);
}

public static class NicheLayoutPolicy
{
    /// <summary>
    /// Decides whether the single-payload enum <c>Option&lt;TPayload&gt;</c> needs an explicit
    /// discriminant slot, or whether the payload itself carries a reserved bit pattern that
    /// can encode the <c>None</c> variant.
    /// </summary>
    public static NicheLayoutDecision OptionLikeFor(string payloadType)
    {
        if (string.IsNullOrWhiteSpace(payloadType))
        {
            return NicheLayoutDecision.NeedsDiscriminantSlot("payload type missing");
        }

        var t = payloadType.Trim();

        // References and raw pointers can use the null bit-pattern as the None tag.
        if (t.EndsWith("*", StringComparison.Ordinal) || t == "ptr"
            || t.StartsWith("&", StringComparison.Ordinal))
        {
            return NicheLayoutDecision.NicheCarrier(NicheCarrierKind.NullablePointer,
                $"{t}: null pointer encodes None");
        }

        // Rust's NonZero* types. Allow both managed-style and LLVM-style spellings.
        if (IsNonZeroInteger(t))
        {
            return NicheLayoutDecision.NicheCarrier(NicheCarrierKind.NonZeroInteger,
                $"{t}: zero encodes None");
        }

        // bool is i1 in LLVM with values restricted to 0/1; reserved bit pattern (e.g. 0x02) can tag None.
        if (t == "i1" || t == "bool")
        {
            return NicheLayoutDecision.NicheCarrier(NicheCarrierKind.NarrowBool,
                "bool: reserved bit pattern encodes None");
        }

        // Plain integers and floats have no spare values; need explicit discriminant.
        if (IsPlainScalar(t))
        {
            return NicheLayoutDecision.NeedsDiscriminantSlot($"{t}: no spare bit pattern, discriminant required");
        }

        // Unknown / user-defined: be conservative.
        return NicheLayoutDecision.NeedsDiscriminantSlot($"{t}: niche analysis not yet implemented for this type");
    }

    private static bool IsNonZeroInteger(string t)
    {
        // Accept "NonZeroU32", "NonZeroI64", "core::num::NonZeroUsize", etc.
        var idx = t.IndexOf("NonZero", StringComparison.Ordinal);
        return idx >= 0 && (idx == 0 || t[idx - 1] == ':' || t[idx - 1] == '.');
    }

    private static bool IsPlainScalar(string t)
    {
        if (t.Length > 1 && (t[0] == 'i' || t[0] == 'I'))
        {
            for (var i = 1; i < t.Length; i++)
            {
                if (!char.IsDigit(t[i])) { return false; }
            }
            return true;
        }
        return t is "half" or "bfloat" or "float" or "double" or "fp128" or "x86_fp80" or "ppc_fp128"
            or "u8" or "u16" or "u32" or "u64" or "u128" or "i8" or "i16" or "i32" or "i64" or "i128"
            or "usize" or "isize" or "f32" or "f64";
    }
}
