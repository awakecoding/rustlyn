/// Exercises real iterator adapter chains that compile to complex
/// phi/loop/inlined-closure IR patterns in optimized Rust.

/// map + filter + sum: sum of doubled even numbers in [1..=n]
/// For n=10: evens are 2,4,6,8,10 -> doubled: 4,8,12,16,20 -> sum = 60
#[unsafe(no_mangle)]
pub extern "C" fn iter_map_filter_sum(n: i32) -> i32 {
    (1..=n)
        .filter(|x| x % 2 == 0)
        .map(|x| x * 2)
        .sum()
}

/// enumerate + filter_map: sum of indices where value > threshold
/// For arr = [3,1,4,1,5,9,2,6], threshold=3: indices 2,4,5,7 -> sum = 18
#[unsafe(no_mangle)]
pub extern "C" fn iter_enumerate_filter_map(threshold: i32) -> i32 {
    let arr = [3i32, 1, 4, 1, 5, 9, 2, 6];
    arr.iter()
        .enumerate()
        .filter_map(|(i, &v)| if v > threshold { Some(i as i32) } else { None })
        .sum()
}

/// zip + map + fold: dot product of two arrays
/// [1,2,3,4] · [5,6,7,8] = 5+12+21+32 = 70
#[unsafe(no_mangle)]
pub extern "C" fn iter_zip_dot_product() -> i32 {
    let a = [1i32, 2, 3, 4];
    let b = [5i32, 6, 7, 8];
    a.iter().zip(b.iter()).map(|(x, y)| x * y).sum()
}

/// chain + take + fold: concatenate two ranges, take first n, multiply
/// chain(1..=5, 6..=10).take(7).product() = 1*2*3*4*5*6*7 = 5040
#[unsafe(no_mangle)]
pub extern "C" fn iter_chain_take_product() -> i32 {
    (1..=5i32)
        .chain(6..=10)
        .take(7)
        .fold(1i32, |acc, x| acc * x)
}

/// flat_map + count: count characters in words longer than 2
/// words: ["hi", "hello", "no", "world"] -> "hello"(5) + "world"(5) = 10 chars
#[unsafe(no_mangle)]
pub extern "C" fn iter_flat_map_count() -> i32 {
    let words: &[&[u8]] = &[b"hi", b"hello", b"no", b"world"];
    words
        .iter()
        .filter(|w| w.len() > 2)
        .flat_map(|w| w.iter())
        .count() as i32
}

/// skip + step_by + collect-as-sum: skip 2, then every 3rd element summed
/// [10,20,30,40,50,60,70,80,90,100].skip(2).step_by(3) = [30,60,90] -> 180
#[unsafe(no_mangle)]
pub extern "C" fn iter_skip_step_sum() -> i32 {
    let arr = [10i32, 20, 30, 40, 50, 60, 70, 80, 90, 100];
    arr.iter().skip(2).step_by(3).sum()
}

/// any + all + position combined:
/// Returns position of first element > threshold, or -1 if none found
/// [1,2,3,7,8,4] with threshold=5: first >5 at position 3 (value 7) -> return 3
#[unsafe(no_mangle)]
pub extern "C" fn iter_any_all_position(threshold: i32) -> i32 {
    let arr = [1i32, 2, 3, 7, 8, 4];
    match arr.iter().position(|&x| x > threshold) {
        Some(p) => p as i32,
        None => -1,
    }
}
