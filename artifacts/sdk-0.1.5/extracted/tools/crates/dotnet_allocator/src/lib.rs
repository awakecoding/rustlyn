
// Copyright 2020 SourceGear

#![no_std]

use core::alloc::{GlobalAlloc, Layout};
use dotnet::System::Runtime::InteropServices::*;

// TODO rename this?
pub struct MyAllocator;

unsafe impl GlobalAlloc for MyAllocator {

    unsafe fn alloc(&self, layout: Layout) -> *mut u8 { 
        // TODO what to do with align and such?

        //return Marshal::AllocHGlobal(layout.size() as i32) as *mut u8;
        // TODO probably use the overload trait here instead
        let p = Marshal::IntPtr__AllocHGlobal__1__i32(layout.size() as i32);
        // TODO arguably this should call p.ToPointer()
        return p.get_value() as *mut u8;
    }

    unsafe fn dealloc(&self, ptr: *mut u8, _layout: Layout) {
        // TODO should the Layout arg be needed here?

        //Marshal::FreeHGlobal(ptr as i64);
        // TODO arguably this should call the IntrPtr(long) ctor
        let p = dotnet::System::IntPtr::from_value(ptr as i64);
        // TODO probably use the overload trait here instead
        Marshal::void__FreeHGlobal__1__IntPtr(p);
    }

}

