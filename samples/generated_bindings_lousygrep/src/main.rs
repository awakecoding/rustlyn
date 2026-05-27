#![no_main]

#[path = "../../generated_bindings_hello/src/system.rs"]
mod system;

fn release_args_and_return(args: system::ManagedStringArray, code: i32) -> i32 {
    let _ = args.release();
    code
}

fn release_args_search_and_return(
    args: system::ManagedStringArray,
    search: system::ManagedString,
    code: i32,
) -> i32 {
    let _ = search.release();
    let _ = args.release();
    code
}

fn release_args_search_directory_and_return(
    args: system::ManagedStringArray,
    search: system::ManagedString,
    directory: system::ManagedString,
    code: i32,
) -> i32 {
    let _ = directory.release();
    let _ = search.release();
    let _ = args.release();
    code
}

fn release_lines_and_return(lines: system::ManagedStringArray, code: i32) -> i32 {
    let _ = lines.release();
    code
}

fn release_path_and_return(path: system::ManagedString, code: i32) -> i32 {
    let _ = path.release();
    code
}

fn release_paths_and_return(paths: system::ManagedStringArray, code: i32) -> i32 {
    let _ = paths.release();
    code
}

fn process_path(search: &system::ManagedString, path: system::ManagedString) -> i32 {
    let lines = match system::io::file::read_all_lines(&path) {
        Ok(value) => value,
        Err(_) => return release_path_and_return(path, 6),
    };

    let line_count = match lines.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = path.release();
            return release_lines_and_return(lines, 7);
        }
    };

    let mut line_index = 0;
    while line_index < line_count {
        let line = match lines.get(line_index) {
            Ok(value) => value,
            Err(_) => {
                let _ = path.release();
                return release_lines_and_return(lines, 8);
            }
        };

        let contains = match line.contains(search) {
            Ok(value) => value,
            Err(_) => {
                let _ = line.release();
                let _ = path.release();
                return release_lines_and_return(lines, 9);
            }
        };

        if contains && system::console::write_line(&line).is_err() {
            let _ = line.release();
            let _ = path.release();
            return release_lines_and_return(lines, 10);
        }

        if line.release().is_err() {
            let _ = path.release();
            return release_lines_and_return(lines, 11);
        }

        line_index += 1;
    }

    if lines.release().is_err() {
        let _ = path.release();
        return 12;
    }

    if path.release().is_err() {
        return 13;
    }

    0
}

fn process_paths(search: &system::ManagedString, paths: system::ManagedStringArray) -> i32 {
    let path_count = match paths.len() {
        Ok(value) => value,
        Err(_) => return release_paths_and_return(paths, 16),
    };

    let mut path_index = 0;
    while path_index < path_count {
        let path = match paths.get(path_index) {
            Ok(value) => value,
            Err(_) => return release_paths_and_return(paths, 17),
        };

        let code = process_path(search, path);
        if code != 0 {
            let _ = paths.release();
            return code;
        }

        path_index += 1;
    }

    if paths.release().is_err() {
        return 18;
    }

    0
}

#[unsafe(no_mangle)]
pub extern "C" fn main() -> i32 {
    let args = match system::environment::get_command_line_args() {
        Ok(value) => value,
        Err(_) => return 1,
    };

    let count = match args.len() {
        Ok(value) => value,
        Err(_) => return release_args_and_return(args, 2),
    };

    if count < 4 {
        return release_args_and_return(args, 3);
    }

    let search = match args.get(1) {
        Ok(value) => value,
        Err(_) => return release_args_and_return(args, 4),
    };

    let directory = match args.get(2) {
        Ok(value) => value,
        Err(_) => return release_args_search_and_return(args, search, 5),
    };

    let mut arg_index = 3;
    while arg_index < count {
        let pattern = match args.get(arg_index) {
            Ok(value) => value,
            Err(_) => return release_args_search_directory_and_return(args, search, directory, 19),
        };

        let paths = match system::io::directory::get_files(&directory, &pattern) {
            Ok(value) => value,
            Err(_) => {
                let _ = pattern.release();
                return release_args_search_directory_and_return(args, search, directory, 20);
            }
        };

        let code = process_paths(&search, paths);
        let pattern_release_failed = pattern.release().is_err();
        if code != 0 {
            return release_args_search_directory_and_return(args, search, directory, code);
        }

        if pattern_release_failed {
            return release_args_search_directory_and_return(args, search, directory, 21);
        }

        arg_index += 1;
    }

    if directory.release().is_err() {
        return release_args_search_and_return(args, search, 22);
    }

    if search.release().is_err() {
        return release_args_and_return(args, 14);
    }

    if args.release().is_err() {
        return 15;
    }

    0
}
