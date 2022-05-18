[![default](https://github.com/Neo-vortex/ADB.NET/actions/workflows/build.yml/badge.svg)](https://github.com/Neo-vortex/ADB.NET/actions/workflows/build.yml)


#### About the Project

I started this project to provide 'cross-platform native adb(Android Debugging Bridge) functionality in pure C# 'code without any external dependency to native and pre-compiled binaries (propably exept USB drivers and interfaces).
This is going to be a long way as having a cross-platform USB cominucation with a language like C# is not that easy (libusb, winusb and ...).Also it is important to note that the adb protocol is niether well-documented nor fully standard across every implementation, So there will be much to learn and even more to implement here!
Right now the project is in a non-functional status and functinality is being added one by one until I can call it a working beta version.
#### Framework

This project is entirely written in C# with dotnet 6 framework. This may change in the future but the final library should be able to run under CLR or as a native lib.

#### Build

A simple ```dotnet build ADB.NET.sln``` should get you started for now

#### Functionalities and goals
This is a long list as every single details needs dedication and R&D
- [x] Parse ADB packet struction 
- [x] Produce basic adb header and packet
- [x] Complete basic adb handshake
- [x] Connect to the device with TCP/IP
- [ ] Connect to the device wih USB
- [ ] Cominucate with the custom encryption of AOSP (also known as ```libmincrypt```)
- [ ] Implement ```adb push```
- [ ] Implement  ```adb pull```
- [ ] Implement ```adb shell```
- [ ] Implement ```adb logcat```
- [ ] Implement ```adb reboot [recovery,bootloader,...]```

#### PR
PRs are the most welcomes as this is a challenging project!
