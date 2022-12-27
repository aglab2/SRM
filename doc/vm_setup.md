# Virtual Machine

This tutorial describes the setup for the virtual machine stored on coreweave.com
* Register on coreweave.com and create a VM on LGA1, RTX 4000, 8 Cores, Windows 10
* * Make sure to use static IP to share with restreamers
* When machine starts, install the following software
* * Virtual Audio Cable
* * 7zip
* * Parsec
* * cmder
* * MPV
* * streamlink
* * MEGA Sync
* * OBS
* * Notepad++
* * LiveSplit
* Use selective sync in MEGASync to synchronize only needed single folder
* Deploy OBS and load the JSON file from 'obs' folder
* Put files in 'config' folder to %appdata%
* Fix paths using script in fix_path.lua
* Setup all info_* scripts in OBS

For parsec setup create an account on the VM and make other hosts add that account as a friend. From VM account allow restreamer friends to have full access over the PC. For the restreamers, make sure to have in the settings > client tab > window mode windowed & immersive mode off.
