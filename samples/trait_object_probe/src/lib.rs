trait Score {
    fn score(&self, value: i32) -> i32;
}

struct AddScore {
    bias: i32,
}

struct MulScore {
    factor: i32,
}

impl Score for AddScore {
    #[inline(never)]
    fn score(&self, value: i32) -> i32 {
        value + self.bias
    }
}

impl Score for MulScore {
    #[inline(never)]
    fn score(&self, value: i32) -> i32 {
        value * self.factor
    }
}

#[inline(never)]
fn select_score<'a>(selector: i32, add: &'a AddScore, mul: &'a MulScore) -> &'a dyn Score {
    if selector == 0 {
        add
    } else {
        mul
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn trait_object_score(selector: i32) -> i32 {
    let add = AddScore { bias: 7 };
    let mul = MulScore { factor: 3 };
    let selected = select_score(selector, &add, &mul);
    selected.score(5)
}
