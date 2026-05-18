unsafe extern "C" {
    fn rust_mcil_dotnet_current_directory_utf8_len() -> i32;
    fn rust_mcil_dotnet_copy_current_directory_utf8(
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_temp_path_utf8_len() -> i32;
    fn rust_mcil_dotnet_copy_temp_path_utf8(
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_documents_utf8_len() -> i32;
    fn rust_mcil_dotnet_copy_documents_utf8(
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_get_full_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
        base_ptr: *const u8,
        base_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_full_utf8(
        path_ptr: *const u8,
        path_len: i64,
        base_ptr: *const u8,
        base_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_get_root_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_root_utf8(
        path_ptr: *const u8,
        path_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_get_relative_utf8_len(
        relative_to_ptr: *const u8,
        relative_to_len: i64,
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_relative_utf8(
        relative_to_ptr: *const u8,
        relative_to_len: i64,
        path_ptr: *const u8,
        path_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_get_directory_name_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_directory_name_utf8(
        path_ptr: *const u8,
        path_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_combine3_utf8_len(
        first_ptr: *const u8,
        first_len: i64,
        second_ptr: *const u8,
        second_len: i64,
        third_ptr: *const u8,
        third_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_combine3_utf8(
        first_ptr: *const u8,
        first_len: i64,
        second_ptr: *const u8,
        second_len: i64,
        third_ptr: *const u8,
        third_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_get_file_name_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_file_name_utf8(
        path_ptr: *const u8,
        path_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_change_extension_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
        extension_ptr: *const u8,
        extension_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_change_extension_utf8(
        path_ptr: *const u8,
        path_len: i64,
        extension_ptr: *const u8,
        extension_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
        path_ptr: *const u8,
        path_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_string_replace_utf8_len(
        source_ptr: *const u8,
        source_len: i64,
        old_ptr: *const u8,
        old_len: i64,
        new_ptr: *const u8,
        new_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_string_copy_replace_utf8(
        source_ptr: *const u8,
        source_len: i64,
        old_ptr: *const u8,
        old_len: i64,
        new_ptr: *const u8,
        new_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_string_contains(
        haystack_ptr: *const u8,
        haystack_len: i64,
        needle_ptr: *const u8,
        needle_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_string_index_of(
        haystack_ptr: *const u8,
        haystack_len: i64,
        needle_ptr: *const u8,
        needle_len: i64,
    ) -> i32;
}

#[unsafe(no_mangle)]
pub extern "C" fn dotnet_runtime_path_thirty_stage_rank_score() -> i32 {
    let current_relative_input = "samples\\std_fs\\fixtures\\..\\fixtures\\input.txt";
    let current_extension = ".data";
    let current_old = "in";
    let current_new = "trace";

    let temp_second = "managed";
    let temp_third = "output.bin";
    let temp_extension = ".data";
    let temp_old = "out";
    let temp_new = "trace";

    let documents_second = "archive";
    let documents_third = "notes.log";
    let documents_extension = ".memo";
    let documents_old = "ot";
    let documents_new = "ace";
    let documents_needle = "ace";

    let trace_old = "out";
    let trace_new = "trace";
    let trace_extension = ".trace";
    let trace_needle = "trace";

    let grid_old = "ot";
    let grid_new = "ace";
    let grid_extension = ".grid";
    let grid_needle = "ace";

    let flow_old = "in";
    let flow_new = "path";
    let flow_extension = ".flow";
    let flow_needle = "path";

    let current_segment = ".";
    let current_stage_needle = "fixtures";
    let temp_stage_needle = "managed";
    let documents_stage_needle = "archive";
    let mesh_old = "ou";
    let mesh_new = "mesh";
    let mesh_needle = "mesh";
    let route_old = "in";
    let route_new = "route";
    let route_needle = "route";
    let array_old = "ot";
    let array_new = "array";
    let array_needle = "array";
    let spine_old = "fixtures";
    let spine_new = "spine";
    let spine_needle = "spine";
    let weave_old = "managed";
    let weave_new = "weave";
    let weave_needle = "weave";
    let orbit_old = "archive";
    let orbit_new = "orbit";
    let orbit_needle = "orbit";
    let self_stage_needle = "path";
    let temp_stage_six_needle = "managed";
    let documents_stage_six_needle = "archive";
    let final_old = "ou";
    let final_new = "path";
    let final_needle = "path";
    let crown_old = "path";
    let crown_new = "crown";
    let crown_needle = "crown";
    let pulse_extension = ".pulse";
    let pulse_needle = "pulse";
    let anchor_old = "path";
    let anchor_new = "anchor";
    let anchor_needle = "anchor";
    let crest_old = "path";
    let crest_new = "crest";
    let crest_needle = "crest";
    let flare_extension = ".flare";
    let flare_needle = "flare";
    let lattice_old = "anchor";
    let lattice_new = "lattice";
    let lattice_needle = "lattice";
    let summit_old = "crest";
    let summit_new = "summit";
    let summit_needle = "summit";
    let prism_extension = ".prism";
    let prism_needle = "prism";
    let vault_old = "lattice";
    let vault_new = "vault";
    let vault_needle = "vault";
    let beacon_old = "summit";
    let beacon_new = "beacon";
    let beacon_needle = "beacon";
    let ember_extension = ".ember";
    let ember_needle = "ember";
    let harbor_old = "vault";
    let harbor_new = "harbor";
    let harbor_needle = "harbor";
    let ridge_old = "beacon";
    let ridge_new = "ridge";
    let ridge_needle = "ridge";
    let glow_extension = ".glow";
    let glow_needle = "glow";
    let quay_old = "harbor";
    let quay_new = "quay";
    let quay_needle = "quay";
    let crestline_old = "ridge";
    let crestline_new = "crestline";
    let crestline_needle = "crestline";
    let spark_extension = ".spark";
    let spark_needle = "spark";
    let inlet_old = "quay";
    let inlet_new = "inlet";
    let inlet_needle = "inlet";
    let skyline_old = "crestline";
    let skyline_new = "skyline";
    let skyline_needle = "skyline";
    let flare_extension = ".flarex";
    let flare_needle = "flarex";
    let harbor_old_two = "inlet";
    let harbor_new_two = "harborx";
    let harbor_needle_two = "harborx";
    let sunline_old = "skyline";
    let sunline_new = "sunline";
    let sunline_needle = "sunline";
    let glint_extension = ".glint";
    let glint_needle = "glint";
    let anchory_old = "harborx";
    let anchory_new = "anchory";
    let anchory_needle = "anchory";
    let starline_old = "sunline";
    let starline_new = "starline";
    let starline_needle = "starline";
    let comet_extension = ".comet";
    let comet_needle = "comet";
    let pier_old = "anchory";
    let pier_new = "pierline";
    let pier_needle = "pierline";
    let moonline_old = "starline";
    let moonline_new = "moonline";
    let moonline_needle = "moonline";
    let nova_extension = ".nova";
    let nova_needle = "nova";
    let jetty_old = "pierline";
    let jetty_new = "jettyline";
    let jetty_needle = "jettyline";
    let tide_old = "moonline";
    let tide_new = "tideway";
    let tide_needle = "tideway";
    let quasar_extension = ".quasar";
    let quasar_needle = "quasar";
    let harbor_old_three = "jettyline";
    let harbor_new_three = "harborline";
    let harbor_needle_three = "harborline";
    let estuary_old = "tideway";
    let estuary_new = "estuary";
    let estuary_needle = "estuary";
    let pulsar_extension = ".pulsar";
    let pulsar_needle = "pulsar";
    let dock_old = "harborline";
    let dock_new = "dockline";
    let dock_needle = "dockline";
    let shoreline_old = "estuary";
    let shoreline_new = "shoreline";
    let shoreline_needle = "shoreline";
    let meteor_extension = ".meteor";
    let meteor_needle = "meteor";
    let quay_old_two = "dockline";
    let quay_new_two = "quayside";
    let quay_needle_two = "quayside";
    let breakwater_old = "shoreline";
    let breakwater_new = "breakwater";
    let breakwater_needle = "breakwater";
    let aurora_extension = ".aurora";
    let aurora_needle = "aurora";
    let pier_old_two = "quayside";
    let pier_new_two = "pierpoint";
    let pier_needle_two = "pierpoint";
    let causeway_old = "breakwater";
    let causeway_new = "causeway";
    let causeway_needle = "causeway";
    let starlight_extension = ".starlight";
    let starlight_needle = "starlight";
    let berth_old = "pierpoint";
    let berth_new = "berthside";
    let berth_needle = "berthside";
    let boardwalk_old = "causeway";
    let boardwalk_new = "boardwalk";
    let boardwalk_needle = "boardwalk";
    let moonrise_extension = ".moonrise";
    let moonrise_needle = "moonrise";
    let dock_old = "berthside";
    let dock_new = "dockfront";
    let dock_needle = "dockfront";
    let esplanade_old = "boardwalk";
    let esplanade_new = "esplanade";
    let esplanade_needle = "esplanade";
    let daybreak_extension = ".daybreak";
    let daybreak_needle = "daybreak";
    let slip_old = "dockfront";
    let slip_new = "slipway";
    let slip_needle = "slipway";
    let promenade_old = "esplanade";
    let promenade_new = "promenade";
    let promenade_needle = "promenade";
    let sunrise_extension = ".sunrise";
    let sunrise_needle = "sunrise";
    let jetty_old = "slipway";
    let jetty_new = "jettyline";
    let jetty_needle = "jettyline";
    let plaza_old = "promenade";
    let plaza_new = "plaza";
    let plaza_needle = "plaza";
    let solstice_extension = ".solstice";
    let solstice_needle = "solstice";
    let wharf_old = "jettyline";
    let wharf_new = "wharfgate";
    let wharf_needle = "wharfgate";
    let arcade_old = "plaza";
    let arcade_new = "arcade";
    let arcade_needle = "arcade";
    let eclipse_extension = ".eclipse";
    let eclipse_needle = "eclipse";
    let quay_old_three = "wharfgate";
    let quay_new_three = "quaylight";
    let quay_needle_three = "quaylight";
    let gallery_old = "arcade";
    let gallery_new = "gallery";
    let gallery_needle = "gallery";
    let equinox_extension = ".equinox";
    let equinox_needle = "equinox";
    let berth_old_three = "quaylight";
    let berth_new_three = "berthdeck";
    let berth_needle_three = "berthdeck";
    let observatory_old = "gallery";
    let observatory_new = "observatory";
    let observatory_needle = "observatory";
    let zenith_extension = ".zenith";
    let zenith_needle = "zenith";
    let harbor_old_four = "berthdeck";
    let harbor_new_four = "harborcrest";
    let harbor_needle_four = "harborcrest";
    let parapet_old = "observatory";
    let parapet_new = "parapet";
    let parapet_needle = "parapet";
    let twilight_extension = ".twilight";
    let twilight_needle = "twilight";
    let mooring_old = "harborcrest";
    let mooring_new = "mooringbay";
    let mooring_needle = "mooringbay";
    let skywalk_old = "parapet";
    let skywalk_new = "skywalk";
    let skywalk_needle = "skywalk";
    let starglow_extension = ".starglow";
    let starglow_needle = "starglow";
    let dock_old_four = "mooringbay";
    let dock_new_four = "dockwatch";
    let dock_needle_four = "dockwatch";

    let current_base_len = unsafe { rust_mcil_dotnet_current_directory_utf8_len() };
    let mut current_base = vec![0u8; current_base_len as usize];
    let current_base_written = unsafe {
        rust_mcil_dotnet_copy_current_directory_utf8(
            current_base.as_mut_ptr(),
            current_base.len() as i64,
        )
    };

    let current_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            current_relative_input.as_ptr(),
            current_relative_input.len() as i64,
            current_base.as_ptr(),
            current_base_written as i64,
        )
    };
    let mut current_full = vec![0u8; current_full_len as usize];
    let current_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            current_relative_input.as_ptr(),
            current_relative_input.len() as i64,
            current_base.as_ptr(),
            current_base_written as i64,
            current_full.as_mut_ptr(),
            current_full.len() as i64,
        )
    };

    let current_root_len = unsafe {
        rust_mcil_dotnet_path_get_root_utf8_len(current_full.as_ptr(), current_full_written as i64)
    };
    let mut current_root = vec![0u8; current_root_len as usize];
    let current_root_written = unsafe {
        rust_mcil_dotnet_path_copy_root_utf8(
            current_full.as_ptr(),
            current_full_written as i64,
            current_root.as_mut_ptr(),
            current_root.len() as i64,
        )
    };

    let current_relative_len = unsafe {
        rust_mcil_dotnet_path_get_relative_utf8_len(
            current_root.as_ptr(),
            current_root_written as i64,
            current_full.as_ptr(),
            current_full_written as i64,
        )
    };
    let mut current_relative = vec![0u8; current_relative_len as usize];
    let current_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_relative_utf8(
            current_root.as_ptr(),
            current_root_written as i64,
            current_full.as_ptr(),
            current_full_written as i64,
            current_relative.as_mut_ptr(),
            current_relative.len() as i64,
        )
    };

    let current_directory_len = unsafe {
        rust_mcil_dotnet_path_get_directory_name_utf8_len(
            current_relative.as_ptr(),
            current_relative_written as i64,
        )
    };
    let mut current_directory = vec![0u8; current_directory_len as usize];
    let current_directory_written = unsafe {
        rust_mcil_dotnet_path_copy_directory_name_utf8(
            current_relative.as_ptr(),
            current_relative_written as i64,
            current_directory.as_mut_ptr(),
            current_directory.len() as i64,
        )
    };

    let current_file_name_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_utf8_len(
            current_relative.as_ptr(),
            current_relative_written as i64,
        )
    };
    let mut current_file_name = vec![0u8; current_file_name_len as usize];
    let current_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_utf8(
            current_relative.as_ptr(),
            current_relative_written as i64,
            current_file_name.as_mut_ptr(),
            current_file_name.len() as i64,
        )
    };
    let current_changed_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            current_file_name.as_ptr(),
            current_file_name_written as i64,
            current_extension.as_ptr(),
            current_extension.len() as i64,
        )
    };
    let mut current_changed = vec![0u8; current_changed_len as usize];
    let current_changed_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            current_file_name.as_ptr(),
            current_file_name_written as i64,
            current_extension.as_ptr(),
            current_extension.len() as i64,
            current_changed.as_mut_ptr(),
            current_changed.len() as i64,
        )
    };
    let current_leaf_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            current_changed.as_ptr(),
            current_changed_written as i64,
        )
    };
    let mut current_leaf = vec![0u8; current_leaf_len as usize];
    let current_leaf_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            current_changed.as_ptr(),
            current_changed_written as i64,
            current_leaf.as_mut_ptr(),
            current_leaf.len() as i64,
        )
    };
    let current_transformed_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            current_leaf.as_ptr(),
            current_leaf_written as i64,
            current_old.as_ptr(),
            current_old.len() as i64,
            current_new.as_ptr(),
            current_new.len() as i64,
        )
    };
    let mut current_transformed = vec![0u8; current_transformed_len as usize];
    let current_transformed_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            current_leaf.as_ptr(),
            current_leaf_written as i64,
            current_old.as_ptr(),
            current_old.len() as i64,
            current_new.as_ptr(),
            current_new.len() as i64,
            current_transformed.as_mut_ptr(),
            current_transformed.len() as i64,
        )
    };
    let current_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            current_transformed.as_ptr(),
            current_transformed_written as i64,
            documents_needle.as_ptr(),
            documents_needle.len() as i64,
        )
    };

    let temp_root_len = unsafe { rust_mcil_dotnet_temp_path_utf8_len() };
    let mut temp_root = vec![0u8; temp_root_len as usize];
    let temp_root_written = unsafe {
        rust_mcil_dotnet_copy_temp_path_utf8(temp_root.as_mut_ptr(), temp_root.len() as i64)
    };
    let temp_combined_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            temp_root.as_ptr(),
            temp_root_written as i64,
            temp_second.as_ptr(),
            temp_second.len() as i64,
            temp_third.as_ptr(),
            temp_third.len() as i64,
        )
    };
    let mut temp_combined = vec![0u8; temp_combined_len as usize];
    let temp_combined_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            temp_root.as_ptr(),
            temp_root_written as i64,
            temp_second.as_ptr(),
            temp_second.len() as i64,
            temp_third.as_ptr(),
            temp_third.len() as i64,
            temp_combined.as_mut_ptr(),
            temp_combined.len() as i64,
        )
    };
    let temp_changed_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            temp_combined.as_ptr(),
            temp_combined_written as i64,
            temp_extension.as_ptr(),
            temp_extension.len() as i64,
        )
    };
    let mut temp_changed = vec![0u8; temp_changed_len as usize];
    let temp_changed_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            temp_combined.as_ptr(),
            temp_combined_written as i64,
            temp_extension.as_ptr(),
            temp_extension.len() as i64,
            temp_changed.as_mut_ptr(),
            temp_changed.len() as i64,
        )
    };
    let temp_relative_len = unsafe {
        rust_mcil_dotnet_path_get_relative_utf8_len(
            temp_root.as_ptr(),
            temp_root_written as i64,
            temp_changed.as_ptr(),
            temp_changed_written as i64,
        )
    };
    let mut temp_relative = vec![0u8; temp_relative_len as usize];
    let temp_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_relative_utf8(
            temp_root.as_ptr(),
            temp_root_written as i64,
            temp_changed.as_ptr(),
            temp_changed_written as i64,
            temp_relative.as_mut_ptr(),
            temp_relative.len() as i64,
        )
    };
    let temp_directory_len = unsafe {
        rust_mcil_dotnet_path_get_directory_name_utf8_len(
            temp_relative.as_ptr(),
            temp_relative_written as i64,
        )
    };
    let mut temp_directory = vec![0u8; temp_directory_len as usize];
    let temp_directory_written = unsafe {
        rust_mcil_dotnet_path_copy_directory_name_utf8(
            temp_relative.as_ptr(),
            temp_relative_written as i64,
            temp_directory.as_mut_ptr(),
            temp_directory.len() as i64,
        )
    };
    let temp_file_name_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_utf8_len(temp_changed.as_ptr(), temp_changed_written as i64)
    };
    let mut temp_file_name = vec![0u8; temp_file_name_len as usize];
    let temp_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_utf8(
            temp_changed.as_ptr(),
            temp_changed_written as i64,
            temp_file_name.as_mut_ptr(),
            temp_file_name.len() as i64,
        )
    };
    let temp_leaf_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            temp_file_name.as_ptr(),
            temp_file_name_written as i64,
        )
    };
    let mut temp_leaf = vec![0u8; temp_leaf_len as usize];
    let temp_leaf_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            temp_file_name.as_ptr(),
            temp_file_name_written as i64,
            temp_leaf.as_mut_ptr(),
            temp_leaf.len() as i64,
        )
    };
    let temp_transformed_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            temp_leaf.as_ptr(),
            temp_leaf_written as i64,
            temp_old.as_ptr(),
            temp_old.len() as i64,
            temp_new.as_ptr(),
            temp_new.len() as i64,
        )
    };
    let mut temp_transformed = vec![0u8; temp_transformed_len as usize];
    let temp_transformed_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            temp_leaf.as_ptr(),
            temp_leaf_written as i64,
            temp_old.as_ptr(),
            temp_old.len() as i64,
            temp_new.as_ptr(),
            temp_new.len() as i64,
            temp_transformed.as_mut_ptr(),
            temp_transformed.len() as i64,
        )
    };
    let temp_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            temp_transformed.as_ptr(),
            temp_transformed_written as i64,
            documents_needle.as_ptr(),
            documents_needle.len() as i64,
        )
    };

    let documents_root_len = unsafe { rust_mcil_dotnet_documents_utf8_len() };
    let mut documents_root = vec![0u8; documents_root_len as usize];
    let documents_root_written = unsafe {
        rust_mcil_dotnet_copy_documents_utf8(
            documents_root.as_mut_ptr(),
            documents_root.len() as i64,
        )
    };
    let documents_combined_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            documents_root.as_ptr(),
            documents_root_written as i64,
            documents_second.as_ptr(),
            documents_second.len() as i64,
            documents_third.as_ptr(),
            documents_third.len() as i64,
        )
    };
    let mut documents_combined = vec![0u8; documents_combined_len as usize];
    let documents_combined_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            documents_root.as_ptr(),
            documents_root_written as i64,
            documents_second.as_ptr(),
            documents_second.len() as i64,
            documents_third.as_ptr(),
            documents_third.len() as i64,
            documents_combined.as_mut_ptr(),
            documents_combined.len() as i64,
        )
    };
    let documents_changed_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            documents_combined.as_ptr(),
            documents_combined_written as i64,
            documents_extension.as_ptr(),
            documents_extension.len() as i64,
        )
    };
    let mut documents_changed = vec![0u8; documents_changed_len as usize];
    let documents_changed_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            documents_combined.as_ptr(),
            documents_combined_written as i64,
            documents_extension.as_ptr(),
            documents_extension.len() as i64,
            documents_changed.as_mut_ptr(),
            documents_changed.len() as i64,
        )
    };
    let documents_relative_len = unsafe {
        rust_mcil_dotnet_path_get_relative_utf8_len(
            documents_root.as_ptr(),
            documents_root_written as i64,
            documents_changed.as_ptr(),
            documents_changed_written as i64,
        )
    };
    let mut documents_relative = vec![0u8; documents_relative_len as usize];
    let documents_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_relative_utf8(
            documents_root.as_ptr(),
            documents_root_written as i64,
            documents_changed.as_ptr(),
            documents_changed_written as i64,
            documents_relative.as_mut_ptr(),
            documents_relative.len() as i64,
        )
    };
    let documents_directory_len = unsafe {
        rust_mcil_dotnet_path_get_directory_name_utf8_len(
            documents_relative.as_ptr(),
            documents_relative_written as i64,
        )
    };
    let mut documents_directory = vec![0u8; documents_directory_len as usize];
    let documents_directory_written = unsafe {
        rust_mcil_dotnet_path_copy_directory_name_utf8(
            documents_relative.as_ptr(),
            documents_relative_written as i64,
            documents_directory.as_mut_ptr(),
            documents_directory.len() as i64,
        )
    };
    let documents_file_name_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_utf8_len(
            documents_changed.as_ptr(),
            documents_changed_written as i64,
        )
    };
    let mut documents_file_name = vec![0u8; documents_file_name_len as usize];
    let documents_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_utf8(
            documents_changed.as_ptr(),
            documents_changed_written as i64,
            documents_file_name.as_mut_ptr(),
            documents_file_name.len() as i64,
        )
    };
    let documents_leaf_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            documents_file_name.as_ptr(),
            documents_file_name_written as i64,
        )
    };
    let mut documents_leaf = vec![0u8; documents_leaf_len as usize];
    let documents_leaf_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            documents_file_name.as_ptr(),
            documents_file_name_written as i64,
            documents_leaf.as_mut_ptr(),
            documents_leaf.len() as i64,
        )
    };
    let documents_transformed_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            documents_leaf.as_ptr(),
            documents_leaf_written as i64,
            documents_old.as_ptr(),
            documents_old.len() as i64,
            documents_new.as_ptr(),
            documents_new.len() as i64,
        )
    };
    let mut documents_transformed = vec![0u8; documents_transformed_len as usize];
    let documents_transformed_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            documents_leaf.as_ptr(),
            documents_leaf_written as i64,
            documents_old.as_ptr(),
            documents_old.len() as i64,
            documents_new.as_ptr(),
            documents_new.len() as i64,
            documents_transformed.as_mut_ptr(),
            documents_transformed.len() as i64,
        )
    };
    let documents_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            documents_transformed.as_ptr(),
            documents_transformed_written as i64,
            documents_needle.as_ptr(),
            documents_needle.len() as i64,
        )
    };

    let mut selected_directory_index = 0i32;
    let mut selected_file_index = 0i32;
    let mut best_pair_score = i32::MIN;
    let mut directory_index = 0i32;
    while directory_index < 3 {
        let mut candidate_directory_rank = current_transformed_written * 10 + current_index;
        if directory_index == 1 {
            candidate_directory_rank = temp_transformed_written * 10 + temp_index;
        } else if directory_index == 2 {
            candidate_directory_rank = documents_contains * 100 + documents_transformed_written;
        }

        let mut file_index = 0i32;
        while file_index < 3 {
            let mut candidate_file_rank = current_changed_written * 10 + current_index;
            if file_index == 1 {
                candidate_file_rank = temp_file_name_written * 10 + temp_index;
            } else if file_index == 2 {
                candidate_file_rank = documents_file_name_written * 10 + documents_contains;
            }

            let mut pair_bonus = 0i32;
            if directory_index == file_index {
                pair_bonus += 200;
            }
            if directory_index == 2 && documents_contains != 0 {
                pair_bonus += 30;
            }
            if file_index == 1 && temp_index >= current_index {
                pair_bonus += 20;
            }
            if file_index == 0 && current_index >= 0 {
                pair_bonus += 10;
            }

            let pair_score = candidate_directory_rank * 100 + candidate_file_rank + pair_bonus;
            if pair_score > best_pair_score {
                best_pair_score = pair_score;
                selected_directory_index = directory_index;
                selected_file_index = file_index;
            }

            file_index += 1;
        }

        directory_index += 1;
    }

    let mut selected_directory_ptr = current_directory.as_ptr();
    let mut selected_directory_written = current_directory_written;
    if selected_directory_index == 1 {
        selected_directory_ptr = temp_directory.as_ptr();
        selected_directory_written = temp_directory_written;
    } else if selected_directory_index == 2 {
        selected_directory_ptr = documents_directory.as_ptr();
        selected_directory_written = documents_directory_written;
    }

    let mut selected_file_name_ptr = current_changed.as_ptr();
    let mut selected_file_name_written = current_changed_written;
    if selected_file_index == 1 {
        selected_file_name_ptr = temp_file_name.as_ptr();
        selected_file_name_written = temp_file_name_written;
    } else if selected_file_index == 2 {
        selected_file_name_ptr = documents_file_name.as_ptr();
        selected_file_name_written = documents_file_name_written;
    }

    let selected_leaf_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_file_name_ptr,
            selected_file_name_written as i64,
        )
    };
    let mut selected_leaf = vec![0u8; selected_leaf_len as usize];
    let selected_leaf_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_file_name_ptr,
            selected_file_name_written as i64,
            selected_leaf.as_mut_ptr(),
            selected_leaf.len() as i64,
        )
    };

    let trace_variant_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_leaf.as_ptr(),
            selected_leaf_written as i64,
            trace_old.as_ptr(),
            trace_old.len() as i64,
            trace_new.as_ptr(),
            trace_new.len() as i64,
        )
    };
    let mut trace_variant = vec![0u8; trace_variant_len as usize];
    let trace_variant_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_leaf.as_ptr(),
            selected_leaf_written as i64,
            trace_old.as_ptr(),
            trace_old.len() as i64,
            trace_new.as_ptr(),
            trace_new.len() as i64,
            trace_variant.as_mut_ptr(),
            trace_variant.len() as i64,
        )
    };
    let trace_variant_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            trace_variant.as_ptr(),
            trace_variant_written as i64,
            trace_needle.as_ptr(),
            trace_needle.len() as i64,
        )
    };

    let grid_variant_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_leaf.as_ptr(),
            selected_leaf_written as i64,
            grid_old.as_ptr(),
            grid_old.len() as i64,
            grid_new.as_ptr(),
            grid_new.len() as i64,
        )
    };
    let mut grid_variant = vec![0u8; grid_variant_len as usize];
    let grid_variant_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_leaf.as_ptr(),
            selected_leaf_written as i64,
            grid_old.as_ptr(),
            grid_old.len() as i64,
            grid_new.as_ptr(),
            grid_new.len() as i64,
            grid_variant.as_mut_ptr(),
            grid_variant.len() as i64,
        )
    };
    let grid_variant_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            grid_variant.as_ptr(),
            grid_variant_written as i64,
            grid_needle.as_ptr(),
            grid_needle.len() as i64,
        )
    };

    let flow_variant_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_leaf.as_ptr(),
            selected_leaf_written as i64,
            flow_old.as_ptr(),
            flow_old.len() as i64,
            flow_new.as_ptr(),
            flow_new.len() as i64,
        )
    };
    let mut flow_variant = vec![0u8; flow_variant_len as usize];
    let flow_variant_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_leaf.as_ptr(),
            selected_leaf_written as i64,
            flow_old.as_ptr(),
            flow_old.len() as i64,
            flow_new.as_ptr(),
            flow_new.len() as i64,
            flow_variant.as_mut_ptr(),
            flow_variant.len() as i64,
        )
    };
    let flow_variant_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            flow_variant.as_ptr(),
            flow_variant_written as i64,
            flow_needle.as_ptr(),
            flow_needle.len() as i64,
        )
    };

    let trace_file_name_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_file_name_ptr,
            selected_file_name_written as i64,
            trace_extension.as_ptr(),
            trace_extension.len() as i64,
        )
    };
    let mut trace_file_name = vec![0u8; trace_file_name_len as usize];
    let trace_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_file_name_ptr,
            selected_file_name_written as i64,
            trace_extension.as_ptr(),
            trace_extension.len() as i64,
            trace_file_name.as_mut_ptr(),
            trace_file_name.len() as i64,
        )
    };

    let grid_file_name_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_file_name_ptr,
            selected_file_name_written as i64,
            grid_extension.as_ptr(),
            grid_extension.len() as i64,
        )
    };
    let mut grid_file_name = vec![0u8; grid_file_name_len as usize];
    let grid_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_file_name_ptr,
            selected_file_name_written as i64,
            grid_extension.as_ptr(),
            grid_extension.len() as i64,
            grid_file_name.as_mut_ptr(),
            grid_file_name.len() as i64,
        )
    };

    let flow_file_name_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_file_name_ptr,
            selected_file_name_written as i64,
            flow_extension.as_ptr(),
            flow_extension.len() as i64,
        )
    };
    let mut flow_file_name = vec![0u8; flow_file_name_len as usize];
    let flow_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_file_name_ptr,
            selected_file_name_written as i64,
            flow_extension.as_ptr(),
            flow_extension.len() as i64,
            flow_file_name.as_mut_ptr(),
            flow_file_name.len() as i64,
        )
    };

    let mut selected_variant_index = 0i32;
    let mut best_variant_score = i32::MIN;
    let mut variant_index = 0i32;
    while variant_index < 3 {
        let mut candidate_variant_score = trace_variant_written * 10 + trace_variant_index;
        if variant_index == 1 {
            candidate_variant_score = grid_variant_written * 10 + grid_variant_contains * 50;
        } else if variant_index == 2 {
            candidate_variant_score = flow_variant_written * 10 + flow_variant_index;
        }

        let mut variant_bonus = 0i32;
        if variant_index == selected_file_index {
            variant_bonus += 25;
        }
        if variant_index == selected_directory_index {
            variant_bonus += 15;
        }
        if selected_file_index == 1 && variant_index == 0 {
            variant_bonus += 40;
        }
        if selected_directory_index == 2 && variant_index == 1 {
            variant_bonus += 10;
        }
        if selected_file_index == 0 && variant_index == 2 {
            variant_bonus += 5;
        }

        let variant_score = candidate_variant_score + variant_bonus;
        if variant_score > best_variant_score {
            best_variant_score = variant_score;
            selected_variant_index = variant_index;
        }

        variant_index += 1;
    }

    let mut selected_variant_file_name_ptr = trace_file_name.as_ptr();
    let mut selected_variant_file_name_written = trace_file_name_written;
    if selected_variant_index == 1 {
        selected_variant_file_name_ptr = grid_file_name.as_ptr();
        selected_variant_file_name_written = grid_file_name_written;
    } else if selected_variant_index == 2 {
        selected_variant_file_name_ptr = flow_file_name.as_ptr();
        selected_variant_file_name_written = flow_file_name_written;
    }

    let current_rebased_relative_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            current_directory.as_ptr(),
            current_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_variant_file_name_ptr,
            selected_variant_file_name_written as i64,
        )
    };
    let mut current_rebased_relative = vec![0u8; current_rebased_relative_len as usize];
    let current_rebased_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            current_directory.as_ptr(),
            current_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_variant_file_name_ptr,
            selected_variant_file_name_written as i64,
            current_rebased_relative.as_mut_ptr(),
            current_rebased_relative.len() as i64,
        )
    };

    let current_rebased_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            current_rebased_relative.as_ptr(),
            current_rebased_relative_written as i64,
            current_root.as_ptr(),
            current_root_written as i64,
        )
    };
    let mut current_rebased_full = vec![0u8; current_rebased_full_len as usize];
    let current_rebased_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            current_rebased_relative.as_ptr(),
            current_rebased_relative_written as i64,
            current_root.as_ptr(),
            current_root_written as i64,
            current_rebased_full.as_mut_ptr(),
            current_rebased_full.len() as i64,
        )
    };

    let current_stage_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            current_rebased_full.as_ptr(),
            current_rebased_full_written as i64,
            current_stage_needle.as_ptr(),
            current_stage_needle.len() as i64,
        )
    };
    let current_stage_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            current_rebased_full.as_ptr(),
            current_rebased_full_written as i64,
            current_stage_needle.as_ptr(),
            current_stage_needle.len() as i64,
        )
    };

    let temp_rebased_relative_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            temp_directory.as_ptr(),
            temp_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_variant_file_name_ptr,
            selected_variant_file_name_written as i64,
        )
    };
    let mut temp_rebased_relative = vec![0u8; temp_rebased_relative_len as usize];
    let temp_rebased_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            temp_directory.as_ptr(),
            temp_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_variant_file_name_ptr,
            selected_variant_file_name_written as i64,
            temp_rebased_relative.as_mut_ptr(),
            temp_rebased_relative.len() as i64,
        )
    };

    let temp_rebased_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            temp_rebased_relative.as_ptr(),
            temp_rebased_relative_written as i64,
            temp_root.as_ptr(),
            temp_root_written as i64,
        )
    };
    let mut temp_rebased_full = vec![0u8; temp_rebased_full_len as usize];
    let temp_rebased_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            temp_rebased_relative.as_ptr(),
            temp_rebased_relative_written as i64,
            temp_root.as_ptr(),
            temp_root_written as i64,
            temp_rebased_full.as_mut_ptr(),
            temp_rebased_full.len() as i64,
        )
    };

    let temp_stage_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            temp_rebased_full.as_ptr(),
            temp_rebased_full_written as i64,
            temp_stage_needle.as_ptr(),
            temp_stage_needle.len() as i64,
        )
    };
    let temp_stage_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            temp_rebased_full.as_ptr(),
            temp_rebased_full_written as i64,
            temp_stage_needle.as_ptr(),
            temp_stage_needle.len() as i64,
        )
    };

    let documents_rebased_relative_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            documents_directory.as_ptr(),
            documents_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_variant_file_name_ptr,
            selected_variant_file_name_written as i64,
        )
    };
    let mut documents_rebased_relative = vec![0u8; documents_rebased_relative_len as usize];
    let documents_rebased_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            documents_directory.as_ptr(),
            documents_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_variant_file_name_ptr,
            selected_variant_file_name_written as i64,
            documents_rebased_relative.as_mut_ptr(),
            documents_rebased_relative.len() as i64,
        )
    };

    let documents_rebased_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            documents_rebased_relative.as_ptr(),
            documents_rebased_relative_written as i64,
            documents_root.as_ptr(),
            documents_root_written as i64,
        )
    };
    let mut documents_rebased_full = vec![0u8; documents_rebased_full_len as usize];
    let documents_rebased_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            documents_rebased_relative.as_ptr(),
            documents_rebased_relative_written as i64,
            documents_root.as_ptr(),
            documents_root_written as i64,
            documents_rebased_full.as_mut_ptr(),
            documents_rebased_full.len() as i64,
        )
    };

    let documents_stage_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            documents_rebased_full.as_ptr(),
            documents_rebased_full_written as i64,
            documents_stage_needle.as_ptr(),
            documents_stage_needle.len() as i64,
        )
    };
    let documents_stage_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            documents_rebased_full.as_ptr(),
            documents_rebased_full_written as i64,
            documents_stage_needle.as_ptr(),
            documents_stage_needle.len() as i64,
        )
    };

    let mut selected_rebase_index = 0i32;
    let mut best_rebase_score = i32::MIN;
    let mut rebase_index = 0i32;
    while rebase_index < 3 {
        let mut candidate_rebase_score = current_rebased_full_written * 10
            + current_stage_contains * 100
            + current_stage_index;
        if rebase_index == 1 {
            candidate_rebase_score = temp_rebased_full_written * 10
                + temp_stage_contains * 100
                + temp_stage_index;
        } else if rebase_index == 2 {
            candidate_rebase_score = documents_rebased_full_written * 10
                + documents_stage_contains * 100
                + documents_stage_index;
        }

        let mut rebase_bonus = 0i32;
        if rebase_index == selected_directory_index {
            rebase_bonus += 25;
        }
        if rebase_index == selected_variant_index {
            rebase_bonus += 15;
        }
        if rebase_index == 0 && current_stage_contains != 0 {
            rebase_bonus += 10;
        }
        if rebase_index == 1 && temp_stage_contains != 0 {
            rebase_bonus += 20;
        }
        if rebase_index == 2 && documents_stage_contains != 0 {
            rebase_bonus += 30;
        }

        let rebase_score = candidate_rebase_score + rebase_bonus;
        if rebase_score > best_rebase_score {
            best_rebase_score = rebase_score;
            selected_rebase_index = rebase_index;
        }

        rebase_index += 1;
    }

    let mut selected_full_path_ptr = current_rebased_full.as_ptr();
    let mut selected_full_path_written = current_rebased_full_written;
    if selected_rebase_index == 1 {
        selected_full_path_ptr = temp_rebased_full.as_ptr();
        selected_full_path_written = temp_rebased_full_written;
    } else if selected_rebase_index == 2 {
        selected_full_path_ptr = documents_rebased_full.as_ptr();
        selected_full_path_written = documents_rebased_full_written;
    }

    let selected_leaf_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_full_path_ptr,
            selected_full_path_written as i64,
        )
    };
    let mut selected_final_leaf = vec![0u8; selected_leaf_len as usize];
    let selected_final_leaf_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_full_path_ptr,
            selected_full_path_written as i64,
            selected_final_leaf.as_mut_ptr(),
            selected_final_leaf.len() as i64,
        )
    };

    let mesh_leaf_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_final_leaf.as_ptr(),
            selected_final_leaf_written as i64,
            mesh_old.as_ptr(),
            mesh_old.len() as i64,
            mesh_new.as_ptr(),
            mesh_new.len() as i64,
        )
    };
    let mut mesh_leaf = vec![0u8; mesh_leaf_len as usize];
    let mesh_leaf_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_final_leaf.as_ptr(),
            selected_final_leaf_written as i64,
            mesh_old.as_ptr(),
            mesh_old.len() as i64,
            mesh_new.as_ptr(),
            mesh_new.len() as i64,
            mesh_leaf.as_mut_ptr(),
            mesh_leaf.len() as i64,
        )
    };
    let mesh_leaf_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            mesh_leaf.as_ptr(),
            mesh_leaf_written as i64,
            mesh_needle.as_ptr(),
            mesh_needle.len() as i64,
        )
    };

    let route_leaf_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_final_leaf.as_ptr(),
            selected_final_leaf_written as i64,
            route_old.as_ptr(),
            route_old.len() as i64,
            route_new.as_ptr(),
            route_new.len() as i64,
        )
    };
    let mut route_leaf = vec![0u8; route_leaf_len as usize];
    let route_leaf_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_final_leaf.as_ptr(),
            selected_final_leaf_written as i64,
            route_old.as_ptr(),
            route_old.len() as i64,
            route_new.as_ptr(),
            route_new.len() as i64,
            route_leaf.as_mut_ptr(),
            route_leaf.len() as i64,
        )
    };
    let route_leaf_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            route_leaf.as_ptr(),
            route_leaf_written as i64,
            route_needle.as_ptr(),
            route_needle.len() as i64,
        )
    };

    let array_leaf_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_final_leaf.as_ptr(),
            selected_final_leaf_written as i64,
            array_old.as_ptr(),
            array_old.len() as i64,
            array_new.as_ptr(),
            array_new.len() as i64,
        )
    };
    let mut array_leaf = vec![0u8; array_leaf_len as usize];
    let array_leaf_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_final_leaf.as_ptr(),
            selected_final_leaf_written as i64,
            array_old.as_ptr(),
            array_old.len() as i64,
            array_new.as_ptr(),
            array_new.len() as i64,
            array_leaf.as_mut_ptr(),
            array_leaf.len() as i64,
        )
    };
    let array_leaf_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            array_leaf.as_ptr(),
            array_leaf_written as i64,
            array_needle.as_ptr(),
            array_needle.len() as i64,
        )
    };

    let mut selected_leaf_transform_index = 0i32;
    let mut best_leaf_transform_score = i32::MIN;
    let mut leaf_transform_index = 0i32;
    while leaf_transform_index < 3 {
        let mut candidate_leaf_transform_score = mesh_leaf_written * 10 + mesh_leaf_contains * 50;
        if leaf_transform_index == 1 {
            candidate_leaf_transform_score = route_leaf_written * 10 + route_leaf_index;
        } else if leaf_transform_index == 2 {
            candidate_leaf_transform_score = array_leaf_written * 10 + array_leaf_contains * 50;
        }

        let mut leaf_transform_bonus = 0i32;
        if leaf_transform_index == selected_rebase_index {
            leaf_transform_bonus += 25;
        }
        if leaf_transform_index == selected_variant_index {
            leaf_transform_bonus += 15;
        }
        if leaf_transform_index == 0 && mesh_leaf_contains != 0 {
            leaf_transform_bonus += 20;
        }
        if leaf_transform_index == 1 && route_leaf_index >= 0 {
            leaf_transform_bonus += 10;
        }
        if leaf_transform_index == 2 && array_leaf_contains != 0 {
            leaf_transform_bonus += 5;
        }

        let leaf_transform_score = candidate_leaf_transform_score + leaf_transform_bonus;
        if leaf_transform_score > best_leaf_transform_score {
            best_leaf_transform_score = leaf_transform_score;
            selected_leaf_transform_index = leaf_transform_index;
        }

        leaf_transform_index += 1;
    }

    let mut selected_leaf_transform_ptr = mesh_leaf.as_ptr();
    let mut selected_leaf_transform_written = mesh_leaf_written;
    if selected_leaf_transform_index == 1 {
        selected_leaf_transform_ptr = route_leaf.as_ptr();
        selected_leaf_transform_written = route_leaf_written;
    } else if selected_leaf_transform_index == 2 {
        selected_leaf_transform_ptr = array_leaf.as_ptr();
        selected_leaf_transform_written = array_leaf_written;
    }

    let spine_path_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_full_path_ptr,
            selected_full_path_written as i64,
            spine_old.as_ptr(),
            spine_old.len() as i64,
            spine_new.as_ptr(),
            spine_new.len() as i64,
        )
    };
    let mut spine_path = vec![0u8; spine_path_len as usize];
    let spine_path_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_full_path_ptr,
            selected_full_path_written as i64,
            spine_old.as_ptr(),
            spine_old.len() as i64,
            spine_new.as_ptr(),
            spine_new.len() as i64,
            spine_path.as_mut_ptr(),
            spine_path.len() as i64,
        )
    };
    let spine_path_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            spine_path.as_ptr(),
            spine_path_written as i64,
            spine_needle.as_ptr(),
            spine_needle.len() as i64,
        )
    };

    let weave_path_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_full_path_ptr,
            selected_full_path_written as i64,
            weave_old.as_ptr(),
            weave_old.len() as i64,
            weave_new.as_ptr(),
            weave_new.len() as i64,
        )
    };
    let mut weave_path = vec![0u8; weave_path_len as usize];
    let weave_path_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_full_path_ptr,
            selected_full_path_written as i64,
            weave_old.as_ptr(),
            weave_old.len() as i64,
            weave_new.as_ptr(),
            weave_new.len() as i64,
            weave_path.as_mut_ptr(),
            weave_path.len() as i64,
        )
    };
    let weave_path_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            weave_path.as_ptr(),
            weave_path_written as i64,
            weave_needle.as_ptr(),
            weave_needle.len() as i64,
        )
    };

    let orbit_path_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_full_path_ptr,
            selected_full_path_written as i64,
            orbit_old.as_ptr(),
            orbit_old.len() as i64,
            orbit_new.as_ptr(),
            orbit_new.len() as i64,
        )
    };
    let mut orbit_path = vec![0u8; orbit_path_len as usize];
    let orbit_path_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_full_path_ptr,
            selected_full_path_written as i64,
            orbit_old.as_ptr(),
            orbit_old.len() as i64,
            orbit_new.as_ptr(),
            orbit_new.len() as i64,
            orbit_path.as_mut_ptr(),
            orbit_path.len() as i64,
        )
    };
    let orbit_path_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            orbit_path.as_ptr(),
            orbit_path_written as i64,
            orbit_needle.as_ptr(),
            orbit_needle.len() as i64,
        )
    };

    let mut selected_path_transform_index = 0i32;
    let mut best_path_transform_score = i32::MIN;
    let mut path_transform_index = 0i32;
    while path_transform_index < 3 {
        let mut candidate_path_transform_score = spine_path_written * 10 + spine_path_contains * 50;
        if path_transform_index == 1 {
            candidate_path_transform_score = weave_path_written * 10 + weave_path_index;
        } else if path_transform_index == 2 {
            candidate_path_transform_score = orbit_path_written * 10 + orbit_path_contains * 50;
        }

        let mut path_transform_bonus = 0i32;
        if path_transform_index == selected_rebase_index {
            path_transform_bonus += 25;
        }
        if path_transform_index == selected_leaf_transform_index {
            path_transform_bonus += 15;
        }
        if path_transform_index == 0 && spine_path_contains != 0 {
            path_transform_bonus += 20;
        }
        if path_transform_index == 1 && weave_path_index >= 0 {
            path_transform_bonus += 10;
        }
        if path_transform_index == 2 && orbit_path_contains != 0 {
            path_transform_bonus += 5;
        }

        let path_transform_score = candidate_path_transform_score + path_transform_bonus;
        if path_transform_score > best_path_transform_score {
            best_path_transform_score = path_transform_score;
            selected_path_transform_index = path_transform_index;
        }

        path_transform_index += 1;
    }

    let mut selected_path_transform_ptr = spine_path.as_ptr();
    let mut selected_path_transform_written = spine_path_written;
    if selected_path_transform_index == 1 {
        selected_path_transform_ptr = weave_path.as_ptr();
        selected_path_transform_written = weave_path_written;
    } else if selected_path_transform_index == 2 {
        selected_path_transform_ptr = orbit_path.as_ptr();
        selected_path_transform_written = orbit_path_written;
    }

    let selected_path_directory_len = unsafe {
        rust_mcil_dotnet_path_get_directory_name_utf8_len(
            selected_path_transform_ptr,
            selected_path_transform_written as i64,
        )
    };
    let mut selected_path_directory = vec![0u8; selected_path_directory_len as usize];
    let selected_path_directory_written = unsafe {
        rust_mcil_dotnet_path_copy_directory_name_utf8(
            selected_path_transform_ptr,
            selected_path_transform_written as i64,
            selected_path_directory.as_mut_ptr(),
            selected_path_directory.len() as i64,
        )
    };

    let selected_path_file_name_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_utf8_len(
            selected_path_transform_ptr,
            selected_path_transform_written as i64,
        )
    };
    let mut selected_path_file_name = vec![0u8; selected_path_file_name_len as usize];
    let selected_path_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_utf8(
            selected_path_transform_ptr,
            selected_path_transform_written as i64,
            selected_path_file_name.as_mut_ptr(),
            selected_path_file_name.len() as i64,
        )
    };

    let self_recomposed_relative_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            selected_path_directory.as_ptr(),
            selected_path_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_path_file_name.as_ptr(),
            selected_path_file_name_written as i64,
        )
    };
    let mut self_recomposed_relative = vec![0u8; self_recomposed_relative_len as usize];
    let self_recomposed_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            selected_path_directory.as_ptr(),
            selected_path_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_path_file_name.as_ptr(),
            selected_path_file_name_written as i64,
            self_recomposed_relative.as_mut_ptr(),
            self_recomposed_relative.len() as i64,
        )
    };
    let self_recomposed_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            self_recomposed_relative.as_ptr(),
            self_recomposed_relative_written as i64,
            current_root.as_ptr(),
            current_root_written as i64,
        )
    };
    let mut self_recomposed_full = vec![0u8; self_recomposed_full_len as usize];
    let self_recomposed_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            self_recomposed_relative.as_ptr(),
            self_recomposed_relative_written as i64,
            current_root.as_ptr(),
            current_root_written as i64,
            self_recomposed_full.as_mut_ptr(),
            self_recomposed_full.len() as i64,
        )
    };
    let self_recomposed_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            self_recomposed_full.as_ptr(),
            self_recomposed_full_written as i64,
            self_stage_needle.as_ptr(),
            self_stage_needle.len() as i64,
        )
    };

    let temp_recomposed_relative_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            temp_directory.as_ptr(),
            temp_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_path_file_name.as_ptr(),
            selected_path_file_name_written as i64,
        )
    };
    let mut temp_recomposed_relative = vec![0u8; temp_recomposed_relative_len as usize];
    let temp_recomposed_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            temp_directory.as_ptr(),
            temp_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_path_file_name.as_ptr(),
            selected_path_file_name_written as i64,
            temp_recomposed_relative.as_mut_ptr(),
            temp_recomposed_relative.len() as i64,
        )
    };
    let temp_recomposed_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            temp_recomposed_relative.as_ptr(),
            temp_recomposed_relative_written as i64,
            temp_root.as_ptr(),
            temp_root_written as i64,
        )
    };
    let mut temp_recomposed_full = vec![0u8; temp_recomposed_full_len as usize];
    let temp_recomposed_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            temp_recomposed_relative.as_ptr(),
            temp_recomposed_relative_written as i64,
            temp_root.as_ptr(),
            temp_root_written as i64,
            temp_recomposed_full.as_mut_ptr(),
            temp_recomposed_full.len() as i64,
        )
    };
    let temp_recomposed_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            temp_recomposed_full.as_ptr(),
            temp_recomposed_full_written as i64,
            temp_stage_six_needle.as_ptr(),
            temp_stage_six_needle.len() as i64,
        )
    };

    let documents_recomposed_relative_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            documents_directory.as_ptr(),
            documents_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_path_file_name.as_ptr(),
            selected_path_file_name_written as i64,
        )
    };
    let mut documents_recomposed_relative = vec![0u8; documents_recomposed_relative_len as usize];
    let documents_recomposed_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            documents_directory.as_ptr(),
            documents_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_path_file_name.as_ptr(),
            selected_path_file_name_written as i64,
            documents_recomposed_relative.as_mut_ptr(),
            documents_recomposed_relative.len() as i64,
        )
    };
    let documents_recomposed_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            documents_recomposed_relative.as_ptr(),
            documents_recomposed_relative_written as i64,
            documents_root.as_ptr(),
            documents_root_written as i64,
        )
    };
    let mut documents_recomposed_full = vec![0u8; documents_recomposed_full_len as usize];
    let documents_recomposed_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            documents_recomposed_relative.as_ptr(),
            documents_recomposed_relative_written as i64,
            documents_root.as_ptr(),
            documents_root_written as i64,
            documents_recomposed_full.as_mut_ptr(),
            documents_recomposed_full.len() as i64,
        )
    };
    let documents_recomposed_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            documents_recomposed_full.as_ptr(),
            documents_recomposed_full_written as i64,
            documents_stage_six_needle.as_ptr(),
            documents_stage_six_needle.len() as i64,
        )
    };

    let mut selected_recomposition_index = 0i32;
    let mut best_recomposition_score = i32::MIN;
    let mut recomposition_index = 0i32;
    while recomposition_index < 3 {
        let mut candidate_recomposition_score = self_recomposed_full_written * 10 + self_recomposed_contains * 50;
        if recomposition_index == 1 {
            candidate_recomposition_score = temp_recomposed_full_written * 10 + temp_recomposed_index;
        } else if recomposition_index == 2 {
            candidate_recomposition_score = documents_recomposed_full_written * 10 + documents_recomposed_contains * 50;
        }

        let mut recomposition_bonus = 0i32;
        if recomposition_index == selected_rebase_index {
            recomposition_bonus += 25;
        }
        if recomposition_index == selected_path_transform_index {
            recomposition_bonus += 15;
        }
        if recomposition_index == 0 && self_recomposed_contains != 0 {
            recomposition_bonus += 20;
        }
        if recomposition_index == 1 && temp_recomposed_index >= 0 {
            recomposition_bonus += 10;
        }
        if recomposition_index == 2 && documents_recomposed_contains != 0 {
            recomposition_bonus += 5;
        }

        let recomposition_score = candidate_recomposition_score + recomposition_bonus;
        if recomposition_score > best_recomposition_score {
            best_recomposition_score = recomposition_score;
            selected_recomposition_index = recomposition_index;
        }

        recomposition_index += 1;
    }

    let mut selected_recomposition_ptr = self_recomposed_full.as_ptr();
    let mut selected_recomposition_written = self_recomposed_full_written;
    if selected_recomposition_index == 1 {
        selected_recomposition_ptr = temp_recomposed_full.as_ptr();
        selected_recomposition_written = temp_recomposed_full_written;
    } else if selected_recomposition_index == 2 {
        selected_recomposition_ptr = documents_recomposed_full.as_ptr();
        selected_recomposition_written = documents_recomposed_full_written;
    }

    let final_transformed_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_recomposition_ptr,
            selected_recomposition_written as i64,
            final_old.as_ptr(),
            final_old.len() as i64,
            final_new.as_ptr(),
            final_new.len() as i64,
        )
    };
    let mut final_transformed = vec![0u8; final_transformed_len as usize];
    let final_transformed_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_recomposition_ptr,
            selected_recomposition_written as i64,
            final_old.as_ptr(),
            final_old.len() as i64,
            final_new.as_ptr(),
            final_new.len() as i64,
            final_transformed.as_mut_ptr(),
            final_transformed.len() as i64,
        )
    };
    let final_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            final_transformed.as_ptr(),
            final_transformed_written as i64,
            final_needle.as_ptr(),
            final_needle.len() as i64,
        )
    };
    let final_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            final_transformed.as_ptr(),
            final_transformed_written as i64,
            final_needle.as_ptr(),
            final_needle.len() as i64,
        )
    };

    let crown_path_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            final_transformed.as_ptr(),
            final_transformed_written as i64,
            crown_old.as_ptr(),
            crown_old.len() as i64,
            crown_new.as_ptr(),
            crown_new.len() as i64,
        )
    };
    let mut crown_path = vec![0u8; crown_path_len as usize];
    let crown_path_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            final_transformed.as_ptr(),
            final_transformed_written as i64,
            crown_old.as_ptr(),
            crown_old.len() as i64,
            crown_new.as_ptr(),
            crown_new.len() as i64,
            crown_path.as_mut_ptr(),
            crown_path.len() as i64,
        )
    };
    let crown_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            crown_path.as_ptr(),
            crown_path_written as i64,
            crown_needle.as_ptr(),
            crown_needle.len() as i64,
        )
    };

    let pulse_path_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_recomposition_ptr,
            selected_recomposition_written as i64,
            pulse_extension.as_ptr(),
            pulse_extension.len() as i64,
        )
    };
    let mut pulse_path = vec![0u8; pulse_path_len as usize];
    let pulse_path_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_recomposition_ptr,
            selected_recomposition_written as i64,
            pulse_extension.as_ptr(),
            pulse_extension.len() as i64,
            pulse_path.as_mut_ptr(),
            pulse_path.len() as i64,
        )
    };
    let pulse_file_name_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_utf8_len(
            pulse_path.as_ptr(),
            pulse_path_written as i64,
        )
    };
    let mut pulse_file_name = vec![0u8; pulse_file_name_len as usize];
    let pulse_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_utf8(
            pulse_path.as_ptr(),
            pulse_path_written as i64,
            pulse_file_name.as_mut_ptr(),
            pulse_file_name.len() as i64,
        )
    };
    let pulse_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            pulse_file_name.as_ptr(),
            pulse_file_name_written as i64,
            pulse_needle.as_ptr(),
            pulse_needle.len() as i64,
        )
    };

    let anchor_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            final_transformed.as_ptr(),
            final_transformed_written as i64,
        )
    };
    let mut anchor_source = vec![0u8; anchor_source_len as usize];
    let anchor_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            final_transformed.as_ptr(),
            final_transformed_written as i64,
            anchor_source.as_mut_ptr(),
            anchor_source.len() as i64,
        )
    };
    let anchor_leaf_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            anchor_source.as_ptr(),
            anchor_source_written as i64,
            anchor_old.as_ptr(),
            anchor_old.len() as i64,
            anchor_new.as_ptr(),
            anchor_new.len() as i64,
        )
    };
    let mut anchor_leaf = vec![0u8; anchor_leaf_len as usize];
    let anchor_leaf_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            anchor_source.as_ptr(),
            anchor_source_written as i64,
            anchor_old.as_ptr(),
            anchor_old.len() as i64,
            anchor_new.as_ptr(),
            anchor_new.len() as i64,
            anchor_leaf.as_mut_ptr(),
            anchor_leaf.len() as i64,
        )
    };
    let anchor_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            anchor_leaf.as_ptr(),
            anchor_leaf_written as i64,
            anchor_needle.as_ptr(),
            anchor_needle.len() as i64,
        )
    };

    let mut selected_closure_index = 0i32;
    let mut best_closure_score = i32::MIN;
    let mut closure_index = 0i32;
    while closure_index < 3 {
        let mut candidate_closure_score = crown_path_written * 10 + crown_contains * 50;
        if closure_index == 1 {
            candidate_closure_score = pulse_file_name_written * 10 + pulse_index;
        } else if closure_index == 2 {
            candidate_closure_score = anchor_leaf_written * 10 + anchor_contains * 50;
        }

        let mut closure_bonus = 0i32;
        if closure_index == selected_recomposition_index {
            closure_bonus += 25;
        }
        if closure_index == selected_path_transform_index {
            closure_bonus += 15;
        }
        if closure_index == selected_variant_index {
            closure_bonus += 5;
        }
        if closure_index == 0 && crown_contains != 0 {
            closure_bonus += 20;
        }
        if closure_index == 1 && pulse_index >= 0 {
            closure_bonus += 10;
        }
        if closure_index == 2 && anchor_contains != 0 {
            closure_bonus += 30;
        }

        let closure_score = candidate_closure_score + closure_bonus;
        if closure_score > best_closure_score {
            best_closure_score = closure_score;
            selected_closure_index = closure_index;
        }

        closure_index += 1;
    }

    let mut selected_closure_ptr = crown_path.as_ptr();
    let mut selected_closure_written = crown_path_written;
    if selected_closure_index == 1 {
        selected_closure_ptr = pulse_file_name.as_ptr();
        selected_closure_written = pulse_file_name_written;
    } else if selected_closure_index == 2 {
        selected_closure_ptr = anchor_leaf.as_ptr();
        selected_closure_written = anchor_leaf_written;
    }

    let crest_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_closure_ptr,
            selected_closure_written as i64,
            crest_old.as_ptr(),
            crest_old.len() as i64,
            crest_new.as_ptr(),
            crest_new.len() as i64,
        )
    };
    let mut crest_value = vec![0u8; crest_value_len as usize];
    let crest_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_closure_ptr,
            selected_closure_written as i64,
            crest_old.as_ptr(),
            crest_old.len() as i64,
            crest_new.as_ptr(),
            crest_new.len() as i64,
            crest_value.as_mut_ptr(),
            crest_value.len() as i64,
        )
    };
    let crest_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            crest_value.as_ptr(),
            crest_value_written as i64,
            crest_needle.as_ptr(),
            crest_needle.len() as i64,
        )
    };

    let flare_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_closure_ptr,
            selected_closure_written as i64,
            flare_extension.as_ptr(),
            flare_extension.len() as i64,
        )
    };
    let mut flare_value = vec![0u8; flare_value_len as usize];
    let flare_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_closure_ptr,
            selected_closure_written as i64,
            flare_extension.as_ptr(),
            flare_extension.len() as i64,
            flare_value.as_mut_ptr(),
            flare_value.len() as i64,
        )
    };
    let flare_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            flare_value.as_ptr(),
            flare_value_written as i64,
            flare_needle.as_ptr(),
            flare_needle.len() as i64,
        )
    };

    let lattice_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_closure_ptr,
            selected_closure_written as i64,
        )
    };
    let mut lattice_source = vec![0u8; lattice_source_len as usize];
    let lattice_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_closure_ptr,
            selected_closure_written as i64,
            lattice_source.as_mut_ptr(),
            lattice_source.len() as i64,
        )
    };
    let lattice_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            lattice_source.as_ptr(),
            lattice_source_written as i64,
            lattice_old.as_ptr(),
            lattice_old.len() as i64,
            lattice_new.as_ptr(),
            lattice_new.len() as i64,
        )
    };
    let mut lattice_value = vec![0u8; lattice_value_len as usize];
    let lattice_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            lattice_source.as_ptr(),
            lattice_source_written as i64,
            lattice_old.as_ptr(),
            lattice_old.len() as i64,
            lattice_new.as_ptr(),
            lattice_new.len() as i64,
            lattice_value.as_mut_ptr(),
            lattice_value.len() as i64,
        )
    };
    let lattice_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            lattice_value.as_ptr(),
            lattice_value_written as i64,
            lattice_needle.as_ptr(),
            lattice_needle.len() as i64,
        )
    };

    let mut selected_apex_index = 0i32;
    let mut best_apex_score = i32::MIN;
    let mut apex_index = 0i32;
    while apex_index < 3 {
        let mut candidate_apex_score = crest_value_written * 10 + crest_contains * 50;
        if apex_index == 1 {
            candidate_apex_score = flare_value_written * 10 + flare_index;
        } else if apex_index == 2 {
            candidate_apex_score = lattice_value_written * 10 + lattice_contains * 50;
        }

        let mut apex_bonus = 0i32;
        if apex_index == selected_closure_index {
            apex_bonus += 25;
        }
        if apex_index == selected_recomposition_index {
            apex_bonus += 15;
        }
        if apex_index == selected_path_transform_index {
            apex_bonus += 5;
        }
        if apex_index == 0 && crest_contains != 0 {
            apex_bonus += 20;
        }
        if apex_index == 1 && flare_index >= 0 {
            apex_bonus += 10;
        }
        if apex_index == 2 && lattice_contains != 0 {
            apex_bonus += 30;
        }

        let apex_score = candidate_apex_score + apex_bonus;
        if apex_score > best_apex_score {
            best_apex_score = apex_score;
            selected_apex_index = apex_index;
        }

        apex_index += 1;
    }

    let mut selected_apex_ptr = crest_value.as_ptr();
    let mut selected_apex_written = crest_value_written;
    if selected_apex_index == 1 {
        selected_apex_ptr = flare_value.as_ptr();
        selected_apex_written = flare_value_written;
    } else if selected_apex_index == 2 {
        selected_apex_ptr = lattice_value.as_ptr();
        selected_apex_written = lattice_value_written;
    }

    let summit_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_apex_ptr,
            selected_apex_written as i64,
            summit_old.as_ptr(),
            summit_old.len() as i64,
            summit_new.as_ptr(),
            summit_new.len() as i64,
        )
    };
    let mut summit_value = vec![0u8; summit_value_len as usize];
    let summit_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_apex_ptr,
            selected_apex_written as i64,
            summit_old.as_ptr(),
            summit_old.len() as i64,
            summit_new.as_ptr(),
            summit_new.len() as i64,
            summit_value.as_mut_ptr(),
            summit_value.len() as i64,
        )
    };
    let summit_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            summit_value.as_ptr(),
            summit_value_written as i64,
            summit_needle.as_ptr(),
            summit_needle.len() as i64,
        )
    };

    let prism_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_apex_ptr,
            selected_apex_written as i64,
            prism_extension.as_ptr(),
            prism_extension.len() as i64,
        )
    };
    let mut prism_value = vec![0u8; prism_value_len as usize];
    let prism_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_apex_ptr,
            selected_apex_written as i64,
            prism_extension.as_ptr(),
            prism_extension.len() as i64,
            prism_value.as_mut_ptr(),
            prism_value.len() as i64,
        )
    };
    let prism_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            prism_value.as_ptr(),
            prism_value_written as i64,
            prism_needle.as_ptr(),
            prism_needle.len() as i64,
        )
    };

    let vault_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_apex_ptr,
            selected_apex_written as i64,
        )
    };
    let mut vault_source = vec![0u8; vault_source_len as usize];
    let vault_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_apex_ptr,
            selected_apex_written as i64,
            vault_source.as_mut_ptr(),
            vault_source.len() as i64,
        )
    };
    let vault_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            vault_source.as_ptr(),
            vault_source_written as i64,
            vault_old.as_ptr(),
            vault_old.len() as i64,
            vault_new.as_ptr(),
            vault_new.len() as i64,
        )
    };
    let mut vault_value = vec![0u8; vault_value_len as usize];
    let vault_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            vault_source.as_ptr(),
            vault_source_written as i64,
            vault_old.as_ptr(),
            vault_old.len() as i64,
            vault_new.as_ptr(),
            vault_new.len() as i64,
            vault_value.as_mut_ptr(),
            vault_value.len() as i64,
        )
    };
    let vault_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            vault_value.as_ptr(),
            vault_value_written as i64,
            vault_needle.as_ptr(),
            vault_needle.len() as i64,
        )
    };

    let mut selected_summit_index = 0i32;
    let mut best_summit_score = i32::MIN;
    let mut summit_index = 0i32;
    while summit_index < 3 {
        let mut candidate_summit_score = summit_value_written * 10 + summit_contains * 50;
        if summit_index == 1 {
            candidate_summit_score = prism_value_written * 10 + prism_index;
        } else if summit_index == 2 {
            candidate_summit_score = vault_value_written * 10 + vault_contains * 50;
        }

        let mut summit_bonus = 0i32;
        if summit_index == selected_apex_index {
            summit_bonus += 25;
        }
        if summit_index == selected_closure_index {
            summit_bonus += 15;
        }
        if summit_index == selected_recomposition_index {
            summit_bonus += 5;
        }
        if summit_index == 0 && summit_contains != 0 {
            summit_bonus += 20;
        }
        if summit_index == 1 && prism_index >= 0 {
            summit_bonus += 10;
        }
        if summit_index == 2 && vault_contains != 0 {
            summit_bonus += 30;
        }

        let summit_score = candidate_summit_score + summit_bonus;
        if summit_score > best_summit_score {
            best_summit_score = summit_score;
            selected_summit_index = summit_index;
        }

        summit_index += 1;
    }

    let mut selected_summit_ptr = summit_value.as_ptr();
    let mut selected_summit_written = summit_value_written;
    if selected_summit_index == 1 {
        selected_summit_ptr = prism_value.as_ptr();
        selected_summit_written = prism_value_written;
    } else if selected_summit_index == 2 {
        selected_summit_ptr = vault_value.as_ptr();
        selected_summit_written = vault_value_written;
    }

    let beacon_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_summit_ptr,
            selected_summit_written as i64,
            beacon_old.as_ptr(),
            beacon_old.len() as i64,
            beacon_new.as_ptr(),
            beacon_new.len() as i64,
        )
    };
    let mut beacon_value = vec![0u8; beacon_value_len as usize];
    let beacon_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_summit_ptr,
            selected_summit_written as i64,
            beacon_old.as_ptr(),
            beacon_old.len() as i64,
            beacon_new.as_ptr(),
            beacon_new.len() as i64,
            beacon_value.as_mut_ptr(),
            beacon_value.len() as i64,
        )
    };
    let beacon_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            beacon_value.as_ptr(),
            beacon_value_written as i64,
            beacon_needle.as_ptr(),
            beacon_needle.len() as i64,
        )
    };

    let ember_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_summit_ptr,
            selected_summit_written as i64,
            ember_extension.as_ptr(),
            ember_extension.len() as i64,
        )
    };
    let mut ember_value = vec![0u8; ember_value_len as usize];
    let ember_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_summit_ptr,
            selected_summit_written as i64,
            ember_extension.as_ptr(),
            ember_extension.len() as i64,
            ember_value.as_mut_ptr(),
            ember_value.len() as i64,
        )
    };
    let ember_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            ember_value.as_ptr(),
            ember_value_written as i64,
            ember_needle.as_ptr(),
            ember_needle.len() as i64,
        )
    };

    let harbor_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_summit_ptr,
            selected_summit_written as i64,
        )
    };
    let mut harbor_source = vec![0u8; harbor_source_len as usize];
    let harbor_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_summit_ptr,
            selected_summit_written as i64,
            harbor_source.as_mut_ptr(),
            harbor_source.len() as i64,
        )
    };
    let harbor_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            harbor_source.as_ptr(),
            harbor_source_written as i64,
            harbor_old.as_ptr(),
            harbor_old.len() as i64,
            harbor_new.as_ptr(),
            harbor_new.len() as i64,
        )
    };
    let mut harbor_value = vec![0u8; harbor_value_len as usize];
    let harbor_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            harbor_source.as_ptr(),
            harbor_source_written as i64,
            harbor_old.as_ptr(),
            harbor_old.len() as i64,
            harbor_new.as_ptr(),
            harbor_new.len() as i64,
            harbor_value.as_mut_ptr(),
            harbor_value.len() as i64,
        )
    };
    let harbor_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            harbor_value.as_ptr(),
            harbor_value_written as i64,
            harbor_needle.as_ptr(),
            harbor_needle.len() as i64,
        )
    };

    let mut selected_terminal_index = 0i32;
    let mut best_terminal_score = i32::MIN;
    let mut terminal_index = 0i32;
    while terminal_index < 3 {
        let mut candidate_terminal_score = beacon_value_written * 10 + beacon_contains * 50;
        if terminal_index == 1 {
            candidate_terminal_score = ember_value_written * 10 + ember_index;
        } else if terminal_index == 2 {
            candidate_terminal_score = harbor_value_written * 10 + harbor_contains * 50;
        }

        let mut terminal_bonus = 0i32;
        if terminal_index == selected_summit_index {
            terminal_bonus += 25;
        }
        if terminal_index == selected_apex_index {
            terminal_bonus += 15;
        }
        if terminal_index == selected_closure_index {
            terminal_bonus += 5;
        }
        if terminal_index == 0 && beacon_contains != 0 {
            terminal_bonus += 20;
        }
        if terminal_index == 1 && ember_index >= 0 {
            terminal_bonus += 10;
        }
        if terminal_index == 2 && harbor_contains != 0 {
            terminal_bonus += 30;
        }

        let terminal_score = candidate_terminal_score + terminal_bonus;
        if terminal_score > best_terminal_score {
            best_terminal_score = terminal_score;
            selected_terminal_index = terminal_index;
        }

        terminal_index += 1;
    }

    let mut selected_terminal_ptr = beacon_value.as_ptr();
    let mut selected_terminal_written = beacon_value_written;
    if selected_terminal_index == 1 {
        selected_terminal_ptr = ember_value.as_ptr();
        selected_terminal_written = ember_value_written;
    } else if selected_terminal_index == 2 {
        selected_terminal_ptr = harbor_value.as_ptr();
        selected_terminal_written = harbor_value_written;
    }

    let ridge_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_terminal_ptr,
            selected_terminal_written as i64,
            ridge_old.as_ptr(),
            ridge_old.len() as i64,
            ridge_new.as_ptr(),
            ridge_new.len() as i64,
        )
    };
    let mut ridge_value = vec![0u8; ridge_value_len as usize];
    let ridge_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_terminal_ptr,
            selected_terminal_written as i64,
            ridge_old.as_ptr(),
            ridge_old.len() as i64,
            ridge_new.as_ptr(),
            ridge_new.len() as i64,
            ridge_value.as_mut_ptr(),
            ridge_value.len() as i64,
        )
    };
    let ridge_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            ridge_value.as_ptr(),
            ridge_value_written as i64,
            ridge_needle.as_ptr(),
            ridge_needle.len() as i64,
        )
    };

    let glow_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_terminal_ptr,
            selected_terminal_written as i64,
            glow_extension.as_ptr(),
            glow_extension.len() as i64,
        )
    };
    let mut glow_value = vec![0u8; glow_value_len as usize];
    let glow_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_terminal_ptr,
            selected_terminal_written as i64,
            glow_extension.as_ptr(),
            glow_extension.len() as i64,
            glow_value.as_mut_ptr(),
            glow_value.len() as i64,
        )
    };
    let glow_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            glow_value.as_ptr(),
            glow_value_written as i64,
            glow_needle.as_ptr(),
            glow_needle.len() as i64,
        )
    };

    let quay_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_terminal_ptr,
            selected_terminal_written as i64,
        )
    };
    let mut quay_source = vec![0u8; quay_source_len as usize];
    let quay_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_terminal_ptr,
            selected_terminal_written as i64,
            quay_source.as_mut_ptr(),
            quay_source.len() as i64,
        )
    };
    let quay_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            quay_source.as_ptr(),
            quay_source_written as i64,
            quay_old.as_ptr(),
            quay_old.len() as i64,
            quay_new.as_ptr(),
            quay_new.len() as i64,
        )
    };
    let mut quay_value = vec![0u8; quay_value_len as usize];
    let quay_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            quay_source.as_ptr(),
            quay_source_written as i64,
            quay_old.as_ptr(),
            quay_old.len() as i64,
            quay_new.as_ptr(),
            quay_new.len() as i64,
            quay_value.as_mut_ptr(),
            quay_value.len() as i64,
        )
    };
    let quay_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            quay_value.as_ptr(),
            quay_value_written as i64,
            quay_needle.as_ptr(),
            quay_needle.len() as i64,
        )
    };

    let mut selected_finale_index = 0i32;
    let mut best_finale_score = i32::MIN;
    let mut finale_index = 0i32;
    while finale_index < 3 {
        let mut candidate_finale_score = ridge_value_written * 10 + ridge_contains * 50;
        if finale_index == 1 {
            candidate_finale_score = glow_value_written * 10 + glow_index;
        } else if finale_index == 2 {
            candidate_finale_score = quay_value_written * 10 + quay_contains * 50;
        }

        let mut finale_bonus = 0i32;
        if finale_index == selected_terminal_index {
            finale_bonus += 25;
        }
        if finale_index == selected_summit_index {
            finale_bonus += 15;
        }
        if finale_index == selected_apex_index {
            finale_bonus += 5;
        }
        if finale_index == 0 && ridge_contains != 0 {
            finale_bonus += 20;
        }
        if finale_index == 1 && glow_index >= 0 {
            finale_bonus += 10;
        }
        if finale_index == 2 && quay_contains != 0 {
            finale_bonus += 30;
        }

        let finale_score = candidate_finale_score + finale_bonus;
        if finale_score > best_finale_score {
            best_finale_score = finale_score;
            selected_finale_index = finale_index;
        }

        finale_index += 1;
    }

    let mut selected_finale_ptr = ridge_value.as_ptr();
    let mut selected_finale_written = ridge_value_written;
    if selected_finale_index == 1 {
        selected_finale_ptr = glow_value.as_ptr();
        selected_finale_written = glow_value_written;
    } else if selected_finale_index == 2 {
        selected_finale_ptr = quay_value.as_ptr();
        selected_finale_written = quay_value_written;
    }

    let crestline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_finale_ptr,
            selected_finale_written as i64,
            crestline_old.as_ptr(),
            crestline_old.len() as i64,
            crestline_new.as_ptr(),
            crestline_new.len() as i64,
        )
    };
    let mut crestline_value = vec![0u8; crestline_value_len as usize];
    let crestline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_finale_ptr,
            selected_finale_written as i64,
            crestline_old.as_ptr(),
            crestline_old.len() as i64,
            crestline_new.as_ptr(),
            crestline_new.len() as i64,
            crestline_value.as_mut_ptr(),
            crestline_value.len() as i64,
        )
    };
    let crestline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            crestline_value.as_ptr(),
            crestline_value_written as i64,
            crestline_needle.as_ptr(),
            crestline_needle.len() as i64,
        )
    };

    let spark_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_finale_ptr,
            selected_finale_written as i64,
            spark_extension.as_ptr(),
            spark_extension.len() as i64,
        )
    };
    let mut spark_value = vec![0u8; spark_value_len as usize];
    let spark_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_finale_ptr,
            selected_finale_written as i64,
            spark_extension.as_ptr(),
            spark_extension.len() as i64,
            spark_value.as_mut_ptr(),
            spark_value.len() as i64,
        )
    };
    let spark_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            spark_value.as_ptr(),
            spark_value_written as i64,
            spark_needle.as_ptr(),
            spark_needle.len() as i64,
        )
    };

    let inlet_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_finale_ptr,
            selected_finale_written as i64,
        )
    };
    let mut inlet_source = vec![0u8; inlet_source_len as usize];
    let inlet_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_finale_ptr,
            selected_finale_written as i64,
            inlet_source.as_mut_ptr(),
            inlet_source.len() as i64,
        )
    };
    let inlet_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            inlet_source.as_ptr(),
            inlet_source_written as i64,
            inlet_old.as_ptr(),
            inlet_old.len() as i64,
            inlet_new.as_ptr(),
            inlet_new.len() as i64,
        )
    };
    let mut inlet_value = vec![0u8; inlet_value_len as usize];
    let inlet_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            inlet_source.as_ptr(),
            inlet_source_written as i64,
            inlet_old.as_ptr(),
            inlet_old.len() as i64,
            inlet_new.as_ptr(),
            inlet_new.len() as i64,
            inlet_value.as_mut_ptr(),
            inlet_value.len() as i64,
        )
    };
    let inlet_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            inlet_value.as_ptr(),
            inlet_value_written as i64,
            inlet_needle.as_ptr(),
            inlet_needle.len() as i64,
        )
    };

    let mut selected_codex_index = 0i32;
    let mut best_codex_score = i32::MIN;
    let mut codex_index = 0i32;
    while codex_index < 3 {
        let mut candidate_codex_score = crestline_value_written * 10 + crestline_contains * 50;
        if codex_index == 1 {
            candidate_codex_score = spark_value_written * 10 + spark_index;
        } else if codex_index == 2 {
            candidate_codex_score = inlet_value_written * 10 + inlet_contains * 50;
        }

        let mut codex_bonus = 0i32;
        if codex_index == selected_finale_index {
            codex_bonus += 25;
        }
        if codex_index == selected_terminal_index {
            codex_bonus += 15;
        }
        if codex_index == selected_summit_index {
            codex_bonus += 5;
        }
        if codex_index == 0 && crestline_contains != 0 {
            codex_bonus += 20;
        }
        if codex_index == 1 && spark_index >= 0 {
            codex_bonus += 10;
        }
        if codex_index == 2 && inlet_contains != 0 {
            codex_bonus += 30;
        }

        let codex_score = candidate_codex_score + codex_bonus;
        if codex_score > best_codex_score {
            best_codex_score = codex_score;
            selected_codex_index = codex_index;
        }

        codex_index += 1;
    }

    let mut selected_codex_ptr = crestline_value.as_ptr();
    let mut selected_codex_written = crestline_value_written;
    if selected_codex_index == 1 {
        selected_codex_ptr = spark_value.as_ptr();
        selected_codex_written = spark_value_written;
    } else if selected_codex_index == 2 {
        selected_codex_ptr = inlet_value.as_ptr();
        selected_codex_written = inlet_value_written;
    }

    let skyline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_codex_ptr,
            selected_codex_written as i64,
            skyline_old.as_ptr(),
            skyline_old.len() as i64,
            skyline_new.as_ptr(),
            skyline_new.len() as i64,
        )
    };
    let mut skyline_value = vec![0u8; skyline_value_len as usize];
    let skyline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_codex_ptr,
            selected_codex_written as i64,
            skyline_old.as_ptr(),
            skyline_old.len() as i64,
            skyline_new.as_ptr(),
            skyline_new.len() as i64,
            skyline_value.as_mut_ptr(),
            skyline_value.len() as i64,
        )
    };
    let skyline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            skyline_value.as_ptr(),
            skyline_value_written as i64,
            skyline_needle.as_ptr(),
            skyline_needle.len() as i64,
        )
    };

    let flare_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_codex_ptr,
            selected_codex_written as i64,
            flare_extension.as_ptr(),
            flare_extension.len() as i64,
        )
    };
    let mut flare_value_two = vec![0u8; flare_value_len as usize];
    let flare_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_codex_ptr,
            selected_codex_written as i64,
            flare_extension.as_ptr(),
            flare_extension.len() as i64,
            flare_value_two.as_mut_ptr(),
            flare_value_two.len() as i64,
        )
    };
    let flare_index_two = unsafe {
        rust_mcil_dotnet_string_index_of(
            flare_value_two.as_ptr(),
            flare_value_written as i64,
            flare_needle.as_ptr(),
            flare_needle.len() as i64,
        )
    };

    let harbor_source_two_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_codex_ptr,
            selected_codex_written as i64,
        )
    };
    let mut harbor_source_two = vec![0u8; harbor_source_two_len as usize];
    let harbor_source_two_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_codex_ptr,
            selected_codex_written as i64,
            harbor_source_two.as_mut_ptr(),
            harbor_source_two.len() as i64,
        )
    };
    let harbor_value_two_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            harbor_source_two.as_ptr(),
            harbor_source_two_written as i64,
            harbor_old_two.as_ptr(),
            harbor_old_two.len() as i64,
            harbor_new_two.as_ptr(),
            harbor_new_two.len() as i64,
        )
    };
    let mut harbor_value_two = vec![0u8; harbor_value_two_len as usize];
    let harbor_value_two_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            harbor_source_two.as_ptr(),
            harbor_source_two_written as i64,
            harbor_old_two.as_ptr(),
            harbor_old_two.len() as i64,
            harbor_new_two.as_ptr(),
            harbor_new_two.len() as i64,
            harbor_value_two.as_mut_ptr(),
            harbor_value_two.len() as i64,
        )
    };
    let harbor_contains_two = unsafe {
        rust_mcil_dotnet_string_contains(
            harbor_value_two.as_ptr(),
            harbor_value_two_written as i64,
            harbor_needle_two.as_ptr(),
            harbor_needle_two.len() as i64,
        )
    };

    let mut selected_horizon_index = 0i32;
    let mut best_horizon_score = i32::MIN;
    let mut horizon_index = 0i32;
    while horizon_index < 3 {
        let mut candidate_horizon_score = skyline_value_written * 10 + skyline_contains * 50;
        if horizon_index == 1 {
            candidate_horizon_score = flare_value_written * 10 + flare_index_two;
        } else if horizon_index == 2 {
            candidate_horizon_score = harbor_value_two_written * 10 + harbor_contains_two * 50;
        }

        let mut horizon_bonus = 0i32;
        if horizon_index == selected_codex_index {
            horizon_bonus += 25;
        }
        if horizon_index == selected_finale_index {
            horizon_bonus += 15;
        }
        if horizon_index == selected_terminal_index {
            horizon_bonus += 5;
        }
        if horizon_index == 0 && skyline_contains != 0 {
            horizon_bonus += 20;
        }
        if horizon_index == 1 && flare_index_two >= 0 {
            horizon_bonus += 10;
        }
        if horizon_index == 2 && harbor_contains_two != 0 {
            horizon_bonus += 30;
        }

        let horizon_score = candidate_horizon_score + horizon_bonus;
        if horizon_score > best_horizon_score {
            best_horizon_score = horizon_score;
            selected_horizon_index = horizon_index;
        }

        horizon_index += 1;
    }

    let mut selected_horizon_ptr = skyline_value.as_ptr();
    let mut selected_horizon_written = skyline_value_written;
    if selected_horizon_index == 1 {
        selected_horizon_ptr = flare_value_two.as_ptr();
        selected_horizon_written = flare_value_written;
    } else if selected_horizon_index == 2 {
        selected_horizon_ptr = harbor_value_two.as_ptr();
        selected_horizon_written = harbor_value_two_written;
    }

    let sunline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_horizon_ptr,
            selected_horizon_written as i64,
            sunline_old.as_ptr(),
            sunline_old.len() as i64,
            sunline_new.as_ptr(),
            sunline_new.len() as i64,
        )
    };
    let mut sunline_value = vec![0u8; sunline_value_len as usize];
    let sunline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_horizon_ptr,
            selected_horizon_written as i64,
            sunline_old.as_ptr(),
            sunline_old.len() as i64,
            sunline_new.as_ptr(),
            sunline_new.len() as i64,
            sunline_value.as_mut_ptr(),
            sunline_value.len() as i64,
        )
    };
    let sunline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            sunline_value.as_ptr(),
            sunline_value_written as i64,
            sunline_needle.as_ptr(),
            sunline_needle.len() as i64,
        )
    };

    let glint_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_horizon_ptr,
            selected_horizon_written as i64,
            glint_extension.as_ptr(),
            glint_extension.len() as i64,
        )
    };
    let mut glint_value = vec![0u8; glint_value_len as usize];
    let glint_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_horizon_ptr,
            selected_horizon_written as i64,
            glint_extension.as_ptr(),
            glint_extension.len() as i64,
            glint_value.as_mut_ptr(),
            glint_value.len() as i64,
        )
    };
    let glint_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            glint_value.as_ptr(),
            glint_value_written as i64,
            glint_needle.as_ptr(),
            glint_needle.len() as i64,
        )
    };

    let anchory_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_horizon_ptr,
            selected_horizon_written as i64,
        )
    };
    let mut anchory_source = vec![0u8; anchory_source_len as usize];
    let anchory_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_horizon_ptr,
            selected_horizon_written as i64,
            anchory_source.as_mut_ptr(),
            anchory_source.len() as i64,
        )
    };
    let anchory_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            anchory_source.as_ptr(),
            anchory_source_written as i64,
            anchory_old.as_ptr(),
            anchory_old.len() as i64,
            anchory_new.as_ptr(),
            anchory_new.len() as i64,
        )
    };
    let mut anchory_value = vec![0u8; anchory_value_len as usize];
    let anchory_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            anchory_source.as_ptr(),
            anchory_source_written as i64,
            anchory_old.as_ptr(),
            anchory_old.len() as i64,
            anchory_new.as_ptr(),
            anchory_new.len() as i64,
            anchory_value.as_mut_ptr(),
            anchory_value.len() as i64,
        )
    };
    let anchory_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            anchory_value.as_ptr(),
            anchory_value_written as i64,
            anchory_needle.as_ptr(),
            anchory_needle.len() as i64,
        )
    };

    let mut selected_orbit_index = 0i32;
    let mut best_orbit_score = i32::MIN;
    let mut orbit_index = 0i32;
    while orbit_index < 3 {
        let mut candidate_orbit_score = sunline_value_written * 10 + sunline_contains * 50;
        if orbit_index == 1 {
            candidate_orbit_score = glint_value_written * 10 + glint_index;
        } else if orbit_index == 2 {
            candidate_orbit_score = anchory_value_written * 10 + anchory_contains * 50;
        }

        let mut orbit_bonus = 0i32;
        if orbit_index == selected_horizon_index {
            orbit_bonus += 25;
        }
        if orbit_index == selected_codex_index {
            orbit_bonus += 15;
        }
        if orbit_index == selected_finale_index {
            orbit_bonus += 5;
        }
        if orbit_index == 0 && sunline_contains != 0 {
            orbit_bonus += 20;
        }
        if orbit_index == 1 && glint_index >= 0 {
            orbit_bonus += 10;
        }
        if orbit_index == 2 && anchory_contains != 0 {
            orbit_bonus += 30;
        }

        let orbit_score = candidate_orbit_score + orbit_bonus;
        if orbit_score > best_orbit_score {
            best_orbit_score = orbit_score;
            selected_orbit_index = orbit_index;
        }

        orbit_index += 1;
    }

    let mut selected_orbit_ptr = sunline_value.as_ptr();
    let mut selected_orbit_written = sunline_value_written;
    if selected_orbit_index == 1 {
        selected_orbit_ptr = glint_value.as_ptr();
        selected_orbit_written = glint_value_written;
    } else if selected_orbit_index == 2 {
        selected_orbit_ptr = anchory_value.as_ptr();
        selected_orbit_written = anchory_value_written;
    }

    let starline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_orbit_ptr,
            selected_orbit_written as i64,
            starline_old.as_ptr(),
            starline_old.len() as i64,
            starline_new.as_ptr(),
            starline_new.len() as i64,
        )
    };
    let mut starline_value = vec![0u8; starline_value_len as usize];
    let starline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_orbit_ptr,
            selected_orbit_written as i64,
            starline_old.as_ptr(),
            starline_old.len() as i64,
            starline_new.as_ptr(),
            starline_new.len() as i64,
            starline_value.as_mut_ptr(),
            starline_value.len() as i64,
        )
    };
    let starline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            starline_value.as_ptr(),
            starline_value_written as i64,
            starline_needle.as_ptr(),
            starline_needle.len() as i64,
        )
    };

    let comet_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_orbit_ptr,
            selected_orbit_written as i64,
            comet_extension.as_ptr(),
            comet_extension.len() as i64,
        )
    };
    let mut comet_value = vec![0u8; comet_value_len as usize];
    let comet_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_orbit_ptr,
            selected_orbit_written as i64,
            comet_extension.as_ptr(),
            comet_extension.len() as i64,
            comet_value.as_mut_ptr(),
            comet_value.len() as i64,
        )
    };
    let comet_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            comet_value.as_ptr(),
            comet_value_written as i64,
            comet_needle.as_ptr(),
            comet_needle.len() as i64,
        )
    };

    let pier_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_orbit_ptr,
            selected_orbit_written as i64,
        )
    };
    let mut pier_source = vec![0u8; pier_source_len as usize];
    let pier_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_orbit_ptr,
            selected_orbit_written as i64,
            pier_source.as_mut_ptr(),
            pier_source.len() as i64,
        )
    };
    let pier_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            pier_source.as_ptr(),
            pier_source_written as i64,
            pier_old.as_ptr(),
            pier_old.len() as i64,
            pier_new.as_ptr(),
            pier_new.len() as i64,
        )
    };
    let mut pier_value = vec![0u8; pier_value_len as usize];
    let pier_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            pier_source.as_ptr(),
            pier_source_written as i64,
            pier_old.as_ptr(),
            pier_old.len() as i64,
            pier_new.as_ptr(),
            pier_new.len() as i64,
            pier_value.as_mut_ptr(),
            pier_value.len() as i64,
        )
    };
    let pier_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            pier_value.as_ptr(),
            pier_value_written as i64,
            pier_needle.as_ptr(),
            pier_needle.len() as i64,
        )
    };

    let mut selected_galaxy_index = 0i32;
    let mut best_galaxy_score = i32::MIN;
    let mut galaxy_index = 0i32;
    while galaxy_index < 3 {
        let mut candidate_galaxy_score = starline_value_written * 10 + starline_contains * 50;
        if galaxy_index == 1 {
            candidate_galaxy_score = comet_value_written * 10 + comet_index;
        } else if galaxy_index == 2 {
            candidate_galaxy_score = pier_value_written * 10 + pier_contains * 50;
        }

        let mut galaxy_bonus = 0i32;
        if galaxy_index == selected_orbit_index {
            galaxy_bonus += 25;
        }
        if galaxy_index == selected_horizon_index {
            galaxy_bonus += 15;
        }
        if galaxy_index == selected_codex_index {
            galaxy_bonus += 5;
        }
        if galaxy_index == 0 && starline_contains != 0 {
            galaxy_bonus += 20;
        }
        if galaxy_index == 1 && comet_index >= 0 {
            galaxy_bonus += 10;
        }
        if galaxy_index == 2 && pier_contains != 0 {
            galaxy_bonus += 30;
        }

        let galaxy_score = candidate_galaxy_score + galaxy_bonus;
        if galaxy_score > best_galaxy_score {
            best_galaxy_score = galaxy_score;
            selected_galaxy_index = galaxy_index;
        }

        galaxy_index += 1;
    }

    let mut selected_galaxy_ptr = starline_value.as_ptr();
    let mut selected_galaxy_written = starline_value_written;
    if selected_galaxy_index == 1 {
        selected_galaxy_ptr = comet_value.as_ptr();
        selected_galaxy_written = comet_value_written;
    } else if selected_galaxy_index == 2 {
        selected_galaxy_ptr = pier_value.as_ptr();
        selected_galaxy_written = pier_value_written;
    }

    let moonline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_galaxy_ptr,
            selected_galaxy_written as i64,
            moonline_old.as_ptr(),
            moonline_old.len() as i64,
            moonline_new.as_ptr(),
            moonline_new.len() as i64,
        )
    };
    let mut moonline_value = vec![0u8; moonline_value_len as usize];
    let moonline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_galaxy_ptr,
            selected_galaxy_written as i64,
            moonline_old.as_ptr(),
            moonline_old.len() as i64,
            moonline_new.as_ptr(),
            moonline_new.len() as i64,
            moonline_value.as_mut_ptr(),
            moonline_value.len() as i64,
        )
    };
    let moonline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            moonline_value.as_ptr(),
            moonline_value_written as i64,
            moonline_needle.as_ptr(),
            moonline_needle.len() as i64,
        )
    };

    let nova_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_galaxy_ptr,
            selected_galaxy_written as i64,
            nova_extension.as_ptr(),
            nova_extension.len() as i64,
        )
    };
    let mut nova_value = vec![0u8; nova_value_len as usize];
    let nova_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_galaxy_ptr,
            selected_galaxy_written as i64,
            nova_extension.as_ptr(),
            nova_extension.len() as i64,
            nova_value.as_mut_ptr(),
            nova_value.len() as i64,
        )
    };
    let nova_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            nova_value.as_ptr(),
            nova_value_written as i64,
            nova_needle.as_ptr(),
            nova_needle.len() as i64,
        )
    };

    let jetty_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_galaxy_ptr,
            selected_galaxy_written as i64,
        )
    };
    let mut jetty_source = vec![0u8; jetty_source_len as usize];
    let jetty_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_galaxy_ptr,
            selected_galaxy_written as i64,
            jetty_source.as_mut_ptr(),
            jetty_source.len() as i64,
        )
    };
    let jetty_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            jetty_source.as_ptr(),
            jetty_source_written as i64,
            jetty_old.as_ptr(),
            jetty_old.len() as i64,
            jetty_new.as_ptr(),
            jetty_new.len() as i64,
        )
    };
    let mut jetty_value = vec![0u8; jetty_value_len as usize];
    let jetty_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            jetty_source.as_ptr(),
            jetty_source_written as i64,
            jetty_old.as_ptr(),
            jetty_old.len() as i64,
            jetty_new.as_ptr(),
            jetty_new.len() as i64,
            jetty_value.as_mut_ptr(),
            jetty_value.len() as i64,
        )
    };
    let jetty_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            jetty_value.as_ptr(),
            jetty_value_written as i64,
            jetty_needle.as_ptr(),
            jetty_needle.len() as i64,
        )
    };

    let mut selected_cosmos_index = 0i32;
    let mut best_cosmos_score = i32::MIN;
    let mut cosmos_index = 0i32;
    while cosmos_index < 3 {
        let mut candidate_cosmos_score = moonline_value_written * 10 + moonline_contains * 50;
        if cosmos_index == 1 {
            candidate_cosmos_score = nova_value_written * 10 + nova_index;
        } else if cosmos_index == 2 {
            candidate_cosmos_score = jetty_value_written * 10 + jetty_contains * 50;
        }

        let mut cosmos_bonus = 0i32;
        if cosmos_index == selected_galaxy_index {
            cosmos_bonus += 25;
        }
        if cosmos_index == selected_orbit_index {
            cosmos_bonus += 15;
        }
        if cosmos_index == selected_horizon_index {
            cosmos_bonus += 5;
        }
        if cosmos_index == 0 && moonline_contains != 0 {
            cosmos_bonus += 20;
        }
        if cosmos_index == 1 && nova_index >= 0 {
            cosmos_bonus += 10;
        }
        if cosmos_index == 2 && jetty_contains != 0 {
            cosmos_bonus += 30;
        }

        let cosmos_score = candidate_cosmos_score + cosmos_bonus;
        if cosmos_score > best_cosmos_score {
            best_cosmos_score = cosmos_score;
            selected_cosmos_index = cosmos_index;
        }

        cosmos_index += 1;
    }

    let mut selected_cosmos_ptr = moonline_value.as_ptr();
    let mut selected_cosmos_written = moonline_value_written;
    if selected_cosmos_index == 1 {
        selected_cosmos_ptr = nova_value.as_ptr();
        selected_cosmos_written = nova_value_written;
    } else if selected_cosmos_index == 2 {
        selected_cosmos_ptr = jetty_value.as_ptr();
        selected_cosmos_written = jetty_value_written;
    }

    let tide_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_cosmos_ptr,
            selected_cosmos_written as i64,
            tide_old.as_ptr(),
            tide_old.len() as i64,
            tide_new.as_ptr(),
            tide_new.len() as i64,
        )
    };
    let mut tide_value = vec![0u8; tide_value_len as usize];
    let tide_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_cosmos_ptr,
            selected_cosmos_written as i64,
            tide_old.as_ptr(),
            tide_old.len() as i64,
            tide_new.as_ptr(),
            tide_new.len() as i64,
            tide_value.as_mut_ptr(),
            tide_value.len() as i64,
        )
    };
    let tide_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            tide_value.as_ptr(),
            tide_value_written as i64,
            tide_needle.as_ptr(),
            tide_needle.len() as i64,
        )
    };

    let quasar_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_cosmos_ptr,
            selected_cosmos_written as i64,
            quasar_extension.as_ptr(),
            quasar_extension.len() as i64,
        )
    };
    let mut quasar_value = vec![0u8; quasar_value_len as usize];
    let quasar_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_cosmos_ptr,
            selected_cosmos_written as i64,
            quasar_extension.as_ptr(),
            quasar_extension.len() as i64,
            quasar_value.as_mut_ptr(),
            quasar_value.len() as i64,
        )
    };
    let quasar_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            quasar_value.as_ptr(),
            quasar_value_written as i64,
            quasar_needle.as_ptr(),
            quasar_needle.len() as i64,
        )
    };

    let harbor_source_three_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_cosmos_ptr,
            selected_cosmos_written as i64,
        )
    };
    let mut harbor_source_three = vec![0u8; harbor_source_three_len as usize];
    let harbor_source_three_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_cosmos_ptr,
            selected_cosmos_written as i64,
            harbor_source_three.as_mut_ptr(),
            harbor_source_three.len() as i64,
        )
    };
    let harbor_value_three_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            harbor_source_three.as_ptr(),
            harbor_source_three_written as i64,
            harbor_old_three.as_ptr(),
            harbor_old_three.len() as i64,
            harbor_new_three.as_ptr(),
            harbor_new_three.len() as i64,
        )
    };
    let mut harbor_value_three = vec![0u8; harbor_value_three_len as usize];
    let harbor_value_three_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            harbor_source_three.as_ptr(),
            harbor_source_three_written as i64,
            harbor_old_three.as_ptr(),
            harbor_old_three.len() as i64,
            harbor_new_three.as_ptr(),
            harbor_new_three.len() as i64,
            harbor_value_three.as_mut_ptr(),
            harbor_value_three.len() as i64,
        )
    };
    let harbor_contains_three = unsafe {
        rust_mcil_dotnet_string_contains(
            harbor_value_three.as_ptr(),
            harbor_value_three_written as i64,
            harbor_needle_three.as_ptr(),
            harbor_needle_three.len() as i64,
        )
    };

    let mut selected_nebula_index = 0i32;
    let mut best_nebula_score = i32::MIN;
    let mut nebula_index = 0i32;
    while nebula_index < 3 {
        let mut candidate_nebula_score = tide_value_written * 10 + tide_contains * 50;
        if nebula_index == 1 {
            candidate_nebula_score = quasar_value_written * 10 + quasar_index;
        } else if nebula_index == 2 {
            candidate_nebula_score = harbor_value_three_written * 10 + harbor_contains_three * 50;
        }

        let mut nebula_bonus = 0i32;
        if nebula_index == selected_cosmos_index {
            nebula_bonus += 25;
        }
        if nebula_index == selected_galaxy_index {
            nebula_bonus += 15;
        }
        if nebula_index == selected_orbit_index {
            nebula_bonus += 5;
        }
        if nebula_index == 0 && tide_contains != 0 {
            nebula_bonus += 20;
        }
        if nebula_index == 1 && quasar_index >= 0 {
            nebula_bonus += 10;
        }
        if nebula_index == 2 && harbor_contains_three != 0 {
            nebula_bonus += 30;
        }

        let nebula_score = candidate_nebula_score + nebula_bonus;
        if nebula_score > best_nebula_score {
            best_nebula_score = nebula_score;
            selected_nebula_index = nebula_index;
        }

        nebula_index += 1;
    }

    let mut selected_nebula_ptr = tide_value.as_ptr();
    let mut selected_nebula_written = tide_value_written;
    if selected_nebula_index == 1 {
        selected_nebula_ptr = quasar_value.as_ptr();
        selected_nebula_written = quasar_value_written;
    } else if selected_nebula_index == 2 {
        selected_nebula_ptr = harbor_value_three.as_ptr();
        selected_nebula_written = harbor_value_three_written;
    }

    let estuary_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_nebula_ptr,
            selected_nebula_written as i64,
            estuary_old.as_ptr(),
            estuary_old.len() as i64,
            estuary_new.as_ptr(),
            estuary_new.len() as i64,
        )
    };
    let mut estuary_value = vec![0u8; estuary_value_len as usize];
    let estuary_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_nebula_ptr,
            selected_nebula_written as i64,
            estuary_old.as_ptr(),
            estuary_old.len() as i64,
            estuary_new.as_ptr(),
            estuary_new.len() as i64,
            estuary_value.as_mut_ptr(),
            estuary_value.len() as i64,
        )
    };
    let estuary_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            estuary_value.as_ptr(),
            estuary_value_written as i64,
            estuary_needle.as_ptr(),
            estuary_needle.len() as i64,
        )
    };

    let pulsar_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_nebula_ptr,
            selected_nebula_written as i64,
            pulsar_extension.as_ptr(),
            pulsar_extension.len() as i64,
        )
    };
    let mut pulsar_value = vec![0u8; pulsar_value_len as usize];
    let pulsar_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_nebula_ptr,
            selected_nebula_written as i64,
            pulsar_extension.as_ptr(),
            pulsar_extension.len() as i64,
            pulsar_value.as_mut_ptr(),
            pulsar_value.len() as i64,
        )
    };
    let pulsar_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            pulsar_value.as_ptr(),
            pulsar_value_written as i64,
            pulsar_needle.as_ptr(),
            pulsar_needle.len() as i64,
        )
    };

    let dock_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_nebula_ptr,
            selected_nebula_written as i64,
        )
    };
    let mut dock_source = vec![0u8; dock_source_len as usize];
    let dock_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_nebula_ptr,
            selected_nebula_written as i64,
            dock_source.as_mut_ptr(),
            dock_source.len() as i64,
        )
    };
    let dock_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            dock_source.as_ptr(),
            dock_source_written as i64,
            dock_old.as_ptr(),
            dock_old.len() as i64,
            dock_new.as_ptr(),
            dock_new.len() as i64,
        )
    };
    let mut dock_value = vec![0u8; dock_value_len as usize];
    let dock_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            dock_source.as_ptr(),
            dock_source_written as i64,
            dock_old.as_ptr(),
            dock_old.len() as i64,
            dock_new.as_ptr(),
            dock_new.len() as i64,
            dock_value.as_mut_ptr(),
            dock_value.len() as i64,
        )
    };
    let dock_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            dock_value.as_ptr(),
            dock_value_written as i64,
            dock_needle.as_ptr(),
            dock_needle.len() as i64,
        )
    };

    let mut selected_delta_index = 0i32;
    let mut best_delta_score = i32::MIN;
    let mut delta_index = 0i32;
    while delta_index < 3 {
        let mut candidate_delta_score = estuary_value_written * 10 + estuary_contains * 50;
        if delta_index == 1 {
            candidate_delta_score = pulsar_value_written * 10 + pulsar_index;
        } else if delta_index == 2 {
            candidate_delta_score = dock_value_written * 10 + dock_contains * 50;
        }

        let mut delta_bonus = 0i32;
        if delta_index == selected_nebula_index {
            delta_bonus += 25;
        }
        if delta_index == selected_cosmos_index {
            delta_bonus += 15;
        }
        if delta_index == selected_galaxy_index {
            delta_bonus += 5;
        }
        if delta_index == 0 && estuary_contains != 0 {
            delta_bonus += 20;
        }
        if delta_index == 1 && pulsar_index >= 0 {
            delta_bonus += 10;
        }
        if delta_index == 2 && dock_contains != 0 {
            delta_bonus += 30;
        }

        let delta_score = candidate_delta_score + delta_bonus;
        if delta_score > best_delta_score {
            best_delta_score = delta_score;
            selected_delta_index = delta_index;
        }

        delta_index += 1;
    }

    let mut selected_delta_ptr = estuary_value.as_ptr();
    let mut selected_delta_written = estuary_value_written;
    if selected_delta_index == 1 {
        selected_delta_ptr = pulsar_value.as_ptr();
        selected_delta_written = pulsar_value_written;
    } else if selected_delta_index == 2 {
        selected_delta_ptr = dock_value.as_ptr();
        selected_delta_written = dock_value_written;
    }

    let shoreline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_delta_ptr,
            selected_delta_written as i64,
            shoreline_old.as_ptr(),
            shoreline_old.len() as i64,
            shoreline_new.as_ptr(),
            shoreline_new.len() as i64,
        )
    };
    let mut shoreline_value = vec![0u8; shoreline_value_len as usize];
    let shoreline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_delta_ptr,
            selected_delta_written as i64,
            shoreline_old.as_ptr(),
            shoreline_old.len() as i64,
            shoreline_new.as_ptr(),
            shoreline_new.len() as i64,
            shoreline_value.as_mut_ptr(),
            shoreline_value.len() as i64,
        )
    };
    let shoreline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            shoreline_value.as_ptr(),
            shoreline_value_written as i64,
            shoreline_needle.as_ptr(),
            shoreline_needle.len() as i64,
        )
    };

    let meteor_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_delta_ptr,
            selected_delta_written as i64,
            meteor_extension.as_ptr(),
            meteor_extension.len() as i64,
        )
    };
    let mut meteor_value = vec![0u8; meteor_value_len as usize];
    let meteor_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_delta_ptr,
            selected_delta_written as i64,
            meteor_extension.as_ptr(),
            meteor_extension.len() as i64,
            meteor_value.as_mut_ptr(),
            meteor_value.len() as i64,
        )
    };
    let meteor_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            meteor_value.as_ptr(),
            meteor_value_written as i64,
            meteor_needle.as_ptr(),
            meteor_needle.len() as i64,
        )
    };

    let quay_source_two_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_delta_ptr,
            selected_delta_written as i64,
        )
    };
    let mut quay_source_two = vec![0u8; quay_source_two_len as usize];
    let quay_source_two_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_delta_ptr,
            selected_delta_written as i64,
            quay_source_two.as_mut_ptr(),
            quay_source_two.len() as i64,
        )
    };
    let quay_value_two_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            quay_source_two.as_ptr(),
            quay_source_two_written as i64,
            quay_old_two.as_ptr(),
            quay_old_two.len() as i64,
            quay_new_two.as_ptr(),
            quay_new_two.len() as i64,
        )
    };
    let mut quay_value_two = vec![0u8; quay_value_two_len as usize];
    let quay_value_two_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            quay_source_two.as_ptr(),
            quay_source_two_written as i64,
            quay_old_two.as_ptr(),
            quay_old_two.len() as i64,
            quay_new_two.as_ptr(),
            quay_new_two.len() as i64,
            quay_value_two.as_mut_ptr(),
            quay_value_two.len() as i64,
        )
    };
    let quay_contains_two = unsafe {
        rust_mcil_dotnet_string_contains(
            quay_value_two.as_ptr(),
            quay_value_two_written as i64,
            quay_needle_two.as_ptr(),
            quay_needle_two.len() as i64,
        )
    };

    let mut selected_harbor_index = 0i32;
    let mut best_harbor_score = i32::MIN;
    let mut harbor_index = 0i32;
    while harbor_index < 3 {
        let mut candidate_harbor_score = shoreline_value_written * 10 + shoreline_contains * 50;
        if harbor_index == 1 {
            candidate_harbor_score = meteor_value_written * 10 + meteor_index;
        } else if harbor_index == 2 {
            candidate_harbor_score = quay_value_two_written * 10 + quay_contains_two * 50;
        }

        let mut harbor_bonus = 0i32;
        if harbor_index == selected_delta_index {
            harbor_bonus += 25;
        }
        if harbor_index == selected_nebula_index {
            harbor_bonus += 15;
        }
        if harbor_index == selected_cosmos_index {
            harbor_bonus += 5;
        }
        if harbor_index == 0 && shoreline_contains != 0 {
            harbor_bonus += 20;
        }
        if harbor_index == 1 && meteor_index >= 0 {
            harbor_bonus += 10;
        }
        if harbor_index == 2 && quay_contains_two != 0 {
            harbor_bonus += 30;
        }

        let harbor_score = candidate_harbor_score + harbor_bonus;
        if harbor_score > best_harbor_score {
            best_harbor_score = harbor_score;
            selected_harbor_index = harbor_index;
        }

        harbor_index += 1;
    }

    let mut selected_harbor_ptr = shoreline_value.as_ptr();
    let mut selected_harbor_written = shoreline_value_written;
    if selected_harbor_index == 1 {
        selected_harbor_ptr = meteor_value.as_ptr();
        selected_harbor_written = meteor_value_written;
    } else if selected_harbor_index == 2 {
        selected_harbor_ptr = quay_value_two.as_ptr();
        selected_harbor_written = quay_value_two_written;
    }

    let breakwater_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_harbor_ptr,
            selected_harbor_written as i64,
            breakwater_old.as_ptr(),
            breakwater_old.len() as i64,
            breakwater_new.as_ptr(),
            breakwater_new.len() as i64,
        )
    };
    let mut breakwater_value = vec![0u8; breakwater_value_len as usize];
    let breakwater_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_harbor_ptr,
            selected_harbor_written as i64,
            breakwater_old.as_ptr(),
            breakwater_old.len() as i64,
            breakwater_new.as_ptr(),
            breakwater_new.len() as i64,
            breakwater_value.as_mut_ptr(),
            breakwater_value.len() as i64,
        )
    };
    let breakwater_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            breakwater_value.as_ptr(),
            breakwater_value_written as i64,
            breakwater_needle.as_ptr(),
            breakwater_needle.len() as i64,
        )
    };

    let aurora_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_harbor_ptr,
            selected_harbor_written as i64,
            aurora_extension.as_ptr(),
            aurora_extension.len() as i64,
        )
    };
    let mut aurora_value = vec![0u8; aurora_value_len as usize];
    let aurora_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_harbor_ptr,
            selected_harbor_written as i64,
            aurora_extension.as_ptr(),
            aurora_extension.len() as i64,
            aurora_value.as_mut_ptr(),
            aurora_value.len() as i64,
        )
    };
    let aurora_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            aurora_value.as_ptr(),
            aurora_value_written as i64,
            aurora_needle.as_ptr(),
            aurora_needle.len() as i64,
        )
    };

    let pier_source_two_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_harbor_ptr,
            selected_harbor_written as i64,
        )
    };
    let mut pier_source_two = vec![0u8; pier_source_two_len as usize];
    let pier_source_two_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_harbor_ptr,
            selected_harbor_written as i64,
            pier_source_two.as_mut_ptr(),
            pier_source_two.len() as i64,
        )
    };
    let pier_value_two_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            pier_source_two.as_ptr(),
            pier_source_two_written as i64,
            pier_old_two.as_ptr(),
            pier_old_two.len() as i64,
            pier_new_two.as_ptr(),
            pier_new_two.len() as i64,
        )
    };
    let mut pier_value_two = vec![0u8; pier_value_two_len as usize];
    let pier_value_two_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            pier_source_two.as_ptr(),
            pier_source_two_written as i64,
            pier_old_two.as_ptr(),
            pier_old_two.len() as i64,
            pier_new_two.as_ptr(),
            pier_new_two.len() as i64,
            pier_value_two.as_mut_ptr(),
            pier_value_two.len() as i64,
        )
    };
    let pier_contains_two = unsafe {
        rust_mcil_dotnet_string_contains(
            pier_value_two.as_ptr(),
            pier_value_two_written as i64,
            pier_needle_two.as_ptr(),
            pier_needle_two.len() as i64,
        )
    };

    let mut selected_beacon_index = 0i32;
    let mut best_beacon_score = i32::MIN;
    let mut beacon_index = 0i32;
    while beacon_index < 3 {
        let mut candidate_beacon_score = breakwater_value_written * 10 + breakwater_contains * 50;
        if beacon_index == 1 {
            candidate_beacon_score = aurora_value_written * 10 + aurora_index;
        } else if beacon_index == 2 {
            candidate_beacon_score = pier_value_two_written * 10 + pier_contains_two * 50;
        }

        let mut beacon_bonus = 0i32;
        if beacon_index == selected_harbor_index {
            beacon_bonus += 25;
        }
        if beacon_index == selected_delta_index {
            beacon_bonus += 15;
        }
        if beacon_index == selected_nebula_index {
            beacon_bonus += 5;
        }
        if beacon_index == 0 && breakwater_contains != 0 {
            beacon_bonus += 20;
        }
        if beacon_index == 1 && aurora_index >= 0 {
            beacon_bonus += 10;
        }
        if beacon_index == 2 && pier_contains_two != 0 {
            beacon_bonus += 30;
        }

        let beacon_score = candidate_beacon_score + beacon_bonus;
        if beacon_score > best_beacon_score {
            best_beacon_score = beacon_score;
            selected_beacon_index = beacon_index;
        }

        beacon_index += 1;
    }

    let mut selected_beacon_ptr = breakwater_value.as_ptr();
    let mut selected_beacon_written = breakwater_value_written;
    if selected_beacon_index == 1 {
        selected_beacon_ptr = aurora_value.as_ptr();
        selected_beacon_written = aurora_value_written;
    } else if selected_beacon_index == 2 {
        selected_beacon_ptr = pier_value_two.as_ptr();
        selected_beacon_written = pier_value_two_written;
    }

    let causeway_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_beacon_ptr,
            selected_beacon_written as i64,
            causeway_old.as_ptr(),
            causeway_old.len() as i64,
            causeway_new.as_ptr(),
            causeway_new.len() as i64,
        )
    };
    let mut causeway_value = vec![0u8; causeway_value_len as usize];
    let causeway_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_beacon_ptr,
            selected_beacon_written as i64,
            causeway_old.as_ptr(),
            causeway_old.len() as i64,
            causeway_new.as_ptr(),
            causeway_new.len() as i64,
            causeway_value.as_mut_ptr(),
            causeway_value.len() as i64,
        )
    };
    let causeway_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            causeway_value.as_ptr(),
            causeway_value_written as i64,
            causeway_needle.as_ptr(),
            causeway_needle.len() as i64,
        )
    };

    let starlight_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_beacon_ptr,
            selected_beacon_written as i64,
            starlight_extension.as_ptr(),
            starlight_extension.len() as i64,
        )
    };
    let mut starlight_value = vec![0u8; starlight_value_len as usize];
    let starlight_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_beacon_ptr,
            selected_beacon_written as i64,
            starlight_extension.as_ptr(),
            starlight_extension.len() as i64,
            starlight_value.as_mut_ptr(),
            starlight_value.len() as i64,
        )
    };
    let starlight_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            starlight_value.as_ptr(),
            starlight_value_written as i64,
            starlight_needle.as_ptr(),
            starlight_needle.len() as i64,
        )
    };

    let berth_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_beacon_ptr,
            selected_beacon_written as i64,
        )
    };
    let mut berth_source = vec![0u8; berth_source_len as usize];
    let berth_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_beacon_ptr,
            selected_beacon_written as i64,
            berth_source.as_mut_ptr(),
            berth_source.len() as i64,
        )
    };
    let berth_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            berth_source.as_ptr(),
            berth_source_written as i64,
            berth_old.as_ptr(),
            berth_old.len() as i64,
            berth_new.as_ptr(),
            berth_new.len() as i64,
        )
    };
    let mut berth_value = vec![0u8; berth_value_len as usize];
    let berth_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            berth_source.as_ptr(),
            berth_source_written as i64,
            berth_old.as_ptr(),
            berth_old.len() as i64,
            berth_new.as_ptr(),
            berth_new.len() as i64,
            berth_value.as_mut_ptr(),
            berth_value.len() as i64,
        )
    };
    let berth_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            berth_value.as_ptr(),
            berth_value_written as i64,
            berth_needle.as_ptr(),
            berth_needle.len() as i64,
        )
    };

    let mut selected_signal_index = 0i32;
    let mut best_signal_score = i32::MIN;
    let mut signal_index = 0i32;
    while signal_index < 3 {
        let mut candidate_signal_score = causeway_value_written * 10 + causeway_contains * 50;
        if signal_index == 1 {
            candidate_signal_score = starlight_value_written * 10 + starlight_index;
        } else if signal_index == 2 {
            candidate_signal_score = berth_value_written * 10 + berth_contains * 50;
        }

        let mut signal_bonus = 0i32;
        if signal_index == selected_beacon_index {
            signal_bonus += 25;
        }
        if signal_index == selected_harbor_index {
            signal_bonus += 15;
        }
        if signal_index == selected_delta_index {
            signal_bonus += 5;
        }
        if signal_index == 0 && causeway_contains != 0 {
            signal_bonus += 20;
        }
        if signal_index == 1 && starlight_index >= 0 {
            signal_bonus += 10;
        }
        if signal_index == 2 && berth_contains != 0 {
            signal_bonus += 30;
        }

        let signal_score = candidate_signal_score + signal_bonus;
        if signal_score > best_signal_score {
            best_signal_score = signal_score;
            selected_signal_index = signal_index;
        }

        signal_index += 1;
    }

    let mut selected_signal_ptr = causeway_value.as_ptr();
    let mut selected_signal_written = causeway_value_written;
    if selected_signal_index == 1 {
        selected_signal_ptr = starlight_value.as_ptr();
        selected_signal_written = starlight_value_written;
    } else if selected_signal_index == 2 {
        selected_signal_ptr = berth_value.as_ptr();
        selected_signal_written = berth_value_written;
    }

    let boardwalk_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_signal_ptr,
            selected_signal_written as i64,
            boardwalk_old.as_ptr(),
            boardwalk_old.len() as i64,
            boardwalk_new.as_ptr(),
            boardwalk_new.len() as i64,
        )
    };
    let mut boardwalk_value = vec![0u8; boardwalk_value_len as usize];
    let boardwalk_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_signal_ptr,
            selected_signal_written as i64,
            boardwalk_old.as_ptr(),
            boardwalk_old.len() as i64,
            boardwalk_new.as_ptr(),
            boardwalk_new.len() as i64,
            boardwalk_value.as_mut_ptr(),
            boardwalk_value.len() as i64,
        )
    };
    let boardwalk_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            boardwalk_value.as_ptr(),
            boardwalk_value_written as i64,
            boardwalk_needle.as_ptr(),
            boardwalk_needle.len() as i64,
        )
    };

    let moonrise_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_signal_ptr,
            selected_signal_written as i64,
            moonrise_extension.as_ptr(),
            moonrise_extension.len() as i64,
        )
    };
    let mut moonrise_value = vec![0u8; moonrise_value_len as usize];
    let moonrise_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_signal_ptr,
            selected_signal_written as i64,
            moonrise_extension.as_ptr(),
            moonrise_extension.len() as i64,
            moonrise_value.as_mut_ptr(),
            moonrise_value.len() as i64,
        )
    };
    let moonrise_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            moonrise_value.as_ptr(),
            moonrise_value_written as i64,
            moonrise_needle.as_ptr(),
            moonrise_needle.len() as i64,
        )
    };

    let dock_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_signal_ptr,
            selected_signal_written as i64,
        )
    };
    let mut dock_source = vec![0u8; dock_source_len as usize];
    let dock_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_signal_ptr,
            selected_signal_written as i64,
            dock_source.as_mut_ptr(),
            dock_source.len() as i64,
        )
    };
    let dock_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            dock_source.as_ptr(),
            dock_source_written as i64,
            dock_old.as_ptr(),
            dock_old.len() as i64,
            dock_new.as_ptr(),
            dock_new.len() as i64,
        )
    };
    let mut dock_value = vec![0u8; dock_value_len as usize];
    let dock_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            dock_source.as_ptr(),
            dock_source_written as i64,
            dock_old.as_ptr(),
            dock_old.len() as i64,
            dock_new.as_ptr(),
            dock_new.len() as i64,
            dock_value.as_mut_ptr(),
            dock_value.len() as i64,
        )
    };
    let dock_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            dock_value.as_ptr(),
            dock_value_written as i64,
            dock_needle.as_ptr(),
            dock_needle.len() as i64,
        )
    };

    let mut selected_channel_index = 0i32;
    let mut best_channel_score = i32::MIN;
    let mut channel_index = 0i32;
    while channel_index < 3 {
        let mut candidate_channel_score = boardwalk_value_written * 10 + boardwalk_contains * 50;
        if channel_index == 1 {
            candidate_channel_score = moonrise_value_written * 10 + moonrise_index;
        } else if channel_index == 2 {
            candidate_channel_score = dock_value_written * 10 + dock_contains * 50;
        }

        let mut channel_bonus = 0i32;
        if channel_index == selected_signal_index {
            channel_bonus += 25;
        }
        if channel_index == selected_beacon_index {
            channel_bonus += 15;
        }
        if channel_index == selected_harbor_index {
            channel_bonus += 5;
        }
        if channel_index == 0 && boardwalk_contains != 0 {
            channel_bonus += 20;
        }
        if channel_index == 1 && moonrise_index >= 0 {
            channel_bonus += 10;
        }
        if channel_index == 2 && dock_contains != 0 {
            channel_bonus += 30;
        }

        let channel_score = candidate_channel_score + channel_bonus;
        if channel_score > best_channel_score {
            best_channel_score = channel_score;
            selected_channel_index = channel_index;
        }

        channel_index += 1;
    }

    let mut selected_channel_ptr = boardwalk_value.as_ptr();
    let mut selected_channel_written = boardwalk_value_written;
    if selected_channel_index == 1 {
        selected_channel_ptr = moonrise_value.as_ptr();
        selected_channel_written = moonrise_value_written;
    } else if selected_channel_index == 2 {
        selected_channel_ptr = dock_value.as_ptr();
        selected_channel_written = dock_value_written;
    }

    let esplanade_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_channel_ptr,
            selected_channel_written as i64,
            esplanade_old.as_ptr(),
            esplanade_old.len() as i64,
            esplanade_new.as_ptr(),
            esplanade_new.len() as i64,
        )
    };
    let mut esplanade_value = vec![0u8; esplanade_value_len as usize];
    let esplanade_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_channel_ptr,
            selected_channel_written as i64,
            esplanade_old.as_ptr(),
            esplanade_old.len() as i64,
            esplanade_new.as_ptr(),
            esplanade_new.len() as i64,
            esplanade_value.as_mut_ptr(),
            esplanade_value.len() as i64,
        )
    };
    let esplanade_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            esplanade_value.as_ptr(),
            esplanade_value_written as i64,
            esplanade_needle.as_ptr(),
            esplanade_needle.len() as i64,
        )
    };

    let daybreak_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_channel_ptr,
            selected_channel_written as i64,
            daybreak_extension.as_ptr(),
            daybreak_extension.len() as i64,
        )
    };
    let mut daybreak_value = vec![0u8; daybreak_value_len as usize];
    let daybreak_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_channel_ptr,
            selected_channel_written as i64,
            daybreak_extension.as_ptr(),
            daybreak_extension.len() as i64,
            daybreak_value.as_mut_ptr(),
            daybreak_value.len() as i64,
        )
    };
    let daybreak_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            daybreak_value.as_ptr(),
            daybreak_value_written as i64,
            daybreak_needle.as_ptr(),
            daybreak_needle.len() as i64,
        )
    };

    let slip_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_channel_ptr,
            selected_channel_written as i64,
        )
    };
    let mut slip_source = vec![0u8; slip_source_len as usize];
    let slip_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_channel_ptr,
            selected_channel_written as i64,
            slip_source.as_mut_ptr(),
            slip_source.len() as i64,
        )
    };
    let slip_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            slip_source.as_ptr(),
            slip_source_written as i64,
            slip_old.as_ptr(),
            slip_old.len() as i64,
            slip_new.as_ptr(),
            slip_new.len() as i64,
        )
    };
    let mut slip_value = vec![0u8; slip_value_len as usize];
    let slip_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            slip_source.as_ptr(),
            slip_source_written as i64,
            slip_old.as_ptr(),
            slip_old.len() as i64,
            slip_new.as_ptr(),
            slip_new.len() as i64,
            slip_value.as_mut_ptr(),
            slip_value.len() as i64,
        )
    };
    let slip_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            slip_value.as_ptr(),
            slip_value_written as i64,
            slip_needle.as_ptr(),
            slip_needle.len() as i64,
        )
    };

    let mut selected_lantern_index = 0i32;
    let mut best_lantern_score = i32::MIN;
    let mut lantern_index = 0i32;
    while lantern_index < 3 {
        let mut candidate_lantern_score = esplanade_value_written * 10 + esplanade_contains * 50;
        if lantern_index == 1 {
            candidate_lantern_score = daybreak_value_written * 10 + daybreak_index;
        } else if lantern_index == 2 {
            candidate_lantern_score = slip_value_written * 10 + slip_contains * 50;
        }

        let mut lantern_bonus = 0i32;
        if lantern_index == selected_channel_index {
            lantern_bonus += 25;
        }
        if lantern_index == selected_signal_index {
            lantern_bonus += 15;
        }
        if lantern_index == selected_beacon_index {
            lantern_bonus += 5;
        }
        if lantern_index == 0 && esplanade_contains != 0 {
            lantern_bonus += 20;
        }
        if lantern_index == 1 && daybreak_index >= 0 {
            lantern_bonus += 10;
        }
        if lantern_index == 2 && slip_contains != 0 {
            lantern_bonus += 30;
        }

        let lantern_score = candidate_lantern_score + lantern_bonus;
        if lantern_score > best_lantern_score {
            best_lantern_score = lantern_score;
            selected_lantern_index = lantern_index;
        }

        lantern_index += 1;
    }

    let mut selected_lantern_ptr = esplanade_value.as_ptr();
    let mut selected_lantern_written = esplanade_value_written;
    if selected_lantern_index == 1 {
        selected_lantern_ptr = daybreak_value.as_ptr();
        selected_lantern_written = daybreak_value_written;
    } else if selected_lantern_index == 2 {
        selected_lantern_ptr = slip_value.as_ptr();
        selected_lantern_written = slip_value_written;
    }

    let promenade_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_lantern_ptr,
            selected_lantern_written as i64,
            promenade_old.as_ptr(),
            promenade_old.len() as i64,
            promenade_new.as_ptr(),
            promenade_new.len() as i64,
        )
    };
    let mut promenade_value = vec![0u8; promenade_value_len as usize];
    let promenade_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_lantern_ptr,
            selected_lantern_written as i64,
            promenade_old.as_ptr(),
            promenade_old.len() as i64,
            promenade_new.as_ptr(),
            promenade_new.len() as i64,
            promenade_value.as_mut_ptr(),
            promenade_value.len() as i64,
        )
    };
    let promenade_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            promenade_value.as_ptr(),
            promenade_value_written as i64,
            promenade_needle.as_ptr(),
            promenade_needle.len() as i64,
        )
    };

    let sunrise_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_lantern_ptr,
            selected_lantern_written as i64,
            sunrise_extension.as_ptr(),
            sunrise_extension.len() as i64,
        )
    };
    let mut sunrise_value = vec![0u8; sunrise_value_len as usize];
    let sunrise_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_lantern_ptr,
            selected_lantern_written as i64,
            sunrise_extension.as_ptr(),
            sunrise_extension.len() as i64,
            sunrise_value.as_mut_ptr(),
            sunrise_value.len() as i64,
        )
    };
    let sunrise_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            sunrise_value.as_ptr(),
            sunrise_value_written as i64,
            sunrise_needle.as_ptr(),
            sunrise_needle.len() as i64,
        )
    };

    let jetty_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_lantern_ptr,
            selected_lantern_written as i64,
        )
    };
    let mut jetty_source = vec![0u8; jetty_source_len as usize];
    let jetty_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_lantern_ptr,
            selected_lantern_written as i64,
            jetty_source.as_mut_ptr(),
            jetty_source.len() as i64,
        )
    };
    let jetty_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            jetty_source.as_ptr(),
            jetty_source_written as i64,
            jetty_old.as_ptr(),
            jetty_old.len() as i64,
            jetty_new.as_ptr(),
            jetty_new.len() as i64,
        )
    };
    let mut jetty_value = vec![0u8; jetty_value_len as usize];
    let jetty_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            jetty_source.as_ptr(),
            jetty_source_written as i64,
            jetty_old.as_ptr(),
            jetty_old.len() as i64,
            jetty_new.as_ptr(),
            jetty_new.len() as i64,
            jetty_value.as_mut_ptr(),
            jetty_value.len() as i64,
        )
    };
    let jetty_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            jetty_value.as_ptr(),
            jetty_value_written as i64,
            jetty_needle.as_ptr(),
            jetty_needle.len() as i64,
        )
    };

    let mut selected_marina_index = 0i32;
    let mut best_marina_score = i32::MIN;
    let mut marina_index = 0i32;
    while marina_index < 3 {
        let mut candidate_marina_score = promenade_value_written * 10 + promenade_contains * 50;
        if marina_index == 1 {
            candidate_marina_score = sunrise_value_written * 10 + sunrise_index;
        } else if marina_index == 2 {
            candidate_marina_score = jetty_value_written * 10 + jetty_contains * 50;
        }

        let mut marina_bonus = 0i32;
        if marina_index == selected_lantern_index {
            marina_bonus += 25;
        }
        if marina_index == selected_channel_index {
            marina_bonus += 15;
        }
        if marina_index == selected_signal_index {
            marina_bonus += 5;
        }
        if marina_index == 0 && promenade_contains != 0 {
            marina_bonus += 20;
        }
        if marina_index == 1 && sunrise_index >= 0 {
            marina_bonus += 10;
        }
        if marina_index == 2 && jetty_contains != 0 {
            marina_bonus += 30;
        }

        let marina_score = candidate_marina_score + marina_bonus;
        if marina_score > best_marina_score {
            best_marina_score = marina_score;
            selected_marina_index = marina_index;
        }

        marina_index += 1;
    }

    let mut selected_marina_ptr = promenade_value.as_ptr();
    let mut selected_marina_written = promenade_value_written;
    if selected_marina_index == 1 {
        selected_marina_ptr = sunrise_value.as_ptr();
        selected_marina_written = sunrise_value_written;
    } else if selected_marina_index == 2 {
        selected_marina_ptr = jetty_value.as_ptr();
        selected_marina_written = jetty_value_written;
    }

    let plaza_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_marina_ptr,
            selected_marina_written as i64,
            plaza_old.as_ptr(),
            plaza_old.len() as i64,
            plaza_new.as_ptr(),
            plaza_new.len() as i64,
        )
    };
    let mut plaza_value = vec![0u8; plaza_value_len as usize];
    let plaza_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_marina_ptr,
            selected_marina_written as i64,
            plaza_old.as_ptr(),
            plaza_old.len() as i64,
            plaza_new.as_ptr(),
            plaza_new.len() as i64,
            plaza_value.as_mut_ptr(),
            plaza_value.len() as i64,
        )
    };
    let plaza_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            plaza_value.as_ptr(),
            plaza_value_written as i64,
            plaza_needle.as_ptr(),
            plaza_needle.len() as i64,
        )
    };

    let solstice_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_marina_ptr,
            selected_marina_written as i64,
            solstice_extension.as_ptr(),
            solstice_extension.len() as i64,
        )
    };
    let mut solstice_value = vec![0u8; solstice_value_len as usize];
    let solstice_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_marina_ptr,
            selected_marina_written as i64,
            solstice_extension.as_ptr(),
            solstice_extension.len() as i64,
            solstice_value.as_mut_ptr(),
            solstice_value.len() as i64,
        )
    };
    let solstice_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            solstice_value.as_ptr(),
            solstice_value_written as i64,
            solstice_needle.as_ptr(),
            solstice_needle.len() as i64,
        )
    };

    let wharf_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_marina_ptr,
            selected_marina_written as i64,
        )
    };
    let mut wharf_source = vec![0u8; wharf_source_len as usize];
    let wharf_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_marina_ptr,
            selected_marina_written as i64,
            wharf_source.as_mut_ptr(),
            wharf_source.len() as i64,
        )
    };
    let wharf_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            wharf_source.as_ptr(),
            wharf_source_written as i64,
            wharf_old.as_ptr(),
            wharf_old.len() as i64,
            wharf_new.as_ptr(),
            wharf_new.len() as i64,
        )
    };
    let mut wharf_value = vec![0u8; wharf_value_len as usize];
    let wharf_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            wharf_source.as_ptr(),
            wharf_source_written as i64,
            wharf_old.as_ptr(),
            wharf_old.len() as i64,
            wharf_new.as_ptr(),
            wharf_new.len() as i64,
            wharf_value.as_mut_ptr(),
            wharf_value.len() as i64,
        )
    };
    let wharf_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            wharf_value.as_ptr(),
            wharf_value_written as i64,
            wharf_needle.as_ptr(),
            wharf_needle.len() as i64,
        )
    };

    let mut selected_pierhead_index = 0i32;
    let mut best_pierhead_score = i32::MIN;
    let mut pierhead_index = 0i32;
    while pierhead_index < 3 {
        let mut candidate_pierhead_score = plaza_value_written * 10 + plaza_contains * 50;
        if pierhead_index == 1 {
            candidate_pierhead_score = solstice_value_written * 10 + solstice_index;
        } else if pierhead_index == 2 {
            candidate_pierhead_score = wharf_value_written * 10 + wharf_contains * 50;
        }

        let mut pierhead_bonus = 0i32;
        if pierhead_index == selected_marina_index {
            pierhead_bonus += 25;
        }
        if pierhead_index == selected_lantern_index {
            pierhead_bonus += 15;
        }
        if pierhead_index == selected_channel_index {
            pierhead_bonus += 5;
        }
        if pierhead_index == 0 && plaza_contains != 0 {
            pierhead_bonus += 20;
        }
        if pierhead_index == 1 && solstice_index >= 0 {
            pierhead_bonus += 10;
        }
        if pierhead_index == 2 && wharf_contains != 0 {
            pierhead_bonus += 30;
        }

        let pierhead_score = candidate_pierhead_score + pierhead_bonus;
        if pierhead_score > best_pierhead_score {
            best_pierhead_score = pierhead_score;
            selected_pierhead_index = pierhead_index;
        }

        pierhead_index += 1;
    }

    let mut selected_pierhead_ptr = plaza_value.as_ptr();
    let mut selected_pierhead_written = plaza_value_written;
    if selected_pierhead_index == 1 {
        selected_pierhead_ptr = solstice_value.as_ptr();
        selected_pierhead_written = solstice_value_written;
    } else if selected_pierhead_index == 2 {
        selected_pierhead_ptr = wharf_value.as_ptr();
        selected_pierhead_written = wharf_value_written;
    }

    let arcade_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_pierhead_ptr,
            selected_pierhead_written as i64,
            arcade_old.as_ptr(),
            arcade_old.len() as i64,
            arcade_new.as_ptr(),
            arcade_new.len() as i64,
        )
    };
    let mut arcade_value = vec![0u8; arcade_value_len as usize];
    let arcade_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_pierhead_ptr,
            selected_pierhead_written as i64,
            arcade_old.as_ptr(),
            arcade_old.len() as i64,
            arcade_new.as_ptr(),
            arcade_new.len() as i64,
            arcade_value.as_mut_ptr(),
            arcade_value.len() as i64,
        )
    };
    let arcade_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            arcade_value.as_ptr(),
            arcade_value_written as i64,
            arcade_needle.as_ptr(),
            arcade_needle.len() as i64,
        )
    };

    let eclipse_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_pierhead_ptr,
            selected_pierhead_written as i64,
            eclipse_extension.as_ptr(),
            eclipse_extension.len() as i64,
        )
    };
    let mut eclipse_value = vec![0u8; eclipse_value_len as usize];
    let eclipse_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_pierhead_ptr,
            selected_pierhead_written as i64,
            eclipse_extension.as_ptr(),
            eclipse_extension.len() as i64,
            eclipse_value.as_mut_ptr(),
            eclipse_value.len() as i64,
        )
    };
    let eclipse_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            eclipse_value.as_ptr(),
            eclipse_value_written as i64,
            eclipse_needle.as_ptr(),
            eclipse_needle.len() as i64,
        )
    };

    let quay_source_three_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_pierhead_ptr,
            selected_pierhead_written as i64,
        )
    };
    let mut quay_source_three = vec![0u8; quay_source_three_len as usize];
    let quay_source_three_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_pierhead_ptr,
            selected_pierhead_written as i64,
            quay_source_three.as_mut_ptr(),
            quay_source_three.len() as i64,
        )
    };
    let quay_value_three_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            quay_source_three.as_ptr(),
            quay_source_three_written as i64,
            quay_old_three.as_ptr(),
            quay_old_three.len() as i64,
            quay_new_three.as_ptr(),
            quay_new_three.len() as i64,
        )
    };
    let mut quay_value_three = vec![0u8; quay_value_three_len as usize];
    let quay_value_three_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            quay_source_three.as_ptr(),
            quay_source_three_written as i64,
            quay_old_three.as_ptr(),
            quay_old_three.len() as i64,
            quay_new_three.as_ptr(),
            quay_new_three.len() as i64,
            quay_value_three.as_mut_ptr(),
            quay_value_three.len() as i64,
        )
    };
    let quay_contains_three = unsafe {
        rust_mcil_dotnet_string_contains(
            quay_value_three.as_ptr(),
            quay_value_three_written as i64,
            quay_needle_three.as_ptr(),
            quay_needle_three.len() as i64,
        )
    };

    let mut selected_outlook_index = 0i32;
    let mut best_outlook_score = i32::MIN;
    let mut outlook_index = 0i32;
    while outlook_index < 3 {
        let mut candidate_outlook_score = arcade_value_written * 10 + arcade_contains * 50;
        if outlook_index == 1 {
            candidate_outlook_score = eclipse_value_written * 10 + eclipse_index;
        } else if outlook_index == 2 {
            candidate_outlook_score = quay_value_three_written * 10 + quay_contains_three * 50;
        }

        let mut outlook_bonus = 0i32;
        if outlook_index == selected_pierhead_index {
            outlook_bonus += 25;
        }
        if outlook_index == selected_marina_index {
            outlook_bonus += 15;
        }
        if outlook_index == selected_lantern_index {
            outlook_bonus += 5;
        }
        if outlook_index == 0 && arcade_contains != 0 {
            outlook_bonus += 20;
        }
        if outlook_index == 1 && eclipse_index >= 0 {
            outlook_bonus += 10;
        }
        if outlook_index == 2 && quay_contains_three != 0 {
            outlook_bonus += 30;
        }

        let outlook_score = candidate_outlook_score + outlook_bonus;
        if outlook_score > best_outlook_score {
            best_outlook_score = outlook_score;
            selected_outlook_index = outlook_index;
        }

        outlook_index += 1;
    }

    let mut selected_outlook_ptr = arcade_value.as_ptr();
    let mut selected_outlook_written = arcade_value_written;
    if selected_outlook_index == 1 {
        selected_outlook_ptr = eclipse_value.as_ptr();
        selected_outlook_written = eclipse_value_written;
    } else if selected_outlook_index == 2 {
        selected_outlook_ptr = quay_value_three.as_ptr();
        selected_outlook_written = quay_value_three_written;
    }

    let gallery_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_outlook_ptr,
            selected_outlook_written as i64,
            gallery_old.as_ptr(),
            gallery_old.len() as i64,
            gallery_new.as_ptr(),
            gallery_new.len() as i64,
        )
    };
    let mut gallery_value = vec![0u8; gallery_value_len as usize];
    let gallery_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_outlook_ptr,
            selected_outlook_written as i64,
            gallery_old.as_ptr(),
            gallery_old.len() as i64,
            gallery_new.as_ptr(),
            gallery_new.len() as i64,
            gallery_value.as_mut_ptr(),
            gallery_value.len() as i64,
        )
    };
    let gallery_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            gallery_value.as_ptr(),
            gallery_value_written as i64,
            gallery_needle.as_ptr(),
            gallery_needle.len() as i64,
        )
    };

    let equinox_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_outlook_ptr,
            selected_outlook_written as i64,
            equinox_extension.as_ptr(),
            equinox_extension.len() as i64,
        )
    };
    let mut equinox_value = vec![0u8; equinox_value_len as usize];
    let equinox_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_outlook_ptr,
            selected_outlook_written as i64,
            equinox_extension.as_ptr(),
            equinox_extension.len() as i64,
            equinox_value.as_mut_ptr(),
            equinox_value.len() as i64,
        )
    };
    let equinox_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            equinox_value.as_ptr(),
            equinox_value_written as i64,
            equinox_needle.as_ptr(),
            equinox_needle.len() as i64,
        )
    };

    let berth_source_three_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_outlook_ptr,
            selected_outlook_written as i64,
        )
    };
    let mut berth_source_three = vec![0u8; berth_source_three_len as usize];
    let berth_source_three_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_outlook_ptr,
            selected_outlook_written as i64,
            berth_source_three.as_mut_ptr(),
            berth_source_three.len() as i64,
        )
    };
    let berth_value_three_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            berth_source_three.as_ptr(),
            berth_source_three_written as i64,
            berth_old_three.as_ptr(),
            berth_old_three.len() as i64,
            berth_new_three.as_ptr(),
            berth_new_three.len() as i64,
        )
    };
    let mut berth_value_three = vec![0u8; berth_value_three_len as usize];
    let berth_value_three_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            berth_source_three.as_ptr(),
            berth_source_three_written as i64,
            berth_old_three.as_ptr(),
            berth_old_three.len() as i64,
            berth_new_three.as_ptr(),
            berth_new_three.len() as i64,
            berth_value_three.as_mut_ptr(),
            berth_value_three.len() as i64,
        )
    };
    let berth_contains_three = unsafe {
        rust_mcil_dotnet_string_contains(
            berth_value_three.as_ptr(),
            berth_value_three_written as i64,
            berth_needle_three.as_ptr(),
            berth_needle_three.len() as i64,
        )
    };

    let mut selected_overlook_index = 0i32;
    let mut best_overlook_score = i32::MIN;
    let mut overlook_index = 0i32;
    while overlook_index < 3 {
        let mut candidate_overlook_score = gallery_value_written * 10 + gallery_contains * 50;
        if overlook_index == 1 {
            candidate_overlook_score = equinox_value_written * 10 + equinox_index;
        } else if overlook_index == 2 {
            candidate_overlook_score = berth_value_three_written * 10 + berth_contains_three * 50;
        }

        let mut overlook_bonus = 0i32;
        if overlook_index == selected_outlook_index {
            overlook_bonus += 25;
        }
        if overlook_index == selected_pierhead_index {
            overlook_bonus += 15;
        }
        if overlook_index == selected_marina_index {
            overlook_bonus += 5;
        }
        if overlook_index == 0 && gallery_contains != 0 {
            overlook_bonus += 20;
        }
        if overlook_index == 1 && equinox_index >= 0 {
            overlook_bonus += 10;
        }
        if overlook_index == 2 && berth_contains_three != 0 {
            overlook_bonus += 30;
        }

        let overlook_score = candidate_overlook_score + overlook_bonus;
        if overlook_score > best_overlook_score {
            best_overlook_score = overlook_score;
            selected_overlook_index = overlook_index;
        }

        overlook_index += 1;
    }

    let mut selected_overlook_ptr = gallery_value.as_ptr();
    let mut selected_overlook_written = gallery_value_written;
    if selected_overlook_index == 1 {
        selected_overlook_ptr = equinox_value.as_ptr();
        selected_overlook_written = equinox_value_written;
    } else if selected_overlook_index == 2 {
        selected_overlook_ptr = berth_value_three.as_ptr();
        selected_overlook_written = berth_value_three_written;
    }

    let observatory_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_overlook_ptr,
            selected_overlook_written as i64,
            observatory_old.as_ptr(),
            observatory_old.len() as i64,
            observatory_new.as_ptr(),
            observatory_new.len() as i64,
        )
    };
    let mut observatory_value = vec![0u8; observatory_value_len as usize];
    let observatory_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_overlook_ptr,
            selected_overlook_written as i64,
            observatory_old.as_ptr(),
            observatory_old.len() as i64,
            observatory_new.as_ptr(),
            observatory_new.len() as i64,
            observatory_value.as_mut_ptr(),
            observatory_value.len() as i64,
        )
    };
    let observatory_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            observatory_value.as_ptr(),
            observatory_value_written as i64,
            observatory_needle.as_ptr(),
            observatory_needle.len() as i64,
        )
    };

    let zenith_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_overlook_ptr,
            selected_overlook_written as i64,
            zenith_extension.as_ptr(),
            zenith_extension.len() as i64,
        )
    };
    let mut zenith_value = vec![0u8; zenith_value_len as usize];
    let zenith_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_overlook_ptr,
            selected_overlook_written as i64,
            zenith_extension.as_ptr(),
            zenith_extension.len() as i64,
            zenith_value.as_mut_ptr(),
            zenith_value.len() as i64,
        )
    };
    let zenith_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            zenith_value.as_ptr(),
            zenith_value_written as i64,
            zenith_needle.as_ptr(),
            zenith_needle.len() as i64,
        )
    };

    let harbor_source_four_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_overlook_ptr,
            selected_overlook_written as i64,
        )
    };
    let mut harbor_source_four = vec![0u8; harbor_source_four_len as usize];
    let harbor_source_four_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_overlook_ptr,
            selected_overlook_written as i64,
            harbor_source_four.as_mut_ptr(),
            harbor_source_four.len() as i64,
        )
    };
    let harbor_value_four_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            harbor_source_four.as_ptr(),
            harbor_source_four_written as i64,
            harbor_old_four.as_ptr(),
            harbor_old_four.len() as i64,
            harbor_new_four.as_ptr(),
            harbor_new_four.len() as i64,
        )
    };
    let mut harbor_value_four = vec![0u8; harbor_value_four_len as usize];
    let harbor_value_four_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            harbor_source_four.as_ptr(),
            harbor_source_four_written as i64,
            harbor_old_four.as_ptr(),
            harbor_old_four.len() as i64,
            harbor_new_four.as_ptr(),
            harbor_new_four.len() as i64,
            harbor_value_four.as_mut_ptr(),
            harbor_value_four.len() as i64,
        )
    };
    let harbor_contains_four = unsafe {
        rust_mcil_dotnet_string_contains(
            harbor_value_four.as_ptr(),
            harbor_value_four_written as i64,
            harbor_needle_four.as_ptr(),
            harbor_needle_four.len() as i64,
        )
    };

    let mut selected_terrace_index = 0i32;
    let mut best_terrace_score = i32::MIN;
    let mut terrace_index = 0i32;
    while terrace_index < 3 {
        let mut candidate_terrace_score = observatory_value_written * 10 + observatory_contains * 50;
        if terrace_index == 1 {
            candidate_terrace_score = zenith_value_written * 10 + zenith_index;
        } else if terrace_index == 2 {
            candidate_terrace_score = harbor_value_four_written * 10 + harbor_contains_four * 50;
        }

        let mut terrace_bonus = 0i32;
        if terrace_index == selected_overlook_index {
            terrace_bonus += 25;
        }
        if terrace_index == selected_outlook_index {
            terrace_bonus += 15;
        }
        if terrace_index == selected_pierhead_index {
            terrace_bonus += 5;
        }
        if terrace_index == 0 && observatory_contains != 0 {
            terrace_bonus += 20;
        }
        if terrace_index == 1 && zenith_index >= 0 {
            terrace_bonus += 10;
        }
        if terrace_index == 2 && harbor_contains_four != 0 {
            terrace_bonus += 30;
        }

        let terrace_score = candidate_terrace_score + terrace_bonus;
        if terrace_score > best_terrace_score {
            best_terrace_score = terrace_score;
            selected_terrace_index = terrace_index;
        }

        terrace_index += 1;
    }

    let mut selected_terrace_ptr = observatory_value.as_ptr();
    let mut selected_terrace_written = observatory_value_written;
    if selected_terrace_index == 1 {
        selected_terrace_ptr = zenith_value.as_ptr();
        selected_terrace_written = zenith_value_written;
    } else if selected_terrace_index == 2 {
        selected_terrace_ptr = harbor_value_four.as_ptr();
        selected_terrace_written = harbor_value_four_written;
    }

    let parapet_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_terrace_ptr,
            selected_terrace_written as i64,
            parapet_old.as_ptr(),
            parapet_old.len() as i64,
            parapet_new.as_ptr(),
            parapet_new.len() as i64,
        )
    };
    let mut parapet_value = vec![0u8; parapet_value_len as usize];
    let parapet_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_terrace_ptr,
            selected_terrace_written as i64,
            parapet_old.as_ptr(),
            parapet_old.len() as i64,
            parapet_new.as_ptr(),
            parapet_new.len() as i64,
            parapet_value.as_mut_ptr(),
            parapet_value.len() as i64,
        )
    };
    let parapet_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            parapet_value.as_ptr(),
            parapet_value_written as i64,
            parapet_needle.as_ptr(),
            parapet_needle.len() as i64,
        )
    };

    let twilight_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_terrace_ptr,
            selected_terrace_written as i64,
            twilight_extension.as_ptr(),
            twilight_extension.len() as i64,
        )
    };
    let mut twilight_value = vec![0u8; twilight_value_len as usize];
    let twilight_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_terrace_ptr,
            selected_terrace_written as i64,
            twilight_extension.as_ptr(),
            twilight_extension.len() as i64,
            twilight_value.as_mut_ptr(),
            twilight_value.len() as i64,
        )
    };
    let twilight_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            twilight_value.as_ptr(),
            twilight_value_written as i64,
            twilight_needle.as_ptr(),
            twilight_needle.len() as i64,
        )
    };

    let mooring_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_terrace_ptr,
            selected_terrace_written as i64,
        )
    };
    let mut mooring_source = vec![0u8; mooring_source_len as usize];
    let mooring_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_terrace_ptr,
            selected_terrace_written as i64,
            mooring_source.as_mut_ptr(),
            mooring_source.len() as i64,
        )
    };
    let mooring_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            mooring_source.as_ptr(),
            mooring_source_written as i64,
            mooring_old.as_ptr(),
            mooring_old.len() as i64,
            mooring_new.as_ptr(),
            mooring_new.len() as i64,
        )
    };
    let mut mooring_value = vec![0u8; mooring_value_len as usize];
    let mooring_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            mooring_source.as_ptr(),
            mooring_source_written as i64,
            mooring_old.as_ptr(),
            mooring_old.len() as i64,
            mooring_new.as_ptr(),
            mooring_new.len() as i64,
            mooring_value.as_mut_ptr(),
            mooring_value.len() as i64,
        )
    };
    let mooring_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            mooring_value.as_ptr(),
            mooring_value_written as i64,
            mooring_needle.as_ptr(),
            mooring_needle.len() as i64,
        )
    };

    let mut selected_summit_index = 0i32;
    let mut best_summit_score = i32::MIN;
    let mut summit_index = 0i32;
    while summit_index < 3 {
        let mut candidate_summit_score = parapet_value_written * 10 + parapet_contains * 50;
        if summit_index == 1 {
            candidate_summit_score = twilight_value_written * 10 + twilight_index;
        } else if summit_index == 2 {
            candidate_summit_score = mooring_value_written * 10 + mooring_contains * 50;
        }

        let mut summit_bonus = 0i32;
        if summit_index == selected_terrace_index {
            summit_bonus += 25;
        }
        if summit_index == selected_overlook_index {
            summit_bonus += 15;
        }
        if summit_index == selected_outlook_index {
            summit_bonus += 5;
        }
        if summit_index == 0 && parapet_contains != 0 {
            summit_bonus += 20;
        }
        if summit_index == 1 && twilight_index >= 0 {
            summit_bonus += 10;
        }
        if summit_index == 2 && mooring_contains != 0 {
            summit_bonus += 30;
        }

        let summit_score = candidate_summit_score + summit_bonus;
        if summit_score > best_summit_score {
            best_summit_score = summit_score;
            selected_summit_index = summit_index;
        }

        summit_index += 1;
    }

    let mut selected_summit_ptr = parapet_value.as_ptr();
    let mut selected_summit_written = parapet_value_written;
    if selected_summit_index == 1 {
        selected_summit_ptr = twilight_value.as_ptr();
        selected_summit_written = twilight_value_written;
    } else if selected_summit_index == 2 {
        selected_summit_ptr = mooring_value.as_ptr();
        selected_summit_written = mooring_value_written;
    }

    let skywalk_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_summit_ptr,
            selected_summit_written as i64,
            skywalk_old.as_ptr(),
            skywalk_old.len() as i64,
            skywalk_new.as_ptr(),
            skywalk_new.len() as i64,
        )
    };
    let mut skywalk_value = vec![0u8; skywalk_value_len as usize];
    let skywalk_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_summit_ptr,
            selected_summit_written as i64,
            skywalk_old.as_ptr(),
            skywalk_old.len() as i64,
            skywalk_new.as_ptr(),
            skywalk_new.len() as i64,
            skywalk_value.as_mut_ptr(),
            skywalk_value.len() as i64,
        )
    };
    let skywalk_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            skywalk_value.as_ptr(),
            skywalk_value_written as i64,
            skywalk_needle.as_ptr(),
            skywalk_needle.len() as i64,
        )
    };

    let starglow_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_summit_ptr,
            selected_summit_written as i64,
            starglow_extension.as_ptr(),
            starglow_extension.len() as i64,
        )
    };
    let mut starglow_value = vec![0u8; starglow_value_len as usize];
    let starglow_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_summit_ptr,
            selected_summit_written as i64,
            starglow_extension.as_ptr(),
            starglow_extension.len() as i64,
            starglow_value.as_mut_ptr(),
            starglow_value.len() as i64,
        )
    };
    let starglow_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            starglow_value.as_ptr(),
            starglow_value_written as i64,
            starglow_needle.as_ptr(),
            starglow_needle.len() as i64,
        )
    };

    let dock_source_four_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_summit_ptr,
            selected_summit_written as i64,
        )
    };
    let mut dock_source_four = vec![0u8; dock_source_four_len as usize];
    let dock_source_four_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_summit_ptr,
            selected_summit_written as i64,
            dock_source_four.as_mut_ptr(),
            dock_source_four.len() as i64,
        )
    };
    let dock_value_four_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            dock_source_four.as_ptr(),
            dock_source_four_written as i64,
            dock_old_four.as_ptr(),
            dock_old_four.len() as i64,
            dock_new_four.as_ptr(),
            dock_new_four.len() as i64,
        )
    };
    let mut dock_value_four = vec![0u8; dock_value_four_len as usize];
    let dock_value_four_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            dock_source_four.as_ptr(),
            dock_source_four_written as i64,
            dock_old_four.as_ptr(),
            dock_old_four.len() as i64,
            dock_new_four.as_ptr(),
            dock_new_four.len() as i64,
            dock_value_four.as_mut_ptr(),
            dock_value_four.len() as i64,
        )
    };
    let dock_contains_four = unsafe {
        rust_mcil_dotnet_string_contains(
            dock_value_four.as_ptr(),
            dock_value_four_written as i64,
            dock_needle_four.as_ptr(),
            dock_needle_four.len() as i64,
        )
    };

    let mut selected_apex_index = 0i32;
    let mut best_apex_score = i32::MIN;
    let mut apex_index = 0i32;
    while apex_index < 3 {
        let mut candidate_apex_score = skywalk_value_written * 10 + skywalk_contains * 50;
        if apex_index == 1 {
            candidate_apex_score = starglow_value_written * 10 + starglow_index;
        } else if apex_index == 2 {
            candidate_apex_score = dock_value_four_written * 10 + dock_contains_four * 50;
        }

        let mut apex_bonus = 0i32;
        if apex_index == selected_summit_index {
            apex_bonus += 25;
        }
        if apex_index == selected_terrace_index {
            apex_bonus += 15;
        }
        if apex_index == selected_overlook_index {
            apex_bonus += 5;
        }
        if apex_index == 0 && skywalk_contains != 0 {
            apex_bonus += 20;
        }
        if apex_index == 1 && starglow_index >= 0 {
            apex_bonus += 10;
        }
        if apex_index == 2 && dock_contains_four != 0 {
            apex_bonus += 30;
        }

        let apex_score = candidate_apex_score + apex_bonus;
        if apex_score > best_apex_score {
            best_apex_score = apex_score;
            selected_apex_index = apex_index;
        }

        apex_index += 1;
    }

    (selected_directory_index + 1) * 100000000
        + (selected_file_index + 1) * 10000000
        + (selected_variant_index + 1) * 1000000
        + (selected_rebase_index + 1) * 100000
        + (selected_leaf_transform_index + 1) * 10000
        + (selected_path_transform_index + 1) * 1000
        + (selected_recomposition_index + 1) * 100
        + (selected_summit_index + 1) * 10
        + (selected_apex_index + 1)
}