use std::env;

fn main() {
    println!("cargo:rerun-if-env-changed=RUSTLYN_NATIVEAOT_LIB_DIR");
    println!("cargo:rerun-if-env-changed=RUSTLYN_NATIVEAOT_RUNTIME_LIB_DIR");
    let lib_dir = env::var("RUSTLYN_NATIVEAOT_LIB_DIR")
        .expect("RUSTLYN_NATIVEAOT_LIB_DIR must point to the directory containing rustlyn_nativeaot.lib");
    println!("cargo:rustc-link-search=native={lib_dir}");
    println!("cargo:rustc-link-lib=static=rustlyn_nativeaot");

    let runtime_lib_dir = env::var("RUSTLYN_NATIVEAOT_RUNTIME_LIB_DIR")
        .expect("RUSTLYN_NATIVEAOT_RUNTIME_LIB_DIR must point to the NativeAOT runtime pack native directory");
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
            "advapi32",
            "bcrypt",
            "crypt32",
            "iphlpapi",
            "mswsock",
            "ncrypt",
            "normaliz",
            "ole32",
            "oleaut32",
            "secur32",
            "shell32",
            "user32",
            "version",
            "ws2_32",
        ] {
            println!("cargo:rustc-link-lib=dylib={lib}");
        }
    }
}
