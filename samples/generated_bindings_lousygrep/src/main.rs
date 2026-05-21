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

fn release_lines_and_return(lines: system::ManagedStringArray, code: i32) -> i32 {
    let _ = lines.release();
    code
}

fn release_all_and_return(
    args: system::ManagedStringArray,
    search: system::ManagedString,
    path: system::ManagedString,
    code: i32,
) -> i32 {
    let _ = path.release();
    let _ = search.release();
    let _ = args.release();
    code
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

    if count < 3 {
        return release_args_and_return(args, 3);
    }

    let search = match args.get(1) {
        Ok(value) => value,
        Err(_) => return release_args_and_return(args, 4),
    };

    let mut arg_index = 2;
    while arg_index < count {
        let path = match args.get(arg_index) {
            Ok(value) => value,
            Err(_) => return release_args_search_and_return(args, search, 5),
        };

        let lines = match system::io::file::read_all_lines(&path) {
            Ok(value) => value,
            Err(_) => return release_all_and_return(args, search, path, 6),
        };

        let line_count = match lines.len() {
            Ok(value) => value,
            Err(_) => {
                let _ = path.release();
                return release_args_search_and_return(args, search, release_lines_and_return(lines, 7));
            }
        };

        let mut line_index = 0;
        while line_index < line_count {
            let line = match lines.get(line_index) {
                Ok(value) => value,
                Err(_) => {
                    let _ = path.release();
                    return release_args_search_and_return(args, search, release_lines_and_return(lines, 8));
                }
            };

            let contains = match line.contains(&search) {
                Ok(value) => value,
                Err(_) => {
                    let _ = line.release();
                    let _ = path.release();
                    return release_args_search_and_return(args, search, release_lines_and_return(lines, 9));
                }
            };

            if contains && system::console::write_line(&line).is_err() {
                let _ = line.release();
                let _ = path.release();
                return release_args_search_and_return(args, search, release_lines_and_return(lines, 10));
            }

            if line.release().is_err() {
                let _ = path.release();
                return release_args_search_and_return(args, search, release_lines_and_return(lines, 11));
            }

            line_index += 1;
        }

        if lines.release().is_err() {
            let _ = path.release();
            return release_args_search_and_return(args, search, 12);
        }

        if path.release().is_err() {
            return release_args_search_and_return(args, search, 13);
        }

        arg_index += 1;
    }

    if search.release().is_err() {
        return release_args_and_return(args, 14);
    }

    if args.release().is_err() {
        return 15;
    }

    0
}