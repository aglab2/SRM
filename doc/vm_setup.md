# Virtual Machine

This tutorial describes the setup for the virtual machine stored on coreweave.com
* Register on coreweave.com and create a VM on LGA1, RTX 4000, 8 Cores, Windows 10
* * Make sure to use static IP to share with restreamers
* When machine starts, install the following software
* * Virtual Audio Cable Lite
* * 7zip
* * RustDesk
* * cmder
* * MPV
* * streamlink
* * MEGA Sync
* * OBS
* * Notepad++
* * LiveSplit
* * VLC
* * Sordum dControl
* * Tuna OBS
* Run dControl and disable all antivirus features except for Firewall. Make sure to check Windows Security panel as well.
* Mute all system sounds in volume mixer (right click sound icon > Volume Mixer). Disable all notifications & actions.
* In RustDesk set permanent password and remember the id.
* Unpack cmder to C:/cmder and mpv to C:/mpv.
* Pin C:/cmder/cmder.exe to task bar. Launch, if asked select "Unblock and Continue"
* Use selective sync in MEGASync to synchronize only needed single folder to C:/SRM
* Install all Kurri fonts.
* Deploy OBS and load the JSON file from 'obs' folder. Provide the paths for missing files in C:/SRM
* In music select the folder with all the tracks C:/SRM/music. Click on cog near music >  Open Tools > Tuna Settings. Change refresh rate to 200ms. Click "Add New". Set song info path to C:/tuna.txt, set format to "{album} - {artists} - {title}". In VLC tab select "Slideshow" scene and add "music" source. Go back to "Basic", select "VLC" song source Click "OK". Click "Apply", click "Start".
* In OBS settings ensure that Mic is disabled, stream resolution is set to 720p 5000 bitrate.
* Put files in 'config' folder to %appdata%
* Fix paths using script in fix_path.lua
* Setup all info_* scripts in OBS

# Add paths to env vars
Search for "environment" and select "Edit the system environment variables".

# Watching

Use cmder to pull up the streams from rtmp server like
`mpv rtmp://NEW_YORK_IP/live/test`
Capture MPV using obs WindowCapture with modern Windows 10 way (not BitBlt)

For the commentators, please allow them to use VLC as it is more convenient as it has UI. Put the same address to Media > Open Network Stream...

# Parsec 

For parsec setup create an account on the VM and make other hosts add that account as a friend. From VM account allow restreamer friends to have full access over the PC. For the restreamers, make sure to have in the settings > client tab > window mode windowed & immersive mode off.
