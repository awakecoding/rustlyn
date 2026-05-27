// SPDX-License-Identifier: MIT
//
// Codifies which LLVM atomic ordering maps to which .NET memory primitive.
// This is the source of truth referenced by docs/support-matrix.md and the
// backend emitter intrinsic dispatch. Adding new mappings should always
// land here first and then surface in the documentation row.
//
// LLVM atomic ordering ladder (weakest -> strongest):
//   Unordered, Monotonic (relaxed), Acquire, Release, AcqRel, SeqCst.
//
// .NET tooling does not expose every level individually. The pragmatic
// mapping below preserves the *observable* guarantees of each ordering on
// .NET 8+:
//
//   Unordered/Monotonic    -> Volatile.Read/Write
//                              (atomic single-word access, no ordering)
//   Acquire                -> Volatile.Read  + acquire fence implicit in Volatile.Read
//   Release                -> Volatile.Write + release fence implicit in Volatile.Write
//   AcqRel                 -> Interlocked.*  (RMW with full barrier)
//   SeqCst                 -> Interlocked.* preceded/followed by Thread.MemoryBarrier
//                              for plain loads/stores, otherwise Interlocked alone.
//
// Compare-and-swap and RMW operations always go through Interlocked.* regardless
// of the requested ordering, because Interlocked.* on .NET is documented as
// sequentially consistent.

namespace Rustlyn.Backend;

public enum AtomicOrdering
{
    Unordered,
    Monotonic,
    Acquire,
    Release,
    AcqRel,
    SeqCst,
}

public enum AtomicLoweringStrategy
{
    /// <summary>Single-word atomic access with no ordering guarantee.</summary>
    VolatileAccess,

    /// <summary>Volatile access whose underlying intrinsic carries an acquire fence.</summary>
    VolatileAcquire,

    /// <summary>Volatile access whose underlying intrinsic carries a release fence.</summary>
    VolatileRelease,

    /// <summary>Read-modify-write through System.Threading.Interlocked.</summary>
    InterlockedRmw,

    /// <summary>Plain access combined with an explicit full memory barrier.</summary>
    BarrierFenced,
}

public readonly record struct AtomicLoweringDecision(
    AtomicOrdering Ordering,
    AtomicLoweringStrategy Strategy,
    string Rationale);

public static class AtomicOrderingMap
{
    /// <summary>
    /// Map an LLVM ordering keyword onto the enum. Returns false for unknown spellings.
    /// </summary>
    public static bool TryParse(string ordering, out AtomicOrdering parsed)
    {
        switch ((ordering ?? string.Empty).Trim().ToLowerInvariant())
        {
            case "unordered":  parsed = AtomicOrdering.Unordered;  return true;
            case "monotonic":
            case "relaxed":    parsed = AtomicOrdering.Monotonic;  return true;
            case "acquire":    parsed = AtomicOrdering.Acquire;    return true;
            case "release":    parsed = AtomicOrdering.Release;    return true;
            case "acq_rel":
            case "acqrel":     parsed = AtomicOrdering.AcqRel;     return true;
            case "seq_cst":
            case "seqcst":     parsed = AtomicOrdering.SeqCst;     return true;
            default:           parsed = AtomicOrdering.SeqCst;     return false;
        }
    }

    /// <summary>
    /// Strategy for a plain atomic load with the supplied ordering.
    /// </summary>
    public static AtomicLoweringDecision ForLoad(AtomicOrdering ordering) => ordering switch
    {
        AtomicOrdering.Unordered or AtomicOrdering.Monotonic
            => new(ordering, AtomicLoweringStrategy.VolatileAccess, "single-word atomic load, no ordering"),
        AtomicOrdering.Acquire
            => new(ordering, AtomicLoweringStrategy.VolatileAcquire, "Volatile.Read carries acquire semantics on .NET"),
        AtomicOrdering.SeqCst
            => new(ordering, AtomicLoweringStrategy.BarrierFenced, "Volatile.Read + Thread.MemoryBarrier preserves sequential consistency"),
        _ => new(ordering, AtomicLoweringStrategy.VolatileAcquire, "loads default to acquire; release-only loads are not meaningful"),
    };

    /// <summary>
    /// Strategy for a plain atomic store with the supplied ordering.
    /// </summary>
    public static AtomicLoweringDecision ForStore(AtomicOrdering ordering) => ordering switch
    {
        AtomicOrdering.Unordered or AtomicOrdering.Monotonic
            => new(ordering, AtomicLoweringStrategy.VolatileAccess, "single-word atomic store, no ordering"),
        AtomicOrdering.Release
            => new(ordering, AtomicLoweringStrategy.VolatileRelease, "Volatile.Write carries release semantics on .NET"),
        AtomicOrdering.SeqCst
            => new(ordering, AtomicLoweringStrategy.BarrierFenced, "Thread.MemoryBarrier + Volatile.Write preserves sequential consistency"),
        _ => new(ordering, AtomicLoweringStrategy.VolatileRelease, "stores default to release; acquire-only stores are not meaningful"),
    };

    /// <summary>
    /// Strategy for an atomic RMW (xchg/add/sub/and/or/xor) or cmpxchg.
    /// Interlocked.* on .NET is sequentially consistent, so the same strategy
    /// is correct for every ordering above Monotonic. Monotonic still goes
    /// through Interlocked.* because Interlocked is the only atomicity
    /// primitive available without resorting to native intrinsics.
    /// </summary>
    public static AtomicLoweringDecision ForReadModifyWrite(AtomicOrdering ordering)
        => new(ordering, AtomicLoweringStrategy.InterlockedRmw,
            "Interlocked.* provides full-barrier RMW on .NET regardless of ordering");

    /// <summary>
    /// Strategy for an LLVM <c>fence</c> instruction.
    /// </summary>
    public static AtomicLoweringDecision ForFence(AtomicOrdering ordering) => ordering switch
    {
        AtomicOrdering.SeqCst or AtomicOrdering.AcqRel
            => new(ordering, AtomicLoweringStrategy.BarrierFenced, "Thread.MemoryBarrier provides full fence"),
        AtomicOrdering.Acquire or AtomicOrdering.Release
            => new(ordering, AtomicLoweringStrategy.BarrierFenced, ".NET does not expose half-fences; Thread.MemoryBarrier is conservative-but-correct"),
        _ => new(ordering, AtomicLoweringStrategy.VolatileAccess, "Monotonic/Unordered fences are no-ops on .NET"),
    };
}
