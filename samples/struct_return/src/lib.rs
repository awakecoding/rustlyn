struct Point {
    x: i32,
    y: i32,
}

struct Rect {
    left: i32,
    top: i32,
    right: i32,
    bottom: i32,
}

#[inline(never)]
fn make_point(x: i32, y: i32) -> Point {
    Point { x, y }
}

#[inline(never)]
fn make_rect(left: i32, top: i32, right: i32, bottom: i32) -> Rect {
    Rect { left, top, right, bottom }
}

#[inline(never)]
fn rect_area(r: &Rect) -> i32 {
    (r.right - r.left) * (r.bottom - r.top)
}

/// Probe: point(3,4).x + point(3,4).y + rect_area(Rect{0,0,5,7}) = 3 + 4 + 35 = 42
#[unsafe(no_mangle)]
pub extern "C" fn struct_return_probe() -> i32 {
    let p = make_point(3, 4);
    let r = make_rect(0, 0, 5, 7);
    p.x + p.y + rect_area(&r)
}
