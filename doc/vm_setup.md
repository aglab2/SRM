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

# Watching

Use cmder to pull up the streams from rtmp server like
`mpv rtmp://NEW_YORK_IP/live/test`
Capture MPV using obs WindowCapture with modern Windows 10 way (not BitBlt)

For the commentators, please allow them to use VLC as it is more convenient as it has UI. Put the same address to Media > Open Network Stream...

# Parsec 

For parsec setup create an account on the VM and make other hosts add that account as a friend. From VM account allow restreamer friends to have full access over the PC. For the restreamers, make sure to have in the settings > client tab > window mode windowed & immersive mode off.
