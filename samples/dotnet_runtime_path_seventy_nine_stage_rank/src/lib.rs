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
pub extern "C" fn dotnet_runtime_path_seventy_nine_stage_rank_score() -> i32 {
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
    let spire_old = "skywalk";
    let spire_new = "spiredeck";
    let spire_needle = "spiredeck";
    let sunburst_extension = ".sunburst";
    let sunburst_needle = "sunburst";
    let anchor_old = "dockwatch";
    let anchor_new = "anchorpoint";
    let anchor_needle = "anchorpoint";
    let citadel_old = "spiredeck";
    let citadel_new = "citadel";
    let citadel_needle = "citadel";
    let moonbeam_extension = ".moonbeam";
    let moonbeam_needle = "moonbeam";
    let keel_old = "anchorpoint";
    let keel_new = "keelguard";
    let keel_needle = "keelguard";
    let belfry_old = "citadel";
    let belfry_new = "belfry";
    let belfry_needle = "belfry";
    let aurora_extension = ".aurora";
    let aurora_needle = "aurora";
    let rudder_old = "keelguard";
    let rudder_new = "rudderline";
    let rudder_needle = "rudderline";
    let lantern_old = "belfry";
    let lantern_new = "lanternway";
    let lantern_needle = "lanternway";
    let daybreak_extension = ".daybreak";
    let daybreak_needle = "daybreak";
    let oar_old = "rudderline";
    let oar_new = "oarcrest";
    let oar_needle = "oarcrest";
    let beacon_old = "lanternway";
    let beacon_new = "beaconhall";
    let beacon_needle = "beaconhall";
    let sunrise_extension = ".sunrise";
    let sunrise_needle = "sunrise";
    let mast_old = "oarcrest";
    let mast_new = "mastline";
    let mast_needle = "mastline";
    let signal_old = "beaconhall";
    let signal_new = "signalpost";
    let signal_needle = "signalpost";
    let sundial_extension = ".sundial";
    let sundial_needle = "sundial";
    let spar_old = "mastline";
    let spar_new = "sparwatch";
    let spar_needle = "sparwatch";
    let pennant_old = "signalpost";
    let pennant_new = "pennantgate";
    let pennant_needle = "pennantgate";
    let solstice_extension = ".solstice";
    let solstice_needle = "solstice";
    let boom_old = "sparwatch";
    let boom_new = "boomrail";
    let boom_needle = "boomrail";
    let heliostat_old = "pennantgate";
    let heliostat_new = "heliostat";
    let heliostat_needle = "heliostat";
    let equinox_extension = ".equinox";
    let equinox_needle = "equinox";
    let davit_old = "boomrail";
    let davit_new = "davitline";
    let davit_needle = "davitline";
    let sunlattice_old = "heliostat";
    let sunlattice_new = "sunlattice";
    let sunlattice_needle = "sunlattice";
    let daystar_extension = ".daystar";
    let daystar_needle = "daystar";
    let yard_old = "davitline";
    let yard_new = "yardarm";
    let yard_needle = "yardarm";
    let orrery_old = "sunlattice";
    let orrery_new = "orrerygate";
    let orrery_needle = "orrerygate";
    let dusk_extension = ".duskfire";
    let dusk_needle = "duskfire";
    let gaff_old = "yardarm";
    let gaff_new = "gaffline";
    let gaff_needle = "gaffline";
    let astrolabe_old = "orrerygate";
    let astrolabe_new = "astrolabe";
    let astrolabe_needle = "astrolabe";
    let twilight_extension = ".twinfire";
    let twilight_needle = "twinfire";
    let brace_old = "gaffline";
    let brace_new = "braceway";
    let brace_needle = "braceway";
    let sextant_old = "astrolabe";
    let sextant_new = "sextant";
    let sextant_needle = "sextant";
    let eventide_extension = ".eventide";
    let eventide_needle = "eventide";
    let halyard_old = "braceway";
    let halyard_new = "halyard";
    let halyard_needle = "halyard";
    let armillary_old = "sextant";
    let armillary_new = "armillary";
    let armillary_needle = "armillary";
    let midwatch_extension = ".midwatch";
    let midwatch_needle = "midwatch";
    let sheet_old = "halyard";
    let sheet_new = "sheetline";
    let sheet_needle = "sheetline";
    let tellurion_old = "armillary";
    let tellurion_new = "tellurion";
    let tellurion_needle = "tellurion";
    let starlit_extension = ".starlit";
    let starlit_needle = "starlit";
    let clew_old = "sheetline";
    let clew_new = "clewline";
    let clew_needle = "clewline";
    let planisphere_old = "tellurion";
    let planisphere_new = "planisphere";
    let planisphere_needle = "planisphere";
    let nightglass_extension = ".nightglass";
    let nightglass_needle = "nightglass";
    let footrope_old = "clewline";
    let footrope_new = "footrope";
    let footrope_needle = "footrope";
    let starwheel_old = "planisphere";
    let starwheel_new = "starwheel";
    let starwheel_needle = "starwheel";
    let moonwake_extension = ".moonwake";
    let moonwake_needle = "moonwake";
    let ratline_old = "footrope";
    let ratline_new = "ratline";
    let ratline_needle = "ratline";
    let planetarium_old = "starwheel";
    let planetarium_new = "planetarium";
    let planetarium_needle = "planetarium";
    let dayring_extension = ".dayring";
    let dayring_needle = "dayring";
    let shroud_old = "ratline";
    let shroud_new = "shroudline";
    let shroud_needle = "shroudline";
    let orrery_old = "planetarium";
    let orrery_new = "orrery";
    let orrery_needle = "orrery";
    let sunwake_extension = ".sunwake";
    let sunwake_needle = "sunwake";
    let backstay_old = "shroudline";
    let backstay_new = "backstay";
    let backstay_needle = "backstay";
    let zenith_old = "orrery";
    let zenith_new = "zenith";
    let zenith_needle = "zenith";
    let solstice_extension = ".solstice";
    let solstice_needle = "solstice";
    let forestay_old = "backstay";
    let forestay_new = "forestay";
    let forestay_needle = "forestay";
    let azimuth_old = "zenith";
    let azimuth_new = "azimuth";
    let azimuth_needle = "azimuth";
    let perihelion_extension = ".perihelion";
    let perihelion_needle = "perihelion";
    let stayline_old = "forestay";
    let stayline_new = "stayline";
    let stayline_needle = "stayline";
    let sidereal_old = "azimuth";
    let sidereal_new = "sidereal";
    let sidereal_needle = "sidereal";
    let chronwake_extension = ".chronwake";
    let chronwake_needle = "chronwake";
    let leechline_old = "stayline";
    let leechline_new = "leechline";
    let leechline_needle = "leechline";
    let ephemeris_old = "sidereal";
    let ephemeris_new = "ephemeris";
    let ephemeris_needle = "ephemeris";
    let tidewake_extension = ".tidewake";
    let tidewake_needle = "tidewake";
    let sheetbend_old = "leechline";
    let sheetbend_new = "sheetbend";
    let sheetbend_needle = "sheetbend";
    let almanac_old = "ephemeris";
    let almanac_new = "almanac";
    let almanac_needle = "almanac";
    let starwake_extension = ".starwake";
    let starwake_needle = "starwake";
    let reefline_old = "sheetbend";
    let reefline_new = "reefline";
    let reefline_needle = "reefline";
    let nocturne_old = "almanac";
    let nocturne_new = "nocturne";
    let nocturne_needle = "nocturne";
    let dawnwake_extension = ".dawnwake";
    let dawnwake_needle = "dawnwake";
    let luffline_old = "reefline";
    let luffline_new = "luffline";
    let luffline_needle = "luffline";
    let solarium_old = "nocturne";
    let solarium_new = "solarium";
    let solarium_needle = "solarium";
    let emberwake_extension = ".emberwake";
    let emberwake_needle = "emberwake";
    let tackline_old = "luffline";
    let tackline_new = "tackline";
    let tackline_needle = "tackline";
    let aurorium_old = "solarium";
    let aurorium_new = "aurorium";
    let aurorium_needle = "aurorium";
    let glimmerwake_extension = ".glimmerwake";
    let glimmerwake_needle = "glimmerwake";
    let keelline_old = "tackline";
    let keelline_new = "keelline";
    let keelline_needle = "keelline";
    let prismarium_old = "aurorium";
    let prismarium_new = "prismarium";
    let prismarium_needle = "prismarium";
    let shimmerwake_extension = ".shimmerwake";
    let shimmerwake_needle = "shimmerwake";
    let plankline_old = "keelline";
    let plankline_new = "plankline";
    let plankline_needle = "plankline";
    let spectrarium_old = "prismarium";
    let spectrarium_new = "spectrarium";
    let spectrarium_needle = "spectrarium";
    let frostwake_extension = ".frostwake";
    let frostwake_needle = "frostwake";
    let ribline_old = "plankline";
    let ribline_new = "ribline";
    let ribline_needle = "ribline";
    let luminarium_old = "spectrarium";
    let luminarium_new = "luminarium";
    let luminarium_needle = "luminarium";
    let icewake_extension = ".icewake";
    let icewake_needle = "icewake";
    let coamline_old = "ribline";
    let coamline_new = "coamline";
    let coamline_needle = "coamline";
    let radiarium_old = "luminarium";
    let radiarium_new = "radiarium";
    let radiarium_needle = "radiarium";
    let glowwake_extension = ".glowwake";
    let glowwake_needle = "glowwake";
    let sparline_old = "coamline";
    let sparline_new = "sparline";
    let sparline_needle = "sparline";
    let coruscarium_old = "radiarium";
    let coruscarium_new = "coruscarium";
    let coruscarium_needle = "coruscarium";
    let flarewake_extension = ".flarewake";
    let flarewake_needle = "flarewake";
    let boomline_old = "sparline";
    let boomline_new = "boomline";
    let boomline_needle = "boomline";
    let caelarium_old = "coruscarium";
    let caelarium_new = "caelarium";
    let caelarium_needle = "caelarium";
    let mistwake_extension = ".mistwake";
    let mistwake_needle = "mistwake";
    let bulwark_old = "boomline";
    let bulwark_new = "bulwark";
    let bulwark_needle = "bulwark";
    let aetherium_old = "caelarium";
    let aetherium_new = "aetherium";
    let aetherium_needle = "aetherium";
    let cloudwake_extension = ".cloudwake";
    let cloudwake_needle = "cloudwake";
    let gunwale_old = "bulwark";
    let gunwale_new = "gunwale";
    let gunwale_needle = "gunwale";
    let nebularium_old = "aetherium";
    let nebularium_new = "nebularium";
    let nebularium_needle = "nebularium";
    let brightwake_extension = ".brightwake";
    let brightwake_needle = "brightwake";
    let sheerline_old = "gunwale";
    let sheerline_new = "sheerline";
    let sheerline_needle = "sheerline";
    let quintarium_old = "nebularium";
    let quintarium_new = "quintarium";
    let quintarium_needle = "quintarium";
    let silverwake_extension = ".silverwake";
    let silverwake_needle = "silverwake";
    let hullguard_old = "sheerline";
    let hullguard_new = "hullguard";
    let hullguard_needle = "hullguard";
    let heliarium_old = "quintarium";
    let heliarium_new = "heliarium";
    let heliarium_needle = "heliarium";
    let cinderwake_extension = ".cinderwake";
    let cinderwake_needle = "cinderwake";
    let deckbrace_old = "hullguard";
    let deckbrace_new = "deckbrace";
    let deckbrace_needle = "deckbrace";
    let zenithium_old = "heliarium";
    let zenithium_new = "zenithium";
    let zenithium_needle = "zenithium";
    let opalwake_extension = ".opalwake";
    let opalwake_needle = "opalwake";
    let sparbrace_old = "deckbrace";
    let sparbrace_new = "sparbrace";
    let sparbrace_needle = "sparbrace";
    let orinthium_old = "zenithium";
    let orinthium_new = "orinthium";
    let orinthium_needle = "orinthium";
    let sablewake_extension = ".sablewake";
    let sablewake_needle = "sablewake";
    let ribbrace_old = "sparbrace";
    let ribbrace_new = "ribbrace";
    let ribbrace_needle = "ribbrace";
    let ecliptium_old = "orinthium";
    let ecliptium_new = "ecliptium";
    let ecliptium_needle = "ecliptium";
    let amberwake_extension = ".amberwake";
    let amberwake_needle = "amberwake";
    let strutbrace_old = "ribbrace";
    let strutbrace_new = "strutbrace";
    let strutbrace_needle = "strutbrace";
    let aurelium_old = "ecliptium";
    let aurelium_new = "aurelium";
    let aurelium_needle = "aurelium";
    let copperwake_extension = ".copperwake";
    let copperwake_needle = "copperwake";
    let girderbrace_old = "strutbrace";
    let girderbrace_new = "girderbrace";
    let girderbrace_needle = "girderbrace";
    let celestium_old = "aurelium";
    let celestium_new = "celestium";
    let celestium_needle = "celestium";
    let brasswake_extension = ".brasswake";
    let brasswake_needle = "brasswake";
    let latticebrace_old = "girderbrace";
    let latticebrace_new = "latticebrace";
    let latticebrace_needle = "latticebrace";
    let obsidium_old = "celestium";
    let obsidium_new = "obsidium";
    let obsidium_needle = "obsidium";
    let tinwake_extension = ".tinwake";
    let tinwake_needle = "tinwake";
    let meshbrace_old = "latticebrace";
    let meshbrace_new = "meshbrace";
    let meshbrace_needle = "meshbrace";
    let meridium_old = "obsidium";
    let meridium_new = "meridium";
    let meridium_needle = "meridium";
    let zincwake_extension = ".zincwake";
    let zincwake_needle = "zincwake";
    let panelbrace_old = "meshbrace";
    let panelbrace_new = "panelbrace";
    let panelbrace_needle = "panelbrace";
    let verdantium_old = "meridium";
    let verdantium_new = "verdantium";
    let verdantium_needle = "verdantium";
    let nickelwake_extension = ".nickelwake";
    let nickelwake_needle = "nickelwake";
    let joistbrace_old = "panelbrace";
    let joistbrace_new = "joistbrace";
    let joistbrace_needle = "joistbrace";
    let lumitium_old = "verdantium";
    let lumitium_new = "lumitium";
    let lumitium_needle = "lumitium";
    let cobaltwake_extension = ".cobaltwake";
    let cobaltwake_needle = "cobaltwake";
    let rafterbrace_old = "joistbrace";
    let rafterbrace_new = "rafterbrace";
    let rafterbrace_needle = "rafterbrace";
    let prismatium_old = "lumitium";
    let prismatium_new = "prismatium";
    let prismatium_needle = "prismatium";
    let chromewake_extension = ".chromewake";
    let chromewake_needle = "chromewake";
    let beambrace_old = "rafterbrace";
    let beambrace_new = "beambrace";
    let beambrace_needle = "beambrace";
    let stellarium_old = "prismatium";
    let stellarium_new = "stellarium";
    let stellarium_needle = "stellarium";
    let titanwake_extension = ".titanwake";
    let titanwake_needle = "titanwake";
    let archbrace_old = "beambrace";
    let archbrace_new = "archbrace";
    let archbrace_needle = "archbrace";
    let novatium_old = "stellarium";
    let novatium_new = "novatium";
    let novatium_needle = "novatium";
    let platinumwake_extension = ".platinumwake";
    let platinumwake_needle = "platinumwake";
    let keelbrace_old = "archbrace";
    let keelbrace_new = "keelbrace";
    let keelbrace_needle = "keelbrace";
    let polarium_old = "novatium";
    let polarium_new = "polarium";
    let polarium_needle = "polarium";
    let osmiumwake_extension = ".osmiumwake";
    let osmiumwake_needle = "osmiumwake";
    let ridgebrace_old = "keelbrace";
    let ridgebrace_new = "ridgebrace";
    let ridgebrace_needle = "ridgebrace";

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

    let mut selected_apex_ptr = skywalk_value.as_ptr();
    let mut selected_apex_written = skywalk_value_written;
    if selected_apex_index == 1 {
        selected_apex_ptr = starglow_value.as_ptr();
        selected_apex_written = starglow_value_written;
    } else if selected_apex_index == 2 {
        selected_apex_ptr = dock_value_four.as_ptr();
        selected_apex_written = dock_value_four_written;
    }

    let spire_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_apex_ptr,
            selected_apex_written as i64,
            spire_old.as_ptr(),
            spire_old.len() as i64,
            spire_new.as_ptr(),
            spire_new.len() as i64,
        )
    };
    let mut spire_value = vec![0u8; spire_value_len as usize];
    let spire_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_apex_ptr,
            selected_apex_written as i64,
            spire_old.as_ptr(),
            spire_old.len() as i64,
            spire_new.as_ptr(),
            spire_new.len() as i64,
            spire_value.as_mut_ptr(),
            spire_value.len() as i64,
        )
    };
    let spire_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            spire_value.as_ptr(),
            spire_value_written as i64,
            spire_needle.as_ptr(),
            spire_needle.len() as i64,
        )
    };

    let sunburst_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_apex_ptr,
            selected_apex_written as i64,
            sunburst_extension.as_ptr(),
            sunburst_extension.len() as i64,
        )
    };
    let mut sunburst_value = vec![0u8; sunburst_value_len as usize];
    let sunburst_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_apex_ptr,
            selected_apex_written as i64,
            sunburst_extension.as_ptr(),
            sunburst_extension.len() as i64,
            sunburst_value.as_mut_ptr(),
            sunburst_value.len() as i64,
        )
    };
    let sunburst_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            sunburst_value.as_ptr(),
            sunburst_value_written as i64,
            sunburst_needle.as_ptr(),
            sunburst_needle.len() as i64,
        )
    };

    let anchor_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_apex_ptr,
            selected_apex_written as i64,
        )
    };
    let mut anchor_source = vec![0u8; anchor_source_len as usize];
    let anchor_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_apex_ptr,
            selected_apex_written as i64,
            anchor_source.as_mut_ptr(),
            anchor_source.len() as i64,
        )
    };
    let anchor_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            anchor_source.as_ptr(),
            anchor_source_written as i64,
            anchor_old.as_ptr(),
            anchor_old.len() as i64,
            anchor_new.as_ptr(),
            anchor_new.len() as i64,
        )
    };
    let mut anchor_value = vec![0u8; anchor_value_len as usize];
    let anchor_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            anchor_source.as_ptr(),
            anchor_source_written as i64,
            anchor_old.as_ptr(),
            anchor_old.len() as i64,
            anchor_new.as_ptr(),
            anchor_new.len() as i64,
            anchor_value.as_mut_ptr(),
            anchor_value.len() as i64,
        )
    };
    let anchor_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            anchor_value.as_ptr(),
            anchor_value_written as i64,
            anchor_needle.as_ptr(),
            anchor_needle.len() as i64,
        )
    };

    let mut selected_crown_index = 0i32;
    let mut best_crown_score = i32::MIN;
    let mut crown_index = 0i32;
    while crown_index < 3 {
        let mut candidate_crown_score = spire_value_written * 10 + spire_contains * 50;
        if crown_index == 1 {
            candidate_crown_score = sunburst_value_written * 10 + sunburst_index;
        } else if crown_index == 2 {
            candidate_crown_score = anchor_value_written * 10 + anchor_contains * 50;
        }

        let mut crown_bonus = 0i32;
        if crown_index == selected_apex_index {
            crown_bonus += 25;
        }
        if crown_index == selected_summit_index {
            crown_bonus += 15;
        }
        if crown_index == selected_terrace_index {
            crown_bonus += 5;
        }
        if crown_index == 0 && spire_contains != 0 {
            crown_bonus += 20;
        }
        if crown_index == 1 && sunburst_index >= 0 {
            crown_bonus += 10;
        }
        if crown_index == 2 && anchor_contains != 0 {
            crown_bonus += 30;
        }

        let crown_score = candidate_crown_score + crown_bonus;
        if crown_score > best_crown_score {
            best_crown_score = crown_score;
            selected_crown_index = crown_index;
        }

        crown_index += 1;
    }

    let mut selected_crown_ptr = spire_value.as_ptr();
    let mut selected_crown_written = spire_value_written;
    if selected_crown_index == 1 {
        selected_crown_ptr = sunburst_value.as_ptr();
        selected_crown_written = sunburst_value_written;
    } else if selected_crown_index == 2 {
        selected_crown_ptr = anchor_value.as_ptr();
        selected_crown_written = anchor_value_written;
    }

    let citadel_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_crown_ptr,
            selected_crown_written as i64,
            citadel_old.as_ptr(),
            citadel_old.len() as i64,
            citadel_new.as_ptr(),
            citadel_new.len() as i64,
        )
    };
    let mut citadel_value = vec![0u8; citadel_value_len as usize];
    let citadel_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_crown_ptr,
            selected_crown_written as i64,
            citadel_old.as_ptr(),
            citadel_old.len() as i64,
            citadel_new.as_ptr(),
            citadel_new.len() as i64,
            citadel_value.as_mut_ptr(),
            citadel_value.len() as i64,
        )
    };
    let citadel_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            citadel_value.as_ptr(),
            citadel_value_written as i64,
            citadel_needle.as_ptr(),
            citadel_needle.len() as i64,
        )
    };

    let moonbeam_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_crown_ptr,
            selected_crown_written as i64,
            moonbeam_extension.as_ptr(),
            moonbeam_extension.len() as i64,
        )
    };
    let mut moonbeam_value = vec![0u8; moonbeam_value_len as usize];
    let moonbeam_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_crown_ptr,
            selected_crown_written as i64,
            moonbeam_extension.as_ptr(),
            moonbeam_extension.len() as i64,
            moonbeam_value.as_mut_ptr(),
            moonbeam_value.len() as i64,
        )
    };
    let moonbeam_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            moonbeam_value.as_ptr(),
            moonbeam_value_written as i64,
            moonbeam_needle.as_ptr(),
            moonbeam_needle.len() as i64,
        )
    };

    let keel_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_crown_ptr,
            selected_crown_written as i64,
        )
    };
    let mut keel_source = vec![0u8; keel_source_len as usize];
    let keel_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_crown_ptr,
            selected_crown_written as i64,
            keel_source.as_mut_ptr(),
            keel_source.len() as i64,
        )
    };
    let keel_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            keel_source.as_ptr(),
            keel_source_written as i64,
            keel_old.as_ptr(),
            keel_old.len() as i64,
            keel_new.as_ptr(),
            keel_new.len() as i64,
        )
    };
    let mut keel_value = vec![0u8; keel_value_len as usize];
    let keel_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            keel_source.as_ptr(),
            keel_source_written as i64,
            keel_old.as_ptr(),
            keel_old.len() as i64,
            keel_new.as_ptr(),
            keel_new.len() as i64,
            keel_value.as_mut_ptr(),
            keel_value.len() as i64,
        )
    };
    let keel_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            keel_value.as_ptr(),
            keel_value_written as i64,
            keel_needle.as_ptr(),
            keel_needle.len() as i64,
        )
    };

    let mut selected_keystone_index = 0i32;
    let mut best_keystone_score = i32::MIN;
    let mut keystone_index = 0i32;
    while keystone_index < 3 {
        let mut candidate_keystone_score = citadel_value_written * 10 + citadel_contains * 50;
        if keystone_index == 1 {
            candidate_keystone_score = moonbeam_value_written * 10 + moonbeam_index;
        } else if keystone_index == 2 {
            candidate_keystone_score = keel_value_written * 10 + keel_contains * 50;
        }

        let mut keystone_bonus = 0i32;
        if keystone_index == selected_crown_index {
            keystone_bonus += 25;
        }
        if keystone_index == selected_apex_index {
            keystone_bonus += 15;
        }
        if keystone_index == selected_summit_index {
            keystone_bonus += 5;
        }
        if keystone_index == 0 && citadel_contains != 0 {
            keystone_bonus += 20;
        }
        if keystone_index == 1 && moonbeam_index >= 0 {
            keystone_bonus += 10;
        }
        if keystone_index == 2 && keel_contains != 0 {
            keystone_bonus += 30;
        }

        let keystone_score = candidate_keystone_score + keystone_bonus;
        if keystone_score > best_keystone_score {
            best_keystone_score = keystone_score;
            selected_keystone_index = keystone_index;
        }

        keystone_index += 1;
    }

    let mut selected_keystone_ptr = citadel_value.as_ptr();
    let mut selected_keystone_written = citadel_value_written;
    if selected_keystone_index == 1 {
        selected_keystone_ptr = moonbeam_value.as_ptr();
        selected_keystone_written = moonbeam_value_written;
    } else if selected_keystone_index == 2 {
        selected_keystone_ptr = keel_value.as_ptr();
        selected_keystone_written = keel_value_written;
    }

    let belfry_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_keystone_ptr,
            selected_keystone_written as i64,
            belfry_old.as_ptr(),
            belfry_old.len() as i64,
            belfry_new.as_ptr(),
            belfry_new.len() as i64,
        )
    };
    let mut belfry_value = vec![0u8; belfry_value_len as usize];
    let belfry_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_keystone_ptr,
            selected_keystone_written as i64,
            belfry_old.as_ptr(),
            belfry_old.len() as i64,
            belfry_new.as_ptr(),
            belfry_new.len() as i64,
            belfry_value.as_mut_ptr(),
            belfry_value.len() as i64,
        )
    };
    let belfry_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            belfry_value.as_ptr(),
            belfry_value_written as i64,
            belfry_needle.as_ptr(),
            belfry_needle.len() as i64,
        )
    };

    let aurora_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_keystone_ptr,
            selected_keystone_written as i64,
            aurora_extension.as_ptr(),
            aurora_extension.len() as i64,
        )
    };
    let mut aurora_value = vec![0u8; aurora_value_len as usize];
    let aurora_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_keystone_ptr,
            selected_keystone_written as i64,
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

    let rudder_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_keystone_ptr,
            selected_keystone_written as i64,
        )
    };
    let mut rudder_source = vec![0u8; rudder_source_len as usize];
    let rudder_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_keystone_ptr,
            selected_keystone_written as i64,
            rudder_source.as_mut_ptr(),
            rudder_source.len() as i64,
        )
    };
    let rudder_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            rudder_source.as_ptr(),
            rudder_source_written as i64,
            rudder_old.as_ptr(),
            rudder_old.len() as i64,
            rudder_new.as_ptr(),
            rudder_new.len() as i64,
        )
    };
    let mut rudder_value = vec![0u8; rudder_value_len as usize];
    let rudder_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            rudder_source.as_ptr(),
            rudder_source_written as i64,
            rudder_old.as_ptr(),
            rudder_old.len() as i64,
            rudder_new.as_ptr(),
            rudder_new.len() as i64,
            rudder_value.as_mut_ptr(),
            rudder_value.len() as i64,
        )
    };
    let rudder_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            rudder_value.as_ptr(),
            rudder_value_written as i64,
            rudder_needle.as_ptr(),
            rudder_needle.len() as i64,
        )
    };

    let mut selected_horizon_index = 0i32;
    let mut best_horizon_score = i32::MIN;
    let mut horizon_index = 0i32;
    while horizon_index < 3 {
        let mut candidate_horizon_score = belfry_value_written * 10 + belfry_contains * 50;
        if horizon_index == 1 {
            candidate_horizon_score = aurora_value_written * 10 + aurora_index;
        } else if horizon_index == 2 {
            candidate_horizon_score = rudder_value_written * 10 + rudder_contains * 50;
        }

        let mut horizon_bonus = 0i32;
        if horizon_index == selected_keystone_index {
            horizon_bonus += 25;
        }
        if horizon_index == selected_crown_index {
            horizon_bonus += 15;
        }
        if horizon_index == selected_apex_index {
            horizon_bonus += 5;
        }
        if horizon_index == 0 && belfry_contains != 0 {
            horizon_bonus += 20;
        }
        if horizon_index == 1 && aurora_index >= 0 {
            horizon_bonus += 10;
        }
        if horizon_index == 2 && rudder_contains != 0 {
            horizon_bonus += 30;
        }

        let horizon_score = candidate_horizon_score + horizon_bonus;
        if horizon_score > best_horizon_score {
            best_horizon_score = horizon_score;
            selected_horizon_index = horizon_index;
        }

        horizon_index += 1;
    }

    let mut selected_horizon_ptr = belfry_value.as_ptr();
    let mut selected_horizon_written = belfry_value_written;
    if selected_horizon_index == 1 {
        selected_horizon_ptr = aurora_value.as_ptr();
        selected_horizon_written = aurora_value_written;
    } else if selected_horizon_index == 2 {
        selected_horizon_ptr = rudder_value.as_ptr();
        selected_horizon_written = rudder_value_written;
    }

    let lantern_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_horizon_ptr,
            selected_horizon_written as i64,
            lantern_old.as_ptr(),
            lantern_old.len() as i64,
            lantern_new.as_ptr(),
            lantern_new.len() as i64,
        )
    };
    let mut lantern_value = vec![0u8; lantern_value_len as usize];
    let lantern_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_horizon_ptr,
            selected_horizon_written as i64,
            lantern_old.as_ptr(),
            lantern_old.len() as i64,
            lantern_new.as_ptr(),
            lantern_new.len() as i64,
            lantern_value.as_mut_ptr(),
            lantern_value.len() as i64,
        )
    };
    let lantern_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            lantern_value.as_ptr(),
            lantern_value_written as i64,
            lantern_needle.as_ptr(),
            lantern_needle.len() as i64,
        )
    };

    let daybreak_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_horizon_ptr,
            selected_horizon_written as i64,
            daybreak_extension.as_ptr(),
            daybreak_extension.len() as i64,
        )
    };
    let mut daybreak_value = vec![0u8; daybreak_value_len as usize];
    let daybreak_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_horizon_ptr,
            selected_horizon_written as i64,
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

    let oar_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_horizon_ptr,
            selected_horizon_written as i64,
        )
    };
    let mut oar_source = vec![0u8; oar_source_len as usize];
    let oar_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_horizon_ptr,
            selected_horizon_written as i64,
            oar_source.as_mut_ptr(),
            oar_source.len() as i64,
        )
    };
    let oar_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            oar_source.as_ptr(),
            oar_source_written as i64,
            oar_old.as_ptr(),
            oar_old.len() as i64,
            oar_new.as_ptr(),
            oar_new.len() as i64,
        )
    };
    let mut oar_value = vec![0u8; oar_value_len as usize];
    let oar_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            oar_source.as_ptr(),
            oar_source_written as i64,
            oar_old.as_ptr(),
            oar_old.len() as i64,
            oar_new.as_ptr(),
            oar_new.len() as i64,
            oar_value.as_mut_ptr(),
            oar_value.len() as i64,
        )
    };
    let oar_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            oar_value.as_ptr(),
            oar_value_written as i64,
            oar_needle.as_ptr(),
            oar_needle.len() as i64,
        )
    };

    let mut selected_meridian_index = 0i32;
    let mut best_meridian_score = i32::MIN;
    let mut meridian_index = 0i32;
    while meridian_index < 3 {
        let mut candidate_meridian_score = lantern_value_written * 10 + lantern_contains * 50;
        if meridian_index == 1 {
            candidate_meridian_score = daybreak_value_written * 10 + daybreak_index;
        } else if meridian_index == 2 {
            candidate_meridian_score = oar_value_written * 10 + oar_contains * 50;
        }

        let mut meridian_bonus = 0i32;
        if meridian_index == selected_horizon_index {
            meridian_bonus += 25;
        }
        if meridian_index == selected_keystone_index {
            meridian_bonus += 15;
        }
        if meridian_index == selected_crown_index {
            meridian_bonus += 5;
        }
        if meridian_index == 0 && lantern_contains != 0 {
            meridian_bonus += 20;
        }
        if meridian_index == 1 && daybreak_index >= 0 {
            meridian_bonus += 10;
        }
        if meridian_index == 2 && oar_contains != 0 {
            meridian_bonus += 30;
        }

        let meridian_score = candidate_meridian_score + meridian_bonus;
        if meridian_score > best_meridian_score {
            best_meridian_score = meridian_score;
            selected_meridian_index = meridian_index;
        }

        meridian_index += 1;
    }

    let mut selected_meridian_ptr = lantern_value.as_ptr();
    let mut selected_meridian_written = lantern_value_written;
    if selected_meridian_index == 1 {
        selected_meridian_ptr = daybreak_value.as_ptr();
        selected_meridian_written = daybreak_value_written;
    } else if selected_meridian_index == 2 {
        selected_meridian_ptr = oar_value.as_ptr();
        selected_meridian_written = oar_value_written;
    }

    let beacon_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_meridian_ptr,
            selected_meridian_written as i64,
            beacon_old.as_ptr(),
            beacon_old.len() as i64,
            beacon_new.as_ptr(),
            beacon_new.len() as i64,
        )
    };
    let mut beacon_value = vec![0u8; beacon_value_len as usize];
    let beacon_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_meridian_ptr,
            selected_meridian_written as i64,
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

    let sunrise_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_meridian_ptr,
            selected_meridian_written as i64,
            sunrise_extension.as_ptr(),
            sunrise_extension.len() as i64,
        )
    };
    let mut sunrise_value = vec![0u8; sunrise_value_len as usize];
    let sunrise_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_meridian_ptr,
            selected_meridian_written as i64,
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

    let mast_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_meridian_ptr,
            selected_meridian_written as i64,
        )
    };
    let mut mast_source = vec![0u8; mast_source_len as usize];
    let mast_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_meridian_ptr,
            selected_meridian_written as i64,
            mast_source.as_mut_ptr(),
            mast_source.len() as i64,
        )
    };
    let mast_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            mast_source.as_ptr(),
            mast_source_written as i64,
            mast_old.as_ptr(),
            mast_old.len() as i64,
            mast_new.as_ptr(),
            mast_new.len() as i64,
        )
    };
    let mut mast_value = vec![0u8; mast_value_len as usize];
    let mast_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            mast_source.as_ptr(),
            mast_source_written as i64,
            mast_old.as_ptr(),
            mast_old.len() as i64,
            mast_new.as_ptr(),
            mast_new.len() as i64,
            mast_value.as_mut_ptr(),
            mast_value.len() as i64,
        )
    };
    let mast_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            mast_value.as_ptr(),
            mast_value_written as i64,
            mast_needle.as_ptr(),
            mast_needle.len() as i64,
        )
    };

    let mut selected_zenith_index = 0i32;
    let mut best_zenith_score = i32::MIN;
    let mut zenith_index = 0i32;
    while zenith_index < 3 {
        let mut candidate_zenith_score = beacon_value_written * 10 + beacon_contains * 50;
        if zenith_index == 1 {
            candidate_zenith_score = sunrise_value_written * 10 + sunrise_index;
        } else if zenith_index == 2 {
            candidate_zenith_score = mast_value_written * 10 + mast_contains * 50;
        }

        let mut zenith_bonus = 0i32;
        if zenith_index == selected_meridian_index {
            zenith_bonus += 25;
        }
        if zenith_index == selected_horizon_index {
            zenith_bonus += 15;
        }
        if zenith_index == selected_keystone_index {
            zenith_bonus += 5;
        }
        if zenith_index == 0 && beacon_contains != 0 {
            zenith_bonus += 20;
        }
        if zenith_index == 1 && sunrise_index >= 0 {
            zenith_bonus += 10;
        }
        if zenith_index == 2 && mast_contains != 0 {
            zenith_bonus += 30;
        }

        let zenith_score = candidate_zenith_score + zenith_bonus;
        if zenith_score > best_zenith_score {
            best_zenith_score = zenith_score;
            selected_zenith_index = zenith_index;
        }

        zenith_index += 1;
    }

    let mut selected_zenith_ptr = beacon_value.as_ptr();
    let mut selected_zenith_written = beacon_value_written;
    if selected_zenith_index == 1 {
        selected_zenith_ptr = sunrise_value.as_ptr();
        selected_zenith_written = sunrise_value_written;
    } else if selected_zenith_index == 2 {
        selected_zenith_ptr = mast_value.as_ptr();
        selected_zenith_written = mast_value_written;
    }

    let signal_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_zenith_ptr,
            selected_zenith_written as i64,
            signal_old.as_ptr(),
            signal_old.len() as i64,
            signal_new.as_ptr(),
            signal_new.len() as i64,
        )
    };
    let mut signal_value = vec![0u8; signal_value_len as usize];
    let signal_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_zenith_ptr,
            selected_zenith_written as i64,
            signal_old.as_ptr(),
            signal_old.len() as i64,
            signal_new.as_ptr(),
            signal_new.len() as i64,
            signal_value.as_mut_ptr(),
            signal_value.len() as i64,
        )
    };
    let signal_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            signal_value.as_ptr(),
            signal_value_written as i64,
            signal_needle.as_ptr(),
            signal_needle.len() as i64,
        )
    };

    let sundial_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_zenith_ptr,
            selected_zenith_written as i64,
            sundial_extension.as_ptr(),
            sundial_extension.len() as i64,
        )
    };
    let mut sundial_value = vec![0u8; sundial_value_len as usize];
    let sundial_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_zenith_ptr,
            selected_zenith_written as i64,
            sundial_extension.as_ptr(),
            sundial_extension.len() as i64,
            sundial_value.as_mut_ptr(),
            sundial_value.len() as i64,
        )
    };
    let sundial_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            sundial_value.as_ptr(),
            sundial_value_written as i64,
            sundial_needle.as_ptr(),
            sundial_needle.len() as i64,
        )
    };

    let spar_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_zenith_ptr,
            selected_zenith_written as i64,
        )
    };
    let mut spar_source = vec![0u8; spar_source_len as usize];
    let spar_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_zenith_ptr,
            selected_zenith_written as i64,
            spar_source.as_mut_ptr(),
            spar_source.len() as i64,
        )
    };
    let spar_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            spar_source.as_ptr(),
            spar_source_written as i64,
            spar_old.as_ptr(),
            spar_old.len() as i64,
            spar_new.as_ptr(),
            spar_new.len() as i64,
        )
    };
    let mut spar_value = vec![0u8; spar_value_len as usize];
    let spar_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            spar_source.as_ptr(),
            spar_source_written as i64,
            spar_old.as_ptr(),
            spar_old.len() as i64,
            spar_new.as_ptr(),
            spar_new.len() as i64,
            spar_value.as_mut_ptr(),
            spar_value.len() as i64,
        )
    };
    let spar_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            spar_value.as_ptr(),
            spar_value_written as i64,
            spar_needle.as_ptr(),
            spar_needle.len() as i64,
        )
    };

    let mut selected_noon_index = 0i32;
    let mut best_noon_score = i32::MIN;
    let mut noon_index = 0i32;
    while noon_index < 3 {
        let mut candidate_noon_score = signal_value_written * 10 + signal_contains * 50;
        if noon_index == 1 {
            candidate_noon_score = sundial_value_written * 10 + sundial_index;
        } else if noon_index == 2 {
            candidate_noon_score = spar_value_written * 10 + spar_contains * 50;
        }

        let mut noon_bonus = 0i32;
        if noon_index == selected_zenith_index {
            noon_bonus += 25;
        }
        if noon_index == selected_meridian_index {
            noon_bonus += 15;
        }
        if noon_index == selected_horizon_index {
            noon_bonus += 5;
        }
        if noon_index == 0 && signal_contains != 0 {
            noon_bonus += 20;
        }
        if noon_index == 1 && sundial_index >= 0 {
            noon_bonus += 10;
        }
        if noon_index == 2 && spar_contains != 0 {
            noon_bonus += 30;
        }

        let noon_score = candidate_noon_score + noon_bonus;
        if noon_score > best_noon_score {
            best_noon_score = noon_score;
            selected_noon_index = noon_index;
        }

        noon_index += 1;
    }

    let mut selected_noon_ptr = signal_value.as_ptr();
    let mut selected_noon_written = signal_value_written;
    if selected_noon_index == 1 {
        selected_noon_ptr = sundial_value.as_ptr();
        selected_noon_written = sundial_value_written;
    } else if selected_noon_index == 2 {
        selected_noon_ptr = spar_value.as_ptr();
        selected_noon_written = spar_value_written;
    }

    let pennant_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_noon_ptr,
            selected_noon_written as i64,
            pennant_old.as_ptr(),
            pennant_old.len() as i64,
            pennant_new.as_ptr(),
            pennant_new.len() as i64,
        )
    };
    let mut pennant_value = vec![0u8; pennant_value_len as usize];
    let pennant_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_noon_ptr,
            selected_noon_written as i64,
            pennant_old.as_ptr(),
            pennant_old.len() as i64,
            pennant_new.as_ptr(),
            pennant_new.len() as i64,
            pennant_value.as_mut_ptr(),
            pennant_value.len() as i64,
        )
    };
    let pennant_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            pennant_value.as_ptr(),
            pennant_value_written as i64,
            pennant_needle.as_ptr(),
            pennant_needle.len() as i64,
        )
    };

    let solstice_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_noon_ptr,
            selected_noon_written as i64,
            solstice_extension.as_ptr(),
            solstice_extension.len() as i64,
        )
    };
    let mut solstice_value = vec![0u8; solstice_value_len as usize];
    let solstice_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_noon_ptr,
            selected_noon_written as i64,
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

    let boom_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_noon_ptr,
            selected_noon_written as i64,
        )
    };
    let mut boom_source = vec![0u8; boom_source_len as usize];
    let boom_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_noon_ptr,
            selected_noon_written as i64,
            boom_source.as_mut_ptr(),
            boom_source.len() as i64,
        )
    };
    let boom_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            boom_source.as_ptr(),
            boom_source_written as i64,
            boom_old.as_ptr(),
            boom_old.len() as i64,
            boom_new.as_ptr(),
            boom_new.len() as i64,
        )
    };
    let mut boom_value = vec![0u8; boom_value_len as usize];
    let boom_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            boom_source.as_ptr(),
            boom_source_written as i64,
            boom_old.as_ptr(),
            boom_old.len() as i64,
            boom_new.as_ptr(),
            boom_new.len() as i64,
            boom_value.as_mut_ptr(),
            boom_value.len() as i64,
        )
    };
    let boom_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            boom_value.as_ptr(),
            boom_value_written as i64,
            boom_needle.as_ptr(),
            boom_needle.len() as i64,
        )
    };

    let mut selected_eclipse_index = 0i32;
    let mut best_eclipse_score = i32::MIN;
    let mut eclipse_index = 0i32;
    while eclipse_index < 3 {
        let mut candidate_eclipse_score = pennant_value_written * 10 + pennant_contains * 50;
        if eclipse_index == 1 {
            candidate_eclipse_score = solstice_value_written * 10 + solstice_index;
        } else if eclipse_index == 2 {
            candidate_eclipse_score = boom_value_written * 10 + boom_contains * 50;
        }

        let mut eclipse_bonus = 0i32;
        if eclipse_index == selected_noon_index {
            eclipse_bonus += 25;
        }
        if eclipse_index == selected_zenith_index {
            eclipse_bonus += 15;
        }
        if eclipse_index == selected_meridian_index {
            eclipse_bonus += 5;
        }
        if eclipse_index == 0 && pennant_contains != 0 {
            eclipse_bonus += 20;
        }
        if eclipse_index == 1 && solstice_index >= 0 {
            eclipse_bonus += 10;
        }
        if eclipse_index == 2 && boom_contains != 0 {
            eclipse_bonus += 30;
        }

        let eclipse_score = candidate_eclipse_score + eclipse_bonus;
        if eclipse_score > best_eclipse_score {
            best_eclipse_score = eclipse_score;
            selected_eclipse_index = eclipse_index;
        }

        eclipse_index += 1;
    }

    let mut selected_eclipse_ptr = pennant_value.as_ptr();
    let mut selected_eclipse_written = pennant_value_written;
    if selected_eclipse_index == 1 {
        selected_eclipse_ptr = solstice_value.as_ptr();
        selected_eclipse_written = solstice_value_written;
    } else if selected_eclipse_index == 2 {
        selected_eclipse_ptr = boom_value.as_ptr();
        selected_eclipse_written = boom_value_written;
    }

    let heliostat_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_eclipse_ptr,
            selected_eclipse_written as i64,
            heliostat_old.as_ptr(),
            heliostat_old.len() as i64,
            heliostat_new.as_ptr(),
            heliostat_new.len() as i64,
        )
    };
    let mut heliostat_value = vec![0u8; heliostat_value_len as usize];
    let heliostat_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_eclipse_ptr,
            selected_eclipse_written as i64,
            heliostat_old.as_ptr(),
            heliostat_old.len() as i64,
            heliostat_new.as_ptr(),
            heliostat_new.len() as i64,
            heliostat_value.as_mut_ptr(),
            heliostat_value.len() as i64,
        )
    };
    let heliostat_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            heliostat_value.as_ptr(),
            heliostat_value_written as i64,
            heliostat_needle.as_ptr(),
            heliostat_needle.len() as i64,
        )
    };

    let equinox_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_eclipse_ptr,
            selected_eclipse_written as i64,
            equinox_extension.as_ptr(),
            equinox_extension.len() as i64,
        )
    };
    let mut equinox_value = vec![0u8; equinox_value_len as usize];
    let equinox_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_eclipse_ptr,
            selected_eclipse_written as i64,
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

    let davit_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_eclipse_ptr,
            selected_eclipse_written as i64,
        )
    };
    let mut davit_source = vec![0u8; davit_source_len as usize];
    let davit_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_eclipse_ptr,
            selected_eclipse_written as i64,
            davit_source.as_mut_ptr(),
            davit_source.len() as i64,
        )
    };
    let davit_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            davit_source.as_ptr(),
            davit_source_written as i64,
            davit_old.as_ptr(),
            davit_old.len() as i64,
            davit_new.as_ptr(),
            davit_new.len() as i64,
        )
    };
    let mut davit_value = vec![0u8; davit_value_len as usize];
    let davit_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            davit_source.as_ptr(),
            davit_source_written as i64,
            davit_old.as_ptr(),
            davit_old.len() as i64,
            davit_new.as_ptr(),
            davit_new.len() as i64,
            davit_value.as_mut_ptr(),
            davit_value.len() as i64,
        )
    };
    let davit_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            davit_value.as_ptr(),
            davit_value_written as i64,
            davit_needle.as_ptr(),
            davit_needle.len() as i64,
        )
    };

    let mut selected_corona_index = 0i32;
    let mut best_corona_score = i32::MIN;
    let mut corona_index = 0i32;
    while corona_index < 3 {
        let mut candidate_corona_score = heliostat_value_written * 10 + heliostat_contains * 50;
        if corona_index == 1 {
            candidate_corona_score = equinox_value_written * 10 + equinox_index;
        } else if corona_index == 2 {
            candidate_corona_score = davit_value_written * 10 + davit_contains * 50;
        }

        let mut corona_bonus = 0i32;
        if corona_index == selected_eclipse_index {
            corona_bonus += 25;
        }
        if corona_index == selected_noon_index {
            corona_bonus += 15;
        }
        if corona_index == selected_zenith_index {
            corona_bonus += 5;
        }
        if corona_index == 0 && heliostat_contains != 0 {
            corona_bonus += 20;
        }
        if corona_index == 1 && equinox_index >= 0 {
            corona_bonus += 10;
        }
        if corona_index == 2 && davit_contains != 0 {
            corona_bonus += 30;
        }

        let corona_score = candidate_corona_score + corona_bonus;
        if corona_score > best_corona_score {
            best_corona_score = corona_score;
            selected_corona_index = corona_index;
        }

        corona_index += 1;
    }

    let mut selected_corona_ptr = heliostat_value.as_ptr();
    let mut selected_corona_written = heliostat_value_written;
    if selected_corona_index == 1 {
        selected_corona_ptr = equinox_value.as_ptr();
        selected_corona_written = equinox_value_written;
    } else if selected_corona_index == 2 {
        selected_corona_ptr = davit_value.as_ptr();
        selected_corona_written = davit_value_written;
    }

    let sunlattice_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_corona_ptr,
            selected_corona_written as i64,
            sunlattice_old.as_ptr(),
            sunlattice_old.len() as i64,
            sunlattice_new.as_ptr(),
            sunlattice_new.len() as i64,
        )
    };
    let mut sunlattice_value = vec![0u8; sunlattice_value_len as usize];
    let sunlattice_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_corona_ptr,
            selected_corona_written as i64,
            sunlattice_old.as_ptr(),
            sunlattice_old.len() as i64,
            sunlattice_new.as_ptr(),
            sunlattice_new.len() as i64,
            sunlattice_value.as_mut_ptr(),
            sunlattice_value.len() as i64,
        )
    };
    let sunlattice_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            sunlattice_value.as_ptr(),
            sunlattice_value_written as i64,
            sunlattice_needle.as_ptr(),
            sunlattice_needle.len() as i64,
        )
    };

    let daystar_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_corona_ptr,
            selected_corona_written as i64,
            daystar_extension.as_ptr(),
            daystar_extension.len() as i64,
        )
    };
    let mut daystar_value = vec![0u8; daystar_value_len as usize];
    let daystar_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_corona_ptr,
            selected_corona_written as i64,
            daystar_extension.as_ptr(),
            daystar_extension.len() as i64,
            daystar_value.as_mut_ptr(),
            daystar_value.len() as i64,
        )
    };
    let daystar_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            daystar_value.as_ptr(),
            daystar_value_written as i64,
            daystar_needle.as_ptr(),
            daystar_needle.len() as i64,
        )
    };

    let yard_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_corona_ptr,
            selected_corona_written as i64,
        )
    };
    let mut yard_source = vec![0u8; yard_source_len as usize];
    let yard_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_corona_ptr,
            selected_corona_written as i64,
            yard_source.as_mut_ptr(),
            yard_source.len() as i64,
        )
    };
    let yard_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            yard_source.as_ptr(),
            yard_source_written as i64,
            yard_old.as_ptr(),
            yard_old.len() as i64,
            yard_new.as_ptr(),
            yard_new.len() as i64,
        )
    };
    let mut yard_value = vec![0u8; yard_value_len as usize];
    let yard_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            yard_source.as_ptr(),
            yard_source_written as i64,
            yard_old.as_ptr(),
            yard_old.len() as i64,
            yard_new.as_ptr(),
            yard_new.len() as i64,
            yard_value.as_mut_ptr(),
            yard_value.len() as i64,
        )
    };
    let yard_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            yard_value.as_ptr(),
            yard_value_written as i64,
            yard_needle.as_ptr(),
            yard_needle.len() as i64,
        )
    };

    let mut selected_aphelion_index = 0i32;
    let mut best_aphelion_score = i32::MIN;
    let mut aphelion_index = 0i32;
    while aphelion_index < 3 {
        let mut candidate_aphelion_score = sunlattice_value_written * 10 + sunlattice_contains * 50;
        if aphelion_index == 1 {
            candidate_aphelion_score = daystar_value_written * 10 + daystar_index;
        } else if aphelion_index == 2 {
            candidate_aphelion_score = yard_value_written * 10 + yard_contains * 50;
        }

        let mut aphelion_bonus = 0i32;
        if aphelion_index == selected_corona_index {
            aphelion_bonus += 25;
        }
        if aphelion_index == selected_eclipse_index {
            aphelion_bonus += 15;
        }
        if aphelion_index == selected_noon_index {
            aphelion_bonus += 5;
        }
        if aphelion_index == 0 && sunlattice_contains != 0 {
            aphelion_bonus += 20;
        }
        if aphelion_index == 1 && daystar_index >= 0 {
            aphelion_bonus += 10;
        }
        if aphelion_index == 2 && yard_contains != 0 {
            aphelion_bonus += 30;
        }

        let aphelion_score = candidate_aphelion_score + aphelion_bonus;
        if aphelion_score > best_aphelion_score {
            best_aphelion_score = aphelion_score;
            selected_aphelion_index = aphelion_index;
        }

        aphelion_index += 1;
    }

    let mut selected_aphelion_ptr = sunlattice_value.as_ptr();
    let mut selected_aphelion_written = sunlattice_value_written;
    if selected_aphelion_index == 1 {
        selected_aphelion_ptr = daystar_value.as_ptr();
        selected_aphelion_written = daystar_value_written;
    } else if selected_aphelion_index == 2 {
        selected_aphelion_ptr = yard_value.as_ptr();
        selected_aphelion_written = yard_value_written;
    }

    let orrery_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_aphelion_ptr,
            selected_aphelion_written as i64,
            orrery_old.as_ptr(),
            orrery_old.len() as i64,
            orrery_new.as_ptr(),
            orrery_new.len() as i64,
        )
    };
    let mut orrery_value = vec![0u8; orrery_value_len as usize];
    let orrery_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_aphelion_ptr,
            selected_aphelion_written as i64,
            orrery_old.as_ptr(),
            orrery_old.len() as i64,
            orrery_new.as_ptr(),
            orrery_new.len() as i64,
            orrery_value.as_mut_ptr(),
            orrery_value.len() as i64,
        )
    };
    let orrery_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            orrery_value.as_ptr(),
            orrery_value_written as i64,
            orrery_needle.as_ptr(),
            orrery_needle.len() as i64,
        )
    };

    let dusk_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_aphelion_ptr,
            selected_aphelion_written as i64,
            dusk_extension.as_ptr(),
            dusk_extension.len() as i64,
        )
    };
    let mut dusk_value = vec![0u8; dusk_value_len as usize];
    let dusk_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_aphelion_ptr,
            selected_aphelion_written as i64,
            dusk_extension.as_ptr(),
            dusk_extension.len() as i64,
            dusk_value.as_mut_ptr(),
            dusk_value.len() as i64,
        )
    };
    let dusk_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            dusk_value.as_ptr(),
            dusk_value_written as i64,
            dusk_needle.as_ptr(),
            dusk_needle.len() as i64,
        )
    };

    let gaff_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_aphelion_ptr,
            selected_aphelion_written as i64,
        )
    };
    let mut gaff_source = vec![0u8; gaff_source_len as usize];
    let gaff_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_aphelion_ptr,
            selected_aphelion_written as i64,
            gaff_source.as_mut_ptr(),
            gaff_source.len() as i64,
        )
    };
    let gaff_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            gaff_source.as_ptr(),
            gaff_source_written as i64,
            gaff_old.as_ptr(),
            gaff_old.len() as i64,
            gaff_new.as_ptr(),
            gaff_new.len() as i64,
        )
    };
    let mut gaff_value = vec![0u8; gaff_value_len as usize];
    let gaff_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            gaff_source.as_ptr(),
            gaff_source_written as i64,
            gaff_old.as_ptr(),
            gaff_old.len() as i64,
            gaff_new.as_ptr(),
            gaff_new.len() as i64,
            gaff_value.as_mut_ptr(),
            gaff_value.len() as i64,
        )
    };
    let gaff_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            gaff_value.as_ptr(),
            gaff_value_written as i64,
            gaff_needle.as_ptr(),
            gaff_needle.len() as i64,
        )
    };

    let mut selected_aurora_index = 0i32;
    let mut best_aurora_score = i32::MIN;
    let mut aurora_index = 0i32;
    while aurora_index < 3 {
        let mut candidate_aurora_score = orrery_value_written * 10 + orrery_contains * 50;
        if aurora_index == 1 {
            candidate_aurora_score = dusk_value_written * 10 + dusk_index;
        } else if aurora_index == 2 {
            candidate_aurora_score = gaff_value_written * 10 + gaff_contains * 50;
        }

        let mut aurora_bonus = 0i32;
        if aurora_index == selected_aphelion_index {
            aurora_bonus += 25;
        }
        if aurora_index == selected_corona_index {
            aurora_bonus += 15;
        }
        if aurora_index == selected_eclipse_index {
            aurora_bonus += 5;
        }
        if aurora_index == 0 && orrery_contains != 0 {
            aurora_bonus += 20;
        }
        if aurora_index == 1 && dusk_index >= 0 {
            aurora_bonus += 10;
        }
        if aurora_index == 2 && gaff_contains != 0 {
            aurora_bonus += 30;
        }

        let aurora_score = candidate_aurora_score + aurora_bonus;
        if aurora_score > best_aurora_score {
            best_aurora_score = aurora_score;
            selected_aurora_index = aurora_index;
        }

        aurora_index += 1;
    }

    let mut selected_aurora_ptr = orrery_value.as_ptr();
    let mut selected_aurora_written = orrery_value_written;
    if selected_aurora_index == 1 {
        selected_aurora_ptr = dusk_value.as_ptr();
        selected_aurora_written = dusk_value_written;
    } else if selected_aurora_index == 2 {
        selected_aurora_ptr = gaff_value.as_ptr();
        selected_aurora_written = gaff_value_written;
    }

    let astrolabe_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_aurora_ptr,
            selected_aurora_written as i64,
            astrolabe_old.as_ptr(),
            astrolabe_old.len() as i64,
            astrolabe_new.as_ptr(),
            astrolabe_new.len() as i64,
        )
    };
    let mut astrolabe_value = vec![0u8; astrolabe_value_len as usize];
    let astrolabe_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_aurora_ptr,
            selected_aurora_written as i64,
            astrolabe_old.as_ptr(),
            astrolabe_old.len() as i64,
            astrolabe_new.as_ptr(),
            astrolabe_new.len() as i64,
            astrolabe_value.as_mut_ptr(),
            astrolabe_value.len() as i64,
        )
    };
    let astrolabe_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            astrolabe_value.as_ptr(),
            astrolabe_value_written as i64,
            astrolabe_needle.as_ptr(),
            astrolabe_needle.len() as i64,
        )
    };

    let twilight_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_aurora_ptr,
            selected_aurora_written as i64,
            twilight_extension.as_ptr(),
            twilight_extension.len() as i64,
        )
    };
    let mut twilight_value = vec![0u8; twilight_value_len as usize];
    let twilight_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_aurora_ptr,
            selected_aurora_written as i64,
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

    let brace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_aurora_ptr,
            selected_aurora_written as i64,
        )
    };
    let mut brace_source = vec![0u8; brace_source_len as usize];
    let brace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_aurora_ptr,
            selected_aurora_written as i64,
            brace_source.as_mut_ptr(),
            brace_source.len() as i64,
        )
    };
    let brace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            brace_source.as_ptr(),
            brace_source_written as i64,
            brace_old.as_ptr(),
            brace_old.len() as i64,
            brace_new.as_ptr(),
            brace_new.len() as i64,
        )
    };
    let mut brace_value = vec![0u8; brace_value_len as usize];
    let brace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            brace_source.as_ptr(),
            brace_source_written as i64,
            brace_old.as_ptr(),
            brace_old.len() as i64,
            brace_new.as_ptr(),
            brace_new.len() as i64,
            brace_value.as_mut_ptr(),
            brace_value.len() as i64,
        )
    };
    let brace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            brace_value.as_ptr(),
            brace_value_written as i64,
            brace_needle.as_ptr(),
            brace_needle.len() as i64,
        )
    };

    let mut selected_parallax_index = 0i32;
    let mut best_parallax_score = i32::MIN;
    let mut parallax_index = 0i32;
    while parallax_index < 3 {
        let mut candidate_parallax_score = astrolabe_value_written * 10 + astrolabe_contains * 50;
        if parallax_index == 1 {
            candidate_parallax_score = twilight_value_written * 10 + twilight_index;
        } else if parallax_index == 2 {
            candidate_parallax_score = brace_value_written * 10 + brace_contains * 50;
        }

        let mut parallax_bonus = 0i32;
        if parallax_index == selected_aurora_index {
            parallax_bonus += 25;
        }
        if parallax_index == selected_aphelion_index {
            parallax_bonus += 15;
        }
        if parallax_index == selected_corona_index {
            parallax_bonus += 5;
        }
        if parallax_index == 0 && astrolabe_contains != 0 {
            parallax_bonus += 20;
        }
        if parallax_index == 1 && twilight_index >= 0 {
            parallax_bonus += 10;
        }
        if parallax_index == 2 && brace_contains != 0 {
            parallax_bonus += 30;
        }

        let parallax_score = candidate_parallax_score + parallax_bonus;
        if parallax_score > best_parallax_score {
            best_parallax_score = parallax_score;
            selected_parallax_index = parallax_index;
        }

        parallax_index += 1;
    }

    let mut selected_parallax_ptr = astrolabe_value.as_ptr();
    let mut selected_parallax_written = astrolabe_value_written;
    if selected_parallax_index == 1 {
        selected_parallax_ptr = twilight_value.as_ptr();
        selected_parallax_written = twilight_value_written;
    } else if selected_parallax_index == 2 {
        selected_parallax_ptr = brace_value.as_ptr();
        selected_parallax_written = brace_value_written;
    }

    let sextant_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_parallax_ptr,
            selected_parallax_written as i64,
            sextant_old.as_ptr(),
            sextant_old.len() as i64,
            sextant_new.as_ptr(),
            sextant_new.len() as i64,
        )
    };
    let mut sextant_value = vec![0u8; sextant_value_len as usize];
    let sextant_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_parallax_ptr,
            selected_parallax_written as i64,
            sextant_old.as_ptr(),
            sextant_old.len() as i64,
            sextant_new.as_ptr(),
            sextant_new.len() as i64,
            sextant_value.as_mut_ptr(),
            sextant_value.len() as i64,
        )
    };
    let sextant_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            sextant_value.as_ptr(),
            sextant_value_written as i64,
            sextant_needle.as_ptr(),
            sextant_needle.len() as i64,
        )
    };

    let eventide_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_parallax_ptr,
            selected_parallax_written as i64,
            eventide_extension.as_ptr(),
            eventide_extension.len() as i64,
        )
    };
    let mut eventide_value = vec![0u8; eventide_value_len as usize];
    let eventide_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_parallax_ptr,
            selected_parallax_written as i64,
            eventide_extension.as_ptr(),
            eventide_extension.len() as i64,
            eventide_value.as_mut_ptr(),
            eventide_value.len() as i64,
        )
    };
    let eventide_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            eventide_value.as_ptr(),
            eventide_value_written as i64,
            eventide_needle.as_ptr(),
            eventide_needle.len() as i64,
        )
    };

    let halyard_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_parallax_ptr,
            selected_parallax_written as i64,
        )
    };
    let mut halyard_source = vec![0u8; halyard_source_len as usize];
    let halyard_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_parallax_ptr,
            selected_parallax_written as i64,
            halyard_source.as_mut_ptr(),
            halyard_source.len() as i64,
        )
    };
    let halyard_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            halyard_source.as_ptr(),
            halyard_source_written as i64,
            halyard_old.as_ptr(),
            halyard_old.len() as i64,
            halyard_new.as_ptr(),
            halyard_new.len() as i64,
        )
    };
    let mut halyard_value = vec![0u8; halyard_value_len as usize];
    let halyard_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            halyard_source.as_ptr(),
            halyard_source_written as i64,
            halyard_old.as_ptr(),
            halyard_old.len() as i64,
            halyard_new.as_ptr(),
            halyard_new.len() as i64,
            halyard_value.as_mut_ptr(),
            halyard_value.len() as i64,
        )
    };
    let halyard_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            halyard_value.as_ptr(),
            halyard_value_written as i64,
            halyard_needle.as_ptr(),
            halyard_needle.len() as i64,
        )
    };

    let mut selected_penumbra_index = 0i32;
    let mut best_penumbra_score = i32::MIN;
    let mut penumbra_index = 0i32;
    while penumbra_index < 3 {
        let mut candidate_penumbra_score = sextant_value_written * 10 + sextant_contains * 50;
        if penumbra_index == 1 {
            candidate_penumbra_score = eventide_value_written * 10 + eventide_index;
        } else if penumbra_index == 2 {
            candidate_penumbra_score = halyard_value_written * 10 + halyard_contains * 50;
        }

        let mut penumbra_bonus = 0i32;
        if penumbra_index == selected_parallax_index {
            penumbra_bonus += 25;
        }
        if penumbra_index == selected_aurora_index {
            penumbra_bonus += 15;
        }
        if penumbra_index == selected_aphelion_index {
            penumbra_bonus += 5;
        }
        if penumbra_index == 0 && sextant_contains != 0 {
            penumbra_bonus += 20;
        }
        if penumbra_index == 1 && eventide_index >= 0 {
            penumbra_bonus += 10;
        }
        if penumbra_index == 2 && halyard_contains != 0 {
            penumbra_bonus += 30;
        }

        let penumbra_score = candidate_penumbra_score + penumbra_bonus;
        if penumbra_score > best_penumbra_score {
            best_penumbra_score = penumbra_score;
            selected_penumbra_index = penumbra_index;
        }

        penumbra_index += 1;
    }

    let mut selected_penumbra_ptr = sextant_value.as_ptr();
    let mut selected_penumbra_written = sextant_value_written;
    if selected_penumbra_index == 1 {
        selected_penumbra_ptr = eventide_value.as_ptr();
        selected_penumbra_written = eventide_value_written;
    } else if selected_penumbra_index == 2 {
        selected_penumbra_ptr = halyard_value.as_ptr();
        selected_penumbra_written = halyard_value_written;
    }

    let armillary_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_penumbra_ptr,
            selected_penumbra_written as i64,
            armillary_old.as_ptr(),
            armillary_old.len() as i64,
            armillary_new.as_ptr(),
            armillary_new.len() as i64,
        )
    };
    let mut armillary_value = vec![0u8; armillary_value_len as usize];
    let armillary_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_penumbra_ptr,
            selected_penumbra_written as i64,
            armillary_old.as_ptr(),
            armillary_old.len() as i64,
            armillary_new.as_ptr(),
            armillary_new.len() as i64,
            armillary_value.as_mut_ptr(),
            armillary_value.len() as i64,
        )
    };
    let armillary_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            armillary_value.as_ptr(),
            armillary_value_written as i64,
            armillary_needle.as_ptr(),
            armillary_needle.len() as i64,
        )
    };

    let midwatch_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_penumbra_ptr,
            selected_penumbra_written as i64,
            midwatch_extension.as_ptr(),
            midwatch_extension.len() as i64,
        )
    };
    let mut midwatch_value = vec![0u8; midwatch_value_len as usize];
    let midwatch_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_penumbra_ptr,
            selected_penumbra_written as i64,
            midwatch_extension.as_ptr(),
            midwatch_extension.len() as i64,
            midwatch_value.as_mut_ptr(),
            midwatch_value.len() as i64,
        )
    };
    let midwatch_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            midwatch_value.as_ptr(),
            midwatch_value_written as i64,
            midwatch_needle.as_ptr(),
            midwatch_needle.len() as i64,
        )
    };

    let sheet_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_penumbra_ptr,
            selected_penumbra_written as i64,
        )
    };
    let mut sheet_source = vec![0u8; sheet_source_len as usize];
    let sheet_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_penumbra_ptr,
            selected_penumbra_written as i64,
            sheet_source.as_mut_ptr(),
            sheet_source.len() as i64,
        )
    };
    let sheet_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            sheet_source.as_ptr(),
            sheet_source_written as i64,
            sheet_old.as_ptr(),
            sheet_old.len() as i64,
            sheet_new.as_ptr(),
            sheet_new.len() as i64,
        )
    };
    let mut sheet_value = vec![0u8; sheet_value_len as usize];
    let sheet_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            sheet_source.as_ptr(),
            sheet_source_written as i64,
            sheet_old.as_ptr(),
            sheet_old.len() as i64,
            sheet_new.as_ptr(),
            sheet_new.len() as i64,
            sheet_value.as_mut_ptr(),
            sheet_value.len() as i64,
        )
    };
    let sheet_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            sheet_value.as_ptr(),
            sheet_value_written as i64,
            sheet_needle.as_ptr(),
            sheet_needle.len() as i64,
        )
    };

    let mut selected_umbra_index = 0i32;
    let mut best_umbra_score = i32::MIN;
    let mut umbra_index = 0i32;
    while umbra_index < 3 {
        let mut candidate_umbra_score = armillary_value_written * 10 + armillary_contains * 50;
        if umbra_index == 1 {
            candidate_umbra_score = midwatch_value_written * 10 + midwatch_index;
        } else if umbra_index == 2 {
            candidate_umbra_score = sheet_value_written * 10 + sheet_contains * 50;
        }

        let mut umbra_bonus = 0i32;
        if umbra_index == selected_penumbra_index {
            umbra_bonus += 25;
        }
        if umbra_index == selected_parallax_index {
            umbra_bonus += 15;
        }
        if umbra_index == selected_aurora_index {
            umbra_bonus += 5;
        }
        if umbra_index == 0 && armillary_contains != 0 {
            umbra_bonus += 20;
        }
        if umbra_index == 1 && midwatch_index >= 0 {
            umbra_bonus += 10;
        }
        if umbra_index == 2 && sheet_contains != 0 {
            umbra_bonus += 30;
        }

        let umbra_score = candidate_umbra_score + umbra_bonus;
        if umbra_score > best_umbra_score {
            best_umbra_score = umbra_score;
            selected_umbra_index = umbra_index;
        }

        umbra_index += 1;
    }

    let mut selected_umbra_ptr = armillary_value.as_ptr();
    let mut selected_umbra_written = armillary_value_written;
    if selected_umbra_index == 1 {
        selected_umbra_ptr = midwatch_value.as_ptr();
        selected_umbra_written = midwatch_value_written;
    } else if selected_umbra_index == 2 {
        selected_umbra_ptr = sheet_value.as_ptr();
        selected_umbra_written = sheet_value_written;
    }

    let tellurion_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_umbra_ptr,
            selected_umbra_written as i64,
            tellurion_old.as_ptr(),
            tellurion_old.len() as i64,
            tellurion_new.as_ptr(),
            tellurion_new.len() as i64,
        )
    };
    let mut tellurion_value = vec![0u8; tellurion_value_len as usize];
    let tellurion_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_umbra_ptr,
            selected_umbra_written as i64,
            tellurion_old.as_ptr(),
            tellurion_old.len() as i64,
            tellurion_new.as_ptr(),
            tellurion_new.len() as i64,
            tellurion_value.as_mut_ptr(),
            tellurion_value.len() as i64,
        )
    };
    let tellurion_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            tellurion_value.as_ptr(),
            tellurion_value_written as i64,
            tellurion_needle.as_ptr(),
            tellurion_needle.len() as i64,
        )
    };

    let starlit_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_umbra_ptr,
            selected_umbra_written as i64,
            starlit_extension.as_ptr(),
            starlit_extension.len() as i64,
        )
    };
    let mut starlit_value = vec![0u8; starlit_value_len as usize];
    let starlit_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_umbra_ptr,
            selected_umbra_written as i64,
            starlit_extension.as_ptr(),
            starlit_extension.len() as i64,
            starlit_value.as_mut_ptr(),
            starlit_value.len() as i64,
        )
    };
    let starlit_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            starlit_value.as_ptr(),
            starlit_value_written as i64,
            starlit_needle.as_ptr(),
            starlit_needle.len() as i64,
        )
    };

    let clew_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_umbra_ptr,
            selected_umbra_written as i64,
        )
    };
    let mut clew_source = vec![0u8; clew_source_len as usize];
    let clew_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_umbra_ptr,
            selected_umbra_written as i64,
            clew_source.as_mut_ptr(),
            clew_source.len() as i64,
        )
    };
    let clew_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            clew_source.as_ptr(),
            clew_source_written as i64,
            clew_old.as_ptr(),
            clew_old.len() as i64,
            clew_new.as_ptr(),
            clew_new.len() as i64,
        )
    };
    let mut clew_value = vec![0u8; clew_value_len as usize];
    let clew_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            clew_source.as_ptr(),
            clew_source_written as i64,
            clew_old.as_ptr(),
            clew_old.len() as i64,
            clew_new.as_ptr(),
            clew_new.len() as i64,
            clew_value.as_mut_ptr(),
            clew_value.len() as i64,
        )
    };
    let clew_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            clew_value.as_ptr(),
            clew_value_written as i64,
            clew_needle.as_ptr(),
            clew_needle.len() as i64,
        )
    };

    let mut selected_antumbra_index = 0i32;
    let mut best_antumbra_score = i32::MIN;
    let mut antumbra_index = 0i32;
    while antumbra_index < 3 {
        let mut candidate_antumbra_score = tellurion_value_written * 10 + tellurion_contains * 50;
        if antumbra_index == 1 {
            candidate_antumbra_score = starlit_value_written * 10 + starlit_index;
        } else if antumbra_index == 2 {
            candidate_antumbra_score = clew_value_written * 10 + clew_contains * 50;
        }

        let mut antumbra_bonus = 0i32;
        if antumbra_index == selected_umbra_index {
            antumbra_bonus += 25;
        }
        if antumbra_index == selected_penumbra_index {
            antumbra_bonus += 15;
        }
        if antumbra_index == selected_parallax_index {
            antumbra_bonus += 5;
        }
        if antumbra_index == 0 && tellurion_contains != 0 {
            antumbra_bonus += 20;
        }
        if antumbra_index == 1 && starlit_index >= 0 {
            antumbra_bonus += 10;
        }
        if antumbra_index == 2 && clew_contains != 0 {
            antumbra_bonus += 30;
        }

        let antumbra_score = candidate_antumbra_score + antumbra_bonus;
        if antumbra_score > best_antumbra_score {
            best_antumbra_score = antumbra_score;
            selected_antumbra_index = antumbra_index;
        }

        antumbra_index += 1;
    }

    let mut selected_antumbra_ptr = tellurion_value.as_ptr();
    let mut selected_antumbra_written = tellurion_value_written;
    if selected_antumbra_index == 1 {
        selected_antumbra_ptr = starlit_value.as_ptr();
        selected_antumbra_written = starlit_value_written;
    } else if selected_antumbra_index == 2 {
        selected_antumbra_ptr = clew_value.as_ptr();
        selected_antumbra_written = clew_value_written;
    }

    let planisphere_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_antumbra_ptr,
            selected_antumbra_written as i64,
            planisphere_old.as_ptr(),
            planisphere_old.len() as i64,
            planisphere_new.as_ptr(),
            planisphere_new.len() as i64,
        )
    };
    let mut planisphere_value = vec![0u8; planisphere_value_len as usize];
    let planisphere_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_antumbra_ptr,
            selected_antumbra_written as i64,
            planisphere_old.as_ptr(),
            planisphere_old.len() as i64,
            planisphere_new.as_ptr(),
            planisphere_new.len() as i64,
            planisphere_value.as_mut_ptr(),
            planisphere_value.len() as i64,
        )
    };
    let planisphere_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            planisphere_value.as_ptr(),
            planisphere_value_written as i64,
            planisphere_needle.as_ptr(),
            planisphere_needle.len() as i64,
        )
    };

    let nightglass_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_antumbra_ptr,
            selected_antumbra_written as i64,
            nightglass_extension.as_ptr(),
            nightglass_extension.len() as i64,
        )
    };
    let mut nightglass_value = vec![0u8; nightglass_value_len as usize];
    let nightglass_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_antumbra_ptr,
            selected_antumbra_written as i64,
            nightglass_extension.as_ptr(),
            nightglass_extension.len() as i64,
            nightglass_value.as_mut_ptr(),
            nightglass_value.len() as i64,
        )
    };
    let nightglass_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            nightglass_value.as_ptr(),
            nightglass_value_written as i64,
            nightglass_needle.as_ptr(),
            nightglass_needle.len() as i64,
        )
    };

    let footrope_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_antumbra_ptr,
            selected_antumbra_written as i64,
        )
    };
    let mut footrope_source = vec![0u8; footrope_source_len as usize];
    let footrope_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_antumbra_ptr,
            selected_antumbra_written as i64,
            footrope_source.as_mut_ptr(),
            footrope_source.len() as i64,
        )
    };
    let footrope_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            footrope_source.as_ptr(),
            footrope_source_written as i64,
            footrope_old.as_ptr(),
            footrope_old.len() as i64,
            footrope_new.as_ptr(),
            footrope_new.len() as i64,
        )
    };
    let mut footrope_value = vec![0u8; footrope_value_len as usize];
    let footrope_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            footrope_source.as_ptr(),
            footrope_source_written as i64,
            footrope_old.as_ptr(),
            footrope_old.len() as i64,
            footrope_new.as_ptr(),
            footrope_new.len() as i64,
            footrope_value.as_mut_ptr(),
            footrope_value.len() as i64,
        )
    };
    let footrope_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            footrope_value.as_ptr(),
            footrope_value_written as i64,
            footrope_needle.as_ptr(),
            footrope_needle.len() as i64,
        )
    };

    let mut selected_halo_index = 0i32;
    let mut best_halo_score = i32::MIN;
    let mut halo_index = 0i32;
    while halo_index < 3 {
        let mut candidate_halo_score = planisphere_value_written * 10 + planisphere_contains * 50;
        if halo_index == 1 {
            candidate_halo_score = nightglass_value_written * 10 + nightglass_index;
        } else if halo_index == 2 {
            candidate_halo_score = footrope_value_written * 10 + footrope_contains * 50;
        }

        let mut halo_bonus = 0i32;
        if halo_index == selected_antumbra_index {
            halo_bonus += 25;
        }
        if halo_index == selected_umbra_index {
            halo_bonus += 15;
        }
        if halo_index == selected_penumbra_index {
            halo_bonus += 5;
        }
        if halo_index == 0 && planisphere_contains != 0 {
            halo_bonus += 20;
        }
        if halo_index == 1 && nightglass_index >= 0 {
            halo_bonus += 10;
        }
        if halo_index == 2 && footrope_contains != 0 {
            halo_bonus += 30;
        }

        let halo_score = candidate_halo_score + halo_bonus;
        if halo_score > best_halo_score {
            best_halo_score = halo_score;
            selected_halo_index = halo_index;
        }

        halo_index += 1;
    }

    let mut selected_halo_ptr = planisphere_value.as_ptr();
    let mut selected_halo_written = planisphere_value_written;
    if selected_halo_index == 1 {
        selected_halo_ptr = nightglass_value.as_ptr();
        selected_halo_written = nightglass_value_written;
    } else if selected_halo_index == 2 {
        selected_halo_ptr = footrope_value.as_ptr();
        selected_halo_written = footrope_value_written;
    }

    let starwheel_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_halo_ptr,
            selected_halo_written as i64,
            starwheel_old.as_ptr(),
            starwheel_old.len() as i64,
            starwheel_new.as_ptr(),
            starwheel_new.len() as i64,
        )
    };
    let mut starwheel_value = vec![0u8; starwheel_value_len as usize];
    let starwheel_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_halo_ptr,
            selected_halo_written as i64,
            starwheel_old.as_ptr(),
            starwheel_old.len() as i64,
            starwheel_new.as_ptr(),
            starwheel_new.len() as i64,
            starwheel_value.as_mut_ptr(),
            starwheel_value.len() as i64,
        )
    };
    let starwheel_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            starwheel_value.as_ptr(),
            starwheel_value_written as i64,
            starwheel_needle.as_ptr(),
            starwheel_needle.len() as i64,
        )
    };

    let moonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_halo_ptr,
            selected_halo_written as i64,
            moonwake_extension.as_ptr(),
            moonwake_extension.len() as i64,
        )
    };
    let mut moonwake_value = vec![0u8; moonwake_value_len as usize];
    let moonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_halo_ptr,
            selected_halo_written as i64,
            moonwake_extension.as_ptr(),
            moonwake_extension.len() as i64,
            moonwake_value.as_mut_ptr(),
            moonwake_value.len() as i64,
        )
    };
    let moonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            moonwake_value.as_ptr(),
            moonwake_value_written as i64,
            moonwake_needle.as_ptr(),
            moonwake_needle.len() as i64,
        )
    };

    let ratline_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_halo_ptr,
            selected_halo_written as i64,
        )
    };
    let mut ratline_source = vec![0u8; ratline_source_len as usize];
    let ratline_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_halo_ptr,
            selected_halo_written as i64,
            ratline_source.as_mut_ptr(),
            ratline_source.len() as i64,
        )
    };
    let ratline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            ratline_source.as_ptr(),
            ratline_source_written as i64,
            ratline_old.as_ptr(),
            ratline_old.len() as i64,
            ratline_new.as_ptr(),
            ratline_new.len() as i64,
        )
    };
    let mut ratline_value = vec![0u8; ratline_value_len as usize];
    let ratline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            ratline_source.as_ptr(),
            ratline_source_written as i64,
            ratline_old.as_ptr(),
            ratline_old.len() as i64,
            ratline_new.as_ptr(),
            ratline_new.len() as i64,
            ratline_value.as_mut_ptr(),
            ratline_value.len() as i64,
        )
    };
    let ratline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            ratline_value.as_ptr(),
            ratline_value_written as i64,
            ratline_needle.as_ptr(),
            ratline_needle.len() as i64,
        )
    };

    let mut selected_aureole_index = 0i32;
    let mut best_aureole_score = i32::MIN;
    let mut aureole_index = 0i32;
    while aureole_index < 3 {
        let mut candidate_aureole_score = starwheel_value_written * 10 + starwheel_contains * 50;
        if aureole_index == 1 {
            candidate_aureole_score = moonwake_value_written * 10 + moonwake_index;
        } else if aureole_index == 2 {
            candidate_aureole_score = ratline_value_written * 10 + ratline_contains * 50;
        }

        let mut aureole_bonus = 0i32;
        if aureole_index == selected_halo_index {
            aureole_bonus += 25;
        }
        if aureole_index == selected_antumbra_index {
            aureole_bonus += 15;
        }
        if aureole_index == selected_umbra_index {
            aureole_bonus += 5;
        }
        if aureole_index == 0 && starwheel_contains != 0 {
            aureole_bonus += 20;
        }
        if aureole_index == 1 && moonwake_index >= 0 {
            aureole_bonus += 10;
        }
        if aureole_index == 2 && ratline_contains != 0 {
            aureole_bonus += 30;
        }

        let aureole_score = candidate_aureole_score + aureole_bonus;
        if aureole_score > best_aureole_score {
            best_aureole_score = aureole_score;
            selected_aureole_index = aureole_index;
        }

        aureole_index += 1;
    }

    let mut selected_aureole_ptr = starwheel_value.as_ptr();
    let mut selected_aureole_written = starwheel_value_written;
    if selected_aureole_index == 1 {
        selected_aureole_ptr = moonwake_value.as_ptr();
        selected_aureole_written = moonwake_value_written;
    } else if selected_aureole_index == 2 {
        selected_aureole_ptr = ratline_value.as_ptr();
        selected_aureole_written = ratline_value_written;
    }

    let planetarium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_aureole_ptr,
            selected_aureole_written as i64,
            planetarium_old.as_ptr(),
            planetarium_old.len() as i64,
            planetarium_new.as_ptr(),
            planetarium_new.len() as i64,
        )
    };
    let mut planetarium_value = vec![0u8; planetarium_value_len as usize];
    let planetarium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_aureole_ptr,
            selected_aureole_written as i64,
            planetarium_old.as_ptr(),
            planetarium_old.len() as i64,
            planetarium_new.as_ptr(),
            planetarium_new.len() as i64,
            planetarium_value.as_mut_ptr(),
            planetarium_value.len() as i64,
        )
    };
    let planetarium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            planetarium_value.as_ptr(),
            planetarium_value_written as i64,
            planetarium_needle.as_ptr(),
            planetarium_needle.len() as i64,
        )
    };

    let dayring_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_aureole_ptr,
            selected_aureole_written as i64,
            dayring_extension.as_ptr(),
            dayring_extension.len() as i64,
        )
    };
    let mut dayring_value = vec![0u8; dayring_value_len as usize];
    let dayring_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_aureole_ptr,
            selected_aureole_written as i64,
            dayring_extension.as_ptr(),
            dayring_extension.len() as i64,
            dayring_value.as_mut_ptr(),
            dayring_value.len() as i64,
        )
    };
    let dayring_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            dayring_value.as_ptr(),
            dayring_value_written as i64,
            dayring_needle.as_ptr(),
            dayring_needle.len() as i64,
        )
    };

    let shroud_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_aureole_ptr,
            selected_aureole_written as i64,
        )
    };
    let mut shroud_source = vec![0u8; shroud_source_len as usize];
    let shroud_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_aureole_ptr,
            selected_aureole_written as i64,
            shroud_source.as_mut_ptr(),
            shroud_source.len() as i64,
        )
    };
    let shroud_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            shroud_source.as_ptr(),
            shroud_source_written as i64,
            shroud_old.as_ptr(),
            shroud_old.len() as i64,
            shroud_new.as_ptr(),
            shroud_new.len() as i64,
        )
    };
    let mut shroud_value = vec![0u8; shroud_value_len as usize];
    let shroud_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            shroud_source.as_ptr(),
            shroud_source_written as i64,
            shroud_old.as_ptr(),
            shroud_old.len() as i64,
            shroud_new.as_ptr(),
            shroud_new.len() as i64,
            shroud_value.as_mut_ptr(),
            shroud_value.len() as i64,
        )
    };
    let shroud_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            shroud_value.as_ptr(),
            shroud_value_written as i64,
            shroud_needle.as_ptr(),
            shroud_needle.len() as i64,
        )
    };

    let mut selected_luminary_index = 0i32;
    let mut best_luminary_score = i32::MIN;
    let mut luminary_index = 0i32;
    while luminary_index < 3 {
        let mut candidate_luminary_score = planetarium_value_written * 10 + planetarium_contains * 50;
        if luminary_index == 1 {
            candidate_luminary_score = dayring_value_written * 10 + dayring_index;
        } else if luminary_index == 2 {
            candidate_luminary_score = shroud_value_written * 10 + shroud_contains * 50;
        }

        let mut luminary_bonus = 0i32;
        if luminary_index == selected_aureole_index {
            luminary_bonus += 25;
        }
        if luminary_index == selected_halo_index {
            luminary_bonus += 15;
        }
        if luminary_index == selected_antumbra_index {
            luminary_bonus += 5;
        }
        if luminary_index == 0 && planetarium_contains != 0 {
            luminary_bonus += 20;
        }
        if luminary_index == 1 && dayring_index >= 0 {
            luminary_bonus += 10;
        }
        if luminary_index == 2 && shroud_contains != 0 {
            luminary_bonus += 30;
        }

        let luminary_score = candidate_luminary_score + luminary_bonus;
        if luminary_score > best_luminary_score {
            best_luminary_score = luminary_score;
            selected_luminary_index = luminary_index;
        }

        luminary_index += 1;
    }

    let mut selected_luminary_ptr = planetarium_value.as_ptr();
    let mut selected_luminary_written = planetarium_value_written;
    if selected_luminary_index == 1 {
        selected_luminary_ptr = dayring_value.as_ptr();
        selected_luminary_written = dayring_value_written;
    } else if selected_luminary_index == 2 {
        selected_luminary_ptr = shroud_value.as_ptr();
        selected_luminary_written = shroud_value_written;
    }

    let orrery_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_luminary_ptr,
            selected_luminary_written as i64,
            orrery_old.as_ptr(),
            orrery_old.len() as i64,
            orrery_new.as_ptr(),
            orrery_new.len() as i64,
        )
    };
    let mut orrery_value = vec![0u8; orrery_value_len as usize];
    let orrery_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_luminary_ptr,
            selected_luminary_written as i64,
            orrery_old.as_ptr(),
            orrery_old.len() as i64,
            orrery_new.as_ptr(),
            orrery_new.len() as i64,
            orrery_value.as_mut_ptr(),
            orrery_value.len() as i64,
        )
    };
    let orrery_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            orrery_value.as_ptr(),
            orrery_value_written as i64,
            orrery_needle.as_ptr(),
            orrery_needle.len() as i64,
        )
    };

    let sunwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_luminary_ptr,
            selected_luminary_written as i64,
            sunwake_extension.as_ptr(),
            sunwake_extension.len() as i64,
        )
    };
    let mut sunwake_value = vec![0u8; sunwake_value_len as usize];
    let sunwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_luminary_ptr,
            selected_luminary_written as i64,
            sunwake_extension.as_ptr(),
            sunwake_extension.len() as i64,
            sunwake_value.as_mut_ptr(),
            sunwake_value.len() as i64,
        )
    };
    let sunwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            sunwake_value.as_ptr(),
            sunwake_value_written as i64,
            sunwake_needle.as_ptr(),
            sunwake_needle.len() as i64,
        )
    };

    let backstay_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_luminary_ptr,
            selected_luminary_written as i64,
        )
    };
    let mut backstay_source = vec![0u8; backstay_source_len as usize];
    let backstay_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_luminary_ptr,
            selected_luminary_written as i64,
            backstay_source.as_mut_ptr(),
            backstay_source.len() as i64,
        )
    };
    let backstay_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            backstay_source.as_ptr(),
            backstay_source_written as i64,
            backstay_old.as_ptr(),
            backstay_old.len() as i64,
            backstay_new.as_ptr(),
            backstay_new.len() as i64,
        )
    };
    let mut backstay_value = vec![0u8; backstay_value_len as usize];
    let backstay_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            backstay_source.as_ptr(),
            backstay_source_written as i64,
            backstay_old.as_ptr(),
            backstay_old.len() as i64,
            backstay_new.as_ptr(),
            backstay_new.len() as i64,
            backstay_value.as_mut_ptr(),
            backstay_value.len() as i64,
        )
    };
    let backstay_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            backstay_value.as_ptr(),
            backstay_value_written as i64,
            backstay_needle.as_ptr(),
            backstay_needle.len() as i64,
        )
    };

    let mut selected_heliopause_index = 0i32;
    let mut best_heliopause_score = i32::MIN;
    let mut heliopause_index = 0i32;
    while heliopause_index < 3 {
        let mut candidate_heliopause_score = orrery_value_written * 10 + orrery_contains * 50;
        if heliopause_index == 1 {
            candidate_heliopause_score = sunwake_value_written * 10 + sunwake_index;
        } else if heliopause_index == 2 {
            candidate_heliopause_score = backstay_value_written * 10 + backstay_contains * 50;
        }

        let mut heliopause_bonus = 0i32;
        if heliopause_index == selected_luminary_index {
            heliopause_bonus += 25;
        }
        if heliopause_index == selected_aureole_index {
            heliopause_bonus += 15;
        }
        if heliopause_index == selected_halo_index {
            heliopause_bonus += 5;
        }
        if heliopause_index == 0 && orrery_contains != 0 {
            heliopause_bonus += 20;
        }
        if heliopause_index == 1 && sunwake_index >= 0 {
            heliopause_bonus += 10;
        }
        if heliopause_index == 2 && backstay_contains != 0 {
            heliopause_bonus += 30;
        }

        let heliopause_score = candidate_heliopause_score + heliopause_bonus;
        if heliopause_score > best_heliopause_score {
            best_heliopause_score = heliopause_score;
            selected_heliopause_index = heliopause_index;
        }

        heliopause_index += 1;
    }

    let mut selected_heliopause_ptr = orrery_value.as_ptr();
    let mut selected_heliopause_written = orrery_value_written;
    if selected_heliopause_index == 1 {
        selected_heliopause_ptr = sunwake_value.as_ptr();
        selected_heliopause_written = sunwake_value_written;
    } else if selected_heliopause_index == 2 {
        selected_heliopause_ptr = backstay_value.as_ptr();
        selected_heliopause_written = backstay_value_written;
    }

    let zenith_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_heliopause_ptr,
            selected_heliopause_written as i64,
            zenith_old.as_ptr(),
            zenith_old.len() as i64,
            zenith_new.as_ptr(),
            zenith_new.len() as i64,
        )
    };
    let mut zenith_value = vec![0u8; zenith_value_len as usize];
    let zenith_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_heliopause_ptr,
            selected_heliopause_written as i64,
            zenith_old.as_ptr(),
            zenith_old.len() as i64,
            zenith_new.as_ptr(),
            zenith_new.len() as i64,
            zenith_value.as_mut_ptr(),
            zenith_value.len() as i64,
        )
    };
    let zenith_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            zenith_value.as_ptr(),
            zenith_value_written as i64,
            zenith_needle.as_ptr(),
            zenith_needle.len() as i64,
        )
    };

    let solstice_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_heliopause_ptr,
            selected_heliopause_written as i64,
            solstice_extension.as_ptr(),
            solstice_extension.len() as i64,
        )
    };
    let mut solstice_value = vec![0u8; solstice_value_len as usize];
    let solstice_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_heliopause_ptr,
            selected_heliopause_written as i64,
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

    let forestay_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_heliopause_ptr,
            selected_heliopause_written as i64,
        )
    };
    let mut forestay_source = vec![0u8; forestay_source_len as usize];
    let forestay_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_heliopause_ptr,
            selected_heliopause_written as i64,
            forestay_source.as_mut_ptr(),
            forestay_source.len() as i64,
        )
    };
    let forestay_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            forestay_source.as_ptr(),
            forestay_source_written as i64,
            forestay_old.as_ptr(),
            forestay_old.len() as i64,
            forestay_new.as_ptr(),
            forestay_new.len() as i64,
        )
    };
    let mut forestay_value = vec![0u8; forestay_value_len as usize];
    let forestay_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            forestay_source.as_ptr(),
            forestay_source_written as i64,
            forestay_old.as_ptr(),
            forestay_old.len() as i64,
            forestay_new.as_ptr(),
            forestay_new.len() as i64,
            forestay_value.as_mut_ptr(),
            forestay_value.len() as i64,
        )
    };
    let forestay_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            forestay_value.as_ptr(),
            forestay_value_written as i64,
            forestay_needle.as_ptr(),
            forestay_needle.len() as i64,
        )
    };

    let mut selected_radiant_index = 0i32;
    let mut best_radiant_score = i32::MIN;
    let mut radiant_index = 0i32;
    while radiant_index < 3 {
        let mut candidate_radiant_score = zenith_value_written * 10 + zenith_contains * 50;
        if radiant_index == 1 {
            candidate_radiant_score = solstice_value_written * 10 + solstice_index;
        } else if radiant_index == 2 {
            candidate_radiant_score = forestay_value_written * 10 + forestay_contains * 50;
        }

        let mut radiant_bonus = 0i32;
        if radiant_index == selected_heliopause_index {
            radiant_bonus += 25;
        }
        if radiant_index == selected_luminary_index {
            radiant_bonus += 15;
        }
        if radiant_index == selected_aureole_index {
            radiant_bonus += 5;
        }
        if radiant_index == 0 && zenith_contains != 0 {
            radiant_bonus += 20;
        }
        if radiant_index == 1 && solstice_index >= 0 {
            radiant_bonus += 10;
        }
        if radiant_index == 2 && forestay_contains != 0 {
            radiant_bonus += 30;
        }

        let radiant_score = candidate_radiant_score + radiant_bonus;
        if radiant_score > best_radiant_score {
            best_radiant_score = radiant_score;
            selected_radiant_index = radiant_index;
        }

        radiant_index += 1;
    }

    let mut selected_radiant_ptr = zenith_value.as_ptr();
    let mut selected_radiant_written = zenith_value_written;
    if selected_radiant_index == 1 {
        selected_radiant_ptr = solstice_value.as_ptr();
        selected_radiant_written = solstice_value_written;
    } else if selected_radiant_index == 2 {
        selected_radiant_ptr = forestay_value.as_ptr();
        selected_radiant_written = forestay_value_written;
    }

    let azimuth_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_radiant_ptr,
            selected_radiant_written as i64,
            azimuth_old.as_ptr(),
            azimuth_old.len() as i64,
            azimuth_new.as_ptr(),
            azimuth_new.len() as i64,
        )
    };
    let mut azimuth_value = vec![0u8; azimuth_value_len as usize];
    let azimuth_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_radiant_ptr,
            selected_radiant_written as i64,
            azimuth_old.as_ptr(),
            azimuth_old.len() as i64,
            azimuth_new.as_ptr(),
            azimuth_new.len() as i64,
            azimuth_value.as_mut_ptr(),
            azimuth_value.len() as i64,
        )
    };
    let azimuth_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            azimuth_value.as_ptr(),
            azimuth_value_written as i64,
            azimuth_needle.as_ptr(),
            azimuth_needle.len() as i64,
        )
    };

    let perihelion_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_radiant_ptr,
            selected_radiant_written as i64,
            perihelion_extension.as_ptr(),
            perihelion_extension.len() as i64,
        )
    };
    let mut perihelion_value = vec![0u8; perihelion_value_len as usize];
    let perihelion_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_radiant_ptr,
            selected_radiant_written as i64,
            perihelion_extension.as_ptr(),
            perihelion_extension.len() as i64,
            perihelion_value.as_mut_ptr(),
            perihelion_value.len() as i64,
        )
    };
    let perihelion_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            perihelion_value.as_ptr(),
            perihelion_value_written as i64,
            perihelion_needle.as_ptr(),
            perihelion_needle.len() as i64,
        )
    };

    let stayline_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_radiant_ptr,
            selected_radiant_written as i64,
        )
    };
    let mut stayline_source = vec![0u8; stayline_source_len as usize];
    let stayline_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_radiant_ptr,
            selected_radiant_written as i64,
            stayline_source.as_mut_ptr(),
            stayline_source.len() as i64,
        )
    };
    let stayline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            stayline_source.as_ptr(),
            stayline_source_written as i64,
            stayline_old.as_ptr(),
            stayline_old.len() as i64,
            stayline_new.as_ptr(),
            stayline_new.len() as i64,
        )
    };
    let mut stayline_value = vec![0u8; stayline_value_len as usize];
    let stayline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            stayline_source.as_ptr(),
            stayline_source_written as i64,
            stayline_old.as_ptr(),
            stayline_old.len() as i64,
            stayline_new.as_ptr(),
            stayline_new.len() as i64,
            stayline_value.as_mut_ptr(),
            stayline_value.len() as i64,
        )
    };
    let stayline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            stayline_value.as_ptr(),
            stayline_value_written as i64,
            stayline_needle.as_ptr(),
            stayline_needle.len() as i64,
        )
    };

    let mut selected_azimuth_index = 0i32;
    let mut best_azimuth_score = i32::MIN;
    let mut azimuth_index = 0i32;
    while azimuth_index < 3 {
        let mut candidate_azimuth_score = azimuth_value_written * 10 + azimuth_contains * 50;
        if azimuth_index == 1 {
            candidate_azimuth_score = perihelion_value_written * 10 + perihelion_index;
        } else if azimuth_index == 2 {
            candidate_azimuth_score = stayline_value_written * 10 + stayline_contains * 50;
        }

        let mut azimuth_bonus = 0i32;
        if azimuth_index == selected_radiant_index {
            azimuth_bonus += 25;
        }
        if azimuth_index == selected_heliopause_index {
            azimuth_bonus += 15;
        }
        if azimuth_index == selected_luminary_index {
            azimuth_bonus += 5;
        }
        if azimuth_index == 0 && azimuth_contains != 0 {
            azimuth_bonus += 20;
        }
        if azimuth_index == 1 && perihelion_index >= 0 {
            azimuth_bonus += 10;
        }
        if azimuth_index == 2 && stayline_contains != 0 {
            azimuth_bonus += 30;
        }

        let azimuth_score = candidate_azimuth_score + azimuth_bonus;
        if azimuth_score > best_azimuth_score {
            best_azimuth_score = azimuth_score;
            selected_azimuth_index = azimuth_index;
        }

        azimuth_index += 1;
    }

    let mut selected_azimuth_ptr = azimuth_value.as_ptr();
    let mut selected_azimuth_written = azimuth_value_written;
    if selected_azimuth_index == 1 {
        selected_azimuth_ptr = perihelion_value.as_ptr();
        selected_azimuth_written = perihelion_value_written;
    } else if selected_azimuth_index == 2 {
        selected_azimuth_ptr = stayline_value.as_ptr();
        selected_azimuth_written = stayline_value_written;
    }

    let sidereal_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_azimuth_ptr,
            selected_azimuth_written as i64,
            sidereal_old.as_ptr(),
            sidereal_old.len() as i64,
            sidereal_new.as_ptr(),
            sidereal_new.len() as i64,
        )
    };
    let mut sidereal_value = vec![0u8; sidereal_value_len as usize];
    let sidereal_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_azimuth_ptr,
            selected_azimuth_written as i64,
            sidereal_old.as_ptr(),
            sidereal_old.len() as i64,
            sidereal_new.as_ptr(),
            sidereal_new.len() as i64,
            sidereal_value.as_mut_ptr(),
            sidereal_value.len() as i64,
        )
    };
    let sidereal_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            sidereal_value.as_ptr(),
            sidereal_value_written as i64,
            sidereal_needle.as_ptr(),
            sidereal_needle.len() as i64,
        )
    };

    let chronwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_azimuth_ptr,
            selected_azimuth_written as i64,
            chronwake_extension.as_ptr(),
            chronwake_extension.len() as i64,
        )
    };
    let mut chronwake_value = vec![0u8; chronwake_value_len as usize];
    let chronwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_azimuth_ptr,
            selected_azimuth_written as i64,
            chronwake_extension.as_ptr(),
            chronwake_extension.len() as i64,
            chronwake_value.as_mut_ptr(),
            chronwake_value.len() as i64,
        )
    };
    let chronwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            chronwake_value.as_ptr(),
            chronwake_value_written as i64,
            chronwake_needle.as_ptr(),
            chronwake_needle.len() as i64,
        )
    };

    let leechline_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_azimuth_ptr,
            selected_azimuth_written as i64,
        )
    };
    let mut leechline_source = vec![0u8; leechline_source_len as usize];
    let leechline_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_azimuth_ptr,
            selected_azimuth_written as i64,
            leechline_source.as_mut_ptr(),
            leechline_source.len() as i64,
        )
    };
    let leechline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            leechline_source.as_ptr(),
            leechline_source_written as i64,
            leechline_old.as_ptr(),
            leechline_old.len() as i64,
            leechline_new.as_ptr(),
            leechline_new.len() as i64,
        )
    };
    let mut leechline_value = vec![0u8; leechline_value_len as usize];
    let leechline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            leechline_source.as_ptr(),
            leechline_source_written as i64,
            leechline_old.as_ptr(),
            leechline_old.len() as i64,
            leechline_new.as_ptr(),
            leechline_new.len() as i64,
            leechline_value.as_mut_ptr(),
            leechline_value.len() as i64,
        )
    };
    let leechline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            leechline_value.as_ptr(),
            leechline_value_written as i64,
            leechline_needle.as_ptr(),
            leechline_needle.len() as i64,
        )
    };

    let mut selected_sidereal_index = 0i32;
    let mut best_sidereal_score = i32::MIN;
    let mut sidereal_index = 0i32;
    while sidereal_index < 3 {
        let mut candidate_sidereal_score = sidereal_value_written * 10 + sidereal_contains * 50;
        if sidereal_index == 1 {
            candidate_sidereal_score = chronwake_value_written * 10 + chronwake_index;
        } else if sidereal_index == 2 {
            candidate_sidereal_score = leechline_value_written * 10 + leechline_contains * 50;
        }

        let mut sidereal_bonus = 0i32;
        if sidereal_index == selected_azimuth_index {
            sidereal_bonus += 25;
        }
        if sidereal_index == selected_radiant_index {
            sidereal_bonus += 15;
        }
        if sidereal_index == selected_heliopause_index {
            sidereal_bonus += 5;
        }
        if sidereal_index == 0 && sidereal_contains != 0 {
            sidereal_bonus += 20;
        }
        if sidereal_index == 1 && chronwake_index >= 0 {
            sidereal_bonus += 10;
        }
        if sidereal_index == 2 && leechline_contains != 0 {
            sidereal_bonus += 30;
        }

        let sidereal_score = candidate_sidereal_score + sidereal_bonus;
        if sidereal_score > best_sidereal_score {
            best_sidereal_score = sidereal_score;
            selected_sidereal_index = sidereal_index;
        }

        sidereal_index += 1;
    }

    let mut selected_sidereal_ptr = sidereal_value.as_ptr();
    let mut selected_sidereal_written = sidereal_value_written;
    if selected_sidereal_index == 1 {
        selected_sidereal_ptr = chronwake_value.as_ptr();
        selected_sidereal_written = chronwake_value_written;
    } else if selected_sidereal_index == 2 {
        selected_sidereal_ptr = leechline_value.as_ptr();
        selected_sidereal_written = leechline_value_written;
    }

    let ephemeris_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_sidereal_ptr,
            selected_sidereal_written as i64,
            ephemeris_old.as_ptr(),
            ephemeris_old.len() as i64,
            ephemeris_new.as_ptr(),
            ephemeris_new.len() as i64,
        )
    };
    let mut ephemeris_value = vec![0u8; ephemeris_value_len as usize];
    let ephemeris_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_sidereal_ptr,
            selected_sidereal_written as i64,
            ephemeris_old.as_ptr(),
            ephemeris_old.len() as i64,
            ephemeris_new.as_ptr(),
            ephemeris_new.len() as i64,
            ephemeris_value.as_mut_ptr(),
            ephemeris_value.len() as i64,
        )
    };
    let ephemeris_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            ephemeris_value.as_ptr(),
            ephemeris_value_written as i64,
            ephemeris_needle.as_ptr(),
            ephemeris_needle.len() as i64,
        )
    };

    let tidewake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_sidereal_ptr,
            selected_sidereal_written as i64,
            tidewake_extension.as_ptr(),
            tidewake_extension.len() as i64,
        )
    };
    let mut tidewake_value = vec![0u8; tidewake_value_len as usize];
    let tidewake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_sidereal_ptr,
            selected_sidereal_written as i64,
            tidewake_extension.as_ptr(),
            tidewake_extension.len() as i64,
            tidewake_value.as_mut_ptr(),
            tidewake_value.len() as i64,
        )
    };
    let tidewake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            tidewake_value.as_ptr(),
            tidewake_value_written as i64,
            tidewake_needle.as_ptr(),
            tidewake_needle.len() as i64,
        )
    };

    let sheetbend_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_sidereal_ptr,
            selected_sidereal_written as i64,
        )
    };
    let mut sheetbend_source = vec![0u8; sheetbend_source_len as usize];
    let sheetbend_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_sidereal_ptr,
            selected_sidereal_written as i64,
            sheetbend_source.as_mut_ptr(),
            sheetbend_source.len() as i64,
        )
    };
    let sheetbend_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            sheetbend_source.as_ptr(),
            sheetbend_source_written as i64,
            sheetbend_old.as_ptr(),
            sheetbend_old.len() as i64,
            sheetbend_new.as_ptr(),
            sheetbend_new.len() as i64,
        )
    };
    let mut sheetbend_value = vec![0u8; sheetbend_value_len as usize];
    let sheetbend_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            sheetbend_source.as_ptr(),
            sheetbend_source_written as i64,
            sheetbend_old.as_ptr(),
            sheetbend_old.len() as i64,
            sheetbend_new.as_ptr(),
            sheetbend_new.len() as i64,
            sheetbend_value.as_mut_ptr(),
            sheetbend_value.len() as i64,
        )
    };
    let sheetbend_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            sheetbend_value.as_ptr(),
            sheetbend_value_written as i64,
            sheetbend_needle.as_ptr(),
            sheetbend_needle.len() as i64,
        )
    };

    let mut selected_ephemeris_index = 0i32;
    let mut best_ephemeris_score = i32::MIN;
    let mut ephemeris_index = 0i32;
    while ephemeris_index < 3 {
        let mut candidate_ephemeris_score = ephemeris_value_written * 10 + ephemeris_contains * 50;
        if ephemeris_index == 1 {
            candidate_ephemeris_score = tidewake_value_written * 10 + tidewake_index;
        } else if ephemeris_index == 2 {
            candidate_ephemeris_score = sheetbend_value_written * 10 + sheetbend_contains * 50;
        }

        let mut ephemeris_bonus = 0i32;
        if ephemeris_index == selected_sidereal_index {
            ephemeris_bonus += 25;
        }
        if ephemeris_index == selected_azimuth_index {
            ephemeris_bonus += 15;
        }
        if ephemeris_index == selected_radiant_index {
            ephemeris_bonus += 5;
        }
        if ephemeris_index == 0 && ephemeris_contains != 0 {
            ephemeris_bonus += 20;
        }
        if ephemeris_index == 1 && tidewake_index >= 0 {
            ephemeris_bonus += 10;
        }
        if ephemeris_index == 2 && sheetbend_contains != 0 {
            ephemeris_bonus += 30;
        }

        let ephemeris_score = candidate_ephemeris_score + ephemeris_bonus;
        if ephemeris_score > best_ephemeris_score {
            best_ephemeris_score = ephemeris_score;
            selected_ephemeris_index = ephemeris_index;
        }

        ephemeris_index += 1;
    }

    let mut selected_ephemeris_ptr = ephemeris_value.as_ptr();
    let mut selected_ephemeris_written = ephemeris_value_written;
    if selected_ephemeris_index == 1 {
        selected_ephemeris_ptr = tidewake_value.as_ptr();
        selected_ephemeris_written = tidewake_value_written;
    } else if selected_ephemeris_index == 2 {
        selected_ephemeris_ptr = sheetbend_value.as_ptr();
        selected_ephemeris_written = sheetbend_value_written;
    }

    let almanac_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_ephemeris_ptr,
            selected_ephemeris_written as i64,
            almanac_old.as_ptr(),
            almanac_old.len() as i64,
            almanac_new.as_ptr(),
            almanac_new.len() as i64,
        )
    };
    let mut almanac_value = vec![0u8; almanac_value_len as usize];
    let almanac_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_ephemeris_ptr,
            selected_ephemeris_written as i64,
            almanac_old.as_ptr(),
            almanac_old.len() as i64,
            almanac_new.as_ptr(),
            almanac_new.len() as i64,
            almanac_value.as_mut_ptr(),
            almanac_value.len() as i64,
        )
    };
    let almanac_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            almanac_value.as_ptr(),
            almanac_value_written as i64,
            almanac_needle.as_ptr(),
            almanac_needle.len() as i64,
        )
    };

    let starwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_ephemeris_ptr,
            selected_ephemeris_written as i64,
            starwake_extension.as_ptr(),
            starwake_extension.len() as i64,
        )
    };
    let mut starwake_value = vec![0u8; starwake_value_len as usize];
    let starwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_ephemeris_ptr,
            selected_ephemeris_written as i64,
            starwake_extension.as_ptr(),
            starwake_extension.len() as i64,
            starwake_value.as_mut_ptr(),
            starwake_value.len() as i64,
        )
    };
    let starwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            starwake_value.as_ptr(),
            starwake_value_written as i64,
            starwake_needle.as_ptr(),
            starwake_needle.len() as i64,
        )
    };

    let reefline_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_ephemeris_ptr,
            selected_ephemeris_written as i64,
        )
    };
    let mut reefline_source = vec![0u8; reefline_source_len as usize];
    let reefline_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_ephemeris_ptr,
            selected_ephemeris_written as i64,
            reefline_source.as_mut_ptr(),
            reefline_source.len() as i64,
        )
    };
    let reefline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            reefline_source.as_ptr(),
            reefline_source_written as i64,
            reefline_old.as_ptr(),
            reefline_old.len() as i64,
            reefline_new.as_ptr(),
            reefline_new.len() as i64,
        )
    };
    let mut reefline_value = vec![0u8; reefline_value_len as usize];
    let reefline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            reefline_source.as_ptr(),
            reefline_source_written as i64,
            reefline_old.as_ptr(),
            reefline_old.len() as i64,
            reefline_new.as_ptr(),
            reefline_new.len() as i64,
            reefline_value.as_mut_ptr(),
            reefline_value.len() as i64,
        )
    };
    let reefline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            reefline_value.as_ptr(),
            reefline_value_written as i64,
            reefline_needle.as_ptr(),
            reefline_needle.len() as i64,
        )
    };

    let mut selected_almanac_index = 0i32;
    let mut best_almanac_score = i32::MIN;
    let mut almanac_index = 0i32;
    while almanac_index < 3 {
        let mut candidate_almanac_score = almanac_value_written * 10 + almanac_contains * 50;
        if almanac_index == 1 {
            candidate_almanac_score = starwake_value_written * 10 + starwake_index;
        } else if almanac_index == 2 {
            candidate_almanac_score = reefline_value_written * 10 + reefline_contains * 50;
        }

        let mut almanac_bonus = 0i32;
        if almanac_index == selected_ephemeris_index {
            almanac_bonus += 25;
        }
        if almanac_index == selected_sidereal_index {
            almanac_bonus += 15;
        }
        if almanac_index == selected_azimuth_index {
            almanac_bonus += 5;
        }
        if almanac_index == 0 && almanac_contains != 0 {
            almanac_bonus += 20;
        }
        if almanac_index == 1 && starwake_index >= 0 {
            almanac_bonus += 10;
        }
        if almanac_index == 2 && reefline_contains != 0 {
            almanac_bonus += 30;
        }

        let almanac_score = candidate_almanac_score + almanac_bonus;
        if almanac_score > best_almanac_score {
            best_almanac_score = almanac_score;
            selected_almanac_index = almanac_index;
        }

        almanac_index += 1;
    }

    let mut selected_almanac_ptr = almanac_value.as_ptr();
    let mut selected_almanac_written = almanac_value_written;
    if selected_almanac_index == 1 {
        selected_almanac_ptr = starwake_value.as_ptr();
        selected_almanac_written = starwake_value_written;
    } else if selected_almanac_index == 2 {
        selected_almanac_ptr = reefline_value.as_ptr();
        selected_almanac_written = reefline_value_written;
    }

    let nocturne_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_almanac_ptr,
            selected_almanac_written as i64,
            nocturne_old.as_ptr(),
            nocturne_old.len() as i64,
            nocturne_new.as_ptr(),
            nocturne_new.len() as i64,
        )
    };
    let mut nocturne_value = vec![0u8; nocturne_value_len as usize];
    let nocturne_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_almanac_ptr,
            selected_almanac_written as i64,
            nocturne_old.as_ptr(),
            nocturne_old.len() as i64,
            nocturne_new.as_ptr(),
            nocturne_new.len() as i64,
            nocturne_value.as_mut_ptr(),
            nocturne_value.len() as i64,
        )
    };
    let nocturne_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            nocturne_value.as_ptr(),
            nocturne_value_written as i64,
            nocturne_needle.as_ptr(),
            nocturne_needle.len() as i64,
        )
    };

    let dawnwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_almanac_ptr,
            selected_almanac_written as i64,
            dawnwake_extension.as_ptr(),
            dawnwake_extension.len() as i64,
        )
    };
    let mut dawnwake_value = vec![0u8; dawnwake_value_len as usize];
    let dawnwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_almanac_ptr,
            selected_almanac_written as i64,
            dawnwake_extension.as_ptr(),
            dawnwake_extension.len() as i64,
            dawnwake_value.as_mut_ptr(),
            dawnwake_value.len() as i64,
        )
    };
    let dawnwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            dawnwake_value.as_ptr(),
            dawnwake_value_written as i64,
            dawnwake_needle.as_ptr(),
            dawnwake_needle.len() as i64,
        )
    };

    let luffline_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_almanac_ptr,
            selected_almanac_written as i64,
        )
    };
    let mut luffline_source = vec![0u8; luffline_source_len as usize];
    let luffline_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_almanac_ptr,
            selected_almanac_written as i64,
            luffline_source.as_mut_ptr(),
            luffline_source.len() as i64,
        )
    };
    let luffline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            luffline_source.as_ptr(),
            luffline_source_written as i64,
            luffline_old.as_ptr(),
            luffline_old.len() as i64,
            luffline_new.as_ptr(),
            luffline_new.len() as i64,
        )
    };
    let mut luffline_value = vec![0u8; luffline_value_len as usize];
    let luffline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            luffline_source.as_ptr(),
            luffline_source_written as i64,
            luffline_old.as_ptr(),
            luffline_old.len() as i64,
            luffline_new.as_ptr(),
            luffline_new.len() as i64,
            luffline_value.as_mut_ptr(),
            luffline_value.len() as i64,
        )
    };
    let luffline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            luffline_value.as_ptr(),
            luffline_value_written as i64,
            luffline_needle.as_ptr(),
            luffline_needle.len() as i64,
        )
    };

    let mut selected_nocturne_index = 0i32;
    let mut best_nocturne_score = i32::MIN;
    let mut nocturne_index = 0i32;
    while nocturne_index < 3 {
        let mut candidate_nocturne_score = nocturne_value_written * 10 + nocturne_contains * 50;
        if nocturne_index == 1 {
            candidate_nocturne_score = dawnwake_value_written * 10 + dawnwake_index;
        } else if nocturne_index == 2 {
            candidate_nocturne_score = luffline_value_written * 10 + luffline_contains * 50;
        }

        let mut nocturne_bonus = 0i32;
        if nocturne_index == selected_almanac_index {
            nocturne_bonus += 25;
        }
        if nocturne_index == selected_ephemeris_index {
            nocturne_bonus += 15;
        }
        if nocturne_index == selected_sidereal_index {
            nocturne_bonus += 5;
        }
        if nocturne_index == 0 && nocturne_contains != 0 {
            nocturne_bonus += 20;
        }
        if nocturne_index == 1 && dawnwake_index >= 0 {
            nocturne_bonus += 10;
        }
        if nocturne_index == 2 && luffline_contains != 0 {
            nocturne_bonus += 30;
        }

        let nocturne_score = candidate_nocturne_score + nocturne_bonus;
        if nocturne_score > best_nocturne_score {
            best_nocturne_score = nocturne_score;
            selected_nocturne_index = nocturne_index;
        }

        nocturne_index += 1;
    }

    let mut selected_nocturne_ptr = nocturne_value.as_ptr();
    let mut selected_nocturne_written = nocturne_value_written;
    if selected_nocturne_index == 1 {
        selected_nocturne_ptr = dawnwake_value.as_ptr();
        selected_nocturne_written = dawnwake_value_written;
    } else if selected_nocturne_index == 2 {
        selected_nocturne_ptr = luffline_value.as_ptr();
        selected_nocturne_written = luffline_value_written;
    }

    let solarium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_nocturne_ptr,
            selected_nocturne_written as i64,
            solarium_old.as_ptr(),
            solarium_old.len() as i64,
            solarium_new.as_ptr(),
            solarium_new.len() as i64,
        )
    };
    let mut solarium_value = vec![0u8; solarium_value_len as usize];
    let solarium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_nocturne_ptr,
            selected_nocturne_written as i64,
            solarium_old.as_ptr(),
            solarium_old.len() as i64,
            solarium_new.as_ptr(),
            solarium_new.len() as i64,
            solarium_value.as_mut_ptr(),
            solarium_value.len() as i64,
        )
    };
    let solarium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            solarium_value.as_ptr(),
            solarium_value_written as i64,
            solarium_needle.as_ptr(),
            solarium_needle.len() as i64,
        )
    };

    let emberwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_nocturne_ptr,
            selected_nocturne_written as i64,
            emberwake_extension.as_ptr(),
            emberwake_extension.len() as i64,
        )
    };
    let mut emberwake_value = vec![0u8; emberwake_value_len as usize];
    let emberwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_nocturne_ptr,
            selected_nocturne_written as i64,
            emberwake_extension.as_ptr(),
            emberwake_extension.len() as i64,
            emberwake_value.as_mut_ptr(),
            emberwake_value.len() as i64,
        )
    };
    let emberwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            emberwake_value.as_ptr(),
            emberwake_value_written as i64,
            emberwake_needle.as_ptr(),
            emberwake_needle.len() as i64,
        )
    };

    let tackline_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_nocturne_ptr,
            selected_nocturne_written as i64,
        )
    };
    let mut tackline_source = vec![0u8; tackline_source_len as usize];
    let tackline_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_nocturne_ptr,
            selected_nocturne_written as i64,
            tackline_source.as_mut_ptr(),
            tackline_source.len() as i64,
        )
    };
    let tackline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            tackline_source.as_ptr(),
            tackline_source_written as i64,
            tackline_old.as_ptr(),
            tackline_old.len() as i64,
            tackline_new.as_ptr(),
            tackline_new.len() as i64,
        )
    };
    let mut tackline_value = vec![0u8; tackline_value_len as usize];
    let tackline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            tackline_source.as_ptr(),
            tackline_source_written as i64,
            tackline_old.as_ptr(),
            tackline_old.len() as i64,
            tackline_new.as_ptr(),
            tackline_new.len() as i64,
            tackline_value.as_mut_ptr(),
            tackline_value.len() as i64,
        )
    };
    let tackline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            tackline_value.as_ptr(),
            tackline_value_written as i64,
            tackline_needle.as_ptr(),
            tackline_needle.len() as i64,
        )
    };

    let mut selected_solarium_index = 0i32;
    let mut best_solarium_score = i32::MIN;
    let mut solarium_index = 0i32;
    while solarium_index < 3 {
        let mut candidate_solarium_score = solarium_value_written * 10 + solarium_contains * 50;
        if solarium_index == 1 {
            candidate_solarium_score = emberwake_value_written * 10 + emberwake_index;
        } else if solarium_index == 2 {
            candidate_solarium_score = tackline_value_written * 10 + tackline_contains * 50;
        }

        let mut solarium_bonus = 0i32;
        if solarium_index == selected_nocturne_index {
            solarium_bonus += 25;
        }
        if solarium_index == selected_almanac_index {
            solarium_bonus += 15;
        }
        if solarium_index == selected_ephemeris_index {
            solarium_bonus += 5;
        }
        if solarium_index == 0 && solarium_contains != 0 {
            solarium_bonus += 20;
        }
        if solarium_index == 1 && emberwake_index >= 0 {
            solarium_bonus += 10;
        }
        if solarium_index == 2 && tackline_contains != 0 {
            solarium_bonus += 30;
        }

        let solarium_score = candidate_solarium_score + solarium_bonus;
        if solarium_score > best_solarium_score {
            best_solarium_score = solarium_score;
            selected_solarium_index = solarium_index;
        }

        solarium_index += 1;
    }

    let mut selected_solarium_ptr = solarium_value.as_ptr();
    let mut selected_solarium_written = solarium_value_written;
    if selected_solarium_index == 1 {
        selected_solarium_ptr = emberwake_value.as_ptr();
        selected_solarium_written = emberwake_value_written;
    } else if selected_solarium_index == 2 {
        selected_solarium_ptr = tackline_value.as_ptr();
        selected_solarium_written = tackline_value_written;
    }

    let aurorium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_solarium_ptr,
            selected_solarium_written as i64,
            aurorium_old.as_ptr(),
            aurorium_old.len() as i64,
            aurorium_new.as_ptr(),
            aurorium_new.len() as i64,
        )
    };
    let mut aurorium_value = vec![0u8; aurorium_value_len as usize];
    let aurorium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_solarium_ptr,
            selected_solarium_written as i64,
            aurorium_old.as_ptr(),
            aurorium_old.len() as i64,
            aurorium_new.as_ptr(),
            aurorium_new.len() as i64,
            aurorium_value.as_mut_ptr(),
            aurorium_value.len() as i64,
        )
    };
    let aurorium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            aurorium_value.as_ptr(),
            aurorium_value_written as i64,
            aurorium_needle.as_ptr(),
            aurorium_needle.len() as i64,
        )
    };

    let glimmerwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_solarium_ptr,
            selected_solarium_written as i64,
            glimmerwake_extension.as_ptr(),
            glimmerwake_extension.len() as i64,
        )
    };
    let mut glimmerwake_value = vec![0u8; glimmerwake_value_len as usize];
    let glimmerwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_solarium_ptr,
            selected_solarium_written as i64,
            glimmerwake_extension.as_ptr(),
            glimmerwake_extension.len() as i64,
            glimmerwake_value.as_mut_ptr(),
            glimmerwake_value.len() as i64,
        )
    };
    let glimmerwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            glimmerwake_value.as_ptr(),
            glimmerwake_value_written as i64,
            glimmerwake_needle.as_ptr(),
            glimmerwake_needle.len() as i64,
        )
    };

    let keelline_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_solarium_ptr,
            selected_solarium_written as i64,
        )
    };
    let mut keelline_source = vec![0u8; keelline_source_len as usize];
    let keelline_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_solarium_ptr,
            selected_solarium_written as i64,
            keelline_source.as_mut_ptr(),
            keelline_source.len() as i64,
        )
    };
    let keelline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            keelline_source.as_ptr(),
            keelline_source_written as i64,
            keelline_old.as_ptr(),
            keelline_old.len() as i64,
            keelline_new.as_ptr(),
            keelline_new.len() as i64,
        )
    };
    let mut keelline_value = vec![0u8; keelline_value_len as usize];
    let keelline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            keelline_source.as_ptr(),
            keelline_source_written as i64,
            keelline_old.as_ptr(),
            keelline_old.len() as i64,
            keelline_new.as_ptr(),
            keelline_new.len() as i64,
            keelline_value.as_mut_ptr(),
            keelline_value.len() as i64,
        )
    };
    let keelline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            keelline_value.as_ptr(),
            keelline_value_written as i64,
            keelline_needle.as_ptr(),
            keelline_needle.len() as i64,
        )
    };

    let mut selected_aurorium_index = 0i32;
    let mut best_aurorium_score = i32::MIN;
    let mut aurorium_index = 0i32;
    while aurorium_index < 3 {
        let mut candidate_aurorium_score = aurorium_value_written * 10 + aurorium_contains * 50;
        if aurorium_index == 1 {
            candidate_aurorium_score = glimmerwake_value_written * 10 + glimmerwake_index;
        } else if aurorium_index == 2 {
            candidate_aurorium_score = keelline_value_written * 10 + keelline_contains * 50;
        }

        let mut aurorium_bonus = 0i32;
        if aurorium_index == selected_solarium_index {
            aurorium_bonus += 25;
        }
        if aurorium_index == selected_nocturne_index {
            aurorium_bonus += 15;
        }
        if aurorium_index == selected_almanac_index {
            aurorium_bonus += 5;
        }
        if aurorium_index == 0 && aurorium_contains != 0 {
            aurorium_bonus += 20;
        }
        if aurorium_index == 1 && glimmerwake_index >= 0 {
            aurorium_bonus += 10;
        }
        if aurorium_index == 2 && keelline_contains != 0 {
            aurorium_bonus += 30;
        }

        let aurorium_score = candidate_aurorium_score + aurorium_bonus;
        if aurorium_score > best_aurorium_score {
            best_aurorium_score = aurorium_score;
            selected_aurorium_index = aurorium_index;
        }

        aurorium_index += 1;
    }

    let mut selected_aurorium_ptr = aurorium_value.as_ptr();
    let mut selected_aurorium_written = aurorium_value_written;
    if selected_aurorium_index == 1 {
        selected_aurorium_ptr = glimmerwake_value.as_ptr();
        selected_aurorium_written = glimmerwake_value_written;
    } else if selected_aurorium_index == 2 {
        selected_aurorium_ptr = keelline_value.as_ptr();
        selected_aurorium_written = keelline_value_written;
    }

    let prismarium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_aurorium_ptr,
            selected_aurorium_written as i64,
            prismarium_old.as_ptr(),
            prismarium_old.len() as i64,
            prismarium_new.as_ptr(),
            prismarium_new.len() as i64,
        )
    };
    let mut prismarium_value = vec![0u8; prismarium_value_len as usize];
    let prismarium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_aurorium_ptr,
            selected_aurorium_written as i64,
            prismarium_old.as_ptr(),
            prismarium_old.len() as i64,
            prismarium_new.as_ptr(),
            prismarium_new.len() as i64,
            prismarium_value.as_mut_ptr(),
            prismarium_value.len() as i64,
        )
    };
    let prismarium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            prismarium_value.as_ptr(),
            prismarium_value_written as i64,
            prismarium_needle.as_ptr(),
            prismarium_needle.len() as i64,
        )
    };

    let shimmerwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_aurorium_ptr,
            selected_aurorium_written as i64,
            shimmerwake_extension.as_ptr(),
            shimmerwake_extension.len() as i64,
        )
    };
    let mut shimmerwake_value = vec![0u8; shimmerwake_value_len as usize];
    let shimmerwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_aurorium_ptr,
            selected_aurorium_written as i64,
            shimmerwake_extension.as_ptr(),
            shimmerwake_extension.len() as i64,
            shimmerwake_value.as_mut_ptr(),
            shimmerwake_value.len() as i64,
        )
    };
    let shimmerwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            shimmerwake_value.as_ptr(),
            shimmerwake_value_written as i64,
            shimmerwake_needle.as_ptr(),
            shimmerwake_needle.len() as i64,
        )
    };

    let plankline_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_aurorium_ptr,
            selected_aurorium_written as i64,
        )
    };
    let mut plankline_source = vec![0u8; plankline_source_len as usize];
    let plankline_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_aurorium_ptr,
            selected_aurorium_written as i64,
            plankline_source.as_mut_ptr(),
            plankline_source.len() as i64,
        )
    };
    let plankline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            plankline_source.as_ptr(),
            plankline_source_written as i64,
            plankline_old.as_ptr(),
            plankline_old.len() as i64,
            plankline_new.as_ptr(),
            plankline_new.len() as i64,
        )
    };
    let mut plankline_value = vec![0u8; plankline_value_len as usize];
    let plankline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            plankline_source.as_ptr(),
            plankline_source_written as i64,
            plankline_old.as_ptr(),
            plankline_old.len() as i64,
            plankline_new.as_ptr(),
            plankline_new.len() as i64,
            plankline_value.as_mut_ptr(),
            plankline_value.len() as i64,
        )
    };
    let plankline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            plankline_value.as_ptr(),
            plankline_value_written as i64,
            plankline_needle.as_ptr(),
            plankline_needle.len() as i64,
        )
    };

    let mut selected_prismarium_index = 0i32;
    let mut best_prismarium_score = i32::MIN;
    let mut prismarium_index = 0i32;
    while prismarium_index < 3 {
        let mut candidate_prismarium_score = prismarium_value_written * 10 + prismarium_contains * 50;
        if prismarium_index == 1 {
            candidate_prismarium_score = shimmerwake_value_written * 10 + shimmerwake_index;
        } else if prismarium_index == 2 {
            candidate_prismarium_score = plankline_value_written * 10 + plankline_contains * 50;
        }

        let mut prismarium_bonus = 0i32;
        if prismarium_index == selected_aurorium_index {
            prismarium_bonus += 25;
        }
        if prismarium_index == selected_solarium_index {
            prismarium_bonus += 15;
        }
        if prismarium_index == selected_nocturne_index {
            prismarium_bonus += 5;
        }
        if prismarium_index == 0 && prismarium_contains != 0 {
            prismarium_bonus += 20;
        }
        if prismarium_index == 1 && shimmerwake_index >= 0 {
            prismarium_bonus += 10;
        }
        if prismarium_index == 2 && plankline_contains != 0 {
            prismarium_bonus += 30;
        }

        let prismarium_score = candidate_prismarium_score + prismarium_bonus;
        if prismarium_score > best_prismarium_score {
            best_prismarium_score = prismarium_score;
            selected_prismarium_index = prismarium_index;
        }

        prismarium_index += 1;
    }

    let mut selected_prismarium_ptr = prismarium_value.as_ptr();
    let mut selected_prismarium_written = prismarium_value_written;
    if selected_prismarium_index == 1 {
        selected_prismarium_ptr = shimmerwake_value.as_ptr();
        selected_prismarium_written = shimmerwake_value_written;
    } else if selected_prismarium_index == 2 {
        selected_prismarium_ptr = plankline_value.as_ptr();
        selected_prismarium_written = plankline_value_written;
    }

    let spectrarium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_prismarium_ptr,
            selected_prismarium_written as i64,
            spectrarium_old.as_ptr(),
            spectrarium_old.len() as i64,
            spectrarium_new.as_ptr(),
            spectrarium_new.len() as i64,
        )
    };
    let mut spectrarium_value = vec![0u8; spectrarium_value_len as usize];
    let spectrarium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_prismarium_ptr,
            selected_prismarium_written as i64,
            spectrarium_old.as_ptr(),
            spectrarium_old.len() as i64,
            spectrarium_new.as_ptr(),
            spectrarium_new.len() as i64,
            spectrarium_value.as_mut_ptr(),
            spectrarium_value.len() as i64,
        )
    };
    let spectrarium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            spectrarium_value.as_ptr(),
            spectrarium_value_written as i64,
            spectrarium_needle.as_ptr(),
            spectrarium_needle.len() as i64,
        )
    };

    let frostwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_prismarium_ptr,
            selected_prismarium_written as i64,
            frostwake_extension.as_ptr(),
            frostwake_extension.len() as i64,
        )
    };
    let mut frostwake_value = vec![0u8; frostwake_value_len as usize];
    let frostwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_prismarium_ptr,
            selected_prismarium_written as i64,
            frostwake_extension.as_ptr(),
            frostwake_extension.len() as i64,
            frostwake_value.as_mut_ptr(),
            frostwake_value.len() as i64,
        )
    };
    let frostwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            frostwake_value.as_ptr(),
            frostwake_value_written as i64,
            frostwake_needle.as_ptr(),
            frostwake_needle.len() as i64,
        )
    };

    let ribline_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_prismarium_ptr,
            selected_prismarium_written as i64,
        )
    };
    let mut ribline_source = vec![0u8; ribline_source_len as usize];
    let ribline_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_prismarium_ptr,
            selected_prismarium_written as i64,
            ribline_source.as_mut_ptr(),
            ribline_source.len() as i64,
        )
    };
    let ribline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            ribline_source.as_ptr(),
            ribline_source_written as i64,
            ribline_old.as_ptr(),
            ribline_old.len() as i64,
            ribline_new.as_ptr(),
            ribline_new.len() as i64,
        )
    };
    let mut ribline_value = vec![0u8; ribline_value_len as usize];
    let ribline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            ribline_source.as_ptr(),
            ribline_source_written as i64,
            ribline_old.as_ptr(),
            ribline_old.len() as i64,
            ribline_new.as_ptr(),
            ribline_new.len() as i64,
            ribline_value.as_mut_ptr(),
            ribline_value.len() as i64,
        )
    };
    let ribline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            ribline_value.as_ptr(),
            ribline_value_written as i64,
            ribline_needle.as_ptr(),
            ribline_needle.len() as i64,
        )
    };

    let mut selected_spectrarium_index = 0i32;
    let mut best_spectrarium_score = i32::MIN;
    let mut spectrarium_index = 0i32;
    while spectrarium_index < 3 {
        let mut candidate_spectrarium_score = spectrarium_value_written * 10 + spectrarium_contains * 50;
        if spectrarium_index == 1 {
            candidate_spectrarium_score = frostwake_value_written * 10 + frostwake_index;
        } else if spectrarium_index == 2 {
            candidate_spectrarium_score = ribline_value_written * 10 + ribline_contains * 50;
        }

        let mut spectrarium_bonus = 0i32;
        if spectrarium_index == selected_prismarium_index {
            spectrarium_bonus += 25;
        }
        if spectrarium_index == selected_aurorium_index {
            spectrarium_bonus += 15;
        }
        if spectrarium_index == selected_solarium_index {
            spectrarium_bonus += 5;
        }
        if spectrarium_index == 0 && spectrarium_contains != 0 {
            spectrarium_bonus += 20;
        }
        if spectrarium_index == 1 && frostwake_index >= 0 {
            spectrarium_bonus += 10;
        }
        if spectrarium_index == 2 && ribline_contains != 0 {
            spectrarium_bonus += 30;
        }

        let spectrarium_score = candidate_spectrarium_score + spectrarium_bonus;
        if spectrarium_score > best_spectrarium_score {
            best_spectrarium_score = spectrarium_score;
            selected_spectrarium_index = spectrarium_index;
        }

        spectrarium_index += 1;
    }

    let mut selected_spectrarium_ptr = spectrarium_value.as_ptr();
    let mut selected_spectrarium_written = spectrarium_value_written;
    if selected_spectrarium_index == 1 {
        selected_spectrarium_ptr = frostwake_value.as_ptr();
        selected_spectrarium_written = frostwake_value_written;
    } else if selected_spectrarium_index == 2 {
        selected_spectrarium_ptr = ribline_value.as_ptr();
        selected_spectrarium_written = ribline_value_written;
    }

    let luminarium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_spectrarium_ptr,
            selected_spectrarium_written as i64,
            luminarium_old.as_ptr(),
            luminarium_old.len() as i64,
            luminarium_new.as_ptr(),
            luminarium_new.len() as i64,
        )
    };
    let mut luminarium_value = vec![0u8; luminarium_value_len as usize];
    let luminarium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_spectrarium_ptr,
            selected_spectrarium_written as i64,
            luminarium_old.as_ptr(),
            luminarium_old.len() as i64,
            luminarium_new.as_ptr(),
            luminarium_new.len() as i64,
            luminarium_value.as_mut_ptr(),
            luminarium_value.len() as i64,
        )
    };
    let luminarium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            luminarium_value.as_ptr(),
            luminarium_value_written as i64,
            luminarium_needle.as_ptr(),
            luminarium_needle.len() as i64,
        )
    };

    let icewake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_spectrarium_ptr,
            selected_spectrarium_written as i64,
            icewake_extension.as_ptr(),
            icewake_extension.len() as i64,
        )
    };
    let mut icewake_value = vec![0u8; icewake_value_len as usize];
    let icewake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_spectrarium_ptr,
            selected_spectrarium_written as i64,
            icewake_extension.as_ptr(),
            icewake_extension.len() as i64,
            icewake_value.as_mut_ptr(),
            icewake_value.len() as i64,
        )
    };
    let icewake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            icewake_value.as_ptr(),
            icewake_value_written as i64,
            icewake_needle.as_ptr(),
            icewake_needle.len() as i64,
        )
    };

    let coamline_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_spectrarium_ptr,
            selected_spectrarium_written as i64,
        )
    };
    let mut coamline_source = vec![0u8; coamline_source_len as usize];
    let coamline_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_spectrarium_ptr,
            selected_spectrarium_written as i64,
            coamline_source.as_mut_ptr(),
            coamline_source.len() as i64,
        )
    };
    let coamline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            coamline_source.as_ptr(),
            coamline_source_written as i64,
            coamline_old.as_ptr(),
            coamline_old.len() as i64,
            coamline_new.as_ptr(),
            coamline_new.len() as i64,
        )
    };
    let mut coamline_value = vec![0u8; coamline_value_len as usize];
    let coamline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            coamline_source.as_ptr(),
            coamline_source_written as i64,
            coamline_old.as_ptr(),
            coamline_old.len() as i64,
            coamline_new.as_ptr(),
            coamline_new.len() as i64,
            coamline_value.as_mut_ptr(),
            coamline_value.len() as i64,
        )
    };
    let coamline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            coamline_value.as_ptr(),
            coamline_value_written as i64,
            coamline_needle.as_ptr(),
            coamline_needle.len() as i64,
        )
    };

    let mut selected_luminarium_index = 0i32;
    let mut best_luminarium_score = i32::MIN;
    let mut luminarium_index = 0i32;
    while luminarium_index < 3 {
        let mut candidate_luminarium_score = luminarium_value_written * 10 + luminarium_contains * 50;
        if luminarium_index == 1 {
            candidate_luminarium_score = icewake_value_written * 10 + icewake_index;
        } else if luminarium_index == 2 {
            candidate_luminarium_score = coamline_value_written * 10 + coamline_contains * 50;
        }

        let mut luminarium_bonus = 0i32;
        if luminarium_index == selected_spectrarium_index {
            luminarium_bonus += 25;
        }
        if luminarium_index == selected_prismarium_index {
            luminarium_bonus += 15;
        }
        if luminarium_index == selected_aurorium_index {
            luminarium_bonus += 5;
        }
        if luminarium_index == 0 && luminarium_contains != 0 {
            luminarium_bonus += 20;
        }
        if luminarium_index == 1 && icewake_index >= 0 {
            luminarium_bonus += 10;
        }
        if luminarium_index == 2 && coamline_contains != 0 {
            luminarium_bonus += 30;
        }

        let luminarium_score = candidate_luminarium_score + luminarium_bonus;
        if luminarium_score > best_luminarium_score {
            best_luminarium_score = luminarium_score;
            selected_luminarium_index = luminarium_index;
        }

        luminarium_index += 1;
    }

    let mut selected_luminarium_ptr = luminarium_value.as_ptr();
    let mut selected_luminarium_written = luminarium_value_written;
    if selected_luminarium_index == 1 {
        selected_luminarium_ptr = icewake_value.as_ptr();
        selected_luminarium_written = icewake_value_written;
    } else if selected_luminarium_index == 2 {
        selected_luminarium_ptr = coamline_value.as_ptr();
        selected_luminarium_written = coamline_value_written;
    }

    let radiarium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_luminarium_ptr,
            selected_luminarium_written as i64,
            radiarium_old.as_ptr(),
            radiarium_old.len() as i64,
            radiarium_new.as_ptr(),
            radiarium_new.len() as i64,
        )
    };
    let mut radiarium_value = vec![0u8; radiarium_value_len as usize];
    let radiarium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_luminarium_ptr,
            selected_luminarium_written as i64,
            radiarium_old.as_ptr(),
            radiarium_old.len() as i64,
            radiarium_new.as_ptr(),
            radiarium_new.len() as i64,
            radiarium_value.as_mut_ptr(),
            radiarium_value.len() as i64,
        )
    };
    let radiarium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            radiarium_value.as_ptr(),
            radiarium_value_written as i64,
            radiarium_needle.as_ptr(),
            radiarium_needle.len() as i64,
        )
    };

    let glowwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_luminarium_ptr,
            selected_luminarium_written as i64,
            glowwake_extension.as_ptr(),
            glowwake_extension.len() as i64,
        )
    };
    let mut glowwake_value = vec![0u8; glowwake_value_len as usize];
    let glowwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_luminarium_ptr,
            selected_luminarium_written as i64,
            glowwake_extension.as_ptr(),
            glowwake_extension.len() as i64,
            glowwake_value.as_mut_ptr(),
            glowwake_value.len() as i64,
        )
    };
    let glowwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            glowwake_value.as_ptr(),
            glowwake_value_written as i64,
            glowwake_needle.as_ptr(),
            glowwake_needle.len() as i64,
        )
    };

    let sparline_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_luminarium_ptr,
            selected_luminarium_written as i64,
        )
    };
    let mut sparline_source = vec![0u8; sparline_source_len as usize];
    let sparline_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_luminarium_ptr,
            selected_luminarium_written as i64,
            sparline_source.as_mut_ptr(),
            sparline_source.len() as i64,
        )
    };
    let sparline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            sparline_source.as_ptr(),
            sparline_source_written as i64,
            sparline_old.as_ptr(),
            sparline_old.len() as i64,
            sparline_new.as_ptr(),
            sparline_new.len() as i64,
        )
    };
    let mut sparline_value = vec![0u8; sparline_value_len as usize];
    let sparline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            sparline_source.as_ptr(),
            sparline_source_written as i64,
            sparline_old.as_ptr(),
            sparline_old.len() as i64,
            sparline_new.as_ptr(),
            sparline_new.len() as i64,
            sparline_value.as_mut_ptr(),
            sparline_value.len() as i64,
        )
    };
    let sparline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            sparline_value.as_ptr(),
            sparline_value_written as i64,
            sparline_needle.as_ptr(),
            sparline_needle.len() as i64,
        )
    };

    let mut selected_radiarium_index = 0i32;
    let mut best_radiarium_score = i32::MIN;
    let mut radiarium_index = 0i32;
    while radiarium_index < 3 {
        let mut candidate_radiarium_score = radiarium_value_written * 10 + radiarium_contains * 50;
        if radiarium_index == 1 {
            candidate_radiarium_score = glowwake_value_written * 10 + glowwake_index;
        } else if radiarium_index == 2 {
            candidate_radiarium_score = sparline_value_written * 10 + sparline_contains * 50;
        }

        let mut radiarium_bonus = 0i32;
        if radiarium_index == selected_luminarium_index {
            radiarium_bonus += 25;
        }
        if radiarium_index == selected_spectrarium_index {
            radiarium_bonus += 15;
        }
        if radiarium_index == selected_prismarium_index {
            radiarium_bonus += 5;
        }
        if radiarium_index == 0 && radiarium_contains != 0 {
            radiarium_bonus += 20;
        }
        if radiarium_index == 1 && glowwake_index >= 0 {
            radiarium_bonus += 10;
        }
        if radiarium_index == 2 && sparline_contains != 0 {
            radiarium_bonus += 30;
        }

        let radiarium_score = candidate_radiarium_score + radiarium_bonus;
        if radiarium_score > best_radiarium_score {
            best_radiarium_score = radiarium_score;
            selected_radiarium_index = radiarium_index;
        }

        radiarium_index += 1;
    }

    let mut selected_radiarium_ptr = radiarium_value.as_ptr();
    let mut selected_radiarium_written = radiarium_value_written;
    if selected_radiarium_index == 1 {
        selected_radiarium_ptr = glowwake_value.as_ptr();
        selected_radiarium_written = glowwake_value_written;
    } else if selected_radiarium_index == 2 {
        selected_radiarium_ptr = sparline_value.as_ptr();
        selected_radiarium_written = sparline_value_written;
    }

    let coruscarium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_radiarium_ptr,
            selected_radiarium_written as i64,
            coruscarium_old.as_ptr(),
            coruscarium_old.len() as i64,
            coruscarium_new.as_ptr(),
            coruscarium_new.len() as i64,
        )
    };
    let mut coruscarium_value = vec![0u8; coruscarium_value_len as usize];
    let coruscarium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_radiarium_ptr,
            selected_radiarium_written as i64,
            coruscarium_old.as_ptr(),
            coruscarium_old.len() as i64,
            coruscarium_new.as_ptr(),
            coruscarium_new.len() as i64,
            coruscarium_value.as_mut_ptr(),
            coruscarium_value.len() as i64,
        )
    };
    let coruscarium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            coruscarium_value.as_ptr(),
            coruscarium_value_written as i64,
            coruscarium_needle.as_ptr(),
            coruscarium_needle.len() as i64,
        )
    };

    let flarewake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_radiarium_ptr,
            selected_radiarium_written as i64,
            flarewake_extension.as_ptr(),
            flarewake_extension.len() as i64,
        )
    };
    let mut flarewake_value = vec![0u8; flarewake_value_len as usize];
    let flarewake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_radiarium_ptr,
            selected_radiarium_written as i64,
            flarewake_extension.as_ptr(),
            flarewake_extension.len() as i64,
            flarewake_value.as_mut_ptr(),
            flarewake_value.len() as i64,
        )
    };
    let flarewake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            flarewake_value.as_ptr(),
            flarewake_value_written as i64,
            flarewake_needle.as_ptr(),
            flarewake_needle.len() as i64,
        )
    };

    let boomline_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_radiarium_ptr,
            selected_radiarium_written as i64,
        )
    };
    let mut boomline_source = vec![0u8; boomline_source_len as usize];
    let boomline_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_radiarium_ptr,
            selected_radiarium_written as i64,
            boomline_source.as_mut_ptr(),
            boomline_source.len() as i64,
        )
    };
    let boomline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            boomline_source.as_ptr(),
            boomline_source_written as i64,
            boomline_old.as_ptr(),
            boomline_old.len() as i64,
            boomline_new.as_ptr(),
            boomline_new.len() as i64,
        )
    };
    let mut boomline_value = vec![0u8; boomline_value_len as usize];
    let boomline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            boomline_source.as_ptr(),
            boomline_source_written as i64,
            boomline_old.as_ptr(),
            boomline_old.len() as i64,
            boomline_new.as_ptr(),
            boomline_new.len() as i64,
            boomline_value.as_mut_ptr(),
            boomline_value.len() as i64,
        )
    };
    let boomline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            boomline_value.as_ptr(),
            boomline_value_written as i64,
            boomline_needle.as_ptr(),
            boomline_needle.len() as i64,
        )
    };

    let mut selected_coruscarium_index = 0i32;
    let mut best_coruscarium_score = i32::MIN;
    let mut coruscarium_index = 0i32;
    while coruscarium_index < 3 {
        let mut candidate_coruscarium_score = coruscarium_value_written * 10 + coruscarium_contains * 50;
        if coruscarium_index == 1 {
            candidate_coruscarium_score = flarewake_value_written * 10 + flarewake_index;
        } else if coruscarium_index == 2 {
            candidate_coruscarium_score = boomline_value_written * 10 + boomline_contains * 50;
        }

        let mut coruscarium_bonus = 0i32;
        if coruscarium_index == selected_radiarium_index {
            coruscarium_bonus += 25;
        }
        if coruscarium_index == selected_luminarium_index {
            coruscarium_bonus += 15;
        }
        if coruscarium_index == selected_spectrarium_index {
            coruscarium_bonus += 5;
        }
        if coruscarium_index == 0 && coruscarium_contains != 0 {
            coruscarium_bonus += 20;
        }
        if coruscarium_index == 1 && flarewake_index >= 0 {
            coruscarium_bonus += 10;
        }
        if coruscarium_index == 2 && boomline_contains != 0 {
            coruscarium_bonus += 30;
        }

        let coruscarium_score = candidate_coruscarium_score + coruscarium_bonus;
        if coruscarium_score > best_coruscarium_score {
            best_coruscarium_score = coruscarium_score;
            selected_coruscarium_index = coruscarium_index;
        }

        coruscarium_index += 1;
    }

    let mut selected_coruscarium_ptr = coruscarium_value.as_ptr();
    let mut selected_coruscarium_written = coruscarium_value_written;
    if selected_coruscarium_index == 1 {
        selected_coruscarium_ptr = flarewake_value.as_ptr();
        selected_coruscarium_written = flarewake_value_written;
    } else if selected_coruscarium_index == 2 {
        selected_coruscarium_ptr = boomline_value.as_ptr();
        selected_coruscarium_written = boomline_value_written;
    }

    let caelarium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_coruscarium_ptr,
            selected_coruscarium_written as i64,
            caelarium_old.as_ptr(),
            caelarium_old.len() as i64,
            caelarium_new.as_ptr(),
            caelarium_new.len() as i64,
        )
    };
    let mut caelarium_value = vec![0u8; caelarium_value_len as usize];
    let caelarium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_coruscarium_ptr,
            selected_coruscarium_written as i64,
            caelarium_old.as_ptr(),
            caelarium_old.len() as i64,
            caelarium_new.as_ptr(),
            caelarium_new.len() as i64,
            caelarium_value.as_mut_ptr(),
            caelarium_value.len() as i64,
        )
    };
    let caelarium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            caelarium_value.as_ptr(),
            caelarium_value_written as i64,
            caelarium_needle.as_ptr(),
            caelarium_needle.len() as i64,
        )
    };

    let mistwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_coruscarium_ptr,
            selected_coruscarium_written as i64,
            mistwake_extension.as_ptr(),
            mistwake_extension.len() as i64,
        )
    };
    let mut mistwake_value = vec![0u8; mistwake_value_len as usize];
    let mistwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_coruscarium_ptr,
            selected_coruscarium_written as i64,
            mistwake_extension.as_ptr(),
            mistwake_extension.len() as i64,
            mistwake_value.as_mut_ptr(),
            mistwake_value.len() as i64,
        )
    };
    let mistwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            mistwake_value.as_ptr(),
            mistwake_value_written as i64,
            mistwake_needle.as_ptr(),
            mistwake_needle.len() as i64,
        )
    };

    let bulwark_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_coruscarium_ptr,
            selected_coruscarium_written as i64,
        )
    };
    let mut bulwark_source = vec![0u8; bulwark_source_len as usize];
    let bulwark_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_coruscarium_ptr,
            selected_coruscarium_written as i64,
            bulwark_source.as_mut_ptr(),
            bulwark_source.len() as i64,
        )
    };
    let bulwark_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            bulwark_source.as_ptr(),
            bulwark_source_written as i64,
            bulwark_old.as_ptr(),
            bulwark_old.len() as i64,
            bulwark_new.as_ptr(),
            bulwark_new.len() as i64,
        )
    };
    let mut bulwark_value = vec![0u8; bulwark_value_len as usize];
    let bulwark_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            bulwark_source.as_ptr(),
            bulwark_source_written as i64,
            bulwark_old.as_ptr(),
            bulwark_old.len() as i64,
            bulwark_new.as_ptr(),
            bulwark_new.len() as i64,
            bulwark_value.as_mut_ptr(),
            bulwark_value.len() as i64,
        )
    };
    let bulwark_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            bulwark_value.as_ptr(),
            bulwark_value_written as i64,
            bulwark_needle.as_ptr(),
            bulwark_needle.len() as i64,
        )
    };

    let mut selected_caelarium_index = 0i32;
    let mut best_caelarium_score = i32::MIN;
    let mut caelarium_index = 0i32;
    while caelarium_index < 3 {
        let mut candidate_caelarium_score = caelarium_value_written * 10 + caelarium_contains * 50;
        if caelarium_index == 1 {
            candidate_caelarium_score = mistwake_value_written * 10 + mistwake_index;
        } else if caelarium_index == 2 {
            candidate_caelarium_score = bulwark_value_written * 10 + bulwark_contains * 50;
        }

        let mut caelarium_bonus = 0i32;
        if caelarium_index == selected_coruscarium_index {
            caelarium_bonus += 25;
        }
        if caelarium_index == selected_radiarium_index {
            caelarium_bonus += 15;
        }
        if caelarium_index == selected_luminarium_index {
            caelarium_bonus += 5;
        }
        if caelarium_index == 0 && caelarium_contains != 0 {
            caelarium_bonus += 20;
        }
        if caelarium_index == 1 && mistwake_index >= 0 {
            caelarium_bonus += 10;
        }
        if caelarium_index == 2 && bulwark_contains != 0 {
            caelarium_bonus += 30;
        }

        let caelarium_score = candidate_caelarium_score + caelarium_bonus;
        if caelarium_score > best_caelarium_score {
            best_caelarium_score = caelarium_score;
            selected_caelarium_index = caelarium_index;
        }

        caelarium_index += 1;
    }

    let mut selected_caelarium_ptr = caelarium_value.as_ptr();
    let mut selected_caelarium_written = caelarium_value_written;
    if selected_caelarium_index == 1 {
        selected_caelarium_ptr = mistwake_value.as_ptr();
        selected_caelarium_written = mistwake_value_written;
    } else if selected_caelarium_index == 2 {
        selected_caelarium_ptr = bulwark_value.as_ptr();
        selected_caelarium_written = bulwark_value_written;
    }

    let aetherium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_caelarium_ptr,
            selected_caelarium_written as i64,
            aetherium_old.as_ptr(),
            aetherium_old.len() as i64,
            aetherium_new.as_ptr(),
            aetherium_new.len() as i64,
        )
    };
    let mut aetherium_value = vec![0u8; aetherium_value_len as usize];
    let aetherium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_caelarium_ptr,
            selected_caelarium_written as i64,
            aetherium_old.as_ptr(),
            aetherium_old.len() as i64,
            aetherium_new.as_ptr(),
            aetherium_new.len() as i64,
            aetherium_value.as_mut_ptr(),
            aetherium_value.len() as i64,
        )
    };
    let aetherium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            aetherium_value.as_ptr(),
            aetherium_value_written as i64,
            aetherium_needle.as_ptr(),
            aetherium_needle.len() as i64,
        )
    };

    let cloudwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_caelarium_ptr,
            selected_caelarium_written as i64,
            cloudwake_extension.as_ptr(),
            cloudwake_extension.len() as i64,
        )
    };
    let mut cloudwake_value = vec![0u8; cloudwake_value_len as usize];
    let cloudwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_caelarium_ptr,
            selected_caelarium_written as i64,
            cloudwake_extension.as_ptr(),
            cloudwake_extension.len() as i64,
            cloudwake_value.as_mut_ptr(),
            cloudwake_value.len() as i64,
        )
    };
    let cloudwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            cloudwake_value.as_ptr(),
            cloudwake_value_written as i64,
            cloudwake_needle.as_ptr(),
            cloudwake_needle.len() as i64,
        )
    };

    let gunwale_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_caelarium_ptr,
            selected_caelarium_written as i64,
        )
    };
    let mut gunwale_source = vec![0u8; gunwale_source_len as usize];
    let gunwale_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_caelarium_ptr,
            selected_caelarium_written as i64,
            gunwale_source.as_mut_ptr(),
            gunwale_source.len() as i64,
        )
    };
    let gunwale_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            gunwale_source.as_ptr(),
            gunwale_source_written as i64,
            gunwale_old.as_ptr(),
            gunwale_old.len() as i64,
            gunwale_new.as_ptr(),
            gunwale_new.len() as i64,
        )
    };
    let mut gunwale_value = vec![0u8; gunwale_value_len as usize];
    let gunwale_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            gunwale_source.as_ptr(),
            gunwale_source_written as i64,
            gunwale_old.as_ptr(),
            gunwale_old.len() as i64,
            gunwale_new.as_ptr(),
            gunwale_new.len() as i64,
            gunwale_value.as_mut_ptr(),
            gunwale_value.len() as i64,
        )
    };
    let gunwale_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            gunwale_value.as_ptr(),
            gunwale_value_written as i64,
            gunwale_needle.as_ptr(),
            gunwale_needle.len() as i64,
        )
    };

    let mut selected_aetherium_index = 0i32;
    let mut best_aetherium_score = i32::MIN;
    let mut aetherium_index = 0i32;
    while aetherium_index < 3 {
        let mut candidate_aetherium_score = aetherium_value_written * 10 + aetherium_contains * 50;
        if aetherium_index == 1 {
            candidate_aetherium_score = cloudwake_value_written * 10 + cloudwake_index;
        } else if aetherium_index == 2 {
            candidate_aetherium_score = gunwale_value_written * 10 + gunwale_contains * 50;
        }

        let mut aetherium_bonus = 0i32;
        if aetherium_index == selected_caelarium_index {
            aetherium_bonus += 25;
        }
        if aetherium_index == selected_coruscarium_index {
            aetherium_bonus += 15;
        }
        if aetherium_index == selected_radiarium_index {
            aetherium_bonus += 5;
        }
        if aetherium_index == 0 && aetherium_contains != 0 {
            aetherium_bonus += 20;
        }
        if aetherium_index == 1 && cloudwake_index >= 0 {
            aetherium_bonus += 10;
        }
        if aetherium_index == 2 && gunwale_contains != 0 {
            aetherium_bonus += 30;
        }

        let aetherium_score = candidate_aetherium_score + aetherium_bonus;
        if aetherium_score > best_aetherium_score {
            best_aetherium_score = aetherium_score;
            selected_aetherium_index = aetherium_index;
        }

        aetherium_index += 1;
    }

    let mut selected_aetherium_ptr = aetherium_value.as_ptr();
    let mut selected_aetherium_written = aetherium_value_written;
    if selected_aetherium_index == 1 {
        selected_aetherium_ptr = cloudwake_value.as_ptr();
        selected_aetherium_written = cloudwake_value_written;
    } else if selected_aetherium_index == 2 {
        selected_aetherium_ptr = gunwale_value.as_ptr();
        selected_aetherium_written = gunwale_value_written;
    }

    let nebularium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_aetherium_ptr,
            selected_aetherium_written as i64,
            nebularium_old.as_ptr(),
            nebularium_old.len() as i64,
            nebularium_new.as_ptr(),
            nebularium_new.len() as i64,
        )
    };
    let mut nebularium_value = vec![0u8; nebularium_value_len as usize];
    let nebularium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_aetherium_ptr,
            selected_aetherium_written as i64,
            nebularium_old.as_ptr(),
            nebularium_old.len() as i64,
            nebularium_new.as_ptr(),
            nebularium_new.len() as i64,
            nebularium_value.as_mut_ptr(),
            nebularium_value.len() as i64,
        )
    };
    let nebularium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            nebularium_value.as_ptr(),
            nebularium_value_written as i64,
            nebularium_needle.as_ptr(),
            nebularium_needle.len() as i64,
        )
    };

    let brightwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_aetherium_ptr,
            selected_aetherium_written as i64,
            brightwake_extension.as_ptr(),
            brightwake_extension.len() as i64,
        )
    };
    let mut brightwake_value = vec![0u8; brightwake_value_len as usize];
    let brightwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_aetherium_ptr,
            selected_aetherium_written as i64,
            brightwake_extension.as_ptr(),
            brightwake_extension.len() as i64,
            brightwake_value.as_mut_ptr(),
            brightwake_value.len() as i64,
        )
    };
    let brightwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            brightwake_value.as_ptr(),
            brightwake_value_written as i64,
            brightwake_needle.as_ptr(),
            brightwake_needle.len() as i64,
        )
    };

    let sheerline_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_aetherium_ptr,
            selected_aetherium_written as i64,
        )
    };
    let mut sheerline_source = vec![0u8; sheerline_source_len as usize];
    let sheerline_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_aetherium_ptr,
            selected_aetherium_written as i64,
            sheerline_source.as_mut_ptr(),
            sheerline_source.len() as i64,
        )
    };
    let sheerline_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            sheerline_source.as_ptr(),
            sheerline_source_written as i64,
            sheerline_old.as_ptr(),
            sheerline_old.len() as i64,
            sheerline_new.as_ptr(),
            sheerline_new.len() as i64,
        )
    };
    let mut sheerline_value = vec![0u8; sheerline_value_len as usize];
    let sheerline_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            sheerline_source.as_ptr(),
            sheerline_source_written as i64,
            sheerline_old.as_ptr(),
            sheerline_old.len() as i64,
            sheerline_new.as_ptr(),
            sheerline_new.len() as i64,
            sheerline_value.as_mut_ptr(),
            sheerline_value.len() as i64,
        )
    };
    let sheerline_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            sheerline_value.as_ptr(),
            sheerline_value_written as i64,
            sheerline_needle.as_ptr(),
            sheerline_needle.len() as i64,
        )
    };

    let mut selected_nebularium_index = 0i32;
    let mut best_nebularium_score = i32::MIN;
    let mut nebularium_index = 0i32;
    while nebularium_index < 3 {
        let mut candidate_nebularium_score = nebularium_value_written * 10 + nebularium_contains * 50;
        if nebularium_index == 1 {
            candidate_nebularium_score = brightwake_value_written * 10 + brightwake_index;
        } else if nebularium_index == 2 {
            candidate_nebularium_score = sheerline_value_written * 10 + sheerline_contains * 50;
        }

        let mut nebularium_bonus = 0i32;
        if nebularium_index == selected_aetherium_index {
            nebularium_bonus += 25;
        }
        if nebularium_index == selected_caelarium_index {
            nebularium_bonus += 15;
        }
        if nebularium_index == selected_coruscarium_index {
            nebularium_bonus += 5;
        }
        if nebularium_index == 0 && nebularium_contains != 0 {
            nebularium_bonus += 20;
        }
        if nebularium_index == 1 && brightwake_index >= 0 {
            nebularium_bonus += 10;
        }
        if nebularium_index == 2 && sheerline_contains != 0 {
            nebularium_bonus += 30;
        }

        let nebularium_score = candidate_nebularium_score + nebularium_bonus;
        if nebularium_score > best_nebularium_score {
            best_nebularium_score = nebularium_score;
            selected_nebularium_index = nebularium_index;
        }

        nebularium_index += 1;
    }

    let mut selected_nebularium_ptr = nebularium_value.as_ptr();
    let mut selected_nebularium_written = nebularium_value_written;
    if selected_nebularium_index == 1 {
        selected_nebularium_ptr = brightwake_value.as_ptr();
        selected_nebularium_written = brightwake_value_written;
    } else if selected_nebularium_index == 2 {
        selected_nebularium_ptr = sheerline_value.as_ptr();
        selected_nebularium_written = sheerline_value_written;
    }

    let quintarium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_nebularium_ptr,
            selected_nebularium_written as i64,
            quintarium_old.as_ptr(),
            quintarium_old.len() as i64,
            quintarium_new.as_ptr(),
            quintarium_new.len() as i64,
        )
    };
    let mut quintarium_value = vec![0u8; quintarium_value_len as usize];
    let quintarium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_nebularium_ptr,
            selected_nebularium_written as i64,
            quintarium_old.as_ptr(),
            quintarium_old.len() as i64,
            quintarium_new.as_ptr(),
            quintarium_new.len() as i64,
            quintarium_value.as_mut_ptr(),
            quintarium_value.len() as i64,
        )
    };
    let quintarium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            quintarium_value.as_ptr(),
            quintarium_value_written as i64,
            quintarium_needle.as_ptr(),
            quintarium_needle.len() as i64,
        )
    };

    let silverwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_nebularium_ptr,
            selected_nebularium_written as i64,
            silverwake_extension.as_ptr(),
            silverwake_extension.len() as i64,
        )
    };
    let mut silverwake_value = vec![0u8; silverwake_value_len as usize];
    let silverwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_nebularium_ptr,
            selected_nebularium_written as i64,
            silverwake_extension.as_ptr(),
            silverwake_extension.len() as i64,
            silverwake_value.as_mut_ptr(),
            silverwake_value.len() as i64,
        )
    };
    let silverwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            silverwake_value.as_ptr(),
            silverwake_value_written as i64,
            silverwake_needle.as_ptr(),
            silverwake_needle.len() as i64,
        )
    };

    let hullguard_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_nebularium_ptr,
            selected_nebularium_written as i64,
        )
    };
    let mut hullguard_source = vec![0u8; hullguard_source_len as usize];
    let hullguard_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_nebularium_ptr,
            selected_nebularium_written as i64,
            hullguard_source.as_mut_ptr(),
            hullguard_source.len() as i64,
        )
    };
    let hullguard_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            hullguard_source.as_ptr(),
            hullguard_source_written as i64,
            hullguard_old.as_ptr(),
            hullguard_old.len() as i64,
            hullguard_new.as_ptr(),
            hullguard_new.len() as i64,
        )
    };
    let mut hullguard_value = vec![0u8; hullguard_value_len as usize];
    let hullguard_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            hullguard_source.as_ptr(),
            hullguard_source_written as i64,
            hullguard_old.as_ptr(),
            hullguard_old.len() as i64,
            hullguard_new.as_ptr(),
            hullguard_new.len() as i64,
            hullguard_value.as_mut_ptr(),
            hullguard_value.len() as i64,
        )
    };
    let hullguard_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            hullguard_value.as_ptr(),
            hullguard_value_written as i64,
            hullguard_needle.as_ptr(),
            hullguard_needle.len() as i64,
        )
    };

    let mut selected_quintarium_index = 0i32;
    let mut best_quintarium_score = i32::MIN;
    let mut quintarium_index = 0i32;
    while quintarium_index < 3 {
        let mut candidate_quintarium_score = quintarium_value_written * 10 + quintarium_contains * 50;
        if quintarium_index == 1 {
            candidate_quintarium_score = silverwake_value_written * 10 + silverwake_index;
        } else if quintarium_index == 2 {
            candidate_quintarium_score = hullguard_value_written * 10 + hullguard_contains * 50;
        }

        let mut quintarium_bonus = 0i32;
        if quintarium_index == selected_nebularium_index {
            quintarium_bonus += 25;
        }
        if quintarium_index == selected_aetherium_index {
            quintarium_bonus += 15;
        }
        if quintarium_index == selected_caelarium_index {
            quintarium_bonus += 5;
        }
        if quintarium_index == 0 && quintarium_contains != 0 {
            quintarium_bonus += 20;
        }
        if quintarium_index == 1 && silverwake_index >= 0 {
            quintarium_bonus += 10;
        }
        if quintarium_index == 2 && hullguard_contains != 0 {
            quintarium_bonus += 30;
        }

        let quintarium_score = candidate_quintarium_score + quintarium_bonus;
        if quintarium_score > best_quintarium_score {
            best_quintarium_score = quintarium_score;
            selected_quintarium_index = quintarium_index;
        }

        quintarium_index += 1;
    }

    let mut selected_quintarium_ptr = quintarium_value.as_ptr();
    let mut selected_quintarium_written = quintarium_value_written;
    if selected_quintarium_index == 1 {
        selected_quintarium_ptr = silverwake_value.as_ptr();
        selected_quintarium_written = silverwake_value_written;
    } else if selected_quintarium_index == 2 {
        selected_quintarium_ptr = hullguard_value.as_ptr();
        selected_quintarium_written = hullguard_value_written;
    }

    let heliarium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_quintarium_ptr,
            selected_quintarium_written as i64,
            heliarium_old.as_ptr(),
            heliarium_old.len() as i64,
            heliarium_new.as_ptr(),
            heliarium_new.len() as i64,
        )
    };
    let mut heliarium_value = vec![0u8; heliarium_value_len as usize];
    let heliarium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_quintarium_ptr,
            selected_quintarium_written as i64,
            heliarium_old.as_ptr(),
            heliarium_old.len() as i64,
            heliarium_new.as_ptr(),
            heliarium_new.len() as i64,
            heliarium_value.as_mut_ptr(),
            heliarium_value.len() as i64,
        )
    };
    let heliarium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            heliarium_value.as_ptr(),
            heliarium_value_written as i64,
            heliarium_needle.as_ptr(),
            heliarium_needle.len() as i64,
        )
    };

    let cinderwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_quintarium_ptr,
            selected_quintarium_written as i64,
            cinderwake_extension.as_ptr(),
            cinderwake_extension.len() as i64,
        )
    };
    let mut cinderwake_value = vec![0u8; cinderwake_value_len as usize];
    let cinderwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_quintarium_ptr,
            selected_quintarium_written as i64,
            cinderwake_extension.as_ptr(),
            cinderwake_extension.len() as i64,
            cinderwake_value.as_mut_ptr(),
            cinderwake_value.len() as i64,
        )
    };
    let cinderwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            cinderwake_value.as_ptr(),
            cinderwake_value_written as i64,
            cinderwake_needle.as_ptr(),
            cinderwake_needle.len() as i64,
        )
    };

    let deckbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_quintarium_ptr,
            selected_quintarium_written as i64,
        )
    };
    let mut deckbrace_source = vec![0u8; deckbrace_source_len as usize];
    let deckbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_quintarium_ptr,
            selected_quintarium_written as i64,
            deckbrace_source.as_mut_ptr(),
            deckbrace_source.len() as i64,
        )
    };
    let deckbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            deckbrace_source.as_ptr(),
            deckbrace_source_written as i64,
            deckbrace_old.as_ptr(),
            deckbrace_old.len() as i64,
            deckbrace_new.as_ptr(),
            deckbrace_new.len() as i64,
        )
    };
    let mut deckbrace_value = vec![0u8; deckbrace_value_len as usize];
    let deckbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            deckbrace_source.as_ptr(),
            deckbrace_source_written as i64,
            deckbrace_old.as_ptr(),
            deckbrace_old.len() as i64,
            deckbrace_new.as_ptr(),
            deckbrace_new.len() as i64,
            deckbrace_value.as_mut_ptr(),
            deckbrace_value.len() as i64,
        )
    };
    let deckbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            deckbrace_value.as_ptr(),
            deckbrace_value_written as i64,
            deckbrace_needle.as_ptr(),
            deckbrace_needle.len() as i64,
        )
    };

    let mut selected_heliarium_index = 0i32;
    let mut best_heliarium_score = i32::MIN;
    let mut heliarium_index = 0i32;
    while heliarium_index < 3 {
        let mut candidate_heliarium_score = heliarium_value_written * 10 + heliarium_contains * 50;
        if heliarium_index == 1 {
            candidate_heliarium_score = cinderwake_value_written * 10 + cinderwake_index;
        } else if heliarium_index == 2 {
            candidate_heliarium_score = deckbrace_value_written * 10 + deckbrace_contains * 50;
        }

        let mut heliarium_bonus = 0i32;
        if heliarium_index == selected_quintarium_index {
            heliarium_bonus += 25;
        }
        if heliarium_index == selected_nebularium_index {
            heliarium_bonus += 15;
        }
        if heliarium_index == selected_aetherium_index {
            heliarium_bonus += 5;
        }
        if heliarium_index == 0 && heliarium_contains != 0 {
            heliarium_bonus += 20;
        }
        if heliarium_index == 1 && cinderwake_index >= 0 {
            heliarium_bonus += 10;
        }
        if heliarium_index == 2 && deckbrace_contains != 0 {
            heliarium_bonus += 30;
        }

        let heliarium_score = candidate_heliarium_score + heliarium_bonus;
        if heliarium_score > best_heliarium_score {
            best_heliarium_score = heliarium_score;
            selected_heliarium_index = heliarium_index;
        }

        heliarium_index += 1;
    }

    let mut selected_heliarium_ptr = heliarium_value.as_ptr();
    let mut selected_heliarium_written = heliarium_value_written;
    if selected_heliarium_index == 1 {
        selected_heliarium_ptr = cinderwake_value.as_ptr();
        selected_heliarium_written = cinderwake_value_written;
    } else if selected_heliarium_index == 2 {
        selected_heliarium_ptr = deckbrace_value.as_ptr();
        selected_heliarium_written = deckbrace_value_written;
    }

    let zenithium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_heliarium_ptr,
            selected_heliarium_written as i64,
            zenithium_old.as_ptr(),
            zenithium_old.len() as i64,
            zenithium_new.as_ptr(),
            zenithium_new.len() as i64,
        )
    };
    let mut zenithium_value = vec![0u8; zenithium_value_len as usize];
    let zenithium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_heliarium_ptr,
            selected_heliarium_written as i64,
            zenithium_old.as_ptr(),
            zenithium_old.len() as i64,
            zenithium_new.as_ptr(),
            zenithium_new.len() as i64,
            zenithium_value.as_mut_ptr(),
            zenithium_value.len() as i64,
        )
    };
    let zenithium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            zenithium_value.as_ptr(),
            zenithium_value_written as i64,
            zenithium_needle.as_ptr(),
            zenithium_needle.len() as i64,
        )
    };

    let opalwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_heliarium_ptr,
            selected_heliarium_written as i64,
            opalwake_extension.as_ptr(),
            opalwake_extension.len() as i64,
        )
    };
    let mut opalwake_value = vec![0u8; opalwake_value_len as usize];
    let opalwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_heliarium_ptr,
            selected_heliarium_written as i64,
            opalwake_extension.as_ptr(),
            opalwake_extension.len() as i64,
            opalwake_value.as_mut_ptr(),
            opalwake_value.len() as i64,
        )
    };
    let opalwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            opalwake_value.as_ptr(),
            opalwake_value_written as i64,
            opalwake_needle.as_ptr(),
            opalwake_needle.len() as i64,
        )
    };

    let sparbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_heliarium_ptr,
            selected_heliarium_written as i64,
        )
    };
    let mut sparbrace_source = vec![0u8; sparbrace_source_len as usize];
    let sparbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_heliarium_ptr,
            selected_heliarium_written as i64,
            sparbrace_source.as_mut_ptr(),
            sparbrace_source.len() as i64,
        )
    };
    let sparbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            sparbrace_source.as_ptr(),
            sparbrace_source_written as i64,
            sparbrace_old.as_ptr(),
            sparbrace_old.len() as i64,
            sparbrace_new.as_ptr(),
            sparbrace_new.len() as i64,
        )
    };
    let mut sparbrace_value = vec![0u8; sparbrace_value_len as usize];
    let sparbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            sparbrace_source.as_ptr(),
            sparbrace_source_written as i64,
            sparbrace_old.as_ptr(),
            sparbrace_old.len() as i64,
            sparbrace_new.as_ptr(),
            sparbrace_new.len() as i64,
            sparbrace_value.as_mut_ptr(),
            sparbrace_value.len() as i64,
        )
    };
    let sparbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            sparbrace_value.as_ptr(),
            sparbrace_value_written as i64,
            sparbrace_needle.as_ptr(),
            sparbrace_needle.len() as i64,
        )
    };

    let mut selected_zenithium_index = 0i32;
    let mut best_zenithium_score = i32::MIN;
    let mut zenithium_index = 0i32;
    while zenithium_index < 3 {
        let mut candidate_zenithium_score = zenithium_value_written * 10 + zenithium_contains * 50;
        if zenithium_index == 1 {
            candidate_zenithium_score = opalwake_value_written * 10 + opalwake_index;
        } else if zenithium_index == 2 {
            candidate_zenithium_score = sparbrace_value_written * 10 + sparbrace_contains * 50;
        }

        let mut zenithium_bonus = 0i32;
        if zenithium_index == selected_heliarium_index {
            zenithium_bonus += 25;
        }
        if zenithium_index == selected_quintarium_index {
            zenithium_bonus += 15;
        }
        if zenithium_index == selected_nebularium_index {
            zenithium_bonus += 5;
        }
        if zenithium_index == 0 && zenithium_contains != 0 {
            zenithium_bonus += 20;
        }
        if zenithium_index == 1 && opalwake_index >= 0 {
            zenithium_bonus += 10;
        }
        if zenithium_index == 2 && sparbrace_contains != 0 {
            zenithium_bonus += 30;
        }

        let zenithium_score = candidate_zenithium_score + zenithium_bonus;
        if zenithium_score > best_zenithium_score {
            best_zenithium_score = zenithium_score;
            selected_zenithium_index = zenithium_index;
        }

        zenithium_index += 1;
    }

    let mut selected_zenithium_ptr = zenithium_value.as_ptr();
    let mut selected_zenithium_written = zenithium_value_written;
    if selected_zenithium_index == 1 {
        selected_zenithium_ptr = opalwake_value.as_ptr();
        selected_zenithium_written = opalwake_value_written;
    } else if selected_zenithium_index == 2 {
        selected_zenithium_ptr = sparbrace_value.as_ptr();
        selected_zenithium_written = sparbrace_value_written;
    }

    let orinthium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_zenithium_ptr,
            selected_zenithium_written as i64,
            orinthium_old.as_ptr(),
            orinthium_old.len() as i64,
            orinthium_new.as_ptr(),
            orinthium_new.len() as i64,
        )
    };
    let mut orinthium_value = vec![0u8; orinthium_value_len as usize];
    let orinthium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_zenithium_ptr,
            selected_zenithium_written as i64,
            orinthium_old.as_ptr(),
            orinthium_old.len() as i64,
            orinthium_new.as_ptr(),
            orinthium_new.len() as i64,
            orinthium_value.as_mut_ptr(),
            orinthium_value.len() as i64,
        )
    };
    let orinthium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            orinthium_value.as_ptr(),
            orinthium_value_written as i64,
            orinthium_needle.as_ptr(),
            orinthium_needle.len() as i64,
        )
    };

    let sablewake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_zenithium_ptr,
            selected_zenithium_written as i64,
            sablewake_extension.as_ptr(),
            sablewake_extension.len() as i64,
        )
    };
    let mut sablewake_value = vec![0u8; sablewake_value_len as usize];
    let sablewake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_zenithium_ptr,
            selected_zenithium_written as i64,
            sablewake_extension.as_ptr(),
            sablewake_extension.len() as i64,
            sablewake_value.as_mut_ptr(),
            sablewake_value.len() as i64,
        )
    };
    let sablewake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            sablewake_value.as_ptr(),
            sablewake_value_written as i64,
            sablewake_needle.as_ptr(),
            sablewake_needle.len() as i64,
        )
    };

    let ribbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_zenithium_ptr,
            selected_zenithium_written as i64,
        )
    };
    let mut ribbrace_source = vec![0u8; ribbrace_source_len as usize];
    let ribbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_zenithium_ptr,
            selected_zenithium_written as i64,
            ribbrace_source.as_mut_ptr(),
            ribbrace_source.len() as i64,
        )
    };
    let ribbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            ribbrace_source.as_ptr(),
            ribbrace_source_written as i64,
            ribbrace_old.as_ptr(),
            ribbrace_old.len() as i64,
            ribbrace_new.as_ptr(),
            ribbrace_new.len() as i64,
        )
    };
    let mut ribbrace_value = vec![0u8; ribbrace_value_len as usize];
    let ribbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            ribbrace_source.as_ptr(),
            ribbrace_source_written as i64,
            ribbrace_old.as_ptr(),
            ribbrace_old.len() as i64,
            ribbrace_new.as_ptr(),
            ribbrace_new.len() as i64,
            ribbrace_value.as_mut_ptr(),
            ribbrace_value.len() as i64,
        )
    };
    let ribbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            ribbrace_value.as_ptr(),
            ribbrace_value_written as i64,
            ribbrace_needle.as_ptr(),
            ribbrace_needle.len() as i64,
        )
    };

    let mut selected_orinthium_index = 0i32;
    let mut best_orinthium_score = i32::MIN;
    let mut orinthium_index = 0i32;
    while orinthium_index < 3 {
        let mut candidate_orinthium_score = orinthium_value_written * 10 + orinthium_contains * 50;
        if orinthium_index == 1 {
            candidate_orinthium_score = sablewake_value_written * 10 + sablewake_index;
        } else if orinthium_index == 2 {
            candidate_orinthium_score = ribbrace_value_written * 10 + ribbrace_contains * 50;
        }

        let mut orinthium_bonus = 0i32;
        if orinthium_index == selected_zenithium_index {
            orinthium_bonus += 25;
        }
        if orinthium_index == selected_heliarium_index {
            orinthium_bonus += 15;
        }
        if orinthium_index == selected_quintarium_index {
            orinthium_bonus += 5;
        }
        if orinthium_index == 0 && orinthium_contains != 0 {
            orinthium_bonus += 20;
        }
        if orinthium_index == 1 && sablewake_index >= 0 {
            orinthium_bonus += 10;
        }
        if orinthium_index == 2 && ribbrace_contains != 0 {
            orinthium_bonus += 30;
        }

        let orinthium_score = candidate_orinthium_score + orinthium_bonus;
        if orinthium_score > best_orinthium_score {
            best_orinthium_score = orinthium_score;
            selected_orinthium_index = orinthium_index;
        }

        orinthium_index += 1;
    }

    let mut selected_orinthium_ptr = orinthium_value.as_ptr();
    let mut selected_orinthium_written = orinthium_value_written;
    if selected_orinthium_index == 1 {
        selected_orinthium_ptr = sablewake_value.as_ptr();
        selected_orinthium_written = sablewake_value_written;
    } else if selected_orinthium_index == 2 {
        selected_orinthium_ptr = ribbrace_value.as_ptr();
        selected_orinthium_written = ribbrace_value_written;
    }

    let ecliptium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_orinthium_ptr,
            selected_orinthium_written as i64,
            ecliptium_old.as_ptr(),
            ecliptium_old.len() as i64,
            ecliptium_new.as_ptr(),
            ecliptium_new.len() as i64,
        )
    };
    let mut ecliptium_value = vec![0u8; ecliptium_value_len as usize];
    let ecliptium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_orinthium_ptr,
            selected_orinthium_written as i64,
            ecliptium_old.as_ptr(),
            ecliptium_old.len() as i64,
            ecliptium_new.as_ptr(),
            ecliptium_new.len() as i64,
            ecliptium_value.as_mut_ptr(),
            ecliptium_value.len() as i64,
        )
    };
    let ecliptium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            ecliptium_value.as_ptr(),
            ecliptium_value_written as i64,
            ecliptium_needle.as_ptr(),
            ecliptium_needle.len() as i64,
        )
    };

    let amberwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_orinthium_ptr,
            selected_orinthium_written as i64,
            amberwake_extension.as_ptr(),
            amberwake_extension.len() as i64,
        )
    };
    let mut amberwake_value = vec![0u8; amberwake_value_len as usize];
    let amberwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_orinthium_ptr,
            selected_orinthium_written as i64,
            amberwake_extension.as_ptr(),
            amberwake_extension.len() as i64,
            amberwake_value.as_mut_ptr(),
            amberwake_value.len() as i64,
        )
    };
    let amberwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            amberwake_value.as_ptr(),
            amberwake_value_written as i64,
            amberwake_needle.as_ptr(),
            amberwake_needle.len() as i64,
        )
    };

    let strutbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_orinthium_ptr,
            selected_orinthium_written as i64,
        )
    };
    let mut strutbrace_source = vec![0u8; strutbrace_source_len as usize];
    let strutbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_orinthium_ptr,
            selected_orinthium_written as i64,
            strutbrace_source.as_mut_ptr(),
            strutbrace_source.len() as i64,
        )
    };
    let strutbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            strutbrace_source.as_ptr(),
            strutbrace_source_written as i64,
            strutbrace_old.as_ptr(),
            strutbrace_old.len() as i64,
            strutbrace_new.as_ptr(),
            strutbrace_new.len() as i64,
        )
    };
    let mut strutbrace_value = vec![0u8; strutbrace_value_len as usize];
    let strutbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            strutbrace_source.as_ptr(),
            strutbrace_source_written as i64,
            strutbrace_old.as_ptr(),
            strutbrace_old.len() as i64,
            strutbrace_new.as_ptr(),
            strutbrace_new.len() as i64,
            strutbrace_value.as_mut_ptr(),
            strutbrace_value.len() as i64,
        )
    };
    let strutbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            strutbrace_value.as_ptr(),
            strutbrace_value_written as i64,
            strutbrace_needle.as_ptr(),
            strutbrace_needle.len() as i64,
        )
    };

    let mut selected_ecliptium_index = 0i32;
    let mut best_ecliptium_score = i32::MIN;
    let mut ecliptium_index = 0i32;
    while ecliptium_index < 3 {
        let mut candidate_ecliptium_score = ecliptium_value_written * 10 + ecliptium_contains * 50;
        if ecliptium_index == 1 {
            candidate_ecliptium_score = amberwake_value_written * 10 + amberwake_index;
        } else if ecliptium_index == 2 {
            candidate_ecliptium_score = strutbrace_value_written * 10 + strutbrace_contains * 50;
        }

        let mut ecliptium_bonus = 0i32;
        if ecliptium_index == selected_orinthium_index {
            ecliptium_bonus += 25;
        }
        if ecliptium_index == selected_zenithium_index {
            ecliptium_bonus += 15;
        }
        if ecliptium_index == selected_heliarium_index {
            ecliptium_bonus += 5;
        }
        if ecliptium_index == 0 && ecliptium_contains != 0 {
            ecliptium_bonus += 20;
        }
        if ecliptium_index == 1 && amberwake_index >= 0 {
            ecliptium_bonus += 10;
        }
        if ecliptium_index == 2 && strutbrace_contains != 0 {
            ecliptium_bonus += 30;
        }

        let ecliptium_score = candidate_ecliptium_score + ecliptium_bonus;
        if ecliptium_score > best_ecliptium_score {
            best_ecliptium_score = ecliptium_score;
            selected_ecliptium_index = ecliptium_index;
        }

        ecliptium_index += 1;
    }

    let mut selected_ecliptium_ptr = ecliptium_value.as_ptr();
    let mut selected_ecliptium_written = ecliptium_value_written;
    if selected_ecliptium_index == 1 {
        selected_ecliptium_ptr = amberwake_value.as_ptr();
        selected_ecliptium_written = amberwake_value_written;
    } else if selected_ecliptium_index == 2 {
        selected_ecliptium_ptr = strutbrace_value.as_ptr();
        selected_ecliptium_written = strutbrace_value_written;
    }

    let aurelium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_ecliptium_ptr,
            selected_ecliptium_written as i64,
            aurelium_old.as_ptr(),
            aurelium_old.len() as i64,
            aurelium_new.as_ptr(),
            aurelium_new.len() as i64,
        )
    };
    let mut aurelium_value = vec![0u8; aurelium_value_len as usize];
    let aurelium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_ecliptium_ptr,
            selected_ecliptium_written as i64,
            aurelium_old.as_ptr(),
            aurelium_old.len() as i64,
            aurelium_new.as_ptr(),
            aurelium_new.len() as i64,
            aurelium_value.as_mut_ptr(),
            aurelium_value.len() as i64,
        )
    };
    let aurelium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            aurelium_value.as_ptr(),
            aurelium_value_written as i64,
            aurelium_needle.as_ptr(),
            aurelium_needle.len() as i64,
        )
    };

    let copperwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_ecliptium_ptr,
            selected_ecliptium_written as i64,
            copperwake_extension.as_ptr(),
            copperwake_extension.len() as i64,
        )
    };
    let mut copperwake_value = vec![0u8; copperwake_value_len as usize];
    let copperwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_ecliptium_ptr,
            selected_ecliptium_written as i64,
            copperwake_extension.as_ptr(),
            copperwake_extension.len() as i64,
            copperwake_value.as_mut_ptr(),
            copperwake_value.len() as i64,
        )
    };
    let copperwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            copperwake_value.as_ptr(),
            copperwake_value_written as i64,
            copperwake_needle.as_ptr(),
            copperwake_needle.len() as i64,
        )
    };

    let girderbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_ecliptium_ptr,
            selected_ecliptium_written as i64,
        )
    };
    let mut girderbrace_source = vec![0u8; girderbrace_source_len as usize];
    let girderbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_ecliptium_ptr,
            selected_ecliptium_written as i64,
            girderbrace_source.as_mut_ptr(),
            girderbrace_source.len() as i64,
        )
    };
    let girderbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            girderbrace_source.as_ptr(),
            girderbrace_source_written as i64,
            girderbrace_old.as_ptr(),
            girderbrace_old.len() as i64,
            girderbrace_new.as_ptr(),
            girderbrace_new.len() as i64,
        )
    };
    let mut girderbrace_value = vec![0u8; girderbrace_value_len as usize];
    let girderbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            girderbrace_source.as_ptr(),
            girderbrace_source_written as i64,
            girderbrace_old.as_ptr(),
            girderbrace_old.len() as i64,
            girderbrace_new.as_ptr(),
            girderbrace_new.len() as i64,
            girderbrace_value.as_mut_ptr(),
            girderbrace_value.len() as i64,
        )
    };
    let girderbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            girderbrace_value.as_ptr(),
            girderbrace_value_written as i64,
            girderbrace_needle.as_ptr(),
            girderbrace_needle.len() as i64,
        )
    };

    let mut selected_aurelium_index = 0i32;
    let mut best_aurelium_score = i32::MIN;
    let mut aurelium_index = 0i32;
    while aurelium_index < 3 {
        let mut candidate_aurelium_score = aurelium_value_written * 10 + aurelium_contains * 50;
        if aurelium_index == 1 {
            candidate_aurelium_score = copperwake_value_written * 10 + copperwake_index;
        } else if aurelium_index == 2 {
            candidate_aurelium_score = girderbrace_value_written * 10 + girderbrace_contains * 50;
        }

        let mut aurelium_bonus = 0i32;
        if aurelium_index == selected_ecliptium_index {
            aurelium_bonus += 25;
        }
        if aurelium_index == selected_orinthium_index {
            aurelium_bonus += 15;
        }
        if aurelium_index == selected_zenithium_index {
            aurelium_bonus += 5;
        }
        if aurelium_index == 0 && aurelium_contains != 0 {
            aurelium_bonus += 20;
        }
        if aurelium_index == 1 && copperwake_index >= 0 {
            aurelium_bonus += 10;
        }
        if aurelium_index == 2 && girderbrace_contains != 0 {
            aurelium_bonus += 30;
        }

        let aurelium_score = candidate_aurelium_score + aurelium_bonus;
        if aurelium_score > best_aurelium_score {
            best_aurelium_score = aurelium_score;
            selected_aurelium_index = aurelium_index;
        }

        aurelium_index += 1;
    }

    let mut selected_aurelium_ptr = aurelium_value.as_ptr();
    let mut selected_aurelium_written = aurelium_value_written;
    if selected_aurelium_index == 1 {
        selected_aurelium_ptr = copperwake_value.as_ptr();
        selected_aurelium_written = copperwake_value_written;
    } else if selected_aurelium_index == 2 {
        selected_aurelium_ptr = girderbrace_value.as_ptr();
        selected_aurelium_written = girderbrace_value_written;
    }

    let celestium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_aurelium_ptr,
            selected_aurelium_written as i64,
            celestium_old.as_ptr(),
            celestium_old.len() as i64,
            celestium_new.as_ptr(),
            celestium_new.len() as i64,
        )
    };
    let mut celestium_value = vec![0u8; celestium_value_len as usize];
    let celestium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_aurelium_ptr,
            selected_aurelium_written as i64,
            celestium_old.as_ptr(),
            celestium_old.len() as i64,
            celestium_new.as_ptr(),
            celestium_new.len() as i64,
            celestium_value.as_mut_ptr(),
            celestium_value.len() as i64,
        )
    };
    let celestium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            celestium_value.as_ptr(),
            celestium_value_written as i64,
            celestium_needle.as_ptr(),
            celestium_needle.len() as i64,
        )
    };

    let brasswake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_aurelium_ptr,
            selected_aurelium_written as i64,
            brasswake_extension.as_ptr(),
            brasswake_extension.len() as i64,
        )
    };
    let mut brasswake_value = vec![0u8; brasswake_value_len as usize];
    let brasswake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_aurelium_ptr,
            selected_aurelium_written as i64,
            brasswake_extension.as_ptr(),
            brasswake_extension.len() as i64,
            brasswake_value.as_mut_ptr(),
            brasswake_value.len() as i64,
        )
    };
    let brasswake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            brasswake_value.as_ptr(),
            brasswake_value_written as i64,
            brasswake_needle.as_ptr(),
            brasswake_needle.len() as i64,
        )
    };

    let latticebrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_aurelium_ptr,
            selected_aurelium_written as i64,
        )
    };
    let mut latticebrace_source = vec![0u8; latticebrace_source_len as usize];
    let latticebrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_aurelium_ptr,
            selected_aurelium_written as i64,
            latticebrace_source.as_mut_ptr(),
            latticebrace_source.len() as i64,
        )
    };
    let latticebrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            latticebrace_source.as_ptr(),
            latticebrace_source_written as i64,
            latticebrace_old.as_ptr(),
            latticebrace_old.len() as i64,
            latticebrace_new.as_ptr(),
            latticebrace_new.len() as i64,
        )
    };
    let mut latticebrace_value = vec![0u8; latticebrace_value_len as usize];
    let latticebrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            latticebrace_source.as_ptr(),
            latticebrace_source_written as i64,
            latticebrace_old.as_ptr(),
            latticebrace_old.len() as i64,
            latticebrace_new.as_ptr(),
            latticebrace_new.len() as i64,
            latticebrace_value.as_mut_ptr(),
            latticebrace_value.len() as i64,
        )
    };
    let latticebrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            latticebrace_value.as_ptr(),
            latticebrace_value_written as i64,
            latticebrace_needle.as_ptr(),
            latticebrace_needle.len() as i64,
        )
    };

    let mut selected_celestium_index = 0i32;
    let mut best_celestium_score = i32::MIN;
    let mut celestium_index = 0i32;
    while celestium_index < 3 {
        let mut candidate_celestium_score = celestium_value_written * 10 + celestium_contains * 50;
        if celestium_index == 1 {
            candidate_celestium_score = brasswake_value_written * 10 + brasswake_index;
        } else if celestium_index == 2 {
            candidate_celestium_score = latticebrace_value_written * 10 + latticebrace_contains * 50;
        }

        let mut celestium_bonus = 0i32;
        if celestium_index == selected_aurelium_index {
            celestium_bonus += 25;
        }
        if celestium_index == selected_ecliptium_index {
            celestium_bonus += 15;
        }
        if celestium_index == selected_orinthium_index {
            celestium_bonus += 5;
        }
        if celestium_index == 0 && celestium_contains != 0 {
            celestium_bonus += 20;
        }
        if celestium_index == 1 && brasswake_index >= 0 {
            celestium_bonus += 10;
        }
        if celestium_index == 2 && latticebrace_contains != 0 {
            celestium_bonus += 30;
        }

        let celestium_score = candidate_celestium_score + celestium_bonus;
        if celestium_score > best_celestium_score {
            best_celestium_score = celestium_score;
            selected_celestium_index = celestium_index;
        }

        celestium_index += 1;
    }

    let mut selected_celestium_ptr = celestium_value.as_ptr();
    let mut selected_celestium_written = celestium_value_written;
    if selected_celestium_index == 1 {
        selected_celestium_ptr = brasswake_value.as_ptr();
        selected_celestium_written = brasswake_value_written;
    } else if selected_celestium_index == 2 {
        selected_celestium_ptr = latticebrace_value.as_ptr();
        selected_celestium_written = latticebrace_value_written;
    }

    let obsidium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_celestium_ptr,
            selected_celestium_written as i64,
            obsidium_old.as_ptr(),
            obsidium_old.len() as i64,
            obsidium_new.as_ptr(),
            obsidium_new.len() as i64,
        )
    };
    let mut obsidium_value = vec![0u8; obsidium_value_len as usize];
    let obsidium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_celestium_ptr,
            selected_celestium_written as i64,
            obsidium_old.as_ptr(),
            obsidium_old.len() as i64,
            obsidium_new.as_ptr(),
            obsidium_new.len() as i64,
            obsidium_value.as_mut_ptr(),
            obsidium_value.len() as i64,
        )
    };
    let obsidium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            obsidium_value.as_ptr(),
            obsidium_value_written as i64,
            obsidium_needle.as_ptr(),
            obsidium_needle.len() as i64,
        )
    };

    let tinwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_celestium_ptr,
            selected_celestium_written as i64,
            tinwake_extension.as_ptr(),
            tinwake_extension.len() as i64,
        )
    };
    let mut tinwake_value = vec![0u8; tinwake_value_len as usize];
    let tinwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_celestium_ptr,
            selected_celestium_written as i64,
            tinwake_extension.as_ptr(),
            tinwake_extension.len() as i64,
            tinwake_value.as_mut_ptr(),
            tinwake_value.len() as i64,
        )
    };
    let tinwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            tinwake_value.as_ptr(),
            tinwake_value_written as i64,
            tinwake_needle.as_ptr(),
            tinwake_needle.len() as i64,
        )
    };

    let meshbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_celestium_ptr,
            selected_celestium_written as i64,
        )
    };
    let mut meshbrace_source = vec![0u8; meshbrace_source_len as usize];
    let meshbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_celestium_ptr,
            selected_celestium_written as i64,
            meshbrace_source.as_mut_ptr(),
            meshbrace_source.len() as i64,
        )
    };
    let meshbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            meshbrace_source.as_ptr(),
            meshbrace_source_written as i64,
            meshbrace_old.as_ptr(),
            meshbrace_old.len() as i64,
            meshbrace_new.as_ptr(),
            meshbrace_new.len() as i64,
        )
    };
    let mut meshbrace_value = vec![0u8; meshbrace_value_len as usize];
    let meshbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            meshbrace_source.as_ptr(),
            meshbrace_source_written as i64,
            meshbrace_old.as_ptr(),
            meshbrace_old.len() as i64,
            meshbrace_new.as_ptr(),
            meshbrace_new.len() as i64,
            meshbrace_value.as_mut_ptr(),
            meshbrace_value.len() as i64,
        )
    };
    let meshbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            meshbrace_value.as_ptr(),
            meshbrace_value_written as i64,
            meshbrace_needle.as_ptr(),
            meshbrace_needle.len() as i64,
        )
    };

    let mut selected_obsidium_index = 0i32;
    let mut best_obsidium_score = i32::MIN;
    let mut obsidium_index = 0i32;
    while obsidium_index < 3 {
        let mut candidate_obsidium_score = obsidium_value_written * 10 + obsidium_contains * 50;
        if obsidium_index == 1 {
            candidate_obsidium_score = tinwake_value_written * 10 + tinwake_index;
        } else if obsidium_index == 2 {
            candidate_obsidium_score = meshbrace_value_written * 10 + meshbrace_contains * 50;
        }

        let mut obsidium_bonus = 0i32;
        if obsidium_index == selected_celestium_index {
            obsidium_bonus += 25;
        }
        if obsidium_index == selected_aurelium_index {
            obsidium_bonus += 15;
        }
        if obsidium_index == selected_ecliptium_index {
            obsidium_bonus += 5;
        }
        if obsidium_index == 0 && obsidium_contains != 0 {
            obsidium_bonus += 20;
        }
        if obsidium_index == 1 && tinwake_index >= 0 {
            obsidium_bonus += 10;
        }
        if obsidium_index == 2 && meshbrace_contains != 0 {
            obsidium_bonus += 30;
        }

        let obsidium_score = candidate_obsidium_score + obsidium_bonus;
        if obsidium_score > best_obsidium_score {
            best_obsidium_score = obsidium_score;
            selected_obsidium_index = obsidium_index;
        }

        obsidium_index += 1;
    }

    let mut selected_obsidium_ptr = obsidium_value.as_ptr();
    let mut selected_obsidium_written = obsidium_value_written;
    if selected_obsidium_index == 1 {
        selected_obsidium_ptr = tinwake_value.as_ptr();
        selected_obsidium_written = tinwake_value_written;
    } else if selected_obsidium_index == 2 {
        selected_obsidium_ptr = meshbrace_value.as_ptr();
        selected_obsidium_written = meshbrace_value_written;
    }

    let meridium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_obsidium_ptr,
            selected_obsidium_written as i64,
            meridium_old.as_ptr(),
            meridium_old.len() as i64,
            meridium_new.as_ptr(),
            meridium_new.len() as i64,
        )
    };
    let mut meridium_value = vec![0u8; meridium_value_len as usize];
    let meridium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_obsidium_ptr,
            selected_obsidium_written as i64,
            meridium_old.as_ptr(),
            meridium_old.len() as i64,
            meridium_new.as_ptr(),
            meridium_new.len() as i64,
            meridium_value.as_mut_ptr(),
            meridium_value.len() as i64,
        )
    };
    let meridium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            meridium_value.as_ptr(),
            meridium_value_written as i64,
            meridium_needle.as_ptr(),
            meridium_needle.len() as i64,
        )
    };

    let zincwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_obsidium_ptr,
            selected_obsidium_written as i64,
            zincwake_extension.as_ptr(),
            zincwake_extension.len() as i64,
        )
    };
    let mut zincwake_value = vec![0u8; zincwake_value_len as usize];
    let zincwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_obsidium_ptr,
            selected_obsidium_written as i64,
            zincwake_extension.as_ptr(),
            zincwake_extension.len() as i64,
            zincwake_value.as_mut_ptr(),
            zincwake_value.len() as i64,
        )
    };
    let zincwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            zincwake_value.as_ptr(),
            zincwake_value_written as i64,
            zincwake_needle.as_ptr(),
            zincwake_needle.len() as i64,
        )
    };

    let panelbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_obsidium_ptr,
            selected_obsidium_written as i64,
        )
    };
    let mut panelbrace_source = vec![0u8; panelbrace_source_len as usize];
    let panelbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_obsidium_ptr,
            selected_obsidium_written as i64,
            panelbrace_source.as_mut_ptr(),
            panelbrace_source.len() as i64,
        )
    };
    let panelbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            panelbrace_source.as_ptr(),
            panelbrace_source_written as i64,
            panelbrace_old.as_ptr(),
            panelbrace_old.len() as i64,
            panelbrace_new.as_ptr(),
            panelbrace_new.len() as i64,
        )
    };
    let mut panelbrace_value = vec![0u8; panelbrace_value_len as usize];
    let panelbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            panelbrace_source.as_ptr(),
            panelbrace_source_written as i64,
            panelbrace_old.as_ptr(),
            panelbrace_old.len() as i64,
            panelbrace_new.as_ptr(),
            panelbrace_new.len() as i64,
            panelbrace_value.as_mut_ptr(),
            panelbrace_value.len() as i64,
        )
    };
    let panelbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            panelbrace_value.as_ptr(),
            panelbrace_value_written as i64,
            panelbrace_needle.as_ptr(),
            panelbrace_needle.len() as i64,
        )
    };

    let mut selected_meridium_index = 0i32;
    let mut best_meridium_score = i32::MIN;
    let mut meridium_index = 0i32;
    while meridium_index < 3 {
        let mut candidate_meridium_score = meridium_value_written * 10 + meridium_contains * 50;
        if meridium_index == 1 {
            candidate_meridium_score = zincwake_value_written * 10 + zincwake_index;
        } else if meridium_index == 2 {
            candidate_meridium_score = panelbrace_value_written * 10 + panelbrace_contains * 50;
        }

        let mut meridium_bonus = 0i32;
        if meridium_index == selected_obsidium_index {
            meridium_bonus += 25;
        }
        if meridium_index == selected_celestium_index {
            meridium_bonus += 15;
        }
        if meridium_index == selected_aurelium_index {
            meridium_bonus += 5;
        }
        if meridium_index == 0 && meridium_contains != 0 {
            meridium_bonus += 20;
        }
        if meridium_index == 1 && zincwake_index >= 0 {
            meridium_bonus += 10;
        }
        if meridium_index == 2 && panelbrace_contains != 0 {
            meridium_bonus += 30;
        }

        let meridium_score = candidate_meridium_score + meridium_bonus;
        if meridium_score > best_meridium_score {
            best_meridium_score = meridium_score;
            selected_meridium_index = meridium_index;
        }

        meridium_index += 1;
    }

    let mut selected_meridium_ptr = meridium_value.as_ptr();
    let mut selected_meridium_written = meridium_value_written;
    if selected_meridium_index == 1 {
        selected_meridium_ptr = zincwake_value.as_ptr();
        selected_meridium_written = zincwake_value_written;
    } else if selected_meridium_index == 2 {
        selected_meridium_ptr = panelbrace_value.as_ptr();
        selected_meridium_written = panelbrace_value_written;
    }

    let verdantium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_meridium_ptr,
            selected_meridium_written as i64,
            verdantium_old.as_ptr(),
            verdantium_old.len() as i64,
            verdantium_new.as_ptr(),
            verdantium_new.len() as i64,
        )
    };
    let mut verdantium_value = vec![0u8; verdantium_value_len as usize];
    let verdantium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_meridium_ptr,
            selected_meridium_written as i64,
            verdantium_old.as_ptr(),
            verdantium_old.len() as i64,
            verdantium_new.as_ptr(),
            verdantium_new.len() as i64,
            verdantium_value.as_mut_ptr(),
            verdantium_value.len() as i64,
        )
    };
    let verdantium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            verdantium_value.as_ptr(),
            verdantium_value_written as i64,
            verdantium_needle.as_ptr(),
            verdantium_needle.len() as i64,
        )
    };

    let nickelwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_meridium_ptr,
            selected_meridium_written as i64,
            nickelwake_extension.as_ptr(),
            nickelwake_extension.len() as i64,
        )
    };
    let mut nickelwake_value = vec![0u8; nickelwake_value_len as usize];
    let nickelwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_meridium_ptr,
            selected_meridium_written as i64,
            nickelwake_extension.as_ptr(),
            nickelwake_extension.len() as i64,
            nickelwake_value.as_mut_ptr(),
            nickelwake_value.len() as i64,
        )
    };
    let nickelwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            nickelwake_value.as_ptr(),
            nickelwake_value_written as i64,
            nickelwake_needle.as_ptr(),
            nickelwake_needle.len() as i64,
        )
    };

    let joistbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_meridium_ptr,
            selected_meridium_written as i64,
        )
    };
    let mut joistbrace_source = vec![0u8; joistbrace_source_len as usize];
    let joistbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_meridium_ptr,
            selected_meridium_written as i64,
            joistbrace_source.as_mut_ptr(),
            joistbrace_source.len() as i64,
        )
    };
    let joistbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            joistbrace_source.as_ptr(),
            joistbrace_source_written as i64,
            joistbrace_old.as_ptr(),
            joistbrace_old.len() as i64,
            joistbrace_new.as_ptr(),
            joistbrace_new.len() as i64,
        )
    };
    let mut joistbrace_value = vec![0u8; joistbrace_value_len as usize];
    let joistbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            joistbrace_source.as_ptr(),
            joistbrace_source_written as i64,
            joistbrace_old.as_ptr(),
            joistbrace_old.len() as i64,
            joistbrace_new.as_ptr(),
            joistbrace_new.len() as i64,
            joistbrace_value.as_mut_ptr(),
            joistbrace_value.len() as i64,
        )
    };
    let joistbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            joistbrace_value.as_ptr(),
            joistbrace_value_written as i64,
            joistbrace_needle.as_ptr(),
            joistbrace_needle.len() as i64,
        )
    };

    let mut selected_verdantium_index = 0i32;
    let mut best_verdantium_score = i32::MIN;
    let mut verdantium_index = 0i32;
    while verdantium_index < 3 {
        let mut candidate_verdantium_score = verdantium_value_written * 10 + verdantium_contains * 50;
        if verdantium_index == 1 {
            candidate_verdantium_score = nickelwake_value_written * 10 + nickelwake_index;
        } else if verdantium_index == 2 {
            candidate_verdantium_score = joistbrace_value_written * 10 + joistbrace_contains * 50;
        }

        let mut verdantium_bonus = 0i32;
        if verdantium_index == selected_meridium_index {
            verdantium_bonus += 25;
        }
        if verdantium_index == selected_obsidium_index {
            verdantium_bonus += 15;
        }
        if verdantium_index == selected_celestium_index {
            verdantium_bonus += 5;
        }
        if verdantium_index == 0 && verdantium_contains != 0 {
            verdantium_bonus += 20;
        }
        if verdantium_index == 1 && nickelwake_index >= 0 {
            verdantium_bonus += 10;
        }
        if verdantium_index == 2 && joistbrace_contains != 0 {
            verdantium_bonus += 30;
        }

        let verdantium_score = candidate_verdantium_score + verdantium_bonus;
        if verdantium_score > best_verdantium_score {
            best_verdantium_score = verdantium_score;
            selected_verdantium_index = verdantium_index;
        }

        verdantium_index += 1;
    }

    let mut selected_verdantium_ptr = verdantium_value.as_ptr();
    let mut selected_verdantium_written = verdantium_value_written;
    if selected_verdantium_index == 1 {
        selected_verdantium_ptr = nickelwake_value.as_ptr();
        selected_verdantium_written = nickelwake_value_written;
    } else if selected_verdantium_index == 2 {
        selected_verdantium_ptr = joistbrace_value.as_ptr();
        selected_verdantium_written = joistbrace_value_written;
    }

    let lumitium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_verdantium_ptr,
            selected_verdantium_written as i64,
            lumitium_old.as_ptr(),
            lumitium_old.len() as i64,
            lumitium_new.as_ptr(),
            lumitium_new.len() as i64,
        )
    };
    let mut lumitium_value = vec![0u8; lumitium_value_len as usize];
    let lumitium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_verdantium_ptr,
            selected_verdantium_written as i64,
            lumitium_old.as_ptr(),
            lumitium_old.len() as i64,
            lumitium_new.as_ptr(),
            lumitium_new.len() as i64,
            lumitium_value.as_mut_ptr(),
            lumitium_value.len() as i64,
        )
    };
    let lumitium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            lumitium_value.as_ptr(),
            lumitium_value_written as i64,
            lumitium_needle.as_ptr(),
            lumitium_needle.len() as i64,
        )
    };

    let cobaltwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_verdantium_ptr,
            selected_verdantium_written as i64,
            cobaltwake_extension.as_ptr(),
            cobaltwake_extension.len() as i64,
        )
    };
    let mut cobaltwake_value = vec![0u8; cobaltwake_value_len as usize];
    let cobaltwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_verdantium_ptr,
            selected_verdantium_written as i64,
            cobaltwake_extension.as_ptr(),
            cobaltwake_extension.len() as i64,
            cobaltwake_value.as_mut_ptr(),
            cobaltwake_value.len() as i64,
        )
    };
    let cobaltwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            cobaltwake_value.as_ptr(),
            cobaltwake_value_written as i64,
            cobaltwake_needle.as_ptr(),
            cobaltwake_needle.len() as i64,
        )
    };

    let rafterbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_verdantium_ptr,
            selected_verdantium_written as i64,
        )
    };
    let mut rafterbrace_source = vec![0u8; rafterbrace_source_len as usize];
    let rafterbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_verdantium_ptr,
            selected_verdantium_written as i64,
            rafterbrace_source.as_mut_ptr(),
            rafterbrace_source.len() as i64,
        )
    };
    let rafterbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            rafterbrace_source.as_ptr(),
            rafterbrace_source_written as i64,
            rafterbrace_old.as_ptr(),
            rafterbrace_old.len() as i64,
            rafterbrace_new.as_ptr(),
            rafterbrace_new.len() as i64,
        )
    };
    let mut rafterbrace_value = vec![0u8; rafterbrace_value_len as usize];
    let rafterbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            rafterbrace_source.as_ptr(),
            rafterbrace_source_written as i64,
            rafterbrace_old.as_ptr(),
            rafterbrace_old.len() as i64,
            rafterbrace_new.as_ptr(),
            rafterbrace_new.len() as i64,
            rafterbrace_value.as_mut_ptr(),
            rafterbrace_value.len() as i64,
        )
    };
    let rafterbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            rafterbrace_value.as_ptr(),
            rafterbrace_value_written as i64,
            rafterbrace_needle.as_ptr(),
            rafterbrace_needle.len() as i64,
        )
    };

    let mut selected_lumitium_index = 0i32;
    let mut best_lumitium_score = i32::MIN;
    let mut lumitium_index = 0i32;
    while lumitium_index < 3 {
        let mut candidate_lumitium_score = lumitium_value_written * 10 + lumitium_contains * 50;
        if lumitium_index == 1 {
            candidate_lumitium_score = cobaltwake_value_written * 10 + cobaltwake_index;
        } else if lumitium_index == 2 {
            candidate_lumitium_score = rafterbrace_value_written * 10 + rafterbrace_contains * 50;
        }

        let mut lumitium_bonus = 0i32;
        if lumitium_index == selected_verdantium_index {
            lumitium_bonus += 25;
        }
        if lumitium_index == selected_meridium_index {
            lumitium_bonus += 15;
        }
        if lumitium_index == selected_obsidium_index {
            lumitium_bonus += 5;
        }
        if lumitium_index == 0 && lumitium_contains != 0 {
            lumitium_bonus += 20;
        }
        if lumitium_index == 1 && cobaltwake_index >= 0 {
            lumitium_bonus += 10;
        }
        if lumitium_index == 2 && rafterbrace_contains != 0 {
            lumitium_bonus += 30;
        }

        let lumitium_score = candidate_lumitium_score + lumitium_bonus;
        if lumitium_score > best_lumitium_score {
            best_lumitium_score = lumitium_score;
            selected_lumitium_index = lumitium_index;
        }

        lumitium_index += 1;
    }

    let mut selected_lumitium_ptr = lumitium_value.as_ptr();
    let mut selected_lumitium_written = lumitium_value_written;
    if selected_lumitium_index == 1 {
        selected_lumitium_ptr = cobaltwake_value.as_ptr();
        selected_lumitium_written = cobaltwake_value_written;
    } else if selected_lumitium_index == 2 {
        selected_lumitium_ptr = rafterbrace_value.as_ptr();
        selected_lumitium_written = rafterbrace_value_written;
    }

    let prismatium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_lumitium_ptr,
            selected_lumitium_written as i64,
            prismatium_old.as_ptr(),
            prismatium_old.len() as i64,
            prismatium_new.as_ptr(),
            prismatium_new.len() as i64,
        )
    };
    let mut prismatium_value = vec![0u8; prismatium_value_len as usize];
    let prismatium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_lumitium_ptr,
            selected_lumitium_written as i64,
            prismatium_old.as_ptr(),
            prismatium_old.len() as i64,
            prismatium_new.as_ptr(),
            prismatium_new.len() as i64,
            prismatium_value.as_mut_ptr(),
            prismatium_value.len() as i64,
        )
    };
    let prismatium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            prismatium_value.as_ptr(),
            prismatium_value_written as i64,
            prismatium_needle.as_ptr(),
            prismatium_needle.len() as i64,
        )
    };

    let chromewake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_lumitium_ptr,
            selected_lumitium_written as i64,
            chromewake_extension.as_ptr(),
            chromewake_extension.len() as i64,
        )
    };
    let mut chromewake_value = vec![0u8; chromewake_value_len as usize];
    let chromewake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_lumitium_ptr,
            selected_lumitium_written as i64,
            chromewake_extension.as_ptr(),
            chromewake_extension.len() as i64,
            chromewake_value.as_mut_ptr(),
            chromewake_value.len() as i64,
        )
    };
    let chromewake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            chromewake_value.as_ptr(),
            chromewake_value_written as i64,
            chromewake_needle.as_ptr(),
            chromewake_needle.len() as i64,
        )
    };

    let beambrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_lumitium_ptr,
            selected_lumitium_written as i64,
        )
    };
    let mut beambrace_source = vec![0u8; beambrace_source_len as usize];
    let beambrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_lumitium_ptr,
            selected_lumitium_written as i64,
            beambrace_source.as_mut_ptr(),
            beambrace_source.len() as i64,
        )
    };
    let beambrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            beambrace_source.as_ptr(),
            beambrace_source_written as i64,
            beambrace_old.as_ptr(),
            beambrace_old.len() as i64,
            beambrace_new.as_ptr(),
            beambrace_new.len() as i64,
        )
    };
    let mut beambrace_value = vec![0u8; beambrace_value_len as usize];
    let beambrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            beambrace_source.as_ptr(),
            beambrace_source_written as i64,
            beambrace_old.as_ptr(),
            beambrace_old.len() as i64,
            beambrace_new.as_ptr(),
            beambrace_new.len() as i64,
            beambrace_value.as_mut_ptr(),
            beambrace_value.len() as i64,
        )
    };
    let beambrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            beambrace_value.as_ptr(),
            beambrace_value_written as i64,
            beambrace_needle.as_ptr(),
            beambrace_needle.len() as i64,
        )
    };

    let mut selected_prismatium_index = 0i32;
    let mut best_prismatium_score = i32::MIN;
    let mut prismatium_index = 0i32;
    while prismatium_index < 3 {
        let mut candidate_prismatium_score = prismatium_value_written * 10 + prismatium_contains * 50;
        if prismatium_index == 1 {
            candidate_prismatium_score = chromewake_value_written * 10 + chromewake_index;
        } else if prismatium_index == 2 {
            candidate_prismatium_score = beambrace_value_written * 10 + beambrace_contains * 50;
        }

        let mut prismatium_bonus = 0i32;
        if prismatium_index == selected_lumitium_index {
            prismatium_bonus += 25;
        }
        if prismatium_index == selected_verdantium_index {
            prismatium_bonus += 15;
        }
        if prismatium_index == selected_meridium_index {
            prismatium_bonus += 5;
        }
        if prismatium_index == 0 && prismatium_contains != 0 {
            prismatium_bonus += 20;
        }
        if prismatium_index == 1 && chromewake_index >= 0 {
            prismatium_bonus += 10;
        }
        if prismatium_index == 2 && beambrace_contains != 0 {
            prismatium_bonus += 30;
        }

        let prismatium_score = candidate_prismatium_score + prismatium_bonus;
        if prismatium_score > best_prismatium_score {
            best_prismatium_score = prismatium_score;
            selected_prismatium_index = prismatium_index;
        }

        prismatium_index += 1;
    }

    let mut selected_prismatium_ptr = prismatium_value.as_ptr();
    let mut selected_prismatium_written = prismatium_value_written;
    if selected_prismatium_index == 1 {
        selected_prismatium_ptr = chromewake_value.as_ptr();
        selected_prismatium_written = chromewake_value_written;
    } else if selected_prismatium_index == 2 {
        selected_prismatium_ptr = beambrace_value.as_ptr();
        selected_prismatium_written = beambrace_value_written;
    }

    let stellarium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_prismatium_ptr,
            selected_prismatium_written as i64,
            stellarium_old.as_ptr(),
            stellarium_old.len() as i64,
            stellarium_new.as_ptr(),
            stellarium_new.len() as i64,
        )
    };
    let mut stellarium_value = vec![0u8; stellarium_value_len as usize];
    let stellarium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_prismatium_ptr,
            selected_prismatium_written as i64,
            stellarium_old.as_ptr(),
            stellarium_old.len() as i64,
            stellarium_new.as_ptr(),
            stellarium_new.len() as i64,
            stellarium_value.as_mut_ptr(),
            stellarium_value.len() as i64,
        )
    };
    let stellarium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            stellarium_value.as_ptr(),
            stellarium_value_written as i64,
            stellarium_needle.as_ptr(),
            stellarium_needle.len() as i64,
        )
    };

    let titanwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_prismatium_ptr,
            selected_prismatium_written as i64,
            titanwake_extension.as_ptr(),
            titanwake_extension.len() as i64,
        )
    };
    let mut titanwake_value = vec![0u8; titanwake_value_len as usize];
    let titanwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_prismatium_ptr,
            selected_prismatium_written as i64,
            titanwake_extension.as_ptr(),
            titanwake_extension.len() as i64,
            titanwake_value.as_mut_ptr(),
            titanwake_value.len() as i64,
        )
    };
    let titanwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            titanwake_value.as_ptr(),
            titanwake_value_written as i64,
            titanwake_needle.as_ptr(),
            titanwake_needle.len() as i64,
        )
    };

    let archbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_prismatium_ptr,
            selected_prismatium_written as i64,
        )
    };
    let mut archbrace_source = vec![0u8; archbrace_source_len as usize];
    let archbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_prismatium_ptr,
            selected_prismatium_written as i64,
            archbrace_source.as_mut_ptr(),
            archbrace_source.len() as i64,
        )
    };
    let archbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            archbrace_source.as_ptr(),
            archbrace_source_written as i64,
            archbrace_old.as_ptr(),
            archbrace_old.len() as i64,
            archbrace_new.as_ptr(),
            archbrace_new.len() as i64,
        )
    };
    let mut archbrace_value = vec![0u8; archbrace_value_len as usize];
    let archbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            archbrace_source.as_ptr(),
            archbrace_source_written as i64,
            archbrace_old.as_ptr(),
            archbrace_old.len() as i64,
            archbrace_new.as_ptr(),
            archbrace_new.len() as i64,
            archbrace_value.as_mut_ptr(),
            archbrace_value.len() as i64,
        )
    };
    let archbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            archbrace_value.as_ptr(),
            archbrace_value_written as i64,
            archbrace_needle.as_ptr(),
            archbrace_needle.len() as i64,
        )
    };

    let mut selected_stellarium_index = 0i32;
    let mut best_stellarium_score = i32::MIN;
    let mut stellarium_index = 0i32;
    while stellarium_index < 3 {
        let mut candidate_stellarium_score = stellarium_value_written * 10 + stellarium_contains * 50;
        if stellarium_index == 1 {
            candidate_stellarium_score = titanwake_value_written * 10 + titanwake_index;
        } else if stellarium_index == 2 {
            candidate_stellarium_score = archbrace_value_written * 10 + archbrace_contains * 50;
        }

        let mut stellarium_bonus = 0i32;
        if stellarium_index == selected_prismatium_index {
            stellarium_bonus += 25;
        }
        if stellarium_index == selected_lumitium_index {
            stellarium_bonus += 15;
        }
        if stellarium_index == selected_verdantium_index {
            stellarium_bonus += 5;
        }
        if stellarium_index == 0 && stellarium_contains != 0 {
            stellarium_bonus += 20;
        }
        if stellarium_index == 1 && titanwake_index >= 0 {
            stellarium_bonus += 10;
        }
        if stellarium_index == 2 && archbrace_contains != 0 {
            stellarium_bonus += 30;
        }

        let stellarium_score = candidate_stellarium_score + stellarium_bonus;
        if stellarium_score > best_stellarium_score {
            best_stellarium_score = stellarium_score;
            selected_stellarium_index = stellarium_index;
        }

        stellarium_index += 1;
    }

    let mut selected_stellarium_ptr = stellarium_value.as_ptr();
    let mut selected_stellarium_written = stellarium_value_written;
    if selected_stellarium_index == 1 {
        selected_stellarium_ptr = titanwake_value.as_ptr();
        selected_stellarium_written = titanwake_value_written;
    } else if selected_stellarium_index == 2 {
        selected_stellarium_ptr = archbrace_value.as_ptr();
        selected_stellarium_written = archbrace_value_written;
    }

    let novatium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_stellarium_ptr,
            selected_stellarium_written as i64,
            novatium_old.as_ptr(),
            novatium_old.len() as i64,
            novatium_new.as_ptr(),
            novatium_new.len() as i64,
        )
    };
    let mut novatium_value = vec![0u8; novatium_value_len as usize];
    let novatium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_stellarium_ptr,
            selected_stellarium_written as i64,
            novatium_old.as_ptr(),
            novatium_old.len() as i64,
            novatium_new.as_ptr(),
            novatium_new.len() as i64,
            novatium_value.as_mut_ptr(),
            novatium_value.len() as i64,
        )
    };
    let novatium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            novatium_value.as_ptr(),
            novatium_value_written as i64,
            novatium_needle.as_ptr(),
            novatium_needle.len() as i64,
        )
    };

    let platinumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_stellarium_ptr,
            selected_stellarium_written as i64,
            platinumwake_extension.as_ptr(),
            platinumwake_extension.len() as i64,
        )
    };
    let mut platinumwake_value = vec![0u8; platinumwake_value_len as usize];
    let platinumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_stellarium_ptr,
            selected_stellarium_written as i64,
            platinumwake_extension.as_ptr(),
            platinumwake_extension.len() as i64,
            platinumwake_value.as_mut_ptr(),
            platinumwake_value.len() as i64,
        )
    };
    let platinumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            platinumwake_value.as_ptr(),
            platinumwake_value_written as i64,
            platinumwake_needle.as_ptr(),
            platinumwake_needle.len() as i64,
        )
    };

    let keelbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_stellarium_ptr,
            selected_stellarium_written as i64,
        )
    };
    let mut keelbrace_source = vec![0u8; keelbrace_source_len as usize];
    let keelbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_stellarium_ptr,
            selected_stellarium_written as i64,
            keelbrace_source.as_mut_ptr(),
            keelbrace_source.len() as i64,
        )
    };
    let keelbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            keelbrace_source.as_ptr(),
            keelbrace_source_written as i64,
            keelbrace_old.as_ptr(),
            keelbrace_old.len() as i64,
            keelbrace_new.as_ptr(),
            keelbrace_new.len() as i64,
        )
    };
    let mut keelbrace_value = vec![0u8; keelbrace_value_len as usize];
    let keelbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            keelbrace_source.as_ptr(),
            keelbrace_source_written as i64,
            keelbrace_old.as_ptr(),
            keelbrace_old.len() as i64,
            keelbrace_new.as_ptr(),
            keelbrace_new.len() as i64,
            keelbrace_value.as_mut_ptr(),
            keelbrace_value.len() as i64,
        )
    };
    let keelbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            keelbrace_value.as_ptr(),
            keelbrace_value_written as i64,
            keelbrace_needle.as_ptr(),
            keelbrace_needle.len() as i64,
        )
    };

    let mut selected_novatium_index = 0i32;
    let mut best_novatium_score = i32::MIN;
    let mut novatium_index = 0i32;
    while novatium_index < 3 {
        let mut candidate_novatium_score = novatium_value_written * 10 + novatium_contains * 50;
        if novatium_index == 1 {
            candidate_novatium_score = platinumwake_value_written * 10 + platinumwake_index;
        } else if novatium_index == 2 {
            candidate_novatium_score = keelbrace_value_written * 10 + keelbrace_contains * 50;
        }

        let mut novatium_bonus = 0i32;
        if novatium_index == selected_stellarium_index {
            novatium_bonus += 25;
        }
        if novatium_index == selected_prismatium_index {
            novatium_bonus += 15;
        }
        if novatium_index == selected_lumitium_index {
            novatium_bonus += 5;
        }
        if novatium_index == 0 && novatium_contains != 0 {
            novatium_bonus += 20;
        }
        if novatium_index == 1 && platinumwake_index >= 0 {
            novatium_bonus += 10;
        }
        if novatium_index == 2 && keelbrace_contains != 0 {
            novatium_bonus += 30;
        }

        let novatium_score = candidate_novatium_score + novatium_bonus;
        if novatium_score > best_novatium_score {
            best_novatium_score = novatium_score;
            selected_novatium_index = novatium_index;
        }

        novatium_index += 1;
    }

    let mut selected_novatium_ptr = novatium_value.as_ptr();
    let mut selected_novatium_written = novatium_value_written;
    if selected_novatium_index == 1 {
        selected_novatium_ptr = platinumwake_value.as_ptr();
        selected_novatium_written = platinumwake_value_written;
    } else if selected_novatium_index == 2 {
        selected_novatium_ptr = keelbrace_value.as_ptr();
        selected_novatium_written = keelbrace_value_written;
    }

    let polarium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_novatium_ptr,
            selected_novatium_written as i64,
            polarium_old.as_ptr(),
            polarium_old.len() as i64,
            polarium_new.as_ptr(),
            polarium_new.len() as i64,
        )
    };
    let mut polarium_value = vec![0u8; polarium_value_len as usize];
    let polarium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_novatium_ptr,
            selected_novatium_written as i64,
            polarium_old.as_ptr(),
            polarium_old.len() as i64,
            polarium_new.as_ptr(),
            polarium_new.len() as i64,
            polarium_value.as_mut_ptr(),
            polarium_value.len() as i64,
        )
    };
    let polarium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            polarium_value.as_ptr(),
            polarium_value_written as i64,
            polarium_needle.as_ptr(),
            polarium_needle.len() as i64,
        )
    };

    let osmiumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_novatium_ptr,
            selected_novatium_written as i64,
            osmiumwake_extension.as_ptr(),
            osmiumwake_extension.len() as i64,
        )
    };
    let mut osmiumwake_value = vec![0u8; osmiumwake_value_len as usize];
    let osmiumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_novatium_ptr,
            selected_novatium_written as i64,
            osmiumwake_extension.as_ptr(),
            osmiumwake_extension.len() as i64,
            osmiumwake_value.as_mut_ptr(),
            osmiumwake_value.len() as i64,
        )
    };
    let osmiumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            osmiumwake_value.as_ptr(),
            osmiumwake_value_written as i64,
            osmiumwake_needle.as_ptr(),
            osmiumwake_needle.len() as i64,
        )
    };

    let ridgebrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_novatium_ptr,
            selected_novatium_written as i64,
        )
    };
    let mut ridgebrace_source = vec![0u8; ridgebrace_source_len as usize];
    let ridgebrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_novatium_ptr,
            selected_novatium_written as i64,
            ridgebrace_source.as_mut_ptr(),
            ridgebrace_source.len() as i64,
        )
    };
    let ridgebrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            ridgebrace_source.as_ptr(),
            ridgebrace_source_written as i64,
            ridgebrace_old.as_ptr(),
            ridgebrace_old.len() as i64,
            ridgebrace_new.as_ptr(),
            ridgebrace_new.len() as i64,
        )
    };
    let mut ridgebrace_value = vec![0u8; ridgebrace_value_len as usize];
    let ridgebrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            ridgebrace_source.as_ptr(),
            ridgebrace_source_written as i64,
            ridgebrace_old.as_ptr(),
            ridgebrace_old.len() as i64,
            ridgebrace_new.as_ptr(),
            ridgebrace_new.len() as i64,
            ridgebrace_value.as_mut_ptr(),
            ridgebrace_value.len() as i64,
        )
    };
    let ridgebrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            ridgebrace_value.as_ptr(),
            ridgebrace_value_written as i64,
            ridgebrace_needle.as_ptr(),
            ridgebrace_needle.len() as i64,
        )
    };

    let mut selected_polarium_index = 0i32;
    let mut best_polarium_score = i32::MIN;
    let mut polarium_index = 0i32;
    while polarium_index < 3 {
        let mut candidate_polarium_score = polarium_value_written * 10 + polarium_contains * 50;
        if polarium_index == 1 {
            candidate_polarium_score = osmiumwake_value_written * 10 + osmiumwake_index;
        } else if polarium_index == 2 {
            candidate_polarium_score = ridgebrace_value_written * 10 + ridgebrace_contains * 50;
        }

        let mut polarium_bonus = 0i32;
        if polarium_index == selected_novatium_index {
            polarium_bonus += 25;
        }
        if polarium_index == selected_stellarium_index {
            polarium_bonus += 15;
        }
        if polarium_index == selected_prismatium_index {
            polarium_bonus += 5;
        }
        if polarium_index == 0 && polarium_contains != 0 {
            polarium_bonus += 20;
        }
        if polarium_index == 1 && osmiumwake_index >= 0 {
            polarium_bonus += 10;
        }
        if polarium_index == 2 && ridgebrace_contains != 0 {
            polarium_bonus += 30;
        }

        let polarium_score = candidate_polarium_score + polarium_bonus;
        if polarium_score > best_polarium_score {
            best_polarium_score = polarium_score;
            selected_polarium_index = polarium_index;
        }

        polarium_index += 1;
    }

    (selected_directory_index + 1) * 100000000
        + (selected_file_index + 1) * 10000000
        + (selected_variant_index + 1) * 1000000
        + (selected_rebase_index + 1) * 100000
        + (selected_leaf_transform_index + 1) * 10000
        + (selected_path_transform_index + 1) * 1000
        + (selected_recomposition_index + 1) * 100
        + (selected_novatium_index + 1) * 10
        + (selected_polarium_index + 1)
}