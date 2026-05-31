use std::env;
use std::fs;
use std::path::{Path, PathBuf};
use std::process::Command;

fn main() {
    println!("cargo:rerun-if-env-changed=RUSTLYN_NATIVEAOT_LINK_MODE");
    println!("cargo:rerun-if-env-changed=RUSTLYN_NATIVEAOT_LIB_DIR");
    println!("cargo:rerun-if-env-changed=RUSTLYN_NATIVEAOT_RUNTIME_LIB_DIR");
    println!("cargo:rerun-if-env-changed=RUSTLYN_LLVM_ROOT");
    println!("cargo:rerun-if-env-changed=RUSTLYN_LLVM_DEV_ROOT");
    println!("cargo:rerun-if-env-changed=LLVM_SYS_201_PREFIX");
    println!("cargo:rerun-if-env-changed=LLVM_SYS_PREFIX");
    println!("cargo:rustc-check-cfg=cfg(rustlyn_nativeaot_static)");

    let link_mode =
        env::var("RUSTLYN_NATIVEAOT_LINK_MODE").unwrap_or_else(|_| "shared".to_string());
    let link_mode = link_mode.to_ascii_lowercase();
    let nativeaot_lib_dir = PathBuf::from(
        env::var("RUSTLYN_NATIVEAOT_LIB_DIR")
            .expect("RUSTLYN_NATIVEAOT_LIB_DIR must point to the NativeAOT publish directory"),
    );

    println!("cargo:warning=Rustlyn NativeAOT link mode: {link_mode}");
    println!(
        "cargo:warning=Rustlyn NativeAOT lib dir: {}",
        nativeaot_lib_dir.display()
    );

    match link_mode.as_str() {
        "shared" | "dynamic" => link_shared_nativeaot(&nativeaot_lib_dir),
        "static" => link_static_nativeaot(&nativeaot_lib_dir),
        value => {
            panic!("unsupported RUSTLYN_NATIVEAOT_LINK_MODE '{value}'; expected shared or static")
        }
    }

    link_llvm(link_mode.as_str() == "static");
}

fn link_shared_nativeaot(lib_dir: &Path) {
    stage_nativeaot_runtime_files(lib_dir);
}

fn link_static_nativeaot(lib_dir: &Path) {
    println!("cargo:rustc-cfg=rustlyn_nativeaot_static");
    if env::var("CARGO_CFG_WINDOWS").is_ok() {
        let target_features = env::var("CARGO_CFG_TARGET_FEATURE").unwrap_or_default();
        if !target_features
            .split(',')
            .any(|feature| feature == "crt-static")
        {
            panic!(
                "static NativeAOT link mode on Windows requires Rust crt-static; set RUSTFLAGS=-C target-feature=+crt-static"
            );
        }
    }

    println!("cargo:rustc-link-search=native={}", lib_dir.display());
    println!("cargo:rustc-link-lib=static=rustlyn_nativeaot");

    let runtime_lib_dir = env::var("RUSTLYN_NATIVEAOT_RUNTIME_LIB_DIR")
        .expect("RUSTLYN_NATIVEAOT_RUNTIME_LIB_DIR must point to the NativeAOT runtime pack native directory");
    let runtime_lib_dir_path = PathBuf::from(&runtime_lib_dir);
    println!("cargo:rustc-link-arg={runtime_lib_dir}\\bootstrapperdll.obj");
    println!("cargo:rustc-link-search=native={runtime_lib_dir}");
    for lib in [
        "aotminipal",
        "Runtime.WorkstationGC",
        "Runtime.VxsortDisabled",
        "eventpipe-disabled",
        "standalonegc-disabled",
        "System.Globalization.Native.Aot",
        "System.IO.Compression.Native.Aot",
        "brotlicommon",
        "brotlidec",
        "brotlienc",
        "zlibstatic",
    ] {
        println!("cargo:rustc-link-lib=static={lib}");
    }

    if env::var("CARGO_CFG_WINDOWS").is_ok() {
        for lib in [
            "advapi32", "bcrypt", "crypt32", "iphlpapi", "mswsock", "ncrypt", "normaliz", "ole32",
            "oleaut32", "secur32", "shell32", "user32", "version", "ws2_32",
        ] {
            println!("cargo:rustc-link-lib=dylib={lib}");
        }
    }

    let llvm_root = llvm_root();
    let llvm_config = find_llvm_config(&llvm_root);
    let llvm_lib_dir = run_llvm_config(&llvm_config, &["--libdir"]);
    if env::var("CARGO_CFG_WINDOWS").is_ok() {
        verify_windows_crt_compatibility(&runtime_lib_dir_path, Path::new(llvm_lib_dir.trim()));
    }
}

fn stage_nativeaot_runtime_files(lib_dir: &Path) {
    let target_dir = cargo_target_profile_dir();
    let dll = lib_dir.join("rustlyn_nativeaot.dll");
    if !dll.is_file() {
        panic!("shared NativeAOT link mode requires '{}'", dll.display());
    }

    for path in [dll, lib_dir.join("rustlyn_nativeaot.pdb")] {
        if !path.is_file() {
            continue;
        }

        println!("cargo:rerun-if-changed={}", path.display());
        let destination = target_dir.join(path.file_name().expect("published file has a name"));
        fs::copy(&path, &destination).unwrap_or_else(|err| {
            panic!(
                "failed to stage '{}' to '{}': {err}",
                path.display(),
                destination.display()
            )
        });
        println!("cargo:warning=staged {}", destination.display());
    }
}

fn cargo_target_profile_dir() -> PathBuf {
    let out_dir = PathBuf::from(env::var_os("OUT_DIR").expect("OUT_DIR is set by Cargo"));
    out_dir
        .ancestors()
        .nth(3)
        .unwrap_or_else(|| {
            panic!(
                "could not derive Cargo target profile directory from '{}'",
                out_dir.display()
            )
        })
        .to_path_buf()
}

fn link_llvm(check_static_crt: bool) {
    let llvm_root = llvm_root();
    let llvm_config = find_llvm_config(&llvm_root);
    let version = run_llvm_config(&llvm_config, &["--version"]);
    println!(
        "cargo:rustc-env=RUSTLYN_LINKED_LLVM_VERSION={}",
        version.trim()
    );

    let llvm_lib_dir = run_llvm_config(&llvm_config, &["--libdir"]);
    let llvm_lib_dir = llvm_lib_dir.trim();
    println!("cargo:warning=Rustlyn LLVM lib dir: {llvm_lib_dir}");
    println!("cargo:rustc-link-search=native={llvm_lib_dir}");

    if check_static_crt {
        println!(
            "cargo:warning=static NativeAOT mode will enforce LLVM/NativeAOT CRT compatibility"
        );
    }

    let libs = run_llvm_config(
        &llvm_config,
        &["--link-static", "--libs", "core", "bitreader"],
    );
    emit_link_args(&libs, true);

    let system_libs = run_llvm_config(
        &llvm_config,
        &["--link-static", "--system-libs", "core", "bitreader"],
    );
    emit_link_args(&system_libs, false);
}

fn llvm_root() -> PathBuf {
    env::var_os("RUSTLYN_LLVM_ROOT")
        .or_else(|| env::var_os("RUSTLYN_LLVM_DEV_ROOT"))
        .map(PathBuf::from)
        .or_else(|| env::var_os("LLVM_SYS_201_PREFIX").map(PathBuf::from))
        .or_else(|| env::var_os("LLVM_SYS_PREFIX").map(PathBuf::from))
        .expect("RUSTLYN_LLVM_ROOT must point to an LLVM development root with bin/llvm-config and static libraries")
}

fn find_llvm_config(root: &Path) -> PathBuf {
    let exe = if cfg!(windows) {
        "llvm-config.exe"
    } else {
        "llvm-config"
    };

    let candidates = [root.join("bin").join(exe), root.join(exe)];
    candidates
        .into_iter()
        .find(|path| path.is_file())
        .unwrap_or_else(|| {
            panic!(
                "could not find {exe} under LLVM development root '{}'",
                root.display()
            )
        })
}

fn run_llvm_config(llvm_config: &Path, args: &[&str]) -> String {
    let output = Command::new(llvm_config)
        .args(args)
        .output()
        .unwrap_or_else(|err| panic!("failed to run '{}': {err}", llvm_config.display()));

    if !output.status.success() {
        panic!(
            "'{} {}' failed with status {}:\n{}",
            llvm_config.display(),
            args.join(" "),
            output.status,
            String::from_utf8_lossy(&output.stderr)
        );
    }

    String::from_utf8(output.stdout).expect("llvm-config output was not valid UTF-8")
}

fn emit_link_args(args: &str, prefer_static: bool) {
    for token in args.split_whitespace() {
        if let Some(path) = token.strip_prefix("-L") {
            println!("cargo:rustc-link-search=native={path}");
            continue;
        }

        if let Some(name) = token.strip_prefix("-l") {
            emit_link_lib(name, prefer_static);
            continue;
        }

        if token.ends_with(".lib") || token.ends_with(".a") {
            let path = Path::new(token);
            if let Some(parent) = path
                .parent()
                .filter(|parent| !parent.as_os_str().is_empty())
            {
                println!("cargo:rustc-link-search=native={}", parent.display());
            }

            if let Some(stem) = path.file_stem().and_then(|stem| stem.to_str()) {
                let name = if token.ends_with(".a") {
                    stem.strip_prefix("lib").unwrap_or(stem)
                } else {
                    stem
                };
                emit_link_lib(name, prefer_static);
            }
            continue;
        }

        if token.ends_with(".framework") {
            continue;
        }
    }
}

fn emit_link_lib(name: &str, prefer_static: bool) {
    if cfg!(windows) && !prefer_static && matches!(name, "zlib" | "xml2") {
        if !windows_lib_available(name) {
            println!(
                "cargo:warning=skipping LLVM system library {name}.lib because it was not found in LIB paths"
            );
            return;
        }
    }

    if prefer_static {
        println!("cargo:rustc-link-lib=static={name}");
    } else {
        println!("cargo:rustc-link-lib={name}");
    }
}

fn windows_lib_available(name: &str) -> bool {
    let file_name = format!("{name}.lib");
    env::var_os("LIB")
        .map(|paths| env::split_paths(&paths).any(|path| path.join(&file_name).is_file()))
        .unwrap_or(false)
}

fn verify_windows_crt_compatibility(runtime_lib_dir: &Path, llvm_lib_dir: &Path) {
    let runtime_gc = runtime_lib_dir.join("Runtime.WorkstationGC.lib");
    let llvm_core = llvm_lib_dir.join("LLVMCore.lib");

    let runtime_uses_static_crt =
        archive_contains_ascii(&runtime_gc, "RuntimeLibrary=MT_StaticRelease");
    let llvm_uses_dynamic_crt =
        archive_contains_ascii(&llvm_core, "RuntimeLibrary=MD_DynamicRelease");
    let llvm_uses_static_crt =
        archive_contains_ascii(&llvm_core, "RuntimeLibrary=MT_StaticRelease");

    if runtime_uses_static_crt && llvm_uses_dynamic_crt {
        panic!(
            "LLVM static libraries in '{}' use the dynamic MSVC CRT (/MD), but the NativeAOT runtime pack in '{}' uses the static MSVC CRT (/MT). Build LLVM with a matching static CRT and point RUSTLYN_LLVM_ROOT at that root.",
            llvm_lib_dir.display(),
            runtime_lib_dir.display()
        );
    }

    if runtime_uses_static_crt && !llvm_uses_static_crt {
        println!(
            "cargo:warning=could not confirm LLVMCore.lib uses the static MSVC CRT; the final link may fail with LNK2038 RuntimeLibrary mismatches"
        );
    }
}

fn archive_contains_ascii(path: &Path, needle: &str) -> bool {
    let bytes =
        fs::read(path).unwrap_or_else(|err| panic!("failed to read '{}': {err}", path.display()));
    bytes
        .windows(needle.len())
        .any(|window| window == needle.as_bytes())
}
