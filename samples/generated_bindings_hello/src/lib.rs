mod system;

fn callback_double(value: i32) -> i32 {
    value * 2
}

fn callback_add(left: i32, right: i32) -> i32 {
    left + right
}

fn callback_string_transform(value_handle: i32) -> i32 {
    let value = unsafe { system::ManagedString::from_borrowed_handle(value_handle) };
    if !matches!(value.len(), Ok(8)) {
        return 0;
    }

    let transformed = b"callback-ok";
    match system::ManagedString::from_utf8_parts(transformed.as_ptr(), transformed.len() as i64) {
        Ok(value) => value.into_handle(),
        Err(_) => 0,
    }
}

fn release_string_projection_inputs(
    source: system::ManagedString,
    needle: system::ManagedString,
    replacement: system::ManagedString,
) {
    let _ = replacement.release();
    let _ = needle.release();
    let _ = source.release();
}

#[unsafe(no_mangle)]
pub extern "C" fn generated_bindings_score() -> i32 {
    let current_directory = match system::io::directory::get_current_directory() {
        Ok(value) => value,
        Err(_) => return -2,
    };

    let directory_len = match current_directory.utf8_len() {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -3;
        }
    };

    let mut directory_buffer = [0u8; 512];
    if directory_len <= 0 || directory_len as usize > directory_buffer.len() {
        let _ = current_directory.release();
        return -5;
    }

    let written = match current_directory.copy_utf8_into(&mut directory_buffer) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -6;
        }
    };

    if written <= 0 {
        let _ = current_directory.release();
        return -7;
    }

    if system::console::write_line_utf8_parts(directory_buffer.as_ptr(), written as i64).is_err() {
        let _ = current_directory.release();
        return -8;
    }

    let roundtrip_directory = match system::ManagedString::from_utf8_parts(
        directory_buffer.as_ptr(),
        written as i64,
    ) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -9;
        }
    };

    if system::console::write_line(&roundtrip_directory).is_err() {
        let _ = roundtrip_directory.release();
        let _ = current_directory.release();
        return -10;
    }

    if roundtrip_directory.release().is_err() {
        let _ = current_directory.release();
        return -11;
    }

    let environment_directory = match system::environment::current_directory() {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -12;
        }
    };

    if system::console::write_line(&environment_directory).is_err() {
        let _ = environment_directory.release();
        let _ = current_directory.release();
        return -13;
    }

    if environment_directory.release().is_err() {
        let _ = current_directory.release();
        return -14;
    }

    let char_len = match current_directory.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -15;
        }
    };

    if char_len <= 0 || char_len > directory_len {
        let _ = current_directory.release();
        return -16;
    }

    let readme_pattern = match system::ManagedString::from_utf8_parts(b"README.md".as_ptr(), 9) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -181;
        }
    };
    let readme_files = match system::io::directory::get_files(&current_directory, &readme_pattern) {
        Ok(value) => value,
        Err(_) => {
            let _ = readme_pattern.release();
            let _ = current_directory.release();
            return -182;
        }
    };
    let readme_file_count = match readme_files.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = readme_files.release();
            let _ = readme_pattern.release();
            let _ = current_directory.release();
            return -183;
        }
    };
    if readme_file_count != 1 {
        let _ = readme_files.release();
        let _ = readme_pattern.release();
        let _ = current_directory.release();
        return -184;
    }
    let readme_file = match readme_files.get(0) {
        Ok(value) => value,
        Err(_) => {
            let _ = readme_files.release();
            let _ = readme_pattern.release();
            let _ = current_directory.release();
            return -185;
        }
    };
    let readme_file_name = match system::io::path::get_file_name(&readme_file) {
        Ok(value) => value,
        Err(_) => {
            let _ = readme_file.release();
            let _ = readme_files.release();
            let _ = readme_pattern.release();
            let _ = current_directory.release();
            return -186;
        }
    };
    let readme_file_name_len = match readme_file_name.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = readme_file_name.release();
            let _ = readme_file.release();
            let _ = readme_files.release();
            let _ = readme_pattern.release();
            let _ = current_directory.release();
            return -187;
        }
    };
    if readme_file_name_len != 9 {
        let _ = readme_file_name.release();
        let _ = readme_file.release();
        let _ = readme_files.release();
        let _ = readme_pattern.release();
        let _ = current_directory.release();
        return -188;
    }
    if readme_file_name.release().is_err() {
        let _ = readme_file.release();
        let _ = readme_files.release();
        let _ = readme_pattern.release();
        let _ = current_directory.release();
        return -189;
    }
    if readme_file.release().is_err() || readme_files.release().is_err() || readme_pattern.release().is_err() {
        let _ = current_directory.release();
        return -190;
    }

    let file_name = match system::io::path::get_file_name(&current_directory) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -17;
        }
    };

    let file_name_len = match file_name.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = file_name.release();
            let _ = current_directory.release();
            return -18;
        }
    };

    if file_name_len <= 0 || file_name_len > char_len {
        let _ = file_name.release();
        let _ = current_directory.release();
        return -19;
    }

    let combined_path = match system::io::path::combine(&current_directory, &file_name) {
        Ok(value) => value,
        Err(_) => {
            let _ = file_name.release();
            let _ = current_directory.release();
            return -20;
        }
    };

    let combined_len = match combined_path.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = combined_path.release();
            let _ = file_name.release();
            let _ = current_directory.release();
            return -21;
        }
    };

    if combined_len <= char_len {
        let _ = combined_path.release();
        let _ = file_name.release();
        let _ = current_directory.release();
        return -22;
    }

    if combined_path.release().is_err() {
        let _ = file_name.release();
        let _ = current_directory.release();
        return -23;
    }

    if file_name.release().is_err() {
        let _ = current_directory.release();
        return -24;
    }

    let file_name_without_extension = match system::io::path::get_file_name_without_extension(&current_directory) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -25;
        }
    };

    let stem_len = match file_name_without_extension.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = file_name_without_extension.release();
            let _ = current_directory.release();
            return -26;
        }
    };

    if stem_len <= 0 || stem_len > file_name_len {
        let _ = file_name_without_extension.release();
        let _ = current_directory.release();
        return -27;
    }

    let changed_path = match system::io::path::change_extension(
        &current_directory,
        &file_name_without_extension,
    ) {
        Ok(value) => value,
        Err(_) => {
            let _ = file_name_without_extension.release();
            let _ = current_directory.release();
            return -28;
        }
    };

    let changed_len = match changed_path.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = changed_path.release();
            let _ = file_name_without_extension.release();
            let _ = current_directory.release();
            return -29;
        }
    };

    if changed_len <= 0 {
        let _ = changed_path.release();
        let _ = file_name_without_extension.release();
        let _ = current_directory.release();
        return -30;
    }

    let changed_extension = match system::io::path::get_extension(&changed_path) {
        Ok(value) => value,
        Err(_) => {
            let _ = changed_path.release();
            let _ = file_name_without_extension.release();
            let _ = current_directory.release();
            return -31;
        }
    };

    let changed_extension_len = match changed_extension.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = changed_extension.release();
            let _ = changed_path.release();
            let _ = file_name_without_extension.release();
            let _ = current_directory.release();
            return -32;
        }
    };

    if changed_extension_len <= 0 {
        let _ = changed_extension.release();
        let _ = changed_path.release();
        let _ = file_name_without_extension.release();
        let _ = current_directory.release();
        return -33;
    }

    let changed_has_extension = match system::io::path::has_extension(&changed_path) {
        Ok(value) => value,
        Err(_) => {
            let _ = changed_extension.release();
            let _ = changed_path.release();
            let _ = file_name_without_extension.release();
            let _ = current_directory.release();
            return -57;
        }
    };

    if changed_has_extension == 0 {
        let _ = changed_extension.release();
        let _ = changed_path.release();
        let _ = file_name_without_extension.release();
        let _ = current_directory.release();
        return -58;
    }

    if changed_extension.release().is_err() {
        let _ = changed_path.release();
        let _ = file_name_without_extension.release();
        let _ = current_directory.release();
        return -34;
    }

    if changed_path.release().is_err() {
        let _ = file_name_without_extension.release();
        let _ = current_directory.release();
        return -35;
    }

    if file_name_without_extension.release().is_err() {
        let _ = current_directory.release();
        return -36;
    }

    let directory_name = match system::io::path::get_directory_name(&current_directory) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -37;
        }
    };

    let directory_name_len = match directory_name.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = directory_name.release();
            let _ = current_directory.release();
            return -38;
        }
    };

    if directory_name_len <= 0 || directory_name_len >= char_len {
        let _ = directory_name.release();
        let _ = current_directory.release();
        return -39;
    }

    if directory_name.release().is_err() {
        let _ = current_directory.release();
        return -40;
    }

    let temp_path = match system::io::path::get_temp_path() {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -41;
        }
    };

    let temp_len = match temp_path.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -42;
        }
    };

    if temp_len <= 0 {
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -43;
    }

    let temp_ends_in_separator = match system::io::path::ends_in_directory_separator(&temp_path) {
        Ok(value) => value,
        Err(_) => {
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -63;
        }
    };

    if temp_ends_in_separator == 0 {
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -64;
    }

    let full_temp_path = match system::io::path::get_full_path(&temp_path) {
        Ok(value) => value,
        Err(_) => {
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -44;
        }
    };

    let full_temp_len = match full_temp_path.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = full_temp_path.release();
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -45;
        }
    };

    if full_temp_len <= 0 {
        let _ = full_temp_path.release();
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -46;
    }

    let full_temp_is_rooted = match system::io::path::is_path_rooted(&full_temp_path) {
        Ok(value) => value,
        Err(_) => {
            let _ = full_temp_path.release();
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -59;
        }
    };

    if full_temp_is_rooted == 0 {
        let _ = full_temp_path.release();
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -60;
    }

    let full_temp_is_fully_qualified = match system::io::path::is_path_fully_qualified(&full_temp_path) {
        Ok(value) => value,
        Err(_) => {
            let _ = full_temp_path.release();
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -61;
        }
    };

    if full_temp_is_fully_qualified == 0 {
        let _ = full_temp_path.release();
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -62;
    }

    let temp_root = match system::io::path::get_path_root(&full_temp_path) {
        Ok(value) => value,
        Err(_) => {
            let _ = full_temp_path.release();
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -47;
        }
    };

    let temp_root_len = match temp_root.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = temp_root.release();
            let _ = full_temp_path.release();
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -48;
        }
    };

    if temp_root_len <= 0 || temp_root_len > full_temp_len {
        let _ = temp_root.release();
        let _ = full_temp_path.release();
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -49;
    }

    let relative_temp_path = match system::io::path::get_relative_path(&temp_root, &full_temp_path) {
        Ok(value) => value,
        Err(_) => {
            let _ = temp_root.release();
            let _ = full_temp_path.release();
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -50;
        }
    };

    let relative_temp_len = match relative_temp_path.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = relative_temp_path.release();
            let _ = temp_root.release();
            let _ = full_temp_path.release();
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -51;
        }
    };

    if relative_temp_len <= 0 || relative_temp_len > full_temp_len {
        let _ = relative_temp_path.release();
        let _ = temp_root.release();
        let _ = full_temp_path.release();
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -52;
    }

    if relative_temp_path.release().is_err() {
        let _ = temp_root.release();
        let _ = full_temp_path.release();
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -53;
    }

    if temp_root.release().is_err() {
        let _ = full_temp_path.release();
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -54;
    }

    if full_temp_path.release().is_err() {
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -55;
    }

    if temp_path.release().is_err() {
        let _ = current_directory.release();
        return -56;
    }

    let sqrt_score = match system::mathf::sqrt(81.0) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -65;
        }
    };

    if sqrt_score < 8.99 || sqrt_score > 9.01 {
        let _ = current_directory.release();
        return -66;
    }

    let mathf_score = match (
        system::mathf::abs(-3.5),
        system::mathf::sin(0.0),
        system::mathf::cos(0.0),
        system::mathf::min(2.0, 7.0),
        system::mathf::max(2.0, 7.0),
    ) {
        (Ok(abs), Ok(sin), Ok(cos), Ok(min), Ok(max)) => abs + sin + cos + min + max,
        _ => {
            let _ = current_directory.release();
            return -67;
        }
    };

    if mathf_score < 13.49 || mathf_score > 13.51 {
        let _ = current_directory.release();
        return -68;
    }

    let math_score = match (system::math::sqrt(144.0), system::math::abs(-4.25)) {
        (Ok(sqrt), Ok(abs)) => sqrt + abs,
        _ => {
            let _ = current_directory.release();
            return -69;
        }
    };

    if math_score < 16.24 || math_score > 16.26 {
        let _ = current_directory.release();
        return -70;
    }

    let source_bytes = b"alpha-runtime-beta";
    let source = match system::ManagedString::from_utf8_parts(
        source_bytes.as_ptr(),
        source_bytes.len() as i64,
    ) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -82;
        }
    };

    let needle_bytes = b"runtime";
    let needle = match system::ManagedString::from_utf8_parts(
        needle_bytes.as_ptr(),
        needle_bytes.len() as i64,
    ) {
        Ok(value) => value,
        Err(_) => {
            let _ = source.release();
            let _ = current_directory.release();
            return -91;
        }
    };

    let replacement_bytes = b"managed";
    let replacement = match system::ManagedString::from_utf8_parts(
        replacement_bytes.as_ptr(),
        replacement_bytes.len() as i64,
    ) {
        Ok(value) => value,
        Err(_) => {
            let _ = needle.release();
            let _ = source.release();
            let _ = current_directory.release();
            return -92;
        }
    };

    let prefix_bytes = b"alpha";
    let prefix = match system::ManagedString::from_utf8_parts(
        prefix_bytes.as_ptr(),
        prefix_bytes.len() as i64,
    ) {
        Ok(value) => value,
        Err(_) => {
            release_string_projection_inputs(source, needle, replacement);
            let _ = current_directory.release();
            return -134;
        }
    };

    let suffix_bytes = b"beta";
    let suffix = match system::ManagedString::from_utf8_parts(
        suffix_bytes.as_ptr(),
        suffix_bytes.len() as i64,
    ) {
        Ok(value) => value,
        Err(_) => {
            let _ = prefix.release();
            release_string_projection_inputs(source, needle, replacement);
            let _ = current_directory.release();
            return -135;
        }
    };

    let prefix_len = match prefix.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = suffix.release();
            let _ = prefix.release();
            release_string_projection_inputs(source, needle, replacement);
            let _ = current_directory.release();
            return -151;
        }
    };

    if prefix_len != 5 {
        let _ = suffix.release();
        let _ = prefix.release();
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -152;
    }

    let suffix_len = match suffix.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = suffix.release();
            let _ = prefix.release();
            release_string_projection_inputs(source, needle, replacement);
            let _ = current_directory.release();
            return -153;
        }
    };

    if suffix_len != 4 {
        let _ = suffix.release();
        let _ = prefix.release();
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -154;
    }

    let contains_prefix = match source.contains_with_comparison(&prefix, system::StringComparison::Ordinal) {
        Ok(value) => value,
        Err(_) => {
            let _ = suffix.release();
            let _ = prefix.release();
            release_string_projection_inputs(source, needle, replacement);
            let _ = current_directory.release();
            return -149;
        }
    };

    if !contains_prefix {
        let _ = suffix.release();
        let _ = prefix.release();
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -150;
    }

    let starts_with_prefix = match source.starts_with_comparison(&prefix, system::StringComparison::Ordinal) {
        Ok(value) => value,
        Err(_) => {
            let _ = suffix.release();
            let _ = prefix.release();
            release_string_projection_inputs(source, needle, replacement);
            let _ = current_directory.release();
            return -136;
        }
    };

    if !starts_with_prefix {
        let _ = suffix.release();
        let _ = prefix.release();
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -139;
    }

    let ends_with_suffix = match source.ends_with_comparison(&suffix, system::StringComparison::Ordinal) {
        Ok(value) => value,
        Err(_) => {
            let _ = suffix.release();
            let _ = prefix.release();
            release_string_projection_inputs(source, needle, replacement);
            let _ = current_directory.release();
            return -140;
        }
    };

    if !ends_with_suffix {
        let _ = suffix.release();
        let _ = prefix.release();
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -141;
    }

    let starts_with_suffix = match source.starts_with_comparison(&suffix, system::StringComparison::Ordinal) {
        Ok(value) => value,
        Err(_) => {
            let _ = suffix.release();
            let _ = prefix.release();
            release_string_projection_inputs(source, needle, replacement);
            let _ = current_directory.release();
            return -142;
        }
    };

    if starts_with_suffix {
        let _ = suffix.release();
        let _ = prefix.release();
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -143;
    }

    let ends_with_prefix = match source.ends_with_comparison(&prefix, system::StringComparison::Ordinal) {
        Ok(value) => value,
        Err(_) => {
            let _ = suffix.release();
            let _ = prefix.release();
            release_string_projection_inputs(source, needle, replacement);
            let _ = current_directory.release();
            return -144;
        }
    };

    if ends_with_prefix {
        let _ = suffix.release();
        let _ = prefix.release();
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -145;
    }

    if suffix.release().is_err() {
        let _ = prefix.release();
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -137;
    }

    if prefix.release().is_err() {
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -138;
    }

    let index_of_handle = match source.index_of_with_comparison(&needle, system::StringComparison::Ordinal) {
        Ok(value) => value,
        Err(_) => {
            release_string_projection_inputs(source, needle, replacement);
            let _ = current_directory.release();
            return -95;
        }
    };

    if index_of_handle != 6 {
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -96;
    }

    let substring = match source.substring(6, 7) {
        Ok(value) => value,
        Err(_) => {
            release_string_projection_inputs(source, needle, replacement);
            let _ = current_directory.release();
            return -97;
        }
    };

    let substring_matches = match (substring.len(), substring.index_of(&needle)) {
        (Ok(length), Ok(index)) => length == 7 && index == 0,
        _ => false,
    };

    if !substring_matches {
        let _ = substring.release();
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -98;
    }

    if substring.release().is_err() {
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -99;
    }

    let separator_bytes = b"-";
    let separator = match system::ManagedString::from_utf8_parts(
        separator_bytes.as_ptr(),
        separator_bytes.len() as i64,
    ) {
        Ok(value) => value,
        Err(_) => {
            release_string_projection_inputs(source, needle, replacement);
            let _ = current_directory.release();
            return -120;
        }
    };

    let split = match source.split(&separator, system::StringSplitOptions::RemoveEmptyEntries) {
        Ok(value) => value,
        Err(_) => {
            let _ = separator.release();
            release_string_projection_inputs(source, needle, replacement);
            let _ = current_directory.release();
            return -121;
        }
    };

    let split_matches = match (split.len(), split.get(1)) {
        (Ok(length), Ok(segment)) => {
            let segment_matches = match segment.index_of_with_comparison(&needle, system::StringComparison::Ordinal) {
                Ok(index) => length == 3 && index == 0,
                Err(_) => false,
            };
            segment.release().is_ok() && segment_matches
        }
        _ => false,
    };

    if !split_matches {
        let _ = split.release();
        let _ = separator.release();
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -122;
    }

    if split.release().is_err() {
        let _ = separator.release();
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -123;
    }

    if separator.release().is_err() {
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -124;
    }

    let replaced = match source.replace(&needle, &replacement) {
        Ok(value) => value,
        Err(_) => {
            release_string_projection_inputs(source, needle, replacement);
            let _ = current_directory.release();
            return -100;
        }
    };

    let replaced_score = match (
        replaced.index_of_with_comparison(&replacement, system::StringComparison::Ordinal),
        replaced.contains_with_comparison(&needle, system::StringComparison::Ordinal),
    ) {
        (Ok(index), Ok(has_old)) => index == 6 && !has_old,
        _ => false,
    };

    if !replaced_score {
        let _ = replaced.release();
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -101;
    }

    if replaced.release().is_err() {
        release_string_projection_inputs(source, needle, replacement);
        let _ = current_directory.release();
        return -102;
    }

    release_string_projection_inputs(source, needle, replacement);

    let time_span = match system::time_span::from_milliseconds(1234.0) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -103;
        }
    };

    let time_span_ticks = match time_span.ticks() {
        Ok(value) => value,
        Err(_) => {
            let _ = time_span.release();
            let _ = current_directory.release();
            return -104;
        }
    };

    if time_span_ticks != 12_340_000 {
        let _ = time_span.release();
        let _ = current_directory.release();
        return -105;
    }

    let total_milliseconds = match time_span.total_milliseconds() {
        Ok(value) => value,
        Err(_) => {
            let _ = time_span.release();
            let _ = current_directory.release();
            return -106;
        }
    };

    if total_milliseconds < 1233.9 || total_milliseconds > 1234.1 {
        let _ = time_span.release();
        let _ = current_directory.release();
        return -107;
    }

    let time_span_text = match time_span.to_string() {
        Ok(value) => value,
        Err(_) => {
            let _ = time_span.release();
            let _ = current_directory.release();
            return -108;
        }
    };

    let time_span_text_len = match time_span_text.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = time_span_text.release();
            let _ = time_span.release();
            let _ = current_directory.release();
            return -109;
        }
    };

    if time_span_text_len <= 0 {
        let _ = time_span_text.release();
        let _ = time_span.release();
        let _ = current_directory.release();
        return -110;
    }

    if time_span_text.release().is_err() {
        let _ = time_span.release();
        let _ = current_directory.release();
        return -111;
    }

    if time_span.release().is_err() {
        let _ = current_directory.release();
        return -112;
    }

    let timestamp = match system::date_time_offset::from_unix_time_milliseconds(123_456_789) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -113;
        }
    };

    let timestamp_milliseconds = match timestamp.to_unix_time_milliseconds() {
        Ok(value) => value,
        Err(_) => {
            let _ = timestamp.release();
            let _ = current_directory.release();
            return -114;
        }
    };

    if timestamp_milliseconds != 123_456_789 {
        let _ = timestamp.release();
        let _ = current_directory.release();
        return -115;
    }

    let timestamp_text = match timestamp.to_string() {
        Ok(value) => value,
        Err(_) => {
            let _ = timestamp.release();
            let _ = current_directory.release();
            return -116;
        }
    };

    let timestamp_text_len = match timestamp_text.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = timestamp_text.release();
            let _ = timestamp.release();
            let _ = current_directory.release();
            return -117;
        }
    };

    if timestamp_text_len <= 0 {
        let _ = timestamp_text.release();
        let _ = timestamp.release();
        let _ = current_directory.release();
        return -118;
    }

    if timestamp_text.release().is_err() {
        let _ = timestamp.release();
        let _ = current_directory.release();
        return -119;
    }

    if timestamp.release().is_err() {
        let _ = current_directory.release();
        return -120;
    }

    let guid_bytes = b"00000000-0000-0000-0000-000000000123";
    let guid_text = match system::ManagedString::from_utf8_parts(
        guid_bytes.as_ptr(),
        guid_bytes.len() as i64,
    ) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -121;
        }
    };

    let guid = match system::guid::parse(&guid_text) {
        Ok(value) => value,
        Err(_) => {
            let _ = guid_text.release();
            let _ = current_directory.release();
            return -122;
        }
    };

    let rendered_guid = match guid.to_string() {
        Ok(value) => value,
        Err(_) => {
            let _ = guid.release();
            let _ = guid_text.release();
            let _ = current_directory.release();
            return -123;
        }
    };

    let rendered_guid_len = match rendered_guid.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = rendered_guid.release();
            let _ = guid.release();
            let _ = guid_text.release();
            let _ = current_directory.release();
            return -124;
        }
    };

    let rendered_guid_index = match rendered_guid.index_of(&guid_text) {
        Ok(value) => value,
        Err(_) => {
            let _ = rendered_guid.release();
            let _ = guid.release();
            let _ = guid_text.release();
            let _ = current_directory.release();
            return -125;
        }
    };

    if rendered_guid_len != 36 || rendered_guid_index != 0 {
        let _ = rendered_guid.release();
        let _ = guid.release();
        let _ = guid_text.release();
        let _ = current_directory.release();
        return -126;
    }

    if rendered_guid.release().is_err() {
        let _ = guid.release();
        let _ = guid_text.release();
        let _ = current_directory.release();
        return -127;
    }

    if guid.release().is_err() {
        let _ = guid_text.release();
        let _ = current_directory.release();
        return -128;
    }

    if guid_text.release().is_err() {
        let _ = current_directory.release();
        return -129;
    }

    let callback_score = match system::callback::apply_i32(callback_double, 21) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -130;
        }
    };

    if callback_score != 42 {
        let _ = current_directory.release();
        return -131;
    }

    let callback_pair_score = match system::callback::apply_i32_i32(callback_add, 20, 22) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -132;
        }
    };

    if callback_pair_score != 42 {
        let _ = current_directory.release();
        return -133;
    }

    let callback_input = match system::ManagedString::from_utf8_parts(b"callback".as_ptr(), 8) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -134;
        }
    };
    let callback_output = match system::callback::transform_managed_string(callback_string_transform, &callback_input) {
        Ok(value) => value,
        Err(_) => {
            let _ = callback_input.release();
            let _ = current_directory.release();
            return -135;
        }
    };
    let callback_output_len = match callback_output.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = callback_output.release();
            let _ = callback_input.release();
            let _ = current_directory.release();
            return -136;
        }
    };
    if callback_output_len != 11 {
        let _ = callback_output.release();
        let _ = callback_input.release();
        let _ = current_directory.release();
        return -137;
    }
    if callback_output.release().is_err() {
        let _ = callback_input.release();
        let _ = current_directory.release();
        return -138;
    }
    if callback_input.release().is_err() {
        let _ = current_directory.release();
        return -139;
    }

    let int_array = match system::ManagedIntArray::from_i32_triplet(11, 22, 33) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -155;
        }
    };

    let int_array_len = match int_array.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = int_array.release();
            let _ = current_directory.release();
            return -156;
        }
    };

    if int_array_len != 3 {
        let _ = int_array.release();
        let _ = current_directory.release();
        return -157;
    }

    let int_array_middle = match int_array.get(1) {
        Ok(value) => value,
        Err(_) => {
            let _ = int_array.release();
            let _ = current_directory.release();
            return -158;
        }
    };

    let mut int_array_copy = [0i32; 2];
    let int_array_required = match int_array.copy_into(&mut int_array_copy) {
        Ok(value) => value,
        Err(_) => {
            let _ = int_array.release();
            let _ = current_directory.release();
            return -159;
        }
    };

    if int_array_middle != 22
        || int_array_required != 3
        || int_array_copy[0] != 11
        || int_array_copy[1] != 22
    {
        let _ = int_array.release();
        let _ = current_directory.release();
        return -160;
    }

    if int_array.release().is_err() {
        let _ = current_directory.release();
        return -161;
    }

    let byte_array = match system::ManagedByteArray::from_u8_triplet(0x11, 0x22, 0xFE) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -162;
        }
    };

    let byte_array_len = match byte_array.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = byte_array.release();
            let _ = current_directory.release();
            return -163;
        }
    };

    if byte_array_len != 3 {
        let _ = byte_array.release();
        let _ = current_directory.release();
        return -164;
    }

    let byte_array_last = match byte_array.get(2) {
        Ok(value) => value,
        Err(_) => {
            let _ = byte_array.release();
            let _ = current_directory.release();
            return -165;
        }
    };

    let mut byte_array_copy = [0u8; 2];
    let byte_array_required = match byte_array.copy_into(&mut byte_array_copy) {
        Ok(value) => value,
        Err(_) => {
            let _ = byte_array.release();
            let _ = current_directory.release();
            return -166;
        }
    };

    if byte_array_last != 0xFE
        || byte_array_required != 3
        || byte_array_copy[0] != 0x11
        || byte_array_copy[1] != 0x22
    {
        let _ = byte_array.release();
        let _ = current_directory.release();
        return -167;
    }

    if byte_array.release().is_err() {
        let _ = current_directory.release();
        return -168;
    }

    let array_first = match system::ManagedString::from_utf8_parts(b"red".as_ptr(), 3) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -169;
        }
    };
    let array_second = match system::ManagedString::from_utf8_parts(b"green".as_ptr(), 5) {
        Ok(value) => value,
        Err(_) => {
            let _ = array_first.release();
            let _ = current_directory.release();
            return -170;
        }
    };
    let array_third = match system::ManagedString::from_utf8_parts(b"blue".as_ptr(), 4) {
        Ok(value) => value,
        Err(_) => {
            let _ = array_second.release();
            let _ = array_first.release();
            let _ = current_directory.release();
            return -171;
        }
    };
    let object_array = match system::ManagedStringArray::from_strings(&array_first, &array_second, &array_third) {
        Ok(value) => value,
        Err(_) => {
            let _ = array_third.release();
            let _ = array_second.release();
            let _ = array_first.release();
            let _ = current_directory.release();
            return -172;
        }
    };
    let object_array_len = match object_array.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = object_array.release();
            let _ = array_third.release();
            let _ = array_second.release();
            let _ = array_first.release();
            let _ = current_directory.release();
            return -173;
        }
    };
    if object_array_len != 3 {
        let _ = object_array.release();
        let _ = array_third.release();
        let _ = array_second.release();
        let _ = array_first.release();
        let _ = current_directory.release();
        return -174;
    }
    let object_array_value = match object_array.get(1) {
        Ok(value) => value,
        Err(_) => {
            let _ = object_array.release();
            let _ = array_third.release();
            let _ = array_second.release();
            let _ = array_first.release();
            let _ = current_directory.release();
            return -175;
        }
    };
    let object_array_value_len = match object_array_value.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = object_array_value.release();
            let _ = object_array.release();
            let _ = array_third.release();
            let _ = array_second.release();
            let _ = array_first.release();
            let _ = current_directory.release();
            return -176;
        }
    };
    if object_array_value_len != 5 {
        let _ = object_array_value.release();
        let _ = object_array.release();
        let _ = array_third.release();
        let _ = array_second.release();
        let _ = array_first.release();
        let _ = current_directory.release();
        return -177;
    }
    if object_array_value.release().is_err() {
        let _ = object_array.release();
        let _ = array_third.release();
        let _ = array_second.release();
        let _ = array_first.release();
        let _ = current_directory.release();
        return -178;
    }
    if object_array.release().is_err() {
        let _ = array_third.release();
        let _ = array_second.release();
        let _ = array_first.release();
        let _ = current_directory.release();
        return -179;
    }
    if array_third.release().is_err() || array_second.release().is_err() || array_first.release().is_err() {
        let _ = current_directory.release();
        return -180;
    }

    let missing_path = b"__rust_mcil_missing_exception_probe__.txt";
    let missing_exception = match system::io::file::read_all_lines_utf8_parts(
        missing_path.as_ptr(),
        missing_path.len() as i64,
    ) {
        Ok(lines) => {
            let _ = lines.release();
            let _ = current_directory.release();
            return -71;
        }
        Err(exception) => exception,
    };

    let type_name_len = match missing_exception.type_name_utf8_len() {
        Ok(value) => value,
        Err(diagnostic_exception) => {
            let _ = diagnostic_exception.release();
            let _ = missing_exception.release();
            let _ = current_directory.release();
            return -72;
        }
    };

    if type_name_len <= 0 || type_name_len > 128 {
        let _ = missing_exception.release();
        let _ = current_directory.release();
        return -73;
    }

    let mut type_name_buffer = [0u8; 128];
    let type_name_written = match missing_exception.copy_type_name_utf8_into(&mut type_name_buffer) {
        Ok(value) => value,
        Err(diagnostic_exception) => {
            let _ = diagnostic_exception.release();
            let _ = missing_exception.release();
            let _ = current_directory.release();
            return -74;
        }
    };

    if type_name_written != type_name_len {
        let _ = missing_exception.release();
        let _ = current_directory.release();
        return -75;
    }

    if !type_name_buffer[..type_name_written as usize].ends_with(b"FileNotFoundException") {
        let _ = missing_exception.release();
        let _ = current_directory.release();
        return -76;
    }

    let message_len = match missing_exception.message_utf8_len() {
        Ok(value) => value,
        Err(diagnostic_exception) => {
            let _ = diagnostic_exception.release();
            let _ = missing_exception.release();
            let _ = current_directory.release();
            return -77;
        }
    };

    if message_len <= type_name_len || message_len > 512 {
        let _ = missing_exception.release();
        let _ = current_directory.release();
        return -78;
    }

    let mut message_buffer = [0u8; 512];
    let message_written = match missing_exception.copy_message_utf8_into(&mut message_buffer) {
        Ok(value) => value,
        Err(diagnostic_exception) => {
            let _ = diagnostic_exception.release();
            let _ = missing_exception.release();
            let _ = current_directory.release();
            return -79;
        }
    };

    if message_written <= 0 || message_written > message_len {
        let _ = missing_exception.release();
        let _ = current_directory.release();
        return -80;
    }

    if missing_exception.release().is_err() {
        let _ = current_directory.release();
        return -81;
    }

    if current_directory.release().is_err() {
        return -4;
    }

    100
}
