# Interop Handle ABI

This document defines the public ABI contract for managed object handles exchanged between translated Rust code and the .NET runtime via `Rustlyn.Interop`.

## Handle Representation

All managed handles are represented as `i32` values at the ABI boundary.

| Value | Meaning |
|-------|---------|
| `0` | Null handle (no object) |
| `> 0` | Valid handle referencing a tracked managed object |
| `< 0` | Reserved (never issued) |

Handles are **assembly-scoped**: they are valid only within the `ManagedInteropRuntime` static store of the emitted assembly's process lifetime. They are not serializable, not transferable across AppDomains or processes, and not stable across invocations.

## Handle Kinds

Each handle carries a kind tag tracked internally:

- **Object** — any managed object (strings, arrays, user types)
- **Exception** — a captured `System.Exception` instance

The kind tag is enforced on every access. Attempting to release an Object handle through the Exception release path (or vice versa) throws `InvalidOperationException`.

## Allocation

Handles are allocated via:

- `ManagedInteropRuntime.AddObjectHandle(object)` → `i32`
- `ManagedInteropRuntime.AddExceptionHandle(Exception)` → `i32`

Allocation never returns `0`. The store uses a monotonically increasing counter starting at `1`. Exhausting `int.MaxValue` handles throws (theoretical limit, not expected in practice).

## Access

Retrieving the underlying object:

- `ManagedInteropRuntime.GetObject<T>(int handle)` — throws `KeyNotFoundException` for missing/null handles, `InvalidOperationException` for kind or type mismatch.
- `ManagedInteropRuntime.GetException(int handle)` — same throwing behavior.

## Release

Releasing a handle removes it from the store:

- `ManagedInteropRuntime.ReleaseObject(int handle)` → `bool`
- `ManagedInteropRuntime.ReleaseException(int handle)` → `bool`

Release semantics:

| Scenario | Behavior |
|----------|----------|
| Valid handle, correct kind | Returns `true`, handle is removed |
| Handle `0` (null) | Returns `false` (no-op, no exception) |
| Already-released handle | Returns `false` (no-op, no exception) |
| Wrong-kind release | Throws `InvalidOperationException` |

**Double-release is safe** — it returns `false` without throwing. This allows Rust `Drop` or error paths to defensively release without panicking.

## Exception Convention

Generated bindings use one of two exception conventions at the ABI boundary:

### Return-exception-handle convention

Used for void-like operations (e.g., `Console.WriteLine`, `object_release`):

```
fn extern_name(...) -> i32;
```

- Returns `0` on success
- Returns a nonzero exception handle on failure

### Write-exception-out convention

Used for operations that return a value (e.g., object handles, integers, booleans):

```
fn extern_name(..., exception_out: *mut i32) -> i32;
```

- Writes `0` to `*exception_out` on success, returns the result value
- Writes a nonzero exception handle to `*exception_out` on failure, returns `0`

## Rust-Side Wrapper Pattern

Generated Rust wrappers translate the ABI convention into `Result<T, Exception>`:

```rust
pub fn method() -> Result<ManagedString, Exception> {
    let mut exception_handle = 0;
    let object_handle = unsafe { extern_name(&mut exception_handle) };
    Exception::from_handle(exception_handle)?;
    Ok(ManagedString::from_handle(object_handle))
}
```

The `Exception` struct wraps a nonzero handle. If a `Result::Err(exception)` is discarded without calling `exception.release()`, the exception handle leaks. This is intentional — the ABI cannot enforce cleanup across the language boundary, so callers must handle errors explicitly.

## String Encoding

Two encoding helpers are provided:

- **InteropUtf8** — for Rust `&str` / `&[u8]` interop (primary path)
- **InteropUtf16** — for .NET native `char` operations (secondary path)

Most generated bindings use UTF-8 at the boundary since Rust strings are natively UTF-8. UTF-16 helpers exist for scenarios where direct .NET char buffer access is needed without transcoding.

## Lifecycle Summary

```
Rust calls extern → managed glue → ManagedInteropRuntime
     ↓                                    ↓
  i32 handle ←──────────────────── AddObjectHandle(obj)
     ↓
  use handle in further calls
     ↓
  release(handle) → ReleaseObject(handle) → true
     ↓
  handle is now invalid (double-release returns false)
```

## Design Decisions

1. **No garbage-collection integration** — handles must be explicitly released. There is no weak-handle or ref-counting scheme.
2. **No handle reuse** — released handle values are never reassigned. This prevents use-after-free from returning a different object.
3. **Thread-safe** — all store operations are `lock`-protected. Concurrent access from multiple translated Rust threads is safe.
4. **Assembly-scoped store** — the static `ManagedInteropRuntime.Store` is per-process, shared across all translated functions in the same assembly load context.
