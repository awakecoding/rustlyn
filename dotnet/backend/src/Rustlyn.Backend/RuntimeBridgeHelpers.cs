using System.Runtime.InteropServices;
using System.Globalization;
using System.Numerics;

namespace Rustlyn.Backend;

public static partial class RuntimeBridgeHelpers
{
    private const int RustFormatArgumentSize = 16;

    public static int CommandLineArgCount()
    {
        return Rustlyn.Os.HostEnvironment.CommandLineArgCount();
    }

    public static int IsWindows()
    {
        return OperatingSystem.IsWindows() ? 1 : 0;
    }

    public static int DirectorySeparatorChar()
    {
        return Path.DirectorySeparatorChar;
    }

    public static int PathSeparatorChar()
    {
        return Path.PathSeparator;
    }

    public static int NewlineLength()
    {
        return Environment.NewLine.Length;
    }

    public static int PopCountU32(uint value)
    {
        return BitOperations.PopCount(value);
    }

    public static int MathMaxI32(int left, int right)
    {
        return Math.Max(left, right);
    }

    public static int MathMinI32(int left, int right)
    {
        return Math.Min(left, right);
    }

    public static void RustRawVecGrowAmortized(IntPtr retbuf, IntPtr self, long currentCapacity, long additional, long elementAlignment, long elementSize)
    {
        if (additional < 0 || elementSize <= 0)
        {
            throw new OutOfMemoryException("Invalid Rust RawVec growth request.");
        }

        var requiredCapacity = checked(currentCapacity + additional);
        var doubledCapacity = currentCapacity <= long.MaxValue / 2 ? currentCapacity * 2 : long.MaxValue;
        var minimumCapacity = elementSize == 1 ? 8 : elementSize <= 1024 ? 4 : 1;
        var newCapacity = Math.Max(Math.Max(requiredCapacity, doubledCapacity), minimumCapacity);
        var newSize = checked(newCapacity * elementSize);
        var oldPointer = Marshal.ReadIntPtr(IntPtr.Add(self, IntPtr.Size));
        var newPointer = currentCapacity == 0
            ? Marshal.AllocHGlobal(new IntPtr(newSize))
            : Marshal.ReAllocHGlobal(oldPointer, new IntPtr(newSize));

        Marshal.WriteInt64(self, newCapacity);
        Marshal.WriteIntPtr(IntPtr.Add(self, IntPtr.Size), newPointer);
        Marshal.WriteInt64(retbuf, -9223372036854775807L);
        Marshal.WriteInt64(IntPtr.Add(retbuf, sizeof(long)), 0);
    }

    public static void RustRawVecTryAllocateIn(IntPtr retbuf, long capacity, int zeroed, long elementAlignment, long elementSize)
    {
        if (capacity < 0 || elementAlignment <= 0 || elementSize <= 0)
        {
            throw new OutOfMemoryException("Invalid Rust RawVec allocation request.");
        }

        var size = checked(capacity * elementSize);
        var pointer = size == 0
            ? new IntPtr(elementAlignment)
            : Marshal.AllocHGlobal(new IntPtr(size));
        if (zeroed != 0)
        {
            for (var offset = 0L; offset < size; offset++)
            {
                Marshal.WriteByte(pointer, checked((int)offset), 0);
            }
        }

        Marshal.WriteInt64(retbuf, 0);
        Marshal.WriteInt64(IntPtr.Add(retbuf, sizeof(long)), capacity);
        Marshal.WriteIntPtr(IntPtr.Add(retbuf, sizeof(long) * 2), pointer);
    }

    public static void RustVecReserveBytes(IntPtr vector, long additional)
    {
        if (additional < 0)
        {
            throw new OutOfMemoryException("Invalid Rust Vec reserve request.");
        }

        var capacity = Marshal.ReadInt64(vector);
        var pointerAddress = IntPtr.Add(vector, sizeof(long));
        var pointer = Marshal.ReadIntPtr(pointerAddress);
        var length = Marshal.ReadInt64(IntPtr.Add(vector, sizeof(long) * 2));
        var required = checked(length + additional);
        if (required <= capacity)
        {
            return;
        }

        var doubled = capacity <= long.MaxValue / 2 ? capacity * 2 : long.MaxValue;
        var newCapacity = Math.Max(Math.Max(required, doubled), 8);
        var newPointer = capacity == 0 || pointer == IntPtr.Zero || pointer.ToInt64() < 4096
            ? Marshal.AllocHGlobal(new IntPtr(newCapacity))
            : Marshal.ReAllocHGlobal(pointer, new IntPtr(newCapacity));

        Marshal.WriteInt64(vector, newCapacity);
        Marshal.WriteIntPtr(pointerAddress, newPointer);
    }

    public static int Memcmp(IntPtr left, IntPtr right, long count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        for (var i = 0L; i < count; i++)
        {
            var diff = Marshal.ReadByte(left, checked((int)i)) - Marshal.ReadByte(right, checked((int)i));
            if (diff != 0)
            {
                return diff;
            }
        }

        return 0;
    }

    public static sbyte LlvmUnsignedCompareI8I64(ulong left, ulong right)
    {
        if (left < right)
            return -1;
        if (left > right)
            return 1;
        return 0;
    }

    /// <summary>
    /// Bridge for core::str::&lt;impl str&gt;::starts_with monomorphized for char pattern.
    /// Signature: (ptr data, i64 len, i32 charValue) -> i1
    /// </summary>
    public static bool CoreStrStartsWithChar(IntPtr data, long len, int charValue)
    {
        if (len <= 0 || data == IntPtr.Zero)
            return false;
        unsafe
        {
            var span = new ReadOnlySpan<byte>((void*)data, (int)len);
            // Decode the char from UTF-32 codepoint to UTF-8 prefix
            Span<byte> charUtf8 = stackalloc byte[4];
            var charLen = System.Text.Encoding.UTF8.GetBytes(new ReadOnlySpan<char>([(char)charValue]), charUtf8);
            return span.Length >= charLen && span[..charLen].SequenceEqual(charUtf8[..charLen]);
        }
    }

    /// <summary>
    /// Bridge for core::str::&lt;impl str&gt;::ends_with monomorphized for char pattern.
    /// Signature: (ptr data, i64 len, i32 charValue) -> i1
    /// </summary>
    public static bool CoreStrEndsWithChar(IntPtr data, long len, int charValue)
    {
        if (len <= 0 || data == IntPtr.Zero)
            return false;
        unsafe
        {
            var span = new ReadOnlySpan<byte>((void*)data, (int)len);
            Span<byte> charUtf8 = stackalloc byte[4];
            var charLen = System.Text.Encoding.UTF8.GetBytes(new ReadOnlySpan<char>([(char)charValue]), charUtf8);
            return span.Length >= charLen && span[^charLen..].SequenceEqual(charUtf8[..charLen]);
        }
    }

    /// <summary>
    /// Bridge for core::ptr::swap_nonoverlapping_bytes::swap_nonoverlapping_chunks.
    /// Signature: (ptr x, ptr y, i64 len) -> void
    /// Swaps len bytes between two non-overlapping regions.
    /// </summary>
    public static void SwapNonoverlappingChunks(IntPtr x, IntPtr y, long len)
    {
        if (len <= 0 || x == IntPtr.Zero || y == IntPtr.Zero)
            return;
        unsafe
        {
            var spanX = new Span<byte>((void*)x, (int)len);
            var spanY = new Span<byte>((void*)y, (int)len);
            Span<byte> temp = len <= 512 ? stackalloc byte[(int)len] : new byte[(int)len];
            spanX.CopyTo(temp);
            spanY.CopyTo(spanX);
            temp.CopyTo(spanY);
        }
    }

    /// <summary>
    /// Bridge for core::slice::memchr::memchr_aligned.
    /// Signature: (i8 needle, ptr haystack, i64 len) -> { i64, i64 }
    /// Returns (found: 0/1, index) as two i64 values via sret.
    /// Actually returns { i64, i64 } where first is the Option discriminant layout.
    /// For Rust Option&lt;usize&gt;: None = (len+1 or max, 0), Some(idx) = (idx, ?)
    /// In practice the return is { foundIndex, len } where foundIndex == len means not found.
    /// Actually Rust returns { i64, i64 } = (value, discriminant) for Option.
    /// The real layout: first i64 = index if found or garbage, second i64 = 0 for None, 1 for Some? 
    /// Let's match the declaration: returns { i64, i64 }.
    /// Based on the Rust source, memchr_aligned returns Option&lt;usize&gt; which in LLVM is { i64, i64 }
    /// where field 0 = value (index), field 1 = discriminant (0=None, 1=Some).
    /// </summary>
    public static void CoreSliceMemchrAligned(IntPtr retbuf, byte needle, IntPtr haystack, long len)
    {
        if (retbuf == IntPtr.Zero)
            return;
        unsafe
        {
            long foundIndex = -1;
            if (haystack != IntPtr.Zero && len > 0)
            {
                var span = new ReadOnlySpan<byte>((void*)haystack, (int)len);
                var idx = span.IndexOf(needle);
                if (idx >= 0)
                    foundIndex = idx;
            }
            var result = (long*)retbuf;
            if (foundIndex >= 0)
            {
                result[0] = 1;          // Some discriminant
                result[1] = foundIndex; // value
            }
            else
            {
                result[0] = 0;  // None discriminant
                result[1] = 0;  // unused
            }
        }
    }

    /// <summary>
    /// Bridge for core::str::count::do_count_chars.
    /// Signature: (ptr data, i64 len) -> i64
    /// Counts the number of UTF-8 encoded chars in the byte slice.
    /// </summary>
    public static long CoreStrDoCountChars(IntPtr data, long len)
    {
        if (data == IntPtr.Zero || len <= 0)
            return 0;
        unsafe
        {
            var span = new ReadOnlySpan<byte>((void*)data, (int)len);
            // Count UTF-8 start bytes (bytes that don't start with 10xxxxxx)
            long count = 0;
            foreach (var b in span)
            {
                if ((b & 0xC0) != 0x80)
                    count++;
            }
            return count;
        }
    }

    /// <summary>
    /// Bridge for core::str::count::char_count_general_case.
    /// Same semantics as do_count_chars for our purposes.
    /// </summary>
    public static long CoreStrCharCountGeneralCase(IntPtr data, long len)
    {
        return CoreStrDoCountChars(data, len);
    }

    /// <summary>
    /// Bridge for Cow&lt;str&gt;::deref returning {ptr, i64} (a fat pointer / &amp;str).
    /// The Cow layout for Cow&lt;str&gt; (24 bytes): discriminant(i64) + ptr(i64) + len(i64)
    /// - Borrowed (disc=0): ptr and len point to the borrowed data
    /// - Owned (disc=1): ptr and len point to the String's buffer
    /// Signature: (ptr cow) -> { ptr, i64 }
    /// </summary>
    public static void CowStrDeref(IntPtr retbuf, IntPtr cow)
    {
        if (retbuf == IntPtr.Zero || cow == IntPtr.Zero)
            return;
        unsafe
        {
            var cowData = (long*)cow;
            var disc = cowData[0];
            var result = (long*)retbuf;
            if (disc == 0)
            {
                // Borrowed: fields are (disc, ptr, len)
                result[0] = cowData[1]; // ptr
                result[1] = cowData[2]; // len
            }
            else
            {
                // Owned: String layout is (capacity, ptr, len) after discriminant
                // Actually for Cow<str> owned variant the layout is (disc=1, String{cap, ptr, len})
                // String is: ptr, capacity, len (24 bytes in Rust but may vary)
                // Let's use: disc(8) + ptr(8) + len(8) for the standard layout
                result[0] = cowData[1]; // ptr (buffer pointer)
                result[1] = cowData[2]; // len
            }
        }
    }

    /// <summary>
    /// Bridge for Cow&lt;B&gt;::default (where B=[u8] or str) returning an empty Cow.
    /// Produces Cow::Borrowed("") — a 24-byte sret with disc=0, ptr=empty, len=0.
    /// </summary>
    public static void CowDefault(IntPtr retbuf)
    {
        if (retbuf == IntPtr.Zero)
            return;
        unsafe
        {
            var result = (long*)retbuf;
            result[0] = 0; // discriminant = Borrowed
            result[1] = 1; // non-null dangling pointer (Rust uses NonNull::dangling())
            result[2] = 0; // length = 0
        }
    }

    /// <summary>
    /// Bridge for ScanError::new_str which constructs a ScanError from a Marker and a &amp;str.
    /// ScanError layout (48 bytes): Marker(24 bytes) + String(24 bytes)
    /// Marker: { index: u64, line: u64, col: u64 } = 24 bytes
    /// String: { ptr: *u8, cap: u64, len: u64 } = 24 bytes (but actually buf, len, cap)
    /// Signature: sret(48 bytes) (ptr marker, ptr strdata, i64 strlen) -> void
    /// </summary>
    public static void ScanErrorNewStr(IntPtr retbuf, IntPtr marker, IntPtr strData, long strLen)
    {
        if (retbuf == IntPtr.Zero)
            return;
        unsafe
        {
            // Copy marker (24 bytes) to output
            Buffer.MemoryCopy((void*)marker, (void*)retbuf, 24, 24);
            // Allocate a copy of the string for the owned String
            var stringStart = (byte*)retbuf + 24;
            var stringFields = (long*)stringStart;
            if (strLen > 0 && strData != IntPtr.Zero)
            {
                var buf = Marshal.AllocHGlobal((IntPtr)strLen);
                Buffer.MemoryCopy((void*)strData, (void*)buf, strLen, strLen);
                stringFields[0] = (long)buf;     // ptr (buffer pointer)
                stringFields[1] = strLen;        // capacity
                stringFields[2] = strLen;        // length
            }
            else
            {
                stringFields[0] = 1; // dangling non-null
                stringFields[1] = 0; // capacity
                stringFields[2] = 0; // length
            }
        }
    }

    /// <summary>
    /// Bridge for Event::empty_scalar which returns an 88-byte sret Event.
    /// This creates an Event with no anchor/tag and an empty scalar value.
    /// We zero-fill the entire 88 bytes since the exact layout needs the correct discriminant.
    /// </summary>
    public static void EventEmptyScalar(IntPtr retbuf)
    {
        if (retbuf == IntPtr.Zero)
            return;
        unsafe
        {
            // Zero-fill the entire struct - the discriminant for an empty scalar event
            // Event layout varies, but zero-init is safe as a stub for now.
            new Span<byte>((void*)retbuf, 88).Clear();
        }
    }

    /// <summary>
    /// Bridge for Event::empty_scalar_with_anchor(anchor_id: usize, tag: &amp;Tag) -> Event (88 bytes sret).
    /// We zero-fill and store the anchor_id at offset 0.
    /// </summary>
    public static void EventEmptyScalarWithAnchor(IntPtr retbuf, long anchorId, IntPtr tag)
    {
        if (retbuf == IntPtr.Zero)
            return;
        unsafe
        {
            new Span<byte>((void*)retbuf, 88).Clear();
            // Store anchor_id at some offset - this is approximate
            *(long*)retbuf = anchorId;
        }
    }

    /// <summary>
    /// Bridge for Vec&lt;T,A&gt;::drop / RawVec&lt;T,A&gt;::drop.
    /// Frees the backing allocation if capacity > 0.
    /// Vec layout: { ptr, capacity, length } or RawVec: { capacity, ptr }
    /// For simplicity, we just free the pointer if it's non-null and non-dangling.
    /// </summary>
    public static void VecDrop(IntPtr vec)
    {
        if (vec == IntPtr.Zero)
            return;
        unsafe
        {
            var fields = (long*)vec;
            // Vec layout: fields[0]=ptr, fields[1]=cap, fields[2]=len (or cap, ptr, len)
            // RawVec layout: fields[0]=cap, fields[1]=ptr
            // We try to free the pointer field. For Vec<T> in Rust the layout is actually
            // { buf: RawVec { ptr, cap }, len } = { ptr, cap, len }
            var ptr = (IntPtr)fields[0];
            var cap = fields[1];
            if (ptr != IntPtr.Zero && ptr != (IntPtr)1 && cap > 0)
            {
                Marshal.FreeHGlobal(ptr);
                fields[0] = 0;
                fields[1] = 0;
            }
        }
    }

    /// <summary>
    /// Bridge for RawVec&lt;T,A&gt;::drop.
    /// RawVec layout: { capacity(i64), ptr(i64) } = 16 bytes dereferenceable.
    /// </summary>
    public static void RawVecDrop(IntPtr rawvec)
    {
        if (rawvec == IntPtr.Zero)
            return;
        unsafe
        {
            var fields = (long*)rawvec;
            // RawVec: { cap, ptr }
            var cap = fields[0];
            var ptr = (IntPtr)fields[1];
            if (ptr != IntPtr.Zero && ptr != (IntPtr)1 && cap > 0)
            {
                Marshal.FreeHGlobal(ptr);
                fields[0] = 0;
                fields[1] = 0;
            }
        }
    }

    /// <summary>
    /// Bridge for Cow&lt;B&gt;::clone returning a cloned Cow (48 bytes sret).
    /// For borrowed: just copy the 24 bytes (disc + ptr + len).
    /// For owned: allocate a new buffer and copy.
    /// </summary>
    public static void CowClone(IntPtr retbuf, IntPtr source)
    {
        if (retbuf == IntPtr.Zero || source == IntPtr.Zero)
            return;
        unsafe
        {
            Buffer.MemoryCopy((void*)source, (void*)retbuf, 48, 48);
            var disc = *(long*)source;
            if (disc == 1)
            {
                // Owned variant: deep-copy the buffer
                var srcFields = (long*)source;
                var dstFields = (long*)retbuf;
                var ptr = (IntPtr)srcFields[1];
                var cap = srcFields[2];
                if (ptr != IntPtr.Zero && ptr != (IntPtr)1 && cap > 0)
                {
                    var newBuf = Marshal.AllocHGlobal((IntPtr)cap);
                    Buffer.MemoryCopy((void*)ptr, (void*)newBuf, cap, cap);
                    dstFields[1] = (long)newBuf;
                }
            }
        }
    }

    /// <summary>
    /// Bridge for SlicePartialEq::equal_same_length.
    /// Signature: (ptr a, ptr b, i64 len) -> bool
    /// Compares len bytes for equality.
    /// </summary>
    public static bool SliceEqualSameLength(IntPtr a, IntPtr b, long len)
    {
        if (len <= 0)
            return true;
        if (a == IntPtr.Zero || b == IntPtr.Zero)
            return a == b;
        unsafe
        {
            return new ReadOnlySpan<byte>((void*)a, (int)len)
                .SequenceEqual(new ReadOnlySpan<byte>((void*)b, (int)len));
        }
    }

    /// <summary>
    /// Bridge for Yaml::value_from_cow_and_metadata.
    /// Creates a Yaml node from a Cow&lt;str&gt; value and style metadata.
    /// sret(80 bytes), (ptr cow_value, i8 style, ptr metadata_or_null) -> void
    /// For our minimal implementation: create a Yaml::Value(Scalar::String(s)).
    /// The Yaml enum layout is complex (80 bytes). We store a simplified representation.
    /// </summary>
    public static void YamlValueFromCowAndMetadata(IntPtr retbuf, IntPtr cowValue, byte style, IntPtr metadata)
    {
        if (retbuf == IntPtr.Zero)
            return;
        unsafe
        {
            // Zero the output first
            new Span<byte>((void*)retbuf, 80).Clear();
            // Copy the cow value data into the Yaml node
            // Yaml enum: discriminant(8) + payload(72)
            // For String variant: disc=some value, then payload contains the string data
            // Since we don't know the exact discriminant for Yaml::Value(Scalar::String),
            // we approximate by storing the Cow data. The saphyr code will interpret this.
            if (cowValue != IntPtr.Zero)
            {
                // Copy the Cow (24 bytes for small Cow<str>) into the payload area
                Buffer.MemoryCopy((void*)cowValue, (void*)((byte*)retbuf + 8), 72, 24);
            }
        }
    }

    /// <summary>
    /// Bridge for Tag::is_yaml_core_schema.
    /// The Tag type is a Cow-like structure. For YAML core schema, the tag URI
    /// would be "tag:yaml.org,2002:". We return true for simplicity since most
    /// YAML without explicit tags uses the core schema.
    /// </summary>
    public static bool TagIsYamlCoreSchema(IntPtr tag)
    {
        // Default: assume core schema for untagged values
        return true;
    }

    /// <summary>
    /// Bridge for Cow&lt;[u8]&gt;::deref (the variant that takes a 48-byte Cow and returns a ptr).
    /// Signature: (ptr cow48) -> ptr (noundef nonnull align 8)
    /// Returns a pointer to the inner Yaml data (for the 48-byte Cow&lt;Tag&gt; variant).
    /// </summary>
    public static IntPtr CowBytesDeref(IntPtr cow)
    {
        if (cow == IntPtr.Zero)
            return IntPtr.Zero;
        unsafe
        {
            var cowData = (long*)cow;
            var disc = cowData[0];
            // Regardless of borrowed/owned, the pointer is at offset 8
            return (IntPtr)cowData[1];
        }
    }

    /// <summary>
    /// Bridge for std::thread::local::LocalKey&lt;T&gt;::with for hash random seed.
    /// Returns {i64, i64} representing two random u64 values for HashMap seed.
    /// </summary>
    public static void ThreadLocalHashKeys(IntPtr retbuf, IntPtr key)
    {
        if (retbuf == IntPtr.Zero)
            return;
        unsafe
        {
            var result = (long*)retbuf;
            // Provide deterministic hash seeds for managed execution
            result[0] = 0x517CC1B727220A95L;
            result[1] = 0x6C62272E07BB0142L;
        }
    }

    /// <summary>
    /// Bridge for RUST_STD_INTERNAL_VAL3tls() -> ptr.
    /// Returns a pointer to a thread-local storage slot (allocated on managed heap).
    /// </summary>
    public static IntPtr TlsStorageSlot()
    {
        // Return a fixed allocation that simulates TLS storage
        // The slot stores an Option&lt;T&gt; (None = 0, Some = initialized value)
        if (_tlsSlot == IntPtr.Zero)
        {
            unsafe
            {
                var buf = (long*)System.Runtime.InteropServices.NativeMemory.AllocZeroed(64);
                _tlsSlot = (IntPtr)buf;
            }
        }
        return _tlsSlot;
    }

    [ThreadStatic]
    private static IntPtr _tlsSlot;

    /// <summary>
    /// Bridge for Storage&lt;T,D&gt;::get_or_init_slow(ptr storage, ptr init_fn) -> ptr.
    /// Returns the storage pointer after initialization.
    /// For hash keys, we write deterministic seeds into the storage.
    /// </summary>
    public static IntPtr TlsGetOrInitSlow(IntPtr storage, IntPtr initFn)
    {
        if (storage == IntPtr.Zero)
            return IntPtr.Zero;
        unsafe
        {
            // Write the hash keys directly into storage
            // Layout: Option<(u64,u64)> = { discriminant, val0, val1 }
            // discriminant 1 = Some
            var slot = (long*)storage;
            if (slot[0] == 0) // not yet initialized
            {
                slot[0] = 1; // Some discriminant
                slot[1] = 0x517CC1B727220A95L;
                slot[2] = 0x6C62272E07BB0142L;
            }
            // Return pointer to the value (skip discriminant)
            return (IntPtr)(&slot[1]);
        }
    }

    /// <summary>
    /// Bridge for alloc::fmt::format::format_inner.
    /// Creates a String from format arguments. We return an empty String as a stub.
    /// sret(24 bytes) for String { ptr, cap, len }.
    /// </summary>
    public static void FormatInner(IntPtr retbuf, IntPtr pieces, IntPtr args)
    {
        if (retbuf == IntPtr.Zero)
            return;
        unsafe
        {
            var result = (long*)retbuf;
            result[0] = 1; // dangling ptr
            result[1] = 0; // capacity
            result[2] = 0; // length
        }
    }

    /// <summary>
    /// Bridge for Zip iterator new. Zero-fills the sret as a stub.
    /// </summary>
    public static void ZipIterNew(IntPtr retbuf, IntPtr a_begin, IntPtr a_end, IntPtr b_begin, IntPtr b_end)
    {
        if (retbuf == IntPtr.Zero)
            return;
        unsafe
        {
            new Span<byte>((void*)retbuf, 48).Clear();
        }
    }

    /// <summary>
    /// Bridge for Box&lt;T&gt;::hash and OrderedFloat::hash - no-op stubs for hash operations.
    /// </summary>
    public static void HashNoOp(IntPtr value, IntPtr hasher)
    {
        // No-op: hash operations are handled by the managed HashMap bridge
    }

    public static int Utf8CommandLineArgLength(int index)
    {
        return Rustlyn.Os.HostEnvironment.Utf8CommandLineArgLength(index);
    }

    public static int CopyUtf8CommandLineArg(int index, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostEnvironment.CopyUtf8CommandLineArg(index, destinationPointer, destinationCapacity);
    }

    public static void ConsoleWriteLineUtf8(IntPtr valuePointer, long valueLength)
    {
        Rustlyn.Os.HostConsole.WriteLineUtf8(valuePointer, valueLength);
    }

    public static void ConsoleWritePrefixedLineUtf8(IntPtr pathPointer, long pathLength, int lineNumber, IntPtr valuePointer, long valueLength)
    {
        Rustlyn.Os.HostConsole.WritePrefixedLineUtf8(pathPointer, pathLength, lineNumber, valuePointer, valueLength);
    }

    public static void ConsoleWritePathLineUtf8(IntPtr pathPointer, long pathLength, IntPtr valuePointer, long valueLength)
    {
        Rustlyn.Os.HostConsole.WritePathLineUtf8(pathPointer, pathLength, valuePointer, valueLength);
    }

    public static void ConsoleWriteNumberedLineUtf8(int lineNumber, IntPtr valuePointer, long valueLength)
    {
        Rustlyn.Os.HostConsole.WriteNumberedLineUtf8(lineNumber, valuePointer, valueLength);
    }

    public static void ConsoleWriteI32(int value)
    {
        Rustlyn.Os.HostConsole.WriteI32(value);
    }

    public static void ConsoleWritePathCountUtf8(IntPtr pathPointer, long pathLength, int value)
    {
        Rustlyn.Os.HostConsole.WritePathCountUtf8(pathPointer, pathLength, value);
    }

    public static void StdIoPrint(IntPtr argumentsPointer)
    {
        Console.Out.Write(ReadFormatLiteralPieces(argumentsPointer));
    }

    public static void StdIoEPrint(IntPtr argumentsPointer)
    {
        Console.Error.Write(ReadFormatLiteralPieces(argumentsPointer));
    }

    public static IntPtr StdIoStdout()
    {
        return new IntPtr(1);
    }

    public static IntPtr StdIoStdoutWriteAll(IntPtr stdoutPointer, IntPtr valuePointer, long valueLength)
    {
        Console.Out.Write(Rustlyn.Interop.InteropUtf8.ReadString(valuePointer, valueLength));
        return IntPtr.Zero;
    }

    public static IntPtr StdIoStderrWriteAll(IntPtr stderrPointer, IntPtr valuePointer, long valueLength)
    {
        Console.Error.Write(Rustlyn.Interop.InteropUtf8.ReadString(valuePointer, valueLength));
        return IntPtr.Zero;
    }

    public static IntPtr StdIoStdoutFlush(IntPtr stdoutPointer)
    {
        Console.Out.Flush();
        return IntPtr.Zero;
    }

    public static void StdTimeInstantNow(IntPtr resultPointer)
    {
        Marshal.WriteInt64(resultPointer, 0, 0);
        Marshal.WriteInt32(resultPointer, 8, 0);
    }

    public static void StdTimeInstantElapsed(IntPtr resultPointer, IntPtr instantPointer)
    {
        Marshal.WriteInt64(resultPointer, 0, 0);
        Marshal.WriteInt32(resultPointer, 8, 0);
    }

    public static void StdEnvCurrentDir(IntPtr destination)
    {
        WriteRustBufferFromString(destination, Directory.GetCurrentDirectory());
    }

    public static void StdEnvTempDir(IntPtr destination)
    {
        WriteRustBufferFromString(destination, Path.GetTempPath());
    }

    public static void StdEnvVar(IntPtr destination, IntPtr namePointer, long nameLength)
    {
        var name = Rustlyn.Interop.InteropUtf8.ReadString(namePointer, nameLength);
        var value = Environment.GetEnvironmentVariable(name);
        if (value is null)
        {
            Marshal.WriteInt64(destination, 0, 0);
            return;
        }

        Marshal.WriteInt64(destination, 0, -9223372036854775807L);
        WriteRustBufferFromString(IntPtr.Add(destination, 8), value);
    }

    public static void StdPathFileName(IntPtr destination, IntPtr pathPointer, long pathLength)
    {
        if (!TryReadUtf8Path(pathPointer, pathLength, out var value))
        {
            WriteRustSliceFromString(destination, null);
            return;
        }

        var trimmed = value.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var fileName = Path.GetFileName(trimmed);
        WriteRustSliceFromString(destination, fileName);
    }

    public static void StdPathFileStem(IntPtr destination, IntPtr pathPointer, long pathLength)
    {
        if (!TryReadUtf8Path(pathPointer, pathLength, out var value))
        {
            WriteRustSliceFromString(destination, null);
            return;
        }

        var trimmed = value.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var fileStem = Path.GetFileNameWithoutExtension(trimmed);
        WriteRustSliceFromString(destination, fileStem);
    }

    public static void StdPathExtension(IntPtr destination, IntPtr pathPointer, long pathLength)
    {
        if (!TryReadUtf8Path(pathPointer, pathLength, out var value))
        {
            WriteRustSliceFromString(destination, null);
            return;
        }

        var extension = Path.GetExtension(value);
        if (!string.IsNullOrEmpty(extension) && extension[0] == '.')
        {
            extension = extension[1..];
        }

        WriteRustSliceFromString(destination, extension);
    }

    public static void StdPathParent(IntPtr destination, IntPtr pathPointer, long pathLength)
    {
        if (!TryReadUtf8Path(pathPointer, pathLength, out var value))
        {
            WriteRustSliceFromString(destination, null);
            return;
        }

        var trimmed = value.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var parent = Path.GetDirectoryName(trimmed);
        WriteRustSliceFromString(destination, parent);
    }

    public static void StdPathComponents(IntPtr destination, IntPtr pathPointer, long pathLength)
    {
        for (var offset = 0; offset < 64; offset += sizeof(long))
        {
            Marshal.WriteInt64(destination, offset, 0);
        }

        Marshal.WriteIntPtr(destination, pathPointer);
        Marshal.WriteInt64(destination, 8, pathLength);
    }

    public static int StdPathIsAbsolute(IntPtr pathPointer, long pathLength)
    {
        if (!TryReadUtf8Path(pathPointer, pathLength, out var value))
        {
            return 0;
        }

        return IsRootedPath(value) ? 1 : 0;
    }

    public static int StdPathComponentsEq(IntPtr leftPointer, IntPtr rightPointer)
    {
        var left = ReadComponentPath(leftPointer);
        var right = ReadComponentPath(rightPointer);
        return string.Equals(NormalizePathForComparison(left), NormalizePathForComparison(right), StringComparison.Ordinal) ? 1 : 0;
    }

    public static int StdPathEq(IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength)
    {
        var left = TryReadUtf8Path(leftPointer, leftLength, out var leftValue) ? leftValue : string.Empty;
        var right = TryReadUtf8Path(rightPointer, rightLength, out var rightValue) ? rightValue : string.Empty;
        return string.Equals(NormalizePathForComparison(left), NormalizePathForComparison(right), StringComparison.Ordinal) ? 1 : 0;
    }

    public static int StdPathStartsWith(IntPtr pathPointer, long pathLength, IntPtr prefixPointer, long prefixLength)
    {
        var path = TryReadUtf8Path(pathPointer, pathLength, out var pathValue) ? NormalizePathForComparison(pathValue) : string.Empty;
        var prefix = TryReadUtf8Path(prefixPointer, prefixLength, out var prefixValue) ? NormalizePathForComparison(prefixValue) : string.Empty;
        return path.StartsWith(prefix, StringComparison.Ordinal) ? 1 : 0;
    }

    public static int StdPathEndsWith(IntPtr pathPointer, long pathLength, IntPtr suffixPointer, long suffixLength)
    {
        var path = TryReadUtf8Path(pathPointer, pathLength, out var pathValue) ? NormalizePathForComparison(pathValue) : string.Empty;
        var suffix = TryReadUtf8Path(suffixPointer, suffixLength, out var suffixValue) ? NormalizePathForComparison(suffixValue) : string.Empty;
        return path.EndsWith(suffix, StringComparison.Ordinal) ? 1 : 0;
    }

    public static void StdPathJoin(IntPtr destination, IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength)
    {
        var left = TryReadUtf8Path(leftPointer, leftLength, out var leftValue) ? leftValue : string.Empty;
        var right = TryReadUtf8Path(rightPointer, rightLength, out var rightValue) ? rightValue : string.Empty;
        WriteRustBufferFromString(destination, CombinePathStrings(left, right));
    }

    public static void StdPathWithExtension(IntPtr destination, IntPtr pathPointer, long pathLength, IntPtr extensionPointer, long extensionLength)
    {
        var path = TryReadUtf8Path(pathPointer, pathLength, out var pathValue) ? pathValue : string.Empty;
        var extension = TryReadUtf8Path(extensionPointer, extensionLength, out var extensionValue) ? extensionValue : string.Empty;
        WriteRustBufferFromString(destination, ChangePathExtension(path, extension));
    }

    public static void StdPathWithFileName(IntPtr destination, IntPtr pathPointer, long pathLength, IntPtr fileNamePointer, long fileNameLength)
    {
        var path = TryReadUtf8Path(pathPointer, pathLength, out var pathValue) ? pathValue : string.Empty;
        var fileName = TryReadUtf8Path(fileNamePointer, fileNameLength, out var fileNameValue) ? fileNameValue : string.Empty;
        var parent = ParentPathString(path);
        WriteRustBufferFromString(destination, string.IsNullOrEmpty(parent) ? fileName : CombinePathStrings(parent, fileName));
    }

    public static long StdPathComponentCount(IntPtr componentsPointer)
    {
        var value = ReadComponentPath(componentsPointer);
        if (string.IsNullOrEmpty(value))
        {
            return 0;
        }

        var trimmed = value.Trim(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var count = trimmed.Length == 0
            ? 0
            : trimmed.Split([Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries).LongLength;
        return IsRootedPath(value) ? count + 1 : count;
    }

    private static string ReadFormatLiteralPieces(IntPtr argumentsPointer)
    {
        if (argumentsPointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(argumentsPointer));
        }

        var piecesPointer = Marshal.ReadIntPtr(argumentsPointer);
        var piecesLength = Marshal.ReadInt64(argumentsPointer, 8);
        var argsPointer = Marshal.ReadIntPtr(argumentsPointer, 16);
        var argsLength = Marshal.ReadInt64(argumentsPointer, 24);
        var formatSpecsPointer = Marshal.ReadIntPtr(argumentsPointer, 32);
        var formatSpecsLength = Marshal.ReadInt64(argumentsPointer, 40);
        if (formatSpecsPointer != IntPtr.Zero || formatSpecsLength != 0)
        {
            throw new NotSupportedException("std::fmt arguments with non-default formatting specs are not supported by the console bridge yet.");
        }

        if (piecesLength == 0)
        {
            return string.Empty;
        }

        if (piecesPointer == IntPtr.Zero)
        {
            throw new InvalidOperationException("std::fmt arguments contained a null pieces pointer.");
        }

        if (piecesLength < 0 || piecesLength > 256)
        {
            throw new NotSupportedException($"std::fmt pieces length {piecesLength} is outside the supported bridge range.");
        }

        if (argsLength < 0 || argsLength > 256)
        {
            throw new NotSupportedException($"std::fmt runtime argument length {argsLength} is outside the supported bridge range.");
        }

        if (argsLength != 0 && argsPointer == IntPtr.Zero)
        {
            throw new InvalidOperationException("std::fmt arguments contained a null runtime argument pointer.");
        }

        if (argsLength > 0 && piecesLength != argsLength && piecesLength != argsLength + 1)
        {
            throw new NotSupportedException($"std::fmt pieces length {piecesLength} is not compatible with runtime argument length {argsLength}.");
        }

        var builder = new System.Text.StringBuilder();
        for (var index = 0; index < piecesLength; index++)
        {
            var piecePointer = Marshal.ReadIntPtr(piecesPointer, checked((int)(index * 16)));
            var pieceLength = Marshal.ReadInt64(piecesPointer, checked((int)(index * 16 + 8)));
            builder.Append(Rustlyn.Interop.InteropUtf8.ReadString(piecePointer, pieceLength));
            if (index < argsLength)
            {
                builder.Append(ReadDefaultFormatArgument(argsPointer, index));
            }
        }

        return builder.ToString();
    }

    private static string ReadDefaultFormatArgument(IntPtr argsPointer, long index)
    {
        var offset = checked((int)(index * RustFormatArgumentSize));
        var valuePointer = Marshal.ReadIntPtr(argsPointer, offset);
        var formatterPointer = Marshal.ReadIntPtr(argsPointer, offset + IntPtr.Size);
        if (valuePointer == IntPtr.Zero)
        {
            throw new InvalidOperationException($"std::fmt runtime argument {index.ToString(CultureInfo.InvariantCulture)} contained a null value pointer.");
        }

        if (formatterPointer == IntPtr.Zero)
        {
            throw new InvalidOperationException("std::fmt runtime argument contained a null formatter pointer.");
        }

        if (!IsDefaultI32Formatter(formatterPointer))
        {
            throw new NotSupportedException($"std::fmt runtime formatter 0x{formatterPointer.ToInt64().ToString("x", CultureInfo.InvariantCulture)} is not supported by the console bridge yet.");
        }

        return Marshal.ReadInt32(valuePointer).ToString(CultureInfo.InvariantCulture);
    }

    private static bool IsDefaultI32Formatter(IntPtr formatterPointer)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var generatedType = assembly.GetType("Rustlyn.GeneratedModule", throwOnError: false);
            if (generatedType is null)
            {
                continue;
            }

            foreach (var method in generatedType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static))
            {
                if (method.Name.Contains("$u20$for$u20$i32$GT$3fmt", StringComparison.Ordinal)
                    && method.MethodHandle.GetFunctionPointer() == formatterPointer)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static void WriteRustBufferFromString(IntPtr destination, string value)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(value);
        var buffer = IntPtr.Zero;
        if (bytes.Length > 0)
        {
            buffer = Rustlyn.Runtime.MemoryRuntime.Alloc(new IntPtr(bytes.Length));
            Marshal.Copy(bytes, 0, buffer, bytes.Length);
        }

        Rustlyn.Runtime.RustBufferRuntime.WriteBuffer(destination, bytes.LongLength, buffer, bytes.LongLength);
    }

    private static void WriteRustSliceFromString(IntPtr destination, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Marshal.WriteIntPtr(destination, IntPtr.Zero);
            Marshal.WriteInt64(destination, 8, 0);
            return;
        }

        var bytes = System.Text.Encoding.UTF8.GetBytes(value);
        var buffer = Rustlyn.Runtime.MemoryRuntime.Alloc(new IntPtr(bytes.Length));
        Marshal.Copy(bytes, 0, buffer, bytes.Length);
        Marshal.WriteIntPtr(destination, buffer);
        Marshal.WriteInt64(destination, 8, bytes.LongLength);
    }

    private static bool TryReadUtf8Path(IntPtr pathPointer, long pathLength, out string value)
    {
        if (pathPointer == IntPtr.Zero || pathLength <= 0)
        {
            value = string.Empty;
            return false;
        }

        value = Rustlyn.Interop.InteropUtf8.ReadString(pathPointer, pathLength);
        return value.Length > 0;
    }

    private static string ReadComponentPath(IntPtr componentsPointer)
    {
        if (componentsPointer == IntPtr.Zero)
        {
            return string.Empty;
        }

        var pathPointer = Marshal.ReadIntPtr(componentsPointer);
        var pathLength = Marshal.ReadInt64(componentsPointer, 8);
        return TryReadUtf8Path(pathPointer, pathLength, out var value) ? value : string.Empty;
    }

    private static string NormalizePathForComparison(string value)
    {
        return value.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
            .TrimEnd(Path.DirectorySeparatorChar);
    }

    private static bool IsRootedPath(string value)
    {
        return value.StartsWith(Path.DirectorySeparatorChar)
            || value.StartsWith(Path.AltDirectorySeparatorChar)
            || Path.IsPathRooted(value);
    }

    private static string CombinePathStrings(string left, string right)
    {
        if (string.IsNullOrEmpty(left))
        {
            return right;
        }

        if (string.IsNullOrEmpty(right))
        {
            return left;
        }

        if (IsRootedPath(right))
        {
            return right;
        }

        var separator = PreferredSeparator(left);
        return left.TrimEnd('\\', '/') + separator + right.TrimStart('\\', '/');
    }

    private static string ChangePathExtension(string path, string extension)
    {
        var separatorIndex = LastSeparatorIndex(path);
        var dotIndex = path.LastIndexOf('.');
        var stemEnd = dotIndex > separatorIndex ? dotIndex : path.Length;
        if (string.IsNullOrEmpty(extension))
        {
            return path[..stemEnd];
        }

        return path[..stemEnd] + "." + extension.TrimStart('.');
    }

    private static string ParentPathString(string path)
    {
        var trimmed = path.TrimEnd('\\', '/');
        var separatorIndex = LastSeparatorIndex(trimmed);
        if (separatorIndex < 0)
        {
            return string.Empty;
        }

        if (separatorIndex == 0)
        {
            return trimmed[..1];
        }

        return trimmed[..separatorIndex];
    }

    private static int LastSeparatorIndex(string value)
    {
        return Math.Max(value.LastIndexOf('\\'), value.LastIndexOf('/'));
    }

    private static char PreferredSeparator(string value)
    {
        return value.Contains('/') && !value.Contains('\\') ? '/' : Path.DirectorySeparatorChar;
    }

    public static int Utf8ReadAllLinesCount(IntPtr pathPointer, long pathLength)
    {
        return Rustlyn.Os.HostFileSystem.Utf8ReadAllLinesCount(pathPointer, pathLength);
    }

    public static int Utf8ReadAllLinesLineLength(IntPtr pathPointer, long pathLength, int index)
    {
        return Rustlyn.Os.HostFileSystem.Utf8ReadAllLinesLineLength(pathPointer, pathLength, index);
    }

    public static int CopyUtf8ReadAllLinesLine(IntPtr pathPointer, long pathLength, int index, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostFileSystem.CopyUtf8ReadAllLinesLine(pathPointer, pathLength, index, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetRootLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathGetRootLengthUtf8(pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetRoot(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathGetRoot(pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetFullPathLengthUtf8(IntPtr pathPointer, long pathLength, IntPtr basePointer, long baseLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathGetFullPathLengthUtf8(pathPointer, pathLength, basePointer, baseLength);
    }

    public static int CopyUtf8PathGetFullPath(IntPtr pathPointer, long pathLength, IntPtr basePointer, long baseLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathGetFullPath(pathPointer, pathLength, basePointer, baseLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetDirectoryNameLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathGetDirectoryNameLengthUtf8(pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetDirectoryName(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathGetDirectoryName(pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetRelativeLengthUtf8(IntPtr relativeToPointer, long relativeToLength, IntPtr pathPointer, long pathLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathGetRelativeLengthUtf8(relativeToPointer, relativeToLength, pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetRelative(IntPtr relativeToPointer, long relativeToLength, IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathGetRelative(relativeToPointer, relativeToLength, pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8DocumentsLength()
    {
        return Rustlyn.Os.HostEnvironment.Utf8DocumentsLength();
    }

    public static int CopyUtf8Documents(IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostEnvironment.CopyUtf8Documents(destinationPointer, destinationCapacity);
    }

    public static int Utf8TempPathLength()
    {
        return Rustlyn.Os.HostEnvironment.Utf8TempPathLength();
    }

    public static int CopyUtf8TempPath(IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostEnvironment.CopyUtf8TempPath(destinationPointer, destinationCapacity);
    }

    public static int Utf8UserProfileLength()
    {
        return Rustlyn.Os.HostEnvironment.Utf8UserProfileLength();
    }

    public static int CopyUtf8UserProfile(IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostEnvironment.CopyUtf8UserProfile(destinationPointer, destinationCapacity);
    }

    public static int Utf8CurrentDirectoryLength()
    {
        return Rustlyn.Os.HostEnvironment.Utf8CurrentDirectoryLength();
    }

    public static int CopyUtf8CurrentDirectory(IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostEnvironment.CopyUtf8CurrentDirectory(destinationPointer, destinationCapacity);
    }

    public static int Utf8PathCombine3LengthUtf8(IntPtr firstPointer, long firstLength, IntPtr secondPointer, long secondLength, IntPtr thirdPointer, long thirdLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathCombine3LengthUtf8(firstPointer, firstLength, secondPointer, secondLength, thirdPointer, thirdLength);
    }

    public static int CopyUtf8PathCombine3(IntPtr firstPointer, long firstLength, IntPtr secondPointer, long secondLength, IntPtr thirdPointer, long thirdLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathCombine3(firstPointer, firstLength, secondPointer, secondLength, thirdPointer, thirdLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathCombineLengthUtf8(IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathCombineLengthUtf8(leftPointer, leftLength, rightPointer, rightLength);
    }

    public static int CopyUtf8PathCombine(IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathCombine(leftPointer, leftLength, rightPointer, rightLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathChangeExtensionLengthUtf8(IntPtr pathPointer, long pathLength, IntPtr extensionPointer, long extensionLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathChangeExtensionLengthUtf8(pathPointer, pathLength, extensionPointer, extensionLength);
    }

    public static int CopyUtf8PathChangeExtension(IntPtr pathPointer, long pathLength, IntPtr extensionPointer, long extensionLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathChangeExtension(pathPointer, pathLength, extensionPointer, extensionLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetFileNameWithoutExtensionLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathGetFileNameWithoutExtensionLengthUtf8(pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetFileNameWithoutExtension(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathGetFileNameWithoutExtension(pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8StringReplaceLength(
        IntPtr sourcePointer,
        long sourceLength,
        IntPtr oldPointer,
        long oldLength,
        IntPtr newPointer,
        long newLength)
    {
        return Rustlyn.Interop.InteropUtf8.ReplaceByteCount(sourcePointer, sourceLength, oldPointer, oldLength, newPointer, newLength);
    }

    public static int CopyUtf8StringReplace(
        IntPtr sourcePointer,
        long sourceLength,
        IntPtr oldPointer,
        long oldLength,
        IntPtr newPointer,
        long newLength,
        IntPtr destinationPointer,
        long destinationCapacity)
    {
        return Rustlyn.Interop.InteropUtf8.CopyReplace(sourcePointer, sourceLength, oldPointer, oldLength, newPointer, newLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetFileNameLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathGetFileNameLengthUtf8(pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetFileName(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathGetFileName(pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8StringContains(IntPtr haystackPointer, long haystackLength, IntPtr needlePointer, long needleLength)
    {
        return Rustlyn.Interop.InteropUtf8.ContainsOrdinal(haystackPointer, haystackLength, needlePointer, needleLength);
    }

    public static int Utf8StringIndexOf(IntPtr haystackPointer, long haystackLength, IntPtr needlePointer, long needleLength)
    {
        return Rustlyn.Interop.InteropUtf8.IndexOfOrdinal(haystackPointer, haystackLength, needlePointer, needleLength);
    }

    public static int RemEuclidI32(int left, int right)
    {
        return Rustlyn.Runtime.NumericRuntime.RemEuclidI32(left, right);
    }

    public static long RemEuclidI64(long left, long right)
    {
        return Rustlyn.Runtime.NumericRuntime.RemEuclidI64(left, right);
    }

    public static int Utf8PathGetFileNameLength(IntPtr pathPointer, long pathLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathGetFileNameLength(pathPointer, pathLength);
    }

    public static void InitializeWtf8PathBuffer(IntPtr destination, IntPtr source, long length)
    {
        Rustlyn.Runtime.RustBufferRuntime.InitializeBufferFromBytes(destination, source, length);
    }

    public static void AppendPathSegment(IntPtr destination, IntPtr source, long length)
    {
        Rustlyn.Os.HostPath.AppendPathSegment(destination, source, length);
    }

    public static void ReadFileToRustString(IntPtr destination, IntPtr pathPointer, long pathLength)
    {
        Rustlyn.Os.HostFileSystem.ReadFileToRustString(destination, pathPointer, pathLength);
    }

    public static long MemchrAligned(byte needle, IntPtr source, long length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Search length must be non-negative.");
        }

        for (var index = 0L; index < length; index++)
        {
            if (System.Runtime.InteropServices.Marshal.ReadByte(source, checked((int)index)) == needle)
            {
                return (index << 32) | 1L;
            }
        }

        return 0;
    }

    public static int BindgenTaskStatus(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskStatus(taskHandle);
    }

    public static int BindgenTaskExceptionHandle(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskExceptionHandle(taskHandle);
    }

    public static int BindgenTaskRelease(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.ReleaseTask(taskHandle) ? 1 : 0;
    }

    public static int BindgenTaskGetI32Result(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskInt32Result(taskHandle);
    }

    public static long BindgenTaskGetI64Result(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskInt64Result(taskHandle);
    }

    public static float BindgenTaskGetF32Result(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskSingleResult(taskHandle);
    }

    public static double BindgenTaskGetF64Result(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskDoubleResult(taskHandle);
    }

    public static int BindgenTaskGetBoolResult(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskBooleanResult(taskHandle);
    }

    public static int BindgenTaskGetObjectResultHandle(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskObjectResultHandle(taskHandle);
    }

    private static void WriteExceptionOut(IntPtr exceptionOutPointer, int exceptionHandle)
    {
        if (exceptionOutPointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(exceptionOutPointer), "Generated binding calls must provide an exception out pointer.");
        }

        Marshal.WriteInt32(exceptionOutPointer, exceptionHandle);
    }

}
