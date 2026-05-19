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
pub extern "C" fn dotnet_runtime_path_one_hundred_forty_one_stage_rank_score() -> i32 {
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
    let virelium_old = "polarium";
    let virelium_new = "virelium";
    let virelium_needle = "virelium";
    let rhodiumwake_extension = ".rhodiumwake";
    let rhodiumwake_needle = "rhodiumwake";
    let cantbrace_old = "ridgebrace";
    let cantbrace_new = "cantbrace";
    let cantbrace_needle = "cantbrace";
    let arcadium_old = "virelium";
    let arcadium_new = "arcadium";
    let arcadium_needle = "arcadium";
    let iridiumwake_extension = ".iridiumwake";
    let iridiumwake_needle = "iridiumwake";
    let soffitbrace_old = "cantbrace";
    let soffitbrace_new = "soffitbrace";
    let soffitbrace_needle = "soffitbrace";
    let solvium_old = "arcadium";
    let solvium_new = "solvium";
    let solvium_needle = "solvium";
    let tungstenwake_extension = ".tungstenwake";
    let tungstenwake_needle = "tungstenwake";
    let lintelbrace_old = "soffitbrace";
    let lintelbrace_new = "lintelbrace";
    let lintelbrace_needle = "lintelbrace";
    let eonitium_old = "solvium";
    let eonitium_new = "eonitium";
    let eonitium_needle = "eonitium";
    let vanadiumwake_extension = ".vanadiumwake";
    let vanadiumwake_needle = "vanadiumwake";
    let pilasterbrace_old = "lintelbrace";
    let pilasterbrace_new = "pilasterbrace";
    let pilasterbrace_needle = "pilasterbrace";
    let selenitium_old = "eonitium";
    let selenitium_new = "selenitium";
    let selenitium_needle = "selenitium";
    let ceriumwake_extension = ".ceriumwake";
    let ceriumwake_needle = "ceriumwake";
    let haunchbrace_old = "pilasterbrace";
    let haunchbrace_new = "haunchbrace";
    let haunchbrace_needle = "haunchbrace";
    let orielium_old = "selenitium";
    let orielium_new = "orielium";
    let orielium_needle = "orielium";
    let yttriumwake_extension = ".yttriumwake";
    let yttriumwake_needle = "yttriumwake";
    let transombrace_old = "haunchbrace";
    let transombrace_new = "transombrace";
    let transombrace_needle = "transombrace";
    let umbratium_old = "orielium";
    let umbratium_new = "umbratium";
    let umbratium_needle = "umbratium";
    let zirconiumwake_extension = ".zirconiumwake";
    let zirconiumwake_needle = "zirconiumwake";
    let mullionbrace_old = "transombrace";
    let mullionbrace_new = "mullionbrace";
    let mullionbrace_needle = "mullionbrace";
    let lumenium_old = "umbratium";
    let lumenium_new = "lumenium";
    let lumenium_needle = "lumenium";
    let hafniumwake_extension = ".hafniumwake";
    let hafniumwake_needle = "hafniumwake";
    let clerestorybrace_old = "mullionbrace";
    let clerestorybrace_new = "clerestorybrace";
    let clerestorybrace_needle = "clerestorybrace";
    let stratium_old = "lumenium";
    let stratium_new = "stratium";
    let stratium_needle = "stratium";
    let tantalumwake_extension = ".tantalumwake";
    let tantalumwake_needle = "tantalumwake";
    let fanlightbrace_old = "clerestorybrace";
    let fanlightbrace_new = "fanlightbrace";
    let fanlightbrace_needle = "fanlightbrace";
    let venturium_old = "stratium";
    let venturium_new = "venturium";
    let venturium_needle = "venturium";
    let niobiumwake_extension = ".niobiumwake";
    let niobiumwake_needle = "niobiumwake";
    let oculusbrace_old = "fanlightbrace";
    let oculusbrace_new = "oculusbrace";
    let oculusbrace_needle = "oculusbrace";
    let meridianium_old = "venturium";
    let meridianium_new = "meridianium";
    let meridianium_needle = "meridianium";
    let xenonwake_extension = ".xenonwake";
    let xenonwake_needle = "xenonwake";
    let lancetbrace_old = "oculusbrace";
    let lancetbrace_new = "lancetbrace";
    let lancetbrace_needle = "lancetbrace";
    let perihelium_old = "meridianium";
    let perihelium_new = "perihelium";
    let perihelium_needle = "perihelium";
    let argonwake_extension = ".argonwake";
    let argonwake_needle = "argonwake";
    let voussoirbrace_old = "lancetbrace";
    let voussoirbrace_new = "voussoirbrace";
    let voussoirbrace_needle = "voussoirbrace";
    let equinoctium_old = "perihelium";
    let equinoctium_new = "equinoctium";
    let equinoctium_needle = "equinoctium";
    let neonwake_extension = ".neonwake";
    let neonwake_needle = "neonwake";
    let keystonebrace_old = "voussoirbrace";
    let keystonebrace_new = "keystonebrace";
    let keystonebrace_needle = "keystonebrace";
    let syzygyium_old = "equinoctium";
    let syzygyium_new = "syzygyium";
    let syzygyium_needle = "syzygyium";
    let radonwake_extension = ".radonwake";
    let radonwake_needle = "radonwake";
    let spandrelbrace_old = "keystonebrace";
    let spandrelbrace_new = "spandrelbrace";
    let spandrelbrace_needle = "spandrelbrace";
    let parallaxium_old = "syzygyium";
    let parallaxium_new = "parallaxium";
    let parallaxium_needle = "parallaxium";
    let cesiumwake_extension = ".cesiumwake";
    let cesiumwake_needle = "cesiumwake";
    let tracerybrace_old = "spandrelbrace";
    let tracerybrace_new = "tracerybrace";
    let tracerybrace_needle = "tracerybrace";
    let declinationium_old = "parallaxium";
    let declinationium_new = "declinationium";
    let declinationium_needle = "declinationium";
    let strontiumwake_extension = ".strontiumwake";
    let strontiumwake_needle = "strontiumwake";
    let extradosbrace_old = "tracerybrace";
    let extradosbrace_new = "extradosbrace";
    let extradosbrace_needle = "extradosbrace";
    let azimuthium_old = "declinationium";
    let azimuthium_new = "azimuthium";
    let azimuthium_needle = "azimuthium";
    let bariumwake_extension = ".bariumwake";
    let bariumwake_needle = "bariumwake";
    let impostbrace_old = "extradosbrace";
    let impostbrace_new = "impostbrace";
    let impostbrace_needle = "impostbrace";
    let aphelionium_old = "azimuthium";
    let aphelionium_new = "aphelionium";
    let aphelionium_needle = "aphelionium";
    let rheniumwake_extension = ".rheniumwake";
    let rheniumwake_needle = "rheniumwake";
    let springerbrace_old = "impostbrace";
    let springerbrace_new = "springerbrace";
    let springerbrace_needle = "springerbrace";
    let periapsisium_old = "aphelionium";
    let periapsisium_new = "periapsisium";
    let periapsisium_needle = "periapsisium";
    let quasarwake_extension = ".quasarwake";
    let quasarwake_needle = "quasarwake";
    let skewbackbrace_old = "springerbrace";
    let skewbackbrace_new = "skewbackbrace";
    let skewbackbrace_needle = "skewbackbrace";
    let apsidialium_old = "periapsisium";
    let apsidialium_new = "apsidialium";
    let apsidialium_needle = "apsidialium";
    let pulsarwake_extension = ".pulsarwake";
    let pulsarwake_needle = "pulsarwake";
    let abutmentbrace_old = "skewbackbrace";
    let abutmentbrace_new = "abutmentbrace";
    let abutmentbrace_needle = "abutmentbrace";
    let eccentricium_old = "apsidialium";
    let eccentricium_new = "eccentricium";
    let eccentricium_needle = "eccentricium";
    let nebularwake_extension = ".nebularwake";
    let nebularwake_needle = "nebularwake";
    let plinthbrace_old = "abutmentbrace";
    let plinthbrace_new = "plinthbrace";
    let plinthbrace_needle = "plinthbrace";
    let epicyclium_old = "eccentricium";
    let epicyclium_new = "epicyclium";
    let epicyclium_needle = "epicyclium";
    let magnetarwake_extension = ".magnetarwake";
    let magnetarwake_needle = "magnetarwake";
    let pedestalbrace_old = "plinthbrace";
    let pedestalbrace_new = "pedestalbrace";
    let pedestalbrace_needle = "pedestalbrace";
    let deferentium_old = "epicyclium";
    let deferentium_new = "deferentium";
    let deferentium_needle = "deferentium";
    let maserwake_extension = ".maserwake";
    let maserwake_needle = "maserwake";
    let soclebrace_old = "pedestalbrace";
    let soclebrace_new = "soclebrace";
    let soclebrace_needle = "soclebrace";
    let anomalyium_old = "deferentium";
    let anomalyium_new = "anomalyium";
    let anomalyium_needle = "anomalyium";
    let blazarwake_extension = ".blazarwake";
    let blazarwake_needle = "blazarwake";
    let stylobatebrace_old = "soclebrace";
    let stylobatebrace_new = "stylobatebrace";
    let stylobatebrace_needle = "stylobatebrace";
    let inclinationium_old = "anomalyium";
    let inclinationium_new = "inclinationium";
    let inclinationium_needle = "inclinationium";
    let novaewake_extension = ".novaewake";
    let novaewake_needle = "novaewake";
    let crepidomabrace_old = "stylobatebrace";
    let crepidomabrace_new = "crepidomabrace";
    let crepidomabrace_needle = "crepidomabrace";
    let apsidium_old = "inclinationium";
    let apsidium_new = "apsidium";
    let apsidium_needle = "apsidium";
    let magnetowake_extension = ".magnetowake";
    let magnetowake_needle = "magnetowake";
    let cryptbrace_old = "crepidomabrace";
    let cryptbrace_new = "cryptbrace";
    let cryptbrace_needle = "cryptbrace";
    let librationium_old = "apsidium";
    let librationium_new = "librationium";
    let librationium_needle = "librationium";
    let coronawake_extension = ".coronawake";
    let coronawake_needle = "coronawake";
    let trumeaubrace_old = "cryptbrace";
    let trumeaubrace_new = "trumeaubrace";
    let trumeaubrace_needle = "trumeaubrace";
    let precessionium_old = "librationium";
    let precessionium_new = "precessionium";
    let precessionium_needle = "precessionium";
    let heliowake_extension = ".heliowake";
    let heliowake_needle = "heliowake";
    let archivoltbrace_old = "trumeaubrace";
    let archivoltbrace_new = "archivoltbrace";
    let archivoltbrace_needle = "archivoltbrace";
    let nutationium_old = "precessionium";
    let nutationium_new = "nutationium";
    let nutationium_needle = "nutationium";
    let photowake_extension = ".photowake";
    let photowake_needle = "photowake";
    let tympanumbrace_old = "archivoltbrace";
    let tympanumbrace_new = "tympanumbrace";
    let tympanumbrace_needle = "tympanumbrace";
    let osculationium_old = "nutationium";
    let osculationium_new = "osculationium";
    let osculationium_needle = "osculationium";
    let spectrawake_extension = ".spectrawake";
    let spectrawake_needle = "spectrawake";
    let lunettebrace_old = "tympanumbrace";
    let lunettebrace_new = "lunettebrace";
    let lunettebrace_needle = "lunettebrace";
    let apselineium_old = "osculationium";
    let apselineium_new = "apselineium";
    let apselineium_needle = "apselineium";
    let radiowake_extension = ".radiowake";
    let radiowake_needle = "radiowake";
    let voussurebrace_old = "lunettebrace";
    let voussurebrace_new = "voussurebrace";
    let voussurebrace_needle = "voussurebrace";
    let periastronium_old = "apselineium";
    let periastronium_new = "periastronium";
    let periastronium_needle = "periastronium";
    let ionowake_extension = ".ionowake";
    let ionowake_needle = "ionowake";
    let pendentivebrace_old = "voussurebrace";
    let pendentivebrace_new = "pendentivebrace";
    let pendentivebrace_needle = "pendentivebrace";
    let barycentrium_old = "periastronium";
    let barycentrium_new = "barycentrium";
    let barycentrium_needle = "barycentrium";
    let plasmawake_extension = ".plasmawake";
    let plasmawake_needle = "plasmawake";
    let squinchbrace_old = "pendentivebrace";
    let squinchbrace_new = "squinchbrace";
    let squinchbrace_needle = "squinchbrace";
    let rochelium_old = "barycentrium";
    let rochelium_new = "rochelium";
    let rochelium_needle = "rochelium";
    let neutrinowake_extension = ".neutrinowake";
    let neutrinowake_needle = "neutrinowake";
    let rusticationbrace_old = "squinchbrace";
    let rusticationbrace_new = "rusticationbrace";
    let rusticationbrace_needle = "rusticationbrace";
    let lissajousium_old = "rochelium";
    let lissajousium_new = "lissajousium";
    let lissajousium_needle = "lissajousium";
    let tachyonwake_extension = ".tachyonwake";
    let tachyonwake_needle = "tachyonwake";
    let ashlarbrace_old = "rusticationbrace";
    let ashlarbrace_new = "ashlarbrace";
    let ashlarbrace_needle = "ashlarbrace";
    let cycloidium_old = "lissajousium";
    let cycloidium_new = "cycloidium";
    let cycloidium_needle = "cycloidium";
    let gravitonwake_extension = ".gravitonwake";
    let gravitonwake_needle = "gravitonwake";
    let quoinsbrace_old = "ashlarbrace";
    let quoinsbrace_new = "quoinsbrace";
    let quoinsbrace_needle = "quoinsbrace";
    let hypotrochoidium_old = "cycloidium";
    let hypotrochoidium_new = "hypotrochoidium";
    let hypotrochoidium_needle = "hypotrochoidium";
    let bosonwake_extension = ".bosonwake";
    let bosonwake_needle = "bosonwake";
    let masonrybrace_old = "quoinsbrace";
    let masonrybrace_new = "masonrybrace";
    let masonrybrace_needle = "masonrybrace";
    let epicycloidium_old = "hypotrochoidium";
    let epicycloidium_new = "epicycloidium";
    let epicycloidium_needle = "epicycloidium";
    let muonwake_extension = ".muonwake";
    let muonwake_needle = "muonwake";
    let fretworkbrace_old = "masonrybrace";
    let fretworkbrace_new = "fretworkbrace";
    let fretworkbrace_needle = "fretworkbrace";
    let pericycloidium_old = "epicycloidium";
    let pericycloidium_new = "pericycloidium";
    let pericycloidium_needle = "pericycloidium";
    let gluonwake_extension = ".gluonwake";
    let gluonwake_needle = "gluonwake";
    let trellisbrace_old = "fretworkbrace";
    let trellisbrace_new = "trellisbrace";
    let trellisbrace_needle = "trellisbrace";
    let astroidium_old = "pericycloidium";
    let astroidium_new = "astroidium";
    let astroidium_needle = "astroidium";
    let phononwake_extension = ".phononwake";
    let phononwake_needle = "phononwake";
    let filigreebrace_old = "trellisbrace";
    let filigreebrace_new = "filigreebrace";
    let filigreebrace_needle = "filigreebrace";
    let deltoidium_old = "astroidium";
    let deltoidium_new = "deltoidium";
    let deltoidium_needle = "deltoidium";
    let magnonwake_extension = ".magnonwake";
    let magnonwake_needle = "magnonwake";
    let arabesquebrace_old = "filigreebrace";
    let arabesquebrace_new = "arabesquebrace";
    let arabesquebrace_needle = "arabesquebrace";
    let lemniscatoidium_old = "deltoidium";
    let lemniscatoidium_new = "lemniscatoidium";
    let lemniscatoidium_needle = "lemniscatoidium";
    let plasmonwake_extension = ".plasmonwake";
    let plasmonwake_needle = "plasmonwake";
    let ornamentbrace_old = "arabesquebrace";
    let ornamentbrace_new = "ornamentbrace";
    let ornamentbrace_needle = "ornamentbrace";
    let rosecurvium_old = "lemniscatoidium";
    let rosecurvium_new = "rosecurvium";
    let rosecurvium_needle = "rosecurvium";
    let solitonwake_extension = ".solitonwake";
    let solitonwake_needle = "solitonwake";
    let scrollbrace_old = "ornamentbrace";
    let scrollbrace_new = "scrollbrace";
    let scrollbrace_needle = "scrollbrace";
    let cardioidium_old = "rosecurvium";
    let cardioidium_new = "cardioidium";
    let cardioidium_needle = "cardioidium";
    let ionwake_extension = ".ionwake";
    let ionwake_needle = "ionwake";
    let embossbrace_old = "scrollbrace";
    let embossbrace_new = "embossbrace";
    let embossbrace_needle = "embossbrace";
    let epitrochoidion_old = "cardioidium";
    let epitrochoidion_new = "epitrochoidion";
    let epitrochoidion_needle = "epitrochoidion";
    let quarkwake_extension = ".quarkwake";
    let quarkwake_needle = "quarkwake";
    let frescobrace_old = "embossbrace";
    let frescobrace_new = "frescobrace";
    let frescobrace_needle = "frescobrace";
    let superellipsium_old = "epitrochoidion";
    let superellipsium_new = "superellipsium";
    let superellipsium_needle = "superellipsium";
    let hadronwake_extension = ".hadronwake";
    let hadronwake_needle = "hadronwake";
    let cartouchebrace_old = "frescobrace";
    let cartouchebrace_new = "cartouchebrace";
    let cartouchebrace_needle = "cartouchebrace";
    let hypocycloidette_old = "superellipsium";
    let hypocycloidette_new = "hypocycloidette";
    let hypocycloidette_needle = "hypocycloidette";
    let mesonwake_extension = ".mesonwake";
    let mesonwake_needle = "mesonwake";
    let basreliefbrace_old = "cartouchebrace";
    let basreliefbrace_new = "basreliefbrace";
    let basreliefbrace_needle = "basreliefbrace";
    let peritrochoidette_old = "hypocycloidette";
    let peritrochoidette_new = "peritrochoidette";
    let peritrochoidette_needle = "peritrochoidette";
    let gluinowake_extension = ".gluinowake";
    let gluinowake_needle = "gluinowake";
    let intagliobrace_old = "basreliefbrace";
    let intagliobrace_new = "intagliobrace";
    let intagliobrace_needle = "intagliobrace";
    let trochoidetta_old = "peritrochoidette";
    let trochoidetta_new = "trochoidetta";
    let trochoidetta_needle = "trochoidetta";
    let sfermionwake_extension = ".sfermionwake";
    let sfermionwake_needle = "sfermionwake";
    let grisaillebrace_old = "intagliobrace";
    let grisaillebrace_new = "grisaillebrace";
    let grisaillebrace_needle = "grisaillebrace";
    let epitrochoidetta_old = "trochoidetta";
    let epitrochoidetta_new = "epitrochoidetta";
    let epitrochoidetta_needle = "epitrochoidetta";
    let tachyonettewake_extension = ".tachyonettewake";
    let tachyonettewake_needle = "tachyonettewake";
    let filletbrace_old = "grisaillebrace";
    let filletbrace_new = "filletbrace";
    let filletbrace_needle = "filletbrace";
    let hypotrochoidetta_old = "epitrochoidetta";
    let hypotrochoidetta_new = "hypotrochoidetta";
    let hypotrochoidetta_needle = "hypotrochoidetta";
    let quintonwake_extension = ".quintonwake";
    let quintonwake_needle = "quintonwake";
    let volutebrace_old = "filletbrace";
    let volutebrace_new = "volutebrace";
    let volutebrace_needle = "volutebrace";
    let epitrochoidula_old = "hypotrochoidetta";
    let epitrochoidula_new = "epitrochoidula";
    let epitrochoidula_needle = "epitrochoidula";
    let septonwake_extension = ".septonwake";
    let septonwake_needle = "septonwake";
    let guillochebrace_old = "volutebrace";
    let guillochebrace_new = "guillochebrace";
    let guillochebrace_needle = "guillochebrace";
    let orthotrochoidula_old = "epitrochoidula";
    let orthotrochoidula_new = "orthotrochoidula";
    let orthotrochoidula_needle = "orthotrochoidula";
    let nononwake_extension = ".nononwake";
    let nononwake_needle = "nononwake";
    let strapworkbrace_old = "guillochebrace";
    let strapworkbrace_new = "strapworkbrace";
    let strapworkbrace_needle = "strapworkbrace";
    let deltoidula_old = "orthotrochoidula";
    let deltoidula_new = "deltoidula";
    let deltoidula_needle = "deltoidula";
    let decuonwake_extension = ".decuonwake";
    let decuonwake_needle = "decuonwake";
    let carcanetbrace_old = "strapworkbrace";
    let carcanetbrace_new = "carcanetbrace";
    let carcanetbrace_needle = "carcanetbrace";
    let astroidula_old = "deltoidula";
    let astroidula_new = "astroidula";
    let astroidula_needle = "astroidula";
    let hendecawake_extension = ".hendecawake";
    let hendecawake_needle = "hendecawake";
    let festoonbrace_old = "carcanetbrace";
    let festoonbrace_new = "festoonbrace";
    let festoonbrace_needle = "festoonbrace";
    let lemniscatula_old = "astroidula";
    let lemniscatula_new = "lemniscatula";
    let lemniscatula_needle = "lemniscatula";
    let duodecawake_extension = ".duodecawake";
    let duodecawake_needle = "duodecawake";
    let cartoucheband_old = "festoonbrace";
    let cartoucheband_new = "cartoucheband";
    let cartoucheband_needle = "cartoucheband";
    let rosecurvula_old = "lemniscatula";
    let rosecurvula_new = "rosecurvula";
    let rosecurvula_needle = "rosecurvula";
    let tridecawake_extension = ".tridecawake";
    let tridecawake_needle = "tridecawake";
    let cinctureband_old = "cartoucheband";
    let cinctureband_new = "cinctureband";
    let cinctureband_needle = "cinctureband";
    let cardioidula_old = "rosecurvula";
    let cardioidula_new = "cardioidula";
    let cardioidula_needle = "cardioidula";
    let tetradecawake_extension = ".tetradecawake";
    let tetradecawake_needle = "tetradecawake";
    let ribbonlace_old = "cinctureband";
    let ribbonlace_new = "ribbonlace";
    let ribbonlace_needle = "ribbonlace";
    let nephroidette_old = "cardioidula";
    let nephroidette_new = "nephroidette";
    let nephroidette_needle = "nephroidette";
    let pentadecawake_extension = ".pentadecawake";
    let pentadecawake_needle = "pentadecawake";
    let gallooncord_old = "ribbonlace";
    let gallooncord_new = "gallooncord";
    let gallooncord_needle = "gallooncord";
    let epicycloidette_old = "nephroidette";
    let epicycloidette_new = "epicycloidette";
    let epicycloidette_needle = "epicycloidette";
    let hexadecawake_extension = ".hexadecawake";
    let hexadecawake_needle = "hexadecawake";
    let brocatelle_old = "gallooncord";
    let brocatelle_new = "brocatelle";
    let brocatelle_needle = "brocatelle";
    let campyloidette_old = "epicycloidette";
    let campyloidette_new = "campyloidette";
    let campyloidette_needle = "campyloidette";
    let heptadecawake_extension = ".heptadecawake";
    let heptadecawake_needle = "heptadecawake";
    let soutachecord_old = "brocatelle";
    let soutachecord_new = "soutachecord";
    let soutachecord_needle = "soutachecord";
    let spheroidette_old = "campyloidette";
    let spheroidette_new = "spheroidette";
    let spheroidette_needle = "spheroidette";
    let octadecawake_extension = ".octadecawake";
    let octadecawake_needle = "octadecawake";
    let tambourcord_old = "soutachecord";
    let tambourcord_new = "tambourcord";
    let tambourcord_needle = "tambourcord";

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

    let mut selected_polarium_ptr = polarium_value.as_ptr();
    let mut selected_polarium_written = polarium_value_written;
    if selected_polarium_index == 1 {
        selected_polarium_ptr = osmiumwake_value.as_ptr();
        selected_polarium_written = osmiumwake_value_written;
    } else if selected_polarium_index == 2 {
        selected_polarium_ptr = ridgebrace_value.as_ptr();
        selected_polarium_written = ridgebrace_value_written;
    }

    let virelium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_polarium_ptr,
            selected_polarium_written as i64,
            virelium_old.as_ptr(),
            virelium_old.len() as i64,
            virelium_new.as_ptr(),
            virelium_new.len() as i64,
        )
    };
    let mut virelium_value = vec![0u8; virelium_value_len as usize];
    let virelium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_polarium_ptr,
            selected_polarium_written as i64,
            virelium_old.as_ptr(),
            virelium_old.len() as i64,
            virelium_new.as_ptr(),
            virelium_new.len() as i64,
            virelium_value.as_mut_ptr(),
            virelium_value.len() as i64,
        )
    };
    let virelium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            virelium_value.as_ptr(),
            virelium_value_written as i64,
            virelium_needle.as_ptr(),
            virelium_needle.len() as i64,
        )
    };

    let rhodiumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_polarium_ptr,
            selected_polarium_written as i64,
            rhodiumwake_extension.as_ptr(),
            rhodiumwake_extension.len() as i64,
        )
    };
    let mut rhodiumwake_value = vec![0u8; rhodiumwake_value_len as usize];
    let rhodiumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_polarium_ptr,
            selected_polarium_written as i64,
            rhodiumwake_extension.as_ptr(),
            rhodiumwake_extension.len() as i64,
            rhodiumwake_value.as_mut_ptr(),
            rhodiumwake_value.len() as i64,
        )
    };
    let rhodiumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            rhodiumwake_value.as_ptr(),
            rhodiumwake_value_written as i64,
            rhodiumwake_needle.as_ptr(),
            rhodiumwake_needle.len() as i64,
        )
    };

    let cantbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_polarium_ptr,
            selected_polarium_written as i64,
        )
    };
    let mut cantbrace_source = vec![0u8; cantbrace_source_len as usize];
    let cantbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_polarium_ptr,
            selected_polarium_written as i64,
            cantbrace_source.as_mut_ptr(),
            cantbrace_source.len() as i64,
        )
    };
    let cantbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            cantbrace_source.as_ptr(),
            cantbrace_source_written as i64,
            cantbrace_old.as_ptr(),
            cantbrace_old.len() as i64,
            cantbrace_new.as_ptr(),
            cantbrace_new.len() as i64,
        )
    };
    let mut cantbrace_value = vec![0u8; cantbrace_value_len as usize];
    let cantbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            cantbrace_source.as_ptr(),
            cantbrace_source_written as i64,
            cantbrace_old.as_ptr(),
            cantbrace_old.len() as i64,
            cantbrace_new.as_ptr(),
            cantbrace_new.len() as i64,
            cantbrace_value.as_mut_ptr(),
            cantbrace_value.len() as i64,
        )
    };
    let cantbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            cantbrace_value.as_ptr(),
            cantbrace_value_written as i64,
            cantbrace_needle.as_ptr(),
            cantbrace_needle.len() as i64,
        )
    };

    let mut selected_virelium_index = 0i32;
    let mut best_virelium_score = i32::MIN;
    let mut virelium_index = 0i32;
    while virelium_index < 3 {
        let mut candidate_virelium_score = virelium_value_written * 10 + virelium_contains * 50;
        if virelium_index == 1 {
            candidate_virelium_score = rhodiumwake_value_written * 10 + rhodiumwake_index;
        } else if virelium_index == 2 {
            candidate_virelium_score = cantbrace_value_written * 10 + cantbrace_contains * 50;
        }

        let mut virelium_bonus = 0i32;
        if virelium_index == selected_polarium_index {
            virelium_bonus += 25;
        }
        if virelium_index == selected_novatium_index {
            virelium_bonus += 15;
        }
        if virelium_index == selected_stellarium_index {
            virelium_bonus += 5;
        }
        if virelium_index == 0 && virelium_contains != 0 {
            virelium_bonus += 20;
        }
        if virelium_index == 1 && rhodiumwake_index >= 0 {
            virelium_bonus += 10;
        }
        if virelium_index == 2 && cantbrace_contains != 0 {
            virelium_bonus += 30;
        }

        let virelium_score = candidate_virelium_score + virelium_bonus;
        if virelium_score > best_virelium_score {
            best_virelium_score = virelium_score;
            selected_virelium_index = virelium_index;
        }

        virelium_index += 1;
    }

    let mut selected_virelium_ptr = virelium_value.as_ptr();
    let mut selected_virelium_written = virelium_value_written;
    if selected_virelium_index == 1 {
        selected_virelium_ptr = rhodiumwake_value.as_ptr();
        selected_virelium_written = rhodiumwake_value_written;
    } else if selected_virelium_index == 2 {
        selected_virelium_ptr = cantbrace_value.as_ptr();
        selected_virelium_written = cantbrace_value_written;
    }

    let arcadium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_virelium_ptr,
            selected_virelium_written as i64,
            arcadium_old.as_ptr(),
            arcadium_old.len() as i64,
            arcadium_new.as_ptr(),
            arcadium_new.len() as i64,
        )
    };
    let mut arcadium_value = vec![0u8; arcadium_value_len as usize];
    let arcadium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_virelium_ptr,
            selected_virelium_written as i64,
            arcadium_old.as_ptr(),
            arcadium_old.len() as i64,
            arcadium_new.as_ptr(),
            arcadium_new.len() as i64,
            arcadium_value.as_mut_ptr(),
            arcadium_value.len() as i64,
        )
    };
    let arcadium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            arcadium_value.as_ptr(),
            arcadium_value_written as i64,
            arcadium_needle.as_ptr(),
            arcadium_needle.len() as i64,
        )
    };

    let iridiumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_virelium_ptr,
            selected_virelium_written as i64,
            iridiumwake_extension.as_ptr(),
            iridiumwake_extension.len() as i64,
        )
    };
    let mut iridiumwake_value = vec![0u8; iridiumwake_value_len as usize];
    let iridiumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_virelium_ptr,
            selected_virelium_written as i64,
            iridiumwake_extension.as_ptr(),
            iridiumwake_extension.len() as i64,
            iridiumwake_value.as_mut_ptr(),
            iridiumwake_value.len() as i64,
        )
    };
    let iridiumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            iridiumwake_value.as_ptr(),
            iridiumwake_value_written as i64,
            iridiumwake_needle.as_ptr(),
            iridiumwake_needle.len() as i64,
        )
    };

    let soffitbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_virelium_ptr,
            selected_virelium_written as i64,
        )
    };
    let mut soffitbrace_source = vec![0u8; soffitbrace_source_len as usize];
    let soffitbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_virelium_ptr,
            selected_virelium_written as i64,
            soffitbrace_source.as_mut_ptr(),
            soffitbrace_source.len() as i64,
        )
    };
    let soffitbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            soffitbrace_source.as_ptr(),
            soffitbrace_source_written as i64,
            soffitbrace_old.as_ptr(),
            soffitbrace_old.len() as i64,
            soffitbrace_new.as_ptr(),
            soffitbrace_new.len() as i64,
        )
    };
    let mut soffitbrace_value = vec![0u8; soffitbrace_value_len as usize];
    let soffitbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            soffitbrace_source.as_ptr(),
            soffitbrace_source_written as i64,
            soffitbrace_old.as_ptr(),
            soffitbrace_old.len() as i64,
            soffitbrace_new.as_ptr(),
            soffitbrace_new.len() as i64,
            soffitbrace_value.as_mut_ptr(),
            soffitbrace_value.len() as i64,
        )
    };
    let soffitbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            soffitbrace_value.as_ptr(),
            soffitbrace_value_written as i64,
            soffitbrace_needle.as_ptr(),
            soffitbrace_needle.len() as i64,
        )
    };

    let mut selected_arcadium_index = 0i32;
    let mut best_arcadium_score = i32::MIN;
    let mut arcadium_index = 0i32;
    while arcadium_index < 3 {
        let mut candidate_arcadium_score = arcadium_value_written * 10 + arcadium_contains * 50;
        if arcadium_index == 1 {
            candidate_arcadium_score = iridiumwake_value_written * 10 + iridiumwake_index;
        } else if arcadium_index == 2 {
            candidate_arcadium_score = soffitbrace_value_written * 10 + soffitbrace_contains * 50;
        }

        let mut arcadium_bonus = 0i32;
        if arcadium_index == selected_virelium_index {
            arcadium_bonus += 25;
        }
        if arcadium_index == selected_polarium_index {
            arcadium_bonus += 15;
        }
        if arcadium_index == selected_novatium_index {
            arcadium_bonus += 5;
        }
        if arcadium_index == 0 && arcadium_contains != 0 {
            arcadium_bonus += 20;
        }
        if arcadium_index == 1 && iridiumwake_index >= 0 {
            arcadium_bonus += 10;
        }
        if arcadium_index == 2 && soffitbrace_contains != 0 {
            arcadium_bonus += 30;
        }

        let arcadium_score = candidate_arcadium_score + arcadium_bonus;
        if arcadium_score > best_arcadium_score {
            best_arcadium_score = arcadium_score;
            selected_arcadium_index = arcadium_index;
        }

        arcadium_index += 1;
    }

    let mut selected_arcadium_ptr = arcadium_value.as_ptr();
    let mut selected_arcadium_written = arcadium_value_written;
    if selected_arcadium_index == 1 {
        selected_arcadium_ptr = iridiumwake_value.as_ptr();
        selected_arcadium_written = iridiumwake_value_written;
    } else if selected_arcadium_index == 2 {
        selected_arcadium_ptr = soffitbrace_value.as_ptr();
        selected_arcadium_written = soffitbrace_value_written;
    }

    let solvium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_arcadium_ptr,
            selected_arcadium_written as i64,
            solvium_old.as_ptr(),
            solvium_old.len() as i64,
            solvium_new.as_ptr(),
            solvium_new.len() as i64,
        )
    };
    let mut solvium_value = vec![0u8; solvium_value_len as usize];
    let solvium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_arcadium_ptr,
            selected_arcadium_written as i64,
            solvium_old.as_ptr(),
            solvium_old.len() as i64,
            solvium_new.as_ptr(),
            solvium_new.len() as i64,
            solvium_value.as_mut_ptr(),
            solvium_value.len() as i64,
        )
    };
    let solvium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            solvium_value.as_ptr(),
            solvium_value_written as i64,
            solvium_needle.as_ptr(),
            solvium_needle.len() as i64,
        )
    };

    let tungstenwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_arcadium_ptr,
            selected_arcadium_written as i64,
            tungstenwake_extension.as_ptr(),
            tungstenwake_extension.len() as i64,
        )
    };
    let mut tungstenwake_value = vec![0u8; tungstenwake_value_len as usize];
    let tungstenwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_arcadium_ptr,
            selected_arcadium_written as i64,
            tungstenwake_extension.as_ptr(),
            tungstenwake_extension.len() as i64,
            tungstenwake_value.as_mut_ptr(),
            tungstenwake_value.len() as i64,
        )
    };
    let tungstenwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            tungstenwake_value.as_ptr(),
            tungstenwake_value_written as i64,
            tungstenwake_needle.as_ptr(),
            tungstenwake_needle.len() as i64,
        )
    };

    let lintelbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_arcadium_ptr,
            selected_arcadium_written as i64,
        )
    };
    let mut lintelbrace_source = vec![0u8; lintelbrace_source_len as usize];
    let lintelbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_arcadium_ptr,
            selected_arcadium_written as i64,
            lintelbrace_source.as_mut_ptr(),
            lintelbrace_source.len() as i64,
        )
    };
    let lintelbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            lintelbrace_source.as_ptr(),
            lintelbrace_source_written as i64,
            lintelbrace_old.as_ptr(),
            lintelbrace_old.len() as i64,
            lintelbrace_new.as_ptr(),
            lintelbrace_new.len() as i64,
        )
    };
    let mut lintelbrace_value = vec![0u8; lintelbrace_value_len as usize];
    let lintelbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            lintelbrace_source.as_ptr(),
            lintelbrace_source_written as i64,
            lintelbrace_old.as_ptr(),
            lintelbrace_old.len() as i64,
            lintelbrace_new.as_ptr(),
            lintelbrace_new.len() as i64,
            lintelbrace_value.as_mut_ptr(),
            lintelbrace_value.len() as i64,
        )
    };
    let lintelbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            lintelbrace_value.as_ptr(),
            lintelbrace_value_written as i64,
            lintelbrace_needle.as_ptr(),
            lintelbrace_needle.len() as i64,
        )
    };

    let mut selected_solvium_index = 0i32;
    let mut best_solvium_score = i32::MIN;
    let mut solvium_index = 0i32;
    while solvium_index < 3 {
        let mut candidate_solvium_score = solvium_value_written * 10 + solvium_contains * 50;
        if solvium_index == 1 {
            candidate_solvium_score = tungstenwake_value_written * 10 + tungstenwake_index;
        } else if solvium_index == 2 {
            candidate_solvium_score = lintelbrace_value_written * 10 + lintelbrace_contains * 50;
        }

        let mut solvium_bonus = 0i32;
        if solvium_index == selected_arcadium_index {
            solvium_bonus += 25;
        }
        if solvium_index == selected_virelium_index {
            solvium_bonus += 15;
        }
        if solvium_index == selected_polarium_index {
            solvium_bonus += 5;
        }
        if solvium_index == 0 && solvium_contains != 0 {
            solvium_bonus += 20;
        }
        if solvium_index == 1 && tungstenwake_index >= 0 {
            solvium_bonus += 10;
        }
        if solvium_index == 2 && lintelbrace_contains != 0 {
            solvium_bonus += 30;
        }

        let solvium_score = candidate_solvium_score + solvium_bonus;
        if solvium_score > best_solvium_score {
            best_solvium_score = solvium_score;
            selected_solvium_index = solvium_index;
        }

        solvium_index += 1;
    }

    let mut selected_solvium_ptr = solvium_value.as_ptr();
    let mut selected_solvium_written = solvium_value_written;
    if selected_solvium_index == 1 {
        selected_solvium_ptr = tungstenwake_value.as_ptr();
        selected_solvium_written = tungstenwake_value_written;
    } else if selected_solvium_index == 2 {
        selected_solvium_ptr = lintelbrace_value.as_ptr();
        selected_solvium_written = lintelbrace_value_written;
    }

    let eonitium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_solvium_ptr,
            selected_solvium_written as i64,
            eonitium_old.as_ptr(),
            eonitium_old.len() as i64,
            eonitium_new.as_ptr(),
            eonitium_new.len() as i64,
        )
    };
    let mut eonitium_value = vec![0u8; eonitium_value_len as usize];
    let eonitium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_solvium_ptr,
            selected_solvium_written as i64,
            eonitium_old.as_ptr(),
            eonitium_old.len() as i64,
            eonitium_new.as_ptr(),
            eonitium_new.len() as i64,
            eonitium_value.as_mut_ptr(),
            eonitium_value.len() as i64,
        )
    };
    let eonitium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            eonitium_value.as_ptr(),
            eonitium_value_written as i64,
            eonitium_needle.as_ptr(),
            eonitium_needle.len() as i64,
        )
    };

    let vanadiumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_solvium_ptr,
            selected_solvium_written as i64,
            vanadiumwake_extension.as_ptr(),
            vanadiumwake_extension.len() as i64,
        )
    };
    let mut vanadiumwake_value = vec![0u8; vanadiumwake_value_len as usize];
    let vanadiumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_solvium_ptr,
            selected_solvium_written as i64,
            vanadiumwake_extension.as_ptr(),
            vanadiumwake_extension.len() as i64,
            vanadiumwake_value.as_mut_ptr(),
            vanadiumwake_value.len() as i64,
        )
    };
    let vanadiumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            vanadiumwake_value.as_ptr(),
            vanadiumwake_value_written as i64,
            vanadiumwake_needle.as_ptr(),
            vanadiumwake_needle.len() as i64,
        )
    };

    let pilasterbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_solvium_ptr,
            selected_solvium_written as i64,
        )
    };
    let mut pilasterbrace_source = vec![0u8; pilasterbrace_source_len as usize];
    let pilasterbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_solvium_ptr,
            selected_solvium_written as i64,
            pilasterbrace_source.as_mut_ptr(),
            pilasterbrace_source.len() as i64,
        )
    };
    let pilasterbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            pilasterbrace_source.as_ptr(),
            pilasterbrace_source_written as i64,
            pilasterbrace_old.as_ptr(),
            pilasterbrace_old.len() as i64,
            pilasterbrace_new.as_ptr(),
            pilasterbrace_new.len() as i64,
        )
    };
    let mut pilasterbrace_value = vec![0u8; pilasterbrace_value_len as usize];
    let pilasterbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            pilasterbrace_source.as_ptr(),
            pilasterbrace_source_written as i64,
            pilasterbrace_old.as_ptr(),
            pilasterbrace_old.len() as i64,
            pilasterbrace_new.as_ptr(),
            pilasterbrace_new.len() as i64,
            pilasterbrace_value.as_mut_ptr(),
            pilasterbrace_value.len() as i64,
        )
    };
    let pilasterbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            pilasterbrace_value.as_ptr(),
            pilasterbrace_value_written as i64,
            pilasterbrace_needle.as_ptr(),
            pilasterbrace_needle.len() as i64,
        )
    };

    let mut selected_eonitium_index = 0i32;
    let mut best_eonitium_score = i32::MIN;
    let mut eonitium_index = 0i32;
    while eonitium_index < 3 {
        let mut candidate_eonitium_score = eonitium_value_written * 10 + eonitium_contains * 50;
        if eonitium_index == 1 {
            candidate_eonitium_score = vanadiumwake_value_written * 10 + vanadiumwake_index;
        } else if eonitium_index == 2 {
            candidate_eonitium_score = pilasterbrace_value_written * 10 + pilasterbrace_contains * 50;
        }

        let mut eonitium_bonus = 0i32;
        if eonitium_index == selected_solvium_index {
            eonitium_bonus += 25;
        }
        if eonitium_index == selected_arcadium_index {
            eonitium_bonus += 15;
        }
        if eonitium_index == selected_virelium_index {
            eonitium_bonus += 5;
        }
        if eonitium_index == 0 && eonitium_contains != 0 {
            eonitium_bonus += 20;
        }
        if eonitium_index == 1 && vanadiumwake_index >= 0 {
            eonitium_bonus += 10;
        }
        if eonitium_index == 2 && pilasterbrace_contains != 0 {
            eonitium_bonus += 30;
        }

        let eonitium_score = candidate_eonitium_score + eonitium_bonus;
        if eonitium_score > best_eonitium_score {
            best_eonitium_score = eonitium_score;
            selected_eonitium_index = eonitium_index;
        }

        eonitium_index += 1;
    }

    let mut selected_eonitium_ptr = eonitium_value.as_ptr();
    let mut selected_eonitium_written = eonitium_value_written;
    if selected_eonitium_index == 1 {
        selected_eonitium_ptr = vanadiumwake_value.as_ptr();
        selected_eonitium_written = vanadiumwake_value_written;
    } else if selected_eonitium_index == 2 {
        selected_eonitium_ptr = pilasterbrace_value.as_ptr();
        selected_eonitium_written = pilasterbrace_value_written;
    }

    let selenitium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_eonitium_ptr,
            selected_eonitium_written as i64,
            selenitium_old.as_ptr(),
            selenitium_old.len() as i64,
            selenitium_new.as_ptr(),
            selenitium_new.len() as i64,
        )
    };
    let mut selenitium_value = vec![0u8; selenitium_value_len as usize];
    let selenitium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_eonitium_ptr,
            selected_eonitium_written as i64,
            selenitium_old.as_ptr(),
            selenitium_old.len() as i64,
            selenitium_new.as_ptr(),
            selenitium_new.len() as i64,
            selenitium_value.as_mut_ptr(),
            selenitium_value.len() as i64,
        )
    };
    let selenitium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            selenitium_value.as_ptr(),
            selenitium_value_written as i64,
            selenitium_needle.as_ptr(),
            selenitium_needle.len() as i64,
        )
    };

    let ceriumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_eonitium_ptr,
            selected_eonitium_written as i64,
            ceriumwake_extension.as_ptr(),
            ceriumwake_extension.len() as i64,
        )
    };
    let mut ceriumwake_value = vec![0u8; ceriumwake_value_len as usize];
    let ceriumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_eonitium_ptr,
            selected_eonitium_written as i64,
            ceriumwake_extension.as_ptr(),
            ceriumwake_extension.len() as i64,
            ceriumwake_value.as_mut_ptr(),
            ceriumwake_value.len() as i64,
        )
    };
    let ceriumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            ceriumwake_value.as_ptr(),
            ceriumwake_value_written as i64,
            ceriumwake_needle.as_ptr(),
            ceriumwake_needle.len() as i64,
        )
    };

    let haunchbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_eonitium_ptr,
            selected_eonitium_written as i64,
        )
    };
    let mut haunchbrace_source = vec![0u8; haunchbrace_source_len as usize];
    let haunchbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_eonitium_ptr,
            selected_eonitium_written as i64,
            haunchbrace_source.as_mut_ptr(),
            haunchbrace_source.len() as i64,
        )
    };
    let haunchbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            haunchbrace_source.as_ptr(),
            haunchbrace_source_written as i64,
            haunchbrace_old.as_ptr(),
            haunchbrace_old.len() as i64,
            haunchbrace_new.as_ptr(),
            haunchbrace_new.len() as i64,
        )
    };
    let mut haunchbrace_value = vec![0u8; haunchbrace_value_len as usize];
    let haunchbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            haunchbrace_source.as_ptr(),
            haunchbrace_source_written as i64,
            haunchbrace_old.as_ptr(),
            haunchbrace_old.len() as i64,
            haunchbrace_new.as_ptr(),
            haunchbrace_new.len() as i64,
            haunchbrace_value.as_mut_ptr(),
            haunchbrace_value.len() as i64,
        )
    };
    let haunchbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            haunchbrace_value.as_ptr(),
            haunchbrace_value_written as i64,
            haunchbrace_needle.as_ptr(),
            haunchbrace_needle.len() as i64,
        )
    };

    let mut selected_selenitium_index = 0i32;
    let mut best_selenitium_score = i32::MIN;
    let mut selenitium_index = 0i32;
    while selenitium_index < 3 {
        let mut candidate_selenitium_score = selenitium_value_written * 10 + selenitium_contains * 50;
        if selenitium_index == 1 {
            candidate_selenitium_score = ceriumwake_value_written * 10 + ceriumwake_index;
        } else if selenitium_index == 2 {
            candidate_selenitium_score = haunchbrace_value_written * 10 + haunchbrace_contains * 50;
        }

        let mut selenitium_bonus = 0i32;
        if selenitium_index == selected_eonitium_index {
            selenitium_bonus += 25;
        }
        if selenitium_index == selected_solvium_index {
            selenitium_bonus += 15;
        }
        if selenitium_index == selected_arcadium_index {
            selenitium_bonus += 5;
        }
        if selenitium_index == 0 && selenitium_contains != 0 {
            selenitium_bonus += 20;
        }
        if selenitium_index == 1 && ceriumwake_index >= 0 {
            selenitium_bonus += 10;
        }
        if selenitium_index == 2 && haunchbrace_contains != 0 {
            selenitium_bonus += 30;
        }

        let selenitium_score = candidate_selenitium_score + selenitium_bonus;
        if selenitium_score > best_selenitium_score {
            best_selenitium_score = selenitium_score;
            selected_selenitium_index = selenitium_index;
        }

        selenitium_index += 1;
    }

    let mut selected_selenitium_ptr = selenitium_value.as_ptr();
    let mut selected_selenitium_written = selenitium_value_written;
    if selected_selenitium_index == 1 {
        selected_selenitium_ptr = ceriumwake_value.as_ptr();
        selected_selenitium_written = ceriumwake_value_written;
    } else if selected_selenitium_index == 2 {
        selected_selenitium_ptr = haunchbrace_value.as_ptr();
        selected_selenitium_written = haunchbrace_value_written;
    }

    let orielium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_selenitium_ptr,
            selected_selenitium_written as i64,
            orielium_old.as_ptr(),
            orielium_old.len() as i64,
            orielium_new.as_ptr(),
            orielium_new.len() as i64,
        )
    };
    let mut orielium_value = vec![0u8; orielium_value_len as usize];
    let orielium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_selenitium_ptr,
            selected_selenitium_written as i64,
            orielium_old.as_ptr(),
            orielium_old.len() as i64,
            orielium_new.as_ptr(),
            orielium_new.len() as i64,
            orielium_value.as_mut_ptr(),
            orielium_value.len() as i64,
        )
    };
    let orielium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            orielium_value.as_ptr(),
            orielium_value_written as i64,
            orielium_needle.as_ptr(),
            orielium_needle.len() as i64,
        )
    };

    let yttriumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_selenitium_ptr,
            selected_selenitium_written as i64,
            yttriumwake_extension.as_ptr(),
            yttriumwake_extension.len() as i64,
        )
    };
    let mut yttriumwake_value = vec![0u8; yttriumwake_value_len as usize];
    let yttriumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_selenitium_ptr,
            selected_selenitium_written as i64,
            yttriumwake_extension.as_ptr(),
            yttriumwake_extension.len() as i64,
            yttriumwake_value.as_mut_ptr(),
            yttriumwake_value.len() as i64,
        )
    };
    let yttriumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            yttriumwake_value.as_ptr(),
            yttriumwake_value_written as i64,
            yttriumwake_needle.as_ptr(),
            yttriumwake_needle.len() as i64,
        )
    };

    let transombrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_selenitium_ptr,
            selected_selenitium_written as i64,
        )
    };
    let mut transombrace_source = vec![0u8; transombrace_source_len as usize];
    let transombrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_selenitium_ptr,
            selected_selenitium_written as i64,
            transombrace_source.as_mut_ptr(),
            transombrace_source.len() as i64,
        )
    };
    let transombrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            transombrace_source.as_ptr(),
            transombrace_source_written as i64,
            transombrace_old.as_ptr(),
            transombrace_old.len() as i64,
            transombrace_new.as_ptr(),
            transombrace_new.len() as i64,
        )
    };
    let mut transombrace_value = vec![0u8; transombrace_value_len as usize];
    let transombrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            transombrace_source.as_ptr(),
            transombrace_source_written as i64,
            transombrace_old.as_ptr(),
            transombrace_old.len() as i64,
            transombrace_new.as_ptr(),
            transombrace_new.len() as i64,
            transombrace_value.as_mut_ptr(),
            transombrace_value.len() as i64,
        )
    };
    let transombrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            transombrace_value.as_ptr(),
            transombrace_value_written as i64,
            transombrace_needle.as_ptr(),
            transombrace_needle.len() as i64,
        )
    };

    let mut selected_orielium_index = 0i32;
    let mut best_orielium_score = i32::MIN;
    let mut orielium_index = 0i32;
    while orielium_index < 3 {
        let mut candidate_orielium_score = orielium_value_written * 10 + orielium_contains * 50;
        if orielium_index == 1 {
            candidate_orielium_score = yttriumwake_value_written * 10 + yttriumwake_index;
        } else if orielium_index == 2 {
            candidate_orielium_score = transombrace_value_written * 10 + transombrace_contains * 50;
        }

        let mut orielium_bonus = 0i32;
        if orielium_index == selected_selenitium_index {
            orielium_bonus += 25;
        }
        if orielium_index == selected_eonitium_index {
            orielium_bonus += 15;
        }
        if orielium_index == selected_solvium_index {
            orielium_bonus += 5;
        }
        if orielium_index == 0 && orielium_contains != 0 {
            orielium_bonus += 20;
        }
        if orielium_index == 1 && yttriumwake_index >= 0 {
            orielium_bonus += 10;
        }
        if orielium_index == 2 && transombrace_contains != 0 {
            orielium_bonus += 30;
        }

        let orielium_score = candidate_orielium_score + orielium_bonus;
        if orielium_score > best_orielium_score {
            best_orielium_score = orielium_score;
            selected_orielium_index = orielium_index;
        }

        orielium_index += 1;
    }

    let mut selected_orielium_ptr = orielium_value.as_ptr();
    let mut selected_orielium_written = orielium_value_written;
    if selected_orielium_index == 1 {
        selected_orielium_ptr = yttriumwake_value.as_ptr();
        selected_orielium_written = yttriumwake_value_written;
    } else if selected_orielium_index == 2 {
        selected_orielium_ptr = transombrace_value.as_ptr();
        selected_orielium_written = transombrace_value_written;
    }

    let umbratium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_orielium_ptr,
            selected_orielium_written as i64,
            umbratium_old.as_ptr(),
            umbratium_old.len() as i64,
            umbratium_new.as_ptr(),
            umbratium_new.len() as i64,
        )
    };
    let mut umbratium_value = vec![0u8; umbratium_value_len as usize];
    let umbratium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_orielium_ptr,
            selected_orielium_written as i64,
            umbratium_old.as_ptr(),
            umbratium_old.len() as i64,
            umbratium_new.as_ptr(),
            umbratium_new.len() as i64,
            umbratium_value.as_mut_ptr(),
            umbratium_value.len() as i64,
        )
    };
    let umbratium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            umbratium_value.as_ptr(),
            umbratium_value_written as i64,
            umbratium_needle.as_ptr(),
            umbratium_needle.len() as i64,
        )
    };

    let zirconiumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_orielium_ptr,
            selected_orielium_written as i64,
            zirconiumwake_extension.as_ptr(),
            zirconiumwake_extension.len() as i64,
        )
    };
    let mut zirconiumwake_value = vec![0u8; zirconiumwake_value_len as usize];
    let zirconiumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_orielium_ptr,
            selected_orielium_written as i64,
            zirconiumwake_extension.as_ptr(),
            zirconiumwake_extension.len() as i64,
            zirconiumwake_value.as_mut_ptr(),
            zirconiumwake_value.len() as i64,
        )
    };
    let zirconiumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            zirconiumwake_value.as_ptr(),
            zirconiumwake_value_written as i64,
            zirconiumwake_needle.as_ptr(),
            zirconiumwake_needle.len() as i64,
        )
    };

    let mullionbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_orielium_ptr,
            selected_orielium_written as i64,
        )
    };
    let mut mullionbrace_source = vec![0u8; mullionbrace_source_len as usize];
    let mullionbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_orielium_ptr,
            selected_orielium_written as i64,
            mullionbrace_source.as_mut_ptr(),
            mullionbrace_source.len() as i64,
        )
    };
    let mullionbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            mullionbrace_source.as_ptr(),
            mullionbrace_source_written as i64,
            mullionbrace_old.as_ptr(),
            mullionbrace_old.len() as i64,
            mullionbrace_new.as_ptr(),
            mullionbrace_new.len() as i64,
        )
    };
    let mut mullionbrace_value = vec![0u8; mullionbrace_value_len as usize];
    let mullionbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            mullionbrace_source.as_ptr(),
            mullionbrace_source_written as i64,
            mullionbrace_old.as_ptr(),
            mullionbrace_old.len() as i64,
            mullionbrace_new.as_ptr(),
            mullionbrace_new.len() as i64,
            mullionbrace_value.as_mut_ptr(),
            mullionbrace_value.len() as i64,
        )
    };
    let mullionbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            mullionbrace_value.as_ptr(),
            mullionbrace_value_written as i64,
            mullionbrace_needle.as_ptr(),
            mullionbrace_needle.len() as i64,
        )
    };

    let mut selected_umbratium_index = 0i32;
    let mut best_umbratium_score = i32::MIN;
    let mut umbratium_index = 0i32;
    while umbratium_index < 3 {
        let mut candidate_umbratium_score = umbratium_value_written * 10 + umbratium_contains * 50;
        if umbratium_index == 1 {
            candidate_umbratium_score = zirconiumwake_value_written * 10 + zirconiumwake_index;
        } else if umbratium_index == 2 {
            candidate_umbratium_score = mullionbrace_value_written * 10 + mullionbrace_contains * 50;
        }

        let mut umbratium_bonus = 0i32;
        if umbratium_index == selected_orielium_index {
            umbratium_bonus += 25;
        }
        if umbratium_index == selected_selenitium_index {
            umbratium_bonus += 15;
        }
        if umbratium_index == selected_eonitium_index {
            umbratium_bonus += 5;
        }
        if umbratium_index == 0 && umbratium_contains != 0 {
            umbratium_bonus += 20;
        }
        if umbratium_index == 1 && zirconiumwake_index >= 0 {
            umbratium_bonus += 10;
        }
        if umbratium_index == 2 && mullionbrace_contains != 0 {
            umbratium_bonus += 30;
        }

        let umbratium_score = candidate_umbratium_score + umbratium_bonus;
        if umbratium_score > best_umbratium_score {
            best_umbratium_score = umbratium_score;
            selected_umbratium_index = umbratium_index;
        }

        umbratium_index += 1;
    }

    let mut selected_umbratium_ptr = umbratium_value.as_ptr();
    let mut selected_umbratium_written = umbratium_value_written;
    if selected_umbratium_index == 1 {
        selected_umbratium_ptr = zirconiumwake_value.as_ptr();
        selected_umbratium_written = zirconiumwake_value_written;
    } else if selected_umbratium_index == 2 {
        selected_umbratium_ptr = mullionbrace_value.as_ptr();
        selected_umbratium_written = mullionbrace_value_written;
    }

    let lumenium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_umbratium_ptr,
            selected_umbratium_written as i64,
            lumenium_old.as_ptr(),
            lumenium_old.len() as i64,
            lumenium_new.as_ptr(),
            lumenium_new.len() as i64,
        )
    };
    let mut lumenium_value = vec![0u8; lumenium_value_len as usize];
    let lumenium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_umbratium_ptr,
            selected_umbratium_written as i64,
            lumenium_old.as_ptr(),
            lumenium_old.len() as i64,
            lumenium_new.as_ptr(),
            lumenium_new.len() as i64,
            lumenium_value.as_mut_ptr(),
            lumenium_value.len() as i64,
        )
    };
    let lumenium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            lumenium_value.as_ptr(),
            lumenium_value_written as i64,
            lumenium_needle.as_ptr(),
            lumenium_needle.len() as i64,
        )
    };

    let hafniumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_umbratium_ptr,
            selected_umbratium_written as i64,
            hafniumwake_extension.as_ptr(),
            hafniumwake_extension.len() as i64,
        )
    };
    let mut hafniumwake_value = vec![0u8; hafniumwake_value_len as usize];
    let hafniumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_umbratium_ptr,
            selected_umbratium_written as i64,
            hafniumwake_extension.as_ptr(),
            hafniumwake_extension.len() as i64,
            hafniumwake_value.as_mut_ptr(),
            hafniumwake_value.len() as i64,
        )
    };
    let hafniumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            hafniumwake_value.as_ptr(),
            hafniumwake_value_written as i64,
            hafniumwake_needle.as_ptr(),
            hafniumwake_needle.len() as i64,
        )
    };

    let clerestorybrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_umbratium_ptr,
            selected_umbratium_written as i64,
        )
    };
    let mut clerestorybrace_source = vec![0u8; clerestorybrace_source_len as usize];
    let clerestorybrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_umbratium_ptr,
            selected_umbratium_written as i64,
            clerestorybrace_source.as_mut_ptr(),
            clerestorybrace_source.len() as i64,
        )
    };
    let clerestorybrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            clerestorybrace_source.as_ptr(),
            clerestorybrace_source_written as i64,
            clerestorybrace_old.as_ptr(),
            clerestorybrace_old.len() as i64,
            clerestorybrace_new.as_ptr(),
            clerestorybrace_new.len() as i64,
        )
    };
    let mut clerestorybrace_value = vec![0u8; clerestorybrace_value_len as usize];
    let clerestorybrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            clerestorybrace_source.as_ptr(),
            clerestorybrace_source_written as i64,
            clerestorybrace_old.as_ptr(),
            clerestorybrace_old.len() as i64,
            clerestorybrace_new.as_ptr(),
            clerestorybrace_new.len() as i64,
            clerestorybrace_value.as_mut_ptr(),
            clerestorybrace_value.len() as i64,
        )
    };
    let clerestorybrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            clerestorybrace_value.as_ptr(),
            clerestorybrace_value_written as i64,
            clerestorybrace_needle.as_ptr(),
            clerestorybrace_needle.len() as i64,
        )
    };

    let mut selected_lumenium_index = 0i32;
    let mut best_lumenium_score = i32::MIN;
    let mut lumenium_index = 0i32;
    while lumenium_index < 3 {
        let mut candidate_lumenium_score = lumenium_value_written * 10 + lumenium_contains * 50;
        if lumenium_index == 1 {
            candidate_lumenium_score = hafniumwake_value_written * 10 + hafniumwake_index;
        } else if lumenium_index == 2 {
            candidate_lumenium_score = clerestorybrace_value_written * 10 + clerestorybrace_contains * 50;
        }

        let mut lumenium_bonus = 0i32;
        if lumenium_index == selected_umbratium_index {
            lumenium_bonus += 25;
        }
        if lumenium_index == selected_orielium_index {
            lumenium_bonus += 15;
        }
        if lumenium_index == selected_selenitium_index {
            lumenium_bonus += 5;
        }
        if lumenium_index == 0 && lumenium_contains != 0 {
            lumenium_bonus += 20;
        }
        if lumenium_index == 1 && hafniumwake_index >= 0 {
            lumenium_bonus += 10;
        }
        if lumenium_index == 2 && clerestorybrace_contains != 0 {
            lumenium_bonus += 30;
        }

        let lumenium_score = candidate_lumenium_score + lumenium_bonus;
        if lumenium_score > best_lumenium_score {
            best_lumenium_score = lumenium_score;
            selected_lumenium_index = lumenium_index;
        }

        lumenium_index += 1;
    }

    let mut selected_lumenium_ptr = lumenium_value.as_ptr();
    let mut selected_lumenium_written = lumenium_value_written;
    if selected_lumenium_index == 1 {
        selected_lumenium_ptr = hafniumwake_value.as_ptr();
        selected_lumenium_written = hafniumwake_value_written;
    } else if selected_lumenium_index == 2 {
        selected_lumenium_ptr = clerestorybrace_value.as_ptr();
        selected_lumenium_written = clerestorybrace_value_written;
    }

    let stratium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_lumenium_ptr,
            selected_lumenium_written as i64,
            stratium_old.as_ptr(),
            stratium_old.len() as i64,
            stratium_new.as_ptr(),
            stratium_new.len() as i64,
        )
    };
    let mut stratium_value = vec![0u8; stratium_value_len as usize];
    let stratium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_lumenium_ptr,
            selected_lumenium_written as i64,
            stratium_old.as_ptr(),
            stratium_old.len() as i64,
            stratium_new.as_ptr(),
            stratium_new.len() as i64,
            stratium_value.as_mut_ptr(),
            stratium_value.len() as i64,
        )
    };
    let stratium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            stratium_value.as_ptr(),
            stratium_value_written as i64,
            stratium_needle.as_ptr(),
            stratium_needle.len() as i64,
        )
    };

    let tantalumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_lumenium_ptr,
            selected_lumenium_written as i64,
            tantalumwake_extension.as_ptr(),
            tantalumwake_extension.len() as i64,
        )
    };
    let mut tantalumwake_value = vec![0u8; tantalumwake_value_len as usize];
    let tantalumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_lumenium_ptr,
            selected_lumenium_written as i64,
            tantalumwake_extension.as_ptr(),
            tantalumwake_extension.len() as i64,
            tantalumwake_value.as_mut_ptr(),
            tantalumwake_value.len() as i64,
        )
    };
    let tantalumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            tantalumwake_value.as_ptr(),
            tantalumwake_value_written as i64,
            tantalumwake_needle.as_ptr(),
            tantalumwake_needle.len() as i64,
        )
    };

    let fanlightbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_lumenium_ptr,
            selected_lumenium_written as i64,
        )
    };
    let mut fanlightbrace_source = vec![0u8; fanlightbrace_source_len as usize];
    let fanlightbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_lumenium_ptr,
            selected_lumenium_written as i64,
            fanlightbrace_source.as_mut_ptr(),
            fanlightbrace_source.len() as i64,
        )
    };
    let fanlightbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            fanlightbrace_source.as_ptr(),
            fanlightbrace_source_written as i64,
            fanlightbrace_old.as_ptr(),
            fanlightbrace_old.len() as i64,
            fanlightbrace_new.as_ptr(),
            fanlightbrace_new.len() as i64,
        )
    };
    let mut fanlightbrace_value = vec![0u8; fanlightbrace_value_len as usize];
    let fanlightbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            fanlightbrace_source.as_ptr(),
            fanlightbrace_source_written as i64,
            fanlightbrace_old.as_ptr(),
            fanlightbrace_old.len() as i64,
            fanlightbrace_new.as_ptr(),
            fanlightbrace_new.len() as i64,
            fanlightbrace_value.as_mut_ptr(),
            fanlightbrace_value.len() as i64,
        )
    };
    let fanlightbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            fanlightbrace_value.as_ptr(),
            fanlightbrace_value_written as i64,
            fanlightbrace_needle.as_ptr(),
            fanlightbrace_needle.len() as i64,
        )
    };

    let mut selected_stratium_index = 0i32;
    let mut best_stratium_score = i32::MIN;
    let mut stratium_index = 0i32;
    while stratium_index < 3 {
        let mut candidate_stratium_score = stratium_value_written * 10 + stratium_contains * 50;
        if stratium_index == 1 {
            candidate_stratium_score = tantalumwake_value_written * 10 + tantalumwake_index;
        } else if stratium_index == 2 {
            candidate_stratium_score = fanlightbrace_value_written * 10 + fanlightbrace_contains * 50;
        }

        let mut stratium_bonus = 0i32;
        if stratium_index == selected_lumenium_index {
            stratium_bonus += 25;
        }
        if stratium_index == selected_umbratium_index {
            stratium_bonus += 15;
        }
        if stratium_index == selected_orielium_index {
            stratium_bonus += 5;
        }
        if stratium_index == 0 && stratium_contains != 0 {
            stratium_bonus += 20;
        }
        if stratium_index == 1 && tantalumwake_index >= 0 {
            stratium_bonus += 10;
        }
        if stratium_index == 2 && fanlightbrace_contains != 0 {
            stratium_bonus += 30;
        }

        let stratium_score = candidate_stratium_score + stratium_bonus;
        if stratium_score > best_stratium_score {
            best_stratium_score = stratium_score;
            selected_stratium_index = stratium_index;
        }

        stratium_index += 1;
    }

    let mut selected_stratium_ptr = stratium_value.as_ptr();
    let mut selected_stratium_written = stratium_value_written;
    if selected_stratium_index == 1 {
        selected_stratium_ptr = tantalumwake_value.as_ptr();
        selected_stratium_written = tantalumwake_value_written;
    } else if selected_stratium_index == 2 {
        selected_stratium_ptr = fanlightbrace_value.as_ptr();
        selected_stratium_written = fanlightbrace_value_written;
    }

    let venturium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_stratium_ptr,
            selected_stratium_written as i64,
            venturium_old.as_ptr(),
            venturium_old.len() as i64,
            venturium_new.as_ptr(),
            venturium_new.len() as i64,
        )
    };
    let mut venturium_value = vec![0u8; venturium_value_len as usize];
    let venturium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_stratium_ptr,
            selected_stratium_written as i64,
            venturium_old.as_ptr(),
            venturium_old.len() as i64,
            venturium_new.as_ptr(),
            venturium_new.len() as i64,
            venturium_value.as_mut_ptr(),
            venturium_value.len() as i64,
        )
    };
    let venturium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            venturium_value.as_ptr(),
            venturium_value_written as i64,
            venturium_needle.as_ptr(),
            venturium_needle.len() as i64,
        )
    };

    let niobiumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_stratium_ptr,
            selected_stratium_written as i64,
            niobiumwake_extension.as_ptr(),
            niobiumwake_extension.len() as i64,
        )
    };
    let mut niobiumwake_value = vec![0u8; niobiumwake_value_len as usize];
    let niobiumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_stratium_ptr,
            selected_stratium_written as i64,
            niobiumwake_extension.as_ptr(),
            niobiumwake_extension.len() as i64,
            niobiumwake_value.as_mut_ptr(),
            niobiumwake_value.len() as i64,
        )
    };
    let niobiumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            niobiumwake_value.as_ptr(),
            niobiumwake_value_written as i64,
            niobiumwake_needle.as_ptr(),
            niobiumwake_needle.len() as i64,
        )
    };

    let oculusbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_stratium_ptr,
            selected_stratium_written as i64,
        )
    };
    let mut oculusbrace_source = vec![0u8; oculusbrace_source_len as usize];
    let oculusbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_stratium_ptr,
            selected_stratium_written as i64,
            oculusbrace_source.as_mut_ptr(),
            oculusbrace_source.len() as i64,
        )
    };
    let oculusbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            oculusbrace_source.as_ptr(),
            oculusbrace_source_written as i64,
            oculusbrace_old.as_ptr(),
            oculusbrace_old.len() as i64,
            oculusbrace_new.as_ptr(),
            oculusbrace_new.len() as i64,
        )
    };
    let mut oculusbrace_value = vec![0u8; oculusbrace_value_len as usize];
    let oculusbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            oculusbrace_source.as_ptr(),
            oculusbrace_source_written as i64,
            oculusbrace_old.as_ptr(),
            oculusbrace_old.len() as i64,
            oculusbrace_new.as_ptr(),
            oculusbrace_new.len() as i64,
            oculusbrace_value.as_mut_ptr(),
            oculusbrace_value.len() as i64,
        )
    };
    let oculusbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            oculusbrace_value.as_ptr(),
            oculusbrace_value_written as i64,
            oculusbrace_needle.as_ptr(),
            oculusbrace_needle.len() as i64,
        )
    };

    let mut selected_venturium_index = 0i32;
    let mut best_venturium_score = i32::MIN;
    let mut venturium_index = 0i32;
    while venturium_index < 3 {
        let mut candidate_venturium_score = venturium_value_written * 10 + venturium_contains * 50;
        if venturium_index == 1 {
            candidate_venturium_score = niobiumwake_value_written * 10 + niobiumwake_index;
        } else if venturium_index == 2 {
            candidate_venturium_score = oculusbrace_value_written * 10 + oculusbrace_contains * 50;
        }

        let mut venturium_bonus = 0i32;
        if venturium_index == selected_stratium_index {
            venturium_bonus += 25;
        }
        if venturium_index == selected_lumenium_index {
            venturium_bonus += 15;
        }
        if venturium_index == selected_umbratium_index {
            venturium_bonus += 5;
        }
        if venturium_index == 0 && venturium_contains != 0 {
            venturium_bonus += 20;
        }
        if venturium_index == 1 && niobiumwake_index >= 0 {
            venturium_bonus += 10;
        }
        if venturium_index == 2 && oculusbrace_contains != 0 {
            venturium_bonus += 30;
        }

        let venturium_score = candidate_venturium_score + venturium_bonus;
        if venturium_score > best_venturium_score {
            best_venturium_score = venturium_score;
            selected_venturium_index = venturium_index;
        }

        venturium_index += 1;
    }

    let mut selected_venturium_ptr = venturium_value.as_ptr();
    let mut selected_venturium_written = venturium_value_written;
    if selected_venturium_index == 1 {
        selected_venturium_ptr = niobiumwake_value.as_ptr();
        selected_venturium_written = niobiumwake_value_written;
    } else if selected_venturium_index == 2 {
        selected_venturium_ptr = oculusbrace_value.as_ptr();
        selected_venturium_written = oculusbrace_value_written;
    }

    let meridianium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_venturium_ptr,
            selected_venturium_written as i64,
            meridianium_old.as_ptr(),
            meridianium_old.len() as i64,
            meridianium_new.as_ptr(),
            meridianium_new.len() as i64,
        )
    };
    let mut meridianium_value = vec![0u8; meridianium_value_len as usize];
    let meridianium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_venturium_ptr,
            selected_venturium_written as i64,
            meridianium_old.as_ptr(),
            meridianium_old.len() as i64,
            meridianium_new.as_ptr(),
            meridianium_new.len() as i64,
            meridianium_value.as_mut_ptr(),
            meridianium_value.len() as i64,
        )
    };
    let meridianium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            meridianium_value.as_ptr(),
            meridianium_value_written as i64,
            meridianium_needle.as_ptr(),
            meridianium_needle.len() as i64,
        )
    };

    let xenonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_venturium_ptr,
            selected_venturium_written as i64,
            xenonwake_extension.as_ptr(),
            xenonwake_extension.len() as i64,
        )
    };
    let mut xenonwake_value = vec![0u8; xenonwake_value_len as usize];
    let xenonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_venturium_ptr,
            selected_venturium_written as i64,
            xenonwake_extension.as_ptr(),
            xenonwake_extension.len() as i64,
            xenonwake_value.as_mut_ptr(),
            xenonwake_value.len() as i64,
        )
    };
    let xenonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            xenonwake_value.as_ptr(),
            xenonwake_value_written as i64,
            xenonwake_needle.as_ptr(),
            xenonwake_needle.len() as i64,
        )
    };

    let lancetbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_venturium_ptr,
            selected_venturium_written as i64,
        )
    };
    let mut lancetbrace_source = vec![0u8; lancetbrace_source_len as usize];
    let lancetbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_venturium_ptr,
            selected_venturium_written as i64,
            lancetbrace_source.as_mut_ptr(),
            lancetbrace_source.len() as i64,
        )
    };
    let lancetbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            lancetbrace_source.as_ptr(),
            lancetbrace_source_written as i64,
            lancetbrace_old.as_ptr(),
            lancetbrace_old.len() as i64,
            lancetbrace_new.as_ptr(),
            lancetbrace_new.len() as i64,
        )
    };
    let mut lancetbrace_value = vec![0u8; lancetbrace_value_len as usize];
    let lancetbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            lancetbrace_source.as_ptr(),
            lancetbrace_source_written as i64,
            lancetbrace_old.as_ptr(),
            lancetbrace_old.len() as i64,
            lancetbrace_new.as_ptr(),
            lancetbrace_new.len() as i64,
            lancetbrace_value.as_mut_ptr(),
            lancetbrace_value.len() as i64,
        )
    };
    let lancetbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            lancetbrace_value.as_ptr(),
            lancetbrace_value_written as i64,
            lancetbrace_needle.as_ptr(),
            lancetbrace_needle.len() as i64,
        )
    };

    let mut selected_meridianium_index = 0i32;
    let mut best_meridianium_score = i32::MIN;
    let mut meridianium_index = 0i32;
    while meridianium_index < 3 {
        let mut candidate_meridianium_score = meridianium_value_written * 10 + meridianium_contains * 50;
        if meridianium_index == 1 {
            candidate_meridianium_score = xenonwake_value_written * 10 + xenonwake_index;
        } else if meridianium_index == 2 {
            candidate_meridianium_score = lancetbrace_value_written * 10 + lancetbrace_contains * 50;
        }

        let mut meridianium_bonus = 0i32;
        if meridianium_index == selected_venturium_index {
            meridianium_bonus += 25;
        }
        if meridianium_index == selected_stratium_index {
            meridianium_bonus += 15;
        }
        if meridianium_index == selected_lumenium_index {
            meridianium_bonus += 5;
        }
        if meridianium_index == 0 && meridianium_contains != 0 {
            meridianium_bonus += 20;
        }
        if meridianium_index == 1 && xenonwake_index >= 0 {
            meridianium_bonus += 10;
        }
        if meridianium_index == 2 && lancetbrace_contains != 0 {
            meridianium_bonus += 30;
        }

        let meridianium_score = candidate_meridianium_score + meridianium_bonus;
        if meridianium_score > best_meridianium_score {
            best_meridianium_score = meridianium_score;
            selected_meridianium_index = meridianium_index;
        }

        meridianium_index += 1;
    }

    let mut selected_meridianium_ptr = meridianium_value.as_ptr();
    let mut selected_meridianium_written = meridianium_value_written;
    if selected_meridianium_index == 1 {
        selected_meridianium_ptr = xenonwake_value.as_ptr();
        selected_meridianium_written = xenonwake_value_written;
    } else if selected_meridianium_index == 2 {
        selected_meridianium_ptr = lancetbrace_value.as_ptr();
        selected_meridianium_written = lancetbrace_value_written;
    }

    let perihelium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_meridianium_ptr,
            selected_meridianium_written as i64,
            perihelium_old.as_ptr(),
            perihelium_old.len() as i64,
            perihelium_new.as_ptr(),
            perihelium_new.len() as i64,
        )
    };
    let mut perihelium_value = vec![0u8; perihelium_value_len as usize];
    let perihelium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_meridianium_ptr,
            selected_meridianium_written as i64,
            perihelium_old.as_ptr(),
            perihelium_old.len() as i64,
            perihelium_new.as_ptr(),
            perihelium_new.len() as i64,
            perihelium_value.as_mut_ptr(),
            perihelium_value.len() as i64,
        )
    };
    let perihelium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            perihelium_value.as_ptr(),
            perihelium_value_written as i64,
            perihelium_needle.as_ptr(),
            perihelium_needle.len() as i64,
        )
    };

    let argonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_meridianium_ptr,
            selected_meridianium_written as i64,
            argonwake_extension.as_ptr(),
            argonwake_extension.len() as i64,
        )
    };
    let mut argonwake_value = vec![0u8; argonwake_value_len as usize];
    let argonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_meridianium_ptr,
            selected_meridianium_written as i64,
            argonwake_extension.as_ptr(),
            argonwake_extension.len() as i64,
            argonwake_value.as_mut_ptr(),
            argonwake_value.len() as i64,
        )
    };
    let argonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            argonwake_value.as_ptr(),
            argonwake_value_written as i64,
            argonwake_needle.as_ptr(),
            argonwake_needle.len() as i64,
        )
    };

    let voussoirbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_meridianium_ptr,
            selected_meridianium_written as i64,
        )
    };
    let mut voussoirbrace_source = vec![0u8; voussoirbrace_source_len as usize];
    let voussoirbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_meridianium_ptr,
            selected_meridianium_written as i64,
            voussoirbrace_source.as_mut_ptr(),
            voussoirbrace_source.len() as i64,
        )
    };
    let voussoirbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            voussoirbrace_source.as_ptr(),
            voussoirbrace_source_written as i64,
            voussoirbrace_old.as_ptr(),
            voussoirbrace_old.len() as i64,
            voussoirbrace_new.as_ptr(),
            voussoirbrace_new.len() as i64,
        )
    };
    let mut voussoirbrace_value = vec![0u8; voussoirbrace_value_len as usize];
    let voussoirbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            voussoirbrace_source.as_ptr(),
            voussoirbrace_source_written as i64,
            voussoirbrace_old.as_ptr(),
            voussoirbrace_old.len() as i64,
            voussoirbrace_new.as_ptr(),
            voussoirbrace_new.len() as i64,
            voussoirbrace_value.as_mut_ptr(),
            voussoirbrace_value.len() as i64,
        )
    };
    let voussoirbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            voussoirbrace_value.as_ptr(),
            voussoirbrace_value_written as i64,
            voussoirbrace_needle.as_ptr(),
            voussoirbrace_needle.len() as i64,
        )
    };

    let mut selected_perihelium_index = 0i32;
    let mut best_perihelium_score = i32::MIN;
    let mut perihelium_index = 0i32;
    while perihelium_index < 3 {
        let mut candidate_perihelium_score = perihelium_value_written * 10 + perihelium_contains * 50;
        if perihelium_index == 1 {
            candidate_perihelium_score = argonwake_value_written * 10 + argonwake_index;
        } else if perihelium_index == 2 {
            candidate_perihelium_score = voussoirbrace_value_written * 10 + voussoirbrace_contains * 50;
        }

        let mut perihelium_bonus = 0i32;
        if perihelium_index == selected_meridianium_index {
            perihelium_bonus += 25;
        }
        if perihelium_index == selected_venturium_index {
            perihelium_bonus += 15;
        }
        if perihelium_index == selected_stratium_index {
            perihelium_bonus += 5;
        }
        if perihelium_index == 0 && perihelium_contains != 0 {
            perihelium_bonus += 20;
        }
        if perihelium_index == 1 && argonwake_index >= 0 {
            perihelium_bonus += 10;
        }
        if perihelium_index == 2 && voussoirbrace_contains != 0 {
            perihelium_bonus += 30;
        }

        let perihelium_score = candidate_perihelium_score + perihelium_bonus;
        if perihelium_score > best_perihelium_score {
            best_perihelium_score = perihelium_score;
            selected_perihelium_index = perihelium_index;
        }

        perihelium_index += 1;
    }

    let mut selected_perihelium_ptr = perihelium_value.as_ptr();
    let mut selected_perihelium_written = perihelium_value_written;
    if selected_perihelium_index == 1 {
        selected_perihelium_ptr = argonwake_value.as_ptr();
        selected_perihelium_written = argonwake_value_written;
    } else if selected_perihelium_index == 2 {
        selected_perihelium_ptr = voussoirbrace_value.as_ptr();
        selected_perihelium_written = voussoirbrace_value_written;
    }

    let equinoctium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_perihelium_ptr,
            selected_perihelium_written as i64,
            equinoctium_old.as_ptr(),
            equinoctium_old.len() as i64,
            equinoctium_new.as_ptr(),
            equinoctium_new.len() as i64,
        )
    };
    let mut equinoctium_value = vec![0u8; equinoctium_value_len as usize];
    let equinoctium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_perihelium_ptr,
            selected_perihelium_written as i64,
            equinoctium_old.as_ptr(),
            equinoctium_old.len() as i64,
            equinoctium_new.as_ptr(),
            equinoctium_new.len() as i64,
            equinoctium_value.as_mut_ptr(),
            equinoctium_value.len() as i64,
        )
    };
    let equinoctium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            equinoctium_value.as_ptr(),
            equinoctium_value_written as i64,
            equinoctium_needle.as_ptr(),
            equinoctium_needle.len() as i64,
        )
    };

    let neonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_perihelium_ptr,
            selected_perihelium_written as i64,
            neonwake_extension.as_ptr(),
            neonwake_extension.len() as i64,
        )
    };
    let mut neonwake_value = vec![0u8; neonwake_value_len as usize];
    let neonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_perihelium_ptr,
            selected_perihelium_written as i64,
            neonwake_extension.as_ptr(),
            neonwake_extension.len() as i64,
            neonwake_value.as_mut_ptr(),
            neonwake_value.len() as i64,
        )
    };
    let neonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            neonwake_value.as_ptr(),
            neonwake_value_written as i64,
            neonwake_needle.as_ptr(),
            neonwake_needle.len() as i64,
        )
    };

    let keystonebrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_perihelium_ptr,
            selected_perihelium_written as i64,
        )
    };
    let mut keystonebrace_source = vec![0u8; keystonebrace_source_len as usize];
    let keystonebrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_perihelium_ptr,
            selected_perihelium_written as i64,
            keystonebrace_source.as_mut_ptr(),
            keystonebrace_source.len() as i64,
        )
    };
    let keystonebrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            keystonebrace_source.as_ptr(),
            keystonebrace_source_written as i64,
            keystonebrace_old.as_ptr(),
            keystonebrace_old.len() as i64,
            keystonebrace_new.as_ptr(),
            keystonebrace_new.len() as i64,
        )
    };
    let mut keystonebrace_value = vec![0u8; keystonebrace_value_len as usize];
    let keystonebrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            keystonebrace_source.as_ptr(),
            keystonebrace_source_written as i64,
            keystonebrace_old.as_ptr(),
            keystonebrace_old.len() as i64,
            keystonebrace_new.as_ptr(),
            keystonebrace_new.len() as i64,
            keystonebrace_value.as_mut_ptr(),
            keystonebrace_value.len() as i64,
        )
    };
    let keystonebrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            keystonebrace_value.as_ptr(),
            keystonebrace_value_written as i64,
            keystonebrace_needle.as_ptr(),
            keystonebrace_needle.len() as i64,
        )
    };

    let mut selected_equinoctium_index = 0i32;
    let mut best_equinoctium_score = i32::MIN;
    let mut equinoctium_index = 0i32;
    while equinoctium_index < 3 {
        let mut candidate_equinoctium_score = equinoctium_value_written * 10 + equinoctium_contains * 50;
        if equinoctium_index == 1 {
            candidate_equinoctium_score = neonwake_value_written * 10 + neonwake_index;
        } else if equinoctium_index == 2 {
            candidate_equinoctium_score = keystonebrace_value_written * 10 + keystonebrace_contains * 50;
        }

        let mut equinoctium_bonus = 0i32;
        if equinoctium_index == selected_perihelium_index {
            equinoctium_bonus += 25;
        }
        if equinoctium_index == selected_meridianium_index {
            equinoctium_bonus += 15;
        }
        if equinoctium_index == selected_venturium_index {
            equinoctium_bonus += 5;
        }
        if equinoctium_index == 0 && equinoctium_contains != 0 {
            equinoctium_bonus += 20;
        }
        if equinoctium_index == 1 && neonwake_index >= 0 {
            equinoctium_bonus += 10;
        }
        if equinoctium_index == 2 && keystonebrace_contains != 0 {
            equinoctium_bonus += 30;
        }

        let equinoctium_score = candidate_equinoctium_score + equinoctium_bonus;
        if equinoctium_score > best_equinoctium_score {
            best_equinoctium_score = equinoctium_score;
            selected_equinoctium_index = equinoctium_index;
        }

        equinoctium_index += 1;
    }

    let mut selected_equinoctium_ptr = equinoctium_value.as_ptr();
    let mut selected_equinoctium_written = equinoctium_value_written;
    if selected_equinoctium_index == 1 {
        selected_equinoctium_ptr = neonwake_value.as_ptr();
        selected_equinoctium_written = neonwake_value_written;
    } else if selected_equinoctium_index == 2 {
        selected_equinoctium_ptr = keystonebrace_value.as_ptr();
        selected_equinoctium_written = keystonebrace_value_written;
    }

    let syzygyium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_equinoctium_ptr,
            selected_equinoctium_written as i64,
            syzygyium_old.as_ptr(),
            syzygyium_old.len() as i64,
            syzygyium_new.as_ptr(),
            syzygyium_new.len() as i64,
        )
    };
    let mut syzygyium_value = vec![0u8; syzygyium_value_len as usize];
    let syzygyium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_equinoctium_ptr,
            selected_equinoctium_written as i64,
            syzygyium_old.as_ptr(),
            syzygyium_old.len() as i64,
            syzygyium_new.as_ptr(),
            syzygyium_new.len() as i64,
            syzygyium_value.as_mut_ptr(),
            syzygyium_value.len() as i64,
        )
    };
    let syzygyium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            syzygyium_value.as_ptr(),
            syzygyium_value_written as i64,
            syzygyium_needle.as_ptr(),
            syzygyium_needle.len() as i64,
        )
    };

    let radonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_equinoctium_ptr,
            selected_equinoctium_written as i64,
            radonwake_extension.as_ptr(),
            radonwake_extension.len() as i64,
        )
    };
    let mut radonwake_value = vec![0u8; radonwake_value_len as usize];
    let radonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_equinoctium_ptr,
            selected_equinoctium_written as i64,
            radonwake_extension.as_ptr(),
            radonwake_extension.len() as i64,
            radonwake_value.as_mut_ptr(),
            radonwake_value.len() as i64,
        )
    };
    let radonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            radonwake_value.as_ptr(),
            radonwake_value_written as i64,
            radonwake_needle.as_ptr(),
            radonwake_needle.len() as i64,
        )
    };

    let spandrelbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_equinoctium_ptr,
            selected_equinoctium_written as i64,
        )
    };
    let mut spandrelbrace_source = vec![0u8; spandrelbrace_source_len as usize];
    let spandrelbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_equinoctium_ptr,
            selected_equinoctium_written as i64,
            spandrelbrace_source.as_mut_ptr(),
            spandrelbrace_source.len() as i64,
        )
    };
    let spandrelbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            spandrelbrace_source.as_ptr(),
            spandrelbrace_source_written as i64,
            spandrelbrace_old.as_ptr(),
            spandrelbrace_old.len() as i64,
            spandrelbrace_new.as_ptr(),
            spandrelbrace_new.len() as i64,
        )
    };
    let mut spandrelbrace_value = vec![0u8; spandrelbrace_value_len as usize];
    let spandrelbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            spandrelbrace_source.as_ptr(),
            spandrelbrace_source_written as i64,
            spandrelbrace_old.as_ptr(),
            spandrelbrace_old.len() as i64,
            spandrelbrace_new.as_ptr(),
            spandrelbrace_new.len() as i64,
            spandrelbrace_value.as_mut_ptr(),
            spandrelbrace_value.len() as i64,
        )
    };
    let spandrelbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            spandrelbrace_value.as_ptr(),
            spandrelbrace_value_written as i64,
            spandrelbrace_needle.as_ptr(),
            spandrelbrace_needle.len() as i64,
        )
    };

    let mut selected_syzygyium_index = 0i32;
    let mut best_syzygyium_score = i32::MIN;
    let mut syzygyium_index = 0i32;
    while syzygyium_index < 3 {
        let mut candidate_syzygyium_score = syzygyium_value_written * 10 + syzygyium_contains * 50;
        if syzygyium_index == 1 {
            candidate_syzygyium_score = radonwake_value_written * 10 + radonwake_index;
        } else if syzygyium_index == 2 {
            candidate_syzygyium_score = spandrelbrace_value_written * 10 + spandrelbrace_contains * 50;
        }

        let mut syzygyium_bonus = 0i32;
        if syzygyium_index == selected_equinoctium_index {
            syzygyium_bonus += 25;
        }
        if syzygyium_index == selected_perihelium_index {
            syzygyium_bonus += 15;
        }
        if syzygyium_index == selected_meridianium_index {
            syzygyium_bonus += 5;
        }
        if syzygyium_index == 0 && syzygyium_contains != 0 {
            syzygyium_bonus += 20;
        }
        if syzygyium_index == 1 && radonwake_index >= 0 {
            syzygyium_bonus += 10;
        }
        if syzygyium_index == 2 && spandrelbrace_contains != 0 {
            syzygyium_bonus += 30;
        }

        let syzygyium_score = candidate_syzygyium_score + syzygyium_bonus;
        if syzygyium_score > best_syzygyium_score {
            best_syzygyium_score = syzygyium_score;
            selected_syzygyium_index = syzygyium_index;
        }

        syzygyium_index += 1;
    }

    let mut selected_syzygyium_ptr = syzygyium_value.as_ptr();
    let mut selected_syzygyium_written = syzygyium_value_written;
    if selected_syzygyium_index == 1 {
        selected_syzygyium_ptr = radonwake_value.as_ptr();
        selected_syzygyium_written = radonwake_value_written;
    } else if selected_syzygyium_index == 2 {
        selected_syzygyium_ptr = spandrelbrace_value.as_ptr();
        selected_syzygyium_written = spandrelbrace_value_written;
    }

    let parallaxium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_syzygyium_ptr,
            selected_syzygyium_written as i64,
            parallaxium_old.as_ptr(),
            parallaxium_old.len() as i64,
            parallaxium_new.as_ptr(),
            parallaxium_new.len() as i64,
        )
    };
    let mut parallaxium_value = vec![0u8; parallaxium_value_len as usize];
    let parallaxium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_syzygyium_ptr,
            selected_syzygyium_written as i64,
            parallaxium_old.as_ptr(),
            parallaxium_old.len() as i64,
            parallaxium_new.as_ptr(),
            parallaxium_new.len() as i64,
            parallaxium_value.as_mut_ptr(),
            parallaxium_value.len() as i64,
        )
    };
    let parallaxium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            parallaxium_value.as_ptr(),
            parallaxium_value_written as i64,
            parallaxium_needle.as_ptr(),
            parallaxium_needle.len() as i64,
        )
    };

    let cesiumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_syzygyium_ptr,
            selected_syzygyium_written as i64,
            cesiumwake_extension.as_ptr(),
            cesiumwake_extension.len() as i64,
        )
    };
    let mut cesiumwake_value = vec![0u8; cesiumwake_value_len as usize];
    let cesiumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_syzygyium_ptr,
            selected_syzygyium_written as i64,
            cesiumwake_extension.as_ptr(),
            cesiumwake_extension.len() as i64,
            cesiumwake_value.as_mut_ptr(),
            cesiumwake_value.len() as i64,
        )
    };
    let cesiumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            cesiumwake_value.as_ptr(),
            cesiumwake_value_written as i64,
            cesiumwake_needle.as_ptr(),
            cesiumwake_needle.len() as i64,
        )
    };

    let tracerybrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_syzygyium_ptr,
            selected_syzygyium_written as i64,
        )
    };
    let mut tracerybrace_source = vec![0u8; tracerybrace_source_len as usize];
    let tracerybrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_syzygyium_ptr,
            selected_syzygyium_written as i64,
            tracerybrace_source.as_mut_ptr(),
            tracerybrace_source.len() as i64,
        )
    };
    let tracerybrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            tracerybrace_source.as_ptr(),
            tracerybrace_source_written as i64,
            tracerybrace_old.as_ptr(),
            tracerybrace_old.len() as i64,
            tracerybrace_new.as_ptr(),
            tracerybrace_new.len() as i64,
        )
    };
    let mut tracerybrace_value = vec![0u8; tracerybrace_value_len as usize];
    let tracerybrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            tracerybrace_source.as_ptr(),
            tracerybrace_source_written as i64,
            tracerybrace_old.as_ptr(),
            tracerybrace_old.len() as i64,
            tracerybrace_new.as_ptr(),
            tracerybrace_new.len() as i64,
            tracerybrace_value.as_mut_ptr(),
            tracerybrace_value.len() as i64,
        )
    };
    let tracerybrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            tracerybrace_value.as_ptr(),
            tracerybrace_value_written as i64,
            tracerybrace_needle.as_ptr(),
            tracerybrace_needle.len() as i64,
        )
    };

    let mut selected_parallaxium_index = 0i32;
    let mut best_parallaxium_score = i32::MIN;
    let mut parallaxium_index = 0i32;
    while parallaxium_index < 3 {
        let mut candidate_parallaxium_score = parallaxium_value_written * 10 + parallaxium_contains * 50;
        if parallaxium_index == 1 {
            candidate_parallaxium_score = cesiumwake_value_written * 10 + cesiumwake_index;
        } else if parallaxium_index == 2 {
            candidate_parallaxium_score = tracerybrace_value_written * 10 + tracerybrace_contains * 50;
        }

        let mut parallaxium_bonus = 0i32;
        if parallaxium_index == selected_syzygyium_index {
            parallaxium_bonus += 25;
        }
        if parallaxium_index == selected_equinoctium_index {
            parallaxium_bonus += 15;
        }
        if parallaxium_index == selected_perihelium_index {
            parallaxium_bonus += 5;
        }
        if parallaxium_index == 0 && parallaxium_contains != 0 {
            parallaxium_bonus += 20;
        }
        if parallaxium_index == 1 && cesiumwake_index >= 0 {
            parallaxium_bonus += 10;
        }
        if parallaxium_index == 2 && tracerybrace_contains != 0 {
            parallaxium_bonus += 30;
        }

        let parallaxium_score = candidate_parallaxium_score + parallaxium_bonus;
        if parallaxium_score > best_parallaxium_score {
            best_parallaxium_score = parallaxium_score;
            selected_parallaxium_index = parallaxium_index;
        }

        parallaxium_index += 1;
    }

    let mut selected_parallaxium_ptr = parallaxium_value.as_ptr();
    let mut selected_parallaxium_written = parallaxium_value_written;
    if selected_parallaxium_index == 1 {
        selected_parallaxium_ptr = cesiumwake_value.as_ptr();
        selected_parallaxium_written = cesiumwake_value_written;
    } else if selected_parallaxium_index == 2 {
        selected_parallaxium_ptr = tracerybrace_value.as_ptr();
        selected_parallaxium_written = tracerybrace_value_written;
    }

    let declinationium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_parallaxium_ptr,
            selected_parallaxium_written as i64,
            declinationium_old.as_ptr(),
            declinationium_old.len() as i64,
            declinationium_new.as_ptr(),
            declinationium_new.len() as i64,
        )
    };
    let mut declinationium_value = vec![0u8; declinationium_value_len as usize];
    let declinationium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_parallaxium_ptr,
            selected_parallaxium_written as i64,
            declinationium_old.as_ptr(),
            declinationium_old.len() as i64,
            declinationium_new.as_ptr(),
            declinationium_new.len() as i64,
            declinationium_value.as_mut_ptr(),
            declinationium_value.len() as i64,
        )
    };
    let declinationium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            declinationium_value.as_ptr(),
            declinationium_value_written as i64,
            declinationium_needle.as_ptr(),
            declinationium_needle.len() as i64,
        )
    };

    let strontiumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_parallaxium_ptr,
            selected_parallaxium_written as i64,
            strontiumwake_extension.as_ptr(),
            strontiumwake_extension.len() as i64,
        )
    };
    let mut strontiumwake_value = vec![0u8; strontiumwake_value_len as usize];
    let strontiumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_parallaxium_ptr,
            selected_parallaxium_written as i64,
            strontiumwake_extension.as_ptr(),
            strontiumwake_extension.len() as i64,
            strontiumwake_value.as_mut_ptr(),
            strontiumwake_value.len() as i64,
        )
    };
    let strontiumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            strontiumwake_value.as_ptr(),
            strontiumwake_value_written as i64,
            strontiumwake_needle.as_ptr(),
            strontiumwake_needle.len() as i64,
        )
    };

    let extradosbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_parallaxium_ptr,
            selected_parallaxium_written as i64,
        )
    };
    let mut extradosbrace_source = vec![0u8; extradosbrace_source_len as usize];
    let extradosbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_parallaxium_ptr,
            selected_parallaxium_written as i64,
            extradosbrace_source.as_mut_ptr(),
            extradosbrace_source.len() as i64,
        )
    };
    let extradosbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            extradosbrace_source.as_ptr(),
            extradosbrace_source_written as i64,
            extradosbrace_old.as_ptr(),
            extradosbrace_old.len() as i64,
            extradosbrace_new.as_ptr(),
            extradosbrace_new.len() as i64,
        )
    };
    let mut extradosbrace_value = vec![0u8; extradosbrace_value_len as usize];
    let extradosbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            extradosbrace_source.as_ptr(),
            extradosbrace_source_written as i64,
            extradosbrace_old.as_ptr(),
            extradosbrace_old.len() as i64,
            extradosbrace_new.as_ptr(),
            extradosbrace_new.len() as i64,
            extradosbrace_value.as_mut_ptr(),
            extradosbrace_value.len() as i64,
        )
    };
    let extradosbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            extradosbrace_value.as_ptr(),
            extradosbrace_value_written as i64,
            extradosbrace_needle.as_ptr(),
            extradosbrace_needle.len() as i64,
        )
    };

    let mut selected_declinationium_index = 0i32;
    let mut best_declinationium_score = i32::MIN;
    let mut declinationium_index = 0i32;
    while declinationium_index < 3 {
        let mut candidate_declinationium_score = declinationium_value_written * 10 + declinationium_contains * 50;
        if declinationium_index == 1 {
            candidate_declinationium_score = strontiumwake_value_written * 10 + strontiumwake_index;
        } else if declinationium_index == 2 {
            candidate_declinationium_score = extradosbrace_value_written * 10 + extradosbrace_contains * 50;
        }

        let mut declinationium_bonus = 0i32;
        if declinationium_index == selected_parallaxium_index {
            declinationium_bonus += 25;
        }
        if declinationium_index == selected_syzygyium_index {
            declinationium_bonus += 15;
        }
        if declinationium_index == selected_equinoctium_index {
            declinationium_bonus += 5;
        }
        if declinationium_index == 0 && declinationium_contains != 0 {
            declinationium_bonus += 20;
        }
        if declinationium_index == 1 && strontiumwake_index >= 0 {
            declinationium_bonus += 10;
        }
        if declinationium_index == 2 && extradosbrace_contains != 0 {
            declinationium_bonus += 30;
        }

        let declinationium_score = candidate_declinationium_score + declinationium_bonus;
        if declinationium_score > best_declinationium_score {
            best_declinationium_score = declinationium_score;
            selected_declinationium_index = declinationium_index;
        }

        declinationium_index += 1;
    }

    let mut selected_declinationium_ptr = declinationium_value.as_ptr();
    let mut selected_declinationium_written = declinationium_value_written;
    if selected_declinationium_index == 1 {
        selected_declinationium_ptr = strontiumwake_value.as_ptr();
        selected_declinationium_written = strontiumwake_value_written;
    } else if selected_declinationium_index == 2 {
        selected_declinationium_ptr = extradosbrace_value.as_ptr();
        selected_declinationium_written = extradosbrace_value_written;
    }

    let azimuthium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_declinationium_ptr,
            selected_declinationium_written as i64,
            azimuthium_old.as_ptr(),
            azimuthium_old.len() as i64,
            azimuthium_new.as_ptr(),
            azimuthium_new.len() as i64,
        )
    };
    let mut azimuthium_value = vec![0u8; azimuthium_value_len as usize];
    let azimuthium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_declinationium_ptr,
            selected_declinationium_written as i64,
            azimuthium_old.as_ptr(),
            azimuthium_old.len() as i64,
            azimuthium_new.as_ptr(),
            azimuthium_new.len() as i64,
            azimuthium_value.as_mut_ptr(),
            azimuthium_value.len() as i64,
        )
    };
    let azimuthium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            azimuthium_value.as_ptr(),
            azimuthium_value_written as i64,
            azimuthium_needle.as_ptr(),
            azimuthium_needle.len() as i64,
        )
    };

    let bariumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_declinationium_ptr,
            selected_declinationium_written as i64,
            bariumwake_extension.as_ptr(),
            bariumwake_extension.len() as i64,
        )
    };
    let mut bariumwake_value = vec![0u8; bariumwake_value_len as usize];
    let bariumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_declinationium_ptr,
            selected_declinationium_written as i64,
            bariumwake_extension.as_ptr(),
            bariumwake_extension.len() as i64,
            bariumwake_value.as_mut_ptr(),
            bariumwake_value.len() as i64,
        )
    };
    let bariumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            bariumwake_value.as_ptr(),
            bariumwake_value_written as i64,
            bariumwake_needle.as_ptr(),
            bariumwake_needle.len() as i64,
        )
    };

    let impostbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_declinationium_ptr,
            selected_declinationium_written as i64,
        )
    };
    let mut impostbrace_source = vec![0u8; impostbrace_source_len as usize];
    let impostbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_declinationium_ptr,
            selected_declinationium_written as i64,
            impostbrace_source.as_mut_ptr(),
            impostbrace_source.len() as i64,
        )
    };
    let impostbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            impostbrace_source.as_ptr(),
            impostbrace_source_written as i64,
            impostbrace_old.as_ptr(),
            impostbrace_old.len() as i64,
            impostbrace_new.as_ptr(),
            impostbrace_new.len() as i64,
        )
    };
    let mut impostbrace_value = vec![0u8; impostbrace_value_len as usize];
    let impostbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            impostbrace_source.as_ptr(),
            impostbrace_source_written as i64,
            impostbrace_old.as_ptr(),
            impostbrace_old.len() as i64,
            impostbrace_new.as_ptr(),
            impostbrace_new.len() as i64,
            impostbrace_value.as_mut_ptr(),
            impostbrace_value.len() as i64,
        )
    };
    let impostbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            impostbrace_value.as_ptr(),
            impostbrace_value_written as i64,
            impostbrace_needle.as_ptr(),
            impostbrace_needle.len() as i64,
        )
    };

    let mut selected_azimuthium_index = 0i32;
    let mut best_azimuthium_score = i32::MIN;
    let mut azimuthium_index = 0i32;
    while azimuthium_index < 3 {
        let mut candidate_azimuthium_score = azimuthium_value_written * 10 + azimuthium_contains * 50;
        if azimuthium_index == 1 {
            candidate_azimuthium_score = bariumwake_value_written * 10 + bariumwake_index;
        } else if azimuthium_index == 2 {
            candidate_azimuthium_score = impostbrace_value_written * 10 + impostbrace_contains * 50;
        }

        let mut azimuthium_bonus = 0i32;
        if azimuthium_index == selected_declinationium_index {
            azimuthium_bonus += 25;
        }
        if azimuthium_index == selected_parallaxium_index {
            azimuthium_bonus += 15;
        }
        if azimuthium_index == selected_syzygyium_index {
            azimuthium_bonus += 5;
        }
        if azimuthium_index == 0 && azimuthium_contains != 0 {
            azimuthium_bonus += 20;
        }
        if azimuthium_index == 1 && bariumwake_index >= 0 {
            azimuthium_bonus += 10;
        }
        if azimuthium_index == 2 && impostbrace_contains != 0 {
            azimuthium_bonus += 30;
        }

        let azimuthium_score = candidate_azimuthium_score + azimuthium_bonus;
        if azimuthium_score > best_azimuthium_score {
            best_azimuthium_score = azimuthium_score;
            selected_azimuthium_index = azimuthium_index;
        }

        azimuthium_index += 1;
    }

    let mut selected_azimuthium_ptr = azimuthium_value.as_ptr();
    let mut selected_azimuthium_written = azimuthium_value_written;
    if selected_azimuthium_index == 1 {
        selected_azimuthium_ptr = bariumwake_value.as_ptr();
        selected_azimuthium_written = bariumwake_value_written;
    } else if selected_azimuthium_index == 2 {
        selected_azimuthium_ptr = impostbrace_value.as_ptr();
        selected_azimuthium_written = impostbrace_value_written;
    }

    let aphelionium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_azimuthium_ptr,
            selected_azimuthium_written as i64,
            aphelionium_old.as_ptr(),
            aphelionium_old.len() as i64,
            aphelionium_new.as_ptr(),
            aphelionium_new.len() as i64,
        )
    };
    let mut aphelionium_value = vec![0u8; aphelionium_value_len as usize];
    let aphelionium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_azimuthium_ptr,
            selected_azimuthium_written as i64,
            aphelionium_old.as_ptr(),
            aphelionium_old.len() as i64,
            aphelionium_new.as_ptr(),
            aphelionium_new.len() as i64,
            aphelionium_value.as_mut_ptr(),
            aphelionium_value.len() as i64,
        )
    };
    let aphelionium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            aphelionium_value.as_ptr(),
            aphelionium_value_written as i64,
            aphelionium_needle.as_ptr(),
            aphelionium_needle.len() as i64,
        )
    };

    let rheniumwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_azimuthium_ptr,
            selected_azimuthium_written as i64,
            rheniumwake_extension.as_ptr(),
            rheniumwake_extension.len() as i64,
        )
    };
    let mut rheniumwake_value = vec![0u8; rheniumwake_value_len as usize];
    let rheniumwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_azimuthium_ptr,
            selected_azimuthium_written as i64,
            rheniumwake_extension.as_ptr(),
            rheniumwake_extension.len() as i64,
            rheniumwake_value.as_mut_ptr(),
            rheniumwake_value.len() as i64,
        )
    };
    let rheniumwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            rheniumwake_value.as_ptr(),
            rheniumwake_value_written as i64,
            rheniumwake_needle.as_ptr(),
            rheniumwake_needle.len() as i64,
        )
    };

    let springerbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_azimuthium_ptr,
            selected_azimuthium_written as i64,
        )
    };
    let mut springerbrace_source = vec![0u8; springerbrace_source_len as usize];
    let springerbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_azimuthium_ptr,
            selected_azimuthium_written as i64,
            springerbrace_source.as_mut_ptr(),
            springerbrace_source.len() as i64,
        )
    };
    let springerbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            springerbrace_source.as_ptr(),
            springerbrace_source_written as i64,
            springerbrace_old.as_ptr(),
            springerbrace_old.len() as i64,
            springerbrace_new.as_ptr(),
            springerbrace_new.len() as i64,
        )
    };
    let mut springerbrace_value = vec![0u8; springerbrace_value_len as usize];
    let springerbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            springerbrace_source.as_ptr(),
            springerbrace_source_written as i64,
            springerbrace_old.as_ptr(),
            springerbrace_old.len() as i64,
            springerbrace_new.as_ptr(),
            springerbrace_new.len() as i64,
            springerbrace_value.as_mut_ptr(),
            springerbrace_value.len() as i64,
        )
    };
    let springerbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            springerbrace_value.as_ptr(),
            springerbrace_value_written as i64,
            springerbrace_needle.as_ptr(),
            springerbrace_needle.len() as i64,
        )
    };

    let mut selected_aphelionium_index = 0i32;
    let mut best_aphelionium_score = i32::MIN;
    let mut aphelionium_index = 0i32;
    while aphelionium_index < 3 {
        let mut candidate_aphelionium_score = aphelionium_value_written * 10 + aphelionium_contains * 50;
        if aphelionium_index == 1 {
            candidate_aphelionium_score = rheniumwake_value_written * 10 + rheniumwake_index;
        } else if aphelionium_index == 2 {
            candidate_aphelionium_score = springerbrace_value_written * 10 + springerbrace_contains * 50;
        }

        let mut aphelionium_bonus = 0i32;
        if aphelionium_index == selected_azimuthium_index {
            aphelionium_bonus += 25;
        }
        if aphelionium_index == selected_declinationium_index {
            aphelionium_bonus += 15;
        }
        if aphelionium_index == selected_parallaxium_index {
            aphelionium_bonus += 5;
        }
        if aphelionium_index == 0 && aphelionium_contains != 0 {
            aphelionium_bonus += 20;
        }
        if aphelionium_index == 1 && rheniumwake_index >= 0 {
            aphelionium_bonus += 10;
        }
        if aphelionium_index == 2 && springerbrace_contains != 0 {
            aphelionium_bonus += 30;
        }

        let aphelionium_score = candidate_aphelionium_score + aphelionium_bonus;
        if aphelionium_score > best_aphelionium_score {
            best_aphelionium_score = aphelionium_score;
            selected_aphelionium_index = aphelionium_index;
        }

        aphelionium_index += 1;
    }

    let mut selected_aphelionium_ptr = aphelionium_value.as_ptr();
    let mut selected_aphelionium_written = aphelionium_value_written;
    if selected_aphelionium_index == 1 {
        selected_aphelionium_ptr = rheniumwake_value.as_ptr();
        selected_aphelionium_written = rheniumwake_value_written;
    } else if selected_aphelionium_index == 2 {
        selected_aphelionium_ptr = springerbrace_value.as_ptr();
        selected_aphelionium_written = springerbrace_value_written;
    }

    let periapsisium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_aphelionium_ptr,
            selected_aphelionium_written as i64,
            periapsisium_old.as_ptr(),
            periapsisium_old.len() as i64,
            periapsisium_new.as_ptr(),
            periapsisium_new.len() as i64,
        )
    };
    let mut periapsisium_value = vec![0u8; periapsisium_value_len as usize];
    let periapsisium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_aphelionium_ptr,
            selected_aphelionium_written as i64,
            periapsisium_old.as_ptr(),
            periapsisium_old.len() as i64,
            periapsisium_new.as_ptr(),
            periapsisium_new.len() as i64,
            periapsisium_value.as_mut_ptr(),
            periapsisium_value.len() as i64,
        )
    };
    let periapsisium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            periapsisium_value.as_ptr(),
            periapsisium_value_written as i64,
            periapsisium_needle.as_ptr(),
            periapsisium_needle.len() as i64,
        )
    };

    let quasarwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_aphelionium_ptr,
            selected_aphelionium_written as i64,
            quasarwake_extension.as_ptr(),
            quasarwake_extension.len() as i64,
        )
    };
    let mut quasarwake_value = vec![0u8; quasarwake_value_len as usize];
    let quasarwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_aphelionium_ptr,
            selected_aphelionium_written as i64,
            quasarwake_extension.as_ptr(),
            quasarwake_extension.len() as i64,
            quasarwake_value.as_mut_ptr(),
            quasarwake_value.len() as i64,
        )
    };
    let quasarwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            quasarwake_value.as_ptr(),
            quasarwake_value_written as i64,
            quasarwake_needle.as_ptr(),
            quasarwake_needle.len() as i64,
        )
    };

    let skewbackbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_aphelionium_ptr,
            selected_aphelionium_written as i64,
        )
    };
    let mut skewbackbrace_source = vec![0u8; skewbackbrace_source_len as usize];
    let skewbackbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_aphelionium_ptr,
            selected_aphelionium_written as i64,
            skewbackbrace_source.as_mut_ptr(),
            skewbackbrace_source.len() as i64,
        )
    };
    let skewbackbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            skewbackbrace_source.as_ptr(),
            skewbackbrace_source_written as i64,
            skewbackbrace_old.as_ptr(),
            skewbackbrace_old.len() as i64,
            skewbackbrace_new.as_ptr(),
            skewbackbrace_new.len() as i64,
        )
    };
    let mut skewbackbrace_value = vec![0u8; skewbackbrace_value_len as usize];
    let skewbackbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            skewbackbrace_source.as_ptr(),
            skewbackbrace_source_written as i64,
            skewbackbrace_old.as_ptr(),
            skewbackbrace_old.len() as i64,
            skewbackbrace_new.as_ptr(),
            skewbackbrace_new.len() as i64,
            skewbackbrace_value.as_mut_ptr(),
            skewbackbrace_value.len() as i64,
        )
    };
    let skewbackbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            skewbackbrace_value.as_ptr(),
            skewbackbrace_value_written as i64,
            skewbackbrace_needle.as_ptr(),
            skewbackbrace_needle.len() as i64,
        )
    };

    let mut selected_periapsisium_index = 0i32;
    let mut best_periapsisium_score = i32::MIN;
    let mut periapsisium_index = 0i32;
    while periapsisium_index < 3 {
        let mut candidate_periapsisium_score = periapsisium_value_written * 10 + periapsisium_contains * 50;
        if periapsisium_index == 1 {
            candidate_periapsisium_score = quasarwake_value_written * 10 + quasarwake_index;
        } else if periapsisium_index == 2 {
            candidate_periapsisium_score = skewbackbrace_value_written * 10 + skewbackbrace_contains * 50;
        }

        let mut periapsisium_bonus = 0i32;
        if periapsisium_index == selected_aphelionium_index {
            periapsisium_bonus += 25;
        }
        if periapsisium_index == selected_azimuthium_index {
            periapsisium_bonus += 15;
        }
        if periapsisium_index == selected_declinationium_index {
            periapsisium_bonus += 5;
        }
        if periapsisium_index == 0 && periapsisium_contains != 0 {
            periapsisium_bonus += 20;
        }
        if periapsisium_index == 1 && quasarwake_index >= 0 {
            periapsisium_bonus += 10;
        }
        if periapsisium_index == 2 && skewbackbrace_contains != 0 {
            periapsisium_bonus += 30;
        }

        let periapsisium_score = candidate_periapsisium_score + periapsisium_bonus;
        if periapsisium_score > best_periapsisium_score {
            best_periapsisium_score = periapsisium_score;
            selected_periapsisium_index = periapsisium_index;
        }

        periapsisium_index += 1;
    }

    let mut selected_periapsisium_ptr = periapsisium_value.as_ptr();
    let mut selected_periapsisium_written = periapsisium_value_written;
    if selected_periapsisium_index == 1 {
        selected_periapsisium_ptr = quasarwake_value.as_ptr();
        selected_periapsisium_written = quasarwake_value_written;
    } else if selected_periapsisium_index == 2 {
        selected_periapsisium_ptr = skewbackbrace_value.as_ptr();
        selected_periapsisium_written = skewbackbrace_value_written;
    }

    let apsidialium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_periapsisium_ptr,
            selected_periapsisium_written as i64,
            apsidialium_old.as_ptr(),
            apsidialium_old.len() as i64,
            apsidialium_new.as_ptr(),
            apsidialium_new.len() as i64,
        )
    };
    let mut apsidialium_value = vec![0u8; apsidialium_value_len as usize];
    let apsidialium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_periapsisium_ptr,
            selected_periapsisium_written as i64,
            apsidialium_old.as_ptr(),
            apsidialium_old.len() as i64,
            apsidialium_new.as_ptr(),
            apsidialium_new.len() as i64,
            apsidialium_value.as_mut_ptr(),
            apsidialium_value.len() as i64,
        )
    };
    let apsidialium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            apsidialium_value.as_ptr(),
            apsidialium_value_written as i64,
            apsidialium_needle.as_ptr(),
            apsidialium_needle.len() as i64,
        )
    };

    let pulsarwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_periapsisium_ptr,
            selected_periapsisium_written as i64,
            pulsarwake_extension.as_ptr(),
            pulsarwake_extension.len() as i64,
        )
    };
    let mut pulsarwake_value = vec![0u8; pulsarwake_value_len as usize];
    let pulsarwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_periapsisium_ptr,
            selected_periapsisium_written as i64,
            pulsarwake_extension.as_ptr(),
            pulsarwake_extension.len() as i64,
            pulsarwake_value.as_mut_ptr(),
            pulsarwake_value.len() as i64,
        )
    };
    let pulsarwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            pulsarwake_value.as_ptr(),
            pulsarwake_value_written as i64,
            pulsarwake_needle.as_ptr(),
            pulsarwake_needle.len() as i64,
        )
    };

    let abutmentbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_periapsisium_ptr,
            selected_periapsisium_written as i64,
        )
    };
    let mut abutmentbrace_source = vec![0u8; abutmentbrace_source_len as usize];
    let abutmentbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_periapsisium_ptr,
            selected_periapsisium_written as i64,
            abutmentbrace_source.as_mut_ptr(),
            abutmentbrace_source.len() as i64,
        )
    };
    let abutmentbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            abutmentbrace_source.as_ptr(),
            abutmentbrace_source_written as i64,
            abutmentbrace_old.as_ptr(),
            abutmentbrace_old.len() as i64,
            abutmentbrace_new.as_ptr(),
            abutmentbrace_new.len() as i64,
        )
    };
    let mut abutmentbrace_value = vec![0u8; abutmentbrace_value_len as usize];
    let abutmentbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            abutmentbrace_source.as_ptr(),
            abutmentbrace_source_written as i64,
            abutmentbrace_old.as_ptr(),
            abutmentbrace_old.len() as i64,
            abutmentbrace_new.as_ptr(),
            abutmentbrace_new.len() as i64,
            abutmentbrace_value.as_mut_ptr(),
            abutmentbrace_value.len() as i64,
        )
    };
    let abutmentbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            abutmentbrace_value.as_ptr(),
            abutmentbrace_value_written as i64,
            abutmentbrace_needle.as_ptr(),
            abutmentbrace_needle.len() as i64,
        )
    };

    let mut selected_apsidialium_index = 0i32;
    let mut best_apsidialium_score = i32::MIN;
    let mut apsidialium_index = 0i32;
    while apsidialium_index < 3 {
        let mut candidate_apsidialium_score = apsidialium_value_written * 10 + apsidialium_contains * 50;
        if apsidialium_index == 1 {
            candidate_apsidialium_score = pulsarwake_value_written * 10 + pulsarwake_index;
        } else if apsidialium_index == 2 {
            candidate_apsidialium_score = abutmentbrace_value_written * 10 + abutmentbrace_contains * 50;
        }

        let mut apsidialium_bonus = 0i32;
        if apsidialium_index == selected_periapsisium_index {
            apsidialium_bonus += 25;
        }
        if apsidialium_index == selected_aphelionium_index {
            apsidialium_bonus += 15;
        }
        if apsidialium_index == selected_azimuthium_index {
            apsidialium_bonus += 5;
        }
        if apsidialium_index == 0 && apsidialium_contains != 0 {
            apsidialium_bonus += 20;
        }
        if apsidialium_index == 1 && pulsarwake_index >= 0 {
            apsidialium_bonus += 10;
        }
        if apsidialium_index == 2 && abutmentbrace_contains != 0 {
            apsidialium_bonus += 30;
        }

        let apsidialium_score = candidate_apsidialium_score + apsidialium_bonus;
        if apsidialium_score > best_apsidialium_score {
            best_apsidialium_score = apsidialium_score;
            selected_apsidialium_index = apsidialium_index;
        }

        apsidialium_index += 1;
    }

    let mut selected_apsidialium_ptr = apsidialium_value.as_ptr();
    let mut selected_apsidialium_written = apsidialium_value_written;
    if selected_apsidialium_index == 1 {
        selected_apsidialium_ptr = pulsarwake_value.as_ptr();
        selected_apsidialium_written = pulsarwake_value_written;
    } else if selected_apsidialium_index == 2 {
        selected_apsidialium_ptr = abutmentbrace_value.as_ptr();
        selected_apsidialium_written = abutmentbrace_value_written;
    }

    let eccentricium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_apsidialium_ptr,
            selected_apsidialium_written as i64,
            eccentricium_old.as_ptr(),
            eccentricium_old.len() as i64,
            eccentricium_new.as_ptr(),
            eccentricium_new.len() as i64,
        )
    };
    let mut eccentricium_value = vec![0u8; eccentricium_value_len as usize];
    let eccentricium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_apsidialium_ptr,
            selected_apsidialium_written as i64,
            eccentricium_old.as_ptr(),
            eccentricium_old.len() as i64,
            eccentricium_new.as_ptr(),
            eccentricium_new.len() as i64,
            eccentricium_value.as_mut_ptr(),
            eccentricium_value.len() as i64,
        )
    };
    let eccentricium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            eccentricium_value.as_ptr(),
            eccentricium_value_written as i64,
            eccentricium_needle.as_ptr(),
            eccentricium_needle.len() as i64,
        )
    };

    let nebularwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_apsidialium_ptr,
            selected_apsidialium_written as i64,
            nebularwake_extension.as_ptr(),
            nebularwake_extension.len() as i64,
        )
    };
    let mut nebularwake_value = vec![0u8; nebularwake_value_len as usize];
    let nebularwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_apsidialium_ptr,
            selected_apsidialium_written as i64,
            nebularwake_extension.as_ptr(),
            nebularwake_extension.len() as i64,
            nebularwake_value.as_mut_ptr(),
            nebularwake_value.len() as i64,
        )
    };
    let nebularwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            nebularwake_value.as_ptr(),
            nebularwake_value_written as i64,
            nebularwake_needle.as_ptr(),
            nebularwake_needle.len() as i64,
        )
    };

    let plinthbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_apsidialium_ptr,
            selected_apsidialium_written as i64,
        )
    };
    let mut plinthbrace_source = vec![0u8; plinthbrace_source_len as usize];
    let plinthbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_apsidialium_ptr,
            selected_apsidialium_written as i64,
            plinthbrace_source.as_mut_ptr(),
            plinthbrace_source.len() as i64,
        )
    };
    let plinthbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            plinthbrace_source.as_ptr(),
            plinthbrace_source_written as i64,
            plinthbrace_old.as_ptr(),
            plinthbrace_old.len() as i64,
            plinthbrace_new.as_ptr(),
            plinthbrace_new.len() as i64,
        )
    };
    let mut plinthbrace_value = vec![0u8; plinthbrace_value_len as usize];
    let plinthbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            plinthbrace_source.as_ptr(),
            plinthbrace_source_written as i64,
            plinthbrace_old.as_ptr(),
            plinthbrace_old.len() as i64,
            plinthbrace_new.as_ptr(),
            plinthbrace_new.len() as i64,
            plinthbrace_value.as_mut_ptr(),
            plinthbrace_value.len() as i64,
        )
    };
    let plinthbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            plinthbrace_value.as_ptr(),
            plinthbrace_value_written as i64,
            plinthbrace_needle.as_ptr(),
            plinthbrace_needle.len() as i64,
        )
    };

    let mut selected_eccentricium_index = 0i32;
    let mut best_eccentricium_score = i32::MIN;
    let mut eccentricium_index = 0i32;
    while eccentricium_index < 3 {
        let mut candidate_eccentricium_score = eccentricium_value_written * 10 + eccentricium_contains * 50;
        if eccentricium_index == 1 {
            candidate_eccentricium_score = nebularwake_value_written * 10 + nebularwake_index;
        } else if eccentricium_index == 2 {
            candidate_eccentricium_score = plinthbrace_value_written * 10 + plinthbrace_contains * 50;
        }

        let mut eccentricium_bonus = 0i32;
        if eccentricium_index == selected_apsidialium_index {
            eccentricium_bonus += 25;
        }
        if eccentricium_index == selected_periapsisium_index {
            eccentricium_bonus += 15;
        }
        if eccentricium_index == selected_aphelionium_index {
            eccentricium_bonus += 5;
        }
        if eccentricium_index == 0 && eccentricium_contains != 0 {
            eccentricium_bonus += 20;
        }
        if eccentricium_index == 1 && nebularwake_index >= 0 {
            eccentricium_bonus += 10;
        }
        if eccentricium_index == 2 && plinthbrace_contains != 0 {
            eccentricium_bonus += 30;
        }

        let eccentricium_score = candidate_eccentricium_score + eccentricium_bonus;
        if eccentricium_score > best_eccentricium_score {
            best_eccentricium_score = eccentricium_score;
            selected_eccentricium_index = eccentricium_index;
        }

        eccentricium_index += 1;
    }

    let mut selected_eccentricium_ptr = eccentricium_value.as_ptr();
    let mut selected_eccentricium_written = eccentricium_value_written;
    if selected_eccentricium_index == 1 {
        selected_eccentricium_ptr = nebularwake_value.as_ptr();
        selected_eccentricium_written = nebularwake_value_written;
    } else if selected_eccentricium_index == 2 {
        selected_eccentricium_ptr = plinthbrace_value.as_ptr();
        selected_eccentricium_written = plinthbrace_value_written;
    }

    let epicyclium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_eccentricium_ptr,
            selected_eccentricium_written as i64,
            epicyclium_old.as_ptr(),
            epicyclium_old.len() as i64,
            epicyclium_new.as_ptr(),
            epicyclium_new.len() as i64,
        )
    };
    let mut epicyclium_value = vec![0u8; epicyclium_value_len as usize];
    let epicyclium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_eccentricium_ptr,
            selected_eccentricium_written as i64,
            epicyclium_old.as_ptr(),
            epicyclium_old.len() as i64,
            epicyclium_new.as_ptr(),
            epicyclium_new.len() as i64,
            epicyclium_value.as_mut_ptr(),
            epicyclium_value.len() as i64,
        )
    };
    let epicyclium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            epicyclium_value.as_ptr(),
            epicyclium_value_written as i64,
            epicyclium_needle.as_ptr(),
            epicyclium_needle.len() as i64,
        )
    };

    let magnetarwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_eccentricium_ptr,
            selected_eccentricium_written as i64,
            magnetarwake_extension.as_ptr(),
            magnetarwake_extension.len() as i64,
        )
    };
    let mut magnetarwake_value = vec![0u8; magnetarwake_value_len as usize];
    let magnetarwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_eccentricium_ptr,
            selected_eccentricium_written as i64,
            magnetarwake_extension.as_ptr(),
            magnetarwake_extension.len() as i64,
            magnetarwake_value.as_mut_ptr(),
            magnetarwake_value.len() as i64,
        )
    };
    let magnetarwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            magnetarwake_value.as_ptr(),
            magnetarwake_value_written as i64,
            magnetarwake_needle.as_ptr(),
            magnetarwake_needle.len() as i64,
        )
    };

    let pedestalbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_eccentricium_ptr,
            selected_eccentricium_written as i64,
        )
    };
    let mut pedestalbrace_source = vec![0u8; pedestalbrace_source_len as usize];
    let pedestalbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_eccentricium_ptr,
            selected_eccentricium_written as i64,
            pedestalbrace_source.as_mut_ptr(),
            pedestalbrace_source.len() as i64,
        )
    };
    let pedestalbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            pedestalbrace_source.as_ptr(),
            pedestalbrace_source_written as i64,
            pedestalbrace_old.as_ptr(),
            pedestalbrace_old.len() as i64,
            pedestalbrace_new.as_ptr(),
            pedestalbrace_new.len() as i64,
        )
    };
    let mut pedestalbrace_value = vec![0u8; pedestalbrace_value_len as usize];
    let pedestalbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            pedestalbrace_source.as_ptr(),
            pedestalbrace_source_written as i64,
            pedestalbrace_old.as_ptr(),
            pedestalbrace_old.len() as i64,
            pedestalbrace_new.as_ptr(),
            pedestalbrace_new.len() as i64,
            pedestalbrace_value.as_mut_ptr(),
            pedestalbrace_value.len() as i64,
        )
    };
    let pedestalbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            pedestalbrace_value.as_ptr(),
            pedestalbrace_value_written as i64,
            pedestalbrace_needle.as_ptr(),
            pedestalbrace_needle.len() as i64,
        )
    };

    let mut selected_epicyclium_index = 0i32;
    let mut best_epicyclium_score = i32::MIN;
    let mut epicyclium_index = 0i32;
    while epicyclium_index < 3 {
        let mut candidate_epicyclium_score = epicyclium_value_written * 10 + epicyclium_contains * 50;
        if epicyclium_index == 1 {
            candidate_epicyclium_score = magnetarwake_value_written * 10 + magnetarwake_index;
        } else if epicyclium_index == 2 {
            candidate_epicyclium_score = pedestalbrace_value_written * 10 + pedestalbrace_contains * 50;
        }

        let mut epicyclium_bonus = 0i32;
        if epicyclium_index == selected_eccentricium_index {
            epicyclium_bonus += 25;
        }
        if epicyclium_index == selected_apsidialium_index {
            epicyclium_bonus += 15;
        }
        if epicyclium_index == selected_periapsisium_index {
            epicyclium_bonus += 5;
        }
        if epicyclium_index == 0 && epicyclium_contains != 0 {
            epicyclium_bonus += 20;
        }
        if epicyclium_index == 1 && magnetarwake_index >= 0 {
            epicyclium_bonus += 10;
        }
        if epicyclium_index == 2 && pedestalbrace_contains != 0 {
            epicyclium_bonus += 30;
        }

        let epicyclium_score = candidate_epicyclium_score + epicyclium_bonus;
        if epicyclium_score > best_epicyclium_score {
            best_epicyclium_score = epicyclium_score;
            selected_epicyclium_index = epicyclium_index;
        }

        epicyclium_index += 1;
    }

    let mut selected_epicyclium_ptr = epicyclium_value.as_ptr();
    let mut selected_epicyclium_written = epicyclium_value_written;
    if selected_epicyclium_index == 1 {
        selected_epicyclium_ptr = magnetarwake_value.as_ptr();
        selected_epicyclium_written = magnetarwake_value_written;
    } else if selected_epicyclium_index == 2 {
        selected_epicyclium_ptr = pedestalbrace_value.as_ptr();
        selected_epicyclium_written = pedestalbrace_value_written;
    }

    let deferentium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_epicyclium_ptr,
            selected_epicyclium_written as i64,
            deferentium_old.as_ptr(),
            deferentium_old.len() as i64,
            deferentium_new.as_ptr(),
            deferentium_new.len() as i64,
        )
    };
    let mut deferentium_value = vec![0u8; deferentium_value_len as usize];
    let deferentium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_epicyclium_ptr,
            selected_epicyclium_written as i64,
            deferentium_old.as_ptr(),
            deferentium_old.len() as i64,
            deferentium_new.as_ptr(),
            deferentium_new.len() as i64,
            deferentium_value.as_mut_ptr(),
            deferentium_value.len() as i64,
        )
    };
    let deferentium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            deferentium_value.as_ptr(),
            deferentium_value_written as i64,
            deferentium_needle.as_ptr(),
            deferentium_needle.len() as i64,
        )
    };

    let maserwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_epicyclium_ptr,
            selected_epicyclium_written as i64,
            maserwake_extension.as_ptr(),
            maserwake_extension.len() as i64,
        )
    };
    let mut maserwake_value = vec![0u8; maserwake_value_len as usize];
    let maserwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_epicyclium_ptr,
            selected_epicyclium_written as i64,
            maserwake_extension.as_ptr(),
            maserwake_extension.len() as i64,
            maserwake_value.as_mut_ptr(),
            maserwake_value.len() as i64,
        )
    };
    let maserwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            maserwake_value.as_ptr(),
            maserwake_value_written as i64,
            maserwake_needle.as_ptr(),
            maserwake_needle.len() as i64,
        )
    };

    let soclebrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_epicyclium_ptr,
            selected_epicyclium_written as i64,
        )
    };
    let mut soclebrace_source = vec![0u8; soclebrace_source_len as usize];
    let soclebrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_epicyclium_ptr,
            selected_epicyclium_written as i64,
            soclebrace_source.as_mut_ptr(),
            soclebrace_source.len() as i64,
        )
    };
    let soclebrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            soclebrace_source.as_ptr(),
            soclebrace_source_written as i64,
            soclebrace_old.as_ptr(),
            soclebrace_old.len() as i64,
            soclebrace_new.as_ptr(),
            soclebrace_new.len() as i64,
        )
    };
    let mut soclebrace_value = vec![0u8; soclebrace_value_len as usize];
    let soclebrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            soclebrace_source.as_ptr(),
            soclebrace_source_written as i64,
            soclebrace_old.as_ptr(),
            soclebrace_old.len() as i64,
            soclebrace_new.as_ptr(),
            soclebrace_new.len() as i64,
            soclebrace_value.as_mut_ptr(),
            soclebrace_value.len() as i64,
        )
    };
    let soclebrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            soclebrace_value.as_ptr(),
            soclebrace_value_written as i64,
            soclebrace_needle.as_ptr(),
            soclebrace_needle.len() as i64,
        )
    };

    let mut selected_deferentium_index = 0i32;
    let mut best_deferentium_score = i32::MIN;
    let mut deferentium_index = 0i32;
    while deferentium_index < 3 {
        let mut candidate_deferentium_score = deferentium_value_written * 10 + deferentium_contains * 50;
        if deferentium_index == 1 {
            candidate_deferentium_score = maserwake_value_written * 10 + maserwake_index;
        } else if deferentium_index == 2 {
            candidate_deferentium_score = soclebrace_value_written * 10 + soclebrace_contains * 50;
        }

        let mut deferentium_bonus = 0i32;
        if deferentium_index == selected_epicyclium_index {
            deferentium_bonus += 25;
        }
        if deferentium_index == selected_eccentricium_index {
            deferentium_bonus += 15;
        }
        if deferentium_index == selected_apsidialium_index {
            deferentium_bonus += 5;
        }
        if deferentium_index == 0 && deferentium_contains != 0 {
            deferentium_bonus += 20;
        }
        if deferentium_index == 1 && maserwake_index >= 0 {
            deferentium_bonus += 10;
        }
        if deferentium_index == 2 && soclebrace_contains != 0 {
            deferentium_bonus += 30;
        }

        let deferentium_score = candidate_deferentium_score + deferentium_bonus;
        if deferentium_score > best_deferentium_score {
            best_deferentium_score = deferentium_score;
            selected_deferentium_index = deferentium_index;
        }

        deferentium_index += 1;
    }

    let mut selected_deferentium_ptr = deferentium_value.as_ptr();
    let mut selected_deferentium_written = deferentium_value_written;
    if selected_deferentium_index == 1 {
        selected_deferentium_ptr = maserwake_value.as_ptr();
        selected_deferentium_written = maserwake_value_written;
    } else if selected_deferentium_index == 2 {
        selected_deferentium_ptr = soclebrace_value.as_ptr();
        selected_deferentium_written = soclebrace_value_written;
    }

    let anomalyium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_deferentium_ptr,
            selected_deferentium_written as i64,
            anomalyium_old.as_ptr(),
            anomalyium_old.len() as i64,
            anomalyium_new.as_ptr(),
            anomalyium_new.len() as i64,
        )
    };
    let mut anomalyium_value = vec![0u8; anomalyium_value_len as usize];
    let anomalyium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_deferentium_ptr,
            selected_deferentium_written as i64,
            anomalyium_old.as_ptr(),
            anomalyium_old.len() as i64,
            anomalyium_new.as_ptr(),
            anomalyium_new.len() as i64,
            anomalyium_value.as_mut_ptr(),
            anomalyium_value.len() as i64,
        )
    };
    let anomalyium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            anomalyium_value.as_ptr(),
            anomalyium_value_written as i64,
            anomalyium_needle.as_ptr(),
            anomalyium_needle.len() as i64,
        )
    };

    let blazarwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_deferentium_ptr,
            selected_deferentium_written as i64,
            blazarwake_extension.as_ptr(),
            blazarwake_extension.len() as i64,
        )
    };
    let mut blazarwake_value = vec![0u8; blazarwake_value_len as usize];
    let blazarwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_deferentium_ptr,
            selected_deferentium_written as i64,
            blazarwake_extension.as_ptr(),
            blazarwake_extension.len() as i64,
            blazarwake_value.as_mut_ptr(),
            blazarwake_value.len() as i64,
        )
    };
    let blazarwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            blazarwake_value.as_ptr(),
            blazarwake_value_written as i64,
            blazarwake_needle.as_ptr(),
            blazarwake_needle.len() as i64,
        )
    };

    let stylobatebrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_deferentium_ptr,
            selected_deferentium_written as i64,
        )
    };
    let mut stylobatebrace_source = vec![0u8; stylobatebrace_source_len as usize];
    let stylobatebrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_deferentium_ptr,
            selected_deferentium_written as i64,
            stylobatebrace_source.as_mut_ptr(),
            stylobatebrace_source.len() as i64,
        )
    };
    let stylobatebrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            stylobatebrace_source.as_ptr(),
            stylobatebrace_source_written as i64,
            stylobatebrace_old.as_ptr(),
            stylobatebrace_old.len() as i64,
            stylobatebrace_new.as_ptr(),
            stylobatebrace_new.len() as i64,
        )
    };
    let mut stylobatebrace_value = vec![0u8; stylobatebrace_value_len as usize];
    let stylobatebrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            stylobatebrace_source.as_ptr(),
            stylobatebrace_source_written as i64,
            stylobatebrace_old.as_ptr(),
            stylobatebrace_old.len() as i64,
            stylobatebrace_new.as_ptr(),
            stylobatebrace_new.len() as i64,
            stylobatebrace_value.as_mut_ptr(),
            stylobatebrace_value.len() as i64,
        )
    };
    let stylobatebrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            stylobatebrace_value.as_ptr(),
            stylobatebrace_value_written as i64,
            stylobatebrace_needle.as_ptr(),
            stylobatebrace_needle.len() as i64,
        )
    };

    let mut selected_anomalyium_index = 0i32;
    let mut best_anomalyium_score = i32::MIN;
    let mut anomalyium_index = 0i32;
    while anomalyium_index < 3 {
        let mut candidate_anomalyium_score = anomalyium_value_written * 10 + anomalyium_contains * 50;
        if anomalyium_index == 1 {
            candidate_anomalyium_score = blazarwake_value_written * 10 + blazarwake_index;
        } else if anomalyium_index == 2 {
            candidate_anomalyium_score = stylobatebrace_value_written * 10 + stylobatebrace_contains * 50;
        }

        let mut anomalyium_bonus = 0i32;
        if anomalyium_index == selected_deferentium_index {
            anomalyium_bonus += 25;
        }
        if anomalyium_index == selected_epicyclium_index {
            anomalyium_bonus += 15;
        }
        if anomalyium_index == selected_eccentricium_index {
            anomalyium_bonus += 5;
        }
        if anomalyium_index == 0 && anomalyium_contains != 0 {
            anomalyium_bonus += 20;
        }
        if anomalyium_index == 1 && blazarwake_index >= 0 {
            anomalyium_bonus += 10;
        }
        if anomalyium_index == 2 && stylobatebrace_contains != 0 {
            anomalyium_bonus += 30;
        }

        let anomalyium_score = candidate_anomalyium_score + anomalyium_bonus;
        if anomalyium_score > best_anomalyium_score {
            best_anomalyium_score = anomalyium_score;
            selected_anomalyium_index = anomalyium_index;
        }

        anomalyium_index += 1;
    }

    let mut selected_anomalyium_ptr = anomalyium_value.as_ptr();
    let mut selected_anomalyium_written = anomalyium_value_written;
    if selected_anomalyium_index == 1 {
        selected_anomalyium_ptr = blazarwake_value.as_ptr();
        selected_anomalyium_written = blazarwake_value_written;
    } else if selected_anomalyium_index == 2 {
        selected_anomalyium_ptr = stylobatebrace_value.as_ptr();
        selected_anomalyium_written = stylobatebrace_value_written;
    }

    let inclinationium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_anomalyium_ptr,
            selected_anomalyium_written as i64,
            inclinationium_old.as_ptr(),
            inclinationium_old.len() as i64,
            inclinationium_new.as_ptr(),
            inclinationium_new.len() as i64,
        )
    };
    let mut inclinationium_value = vec![0u8; inclinationium_value_len as usize];
    let inclinationium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_anomalyium_ptr,
            selected_anomalyium_written as i64,
            inclinationium_old.as_ptr(),
            inclinationium_old.len() as i64,
            inclinationium_new.as_ptr(),
            inclinationium_new.len() as i64,
            inclinationium_value.as_mut_ptr(),
            inclinationium_value.len() as i64,
        )
    };
    let inclinationium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            inclinationium_value.as_ptr(),
            inclinationium_value_written as i64,
            inclinationium_needle.as_ptr(),
            inclinationium_needle.len() as i64,
        )
    };

    let novaewake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_anomalyium_ptr,
            selected_anomalyium_written as i64,
            novaewake_extension.as_ptr(),
            novaewake_extension.len() as i64,
        )
    };
    let mut novaewake_value = vec![0u8; novaewake_value_len as usize];
    let novaewake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_anomalyium_ptr,
            selected_anomalyium_written as i64,
            novaewake_extension.as_ptr(),
            novaewake_extension.len() as i64,
            novaewake_value.as_mut_ptr(),
            novaewake_value.len() as i64,
        )
    };
    let novaewake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            novaewake_value.as_ptr(),
            novaewake_value_written as i64,
            novaewake_needle.as_ptr(),
            novaewake_needle.len() as i64,
        )
    };

    let crepidomabrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_anomalyium_ptr,
            selected_anomalyium_written as i64,
        )
    };
    let mut crepidomabrace_source = vec![0u8; crepidomabrace_source_len as usize];
    let crepidomabrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_anomalyium_ptr,
            selected_anomalyium_written as i64,
            crepidomabrace_source.as_mut_ptr(),
            crepidomabrace_source.len() as i64,
        )
    };
    let crepidomabrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            crepidomabrace_source.as_ptr(),
            crepidomabrace_source_written as i64,
            crepidomabrace_old.as_ptr(),
            crepidomabrace_old.len() as i64,
            crepidomabrace_new.as_ptr(),
            crepidomabrace_new.len() as i64,
        )
    };
    let mut crepidomabrace_value = vec![0u8; crepidomabrace_value_len as usize];
    let crepidomabrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            crepidomabrace_source.as_ptr(),
            crepidomabrace_source_written as i64,
            crepidomabrace_old.as_ptr(),
            crepidomabrace_old.len() as i64,
            crepidomabrace_new.as_ptr(),
            crepidomabrace_new.len() as i64,
            crepidomabrace_value.as_mut_ptr(),
            crepidomabrace_value.len() as i64,
        )
    };
    let crepidomabrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            crepidomabrace_value.as_ptr(),
            crepidomabrace_value_written as i64,
            crepidomabrace_needle.as_ptr(),
            crepidomabrace_needle.len() as i64,
        )
    };

    let mut selected_inclinationium_index = 0i32;
    let mut best_inclinationium_score = i32::MIN;
    let mut inclinationium_index = 0i32;
    while inclinationium_index < 3 {
        let mut candidate_inclinationium_score = inclinationium_value_written * 10 + inclinationium_contains * 50;
        if inclinationium_index == 1 {
            candidate_inclinationium_score = novaewake_value_written * 10 + novaewake_index;
        } else if inclinationium_index == 2 {
            candidate_inclinationium_score = crepidomabrace_value_written * 10 + crepidomabrace_contains * 50;
        }

        let mut inclinationium_bonus = 0i32;
        if inclinationium_index == selected_anomalyium_index {
            inclinationium_bonus += 25;
        }
        if inclinationium_index == selected_deferentium_index {
            inclinationium_bonus += 15;
        }
        if inclinationium_index == selected_epicyclium_index {
            inclinationium_bonus += 5;
        }
        if inclinationium_index == 0 && inclinationium_contains != 0 {
            inclinationium_bonus += 20;
        }
        if inclinationium_index == 1 && novaewake_index >= 0 {
            inclinationium_bonus += 10;
        }
        if inclinationium_index == 2 && crepidomabrace_contains != 0 {
            inclinationium_bonus += 30;
        }

        let inclinationium_score = candidate_inclinationium_score + inclinationium_bonus;
        if inclinationium_score > best_inclinationium_score {
            best_inclinationium_score = inclinationium_score;
            selected_inclinationium_index = inclinationium_index;
        }

        inclinationium_index += 1;
    }

    let mut selected_inclinationium_ptr = inclinationium_value.as_ptr();
    let mut selected_inclinationium_written = inclinationium_value_written;
    if selected_inclinationium_index == 1 {
        selected_inclinationium_ptr = novaewake_value.as_ptr();
        selected_inclinationium_written = novaewake_value_written;
    } else if selected_inclinationium_index == 2 {
        selected_inclinationium_ptr = crepidomabrace_value.as_ptr();
        selected_inclinationium_written = crepidomabrace_value_written;
    }

    let apsidium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_inclinationium_ptr,
            selected_inclinationium_written as i64,
            apsidium_old.as_ptr(),
            apsidium_old.len() as i64,
            apsidium_new.as_ptr(),
            apsidium_new.len() as i64,
        )
    };
    let mut apsidium_value = vec![0u8; apsidium_value_len as usize];
    let apsidium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_inclinationium_ptr,
            selected_inclinationium_written as i64,
            apsidium_old.as_ptr(),
            apsidium_old.len() as i64,
            apsidium_new.as_ptr(),
            apsidium_new.len() as i64,
            apsidium_value.as_mut_ptr(),
            apsidium_value.len() as i64,
        )
    };
    let apsidium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            apsidium_value.as_ptr(),
            apsidium_value_written as i64,
            apsidium_needle.as_ptr(),
            apsidium_needle.len() as i64,
        )
    };

    let magnetowake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_inclinationium_ptr,
            selected_inclinationium_written as i64,
            magnetowake_extension.as_ptr(),
            magnetowake_extension.len() as i64,
        )
    };
    let mut magnetowake_value = vec![0u8; magnetowake_value_len as usize];
    let magnetowake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_inclinationium_ptr,
            selected_inclinationium_written as i64,
            magnetowake_extension.as_ptr(),
            magnetowake_extension.len() as i64,
            magnetowake_value.as_mut_ptr(),
            magnetowake_value.len() as i64,
        )
    };
    let magnetowake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            magnetowake_value.as_ptr(),
            magnetowake_value_written as i64,
            magnetowake_needle.as_ptr(),
            magnetowake_needle.len() as i64,
        )
    };

    let cryptbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_inclinationium_ptr,
            selected_inclinationium_written as i64,
        )
    };
    let mut cryptbrace_source = vec![0u8; cryptbrace_source_len as usize];
    let cryptbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_inclinationium_ptr,
            selected_inclinationium_written as i64,
            cryptbrace_source.as_mut_ptr(),
            cryptbrace_source.len() as i64,
        )
    };
    let cryptbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            cryptbrace_source.as_ptr(),
            cryptbrace_source_written as i64,
            cryptbrace_old.as_ptr(),
            cryptbrace_old.len() as i64,
            cryptbrace_new.as_ptr(),
            cryptbrace_new.len() as i64,
        )
    };
    let mut cryptbrace_value = vec![0u8; cryptbrace_value_len as usize];
    let cryptbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            cryptbrace_source.as_ptr(),
            cryptbrace_source_written as i64,
            cryptbrace_old.as_ptr(),
            cryptbrace_old.len() as i64,
            cryptbrace_new.as_ptr(),
            cryptbrace_new.len() as i64,
            cryptbrace_value.as_mut_ptr(),
            cryptbrace_value.len() as i64,
        )
    };
    let cryptbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            cryptbrace_value.as_ptr(),
            cryptbrace_value_written as i64,
            cryptbrace_needle.as_ptr(),
            cryptbrace_needle.len() as i64,
        )
    };

    let mut selected_apsidium_index = 0i32;
    let mut best_apsidium_score = i32::MIN;
    let mut apsidium_index = 0i32;
    while apsidium_index < 3 {
        let mut candidate_apsidium_score = apsidium_value_written * 10 + apsidium_contains * 50;
        if apsidium_index == 1 {
            candidate_apsidium_score = magnetowake_value_written * 10 + magnetowake_index;
        } else if apsidium_index == 2 {
            candidate_apsidium_score = cryptbrace_value_written * 10 + cryptbrace_contains * 50;
        }

        let mut apsidium_bonus = 0i32;
        if apsidium_index == selected_inclinationium_index {
            apsidium_bonus += 25;
        }
        if apsidium_index == selected_anomalyium_index {
            apsidium_bonus += 15;
        }
        if apsidium_index == selected_deferentium_index {
            apsidium_bonus += 5;
        }
        if apsidium_index == 0 && apsidium_contains != 0 {
            apsidium_bonus += 20;
        }
        if apsidium_index == 1 && magnetowake_index >= 0 {
            apsidium_bonus += 10;
        }
        if apsidium_index == 2 && cryptbrace_contains != 0 {
            apsidium_bonus += 30;
        }

        let apsidium_score = candidate_apsidium_score + apsidium_bonus;
        if apsidium_score > best_apsidium_score {
            best_apsidium_score = apsidium_score;
            selected_apsidium_index = apsidium_index;
        }

        apsidium_index += 1;
    }

    let mut selected_apsidium_ptr = apsidium_value.as_ptr();
    let mut selected_apsidium_written = apsidium_value_written;
    if selected_apsidium_index == 1 {
        selected_apsidium_ptr = magnetowake_value.as_ptr();
        selected_apsidium_written = magnetowake_value_written;
    } else if selected_apsidium_index == 2 {
        selected_apsidium_ptr = cryptbrace_value.as_ptr();
        selected_apsidium_written = cryptbrace_value_written;
    }

    let librationium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_apsidium_ptr,
            selected_apsidium_written as i64,
            librationium_old.as_ptr(),
            librationium_old.len() as i64,
            librationium_new.as_ptr(),
            librationium_new.len() as i64,
        )
    };
    let mut librationium_value = vec![0u8; librationium_value_len as usize];
    let librationium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_apsidium_ptr,
            selected_apsidium_written as i64,
            librationium_old.as_ptr(),
            librationium_old.len() as i64,
            librationium_new.as_ptr(),
            librationium_new.len() as i64,
            librationium_value.as_mut_ptr(),
            librationium_value.len() as i64,
        )
    };
    let librationium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            librationium_value.as_ptr(),
            librationium_value_written as i64,
            librationium_needle.as_ptr(),
            librationium_needle.len() as i64,
        )
    };

    let coronawake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_apsidium_ptr,
            selected_apsidium_written as i64,
            coronawake_extension.as_ptr(),
            coronawake_extension.len() as i64,
        )
    };
    let mut coronawake_value = vec![0u8; coronawake_value_len as usize];
    let coronawake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_apsidium_ptr,
            selected_apsidium_written as i64,
            coronawake_extension.as_ptr(),
            coronawake_extension.len() as i64,
            coronawake_value.as_mut_ptr(),
            coronawake_value.len() as i64,
        )
    };
    let coronawake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            coronawake_value.as_ptr(),
            coronawake_value_written as i64,
            coronawake_needle.as_ptr(),
            coronawake_needle.len() as i64,
        )
    };

    let trumeaubrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_apsidium_ptr,
            selected_apsidium_written as i64,
        )
    };
    let mut trumeaubrace_source = vec![0u8; trumeaubrace_source_len as usize];
    let trumeaubrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_apsidium_ptr,
            selected_apsidium_written as i64,
            trumeaubrace_source.as_mut_ptr(),
            trumeaubrace_source.len() as i64,
        )
    };
    let trumeaubrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            trumeaubrace_source.as_ptr(),
            trumeaubrace_source_written as i64,
            trumeaubrace_old.as_ptr(),
            trumeaubrace_old.len() as i64,
            trumeaubrace_new.as_ptr(),
            trumeaubrace_new.len() as i64,
        )
    };
    let mut trumeaubrace_value = vec![0u8; trumeaubrace_value_len as usize];
    let trumeaubrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            trumeaubrace_source.as_ptr(),
            trumeaubrace_source_written as i64,
            trumeaubrace_old.as_ptr(),
            trumeaubrace_old.len() as i64,
            trumeaubrace_new.as_ptr(),
            trumeaubrace_new.len() as i64,
            trumeaubrace_value.as_mut_ptr(),
            trumeaubrace_value.len() as i64,
        )
    };
    let trumeaubrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            trumeaubrace_value.as_ptr(),
            trumeaubrace_value_written as i64,
            trumeaubrace_needle.as_ptr(),
            trumeaubrace_needle.len() as i64,
        )
    };

    let mut selected_librationium_index = 0i32;
    let mut best_librationium_score = i32::MIN;
    let mut librationium_index = 0i32;
    while librationium_index < 3 {
        let mut candidate_librationium_score = librationium_value_written * 10 + librationium_contains * 50;
        if librationium_index == 1 {
            candidate_librationium_score = coronawake_value_written * 10 + coronawake_index;
        } else if librationium_index == 2 {
            candidate_librationium_score = trumeaubrace_value_written * 10 + trumeaubrace_contains * 50;
        }

        let mut librationium_bonus = 0i32;
        if librationium_index == selected_apsidium_index {
            librationium_bonus += 25;
        }
        if librationium_index == selected_inclinationium_index {
            librationium_bonus += 15;
        }
        if librationium_index == selected_anomalyium_index {
            librationium_bonus += 5;
        }
        if librationium_index == 0 && librationium_contains != 0 {
            librationium_bonus += 20;
        }
        if librationium_index == 1 && coronawake_index >= 0 {
            librationium_bonus += 10;
        }
        if librationium_index == 2 && trumeaubrace_contains != 0 {
            librationium_bonus += 30;
        }

        let librationium_score = candidate_librationium_score + librationium_bonus;
        if librationium_score > best_librationium_score {
            best_librationium_score = librationium_score;
            selected_librationium_index = librationium_index;
        }

        librationium_index += 1;
    }

    let mut selected_librationium_ptr = librationium_value.as_ptr();
    let mut selected_librationium_written = librationium_value_written;
    if selected_librationium_index == 1 {
        selected_librationium_ptr = coronawake_value.as_ptr();
        selected_librationium_written = coronawake_value_written;
    } else if selected_librationium_index == 2 {
        selected_librationium_ptr = trumeaubrace_value.as_ptr();
        selected_librationium_written = trumeaubrace_value_written;
    }

    let precessionium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_librationium_ptr,
            selected_librationium_written as i64,
            precessionium_old.as_ptr(),
            precessionium_old.len() as i64,
            precessionium_new.as_ptr(),
            precessionium_new.len() as i64,
        )
    };
    let mut precessionium_value = vec![0u8; precessionium_value_len as usize];
    let precessionium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_librationium_ptr,
            selected_librationium_written as i64,
            precessionium_old.as_ptr(),
            precessionium_old.len() as i64,
            precessionium_new.as_ptr(),
            precessionium_new.len() as i64,
            precessionium_value.as_mut_ptr(),
            precessionium_value.len() as i64,
        )
    };
    let precessionium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            precessionium_value.as_ptr(),
            precessionium_value_written as i64,
            precessionium_needle.as_ptr(),
            precessionium_needle.len() as i64,
        )
    };

    let heliowake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_librationium_ptr,
            selected_librationium_written as i64,
            heliowake_extension.as_ptr(),
            heliowake_extension.len() as i64,
        )
    };
    let mut heliowake_value = vec![0u8; heliowake_value_len as usize];
    let heliowake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_librationium_ptr,
            selected_librationium_written as i64,
            heliowake_extension.as_ptr(),
            heliowake_extension.len() as i64,
            heliowake_value.as_mut_ptr(),
            heliowake_value.len() as i64,
        )
    };
    let heliowake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            heliowake_value.as_ptr(),
            heliowake_value_written as i64,
            heliowake_needle.as_ptr(),
            heliowake_needle.len() as i64,
        )
    };

    let archivoltbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_librationium_ptr,
            selected_librationium_written as i64,
        )
    };
    let mut archivoltbrace_source = vec![0u8; archivoltbrace_source_len as usize];
    let archivoltbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_librationium_ptr,
            selected_librationium_written as i64,
            archivoltbrace_source.as_mut_ptr(),
            archivoltbrace_source.len() as i64,
        )
    };
    let archivoltbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            archivoltbrace_source.as_ptr(),
            archivoltbrace_source_written as i64,
            archivoltbrace_old.as_ptr(),
            archivoltbrace_old.len() as i64,
            archivoltbrace_new.as_ptr(),
            archivoltbrace_new.len() as i64,
        )
    };
    let mut archivoltbrace_value = vec![0u8; archivoltbrace_value_len as usize];
    let archivoltbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            archivoltbrace_source.as_ptr(),
            archivoltbrace_source_written as i64,
            archivoltbrace_old.as_ptr(),
            archivoltbrace_old.len() as i64,
            archivoltbrace_new.as_ptr(),
            archivoltbrace_new.len() as i64,
            archivoltbrace_value.as_mut_ptr(),
            archivoltbrace_value.len() as i64,
        )
    };
    let archivoltbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            archivoltbrace_value.as_ptr(),
            archivoltbrace_value_written as i64,
            archivoltbrace_needle.as_ptr(),
            archivoltbrace_needle.len() as i64,
        )
    };

    let mut selected_precessionium_index = 0i32;
    let mut best_precessionium_score = i32::MIN;
    let mut precessionium_index = 0i32;
    while precessionium_index < 3 {
        let mut candidate_precessionium_score = precessionium_value_written * 10 + precessionium_contains * 50;
        if precessionium_index == 1 {
            candidate_precessionium_score = heliowake_value_written * 10 + heliowake_index;
        } else if precessionium_index == 2 {
            candidate_precessionium_score = archivoltbrace_value_written * 10 + archivoltbrace_contains * 50;
        }

        let mut precessionium_bonus = 0i32;
        if precessionium_index == selected_librationium_index {
            precessionium_bonus += 25;
        }
        if precessionium_index == selected_apsidium_index {
            precessionium_bonus += 15;
        }
        if precessionium_index == selected_inclinationium_index {
            precessionium_bonus += 5;
        }
        if precessionium_index == 0 && precessionium_contains != 0 {
            precessionium_bonus += 20;
        }
        if precessionium_index == 1 && heliowake_index >= 0 {
            precessionium_bonus += 10;
        }
        if precessionium_index == 2 && archivoltbrace_contains != 0 {
            precessionium_bonus += 30;
        }

        let precessionium_score = candidate_precessionium_score + precessionium_bonus;
        if precessionium_score > best_precessionium_score {
            best_precessionium_score = precessionium_score;
            selected_precessionium_index = precessionium_index;
        }

        precessionium_index += 1;
    }

    let mut selected_precessionium_ptr = precessionium_value.as_ptr();
    let mut selected_precessionium_written = precessionium_value_written;
    if selected_precessionium_index == 1 {
        selected_precessionium_ptr = heliowake_value.as_ptr();
        selected_precessionium_written = heliowake_value_written;
    } else if selected_precessionium_index == 2 {
        selected_precessionium_ptr = archivoltbrace_value.as_ptr();
        selected_precessionium_written = archivoltbrace_value_written;
    }

    let nutationium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_precessionium_ptr,
            selected_precessionium_written as i64,
            nutationium_old.as_ptr(),
            nutationium_old.len() as i64,
            nutationium_new.as_ptr(),
            nutationium_new.len() as i64,
        )
    };
    let mut nutationium_value = vec![0u8; nutationium_value_len as usize];
    let nutationium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_precessionium_ptr,
            selected_precessionium_written as i64,
            nutationium_old.as_ptr(),
            nutationium_old.len() as i64,
            nutationium_new.as_ptr(),
            nutationium_new.len() as i64,
            nutationium_value.as_mut_ptr(),
            nutationium_value.len() as i64,
        )
    };
    let nutationium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            nutationium_value.as_ptr(),
            nutationium_value_written as i64,
            nutationium_needle.as_ptr(),
            nutationium_needle.len() as i64,
        )
    };

    let photowake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_precessionium_ptr,
            selected_precessionium_written as i64,
            photowake_extension.as_ptr(),
            photowake_extension.len() as i64,
        )
    };
    let mut photowake_value = vec![0u8; photowake_value_len as usize];
    let photowake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_precessionium_ptr,
            selected_precessionium_written as i64,
            photowake_extension.as_ptr(),
            photowake_extension.len() as i64,
            photowake_value.as_mut_ptr(),
            photowake_value.len() as i64,
        )
    };
    let photowake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            photowake_value.as_ptr(),
            photowake_value_written as i64,
            photowake_needle.as_ptr(),
            photowake_needle.len() as i64,
        )
    };

    let tympanumbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_precessionium_ptr,
            selected_precessionium_written as i64,
        )
    };
    let mut tympanumbrace_source = vec![0u8; tympanumbrace_source_len as usize];
    let tympanumbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_precessionium_ptr,
            selected_precessionium_written as i64,
            tympanumbrace_source.as_mut_ptr(),
            tympanumbrace_source.len() as i64,
        )
    };
    let tympanumbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            tympanumbrace_source.as_ptr(),
            tympanumbrace_source_written as i64,
            tympanumbrace_old.as_ptr(),
            tympanumbrace_old.len() as i64,
            tympanumbrace_new.as_ptr(),
            tympanumbrace_new.len() as i64,
        )
    };
    let mut tympanumbrace_value = vec![0u8; tympanumbrace_value_len as usize];
    let tympanumbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            tympanumbrace_source.as_ptr(),
            tympanumbrace_source_written as i64,
            tympanumbrace_old.as_ptr(),
            tympanumbrace_old.len() as i64,
            tympanumbrace_new.as_ptr(),
            tympanumbrace_new.len() as i64,
            tympanumbrace_value.as_mut_ptr(),
            tympanumbrace_value.len() as i64,
        )
    };
    let tympanumbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            tympanumbrace_value.as_ptr(),
            tympanumbrace_value_written as i64,
            tympanumbrace_needle.as_ptr(),
            tympanumbrace_needle.len() as i64,
        )
    };

    let mut selected_nutationium_index = 0i32;
    let mut best_nutationium_score = i32::MIN;
    let mut nutationium_index = 0i32;
    while nutationium_index < 3 {
        let mut candidate_nutationium_score = nutationium_value_written * 10 + nutationium_contains * 50;
        if nutationium_index == 1 {
            candidate_nutationium_score = photowake_value_written * 10 + photowake_index;
        } else if nutationium_index == 2 {
            candidate_nutationium_score = tympanumbrace_value_written * 10 + tympanumbrace_contains * 50;
        }

        let mut nutationium_bonus = 0i32;
        if nutationium_index == selected_precessionium_index {
            nutationium_bonus += 25;
        }
        if nutationium_index == selected_librationium_index {
            nutationium_bonus += 15;
        }
        if nutationium_index == selected_apsidium_index {
            nutationium_bonus += 5;
        }
        if nutationium_index == 0 && nutationium_contains != 0 {
            nutationium_bonus += 20;
        }
        if nutationium_index == 1 && photowake_index >= 0 {
            nutationium_bonus += 10;
        }
        if nutationium_index == 2 && tympanumbrace_contains != 0 {
            nutationium_bonus += 30;
        }

        let nutationium_score = candidate_nutationium_score + nutationium_bonus;
        if nutationium_score > best_nutationium_score {
            best_nutationium_score = nutationium_score;
            selected_nutationium_index = nutationium_index;
        }

        nutationium_index += 1;
    }

    let mut selected_nutationium_ptr = nutationium_value.as_ptr();
    let mut selected_nutationium_written = nutationium_value_written;
    if selected_nutationium_index == 1 {
        selected_nutationium_ptr = photowake_value.as_ptr();
        selected_nutationium_written = photowake_value_written;
    } else if selected_nutationium_index == 2 {
        selected_nutationium_ptr = tympanumbrace_value.as_ptr();
        selected_nutationium_written = tympanumbrace_value_written;
    }

    let osculationium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_nutationium_ptr,
            selected_nutationium_written as i64,
            osculationium_old.as_ptr(),
            osculationium_old.len() as i64,
            osculationium_new.as_ptr(),
            osculationium_new.len() as i64,
        )
    };
    let mut osculationium_value = vec![0u8; osculationium_value_len as usize];
    let osculationium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_nutationium_ptr,
            selected_nutationium_written as i64,
            osculationium_old.as_ptr(),
            osculationium_old.len() as i64,
            osculationium_new.as_ptr(),
            osculationium_new.len() as i64,
            osculationium_value.as_mut_ptr(),
            osculationium_value.len() as i64,
        )
    };
    let osculationium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            osculationium_value.as_ptr(),
            osculationium_value_written as i64,
            osculationium_needle.as_ptr(),
            osculationium_needle.len() as i64,
        )
    };

    let spectrawake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_nutationium_ptr,
            selected_nutationium_written as i64,
            spectrawake_extension.as_ptr(),
            spectrawake_extension.len() as i64,
        )
    };
    let mut spectrawake_value = vec![0u8; spectrawake_value_len as usize];
    let spectrawake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_nutationium_ptr,
            selected_nutationium_written as i64,
            spectrawake_extension.as_ptr(),
            spectrawake_extension.len() as i64,
            spectrawake_value.as_mut_ptr(),
            spectrawake_value.len() as i64,
        )
    };
    let spectrawake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            spectrawake_value.as_ptr(),
            spectrawake_value_written as i64,
            spectrawake_needle.as_ptr(),
            spectrawake_needle.len() as i64,
        )
    };

    let lunettebrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_nutationium_ptr,
            selected_nutationium_written as i64,
        )
    };
    let mut lunettebrace_source = vec![0u8; lunettebrace_source_len as usize];
    let lunettebrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_nutationium_ptr,
            selected_nutationium_written as i64,
            lunettebrace_source.as_mut_ptr(),
            lunettebrace_source.len() as i64,
        )
    };
    let lunettebrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            lunettebrace_source.as_ptr(),
            lunettebrace_source_written as i64,
            lunettebrace_old.as_ptr(),
            lunettebrace_old.len() as i64,
            lunettebrace_new.as_ptr(),
            lunettebrace_new.len() as i64,
        )
    };
    let mut lunettebrace_value = vec![0u8; lunettebrace_value_len as usize];
    let lunettebrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            lunettebrace_source.as_ptr(),
            lunettebrace_source_written as i64,
            lunettebrace_old.as_ptr(),
            lunettebrace_old.len() as i64,
            lunettebrace_new.as_ptr(),
            lunettebrace_new.len() as i64,
            lunettebrace_value.as_mut_ptr(),
            lunettebrace_value.len() as i64,
        )
    };
    let lunettebrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            lunettebrace_value.as_ptr(),
            lunettebrace_value_written as i64,
            lunettebrace_needle.as_ptr(),
            lunettebrace_needle.len() as i64,
        )
    };

    let mut selected_osculationium_index = 0i32;
    let mut best_osculationium_score = i32::MIN;
    let mut osculationium_index = 0i32;
    while osculationium_index < 3 {
        let mut candidate_osculationium_score = osculationium_value_written * 10 + osculationium_contains * 50;
        if osculationium_index == 1 {
            candidate_osculationium_score = spectrawake_value_written * 10 + spectrawake_index;
        } else if osculationium_index == 2 {
            candidate_osculationium_score = lunettebrace_value_written * 10 + lunettebrace_contains * 50;
        }

        let mut osculationium_bonus = 0i32;
        if osculationium_index == selected_nutationium_index {
            osculationium_bonus += 25;
        }
        if osculationium_index == selected_precessionium_index {
            osculationium_bonus += 15;
        }
        if osculationium_index == selected_librationium_index {
            osculationium_bonus += 5;
        }
        if osculationium_index == 0 && osculationium_contains != 0 {
            osculationium_bonus += 20;
        }
        if osculationium_index == 1 && spectrawake_index >= 0 {
            osculationium_bonus += 10;
        }
        if osculationium_index == 2 && lunettebrace_contains != 0 {
            osculationium_bonus += 30;
        }

        let osculationium_score = candidate_osculationium_score + osculationium_bonus;
        if osculationium_score > best_osculationium_score {
            best_osculationium_score = osculationium_score;
            selected_osculationium_index = osculationium_index;
        }

        osculationium_index += 1;
    }

    let mut selected_osculationium_ptr = osculationium_value.as_ptr();
    let mut selected_osculationium_written = osculationium_value_written;
    if selected_osculationium_index == 1 {
        selected_osculationium_ptr = spectrawake_value.as_ptr();
        selected_osculationium_written = spectrawake_value_written;
    } else if selected_osculationium_index == 2 {
        selected_osculationium_ptr = lunettebrace_value.as_ptr();
        selected_osculationium_written = lunettebrace_value_written;
    }

    let apselineium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_osculationium_ptr,
            selected_osculationium_written as i64,
            apselineium_old.as_ptr(),
            apselineium_old.len() as i64,
            apselineium_new.as_ptr(),
            apselineium_new.len() as i64,
        )
    };
    let mut apselineium_value = vec![0u8; apselineium_value_len as usize];
    let apselineium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_osculationium_ptr,
            selected_osculationium_written as i64,
            apselineium_old.as_ptr(),
            apselineium_old.len() as i64,
            apselineium_new.as_ptr(),
            apselineium_new.len() as i64,
            apselineium_value.as_mut_ptr(),
            apselineium_value.len() as i64,
        )
    };
    let apselineium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            apselineium_value.as_ptr(),
            apselineium_value_written as i64,
            apselineium_needle.as_ptr(),
            apselineium_needle.len() as i64,
        )
    };

    let radiowake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_osculationium_ptr,
            selected_osculationium_written as i64,
            radiowake_extension.as_ptr(),
            radiowake_extension.len() as i64,
        )
    };
    let mut radiowake_value = vec![0u8; radiowake_value_len as usize];
    let radiowake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_osculationium_ptr,
            selected_osculationium_written as i64,
            radiowake_extension.as_ptr(),
            radiowake_extension.len() as i64,
            radiowake_value.as_mut_ptr(),
            radiowake_value.len() as i64,
        )
    };
    let radiowake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            radiowake_value.as_ptr(),
            radiowake_value_written as i64,
            radiowake_needle.as_ptr(),
            radiowake_needle.len() as i64,
        )
    };

    let voussurebrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_osculationium_ptr,
            selected_osculationium_written as i64,
        )
    };
    let mut voussurebrace_source = vec![0u8; voussurebrace_source_len as usize];
    let voussurebrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_osculationium_ptr,
            selected_osculationium_written as i64,
            voussurebrace_source.as_mut_ptr(),
            voussurebrace_source.len() as i64,
        )
    };
    let voussurebrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            voussurebrace_source.as_ptr(),
            voussurebrace_source_written as i64,
            voussurebrace_old.as_ptr(),
            voussurebrace_old.len() as i64,
            voussurebrace_new.as_ptr(),
            voussurebrace_new.len() as i64,
        )
    };
    let mut voussurebrace_value = vec![0u8; voussurebrace_value_len as usize];
    let voussurebrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            voussurebrace_source.as_ptr(),
            voussurebrace_source_written as i64,
            voussurebrace_old.as_ptr(),
            voussurebrace_old.len() as i64,
            voussurebrace_new.as_ptr(),
            voussurebrace_new.len() as i64,
            voussurebrace_value.as_mut_ptr(),
            voussurebrace_value.len() as i64,
        )
    };
    let voussurebrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            voussurebrace_value.as_ptr(),
            voussurebrace_value_written as i64,
            voussurebrace_needle.as_ptr(),
            voussurebrace_needle.len() as i64,
        )
    };

    let mut selected_apselineium_index = 0i32;
    let mut best_apselineium_score = i32::MIN;
    let mut apselineium_index = 0i32;
    while apselineium_index < 3 {
        let mut candidate_apselineium_score = apselineium_value_written * 10 + apselineium_contains * 50;
        if apselineium_index == 1 {
            candidate_apselineium_score = radiowake_value_written * 10 + radiowake_index;
        } else if apselineium_index == 2 {
            candidate_apselineium_score = voussurebrace_value_written * 10 + voussurebrace_contains * 50;
        }

        let mut apselineium_bonus = 0i32;
        if apselineium_index == selected_osculationium_index {
            apselineium_bonus += 25;
        }
        if apselineium_index == selected_nutationium_index {
            apselineium_bonus += 15;
        }
        if apselineium_index == selected_precessionium_index {
            apselineium_bonus += 5;
        }
        if apselineium_index == 0 && apselineium_contains != 0 {
            apselineium_bonus += 20;
        }
        if apselineium_index == 1 && radiowake_index >= 0 {
            apselineium_bonus += 10;
        }
        if apselineium_index == 2 && voussurebrace_contains != 0 {
            apselineium_bonus += 30;
        }

        let apselineium_score = candidate_apselineium_score + apselineium_bonus;
        if apselineium_score > best_apselineium_score {
            best_apselineium_score = apselineium_score;
            selected_apselineium_index = apselineium_index;
        }

        apselineium_index += 1;
    }

    let mut selected_apselineium_ptr = apselineium_value.as_ptr();
    let mut selected_apselineium_written = apselineium_value_written;
    if selected_apselineium_index == 1 {
        selected_apselineium_ptr = radiowake_value.as_ptr();
        selected_apselineium_written = radiowake_value_written;
    } else if selected_apselineium_index == 2 {
        selected_apselineium_ptr = voussurebrace_value.as_ptr();
        selected_apselineium_written = voussurebrace_value_written;
    }

    let periastronium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_apselineium_ptr,
            selected_apselineium_written as i64,
            periastronium_old.as_ptr(),
            periastronium_old.len() as i64,
            periastronium_new.as_ptr(),
            periastronium_new.len() as i64,
        )
    };
    let mut periastronium_value = vec![0u8; periastronium_value_len as usize];
    let periastronium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_apselineium_ptr,
            selected_apselineium_written as i64,
            periastronium_old.as_ptr(),
            periastronium_old.len() as i64,
            periastronium_new.as_ptr(),
            periastronium_new.len() as i64,
            periastronium_value.as_mut_ptr(),
            periastronium_value.len() as i64,
        )
    };
    let periastronium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            periastronium_value.as_ptr(),
            periastronium_value_written as i64,
            periastronium_needle.as_ptr(),
            periastronium_needle.len() as i64,
        )
    };

    let ionowake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_apselineium_ptr,
            selected_apselineium_written as i64,
            ionowake_extension.as_ptr(),
            ionowake_extension.len() as i64,
        )
    };
    let mut ionowake_value = vec![0u8; ionowake_value_len as usize];
    let ionowake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_apselineium_ptr,
            selected_apselineium_written as i64,
            ionowake_extension.as_ptr(),
            ionowake_extension.len() as i64,
            ionowake_value.as_mut_ptr(),
            ionowake_value.len() as i64,
        )
    };
    let ionowake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            ionowake_value.as_ptr(),
            ionowake_value_written as i64,
            ionowake_needle.as_ptr(),
            ionowake_needle.len() as i64,
        )
    };

    let pendentivebrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_apselineium_ptr,
            selected_apselineium_written as i64,
        )
    };
    let mut pendentivebrace_source = vec![0u8; pendentivebrace_source_len as usize];
    let pendentivebrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_apselineium_ptr,
            selected_apselineium_written as i64,
            pendentivebrace_source.as_mut_ptr(),
            pendentivebrace_source.len() as i64,
        )
    };
    let pendentivebrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            pendentivebrace_source.as_ptr(),
            pendentivebrace_source_written as i64,
            pendentivebrace_old.as_ptr(),
            pendentivebrace_old.len() as i64,
            pendentivebrace_new.as_ptr(),
            pendentivebrace_new.len() as i64,
        )
    };
    let mut pendentivebrace_value = vec![0u8; pendentivebrace_value_len as usize];
    let pendentivebrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            pendentivebrace_source.as_ptr(),
            pendentivebrace_source_written as i64,
            pendentivebrace_old.as_ptr(),
            pendentivebrace_old.len() as i64,
            pendentivebrace_new.as_ptr(),
            pendentivebrace_new.len() as i64,
            pendentivebrace_value.as_mut_ptr(),
            pendentivebrace_value.len() as i64,
        )
    };
    let pendentivebrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            pendentivebrace_value.as_ptr(),
            pendentivebrace_value_written as i64,
            pendentivebrace_needle.as_ptr(),
            pendentivebrace_needle.len() as i64,
        )
    };

    let mut selected_periastronium_index = 0i32;
    let mut best_periastronium_score = i32::MIN;
    let mut periastronium_index = 0i32;
    while periastronium_index < 3 {
        let mut candidate_periastronium_score = periastronium_value_written * 10 + periastronium_contains * 50;
        if periastronium_index == 1 {
            candidate_periastronium_score = ionowake_value_written * 10 + ionowake_index;
        } else if periastronium_index == 2 {
            candidate_periastronium_score = pendentivebrace_value_written * 10 + pendentivebrace_contains * 50;
        }

        let mut periastronium_bonus = 0i32;
        if periastronium_index == selected_apselineium_index {
            periastronium_bonus += 25;
        }
        if periastronium_index == selected_osculationium_index {
            periastronium_bonus += 15;
        }
        if periastronium_index == selected_nutationium_index {
            periastronium_bonus += 5;
        }
        if periastronium_index == 0 && periastronium_contains != 0 {
            periastronium_bonus += 20;
        }
        if periastronium_index == 1 && ionowake_index >= 0 {
            periastronium_bonus += 10;
        }
        if periastronium_index == 2 && pendentivebrace_contains != 0 {
            periastronium_bonus += 30;
        }

        let periastronium_score = candidate_periastronium_score + periastronium_bonus;
        if periastronium_score > best_periastronium_score {
            best_periastronium_score = periastronium_score;
            selected_periastronium_index = periastronium_index;
        }

        periastronium_index += 1;
    }

    let mut selected_periastronium_ptr = periastronium_value.as_ptr();
    let mut selected_periastronium_written = periastronium_value_written;
    if selected_periastronium_index == 1 {
        selected_periastronium_ptr = ionowake_value.as_ptr();
        selected_periastronium_written = ionowake_value_written;
    } else if selected_periastronium_index == 2 {
        selected_periastronium_ptr = pendentivebrace_value.as_ptr();
        selected_periastronium_written = pendentivebrace_value_written;
    }

    let barycentrium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_periastronium_ptr,
            selected_periastronium_written as i64,
            barycentrium_old.as_ptr(),
            barycentrium_old.len() as i64,
            barycentrium_new.as_ptr(),
            barycentrium_new.len() as i64,
        )
    };
    let mut barycentrium_value = vec![0u8; barycentrium_value_len as usize];
    let barycentrium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_periastronium_ptr,
            selected_periastronium_written as i64,
            barycentrium_old.as_ptr(),
            barycentrium_old.len() as i64,
            barycentrium_new.as_ptr(),
            barycentrium_new.len() as i64,
            barycentrium_value.as_mut_ptr(),
            barycentrium_value.len() as i64,
        )
    };
    let barycentrium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            barycentrium_value.as_ptr(),
            barycentrium_value_written as i64,
            barycentrium_needle.as_ptr(),
            barycentrium_needle.len() as i64,
        )
    };

    let plasmawake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_periastronium_ptr,
            selected_periastronium_written as i64,
            plasmawake_extension.as_ptr(),
            plasmawake_extension.len() as i64,
        )
    };
    let mut plasmawake_value = vec![0u8; plasmawake_value_len as usize];
    let plasmawake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_periastronium_ptr,
            selected_periastronium_written as i64,
            plasmawake_extension.as_ptr(),
            plasmawake_extension.len() as i64,
            plasmawake_value.as_mut_ptr(),
            plasmawake_value.len() as i64,
        )
    };
    let plasmawake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            plasmawake_value.as_ptr(),
            plasmawake_value_written as i64,
            plasmawake_needle.as_ptr(),
            plasmawake_needle.len() as i64,
        )
    };

    let squinchbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_periastronium_ptr,
            selected_periastronium_written as i64,
        )
    };
    let mut squinchbrace_source = vec![0u8; squinchbrace_source_len as usize];
    let squinchbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_periastronium_ptr,
            selected_periastronium_written as i64,
            squinchbrace_source.as_mut_ptr(),
            squinchbrace_source.len() as i64,
        )
    };
    let squinchbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            squinchbrace_source.as_ptr(),
            squinchbrace_source_written as i64,
            squinchbrace_old.as_ptr(),
            squinchbrace_old.len() as i64,
            squinchbrace_new.as_ptr(),
            squinchbrace_new.len() as i64,
        )
    };
    let mut squinchbrace_value = vec![0u8; squinchbrace_value_len as usize];
    let squinchbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            squinchbrace_source.as_ptr(),
            squinchbrace_source_written as i64,
            squinchbrace_old.as_ptr(),
            squinchbrace_old.len() as i64,
            squinchbrace_new.as_ptr(),
            squinchbrace_new.len() as i64,
            squinchbrace_value.as_mut_ptr(),
            squinchbrace_value.len() as i64,
        )
    };
    let squinchbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            squinchbrace_value.as_ptr(),
            squinchbrace_value_written as i64,
            squinchbrace_needle.as_ptr(),
            squinchbrace_needle.len() as i64,
        )
    };

    let mut selected_barycentrium_index = 0i32;
    let mut best_barycentrium_score = i32::MIN;
    let mut barycentrium_index = 0i32;
    while barycentrium_index < 3 {
        let mut candidate_barycentrium_score = barycentrium_value_written * 10 + barycentrium_contains * 50;
        if barycentrium_index == 1 {
            candidate_barycentrium_score = plasmawake_value_written * 10 + plasmawake_index;
        } else if barycentrium_index == 2 {
            candidate_barycentrium_score = squinchbrace_value_written * 10 + squinchbrace_contains * 50;
        }

        let mut barycentrium_bonus = 0i32;
        if barycentrium_index == selected_periastronium_index {
            barycentrium_bonus += 25;
        }
        if barycentrium_index == selected_apselineium_index {
            barycentrium_bonus += 15;
        }
        if barycentrium_index == selected_osculationium_index {
            barycentrium_bonus += 5;
        }
        if barycentrium_index == 0 && barycentrium_contains != 0 {
            barycentrium_bonus += 20;
        }
        if barycentrium_index == 1 && plasmawake_index >= 0 {
            barycentrium_bonus += 10;
        }
        if barycentrium_index == 2 && squinchbrace_contains != 0 {
            barycentrium_bonus += 30;
        }

        let barycentrium_score = candidate_barycentrium_score + barycentrium_bonus;
        if barycentrium_score > best_barycentrium_score {
            best_barycentrium_score = barycentrium_score;
            selected_barycentrium_index = barycentrium_index;
        }

        barycentrium_index += 1;
    }

    let mut selected_barycentrium_ptr = barycentrium_value.as_ptr();
    let mut selected_barycentrium_written = barycentrium_value_written;
    if selected_barycentrium_index == 1 {
        selected_barycentrium_ptr = plasmawake_value.as_ptr();
        selected_barycentrium_written = plasmawake_value_written;
    } else if selected_barycentrium_index == 2 {
        selected_barycentrium_ptr = squinchbrace_value.as_ptr();
        selected_barycentrium_written = squinchbrace_value_written;
    }

    let rochelium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_barycentrium_ptr,
            selected_barycentrium_written as i64,
            rochelium_old.as_ptr(),
            rochelium_old.len() as i64,
            rochelium_new.as_ptr(),
            rochelium_new.len() as i64,
        )
    };
    let mut rochelium_value = vec![0u8; rochelium_value_len as usize];
    let rochelium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_barycentrium_ptr,
            selected_barycentrium_written as i64,
            rochelium_old.as_ptr(),
            rochelium_old.len() as i64,
            rochelium_new.as_ptr(),
            rochelium_new.len() as i64,
            rochelium_value.as_mut_ptr(),
            rochelium_value.len() as i64,
        )
    };
    let rochelium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            rochelium_value.as_ptr(),
            rochelium_value_written as i64,
            rochelium_needle.as_ptr(),
            rochelium_needle.len() as i64,
        )
    };

    let neutrinowake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_barycentrium_ptr,
            selected_barycentrium_written as i64,
            neutrinowake_extension.as_ptr(),
            neutrinowake_extension.len() as i64,
        )
    };
    let mut neutrinowake_value = vec![0u8; neutrinowake_value_len as usize];
    let neutrinowake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_barycentrium_ptr,
            selected_barycentrium_written as i64,
            neutrinowake_extension.as_ptr(),
            neutrinowake_extension.len() as i64,
            neutrinowake_value.as_mut_ptr(),
            neutrinowake_value.len() as i64,
        )
    };
    let neutrinowake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            neutrinowake_value.as_ptr(),
            neutrinowake_value_written as i64,
            neutrinowake_needle.as_ptr(),
            neutrinowake_needle.len() as i64,
        )
    };

    let rusticationbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_barycentrium_ptr,
            selected_barycentrium_written as i64,
        )
    };
    let mut rusticationbrace_source = vec![0u8; rusticationbrace_source_len as usize];
    let rusticationbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_barycentrium_ptr,
            selected_barycentrium_written as i64,
            rusticationbrace_source.as_mut_ptr(),
            rusticationbrace_source.len() as i64,
        )
    };
    let rusticationbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            rusticationbrace_source.as_ptr(),
            rusticationbrace_source_written as i64,
            rusticationbrace_old.as_ptr(),
            rusticationbrace_old.len() as i64,
            rusticationbrace_new.as_ptr(),
            rusticationbrace_new.len() as i64,
        )
    };
    let mut rusticationbrace_value = vec![0u8; rusticationbrace_value_len as usize];
    let rusticationbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            rusticationbrace_source.as_ptr(),
            rusticationbrace_source_written as i64,
            rusticationbrace_old.as_ptr(),
            rusticationbrace_old.len() as i64,
            rusticationbrace_new.as_ptr(),
            rusticationbrace_new.len() as i64,
            rusticationbrace_value.as_mut_ptr(),
            rusticationbrace_value.len() as i64,
        )
    };
    let rusticationbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            rusticationbrace_value.as_ptr(),
            rusticationbrace_value_written as i64,
            rusticationbrace_needle.as_ptr(),
            rusticationbrace_needle.len() as i64,
        )
    };

    let mut selected_rochelium_index = 0i32;
    let mut best_rochelium_score = i32::MIN;
    let mut rochelium_index = 0i32;
    while rochelium_index < 3 {
        let mut candidate_rochelium_score = rochelium_value_written * 10 + rochelium_contains * 50;
        if rochelium_index == 1 {
            candidate_rochelium_score = neutrinowake_value_written * 10 + neutrinowake_index;
        } else if rochelium_index == 2 {
            candidate_rochelium_score = rusticationbrace_value_written * 10 + rusticationbrace_contains * 50;
        }

        let mut rochelium_bonus = 0i32;
        if rochelium_index == selected_barycentrium_index {
            rochelium_bonus += 25;
        }
        if rochelium_index == selected_periastronium_index {
            rochelium_bonus += 15;
        }
        if rochelium_index == selected_apselineium_index {
            rochelium_bonus += 5;
        }
        if rochelium_index == 0 && rochelium_contains != 0 {
            rochelium_bonus += 20;
        }
        if rochelium_index == 1 && neutrinowake_index >= 0 {
            rochelium_bonus += 10;
        }
        if rochelium_index == 2 && rusticationbrace_contains != 0 {
            rochelium_bonus += 30;
        }

        let rochelium_score = candidate_rochelium_score + rochelium_bonus;
        if rochelium_score > best_rochelium_score {
            best_rochelium_score = rochelium_score;
            selected_rochelium_index = rochelium_index;
        }

        rochelium_index += 1;
    }

    let mut selected_rochelium_ptr = rochelium_value.as_ptr();
    let mut selected_rochelium_written = rochelium_value_written;
    if selected_rochelium_index == 1 {
        selected_rochelium_ptr = neutrinowake_value.as_ptr();
        selected_rochelium_written = neutrinowake_value_written;
    } else if selected_rochelium_index == 2 {
        selected_rochelium_ptr = rusticationbrace_value.as_ptr();
        selected_rochelium_written = rusticationbrace_value_written;
    }

    let lissajousium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_rochelium_ptr,
            selected_rochelium_written as i64,
            lissajousium_old.as_ptr(),
            lissajousium_old.len() as i64,
            lissajousium_new.as_ptr(),
            lissajousium_new.len() as i64,
        )
    };
    let mut lissajousium_value = vec![0u8; lissajousium_value_len as usize];
    let lissajousium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_rochelium_ptr,
            selected_rochelium_written as i64,
            lissajousium_old.as_ptr(),
            lissajousium_old.len() as i64,
            lissajousium_new.as_ptr(),
            lissajousium_new.len() as i64,
            lissajousium_value.as_mut_ptr(),
            lissajousium_value.len() as i64,
        )
    };
    let lissajousium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            lissajousium_value.as_ptr(),
            lissajousium_value_written as i64,
            lissajousium_needle.as_ptr(),
            lissajousium_needle.len() as i64,
        )
    };

    let tachyonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_rochelium_ptr,
            selected_rochelium_written as i64,
            tachyonwake_extension.as_ptr(),
            tachyonwake_extension.len() as i64,
        )
    };
    let mut tachyonwake_value = vec![0u8; tachyonwake_value_len as usize];
    let tachyonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_rochelium_ptr,
            selected_rochelium_written as i64,
            tachyonwake_extension.as_ptr(),
            tachyonwake_extension.len() as i64,
            tachyonwake_value.as_mut_ptr(),
            tachyonwake_value.len() as i64,
        )
    };
    let tachyonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            tachyonwake_value.as_ptr(),
            tachyonwake_value_written as i64,
            tachyonwake_needle.as_ptr(),
            tachyonwake_needle.len() as i64,
        )
    };

    let ashlarbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_rochelium_ptr,
            selected_rochelium_written as i64,
        )
    };
    let mut ashlarbrace_source = vec![0u8; ashlarbrace_source_len as usize];
    let ashlarbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_rochelium_ptr,
            selected_rochelium_written as i64,
            ashlarbrace_source.as_mut_ptr(),
            ashlarbrace_source.len() as i64,
        )
    };
    let ashlarbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            ashlarbrace_source.as_ptr(),
            ashlarbrace_source_written as i64,
            ashlarbrace_old.as_ptr(),
            ashlarbrace_old.len() as i64,
            ashlarbrace_new.as_ptr(),
            ashlarbrace_new.len() as i64,
        )
    };
    let mut ashlarbrace_value = vec![0u8; ashlarbrace_value_len as usize];
    let ashlarbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            ashlarbrace_source.as_ptr(),
            ashlarbrace_source_written as i64,
            ashlarbrace_old.as_ptr(),
            ashlarbrace_old.len() as i64,
            ashlarbrace_new.as_ptr(),
            ashlarbrace_new.len() as i64,
            ashlarbrace_value.as_mut_ptr(),
            ashlarbrace_value.len() as i64,
        )
    };
    let ashlarbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            ashlarbrace_value.as_ptr(),
            ashlarbrace_value_written as i64,
            ashlarbrace_needle.as_ptr(),
            ashlarbrace_needle.len() as i64,
        )
    };

    let mut selected_lissajousium_index = 0i32;
    let mut best_lissajousium_score = i32::MIN;
    let mut lissajousium_index = 0i32;
    while lissajousium_index < 3 {
        let mut candidate_lissajousium_score = lissajousium_value_written * 10 + lissajousium_contains * 50;
        if lissajousium_index == 1 {
            candidate_lissajousium_score = tachyonwake_value_written * 10 + tachyonwake_index;
        } else if lissajousium_index == 2 {
            candidate_lissajousium_score = ashlarbrace_value_written * 10 + ashlarbrace_contains * 50;
        }

        let mut lissajousium_bonus = 0i32;
        if lissajousium_index == selected_rochelium_index {
            lissajousium_bonus += 25;
        }
        if lissajousium_index == selected_barycentrium_index {
            lissajousium_bonus += 15;
        }
        if lissajousium_index == selected_periastronium_index {
            lissajousium_bonus += 5;
        }
        if lissajousium_index == 0 && lissajousium_contains != 0 {
            lissajousium_bonus += 20;
        }
        if lissajousium_index == 1 && tachyonwake_index >= 0 {
            lissajousium_bonus += 10;
        }
        if lissajousium_index == 2 && ashlarbrace_contains != 0 {
            lissajousium_bonus += 30;
        }

        let lissajousium_score = candidate_lissajousium_score + lissajousium_bonus;
        if lissajousium_score > best_lissajousium_score {
            best_lissajousium_score = lissajousium_score;
            selected_lissajousium_index = lissajousium_index;
        }

        lissajousium_index += 1;
    }

    let mut selected_lissajousium_ptr = lissajousium_value.as_ptr();
    let mut selected_lissajousium_written = lissajousium_value_written;
    if selected_lissajousium_index == 1 {
        selected_lissajousium_ptr = tachyonwake_value.as_ptr();
        selected_lissajousium_written = tachyonwake_value_written;
    } else if selected_lissajousium_index == 2 {
        selected_lissajousium_ptr = ashlarbrace_value.as_ptr();
        selected_lissajousium_written = ashlarbrace_value_written;
    }

    let cycloidium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_lissajousium_ptr,
            selected_lissajousium_written as i64,
            cycloidium_old.as_ptr(),
            cycloidium_old.len() as i64,
            cycloidium_new.as_ptr(),
            cycloidium_new.len() as i64,
        )
    };
    let mut cycloidium_value = vec![0u8; cycloidium_value_len as usize];
    let cycloidium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_lissajousium_ptr,
            selected_lissajousium_written as i64,
            cycloidium_old.as_ptr(),
            cycloidium_old.len() as i64,
            cycloidium_new.as_ptr(),
            cycloidium_new.len() as i64,
            cycloidium_value.as_mut_ptr(),
            cycloidium_value.len() as i64,
        )
    };
    let cycloidium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            cycloidium_value.as_ptr(),
            cycloidium_value_written as i64,
            cycloidium_needle.as_ptr(),
            cycloidium_needle.len() as i64,
        )
    };

    let gravitonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_lissajousium_ptr,
            selected_lissajousium_written as i64,
            gravitonwake_extension.as_ptr(),
            gravitonwake_extension.len() as i64,
        )
    };
    let mut gravitonwake_value = vec![0u8; gravitonwake_value_len as usize];
    let gravitonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_lissajousium_ptr,
            selected_lissajousium_written as i64,
            gravitonwake_extension.as_ptr(),
            gravitonwake_extension.len() as i64,
            gravitonwake_value.as_mut_ptr(),
            gravitonwake_value.len() as i64,
        )
    };
    let gravitonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            gravitonwake_value.as_ptr(),
            gravitonwake_value_written as i64,
            gravitonwake_needle.as_ptr(),
            gravitonwake_needle.len() as i64,
        )
    };

    let quoinsbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_lissajousium_ptr,
            selected_lissajousium_written as i64,
        )
    };
    let mut quoinsbrace_source = vec![0u8; quoinsbrace_source_len as usize];
    let quoinsbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_lissajousium_ptr,
            selected_lissajousium_written as i64,
            quoinsbrace_source.as_mut_ptr(),
            quoinsbrace_source.len() as i64,
        )
    };
    let quoinsbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            quoinsbrace_source.as_ptr(),
            quoinsbrace_source_written as i64,
            quoinsbrace_old.as_ptr(),
            quoinsbrace_old.len() as i64,
            quoinsbrace_new.as_ptr(),
            quoinsbrace_new.len() as i64,
        )
    };
    let mut quoinsbrace_value = vec![0u8; quoinsbrace_value_len as usize];
    let quoinsbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            quoinsbrace_source.as_ptr(),
            quoinsbrace_source_written as i64,
            quoinsbrace_old.as_ptr(),
            quoinsbrace_old.len() as i64,
            quoinsbrace_new.as_ptr(),
            quoinsbrace_new.len() as i64,
            quoinsbrace_value.as_mut_ptr(),
            quoinsbrace_value.len() as i64,
        )
    };
    let quoinsbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            quoinsbrace_value.as_ptr(),
            quoinsbrace_value_written as i64,
            quoinsbrace_needle.as_ptr(),
            quoinsbrace_needle.len() as i64,
        )
    };

    let mut selected_cycloidium_index = 0i32;
    let mut best_cycloidium_score = i32::MIN;
    let mut cycloidium_index = 0i32;
    while cycloidium_index < 3 {
        let mut candidate_cycloidium_score = cycloidium_value_written * 10 + cycloidium_contains * 50;
        if cycloidium_index == 1 {
            candidate_cycloidium_score = gravitonwake_value_written * 10 + gravitonwake_index;
        } else if cycloidium_index == 2 {
            candidate_cycloidium_score = quoinsbrace_value_written * 10 + quoinsbrace_contains * 50;
        }

        let mut cycloidium_bonus = 0i32;
        if cycloidium_index == selected_lissajousium_index {
            cycloidium_bonus += 25;
        }
        if cycloidium_index == selected_rochelium_index {
            cycloidium_bonus += 15;
        }
        if cycloidium_index == selected_barycentrium_index {
            cycloidium_bonus += 5;
        }
        if cycloidium_index == 0 && cycloidium_contains != 0 {
            cycloidium_bonus += 20;
        }
        if cycloidium_index == 1 && gravitonwake_index >= 0 {
            cycloidium_bonus += 10;
        }
        if cycloidium_index == 2 && quoinsbrace_contains != 0 {
            cycloidium_bonus += 30;
        }

        let cycloidium_score = candidate_cycloidium_score + cycloidium_bonus;
        if cycloidium_score > best_cycloidium_score {
            best_cycloidium_score = cycloidium_score;
            selected_cycloidium_index = cycloidium_index;
        }

        cycloidium_index += 1;
    }

    let mut selected_cycloidium_ptr = cycloidium_value.as_ptr();
    let mut selected_cycloidium_written = cycloidium_value_written;
    if selected_cycloidium_index == 1 {
        selected_cycloidium_ptr = gravitonwake_value.as_ptr();
        selected_cycloidium_written = gravitonwake_value_written;
    } else if selected_cycloidium_index == 2 {
        selected_cycloidium_ptr = quoinsbrace_value.as_ptr();
        selected_cycloidium_written = quoinsbrace_value_written;
    }

    let hypotrochoidium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_cycloidium_ptr,
            selected_cycloidium_written as i64,
            hypotrochoidium_old.as_ptr(),
            hypotrochoidium_old.len() as i64,
            hypotrochoidium_new.as_ptr(),
            hypotrochoidium_new.len() as i64,
        )
    };
    let mut hypotrochoidium_value = vec![0u8; hypotrochoidium_value_len as usize];
    let hypotrochoidium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_cycloidium_ptr,
            selected_cycloidium_written as i64,
            hypotrochoidium_old.as_ptr(),
            hypotrochoidium_old.len() as i64,
            hypotrochoidium_new.as_ptr(),
            hypotrochoidium_new.len() as i64,
            hypotrochoidium_value.as_mut_ptr(),
            hypotrochoidium_value.len() as i64,
        )
    };
    let hypotrochoidium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            hypotrochoidium_value.as_ptr(),
            hypotrochoidium_value_written as i64,
            hypotrochoidium_needle.as_ptr(),
            hypotrochoidium_needle.len() as i64,
        )
    };

    let bosonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_cycloidium_ptr,
            selected_cycloidium_written as i64,
            bosonwake_extension.as_ptr(),
            bosonwake_extension.len() as i64,
        )
    };
    let mut bosonwake_value = vec![0u8; bosonwake_value_len as usize];
    let bosonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_cycloidium_ptr,
            selected_cycloidium_written as i64,
            bosonwake_extension.as_ptr(),
            bosonwake_extension.len() as i64,
            bosonwake_value.as_mut_ptr(),
            bosonwake_value.len() as i64,
        )
    };
    let bosonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            bosonwake_value.as_ptr(),
            bosonwake_value_written as i64,
            bosonwake_needle.as_ptr(),
            bosonwake_needle.len() as i64,
        )
    };

    let masonrybrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_cycloidium_ptr,
            selected_cycloidium_written as i64,
        )
    };
    let mut masonrybrace_source = vec![0u8; masonrybrace_source_len as usize];
    let masonrybrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_cycloidium_ptr,
            selected_cycloidium_written as i64,
            masonrybrace_source.as_mut_ptr(),
            masonrybrace_source.len() as i64,
        )
    };
    let masonrybrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            masonrybrace_source.as_ptr(),
            masonrybrace_source_written as i64,
            masonrybrace_old.as_ptr(),
            masonrybrace_old.len() as i64,
            masonrybrace_new.as_ptr(),
            masonrybrace_new.len() as i64,
        )
    };
    let mut masonrybrace_value = vec![0u8; masonrybrace_value_len as usize];
    let masonrybrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            masonrybrace_source.as_ptr(),
            masonrybrace_source_written as i64,
            masonrybrace_old.as_ptr(),
            masonrybrace_old.len() as i64,
            masonrybrace_new.as_ptr(),
            masonrybrace_new.len() as i64,
            masonrybrace_value.as_mut_ptr(),
            masonrybrace_value.len() as i64,
        )
    };
    let masonrybrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            masonrybrace_value.as_ptr(),
            masonrybrace_value_written as i64,
            masonrybrace_needle.as_ptr(),
            masonrybrace_needle.len() as i64,
        )
    };

    let mut selected_hypotrochoidium_index = 0i32;
    let mut best_hypotrochoidium_score = i32::MIN;
    let mut hypotrochoidium_index = 0i32;
    while hypotrochoidium_index < 3 {
        let mut candidate_hypotrochoidium_score = hypotrochoidium_value_written * 10 + hypotrochoidium_contains * 50;
        if hypotrochoidium_index == 1 {
            candidate_hypotrochoidium_score = bosonwake_value_written * 10 + bosonwake_index;
        } else if hypotrochoidium_index == 2 {
            candidate_hypotrochoidium_score = masonrybrace_value_written * 10 + masonrybrace_contains * 50;
        }

        let mut hypotrochoidium_bonus = 0i32;
        if hypotrochoidium_index == selected_cycloidium_index {
            hypotrochoidium_bonus += 25;
        }
        if hypotrochoidium_index == selected_lissajousium_index {
            hypotrochoidium_bonus += 15;
        }
        if hypotrochoidium_index == selected_rochelium_index {
            hypotrochoidium_bonus += 5;
        }
        if hypotrochoidium_index == 0 && hypotrochoidium_contains != 0 {
            hypotrochoidium_bonus += 20;
        }
        if hypotrochoidium_index == 1 && bosonwake_index >= 0 {
            hypotrochoidium_bonus += 10;
        }
        if hypotrochoidium_index == 2 && masonrybrace_contains != 0 {
            hypotrochoidium_bonus += 30;
        }

        let hypotrochoidium_score = candidate_hypotrochoidium_score + hypotrochoidium_bonus;
        if hypotrochoidium_score > best_hypotrochoidium_score {
            best_hypotrochoidium_score = hypotrochoidium_score;
            selected_hypotrochoidium_index = hypotrochoidium_index;
        }

        hypotrochoidium_index += 1;
    }

    let mut selected_hypotrochoidium_ptr = hypotrochoidium_value.as_ptr();
    let mut selected_hypotrochoidium_written = hypotrochoidium_value_written;
    if selected_hypotrochoidium_index == 1 {
        selected_hypotrochoidium_ptr = bosonwake_value.as_ptr();
        selected_hypotrochoidium_written = bosonwake_value_written;
    } else if selected_hypotrochoidium_index == 2 {
        selected_hypotrochoidium_ptr = masonrybrace_value.as_ptr();
        selected_hypotrochoidium_written = masonrybrace_value_written;
    }

    let epicycloidium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_hypotrochoidium_ptr,
            selected_hypotrochoidium_written as i64,
            epicycloidium_old.as_ptr(),
            epicycloidium_old.len() as i64,
            epicycloidium_new.as_ptr(),
            epicycloidium_new.len() as i64,
        )
    };
    let mut epicycloidium_value = vec![0u8; epicycloidium_value_len as usize];
    let epicycloidium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_hypotrochoidium_ptr,
            selected_hypotrochoidium_written as i64,
            epicycloidium_old.as_ptr(),
            epicycloidium_old.len() as i64,
            epicycloidium_new.as_ptr(),
            epicycloidium_new.len() as i64,
            epicycloidium_value.as_mut_ptr(),
            epicycloidium_value.len() as i64,
        )
    };
    let epicycloidium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            epicycloidium_value.as_ptr(),
            epicycloidium_value_written as i64,
            epicycloidium_needle.as_ptr(),
            epicycloidium_needle.len() as i64,
        )
    };

    let muonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_hypotrochoidium_ptr,
            selected_hypotrochoidium_written as i64,
            muonwake_extension.as_ptr(),
            muonwake_extension.len() as i64,
        )
    };
    let mut muonwake_value = vec![0u8; muonwake_value_len as usize];
    let muonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_hypotrochoidium_ptr,
            selected_hypotrochoidium_written as i64,
            muonwake_extension.as_ptr(),
            muonwake_extension.len() as i64,
            muonwake_value.as_mut_ptr(),
            muonwake_value.len() as i64,
        )
    };
    let muonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            muonwake_value.as_ptr(),
            muonwake_value_written as i64,
            muonwake_needle.as_ptr(),
            muonwake_needle.len() as i64,
        )
    };

    let fretworkbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_hypotrochoidium_ptr,
            selected_hypotrochoidium_written as i64,
        )
    };
    let mut fretworkbrace_source = vec![0u8; fretworkbrace_source_len as usize];
    let fretworkbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_hypotrochoidium_ptr,
            selected_hypotrochoidium_written as i64,
            fretworkbrace_source.as_mut_ptr(),
            fretworkbrace_source.len() as i64,
        )
    };
    let fretworkbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            fretworkbrace_source.as_ptr(),
            fretworkbrace_source_written as i64,
            fretworkbrace_old.as_ptr(),
            fretworkbrace_old.len() as i64,
            fretworkbrace_new.as_ptr(),
            fretworkbrace_new.len() as i64,
        )
    };
    let mut fretworkbrace_value = vec![0u8; fretworkbrace_value_len as usize];
    let fretworkbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            fretworkbrace_source.as_ptr(),
            fretworkbrace_source_written as i64,
            fretworkbrace_old.as_ptr(),
            fretworkbrace_old.len() as i64,
            fretworkbrace_new.as_ptr(),
            fretworkbrace_new.len() as i64,
            fretworkbrace_value.as_mut_ptr(),
            fretworkbrace_value.len() as i64,
        )
    };
    let fretworkbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            fretworkbrace_value.as_ptr(),
            fretworkbrace_value_written as i64,
            fretworkbrace_needle.as_ptr(),
            fretworkbrace_needle.len() as i64,
        )
    };

    let mut selected_epicycloidium_index = 0i32;
    let mut best_epicycloidium_score = i32::MIN;
    let mut epicycloidium_index = 0i32;
    while epicycloidium_index < 3 {
        let mut candidate_epicycloidium_score = epicycloidium_value_written * 10 + epicycloidium_contains * 50;
        if epicycloidium_index == 1 {
            candidate_epicycloidium_score = muonwake_value_written * 10 + muonwake_index;
        } else if epicycloidium_index == 2 {
            candidate_epicycloidium_score = fretworkbrace_value_written * 10 + fretworkbrace_contains * 50;
        }

        let mut epicycloidium_bonus = 0i32;
        if epicycloidium_index == selected_hypotrochoidium_index {
            epicycloidium_bonus += 25;
        }
        if epicycloidium_index == selected_cycloidium_index {
            epicycloidium_bonus += 15;
        }
        if epicycloidium_index == selected_lissajousium_index {
            epicycloidium_bonus += 5;
        }
        if epicycloidium_index == 0 && epicycloidium_contains != 0 {
            epicycloidium_bonus += 20;
        }
        if epicycloidium_index == 1 && muonwake_index >= 0 {
            epicycloidium_bonus += 10;
        }
        if epicycloidium_index == 2 && fretworkbrace_contains != 0 {
            epicycloidium_bonus += 30;
        }

        let epicycloidium_score = candidate_epicycloidium_score + epicycloidium_bonus;
        if epicycloidium_score > best_epicycloidium_score {
            best_epicycloidium_score = epicycloidium_score;
            selected_epicycloidium_index = epicycloidium_index;
        }

        epicycloidium_index += 1;
    }

    let mut selected_epicycloidium_ptr = epicycloidium_value.as_ptr();
    let mut selected_epicycloidium_written = epicycloidium_value_written;
    if selected_epicycloidium_index == 1 {
        selected_epicycloidium_ptr = muonwake_value.as_ptr();
        selected_epicycloidium_written = muonwake_value_written;
    } else if selected_epicycloidium_index == 2 {
        selected_epicycloidium_ptr = fretworkbrace_value.as_ptr();
        selected_epicycloidium_written = fretworkbrace_value_written;
    }

    let pericycloidium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_epicycloidium_ptr,
            selected_epicycloidium_written as i64,
            pericycloidium_old.as_ptr(),
            pericycloidium_old.len() as i64,
            pericycloidium_new.as_ptr(),
            pericycloidium_new.len() as i64,
        )
    };
    let mut pericycloidium_value = vec![0u8; pericycloidium_value_len as usize];
    let pericycloidium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_epicycloidium_ptr,
            selected_epicycloidium_written as i64,
            pericycloidium_old.as_ptr(),
            pericycloidium_old.len() as i64,
            pericycloidium_new.as_ptr(),
            pericycloidium_new.len() as i64,
            pericycloidium_value.as_mut_ptr(),
            pericycloidium_value.len() as i64,
        )
    };
    let pericycloidium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            pericycloidium_value.as_ptr(),
            pericycloidium_value_written as i64,
            pericycloidium_needle.as_ptr(),
            pericycloidium_needle.len() as i64,
        )
    };

    let gluonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_epicycloidium_ptr,
            selected_epicycloidium_written as i64,
            gluonwake_extension.as_ptr(),
            gluonwake_extension.len() as i64,
        )
    };
    let mut gluonwake_value = vec![0u8; gluonwake_value_len as usize];
    let gluonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_epicycloidium_ptr,
            selected_epicycloidium_written as i64,
            gluonwake_extension.as_ptr(),
            gluonwake_extension.len() as i64,
            gluonwake_value.as_mut_ptr(),
            gluonwake_value.len() as i64,
        )
    };
    let gluonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            gluonwake_value.as_ptr(),
            gluonwake_value_written as i64,
            gluonwake_needle.as_ptr(),
            gluonwake_needle.len() as i64,
        )
    };

    let trellisbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_epicycloidium_ptr,
            selected_epicycloidium_written as i64,
        )
    };
    let mut trellisbrace_source = vec![0u8; trellisbrace_source_len as usize];
    let trellisbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_epicycloidium_ptr,
            selected_epicycloidium_written as i64,
            trellisbrace_source.as_mut_ptr(),
            trellisbrace_source.len() as i64,
        )
    };
    let trellisbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            trellisbrace_source.as_ptr(),
            trellisbrace_source_written as i64,
            trellisbrace_old.as_ptr(),
            trellisbrace_old.len() as i64,
            trellisbrace_new.as_ptr(),
            trellisbrace_new.len() as i64,
        )
    };
    let mut trellisbrace_value = vec![0u8; trellisbrace_value_len as usize];
    let trellisbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            trellisbrace_source.as_ptr(),
            trellisbrace_source_written as i64,
            trellisbrace_old.as_ptr(),
            trellisbrace_old.len() as i64,
            trellisbrace_new.as_ptr(),
            trellisbrace_new.len() as i64,
            trellisbrace_value.as_mut_ptr(),
            trellisbrace_value.len() as i64,
        )
    };
    let trellisbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            trellisbrace_value.as_ptr(),
            trellisbrace_value_written as i64,
            trellisbrace_needle.as_ptr(),
            trellisbrace_needle.len() as i64,
        )
    };

    let mut selected_pericycloidium_index = 0i32;
    let mut best_pericycloidium_score = i32::MIN;
    let mut pericycloidium_index = 0i32;
    while pericycloidium_index < 3 {
        let mut candidate_pericycloidium_score = pericycloidium_value_written * 10 + pericycloidium_contains * 50;
        if pericycloidium_index == 1 {
            candidate_pericycloidium_score = gluonwake_value_written * 10 + gluonwake_index;
        } else if pericycloidium_index == 2 {
            candidate_pericycloidium_score = trellisbrace_value_written * 10 + trellisbrace_contains * 50;
        }

        let mut pericycloidium_bonus = 0i32;
        if pericycloidium_index == selected_epicycloidium_index {
            pericycloidium_bonus += 25;
        }
        if pericycloidium_index == selected_hypotrochoidium_index {
            pericycloidium_bonus += 15;
        }
        if pericycloidium_index == selected_cycloidium_index {
            pericycloidium_bonus += 5;
        }
        if pericycloidium_index == 0 && pericycloidium_contains != 0 {
            pericycloidium_bonus += 20;
        }
        if pericycloidium_index == 1 && gluonwake_index >= 0 {
            pericycloidium_bonus += 10;
        }
        if pericycloidium_index == 2 && trellisbrace_contains != 0 {
            pericycloidium_bonus += 30;
        }

        let pericycloidium_score = candidate_pericycloidium_score + pericycloidium_bonus;
        if pericycloidium_score > best_pericycloidium_score {
            best_pericycloidium_score = pericycloidium_score;
            selected_pericycloidium_index = pericycloidium_index;
        }

        pericycloidium_index += 1;
    }

    let mut selected_pericycloidium_ptr = pericycloidium_value.as_ptr();
    let mut selected_pericycloidium_written = pericycloidium_value_written;
    if selected_pericycloidium_index == 1 {
        selected_pericycloidium_ptr = gluonwake_value.as_ptr();
        selected_pericycloidium_written = gluonwake_value_written;
    } else if selected_pericycloidium_index == 2 {
        selected_pericycloidium_ptr = trellisbrace_value.as_ptr();
        selected_pericycloidium_written = trellisbrace_value_written;
    }

    let astroidium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_pericycloidium_ptr,
            selected_pericycloidium_written as i64,
            astroidium_old.as_ptr(),
            astroidium_old.len() as i64,
            astroidium_new.as_ptr(),
            astroidium_new.len() as i64,
        )
    };
    let mut astroidium_value = vec![0u8; astroidium_value_len as usize];
    let astroidium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_pericycloidium_ptr,
            selected_pericycloidium_written as i64,
            astroidium_old.as_ptr(),
            astroidium_old.len() as i64,
            astroidium_new.as_ptr(),
            astroidium_new.len() as i64,
            astroidium_value.as_mut_ptr(),
            astroidium_value.len() as i64,
        )
    };
    let astroidium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            astroidium_value.as_ptr(),
            astroidium_value_written as i64,
            astroidium_needle.as_ptr(),
            astroidium_needle.len() as i64,
        )
    };

    let phononwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_pericycloidium_ptr,
            selected_pericycloidium_written as i64,
            phononwake_extension.as_ptr(),
            phononwake_extension.len() as i64,
        )
    };
    let mut phononwake_value = vec![0u8; phononwake_value_len as usize];
    let phononwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_pericycloidium_ptr,
            selected_pericycloidium_written as i64,
            phononwake_extension.as_ptr(),
            phononwake_extension.len() as i64,
            phononwake_value.as_mut_ptr(),
            phononwake_value.len() as i64,
        )
    };
    let phononwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            phononwake_value.as_ptr(),
            phononwake_value_written as i64,
            phononwake_needle.as_ptr(),
            phononwake_needle.len() as i64,
        )
    };

    let filigreebrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_pericycloidium_ptr,
            selected_pericycloidium_written as i64,
        )
    };
    let mut filigreebrace_source = vec![0u8; filigreebrace_source_len as usize];
    let filigreebrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_pericycloidium_ptr,
            selected_pericycloidium_written as i64,
            filigreebrace_source.as_mut_ptr(),
            filigreebrace_source.len() as i64,
        )
    };
    let filigreebrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            filigreebrace_source.as_ptr(),
            filigreebrace_source_written as i64,
            filigreebrace_old.as_ptr(),
            filigreebrace_old.len() as i64,
            filigreebrace_new.as_ptr(),
            filigreebrace_new.len() as i64,
        )
    };
    let mut filigreebrace_value = vec![0u8; filigreebrace_value_len as usize];
    let filigreebrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            filigreebrace_source.as_ptr(),
            filigreebrace_source_written as i64,
            filigreebrace_old.as_ptr(),
            filigreebrace_old.len() as i64,
            filigreebrace_new.as_ptr(),
            filigreebrace_new.len() as i64,
            filigreebrace_value.as_mut_ptr(),
            filigreebrace_value.len() as i64,
        )
    };
    let filigreebrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            filigreebrace_value.as_ptr(),
            filigreebrace_value_written as i64,
            filigreebrace_needle.as_ptr(),
            filigreebrace_needle.len() as i64,
        )
    };

    let mut selected_astroidium_index = 0i32;
    let mut best_astroidium_score = i32::MIN;
    let mut astroidium_index = 0i32;
    while astroidium_index < 3 {
        let mut candidate_astroidium_score = astroidium_value_written * 10 + astroidium_contains * 50;
        if astroidium_index == 1 {
            candidate_astroidium_score = phononwake_value_written * 10 + phononwake_index;
        } else if astroidium_index == 2 {
            candidate_astroidium_score = filigreebrace_value_written * 10 + filigreebrace_contains * 50;
        }

        let mut astroidium_bonus = 0i32;
        if astroidium_index == selected_pericycloidium_index {
            astroidium_bonus += 25;
        }
        if astroidium_index == selected_epicycloidium_index {
            astroidium_bonus += 15;
        }
        if astroidium_index == selected_hypotrochoidium_index {
            astroidium_bonus += 5;
        }
        if astroidium_index == 0 && astroidium_contains != 0 {
            astroidium_bonus += 20;
        }
        if astroidium_index == 1 && phononwake_index >= 0 {
            astroidium_bonus += 10;
        }
        if astroidium_index == 2 && filigreebrace_contains != 0 {
            astroidium_bonus += 30;
        }

        let astroidium_score = candidate_astroidium_score + astroidium_bonus;
        if astroidium_score > best_astroidium_score {
            best_astroidium_score = astroidium_score;
            selected_astroidium_index = astroidium_index;
        }

        astroidium_index += 1;
    }

    let mut selected_astroidium_ptr = astroidium_value.as_ptr();
    let mut selected_astroidium_written = astroidium_value_written;
    if selected_astroidium_index == 1 {
        selected_astroidium_ptr = phononwake_value.as_ptr();
        selected_astroidium_written = phononwake_value_written;
    } else if selected_astroidium_index == 2 {
        selected_astroidium_ptr = filigreebrace_value.as_ptr();
        selected_astroidium_written = filigreebrace_value_written;
    }

    let deltoidium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_astroidium_ptr,
            selected_astroidium_written as i64,
            deltoidium_old.as_ptr(),
            deltoidium_old.len() as i64,
            deltoidium_new.as_ptr(),
            deltoidium_new.len() as i64,
        )
    };
    let mut deltoidium_value = vec![0u8; deltoidium_value_len as usize];
    let deltoidium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_astroidium_ptr,
            selected_astroidium_written as i64,
            deltoidium_old.as_ptr(),
            deltoidium_old.len() as i64,
            deltoidium_new.as_ptr(),
            deltoidium_new.len() as i64,
            deltoidium_value.as_mut_ptr(),
            deltoidium_value.len() as i64,
        )
    };
    let deltoidium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            deltoidium_value.as_ptr(),
            deltoidium_value_written as i64,
            deltoidium_needle.as_ptr(),
            deltoidium_needle.len() as i64,
        )
    };

    let magnonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_astroidium_ptr,
            selected_astroidium_written as i64,
            magnonwake_extension.as_ptr(),
            magnonwake_extension.len() as i64,
        )
    };
    let mut magnonwake_value = vec![0u8; magnonwake_value_len as usize];
    let magnonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_astroidium_ptr,
            selected_astroidium_written as i64,
            magnonwake_extension.as_ptr(),
            magnonwake_extension.len() as i64,
            magnonwake_value.as_mut_ptr(),
            magnonwake_value.len() as i64,
        )
    };
    let magnonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            magnonwake_value.as_ptr(),
            magnonwake_value_written as i64,
            magnonwake_needle.as_ptr(),
            magnonwake_needle.len() as i64,
        )
    };

    let arabesquebrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_astroidium_ptr,
            selected_astroidium_written as i64,
        )
    };
    let mut arabesquebrace_source = vec![0u8; arabesquebrace_source_len as usize];
    let arabesquebrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_astroidium_ptr,
            selected_astroidium_written as i64,
            arabesquebrace_source.as_mut_ptr(),
            arabesquebrace_source.len() as i64,
        )
    };
    let arabesquebrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            arabesquebrace_source.as_ptr(),
            arabesquebrace_source_written as i64,
            arabesquebrace_old.as_ptr(),
            arabesquebrace_old.len() as i64,
            arabesquebrace_new.as_ptr(),
            arabesquebrace_new.len() as i64,
        )
    };
    let mut arabesquebrace_value = vec![0u8; arabesquebrace_value_len as usize];
    let arabesquebrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            arabesquebrace_source.as_ptr(),
            arabesquebrace_source_written as i64,
            arabesquebrace_old.as_ptr(),
            arabesquebrace_old.len() as i64,
            arabesquebrace_new.as_ptr(),
            arabesquebrace_new.len() as i64,
            arabesquebrace_value.as_mut_ptr(),
            arabesquebrace_value.len() as i64,
        )
    };
    let arabesquebrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            arabesquebrace_value.as_ptr(),
            arabesquebrace_value_written as i64,
            arabesquebrace_needle.as_ptr(),
            arabesquebrace_needle.len() as i64,
        )
    };

    let mut selected_deltoidium_index = 0i32;
    let mut best_deltoidium_score = i32::MIN;
    let mut deltoidium_index = 0i32;
    while deltoidium_index < 3 {
        let mut candidate_deltoidium_score = deltoidium_value_written * 10 + deltoidium_contains * 50;
        if deltoidium_index == 1 {
            candidate_deltoidium_score = magnonwake_value_written * 10 + magnonwake_index;
        } else if deltoidium_index == 2 {
            candidate_deltoidium_score = arabesquebrace_value_written * 10 + arabesquebrace_contains * 50;
        }

        let mut deltoidium_bonus = 0i32;
        if deltoidium_index == selected_astroidium_index {
            deltoidium_bonus += 25;
        }
        if deltoidium_index == selected_pericycloidium_index {
            deltoidium_bonus += 15;
        }
        if deltoidium_index == selected_epicycloidium_index {
            deltoidium_bonus += 5;
        }
        if deltoidium_index == 0 && deltoidium_contains != 0 {
            deltoidium_bonus += 20;
        }
        if deltoidium_index == 1 && magnonwake_index >= 0 {
            deltoidium_bonus += 10;
        }
        if deltoidium_index == 2 && arabesquebrace_contains != 0 {
            deltoidium_bonus += 30;
        }

        let deltoidium_score = candidate_deltoidium_score + deltoidium_bonus;
        if deltoidium_score > best_deltoidium_score {
            best_deltoidium_score = deltoidium_score;
            selected_deltoidium_index = deltoidium_index;
        }

        deltoidium_index += 1;
    }

    let mut selected_deltoidium_ptr = deltoidium_value.as_ptr();
    let mut selected_deltoidium_written = deltoidium_value_written;
    if selected_deltoidium_index == 1 {
        selected_deltoidium_ptr = magnonwake_value.as_ptr();
        selected_deltoidium_written = magnonwake_value_written;
    } else if selected_deltoidium_index == 2 {
        selected_deltoidium_ptr = arabesquebrace_value.as_ptr();
        selected_deltoidium_written = arabesquebrace_value_written;
    }

    let lemniscatoidium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_deltoidium_ptr,
            selected_deltoidium_written as i64,
            lemniscatoidium_old.as_ptr(),
            lemniscatoidium_old.len() as i64,
            lemniscatoidium_new.as_ptr(),
            lemniscatoidium_new.len() as i64,
        )
    };
    let mut lemniscatoidium_value = vec![0u8; lemniscatoidium_value_len as usize];
    let lemniscatoidium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_deltoidium_ptr,
            selected_deltoidium_written as i64,
            lemniscatoidium_old.as_ptr(),
            lemniscatoidium_old.len() as i64,
            lemniscatoidium_new.as_ptr(),
            lemniscatoidium_new.len() as i64,
            lemniscatoidium_value.as_mut_ptr(),
            lemniscatoidium_value.len() as i64,
        )
    };
    let lemniscatoidium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            lemniscatoidium_value.as_ptr(),
            lemniscatoidium_value_written as i64,
            lemniscatoidium_needle.as_ptr(),
            lemniscatoidium_needle.len() as i64,
        )
    };

    let plasmonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_deltoidium_ptr,
            selected_deltoidium_written as i64,
            plasmonwake_extension.as_ptr(),
            plasmonwake_extension.len() as i64,
        )
    };
    let mut plasmonwake_value = vec![0u8; plasmonwake_value_len as usize];
    let plasmonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_deltoidium_ptr,
            selected_deltoidium_written as i64,
            plasmonwake_extension.as_ptr(),
            plasmonwake_extension.len() as i64,
            plasmonwake_value.as_mut_ptr(),
            plasmonwake_value.len() as i64,
        )
    };
    let plasmonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            plasmonwake_value.as_ptr(),
            plasmonwake_value_written as i64,
            plasmonwake_needle.as_ptr(),
            plasmonwake_needle.len() as i64,
        )
    };

    let ornamentbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_deltoidium_ptr,
            selected_deltoidium_written as i64,
        )
    };
    let mut ornamentbrace_source = vec![0u8; ornamentbrace_source_len as usize];
    let ornamentbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_deltoidium_ptr,
            selected_deltoidium_written as i64,
            ornamentbrace_source.as_mut_ptr(),
            ornamentbrace_source.len() as i64,
        )
    };
    let ornamentbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            ornamentbrace_source.as_ptr(),
            ornamentbrace_source_written as i64,
            ornamentbrace_old.as_ptr(),
            ornamentbrace_old.len() as i64,
            ornamentbrace_new.as_ptr(),
            ornamentbrace_new.len() as i64,
        )
    };
    let mut ornamentbrace_value = vec![0u8; ornamentbrace_value_len as usize];
    let ornamentbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            ornamentbrace_source.as_ptr(),
            ornamentbrace_source_written as i64,
            ornamentbrace_old.as_ptr(),
            ornamentbrace_old.len() as i64,
            ornamentbrace_new.as_ptr(),
            ornamentbrace_new.len() as i64,
            ornamentbrace_value.as_mut_ptr(),
            ornamentbrace_value.len() as i64,
        )
    };
    let ornamentbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            ornamentbrace_value.as_ptr(),
            ornamentbrace_value_written as i64,
            ornamentbrace_needle.as_ptr(),
            ornamentbrace_needle.len() as i64,
        )
    };

    let mut selected_lemniscatoidium_index = 0i32;
    let mut best_lemniscatoidium_score = i32::MIN;
    let mut lemniscatoidium_index = 0i32;
    while lemniscatoidium_index < 3 {
        let mut candidate_lemniscatoidium_score = lemniscatoidium_value_written * 10 + lemniscatoidium_contains * 50;
        if lemniscatoidium_index == 1 {
            candidate_lemniscatoidium_score = plasmonwake_value_written * 10 + plasmonwake_index;
        } else if lemniscatoidium_index == 2 {
            candidate_lemniscatoidium_score = ornamentbrace_value_written * 10 + ornamentbrace_contains * 50;
        }

        let mut lemniscatoidium_bonus = 0i32;
        if lemniscatoidium_index == selected_deltoidium_index {
            lemniscatoidium_bonus += 25;
        }
        if lemniscatoidium_index == selected_astroidium_index {
            lemniscatoidium_bonus += 15;
        }
        if lemniscatoidium_index == selected_pericycloidium_index {
            lemniscatoidium_bonus += 5;
        }
        if lemniscatoidium_index == 0 && lemniscatoidium_contains != 0 {
            lemniscatoidium_bonus += 20;
        }
        if lemniscatoidium_index == 1 && plasmonwake_index >= 0 {
            lemniscatoidium_bonus += 10;
        }
        if lemniscatoidium_index == 2 && ornamentbrace_contains != 0 {
            lemniscatoidium_bonus += 30;
        }

        let lemniscatoidium_score = candidate_lemniscatoidium_score + lemniscatoidium_bonus;
        if lemniscatoidium_score > best_lemniscatoidium_score {
            best_lemniscatoidium_score = lemniscatoidium_score;
            selected_lemniscatoidium_index = lemniscatoidium_index;
        }

        lemniscatoidium_index += 1;
    }

    let mut selected_lemniscatoidium_ptr = lemniscatoidium_value.as_ptr();
    let mut selected_lemniscatoidium_written = lemniscatoidium_value_written;
    if selected_lemniscatoidium_index == 1 {
        selected_lemniscatoidium_ptr = plasmonwake_value.as_ptr();
        selected_lemniscatoidium_written = plasmonwake_value_written;
    } else if selected_lemniscatoidium_index == 2 {
        selected_lemniscatoidium_ptr = ornamentbrace_value.as_ptr();
        selected_lemniscatoidium_written = ornamentbrace_value_written;
    }

    let rosecurvium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_lemniscatoidium_ptr,
            selected_lemniscatoidium_written as i64,
            rosecurvium_old.as_ptr(),
            rosecurvium_old.len() as i64,
            rosecurvium_new.as_ptr(),
            rosecurvium_new.len() as i64,
        )
    };
    let mut rosecurvium_value = vec![0u8; rosecurvium_value_len as usize];
    let rosecurvium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_lemniscatoidium_ptr,
            selected_lemniscatoidium_written as i64,
            rosecurvium_old.as_ptr(),
            rosecurvium_old.len() as i64,
            rosecurvium_new.as_ptr(),
            rosecurvium_new.len() as i64,
            rosecurvium_value.as_mut_ptr(),
            rosecurvium_value.len() as i64,
        )
    };
    let rosecurvium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            rosecurvium_value.as_ptr(),
            rosecurvium_value_written as i64,
            rosecurvium_needle.as_ptr(),
            rosecurvium_needle.len() as i64,
        )
    };

    let solitonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_lemniscatoidium_ptr,
            selected_lemniscatoidium_written as i64,
            solitonwake_extension.as_ptr(),
            solitonwake_extension.len() as i64,
        )
    };
    let mut solitonwake_value = vec![0u8; solitonwake_value_len as usize];
    let solitonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_lemniscatoidium_ptr,
            selected_lemniscatoidium_written as i64,
            solitonwake_extension.as_ptr(),
            solitonwake_extension.len() as i64,
            solitonwake_value.as_mut_ptr(),
            solitonwake_value.len() as i64,
        )
    };
    let solitonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            solitonwake_value.as_ptr(),
            solitonwake_value_written as i64,
            solitonwake_needle.as_ptr(),
            solitonwake_needle.len() as i64,
        )
    };

    let scrollbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_lemniscatoidium_ptr,
            selected_lemniscatoidium_written as i64,
        )
    };
    let mut scrollbrace_source = vec![0u8; scrollbrace_source_len as usize];
    let scrollbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_lemniscatoidium_ptr,
            selected_lemniscatoidium_written as i64,
            scrollbrace_source.as_mut_ptr(),
            scrollbrace_source.len() as i64,
        )
    };
    let scrollbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            scrollbrace_source.as_ptr(),
            scrollbrace_source_written as i64,
            scrollbrace_old.as_ptr(),
            scrollbrace_old.len() as i64,
            scrollbrace_new.as_ptr(),
            scrollbrace_new.len() as i64,
        )
    };
    let mut scrollbrace_value = vec![0u8; scrollbrace_value_len as usize];
    let scrollbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            scrollbrace_source.as_ptr(),
            scrollbrace_source_written as i64,
            scrollbrace_old.as_ptr(),
            scrollbrace_old.len() as i64,
            scrollbrace_new.as_ptr(),
            scrollbrace_new.len() as i64,
            scrollbrace_value.as_mut_ptr(),
            scrollbrace_value.len() as i64,
        )
    };
    let scrollbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            scrollbrace_value.as_ptr(),
            scrollbrace_value_written as i64,
            scrollbrace_needle.as_ptr(),
            scrollbrace_needle.len() as i64,
        )
    };

    let mut selected_rosecurvium_index = 0i32;
    let mut best_rosecurvium_score = i32::MIN;
    let mut rosecurvium_index = 0i32;
    while rosecurvium_index < 3 {
        let mut candidate_rosecurvium_score = rosecurvium_value_written * 10 + rosecurvium_contains * 50;
        if rosecurvium_index == 1 {
            candidate_rosecurvium_score = solitonwake_value_written * 10 + solitonwake_index;
        } else if rosecurvium_index == 2 {
            candidate_rosecurvium_score = scrollbrace_value_written * 10 + scrollbrace_contains * 50;
        }

        let mut rosecurvium_bonus = 0i32;
        if rosecurvium_index == selected_lemniscatoidium_index {
            rosecurvium_bonus += 25;
        }
        if rosecurvium_index == selected_deltoidium_index {
            rosecurvium_bonus += 15;
        }
        if rosecurvium_index == selected_astroidium_index {
            rosecurvium_bonus += 5;
        }
        if rosecurvium_index == 0 && rosecurvium_contains != 0 {
            rosecurvium_bonus += 20;
        }
        if rosecurvium_index == 1 && solitonwake_index >= 0 {
            rosecurvium_bonus += 10;
        }
        if rosecurvium_index == 2 && scrollbrace_contains != 0 {
            rosecurvium_bonus += 30;
        }

        let rosecurvium_score = candidate_rosecurvium_score + rosecurvium_bonus;
        if rosecurvium_score > best_rosecurvium_score {
            best_rosecurvium_score = rosecurvium_score;
            selected_rosecurvium_index = rosecurvium_index;
        }

        rosecurvium_index += 1;
    }

    let mut selected_rosecurvium_ptr = rosecurvium_value.as_ptr();
    let mut selected_rosecurvium_written = rosecurvium_value_written;
    if selected_rosecurvium_index == 1 {
        selected_rosecurvium_ptr = solitonwake_value.as_ptr();
        selected_rosecurvium_written = solitonwake_value_written;
    } else if selected_rosecurvium_index == 2 {
        selected_rosecurvium_ptr = scrollbrace_value.as_ptr();
        selected_rosecurvium_written = scrollbrace_value_written;
    }

    let cardioidium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_rosecurvium_ptr,
            selected_rosecurvium_written as i64,
            cardioidium_old.as_ptr(),
            cardioidium_old.len() as i64,
            cardioidium_new.as_ptr(),
            cardioidium_new.len() as i64,
        )
    };
    let mut cardioidium_value = vec![0u8; cardioidium_value_len as usize];
    let cardioidium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_rosecurvium_ptr,
            selected_rosecurvium_written as i64,
            cardioidium_old.as_ptr(),
            cardioidium_old.len() as i64,
            cardioidium_new.as_ptr(),
            cardioidium_new.len() as i64,
            cardioidium_value.as_mut_ptr(),
            cardioidium_value.len() as i64,
        )
    };
    let cardioidium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            cardioidium_value.as_ptr(),
            cardioidium_value_written as i64,
            cardioidium_needle.as_ptr(),
            cardioidium_needle.len() as i64,
        )
    };

    let ionwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_rosecurvium_ptr,
            selected_rosecurvium_written as i64,
            ionwake_extension.as_ptr(),
            ionwake_extension.len() as i64,
        )
    };
    let mut ionwake_value = vec![0u8; ionwake_value_len as usize];
    let ionwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_rosecurvium_ptr,
            selected_rosecurvium_written as i64,
            ionwake_extension.as_ptr(),
            ionwake_extension.len() as i64,
            ionwake_value.as_mut_ptr(),
            ionwake_value.len() as i64,
        )
    };
    let ionwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            ionwake_value.as_ptr(),
            ionwake_value_written as i64,
            ionwake_needle.as_ptr(),
            ionwake_needle.len() as i64,
        )
    };

    let embossbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_rosecurvium_ptr,
            selected_rosecurvium_written as i64,
        )
    };
    let mut embossbrace_source = vec![0u8; embossbrace_source_len as usize];
    let embossbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_rosecurvium_ptr,
            selected_rosecurvium_written as i64,
            embossbrace_source.as_mut_ptr(),
            embossbrace_source.len() as i64,
        )
    };
    let embossbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            embossbrace_source.as_ptr(),
            embossbrace_source_written as i64,
            embossbrace_old.as_ptr(),
            embossbrace_old.len() as i64,
            embossbrace_new.as_ptr(),
            embossbrace_new.len() as i64,
        )
    };
    let mut embossbrace_value = vec![0u8; embossbrace_value_len as usize];
    let embossbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            embossbrace_source.as_ptr(),
            embossbrace_source_written as i64,
            embossbrace_old.as_ptr(),
            embossbrace_old.len() as i64,
            embossbrace_new.as_ptr(),
            embossbrace_new.len() as i64,
            embossbrace_value.as_mut_ptr(),
            embossbrace_value.len() as i64,
        )
    };
    let embossbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            embossbrace_value.as_ptr(),
            embossbrace_value_written as i64,
            embossbrace_needle.as_ptr(),
            embossbrace_needle.len() as i64,
        )
    };

    let mut selected_cardioidium_index = 0i32;
    let mut best_cardioidium_score = i32::MIN;
    let mut cardioidium_index = 0i32;
    while cardioidium_index < 3 {
        let mut candidate_cardioidium_score = cardioidium_value_written * 10 + cardioidium_contains * 50;
        if cardioidium_index == 1 {
            candidate_cardioidium_score = ionwake_value_written * 10 + ionwake_index;
        } else if cardioidium_index == 2 {
            candidate_cardioidium_score = embossbrace_value_written * 10 + embossbrace_contains * 50;
        }

        let mut cardioidium_bonus = 0i32;
        if cardioidium_index == selected_rosecurvium_index {
            cardioidium_bonus += 25;
        }
        if cardioidium_index == selected_lemniscatoidium_index {
            cardioidium_bonus += 15;
        }
        if cardioidium_index == selected_deltoidium_index {
            cardioidium_bonus += 5;
        }
        if cardioidium_index == 0 && cardioidium_contains != 0 {
            cardioidium_bonus += 20;
        }
        if cardioidium_index == 1 && ionwake_index >= 0 {
            cardioidium_bonus += 10;
        }
        if cardioidium_index == 2 && embossbrace_contains != 0 {
            cardioidium_bonus += 30;
        }

        let cardioidium_score = candidate_cardioidium_score + cardioidium_bonus;
        if cardioidium_score > best_cardioidium_score {
            best_cardioidium_score = cardioidium_score;
            selected_cardioidium_index = cardioidium_index;
        }

        cardioidium_index += 1;
    }

    let mut selected_cardioidium_ptr = cardioidium_value.as_ptr();
    let mut selected_cardioidium_written = cardioidium_value_written;
    if selected_cardioidium_index == 1 {
        selected_cardioidium_ptr = ionwake_value.as_ptr();
        selected_cardioidium_written = ionwake_value_written;
    } else if selected_cardioidium_index == 2 {
        selected_cardioidium_ptr = embossbrace_value.as_ptr();
        selected_cardioidium_written = embossbrace_value_written;
    }

    let epitrochoidion_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_cardioidium_ptr,
            selected_cardioidium_written as i64,
            epitrochoidion_old.as_ptr(),
            epitrochoidion_old.len() as i64,
            epitrochoidion_new.as_ptr(),
            epitrochoidion_new.len() as i64,
        )
    };
    let mut epitrochoidion_value = vec![0u8; epitrochoidion_value_len as usize];
    let epitrochoidion_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_cardioidium_ptr,
            selected_cardioidium_written as i64,
            epitrochoidion_old.as_ptr(),
            epitrochoidion_old.len() as i64,
            epitrochoidion_new.as_ptr(),
            epitrochoidion_new.len() as i64,
            epitrochoidion_value.as_mut_ptr(),
            epitrochoidion_value.len() as i64,
        )
    };
    let epitrochoidion_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            epitrochoidion_value.as_ptr(),
            epitrochoidion_value_written as i64,
            epitrochoidion_needle.as_ptr(),
            epitrochoidion_needle.len() as i64,
        )
    };

    let quarkwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_cardioidium_ptr,
            selected_cardioidium_written as i64,
            quarkwake_extension.as_ptr(),
            quarkwake_extension.len() as i64,
        )
    };
    let mut quarkwake_value = vec![0u8; quarkwake_value_len as usize];
    let quarkwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_cardioidium_ptr,
            selected_cardioidium_written as i64,
            quarkwake_extension.as_ptr(),
            quarkwake_extension.len() as i64,
            quarkwake_value.as_mut_ptr(),
            quarkwake_value.len() as i64,
        )
    };
    let quarkwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            quarkwake_value.as_ptr(),
            quarkwake_value_written as i64,
            quarkwake_needle.as_ptr(),
            quarkwake_needle.len() as i64,
        )
    };

    let frescobrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_cardioidium_ptr,
            selected_cardioidium_written as i64,
        )
    };
    let mut frescobrace_source = vec![0u8; frescobrace_source_len as usize];
    let frescobrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_cardioidium_ptr,
            selected_cardioidium_written as i64,
            frescobrace_source.as_mut_ptr(),
            frescobrace_source.len() as i64,
        )
    };
    let frescobrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            frescobrace_source.as_ptr(),
            frescobrace_source_written as i64,
            frescobrace_old.as_ptr(),
            frescobrace_old.len() as i64,
            frescobrace_new.as_ptr(),
            frescobrace_new.len() as i64,
        )
    };
    let mut frescobrace_value = vec![0u8; frescobrace_value_len as usize];
    let frescobrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            frescobrace_source.as_ptr(),
            frescobrace_source_written as i64,
            frescobrace_old.as_ptr(),
            frescobrace_old.len() as i64,
            frescobrace_new.as_ptr(),
            frescobrace_new.len() as i64,
            frescobrace_value.as_mut_ptr(),
            frescobrace_value.len() as i64,
        )
    };
    let frescobrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            frescobrace_value.as_ptr(),
            frescobrace_value_written as i64,
            frescobrace_needle.as_ptr(),
            frescobrace_needle.len() as i64,
        )
    };

    let mut selected_epitrochoidion_index = 0i32;
    let mut best_epitrochoidion_score = i32::MIN;
    let mut epitrochoidion_index = 0i32;
    while epitrochoidion_index < 3 {
        let mut candidate_epitrochoidion_score = epitrochoidion_value_written * 10 + epitrochoidion_contains * 50;
        if epitrochoidion_index == 1 {
            candidate_epitrochoidion_score = quarkwake_value_written * 10 + quarkwake_index;
        } else if epitrochoidion_index == 2 {
            candidate_epitrochoidion_score = frescobrace_value_written * 10 + frescobrace_contains * 50;
        }

        let mut epitrochoidion_bonus = 0i32;
        if epitrochoidion_index == selected_cardioidium_index {
            epitrochoidion_bonus += 25;
        }
        if epitrochoidion_index == selected_rosecurvium_index {
            epitrochoidion_bonus += 15;
        }
        if epitrochoidion_index == selected_lemniscatoidium_index {
            epitrochoidion_bonus += 5;
        }
        if epitrochoidion_index == 0 && epitrochoidion_contains != 0 {
            epitrochoidion_bonus += 20;
        }
        if epitrochoidion_index == 1 && quarkwake_index >= 0 {
            epitrochoidion_bonus += 10;
        }
        if epitrochoidion_index == 2 && frescobrace_contains != 0 {
            epitrochoidion_bonus += 30;
        }

        let epitrochoidion_score = candidate_epitrochoidion_score + epitrochoidion_bonus;
        if epitrochoidion_score > best_epitrochoidion_score {
            best_epitrochoidion_score = epitrochoidion_score;
            selected_epitrochoidion_index = epitrochoidion_index;
        }

        epitrochoidion_index += 1;
    }

    let mut selected_epitrochoidion_ptr = epitrochoidion_value.as_ptr();
    let mut selected_epitrochoidion_written = epitrochoidion_value_written;
    if selected_epitrochoidion_index == 1 {
        selected_epitrochoidion_ptr = quarkwake_value.as_ptr();
        selected_epitrochoidion_written = quarkwake_value_written;
    } else if selected_epitrochoidion_index == 2 {
        selected_epitrochoidion_ptr = frescobrace_value.as_ptr();
        selected_epitrochoidion_written = frescobrace_value_written;
    }

    let superellipsium_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_epitrochoidion_ptr,
            selected_epitrochoidion_written as i64,
            superellipsium_old.as_ptr(),
            superellipsium_old.len() as i64,
            superellipsium_new.as_ptr(),
            superellipsium_new.len() as i64,
        )
    };
    let mut superellipsium_value = vec![0u8; superellipsium_value_len as usize];
    let superellipsium_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_epitrochoidion_ptr,
            selected_epitrochoidion_written as i64,
            superellipsium_old.as_ptr(),
            superellipsium_old.len() as i64,
            superellipsium_new.as_ptr(),
            superellipsium_new.len() as i64,
            superellipsium_value.as_mut_ptr(),
            superellipsium_value.len() as i64,
        )
    };
    let superellipsium_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            superellipsium_value.as_ptr(),
            superellipsium_value_written as i64,
            superellipsium_needle.as_ptr(),
            superellipsium_needle.len() as i64,
        )
    };

    let hadronwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_epitrochoidion_ptr,
            selected_epitrochoidion_written as i64,
            hadronwake_extension.as_ptr(),
            hadronwake_extension.len() as i64,
        )
    };
    let mut hadronwake_value = vec![0u8; hadronwake_value_len as usize];
    let hadronwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_epitrochoidion_ptr,
            selected_epitrochoidion_written as i64,
            hadronwake_extension.as_ptr(),
            hadronwake_extension.len() as i64,
            hadronwake_value.as_mut_ptr(),
            hadronwake_value.len() as i64,
        )
    };
    let hadronwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            hadronwake_value.as_ptr(),
            hadronwake_value_written as i64,
            hadronwake_needle.as_ptr(),
            hadronwake_needle.len() as i64,
        )
    };

    let cartouchebrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_epitrochoidion_ptr,
            selected_epitrochoidion_written as i64,
        )
    };
    let mut cartouchebrace_source = vec![0u8; cartouchebrace_source_len as usize];
    let cartouchebrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_epitrochoidion_ptr,
            selected_epitrochoidion_written as i64,
            cartouchebrace_source.as_mut_ptr(),
            cartouchebrace_source.len() as i64,
        )
    };
    let cartouchebrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            cartouchebrace_source.as_ptr(),
            cartouchebrace_source_written as i64,
            cartouchebrace_old.as_ptr(),
            cartouchebrace_old.len() as i64,
            cartouchebrace_new.as_ptr(),
            cartouchebrace_new.len() as i64,
        )
    };
    let mut cartouchebrace_value = vec![0u8; cartouchebrace_value_len as usize];
    let cartouchebrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            cartouchebrace_source.as_ptr(),
            cartouchebrace_source_written as i64,
            cartouchebrace_old.as_ptr(),
            cartouchebrace_old.len() as i64,
            cartouchebrace_new.as_ptr(),
            cartouchebrace_new.len() as i64,
            cartouchebrace_value.as_mut_ptr(),
            cartouchebrace_value.len() as i64,
        )
    };
    let cartouchebrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            cartouchebrace_value.as_ptr(),
            cartouchebrace_value_written as i64,
            cartouchebrace_needle.as_ptr(),
            cartouchebrace_needle.len() as i64,
        )
    };

    let mut selected_superellipsium_index = 0i32;
    let mut best_superellipsium_score = i32::MIN;
    let mut superellipsium_index = 0i32;
    while superellipsium_index < 3 {
        let mut candidate_superellipsium_score = superellipsium_value_written * 10 + superellipsium_contains * 50;
        if superellipsium_index == 1 {
            candidate_superellipsium_score = hadronwake_value_written * 10 + hadronwake_index;
        } else if superellipsium_index == 2 {
            candidate_superellipsium_score = cartouchebrace_value_written * 10 + cartouchebrace_contains * 50;
        }

        let mut superellipsium_bonus = 0i32;
        if superellipsium_index == selected_epitrochoidion_index {
            superellipsium_bonus += 25;
        }
        if superellipsium_index == selected_cardioidium_index {
            superellipsium_bonus += 15;
        }
        if superellipsium_index == selected_rosecurvium_index {
            superellipsium_bonus += 5;
        }
        if superellipsium_index == 0 && superellipsium_contains != 0 {
            superellipsium_bonus += 20;
        }
        if superellipsium_index == 1 && hadronwake_index >= 0 {
            superellipsium_bonus += 10;
        }
        if superellipsium_index == 2 && cartouchebrace_contains != 0 {
            superellipsium_bonus += 30;
        }

        let superellipsium_score = candidate_superellipsium_score + superellipsium_bonus;
        if superellipsium_score > best_superellipsium_score {
            best_superellipsium_score = superellipsium_score;
            selected_superellipsium_index = superellipsium_index;
        }

        superellipsium_index += 1;
    }

    let mut selected_superellipsium_ptr = superellipsium_value.as_ptr();
    let mut selected_superellipsium_written = superellipsium_value_written;
    if selected_superellipsium_index == 1 {
        selected_superellipsium_ptr = hadronwake_value.as_ptr();
        selected_superellipsium_written = hadronwake_value_written;
    } else if selected_superellipsium_index == 2 {
        selected_superellipsium_ptr = cartouchebrace_value.as_ptr();
        selected_superellipsium_written = cartouchebrace_value_written;
    }

    let hypocycloidette_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_superellipsium_ptr,
            selected_superellipsium_written as i64,
            hypocycloidette_old.as_ptr(),
            hypocycloidette_old.len() as i64,
            hypocycloidette_new.as_ptr(),
            hypocycloidette_new.len() as i64,
        )
    };
    let mut hypocycloidette_value = vec![0u8; hypocycloidette_value_len as usize];
    let hypocycloidette_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_superellipsium_ptr,
            selected_superellipsium_written as i64,
            hypocycloidette_old.as_ptr(),
            hypocycloidette_old.len() as i64,
            hypocycloidette_new.as_ptr(),
            hypocycloidette_new.len() as i64,
            hypocycloidette_value.as_mut_ptr(),
            hypocycloidette_value.len() as i64,
        )
    };
    let hypocycloidette_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            hypocycloidette_value.as_ptr(),
            hypocycloidette_value_written as i64,
            hypocycloidette_needle.as_ptr(),
            hypocycloidette_needle.len() as i64,
        )
    };

    let mesonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_superellipsium_ptr,
            selected_superellipsium_written as i64,
            mesonwake_extension.as_ptr(),
            mesonwake_extension.len() as i64,
        )
    };
    let mut mesonwake_value = vec![0u8; mesonwake_value_len as usize];
    let mesonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_superellipsium_ptr,
            selected_superellipsium_written as i64,
            mesonwake_extension.as_ptr(),
            mesonwake_extension.len() as i64,
            mesonwake_value.as_mut_ptr(),
            mesonwake_value.len() as i64,
        )
    };
    let mesonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            mesonwake_value.as_ptr(),
            mesonwake_value_written as i64,
            mesonwake_needle.as_ptr(),
            mesonwake_needle.len() as i64,
        )
    };

    let basreliefbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_superellipsium_ptr,
            selected_superellipsium_written as i64,
        )
    };
    let mut basreliefbrace_source = vec![0u8; basreliefbrace_source_len as usize];
    let basreliefbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_superellipsium_ptr,
            selected_superellipsium_written as i64,
            basreliefbrace_source.as_mut_ptr(),
            basreliefbrace_source.len() as i64,
        )
    };
    let basreliefbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            basreliefbrace_source.as_ptr(),
            basreliefbrace_source_written as i64,
            basreliefbrace_old.as_ptr(),
            basreliefbrace_old.len() as i64,
            basreliefbrace_new.as_ptr(),
            basreliefbrace_new.len() as i64,
        )
    };
    let mut basreliefbrace_value = vec![0u8; basreliefbrace_value_len as usize];
    let basreliefbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            basreliefbrace_source.as_ptr(),
            basreliefbrace_source_written as i64,
            basreliefbrace_old.as_ptr(),
            basreliefbrace_old.len() as i64,
            basreliefbrace_new.as_ptr(),
            basreliefbrace_new.len() as i64,
            basreliefbrace_value.as_mut_ptr(),
            basreliefbrace_value.len() as i64,
        )
    };
    let basreliefbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            basreliefbrace_value.as_ptr(),
            basreliefbrace_value_written as i64,
            basreliefbrace_needle.as_ptr(),
            basreliefbrace_needle.len() as i64,
        )
    };

    let mut selected_hypocycloidette_index = 0i32;
    let mut best_hypocycloidette_score = i32::MIN;
    let mut hypocycloidette_index = 0i32;
    while hypocycloidette_index < 3 {
        let mut candidate_hypocycloidette_score =
            hypocycloidette_value_written * 10 + hypocycloidette_contains * 50;
        if hypocycloidette_index == 1 {
            candidate_hypocycloidette_score = mesonwake_value_written * 10 + mesonwake_index;
        } else if hypocycloidette_index == 2 {
            candidate_hypocycloidette_score =
                basreliefbrace_value_written * 10 + basreliefbrace_contains * 50;
        }

        let mut hypocycloidette_bonus = 0i32;
        if hypocycloidette_index == selected_superellipsium_index {
            hypocycloidette_bonus += 25;
        }
        if hypocycloidette_index == selected_epitrochoidion_index {
            hypocycloidette_bonus += 15;
        }
        if hypocycloidette_index == selected_cardioidium_index {
            hypocycloidette_bonus += 5;
        }
        if hypocycloidette_index == 0 && hypocycloidette_contains != 0 {
            hypocycloidette_bonus += 20;
        }
        if hypocycloidette_index == 1 && mesonwake_index >= 0 {
            hypocycloidette_bonus += 10;
        }
        if hypocycloidette_index == 2 && basreliefbrace_contains != 0 {
            hypocycloidette_bonus += 30;
        }

        let hypocycloidette_score = candidate_hypocycloidette_score + hypocycloidette_bonus;
        if hypocycloidette_score > best_hypocycloidette_score {
            best_hypocycloidette_score = hypocycloidette_score;
            selected_hypocycloidette_index = hypocycloidette_index;
        }

        hypocycloidette_index += 1;
    }

    let mut selected_hypocycloidette_ptr = hypocycloidette_value.as_ptr();
    let mut selected_hypocycloidette_written = hypocycloidette_value_written;
    if selected_hypocycloidette_index == 1 {
        selected_hypocycloidette_ptr = mesonwake_value.as_ptr();
        selected_hypocycloidette_written = mesonwake_value_written;
    } else if selected_hypocycloidette_index == 2 {
        selected_hypocycloidette_ptr = basreliefbrace_value.as_ptr();
        selected_hypocycloidette_written = basreliefbrace_value_written;
    }

    let peritrochoidette_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_hypocycloidette_ptr,
            selected_hypocycloidette_written as i64,
            peritrochoidette_old.as_ptr(),
            peritrochoidette_old.len() as i64,
            peritrochoidette_new.as_ptr(),
            peritrochoidette_new.len() as i64,
        )
    };
    let mut peritrochoidette_value = vec![0u8; peritrochoidette_value_len as usize];
    let peritrochoidette_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_hypocycloidette_ptr,
            selected_hypocycloidette_written as i64,
            peritrochoidette_old.as_ptr(),
            peritrochoidette_old.len() as i64,
            peritrochoidette_new.as_ptr(),
            peritrochoidette_new.len() as i64,
            peritrochoidette_value.as_mut_ptr(),
            peritrochoidette_value.len() as i64,
        )
    };
    let peritrochoidette_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            peritrochoidette_value.as_ptr(),
            peritrochoidette_value_written as i64,
            peritrochoidette_needle.as_ptr(),
            peritrochoidette_needle.len() as i64,
        )
    };

    let gluinowake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_hypocycloidette_ptr,
            selected_hypocycloidette_written as i64,
            gluinowake_extension.as_ptr(),
            gluinowake_extension.len() as i64,
        )
    };
    let mut gluinowake_value = vec![0u8; gluinowake_value_len as usize];
    let gluinowake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_hypocycloidette_ptr,
            selected_hypocycloidette_written as i64,
            gluinowake_extension.as_ptr(),
            gluinowake_extension.len() as i64,
            gluinowake_value.as_mut_ptr(),
            gluinowake_value.len() as i64,
        )
    };
    let gluinowake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            gluinowake_value.as_ptr(),
            gluinowake_value_written as i64,
            gluinowake_needle.as_ptr(),
            gluinowake_needle.len() as i64,
        )
    };

    let intagliobrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_hypocycloidette_ptr,
            selected_hypocycloidette_written as i64,
        )
    };
    let mut intagliobrace_source = vec![0u8; intagliobrace_source_len as usize];
    let intagliobrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_hypocycloidette_ptr,
            selected_hypocycloidette_written as i64,
            intagliobrace_source.as_mut_ptr(),
            intagliobrace_source.len() as i64,
        )
    };
    let intagliobrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            intagliobrace_source.as_ptr(),
            intagliobrace_source_written as i64,
            intagliobrace_old.as_ptr(),
            intagliobrace_old.len() as i64,
            intagliobrace_new.as_ptr(),
            intagliobrace_new.len() as i64,
        )
    };
    let mut intagliobrace_value = vec![0u8; intagliobrace_value_len as usize];
    let intagliobrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            intagliobrace_source.as_ptr(),
            intagliobrace_source_written as i64,
            intagliobrace_old.as_ptr(),
            intagliobrace_old.len() as i64,
            intagliobrace_new.as_ptr(),
            intagliobrace_new.len() as i64,
            intagliobrace_value.as_mut_ptr(),
            intagliobrace_value.len() as i64,
        )
    };
    let intagliobrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            intagliobrace_value.as_ptr(),
            intagliobrace_value_written as i64,
            intagliobrace_needle.as_ptr(),
            intagliobrace_needle.len() as i64,
        )
    };

    let mut selected_peritrochoidette_index = 0i32;
    let mut best_peritrochoidette_score = i32::MIN;
    let mut peritrochoidette_index = 0i32;
    while peritrochoidette_index < 3 {
        let mut candidate_peritrochoidette_score =
            peritrochoidette_value_written * 10 + peritrochoidette_contains * 50;
        if peritrochoidette_index == 1 {
            candidate_peritrochoidette_score = gluinowake_value_written * 10 + gluinowake_index;
        } else if peritrochoidette_index == 2 {
            candidate_peritrochoidette_score =
                intagliobrace_value_written * 10 + intagliobrace_contains * 50;
        }

        let mut peritrochoidette_bonus = 0i32;
        if peritrochoidette_index == selected_hypocycloidette_index {
            peritrochoidette_bonus += 25;
        }
        if peritrochoidette_index == selected_superellipsium_index {
            peritrochoidette_bonus += 15;
        }
        if peritrochoidette_index == selected_epitrochoidion_index {
            peritrochoidette_bonus += 5;
        }
        if peritrochoidette_index == 0 && peritrochoidette_contains != 0 {
            peritrochoidette_bonus += 20;
        }
        if peritrochoidette_index == 1 && gluinowake_index >= 0 {
            peritrochoidette_bonus += 10;
        }
        if peritrochoidette_index == 2 && intagliobrace_contains != 0 {
            peritrochoidette_bonus += 30;
        }

        let peritrochoidette_score = candidate_peritrochoidette_score + peritrochoidette_bonus;
        if peritrochoidette_score > best_peritrochoidette_score {
            best_peritrochoidette_score = peritrochoidette_score;
            selected_peritrochoidette_index = peritrochoidette_index;
        }

        peritrochoidette_index += 1;
    }

    let mut selected_peritrochoidette_ptr = peritrochoidette_value.as_ptr();
    let mut selected_peritrochoidette_written = peritrochoidette_value_written;
    if selected_peritrochoidette_index == 1 {
        selected_peritrochoidette_ptr = gluinowake_value.as_ptr();
        selected_peritrochoidette_written = gluinowake_value_written;
    } else if selected_peritrochoidette_index == 2 {
        selected_peritrochoidette_ptr = intagliobrace_value.as_ptr();
        selected_peritrochoidette_written = intagliobrace_value_written;
    }

    let trochoidetta_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_peritrochoidette_ptr,
            selected_peritrochoidette_written as i64,
            trochoidetta_old.as_ptr(),
            trochoidetta_old.len() as i64,
            trochoidetta_new.as_ptr(),
            trochoidetta_new.len() as i64,
        )
    };
    let mut trochoidetta_value = vec![0u8; trochoidetta_value_len as usize];
    let trochoidetta_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_peritrochoidette_ptr,
            selected_peritrochoidette_written as i64,
            trochoidetta_old.as_ptr(),
            trochoidetta_old.len() as i64,
            trochoidetta_new.as_ptr(),
            trochoidetta_new.len() as i64,
            trochoidetta_value.as_mut_ptr(),
            trochoidetta_value.len() as i64,
        )
    };
    let trochoidetta_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            trochoidetta_value.as_ptr(),
            trochoidetta_value_written as i64,
            trochoidetta_needle.as_ptr(),
            trochoidetta_needle.len() as i64,
        )
    };

    let sfermionwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_peritrochoidette_ptr,
            selected_peritrochoidette_written as i64,
            sfermionwake_extension.as_ptr(),
            sfermionwake_extension.len() as i64,
        )
    };
    let mut sfermionwake_value = vec![0u8; sfermionwake_value_len as usize];
    let sfermionwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_peritrochoidette_ptr,
            selected_peritrochoidette_written as i64,
            sfermionwake_extension.as_ptr(),
            sfermionwake_extension.len() as i64,
            sfermionwake_value.as_mut_ptr(),
            sfermionwake_value.len() as i64,
        )
    };
    let sfermionwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            sfermionwake_value.as_ptr(),
            sfermionwake_value_written as i64,
            sfermionwake_needle.as_ptr(),
            sfermionwake_needle.len() as i64,
        )
    };

    let grisaillebrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_peritrochoidette_ptr,
            selected_peritrochoidette_written as i64,
        )
    };
    let mut grisaillebrace_source = vec![0u8; grisaillebrace_source_len as usize];
    let grisaillebrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_peritrochoidette_ptr,
            selected_peritrochoidette_written as i64,
            grisaillebrace_source.as_mut_ptr(),
            grisaillebrace_source.len() as i64,
        )
    };
    let grisaillebrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            grisaillebrace_source.as_ptr(),
            grisaillebrace_source_written as i64,
            grisaillebrace_old.as_ptr(),
            grisaillebrace_old.len() as i64,
            grisaillebrace_new.as_ptr(),
            grisaillebrace_new.len() as i64,
        )
    };
    let mut grisaillebrace_value = vec![0u8; grisaillebrace_value_len as usize];
    let grisaillebrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            grisaillebrace_source.as_ptr(),
            grisaillebrace_source_written as i64,
            grisaillebrace_old.as_ptr(),
            grisaillebrace_old.len() as i64,
            grisaillebrace_new.as_ptr(),
            grisaillebrace_new.len() as i64,
            grisaillebrace_value.as_mut_ptr(),
            grisaillebrace_value.len() as i64,
        )
    };
    let grisaillebrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            grisaillebrace_value.as_ptr(),
            grisaillebrace_value_written as i64,
            grisaillebrace_needle.as_ptr(),
            grisaillebrace_needle.len() as i64,
        )
    };

    let mut selected_trochoidetta_index = 0i32;
    let mut best_trochoidetta_score = i32::MIN;
    let mut trochoidetta_index = 0i32;
    while trochoidetta_index < 3 {
        let mut candidate_trochoidetta_score =
            trochoidetta_value_written * 10 + trochoidetta_contains * 50;
        if trochoidetta_index == 1 {
            candidate_trochoidetta_score = sfermionwake_value_written * 10 + sfermionwake_index;
        } else if trochoidetta_index == 2 {
            candidate_trochoidetta_score =
                grisaillebrace_value_written * 10 + grisaillebrace_contains * 50;
        }

        let mut trochoidetta_bonus = 0i32;
        if trochoidetta_index == selected_peritrochoidette_index {
            trochoidetta_bonus += 25;
        }
        if trochoidetta_index == selected_hypocycloidette_index {
            trochoidetta_bonus += 15;
        }
        if trochoidetta_index == selected_superellipsium_index {
            trochoidetta_bonus += 5;
        }
        if trochoidetta_index == 0 && trochoidetta_contains != 0 {
            trochoidetta_bonus += 20;
        }
        if trochoidetta_index == 1 && sfermionwake_index >= 0 {
            trochoidetta_bonus += 10;
        }
        if trochoidetta_index == 2 && grisaillebrace_contains != 0 {
            trochoidetta_bonus += 30;
        }

        let trochoidetta_score = candidate_trochoidetta_score + trochoidetta_bonus;
        if trochoidetta_score > best_trochoidetta_score {
            best_trochoidetta_score = trochoidetta_score;
            selected_trochoidetta_index = trochoidetta_index;
        }

        trochoidetta_index += 1;
    }

    let mut selected_trochoidetta_ptr = trochoidetta_value.as_ptr();
    let mut selected_trochoidetta_written = trochoidetta_value_written;
    if selected_trochoidetta_index == 1 {
        selected_trochoidetta_ptr = sfermionwake_value.as_ptr();
        selected_trochoidetta_written = sfermionwake_value_written;
    } else if selected_trochoidetta_index == 2 {
        selected_trochoidetta_ptr = grisaillebrace_value.as_ptr();
        selected_trochoidetta_written = grisaillebrace_value_written;
    }

    let epitrochoidetta_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_trochoidetta_ptr,
            selected_trochoidetta_written as i64,
            epitrochoidetta_old.as_ptr(),
            epitrochoidetta_old.len() as i64,
            epitrochoidetta_new.as_ptr(),
            epitrochoidetta_new.len() as i64,
        )
    };
    let mut epitrochoidetta_value = vec![0u8; epitrochoidetta_value_len as usize];
    let epitrochoidetta_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_trochoidetta_ptr,
            selected_trochoidetta_written as i64,
            epitrochoidetta_old.as_ptr(),
            epitrochoidetta_old.len() as i64,
            epitrochoidetta_new.as_ptr(),
            epitrochoidetta_new.len() as i64,
            epitrochoidetta_value.as_mut_ptr(),
            epitrochoidetta_value.len() as i64,
        )
    };
    let epitrochoidetta_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            epitrochoidetta_value.as_ptr(),
            epitrochoidetta_value_written as i64,
            epitrochoidetta_needle.as_ptr(),
            epitrochoidetta_needle.len() as i64,
        )
    };

    let tachyonettewake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_trochoidetta_ptr,
            selected_trochoidetta_written as i64,
            tachyonettewake_extension.as_ptr(),
            tachyonettewake_extension.len() as i64,
        )
    };
    let mut tachyonettewake_value = vec![0u8; tachyonettewake_value_len as usize];
    let tachyonettewake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_trochoidetta_ptr,
            selected_trochoidetta_written as i64,
            tachyonettewake_extension.as_ptr(),
            tachyonettewake_extension.len() as i64,
            tachyonettewake_value.as_mut_ptr(),
            tachyonettewake_value.len() as i64,
        )
    };
    let tachyonettewake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            tachyonettewake_value.as_ptr(),
            tachyonettewake_value_written as i64,
            tachyonettewake_needle.as_ptr(),
            tachyonettewake_needle.len() as i64,
        )
    };

    let filletbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_trochoidetta_ptr,
            selected_trochoidetta_written as i64,
        )
    };
    let mut filletbrace_source = vec![0u8; filletbrace_source_len as usize];
    let filletbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_trochoidetta_ptr,
            selected_trochoidetta_written as i64,
            filletbrace_source.as_mut_ptr(),
            filletbrace_source.len() as i64,
        )
    };
    let filletbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            filletbrace_source.as_ptr(),
            filletbrace_source_written as i64,
            filletbrace_old.as_ptr(),
            filletbrace_old.len() as i64,
            filletbrace_new.as_ptr(),
            filletbrace_new.len() as i64,
        )
    };
    let mut filletbrace_value = vec![0u8; filletbrace_value_len as usize];
    let filletbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            filletbrace_source.as_ptr(),
            filletbrace_source_written as i64,
            filletbrace_old.as_ptr(),
            filletbrace_old.len() as i64,
            filletbrace_new.as_ptr(),
            filletbrace_new.len() as i64,
            filletbrace_value.as_mut_ptr(),
            filletbrace_value.len() as i64,
        )
    };
    let filletbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            filletbrace_value.as_ptr(),
            filletbrace_value_written as i64,
            filletbrace_needle.as_ptr(),
            filletbrace_needle.len() as i64,
        )
    };

    let mut selected_epitrochoidetta_index = 0i32;
    let mut best_epitrochoidetta_score = i32::MIN;
    let mut epitrochoidetta_index = 0i32;
    while epitrochoidetta_index < 3 {
        let mut candidate_epitrochoidetta_score =
            epitrochoidetta_value_written * 10 + epitrochoidetta_contains * 50;
        if epitrochoidetta_index == 1 {
            candidate_epitrochoidetta_score =
                tachyonettewake_value_written * 10 + tachyonettewake_index;
        } else if epitrochoidetta_index == 2 {
            candidate_epitrochoidetta_score =
                filletbrace_value_written * 10 + filletbrace_contains * 50;
        }

        let mut epitrochoidetta_bonus = 0i32;
        if epitrochoidetta_index == selected_trochoidetta_index {
            epitrochoidetta_bonus += 25;
        }
        if epitrochoidetta_index == selected_peritrochoidette_index {
            epitrochoidetta_bonus += 15;
        }
        if epitrochoidetta_index == selected_hypocycloidette_index {
            epitrochoidetta_bonus += 5;
        }
        if epitrochoidetta_index == 0 && epitrochoidetta_contains != 0 {
            epitrochoidetta_bonus += 20;
        }
        if epitrochoidetta_index == 1 && tachyonettewake_index >= 0 {
            epitrochoidetta_bonus += 10;
        }
        if epitrochoidetta_index == 2 && filletbrace_contains != 0 {
            epitrochoidetta_bonus += 30;
        }

        let epitrochoidetta_score = candidate_epitrochoidetta_score + epitrochoidetta_bonus;
        if epitrochoidetta_score > best_epitrochoidetta_score {
            best_epitrochoidetta_score = epitrochoidetta_score;
            selected_epitrochoidetta_index = epitrochoidetta_index;
        }

        epitrochoidetta_index += 1;
    }

    let mut selected_epitrochoidetta_ptr = epitrochoidetta_value.as_ptr();
    let mut selected_epitrochoidetta_written = epitrochoidetta_value_written;
    if selected_epitrochoidetta_index == 1 {
        selected_epitrochoidetta_ptr = tachyonettewake_value.as_ptr();
        selected_epitrochoidetta_written = tachyonettewake_value_written;
    } else if selected_epitrochoidetta_index == 2 {
        selected_epitrochoidetta_ptr = filletbrace_value.as_ptr();
        selected_epitrochoidetta_written = filletbrace_value_written;
    }

    let hypotrochoidetta_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_epitrochoidetta_ptr,
            selected_epitrochoidetta_written as i64,
            hypotrochoidetta_old.as_ptr(),
            hypotrochoidetta_old.len() as i64,
            hypotrochoidetta_new.as_ptr(),
            hypotrochoidetta_new.len() as i64,
        )
    };
    let mut hypotrochoidetta_value = vec![0u8; hypotrochoidetta_value_len as usize];
    let hypotrochoidetta_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_epitrochoidetta_ptr,
            selected_epitrochoidetta_written as i64,
            hypotrochoidetta_old.as_ptr(),
            hypotrochoidetta_old.len() as i64,
            hypotrochoidetta_new.as_ptr(),
            hypotrochoidetta_new.len() as i64,
            hypotrochoidetta_value.as_mut_ptr(),
            hypotrochoidetta_value.len() as i64,
        )
    };
    let hypotrochoidetta_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            hypotrochoidetta_value.as_ptr(),
            hypotrochoidetta_value_written as i64,
            hypotrochoidetta_needle.as_ptr(),
            hypotrochoidetta_needle.len() as i64,
        )
    };

    let quintonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_epitrochoidetta_ptr,
            selected_epitrochoidetta_written as i64,
            quintonwake_extension.as_ptr(),
            quintonwake_extension.len() as i64,
        )
    };
    let mut quintonwake_value = vec![0u8; quintonwake_value_len as usize];
    let quintonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_epitrochoidetta_ptr,
            selected_epitrochoidetta_written as i64,
            quintonwake_extension.as_ptr(),
            quintonwake_extension.len() as i64,
            quintonwake_value.as_mut_ptr(),
            quintonwake_value.len() as i64,
        )
    };
    let quintonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            quintonwake_value.as_ptr(),
            quintonwake_value_written as i64,
            quintonwake_needle.as_ptr(),
            quintonwake_needle.len() as i64,
        )
    };

    let volutebrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_epitrochoidetta_ptr,
            selected_epitrochoidetta_written as i64,
        )
    };
    let mut volutebrace_source = vec![0u8; volutebrace_source_len as usize];
    let volutebrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_epitrochoidetta_ptr,
            selected_epitrochoidetta_written as i64,
            volutebrace_source.as_mut_ptr(),
            volutebrace_source.len() as i64,
        )
    };
    let volutebrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            volutebrace_source.as_ptr(),
            volutebrace_source_written as i64,
            volutebrace_old.as_ptr(),
            volutebrace_old.len() as i64,
            volutebrace_new.as_ptr(),
            volutebrace_new.len() as i64,
        )
    };
    let mut volutebrace_value = vec![0u8; volutebrace_value_len as usize];
    let volutebrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            volutebrace_source.as_ptr(),
            volutebrace_source_written as i64,
            volutebrace_old.as_ptr(),
            volutebrace_old.len() as i64,
            volutebrace_new.as_ptr(),
            volutebrace_new.len() as i64,
            volutebrace_value.as_mut_ptr(),
            volutebrace_value.len() as i64,
        )
    };
    let volutebrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            volutebrace_value.as_ptr(),
            volutebrace_value_written as i64,
            volutebrace_needle.as_ptr(),
            volutebrace_needle.len() as i64,
        )
    };

    let mut selected_hypotrochoidetta_index = 0i32;
    let mut best_hypotrochoidetta_score = i32::MIN;
    let mut hypotrochoidetta_index = 0i32;
    while hypotrochoidetta_index < 3 {
        let mut candidate_hypotrochoidetta_score =
            hypotrochoidetta_value_written * 10 + hypotrochoidetta_contains * 50;
        if hypotrochoidetta_index == 1 {
            candidate_hypotrochoidetta_score = quintonwake_value_written * 10 + quintonwake_index;
        } else if hypotrochoidetta_index == 2 {
            candidate_hypotrochoidetta_score =
                volutebrace_value_written * 10 + volutebrace_contains * 50;
        }

        let mut hypotrochoidetta_bonus = 0i32;
        if hypotrochoidetta_index == selected_epitrochoidetta_index {
            hypotrochoidetta_bonus += 25;
        }
        if hypotrochoidetta_index == selected_trochoidetta_index {
            hypotrochoidetta_bonus += 15;
        }
        if hypotrochoidetta_index == selected_peritrochoidette_index {
            hypotrochoidetta_bonus += 5;
        }
        if hypotrochoidetta_index == 0 && hypotrochoidetta_contains != 0 {
            hypotrochoidetta_bonus += 20;
        }
        if hypotrochoidetta_index == 1 && quintonwake_index >= 0 {
            hypotrochoidetta_bonus += 10;
        }
        if hypotrochoidetta_index == 2 && volutebrace_contains != 0 {
            hypotrochoidetta_bonus += 30;
        }

        let hypotrochoidetta_score = candidate_hypotrochoidetta_score + hypotrochoidetta_bonus;
        if hypotrochoidetta_score > best_hypotrochoidetta_score {
            best_hypotrochoidetta_score = hypotrochoidetta_score;
            selected_hypotrochoidetta_index = hypotrochoidetta_index;
        }

        hypotrochoidetta_index += 1;
    }

    let mut selected_hypotrochoidetta_ptr = hypotrochoidetta_value.as_ptr();
    let mut selected_hypotrochoidetta_written = hypotrochoidetta_value_written;
    if selected_hypotrochoidetta_index == 1 {
        selected_hypotrochoidetta_ptr = quintonwake_value.as_ptr();
        selected_hypotrochoidetta_written = quintonwake_value_written;
    } else if selected_hypotrochoidetta_index == 2 {
        selected_hypotrochoidetta_ptr = volutebrace_value.as_ptr();
        selected_hypotrochoidetta_written = volutebrace_value_written;
    }

    let epitrochoidula_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_hypotrochoidetta_ptr,
            selected_hypotrochoidetta_written as i64,
            epitrochoidula_old.as_ptr(),
            epitrochoidula_old.len() as i64,
            epitrochoidula_new.as_ptr(),
            epitrochoidula_new.len() as i64,
        )
    };
    let mut epitrochoidula_value = vec![0u8; epitrochoidula_value_len as usize];
    let epitrochoidula_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_hypotrochoidetta_ptr,
            selected_hypotrochoidetta_written as i64,
            epitrochoidula_old.as_ptr(),
            epitrochoidula_old.len() as i64,
            epitrochoidula_new.as_ptr(),
            epitrochoidula_new.len() as i64,
            epitrochoidula_value.as_mut_ptr(),
            epitrochoidula_value.len() as i64,
        )
    };
    let epitrochoidula_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            epitrochoidula_value.as_ptr(),
            epitrochoidula_value_written as i64,
            epitrochoidula_needle.as_ptr(),
            epitrochoidula_needle.len() as i64,
        )
    };

    let septonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_hypotrochoidetta_ptr,
            selected_hypotrochoidetta_written as i64,
            septonwake_extension.as_ptr(),
            septonwake_extension.len() as i64,
        )
    };
    let mut septonwake_value = vec![0u8; septonwake_value_len as usize];
    let septonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_hypotrochoidetta_ptr,
            selected_hypotrochoidetta_written as i64,
            septonwake_extension.as_ptr(),
            septonwake_extension.len() as i64,
            septonwake_value.as_mut_ptr(),
            septonwake_value.len() as i64,
        )
    };
    let septonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            septonwake_value.as_ptr(),
            septonwake_value_written as i64,
            septonwake_needle.as_ptr(),
            septonwake_needle.len() as i64,
        )
    };

    let guillochebrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_hypotrochoidetta_ptr,
            selected_hypotrochoidetta_written as i64,
        )
    };
    let mut guillochebrace_source = vec![0u8; guillochebrace_source_len as usize];
    let guillochebrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_hypotrochoidetta_ptr,
            selected_hypotrochoidetta_written as i64,
            guillochebrace_source.as_mut_ptr(),
            guillochebrace_source.len() as i64,
        )
    };
    let guillochebrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            guillochebrace_source.as_ptr(),
            guillochebrace_source_written as i64,
            guillochebrace_old.as_ptr(),
            guillochebrace_old.len() as i64,
            guillochebrace_new.as_ptr(),
            guillochebrace_new.len() as i64,
        )
    };
    let mut guillochebrace_value = vec![0u8; guillochebrace_value_len as usize];
    let guillochebrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            guillochebrace_source.as_ptr(),
            guillochebrace_source_written as i64,
            guillochebrace_old.as_ptr(),
            guillochebrace_old.len() as i64,
            guillochebrace_new.as_ptr(),
            guillochebrace_new.len() as i64,
            guillochebrace_value.as_mut_ptr(),
            guillochebrace_value.len() as i64,
        )
    };
    let guillochebrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            guillochebrace_value.as_ptr(),
            guillochebrace_value_written as i64,
            guillochebrace_needle.as_ptr(),
            guillochebrace_needle.len() as i64,
        )
    };

    let mut selected_epitrochoidula_index = 0i32;
    let mut best_epitrochoidula_score = i32::MIN;
    let mut epitrochoidula_index = 0i32;
    while epitrochoidula_index < 3 {
        let mut candidate_epitrochoidula_score =
            epitrochoidula_value_written * 10 + epitrochoidula_contains * 50;
        if epitrochoidula_index == 1 {
            candidate_epitrochoidula_score = septonwake_value_written * 10 + septonwake_index;
        } else if epitrochoidula_index == 2 {
            candidate_epitrochoidula_score =
                guillochebrace_value_written * 10 + guillochebrace_contains * 50;
        }

        let mut epitrochoidula_bonus = 0i32;
        if epitrochoidula_index == selected_hypotrochoidetta_index {
            epitrochoidula_bonus += 25;
        }
        if epitrochoidula_index == selected_epitrochoidetta_index {
            epitrochoidula_bonus += 15;
        }
        if epitrochoidula_index == selected_trochoidetta_index {
            epitrochoidula_bonus += 5;
        }
        if epitrochoidula_index == 0 && epitrochoidula_contains != 0 {
            epitrochoidula_bonus += 20;
        }
        if epitrochoidula_index == 1 && septonwake_index >= 0 {
            epitrochoidula_bonus += 10;
        }
        if epitrochoidula_index == 2 && guillochebrace_contains != 0 {
            epitrochoidula_bonus += 30;
        }

        let epitrochoidula_score = candidate_epitrochoidula_score + epitrochoidula_bonus;
        if epitrochoidula_score > best_epitrochoidula_score {
            best_epitrochoidula_score = epitrochoidula_score;
            selected_epitrochoidula_index = epitrochoidula_index;
        }

        epitrochoidula_index += 1;
    }

    let mut selected_epitrochoidula_ptr = epitrochoidula_value.as_ptr();
    let mut selected_epitrochoidula_written = epitrochoidula_value_written;
    if selected_epitrochoidula_index == 1 {
        selected_epitrochoidula_ptr = septonwake_value.as_ptr();
        selected_epitrochoidula_written = septonwake_value_written;
    } else if selected_epitrochoidula_index == 2 {
        selected_epitrochoidula_ptr = guillochebrace_value.as_ptr();
        selected_epitrochoidula_written = guillochebrace_value_written;
    }

    let orthotrochoidula_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_epitrochoidula_ptr,
            selected_epitrochoidula_written as i64,
            orthotrochoidula_old.as_ptr(),
            orthotrochoidula_old.len() as i64,
            orthotrochoidula_new.as_ptr(),
            orthotrochoidula_new.len() as i64,
        )
    };
    let mut orthotrochoidula_value = vec![0u8; orthotrochoidula_value_len as usize];
    let orthotrochoidula_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_epitrochoidula_ptr,
            selected_epitrochoidula_written as i64,
            orthotrochoidula_old.as_ptr(),
            orthotrochoidula_old.len() as i64,
            orthotrochoidula_new.as_ptr(),
            orthotrochoidula_new.len() as i64,
            orthotrochoidula_value.as_mut_ptr(),
            orthotrochoidula_value.len() as i64,
        )
    };
    let orthotrochoidula_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            orthotrochoidula_value.as_ptr(),
            orthotrochoidula_value_written as i64,
            orthotrochoidula_needle.as_ptr(),
            orthotrochoidula_needle.len() as i64,
        )
    };

    let nononwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_epitrochoidula_ptr,
            selected_epitrochoidula_written as i64,
            nononwake_extension.as_ptr(),
            nononwake_extension.len() as i64,
        )
    };
    let mut nononwake_value = vec![0u8; nononwake_value_len as usize];
    let nononwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_epitrochoidula_ptr,
            selected_epitrochoidula_written as i64,
            nononwake_extension.as_ptr(),
            nononwake_extension.len() as i64,
            nononwake_value.as_mut_ptr(),
            nononwake_value.len() as i64,
        )
    };
    let nononwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            nononwake_value.as_ptr(),
            nononwake_value_written as i64,
            nononwake_needle.as_ptr(),
            nononwake_needle.len() as i64,
        )
    };

    let strapworkbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_epitrochoidula_ptr,
            selected_epitrochoidula_written as i64,
        )
    };
    let mut strapworkbrace_source = vec![0u8; strapworkbrace_source_len as usize];
    let strapworkbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_epitrochoidula_ptr,
            selected_epitrochoidula_written as i64,
            strapworkbrace_source.as_mut_ptr(),
            strapworkbrace_source.len() as i64,
        )
    };
    let strapworkbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            strapworkbrace_source.as_ptr(),
            strapworkbrace_source_written as i64,
            strapworkbrace_old.as_ptr(),
            strapworkbrace_old.len() as i64,
            strapworkbrace_new.as_ptr(),
            strapworkbrace_new.len() as i64,
        )
    };
    let mut strapworkbrace_value = vec![0u8; strapworkbrace_value_len as usize];
    let strapworkbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            strapworkbrace_source.as_ptr(),
            strapworkbrace_source_written as i64,
            strapworkbrace_old.as_ptr(),
            strapworkbrace_old.len() as i64,
            strapworkbrace_new.as_ptr(),
            strapworkbrace_new.len() as i64,
            strapworkbrace_value.as_mut_ptr(),
            strapworkbrace_value.len() as i64,
        )
    };
    let strapworkbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            strapworkbrace_value.as_ptr(),
            strapworkbrace_value_written as i64,
            strapworkbrace_needle.as_ptr(),
            strapworkbrace_needle.len() as i64,
        )
    };

    let mut selected_orthotrochoidula_index = 0i32;
    let mut best_orthotrochoidula_score = i32::MIN;
    let mut orthotrochoidula_index = 0i32;
    while orthotrochoidula_index < 3 {
        let mut candidate_orthotrochoidula_score =
            orthotrochoidula_value_written * 10 + orthotrochoidula_contains * 50;
        if orthotrochoidula_index == 1 {
            candidate_orthotrochoidula_score = nononwake_value_written * 10 + nononwake_index;
        } else if orthotrochoidula_index == 2 {
            candidate_orthotrochoidula_score =
                strapworkbrace_value_written * 10 + strapworkbrace_contains * 50;
        }

        let mut orthotrochoidula_bonus = 0i32;
        if orthotrochoidula_index == selected_epitrochoidula_index {
            orthotrochoidula_bonus += 25;
        }
        if orthotrochoidula_index == selected_hypotrochoidetta_index {
            orthotrochoidula_bonus += 15;
        }
        if orthotrochoidula_index == selected_epitrochoidetta_index {
            orthotrochoidula_bonus += 5;
        }
        if orthotrochoidula_index == 0 && orthotrochoidula_contains != 0 {
            orthotrochoidula_bonus += 20;
        }
        if orthotrochoidula_index == 1 && nononwake_index >= 0 {
            orthotrochoidula_bonus += 10;
        }
        if orthotrochoidula_index == 2 && strapworkbrace_contains != 0 {
            orthotrochoidula_bonus += 30;
        }

        let orthotrochoidula_score = candidate_orthotrochoidula_score + orthotrochoidula_bonus;
        if orthotrochoidula_score > best_orthotrochoidula_score {
            best_orthotrochoidula_score = orthotrochoidula_score;
            selected_orthotrochoidula_index = orthotrochoidula_index;
        }

        orthotrochoidula_index += 1;
    }

    let mut selected_orthotrochoidula_ptr = orthotrochoidula_value.as_ptr();
    let mut selected_orthotrochoidula_written = orthotrochoidula_value_written;
    if selected_orthotrochoidula_index == 1 {
        selected_orthotrochoidula_ptr = nononwake_value.as_ptr();
        selected_orthotrochoidula_written = nononwake_value_written;
    } else if selected_orthotrochoidula_index == 2 {
        selected_orthotrochoidula_ptr = strapworkbrace_value.as_ptr();
        selected_orthotrochoidula_written = strapworkbrace_value_written;
    }

    let deltoidula_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_orthotrochoidula_ptr,
            selected_orthotrochoidula_written as i64,
            deltoidula_old.as_ptr(),
            deltoidula_old.len() as i64,
            deltoidula_new.as_ptr(),
            deltoidula_new.len() as i64,
        )
    };
    let mut deltoidula_value = vec![0u8; deltoidula_value_len as usize];
    let deltoidula_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_orthotrochoidula_ptr,
            selected_orthotrochoidula_written as i64,
            deltoidula_old.as_ptr(),
            deltoidula_old.len() as i64,
            deltoidula_new.as_ptr(),
            deltoidula_new.len() as i64,
            deltoidula_value.as_mut_ptr(),
            deltoidula_value.len() as i64,
        )
    };
    let deltoidula_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            deltoidula_value.as_ptr(),
            deltoidula_value_written as i64,
            deltoidula_needle.as_ptr(),
            deltoidula_needle.len() as i64,
        )
    };

    let decuonwake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_orthotrochoidula_ptr,
            selected_orthotrochoidula_written as i64,
            decuonwake_extension.as_ptr(),
            decuonwake_extension.len() as i64,
        )
    };
    let mut decuonwake_value = vec![0u8; decuonwake_value_len as usize];
    let decuonwake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_orthotrochoidula_ptr,
            selected_orthotrochoidula_written as i64,
            decuonwake_extension.as_ptr(),
            decuonwake_extension.len() as i64,
            decuonwake_value.as_mut_ptr(),
            decuonwake_value.len() as i64,
        )
    };
    let decuonwake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            decuonwake_value.as_ptr(),
            decuonwake_value_written as i64,
            decuonwake_needle.as_ptr(),
            decuonwake_needle.len() as i64,
        )
    };

    let carcanetbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_orthotrochoidula_ptr,
            selected_orthotrochoidula_written as i64,
        )
    };
    let mut carcanetbrace_source = vec![0u8; carcanetbrace_source_len as usize];
    let carcanetbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_orthotrochoidula_ptr,
            selected_orthotrochoidula_written as i64,
            carcanetbrace_source.as_mut_ptr(),
            carcanetbrace_source.len() as i64,
        )
    };
    let carcanetbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            carcanetbrace_source.as_ptr(),
            carcanetbrace_source_written as i64,
            carcanetbrace_old.as_ptr(),
            carcanetbrace_old.len() as i64,
            carcanetbrace_new.as_ptr(),
            carcanetbrace_new.len() as i64,
        )
    };
    let mut carcanetbrace_value = vec![0u8; carcanetbrace_value_len as usize];
    let carcanetbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            carcanetbrace_source.as_ptr(),
            carcanetbrace_source_written as i64,
            carcanetbrace_old.as_ptr(),
            carcanetbrace_old.len() as i64,
            carcanetbrace_new.as_ptr(),
            carcanetbrace_new.len() as i64,
            carcanetbrace_value.as_mut_ptr(),
            carcanetbrace_value.len() as i64,
        )
    };
    let carcanetbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            carcanetbrace_value.as_ptr(),
            carcanetbrace_value_written as i64,
            carcanetbrace_needle.as_ptr(),
            carcanetbrace_needle.len() as i64,
        )
    };

    let mut selected_deltoidula_index = 0i32;
    let mut best_deltoidula_score = i32::MIN;
    let mut deltoidula_index = 0i32;
    while deltoidula_index < 3 {
        let mut candidate_deltoidula_score =
            deltoidula_value_written * 10 + deltoidula_contains * 50;
        if deltoidula_index == 1 {
            candidate_deltoidula_score = decuonwake_value_written * 10 + decuonwake_index;
        } else if deltoidula_index == 2 {
            candidate_deltoidula_score =
                carcanetbrace_value_written * 10 + carcanetbrace_contains * 50;
        }

        let mut deltoidula_bonus = 0i32;
        if deltoidula_index == selected_orthotrochoidula_index {
            deltoidula_bonus += 25;
        }
        if deltoidula_index == selected_epitrochoidula_index {
            deltoidula_bonus += 15;
        }
        if deltoidula_index == selected_hypotrochoidetta_index {
            deltoidula_bonus += 5;
        }
        if deltoidula_index == 0 && deltoidula_contains != 0 {
            deltoidula_bonus += 20;
        }
        if deltoidula_index == 1 && decuonwake_index >= 0 {
            deltoidula_bonus += 10;
        }
        if deltoidula_index == 2 && carcanetbrace_contains != 0 {
            deltoidula_bonus += 30;
        }

        let deltoidula_score = candidate_deltoidula_score + deltoidula_bonus;
        if deltoidula_score > best_deltoidula_score {
            best_deltoidula_score = deltoidula_score;
            selected_deltoidula_index = deltoidula_index;
        }

        deltoidula_index += 1;
    }

    let mut selected_deltoidula_ptr = deltoidula_value.as_ptr();
    let mut selected_deltoidula_written = deltoidula_value_written;
    if selected_deltoidula_index == 1 {
        selected_deltoidula_ptr = decuonwake_value.as_ptr();
        selected_deltoidula_written = decuonwake_value_written;
    } else if selected_deltoidula_index == 2 {
        selected_deltoidula_ptr = carcanetbrace_value.as_ptr();
        selected_deltoidula_written = carcanetbrace_value_written;
    }

    let astroidula_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_deltoidula_ptr,
            selected_deltoidula_written as i64,
            astroidula_old.as_ptr(),
            astroidula_old.len() as i64,
            astroidula_new.as_ptr(),
            astroidula_new.len() as i64,
        )
    };
    let mut astroidula_value = vec![0u8; astroidula_value_len as usize];
    let astroidula_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_deltoidula_ptr,
            selected_deltoidula_written as i64,
            astroidula_old.as_ptr(),
            astroidula_old.len() as i64,
            astroidula_new.as_ptr(),
            astroidula_new.len() as i64,
            astroidula_value.as_mut_ptr(),
            astroidula_value.len() as i64,
        )
    };
    let astroidula_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            astroidula_value.as_ptr(),
            astroidula_value_written as i64,
            astroidula_needle.as_ptr(),
            astroidula_needle.len() as i64,
        )
    };

    let hendecawake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_deltoidula_ptr,
            selected_deltoidula_written as i64,
            hendecawake_extension.as_ptr(),
            hendecawake_extension.len() as i64,
        )
    };
    let mut hendecawake_value = vec![0u8; hendecawake_value_len as usize];
    let hendecawake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_deltoidula_ptr,
            selected_deltoidula_written as i64,
            hendecawake_extension.as_ptr(),
            hendecawake_extension.len() as i64,
            hendecawake_value.as_mut_ptr(),
            hendecawake_value.len() as i64,
        )
    };
    let hendecawake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            hendecawake_value.as_ptr(),
            hendecawake_value_written as i64,
            hendecawake_needle.as_ptr(),
            hendecawake_needle.len() as i64,
        )
    };

    let festoonbrace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_deltoidula_ptr,
            selected_deltoidula_written as i64,
        )
    };
    let mut festoonbrace_source = vec![0u8; festoonbrace_source_len as usize];
    let festoonbrace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_deltoidula_ptr,
            selected_deltoidula_written as i64,
            festoonbrace_source.as_mut_ptr(),
            festoonbrace_source.len() as i64,
        )
    };
    let festoonbrace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            festoonbrace_source.as_ptr(),
            festoonbrace_source_written as i64,
            festoonbrace_old.as_ptr(),
            festoonbrace_old.len() as i64,
            festoonbrace_new.as_ptr(),
            festoonbrace_new.len() as i64,
        )
    };
    let mut festoonbrace_value = vec![0u8; festoonbrace_value_len as usize];
    let festoonbrace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            festoonbrace_source.as_ptr(),
            festoonbrace_source_written as i64,
            festoonbrace_old.as_ptr(),
            festoonbrace_old.len() as i64,
            festoonbrace_new.as_ptr(),
            festoonbrace_new.len() as i64,
            festoonbrace_value.as_mut_ptr(),
            festoonbrace_value.len() as i64,
        )
    };
    let festoonbrace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            festoonbrace_value.as_ptr(),
            festoonbrace_value_written as i64,
            festoonbrace_needle.as_ptr(),
            festoonbrace_needle.len() as i64,
        )
    };

    let mut selected_astroidula_index = 0i32;
    let mut best_astroidula_score = i32::MIN;
    let mut astroidula_index = 0i32;
    while astroidula_index < 3 {
        let mut candidate_astroidula_score =
            astroidula_value_written * 10 + astroidula_contains * 50;
        if astroidula_index == 1 {
            candidate_astroidula_score = hendecawake_value_written * 10 + hendecawake_index;
        } else if astroidula_index == 2 {
            candidate_astroidula_score =
                festoonbrace_value_written * 10 + festoonbrace_contains * 50;
        }

        let mut astroidula_bonus = 0i32;
        if astroidula_index == selected_deltoidula_index {
            astroidula_bonus += 25;
        }
        if astroidula_index == selected_orthotrochoidula_index {
            astroidula_bonus += 15;
        }
        if astroidula_index == selected_epitrochoidula_index {
            astroidula_bonus += 5;
        }
        if astroidula_index == 0 && astroidula_contains != 0 {
            astroidula_bonus += 20;
        }
        if astroidula_index == 1 && hendecawake_index >= 0 {
            astroidula_bonus += 10;
        }
        if astroidula_index == 2 && festoonbrace_contains != 0 {
            astroidula_bonus += 30;
        }

        let astroidula_score = candidate_astroidula_score + astroidula_bonus;
        if astroidula_score > best_astroidula_score {
            best_astroidula_score = astroidula_score;
            selected_astroidula_index = astroidula_index;
        }

        astroidula_index += 1;
    }

    let mut selected_astroidula_ptr = astroidula_value.as_ptr();
    let mut selected_astroidula_written = astroidula_value_written;
    if selected_astroidula_index == 1 {
        selected_astroidula_ptr = hendecawake_value.as_ptr();
        selected_astroidula_written = hendecawake_value_written;
    } else if selected_astroidula_index == 2 {
        selected_astroidula_ptr = festoonbrace_value.as_ptr();
        selected_astroidula_written = festoonbrace_value_written;
    }

    let lemniscatula_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_astroidula_ptr,
            selected_astroidula_written as i64,
            lemniscatula_old.as_ptr(),
            lemniscatula_old.len() as i64,
            lemniscatula_new.as_ptr(),
            lemniscatula_new.len() as i64,
        )
    };
    let mut lemniscatula_value = vec![0u8; lemniscatula_value_len as usize];
    let lemniscatula_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_astroidula_ptr,
            selected_astroidula_written as i64,
            lemniscatula_old.as_ptr(),
            lemniscatula_old.len() as i64,
            lemniscatula_new.as_ptr(),
            lemniscatula_new.len() as i64,
            lemniscatula_value.as_mut_ptr(),
            lemniscatula_value.len() as i64,
        )
    };
    let lemniscatula_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            lemniscatula_value.as_ptr(),
            lemniscatula_value_written as i64,
            lemniscatula_needle.as_ptr(),
            lemniscatula_needle.len() as i64,
        )
    };

    let duodecawake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_astroidula_ptr,
            selected_astroidula_written as i64,
            duodecawake_extension.as_ptr(),
            duodecawake_extension.len() as i64,
        )
    };
    let mut duodecawake_value = vec![0u8; duodecawake_value_len as usize];
    let duodecawake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_astroidula_ptr,
            selected_astroidula_written as i64,
            duodecawake_extension.as_ptr(),
            duodecawake_extension.len() as i64,
            duodecawake_value.as_mut_ptr(),
            duodecawake_value.len() as i64,
        )
    };
    let duodecawake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            duodecawake_value.as_ptr(),
            duodecawake_value_written as i64,
            duodecawake_needle.as_ptr(),
            duodecawake_needle.len() as i64,
        )
    };

    let cartoucheband_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_astroidula_ptr,
            selected_astroidula_written as i64,
        )
    };
    let mut cartoucheband_source = vec![0u8; cartoucheband_source_len as usize];
    let cartoucheband_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_astroidula_ptr,
            selected_astroidula_written as i64,
            cartoucheband_source.as_mut_ptr(),
            cartoucheband_source.len() as i64,
        )
    };
    let cartoucheband_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            cartoucheband_source.as_ptr(),
            cartoucheband_source_written as i64,
            cartoucheband_old.as_ptr(),
            cartoucheband_old.len() as i64,
            cartoucheband_new.as_ptr(),
            cartoucheband_new.len() as i64,
        )
    };
    let mut cartoucheband_value = vec![0u8; cartoucheband_value_len as usize];
    let cartoucheband_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            cartoucheband_source.as_ptr(),
            cartoucheband_source_written as i64,
            cartoucheband_old.as_ptr(),
            cartoucheband_old.len() as i64,
            cartoucheband_new.as_ptr(),
            cartoucheband_new.len() as i64,
            cartoucheband_value.as_mut_ptr(),
            cartoucheband_value.len() as i64,
        )
    };
    let cartoucheband_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            cartoucheband_value.as_ptr(),
            cartoucheband_value_written as i64,
            cartoucheband_needle.as_ptr(),
            cartoucheband_needle.len() as i64,
        )
    };

    let mut selected_lemniscatula_index = 0i32;
    let mut best_lemniscatula_score = i32::MIN;
    let mut lemniscatula_index = 0i32;
    while lemniscatula_index < 3 {
        let mut candidate_lemniscatula_score =
            lemniscatula_value_written * 10 + lemniscatula_contains * 50;
        if lemniscatula_index == 1 {
            candidate_lemniscatula_score = duodecawake_value_written * 10 + duodecawake_index;
        } else if lemniscatula_index == 2 {
            candidate_lemniscatula_score =
                cartoucheband_value_written * 10 + cartoucheband_contains * 50;
        }

        let mut lemniscatula_bonus = 0i32;
        if lemniscatula_index == selected_astroidula_index {
            lemniscatula_bonus += 25;
        }
        if lemniscatula_index == selected_deltoidula_index {
            lemniscatula_bonus += 15;
        }
        if lemniscatula_index == selected_orthotrochoidula_index {
            lemniscatula_bonus += 5;
        }
        if lemniscatula_index == 0 && lemniscatula_contains != 0 {
            lemniscatula_bonus += 20;
        }
        if lemniscatula_index == 1 && duodecawake_index >= 0 {
            lemniscatula_bonus += 10;
        }
        if lemniscatula_index == 2 && cartoucheband_contains != 0 {
            lemniscatula_bonus += 30;
        }

        let lemniscatula_score = candidate_lemniscatula_score + lemniscatula_bonus;
        if lemniscatula_score > best_lemniscatula_score {
            best_lemniscatula_score = lemniscatula_score;
            selected_lemniscatula_index = lemniscatula_index;
        }

        lemniscatula_index += 1;
    }

    let mut selected_lemniscatula_ptr = lemniscatula_value.as_ptr();
    let mut selected_lemniscatula_written = lemniscatula_value_written;
    if selected_lemniscatula_index == 1 {
        selected_lemniscatula_ptr = duodecawake_value.as_ptr();
        selected_lemniscatula_written = duodecawake_value_written;
    } else if selected_lemniscatula_index == 2 {
        selected_lemniscatula_ptr = cartoucheband_value.as_ptr();
        selected_lemniscatula_written = cartoucheband_value_written;
    }

    let rosecurvula_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_lemniscatula_ptr,
            selected_lemniscatula_written as i64,
            rosecurvula_old.as_ptr(),
            rosecurvula_old.len() as i64,
            rosecurvula_new.as_ptr(),
            rosecurvula_new.len() as i64,
        )
    };
    let mut rosecurvula_value = vec![0u8; rosecurvula_value_len as usize];
    let rosecurvula_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_lemniscatula_ptr,
            selected_lemniscatula_written as i64,
            rosecurvula_old.as_ptr(),
            rosecurvula_old.len() as i64,
            rosecurvula_new.as_ptr(),
            rosecurvula_new.len() as i64,
            rosecurvula_value.as_mut_ptr(),
            rosecurvula_value.len() as i64,
        )
    };
    let rosecurvula_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            rosecurvula_value.as_ptr(),
            rosecurvula_value_written as i64,
            rosecurvula_needle.as_ptr(),
            rosecurvula_needle.len() as i64,
        )
    };

    let tridecawake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_lemniscatula_ptr,
            selected_lemniscatula_written as i64,
            tridecawake_extension.as_ptr(),
            tridecawake_extension.len() as i64,
        )
    };
    let mut tridecawake_value = vec![0u8; tridecawake_value_len as usize];
    let tridecawake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_lemniscatula_ptr,
            selected_lemniscatula_written as i64,
            tridecawake_extension.as_ptr(),
            tridecawake_extension.len() as i64,
            tridecawake_value.as_mut_ptr(),
            tridecawake_value.len() as i64,
        )
    };
    let tridecawake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            tridecawake_value.as_ptr(),
            tridecawake_value_written as i64,
            tridecawake_needle.as_ptr(),
            tridecawake_needle.len() as i64,
        )
    };

    let cinctureband_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_lemniscatula_ptr,
            selected_lemniscatula_written as i64,
        )
    };
    let mut cinctureband_source = vec![0u8; cinctureband_source_len as usize];
    let cinctureband_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_lemniscatula_ptr,
            selected_lemniscatula_written as i64,
            cinctureband_source.as_mut_ptr(),
            cinctureband_source.len() as i64,
        )
    };
    let cinctureband_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            cinctureband_source.as_ptr(),
            cinctureband_source_written as i64,
            cinctureband_old.as_ptr(),
            cinctureband_old.len() as i64,
            cinctureband_new.as_ptr(),
            cinctureband_new.len() as i64,
        )
    };
    let mut cinctureband_value = vec![0u8; cinctureband_value_len as usize];
    let cinctureband_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            cinctureband_source.as_ptr(),
            cinctureband_source_written as i64,
            cinctureband_old.as_ptr(),
            cinctureband_old.len() as i64,
            cinctureband_new.as_ptr(),
            cinctureband_new.len() as i64,
            cinctureband_value.as_mut_ptr(),
            cinctureband_value.len() as i64,
        )
    };
    let cinctureband_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            cinctureband_value.as_ptr(),
            cinctureband_value_written as i64,
            cinctureband_needle.as_ptr(),
            cinctureband_needle.len() as i64,
        )
    };

    let mut selected_rosecurvula_index = 0i32;
    let mut best_rosecurvula_score = i32::MIN;
    let mut rosecurvula_index = 0i32;
    while rosecurvula_index < 3 {
        let mut candidate_rosecurvula_score =
            rosecurvula_value_written * 10 + rosecurvula_contains * 50;
        if rosecurvula_index == 1 {
            candidate_rosecurvula_score = tridecawake_value_written * 10 + tridecawake_index;
        } else if rosecurvula_index == 2 {
            candidate_rosecurvula_score =
                cinctureband_value_written * 10 + cinctureband_contains * 50;
        }

        let mut rosecurvula_bonus = 0i32;
        if rosecurvula_index == selected_lemniscatula_index {
            rosecurvula_bonus += 25;
        }
        if rosecurvula_index == selected_astroidula_index {
            rosecurvula_bonus += 15;
        }
        if rosecurvula_index == selected_deltoidula_index {
            rosecurvula_bonus += 5;
        }
        if rosecurvula_index == 0 && rosecurvula_contains != 0 {
            rosecurvula_bonus += 20;
        }
        if rosecurvula_index == 1 && tridecawake_index >= 0 {
            rosecurvula_bonus += 10;
        }
        if rosecurvula_index == 2 && cinctureband_contains != 0 {
            rosecurvula_bonus += 30;
        }

        let rosecurvula_score = candidate_rosecurvula_score + rosecurvula_bonus;
        if rosecurvula_score > best_rosecurvula_score {
            best_rosecurvula_score = rosecurvula_score;
            selected_rosecurvula_index = rosecurvula_index;
        }

        rosecurvula_index += 1;
    }

    let mut selected_rosecurvula_ptr = rosecurvula_value.as_ptr();
    let mut selected_rosecurvula_written = rosecurvula_value_written;
    if selected_rosecurvula_index == 1 {
        selected_rosecurvula_ptr = tridecawake_value.as_ptr();
        selected_rosecurvula_written = tridecawake_value_written;
    } else if selected_rosecurvula_index == 2 {
        selected_rosecurvula_ptr = cinctureband_value.as_ptr();
        selected_rosecurvula_written = cinctureband_value_written;
    }

    let cardioidula_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_rosecurvula_ptr,
            selected_rosecurvula_written as i64,
            cardioidula_old.as_ptr(),
            cardioidula_old.len() as i64,
            cardioidula_new.as_ptr(),
            cardioidula_new.len() as i64,
        )
    };
    let mut cardioidula_value = vec![0u8; cardioidula_value_len as usize];
    let cardioidula_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_rosecurvula_ptr,
            selected_rosecurvula_written as i64,
            cardioidula_old.as_ptr(),
            cardioidula_old.len() as i64,
            cardioidula_new.as_ptr(),
            cardioidula_new.len() as i64,
            cardioidula_value.as_mut_ptr(),
            cardioidula_value.len() as i64,
        )
    };
    let cardioidula_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            cardioidula_value.as_ptr(),
            cardioidula_value_written as i64,
            cardioidula_needle.as_ptr(),
            cardioidula_needle.len() as i64,
        )
    };

    let tetradecawake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_rosecurvula_ptr,
            selected_rosecurvula_written as i64,
            tetradecawake_extension.as_ptr(),
            tetradecawake_extension.len() as i64,
        )
    };
    let mut tetradecawake_value = vec![0u8; tetradecawake_value_len as usize];
    let tetradecawake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_rosecurvula_ptr,
            selected_rosecurvula_written as i64,
            tetradecawake_extension.as_ptr(),
            tetradecawake_extension.len() as i64,
            tetradecawake_value.as_mut_ptr(),
            tetradecawake_value.len() as i64,
        )
    };
    let tetradecawake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            tetradecawake_value.as_ptr(),
            tetradecawake_value_written as i64,
            tetradecawake_needle.as_ptr(),
            tetradecawake_needle.len() as i64,
        )
    };

    let ribbonlace_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_rosecurvula_ptr,
            selected_rosecurvula_written as i64,
        )
    };
    let mut ribbonlace_source = vec![0u8; ribbonlace_source_len as usize];
    let ribbonlace_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_rosecurvula_ptr,
            selected_rosecurvula_written as i64,
            ribbonlace_source.as_mut_ptr(),
            ribbonlace_source.len() as i64,
        )
    };
    let ribbonlace_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            ribbonlace_source.as_ptr(),
            ribbonlace_source_written as i64,
            ribbonlace_old.as_ptr(),
            ribbonlace_old.len() as i64,
            ribbonlace_new.as_ptr(),
            ribbonlace_new.len() as i64,
        )
    };
    let mut ribbonlace_value = vec![0u8; ribbonlace_value_len as usize];
    let ribbonlace_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            ribbonlace_source.as_ptr(),
            ribbonlace_source_written as i64,
            ribbonlace_old.as_ptr(),
            ribbonlace_old.len() as i64,
            ribbonlace_new.as_ptr(),
            ribbonlace_new.len() as i64,
            ribbonlace_value.as_mut_ptr(),
            ribbonlace_value.len() as i64,
        )
    };
    let ribbonlace_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            ribbonlace_value.as_ptr(),
            ribbonlace_value_written as i64,
            ribbonlace_needle.as_ptr(),
            ribbonlace_needle.len() as i64,
        )
    };

    let mut selected_cardioidula_index = 0i32;
    let mut best_cardioidula_score = i32::MIN;
    let mut cardioidula_index = 0i32;
    while cardioidula_index < 3 {
        let mut candidate_cardioidula_score =
            cardioidula_value_written * 10 + cardioidula_contains * 50;
        if cardioidula_index == 1 {
            candidate_cardioidula_score = tetradecawake_value_written * 10 + tetradecawake_index;
        } else if cardioidula_index == 2 {
            candidate_cardioidula_score =
                ribbonlace_value_written * 10 + ribbonlace_contains * 50;
        }

        let mut cardioidula_bonus = 0i32;
        if cardioidula_index == selected_rosecurvula_index {
            cardioidula_bonus += 25;
        }
        if cardioidula_index == selected_lemniscatula_index {
            cardioidula_bonus += 15;
        }
        if cardioidula_index == selected_astroidula_index {
            cardioidula_bonus += 5;
        }
        if cardioidula_index == 0 && cardioidula_contains != 0 {
            cardioidula_bonus += 20;
        }
        if cardioidula_index == 1 && tetradecawake_index >= 0 {
            cardioidula_bonus += 10;
        }
        if cardioidula_index == 2 && ribbonlace_contains != 0 {
            cardioidula_bonus += 30;
        }

        let cardioidula_score = candidate_cardioidula_score + cardioidula_bonus;
        if cardioidula_score > best_cardioidula_score {
            best_cardioidula_score = cardioidula_score;
            selected_cardioidula_index = cardioidula_index;
        }

        cardioidula_index += 1;
    }

    let mut selected_cardioidula_ptr = cardioidula_value.as_ptr();
    let mut selected_cardioidula_written = cardioidula_value_written;
    if selected_cardioidula_index == 1 {
        selected_cardioidula_ptr = tetradecawake_value.as_ptr();
        selected_cardioidula_written = tetradecawake_value_written;
    } else if selected_cardioidula_index == 2 {
        selected_cardioidula_ptr = ribbonlace_value.as_ptr();
        selected_cardioidula_written = ribbonlace_value_written;
    }

    let nephroidette_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_cardioidula_ptr,
            selected_cardioidula_written as i64,
            nephroidette_old.as_ptr(),
            nephroidette_old.len() as i64,
            nephroidette_new.as_ptr(),
            nephroidette_new.len() as i64,
        )
    };
    let mut nephroidette_value = vec![0u8; nephroidette_value_len as usize];
    let nephroidette_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_cardioidula_ptr,
            selected_cardioidula_written as i64,
            nephroidette_old.as_ptr(),
            nephroidette_old.len() as i64,
            nephroidette_new.as_ptr(),
            nephroidette_new.len() as i64,
            nephroidette_value.as_mut_ptr(),
            nephroidette_value.len() as i64,
        )
    };
    let nephroidette_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            nephroidette_value.as_ptr(),
            nephroidette_value_written as i64,
            nephroidette_needle.as_ptr(),
            nephroidette_needle.len() as i64,
        )
    };

    let pentadecawake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_cardioidula_ptr,
            selected_cardioidula_written as i64,
            pentadecawake_extension.as_ptr(),
            pentadecawake_extension.len() as i64,
        )
    };
    let mut pentadecawake_value = vec![0u8; pentadecawake_value_len as usize];
    let pentadecawake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_cardioidula_ptr,
            selected_cardioidula_written as i64,
            pentadecawake_extension.as_ptr(),
            pentadecawake_extension.len() as i64,
            pentadecawake_value.as_mut_ptr(),
            pentadecawake_value.len() as i64,
        )
    };
    let pentadecawake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            pentadecawake_value.as_ptr(),
            pentadecawake_value_written as i64,
            pentadecawake_needle.as_ptr(),
            pentadecawake_needle.len() as i64,
        )
    };

    let gallooncord_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_cardioidula_ptr,
            selected_cardioidula_written as i64,
        )
    };
    let mut gallooncord_source = vec![0u8; gallooncord_source_len as usize];
    let gallooncord_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_cardioidula_ptr,
            selected_cardioidula_written as i64,
            gallooncord_source.as_mut_ptr(),
            gallooncord_source.len() as i64,
        )
    };
    let gallooncord_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            gallooncord_source.as_ptr(),
            gallooncord_source_written as i64,
            gallooncord_old.as_ptr(),
            gallooncord_old.len() as i64,
            gallooncord_new.as_ptr(),
            gallooncord_new.len() as i64,
        )
    };
    let mut gallooncord_value = vec![0u8; gallooncord_value_len as usize];
    let gallooncord_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            gallooncord_source.as_ptr(),
            gallooncord_source_written as i64,
            gallooncord_old.as_ptr(),
            gallooncord_old.len() as i64,
            gallooncord_new.as_ptr(),
            gallooncord_new.len() as i64,
            gallooncord_value.as_mut_ptr(),
            gallooncord_value.len() as i64,
        )
    };
    let gallooncord_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            gallooncord_value.as_ptr(),
            gallooncord_value_written as i64,
            gallooncord_needle.as_ptr(),
            gallooncord_needle.len() as i64,
        )
    };

    let mut selected_nephroidette_index = 0i32;
    let mut best_nephroidette_score = i32::MIN;
    let mut nephroidette_index = 0i32;
    while nephroidette_index < 3 {
        let mut candidate_nephroidette_score =
            nephroidette_value_written * 10 + nephroidette_contains * 50;
        if nephroidette_index == 1 {
            candidate_nephroidette_score = pentadecawake_value_written * 10 + pentadecawake_index;
        } else if nephroidette_index == 2 {
            candidate_nephroidette_score =
                gallooncord_value_written * 10 + gallooncord_contains * 50;
        }

        let mut nephroidette_bonus = 0i32;
        if nephroidette_index == selected_cardioidula_index {
            nephroidette_bonus += 25;
        }
        if nephroidette_index == selected_rosecurvula_index {
            nephroidette_bonus += 15;
        }
        if nephroidette_index == selected_lemniscatula_index {
            nephroidette_bonus += 5;
        }
        if nephroidette_index == 0 && nephroidette_contains != 0 {
            nephroidette_bonus += 20;
        }
        if nephroidette_index == 1 && pentadecawake_index >= 0 {
            nephroidette_bonus += 10;
        }
        if nephroidette_index == 2 && gallooncord_contains != 0 {
            nephroidette_bonus += 30;
        }

        let nephroidette_score = candidate_nephroidette_score + nephroidette_bonus;
        if nephroidette_score > best_nephroidette_score {
            best_nephroidette_score = nephroidette_score;
            selected_nephroidette_index = nephroidette_index;
        }

        nephroidette_index += 1;
    }

    let mut selected_nephroidette_ptr = nephroidette_value.as_ptr();
    let mut selected_nephroidette_written = nephroidette_value_written;
    if selected_nephroidette_index == 1 {
        selected_nephroidette_ptr = pentadecawake_value.as_ptr();
        selected_nephroidette_written = pentadecawake_value_written;
    } else if selected_nephroidette_index == 2 {
        selected_nephroidette_ptr = gallooncord_value.as_ptr();
        selected_nephroidette_written = gallooncord_value_written;
    }

    let epicycloidette_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_nephroidette_ptr,
            selected_nephroidette_written as i64,
            epicycloidette_old.as_ptr(),
            epicycloidette_old.len() as i64,
            epicycloidette_new.as_ptr(),
            epicycloidette_new.len() as i64,
        )
    };
    let mut epicycloidette_value = vec![0u8; epicycloidette_value_len as usize];
    let epicycloidette_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_nephroidette_ptr,
            selected_nephroidette_written as i64,
            epicycloidette_old.as_ptr(),
            epicycloidette_old.len() as i64,
            epicycloidette_new.as_ptr(),
            epicycloidette_new.len() as i64,
            epicycloidette_value.as_mut_ptr(),
            epicycloidette_value.len() as i64,
        )
    };
    let epicycloidette_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            epicycloidette_value.as_ptr(),
            epicycloidette_value_written as i64,
            epicycloidette_needle.as_ptr(),
            epicycloidette_needle.len() as i64,
        )
    };

    let hexadecawake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_nephroidette_ptr,
            selected_nephroidette_written as i64,
            hexadecawake_extension.as_ptr(),
            hexadecawake_extension.len() as i64,
        )
    };
    let mut hexadecawake_value = vec![0u8; hexadecawake_value_len as usize];
    let hexadecawake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_nephroidette_ptr,
            selected_nephroidette_written as i64,
            hexadecawake_extension.as_ptr(),
            hexadecawake_extension.len() as i64,
            hexadecawake_value.as_mut_ptr(),
            hexadecawake_value.len() as i64,
        )
    };
    let hexadecawake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            hexadecawake_value.as_ptr(),
            hexadecawake_value_written as i64,
            hexadecawake_needle.as_ptr(),
            hexadecawake_needle.len() as i64,
        )
    };

    let brocatelle_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_nephroidette_ptr,
            selected_nephroidette_written as i64,
        )
    };
    let mut brocatelle_source = vec![0u8; brocatelle_source_len as usize];
    let brocatelle_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_nephroidette_ptr,
            selected_nephroidette_written as i64,
            brocatelle_source.as_mut_ptr(),
            brocatelle_source.len() as i64,
        )
    };
    let brocatelle_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            brocatelle_source.as_ptr(),
            brocatelle_source_written as i64,
            brocatelle_old.as_ptr(),
            brocatelle_old.len() as i64,
            brocatelle_new.as_ptr(),
            brocatelle_new.len() as i64,
        )
    };
    let mut brocatelle_value = vec![0u8; brocatelle_value_len as usize];
    let brocatelle_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            brocatelle_source.as_ptr(),
            brocatelle_source_written as i64,
            brocatelle_old.as_ptr(),
            brocatelle_old.len() as i64,
            brocatelle_new.as_ptr(),
            brocatelle_new.len() as i64,
            brocatelle_value.as_mut_ptr(),
            brocatelle_value.len() as i64,
        )
    };
    let brocatelle_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            brocatelle_value.as_ptr(),
            brocatelle_value_written as i64,
            brocatelle_needle.as_ptr(),
            brocatelle_needle.len() as i64,
        )
    };

    let mut selected_epicycloidette_index = 0i32;
    let mut best_epicycloidette_score = i32::MIN;
    let mut epicycloidette_index = 0i32;
    while epicycloidette_index < 3 {
        let mut candidate_epicycloidette_score =
            epicycloidette_value_written * 10 + epicycloidette_contains * 50;
        if epicycloidette_index == 1 {
            candidate_epicycloidette_score = hexadecawake_value_written * 10 + hexadecawake_index;
        } else if epicycloidette_index == 2 {
            candidate_epicycloidette_score =
                brocatelle_value_written * 10 + brocatelle_contains * 50;
        }

        let mut epicycloidette_bonus = 0i32;
        if epicycloidette_index == selected_nephroidette_index {
            epicycloidette_bonus += 25;
        }
        if epicycloidette_index == selected_cardioidula_index {
            epicycloidette_bonus += 15;
        }
        if epicycloidette_index == selected_rosecurvula_index {
            epicycloidette_bonus += 5;
        }
        if epicycloidette_index == 0 && epicycloidette_contains != 0 {
            epicycloidette_bonus += 20;
        }
        if epicycloidette_index == 1 && hexadecawake_index >= 0 {
            epicycloidette_bonus += 10;
        }
        if epicycloidette_index == 2 && brocatelle_contains != 0 {
            epicycloidette_bonus += 30;
        }

        let epicycloidette_score = candidate_epicycloidette_score + epicycloidette_bonus;
        if epicycloidette_score > best_epicycloidette_score {
            best_epicycloidette_score = epicycloidette_score;
            selected_epicycloidette_index = epicycloidette_index;
        }

        epicycloidette_index += 1;
    }

    let mut selected_epicycloidette_ptr = epicycloidette_value.as_ptr();
    let mut selected_epicycloidette_written = epicycloidette_value_written;
    if selected_epicycloidette_index == 1 {
        selected_epicycloidette_ptr = hexadecawake_value.as_ptr();
        selected_epicycloidette_written = hexadecawake_value_written;
    } else if selected_epicycloidette_index == 2 {
        selected_epicycloidette_ptr = brocatelle_value.as_ptr();
        selected_epicycloidette_written = brocatelle_value_written;
    }

    let campyloidette_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_epicycloidette_ptr,
            selected_epicycloidette_written as i64,
            campyloidette_old.as_ptr(),
            campyloidette_old.len() as i64,
            campyloidette_new.as_ptr(),
            campyloidette_new.len() as i64,
        )
    };
    let mut campyloidette_value = vec![0u8; campyloidette_value_len as usize];
    let campyloidette_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_epicycloidette_ptr,
            selected_epicycloidette_written as i64,
            campyloidette_old.as_ptr(),
            campyloidette_old.len() as i64,
            campyloidette_new.as_ptr(),
            campyloidette_new.len() as i64,
            campyloidette_value.as_mut_ptr(),
            campyloidette_value.len() as i64,
        )
    };
    let campyloidette_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            campyloidette_value.as_ptr(),
            campyloidette_value_written as i64,
            campyloidette_needle.as_ptr(),
            campyloidette_needle.len() as i64,
        )
    };

    let heptadecawake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_epicycloidette_ptr,
            selected_epicycloidette_written as i64,
            heptadecawake_extension.as_ptr(),
            heptadecawake_extension.len() as i64,
        )
    };
    let mut heptadecawake_value = vec![0u8; heptadecawake_value_len as usize];
    let heptadecawake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_epicycloidette_ptr,
            selected_epicycloidette_written as i64,
            heptadecawake_extension.as_ptr(),
            heptadecawake_extension.len() as i64,
            heptadecawake_value.as_mut_ptr(),
            heptadecawake_value.len() as i64,
        )
    };
    let heptadecawake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            heptadecawake_value.as_ptr(),
            heptadecawake_value_written as i64,
            heptadecawake_needle.as_ptr(),
            heptadecawake_needle.len() as i64,
        )
    };

    let soutachecord_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_epicycloidette_ptr,
            selected_epicycloidette_written as i64,
        )
    };
    let mut soutachecord_source = vec![0u8; soutachecord_source_len as usize];
    let soutachecord_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_epicycloidette_ptr,
            selected_epicycloidette_written as i64,
            soutachecord_source.as_mut_ptr(),
            soutachecord_source.len() as i64,
        )
    };
    let soutachecord_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            soutachecord_source.as_ptr(),
            soutachecord_source_written as i64,
            soutachecord_old.as_ptr(),
            soutachecord_old.len() as i64,
            soutachecord_new.as_ptr(),
            soutachecord_new.len() as i64,
        )
    };
    let mut soutachecord_value = vec![0u8; soutachecord_value_len as usize];
    let soutachecord_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            soutachecord_source.as_ptr(),
            soutachecord_source_written as i64,
            soutachecord_old.as_ptr(),
            soutachecord_old.len() as i64,
            soutachecord_new.as_ptr(),
            soutachecord_new.len() as i64,
            soutachecord_value.as_mut_ptr(),
            soutachecord_value.len() as i64,
        )
    };
    let soutachecord_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            soutachecord_value.as_ptr(),
            soutachecord_value_written as i64,
            soutachecord_needle.as_ptr(),
            soutachecord_needle.len() as i64,
        )
    };

    let mut selected_campyloidette_index = 0i32;
    let mut best_campyloidette_score = i32::MIN;
    let mut campyloidette_index = 0i32;
    while campyloidette_index < 3 {
        let mut candidate_campyloidette_score =
            campyloidette_value_written * 10 + campyloidette_contains * 50;
        if campyloidette_index == 1 {
            candidate_campyloidette_score = heptadecawake_value_written * 10 + heptadecawake_index;
        } else if campyloidette_index == 2 {
            candidate_campyloidette_score =
                soutachecord_value_written * 10 + soutachecord_contains * 50;
        }

        let mut campyloidette_bonus = 0i32;
        if campyloidette_index == selected_epicycloidette_index {
            campyloidette_bonus += 25;
        }
        if campyloidette_index == selected_nephroidette_index {
            campyloidette_bonus += 15;
        }
        if campyloidette_index == selected_cardioidula_index {
            campyloidette_bonus += 5;
        }
        if campyloidette_index == 0 && campyloidette_contains != 0 {
            campyloidette_bonus += 20;
        }
        if campyloidette_index == 1 && heptadecawake_index >= 0 {
            campyloidette_bonus += 10;
        }
        if campyloidette_index == 2 && soutachecord_contains != 0 {
            campyloidette_bonus += 30;
        }

        let campyloidette_score = candidate_campyloidette_score + campyloidette_bonus;
        if campyloidette_score > best_campyloidette_score {
            best_campyloidette_score = campyloidette_score;
            selected_campyloidette_index = campyloidette_index;
        }

        campyloidette_index += 1;
    }

    let mut selected_campyloidette_ptr = campyloidette_value.as_ptr();
    let mut selected_campyloidette_written = campyloidette_value_written;
    if selected_campyloidette_index == 1 {
        selected_campyloidette_ptr = heptadecawake_value.as_ptr();
        selected_campyloidette_written = heptadecawake_value_written;
    } else if selected_campyloidette_index == 2 {
        selected_campyloidette_ptr = soutachecord_value.as_ptr();
        selected_campyloidette_written = soutachecord_value_written;
    }

    let spheroidette_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_campyloidette_ptr,
            selected_campyloidette_written as i64,
            spheroidette_old.as_ptr(),
            spheroidette_old.len() as i64,
            spheroidette_new.as_ptr(),
            spheroidette_new.len() as i64,
        )
    };
    let mut spheroidette_value = vec![0u8; spheroidette_value_len as usize];
    let spheroidette_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_campyloidette_ptr,
            selected_campyloidette_written as i64,
            spheroidette_old.as_ptr(),
            spheroidette_old.len() as i64,
            spheroidette_new.as_ptr(),
            spheroidette_new.len() as i64,
            spheroidette_value.as_mut_ptr(),
            spheroidette_value.len() as i64,
        )
    };
    let spheroidette_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            spheroidette_value.as_ptr(),
            spheroidette_value_written as i64,
            spheroidette_needle.as_ptr(),
            spheroidette_needle.len() as i64,
        )
    };

    let octadecawake_value_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_campyloidette_ptr,
            selected_campyloidette_written as i64,
            octadecawake_extension.as_ptr(),
            octadecawake_extension.len() as i64,
        )
    };
    let mut octadecawake_value = vec![0u8; octadecawake_value_len as usize];
    let octadecawake_value_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_campyloidette_ptr,
            selected_campyloidette_written as i64,
            octadecawake_extension.as_ptr(),
            octadecawake_extension.len() as i64,
            octadecawake_value.as_mut_ptr(),
            octadecawake_value.len() as i64,
        )
    };
    let octadecawake_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            octadecawake_value.as_ptr(),
            octadecawake_value_written as i64,
            octadecawake_needle.as_ptr(),
            octadecawake_needle.len() as i64,
        )
    };

    let tambourcord_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_campyloidette_ptr,
            selected_campyloidette_written as i64,
        )
    };
    let mut tambourcord_source = vec![0u8; tambourcord_source_len as usize];
    let tambourcord_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_campyloidette_ptr,
            selected_campyloidette_written as i64,
            tambourcord_source.as_mut_ptr(),
            tambourcord_source.len() as i64,
        )
    };
    let tambourcord_value_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            tambourcord_source.as_ptr(),
            tambourcord_source_written as i64,
            tambourcord_old.as_ptr(),
            tambourcord_old.len() as i64,
            tambourcord_new.as_ptr(),
            tambourcord_new.len() as i64,
        )
    };
    let mut tambourcord_value = vec![0u8; tambourcord_value_len as usize];
    let tambourcord_value_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            tambourcord_source.as_ptr(),
            tambourcord_source_written as i64,
            tambourcord_old.as_ptr(),
            tambourcord_old.len() as i64,
            tambourcord_new.as_ptr(),
            tambourcord_new.len() as i64,
            tambourcord_value.as_mut_ptr(),
            tambourcord_value.len() as i64,
        )
    };
    let tambourcord_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            tambourcord_value.as_ptr(),
            tambourcord_value_written as i64,
            tambourcord_needle.as_ptr(),
            tambourcord_needle.len() as i64,
        )
    };

    let mut selected_spheroidette_index = 0i32;
    let mut best_spheroidette_score = i32::MIN;
    let mut spheroidette_index = 0i32;
    while spheroidette_index < 3 {
        let mut candidate_spheroidette_score =
            spheroidette_value_written * 10 + spheroidette_contains * 50;
        if spheroidette_index == 1 {
            candidate_spheroidette_score = octadecawake_value_written * 10 + octadecawake_index;
        } else if spheroidette_index == 2 {
            candidate_spheroidette_score =
                tambourcord_value_written * 10 + tambourcord_contains * 50;
        }

        let mut spheroidette_bonus = 0i32;
        if spheroidette_index == selected_campyloidette_index {
            spheroidette_bonus += 25;
        }
        if spheroidette_index == selected_epicycloidette_index {
            spheroidette_bonus += 15;
        }
        if spheroidette_index == selected_nephroidette_index {
            spheroidette_bonus += 5;
        }
        if spheroidette_index == 0 && spheroidette_contains != 0 {
            spheroidette_bonus += 20;
        }
        if spheroidette_index == 1 && octadecawake_index >= 0 {
            spheroidette_bonus += 10;
        }
        if spheroidette_index == 2 && tambourcord_contains != 0 {
            spheroidette_bonus += 30;
        }

        let spheroidette_score = candidate_spheroidette_score + spheroidette_bonus;
        if spheroidette_score > best_spheroidette_score {
            best_spheroidette_score = spheroidette_score;
            selected_spheroidette_index = spheroidette_index;
        }

        spheroidette_index += 1;
    }

    (selected_directory_index + 1) * 100000000
        + (selected_file_index + 1) * 10000000
        + (selected_variant_index + 1) * 1000000
        + (selected_rebase_index + 1) * 100000
        + (selected_leaf_transform_index + 1) * 10000
        + (selected_path_transform_index + 1) * 1000
        + (selected_recomposition_index + 1) * 100
        + (selected_campyloidette_index + 1) * 10
        + (selected_spheroidette_index + 1)
}