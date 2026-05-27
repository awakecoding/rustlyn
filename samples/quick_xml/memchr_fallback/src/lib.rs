pub fn memchr(needle: u8, haystack: &[u8]) -> Option<usize> {
    haystack.iter().position(|&byte| byte == needle)
}

pub fn memchr2(needle1: u8, needle2: u8, haystack: &[u8]) -> Option<usize> {
    haystack
        .iter()
        .position(|&byte| byte == needle1 || byte == needle2)
}

pub fn memchr3(needle1: u8, needle2: u8, needle3: u8, haystack: &[u8]) -> Option<usize> {
    haystack
        .iter()
        .position(|&byte| byte == needle1 || byte == needle2 || byte == needle3)
}

pub fn memchr_iter(needle: u8, haystack: &[u8]) -> MemchrIter<'_> {
    MemchrIter {
        haystack,
        offset: 0,
        needles: [needle, 0, 0],
        needle_count: 1,
    }
}

pub fn memchr2_iter(needle1: u8, needle2: u8, haystack: &[u8]) -> MemchrIter<'_> {
    MemchrIter {
        haystack,
        offset: 0,
        needles: [needle1, needle2, 0],
        needle_count: 2,
    }
}

pub fn memchr3_iter(needle1: u8, needle2: u8, needle3: u8, haystack: &[u8]) -> MemchrIter<'_> {
    MemchrIter {
        haystack,
        offset: 0,
        needles: [needle1, needle2, needle3],
        needle_count: 3,
    }
}

pub struct MemchrIter<'a> {
    haystack: &'a [u8],
    offset: usize,
    needles: [u8; 3],
    needle_count: usize,
}

impl Iterator for MemchrIter<'_> {
    type Item = usize;

    fn next(&mut self) -> Option<Self::Item> {
        while self.offset < self.haystack.len() {
            let index = self.offset;
            self.offset += 1;
            let byte = self.haystack[index];
            let mut matched = false;
            for needle in &self.needles[..self.needle_count] {
                matched |= byte == *needle;
            }
            if matched {
                return Some(index);
            }
        }

        None
    }
}
