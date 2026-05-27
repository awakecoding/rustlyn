trait Score {
    fn score(&self, value: i32) -> i32;
    fn pair_score(&self, value: i32) -> ScorePair;
}

#[repr(C)]
struct ScorePair {
    left: i32,
    right: i32,
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

    #[inline(never)]
    fn pair_score(&self, value: i32) -> ScorePair {
        ScorePair {
            left: value,
            right: self.bias,
        }
    }
}

impl Score for MulScore {
    #[inline(never)]
    fn score(&self, value: i32) -> i32 {
        value * self.factor
    }

    #[inline(never)]
    fn pair_score(&self, value: i32) -> ScorePair {
        ScorePair {
            left: value * self.factor,
            right: self.factor,
        }
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

#[unsafe(no_mangle)]
pub extern "C" fn trait_object_pair_score(selector: i32) -> i32 {
    let add = AddScore { bias: 7 };
    let mul = MulScore { factor: 3 };
    let selected = select_score(selector, &add, &mul);
    let pair = selected.pair_score(5);
    pair.left * 100 + pair.right
}
