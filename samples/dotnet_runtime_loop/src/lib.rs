unsafe extern "C" {
    fn rustlyn_dotnet_math_max_i32(left: i32, right: i32) -> i32;
    fn rustlyn_dotnet_math_min_i32(left: i32, right: i32) -> i32;
    fn rustlyn_dotnet_bitops_popcount_u32(value: u32) -> i32;
}

#[unsafe(no_mangle)]
pub extern "C" fn dotnet_runtime_loop(seed: i32, mask: u32) -> i32 {
    let mut acc = 0;
    let mut value = seed;

    for shift in 0..3u32 {
        let bits = unsafe { rustlyn_dotnet_bitops_popcount_u32(mask >> shift) };
        let raised = unsafe { rustlyn_dotnet_math_max_i32(value + bits, 8) };
        let bounded = unsafe { rustlyn_dotnet_math_min_i32(raised, 40) };
        acc += bounded;
        value += 3;
    }

    acc
}