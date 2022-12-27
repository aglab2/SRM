# RTMP
RTMP Servers are used to avoid restreaming content from twitch servers. Unlikely twitch, RTMP servers do not introduce extra delay and do not have any ads.

* Register account at digitalocean.com.
* Create a main droplet - New York matching LGA1 for coreweave. Droplet is basically a tiny VM hosted on digitalocean servers.
* * The recommended machine setup is 'Regular with SSD' + '$12/month' machine, there is no need to buy a beefy one.
* * Make sure appropriate 'Authentication' so 'ssh' can be used.
* * I suggest using good names for 'Choose a hostname' as it will make it easier to distinguish the droplets.
* Connect to the droplet over SSH and follow https://github.com/aglab2/SRM/blob/main/doc/rtmp.md to setup 'nginx' + 'rtmp server' till 'Multi-server Setup' section.
* * Make sure to check http://nginx.org/download/ for the newest version of nginx, as of time of writing http://nginx.org/download/nginx-1.23.1.tar.gz is the latest one so download command should look like this:
```
wget http://nginx.org/download/nginx-1.15.1.tar.gz
```
* Check if you can stream to it and watch the content back using MPV/VLC.
* * Set the stream key to 'test' and have 'Server' set as 'rtmp://SERVER_IP/live'
* * I suggest using MPV as the same tool will be used on the VM:
```
cd "C:\Program Files\MPV\"
mpv rtmp://SERVER_IP/live/test
```
* Perform the same procedure for servers for San Francisco/New York (the other than main one), Singapore and Amsterdam server and set them up using same method plus 'Multi-server Setup' while using IP of the main server.
* In the end, you will end up with 4 servers across the globe for different regions sinking content in the main New York/San Francisco server.
* Post these IPs to #runner_information channel in the form like
```
us/new york       = rtmp://IP1/live
us/san francisco  = rtmp://IP2/live
eu/amsterdam      = rtmp://IP3/live
asia/singapore    = rtmp://IP4/live
```
