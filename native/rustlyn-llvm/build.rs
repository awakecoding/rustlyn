use std::env;
use std::path::{Path, PathBuf};
use std::process::Command;

fn main() {
    let llvm_root = env::var_os("RUSTLYN_LLVM_DEV_ROOT")
        .map(PathBuf::from)
        .or_else(|| env::var_os("LLVM_SYS_201_PREFIX").map(PathBuf::from))
        .or_else(|| env::var_os("LLVM_SYS_PREFIX").map(PathBuf::from))
        .expect("RUSTLYN_LLVM_DEV_ROOT must point to an LLVM development root with bin/llvm-config and static libraries");

    let llvm_config = find_llvm_config(&llvm_root);
    let version = run_llvm_config(&llvm_config, &["--version"]);
    println!("cargo:rustc-env=RUSTLYN_LLVM_VERSION={}", version.trim());

    let lib_dir = run_llvm_config(&llvm_config, &["--libdir"]);
    println!("cargo:rustc-link-search=native={}", lib_dir.trim());

    let libs = run_llvm_config(
        &llvm_config,
        &[
            "--link-static",
            "--libs",
            "core",
            "bitreader",
            "bitwriter",
            "analysis",
            "passes",
        ],
    );
    emit_link_args(&libs, true);

    let system_libs = run_llvm_config(&llvm_config, &["--link-static", "--system-libs"]);
    emit_link_args(&system_libs, false);

    println!("cargo:rerun-if-env-changed=RUSTLYN_LLVM_DEV_ROOT");
    println!("cargo:rerun-if-env-changed=LLVM_SYS_201_PREFIX");
    println!("cargo:rerun-if-env-changed=LLVM_SYS_PREFIX");
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
        .unwrap_or_else(|| panic!("could not find {exe} under LLVM development root '{}'", root.display()))
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
            if let Some(parent) = path.parent().filter(|parent| !parent.as_os_str().is_empty()) {
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
    if prefer_static {
        println!("cargo:rustc-link-lib=static={name}");
    } else {
        println!("cargo:rustc-link-lib={name}");
    }
}
