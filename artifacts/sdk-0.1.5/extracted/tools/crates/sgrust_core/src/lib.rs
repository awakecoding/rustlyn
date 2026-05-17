
// Copyright 2020 SourceGear

#![no_std]

pub trait SGRustValue {
    fn get_type_handle() -> i64;
    fn from_ffi(hndl : i64) -> Self;
    fn to_ffi(&self) -> i64;
}

pub trait SGRustObjectHandle {
    fn from_handle(hndl : i64) -> Self;
    fn get_handle(&self) -> i64;
}

pub struct Dropper<T : SGRustObjectHandle> {
    inner : T
}

impl<T : SGRustObjectHandle> Drop for Dropper<T> {
    fn drop(&mut self) {
        extern {
            fn __drop_handle(self_h : i64);
        }
        unsafe {
            __drop_handle(self.inner.get_handle());
        }
    }
}


impl SGRustValue for i8 {
    fn get_type_handle() -> i64 {
        extern {
            fn System_SByte_get_type_handle() -> i64;
        }
        unsafe {
            return System_SByte_get_type_handle();
        }
    }
    fn to_ffi(&self) -> i64 { return *self as i64; }
    fn from_ffi(hndl : i64) -> Self { return hndl as i8; }
}

impl SGRustValue for u8 {
    fn get_type_handle() -> i64 {
        extern {
            fn System_Byte_get_type_handle() -> i64;
        }
        unsafe {
            return System_Byte_get_type_handle();
        }
    }
    fn to_ffi(&self) -> i64 { return *self as i64; }
    fn from_ffi(hndl : i64) -> Self { return hndl as u8; }
}

impl SGRustValue for i16 {
    fn get_type_handle() -> i64 {
        extern {
            fn System_Int16_get_type_handle() -> i64;
        }
        unsafe {
            return System_Int16_get_type_handle();
        }
    }
    fn to_ffi(&self) -> i64 { return *self as i64; }
    fn from_ffi(hndl : i64) -> Self { return hndl as i16; }
}

impl SGRustValue for u16 {
    fn get_type_handle() -> i64 {
        extern {
            fn System_UInt16_get_type_handle() -> i64;
        }
        unsafe {
            return System_UInt16_get_type_handle();
        }
    }
    fn to_ffi(&self) -> i64 { return *self as i64; }
    fn from_ffi(hndl : i64) -> Self { return hndl as u16; }
}

impl SGRustValue for i32 {
    fn get_type_handle() -> i64 {
        extern {
            fn System_Int32_get_type_handle() -> i64;
        }
        unsafe {
            return System_Int32_get_type_handle();
        }
    }
    fn to_ffi(&self) -> i64 { return *self as i64; }
    fn from_ffi(hndl : i64) -> Self { return hndl as i32; }
}

impl SGRustValue for u32 {
    fn get_type_handle() -> i64 {
        extern {
            fn System_UInt32_get_type_handle() -> i64;
        }
        unsafe {
            return System_UInt32_get_type_handle();
        }
    }
    fn to_ffi(&self) -> i64 { return *self as i64; }
    fn from_ffi(hndl : i64) -> Self { return hndl as u32; }
}

impl SGRustValue for i64 {
    fn get_type_handle() -> i64 {
        extern {
            fn System_Int64_get_type_handle() -> i64;
        }
        unsafe {
            return System_Int64_get_type_handle();
        }
    }
    fn to_ffi(&self) -> i64 { return *self; }
    fn from_ffi(hndl : i64) -> Self { return hndl; }
}

impl SGRustValue for u64 {
    fn get_type_handle() -> i64 {
        extern {
            fn System_UInt64_get_type_handle() -> i64;
        }
        unsafe {
            return System_UInt64_get_type_handle();
        }
    }
    fn to_ffi(&self) -> i64 { return *self as i64; } // TODO wrong, transmute
    fn from_ffi(hndl : i64) -> Self { return hndl as u64; } // TODO wrong, transmute
}

impl SGRustValue for f32 {
    fn get_type_handle() -> i64 {
        extern {
            fn System_Single_get_type_handle() -> i64;
        }
        unsafe {
            return System_Single_get_type_handle();
        }
    }
    fn to_ffi(&self) -> i64 { return *self as i64; } // TODO wrong, transmute
    fn from_ffi(hndl : i64) -> Self { return hndl as f32; } // TODO wrong, transmute
}

impl SGRustValue for f64 {
    fn get_type_handle() -> i64 {
        extern {
            fn System_Double_get_type_handle() -> i64;
        }
        unsafe {
            return System_Double_get_type_handle();
        }
    }
    fn to_ffi(&self) -> i64 { return *self as i64; } // TODO wrong, transmute
    fn from_ffi(hndl : i64) -> Self { return hndl as f64; } // TODO wrong, transmute
}

impl SGRustValue for bool {
    fn get_type_handle() -> i64 {
        extern {
            fn System_Boolean_get_type_handle() -> i64;
        }
        unsafe {
            return System_Boolean_get_type_handle();
        }
    }
    fn to_ffi(&self) -> i64 { return if *self {1} else {0}; }
    fn from_ffi(hndl : i64) -> Self { return hndl != 0; }
}


