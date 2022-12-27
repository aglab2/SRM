# RTMP Streaming Setup

## Initial RTMP Setup

Once you have your machine booted up, you'll need to install the RTMP server software. There's a great, open source Nginx plugin available that most people use, and that we'll use in this guide: https://github.com/arut/nginx-rtmp-module.

### Install Nginx and the RTMP module

This is a mostly automatic process that just takes a few commands to download and install everything. It does not matter where you run any of these commands from, as the install will automatically put them in the right places. First up, dependencies:

```
sudo apt-get update
sudo apt-get install build-essential libpcre3 libpcre3-dev libssl-dev
apt-get install --reinstall zlib1g zlib1g-dev
```

The top line are general dependencies of Nginx and the RTMP module. The second line is for unzipping the source archives while we're installing. If you can use `unzip` on the machine successfully, you probably don't need to run that line.

Up next, downloading and unpacking:

```
wget http://nginx.org/download/nginx-1.23.3.tar.gz
wget https://github.com/arut/nginx-rtmp-module/archive/master.zip
apt install unzip
unzip master.zip 
tar xvf nginx-1.23.3.tar.gz
cd nginx-1.23.3/
```

Nothing crazy here, just downlading the source archives and unzipping them into full folders. Last step of installation, building Nginx with the RTMP module included:

```
./configure --with-http_ssl_module --add-module=../nginx-rtmp-module-master
make
sudo make install
```

With that you should be able to start Nginx and see that it's running:

```
sudo /usr/local/nginx/sbin/nginx
ps aux | grep nginx
```

The `ps aux` line should show two entries for Nginx: a master and a worker process. If it does, nginx is running and your installation is complete.

### Configuring RTMP

Now that everything is running, we can edit the configuration to make the RTMP service available. Open up the nginx configuration in an editor of your choice:

```
nano /usr/local/nginx/conf/nginx.conf
```

Then add these lines to the bottom of the file:

```
rtmp {
  server {
    listen 1935;
    chunk_size 4096;

    application live {
      live on;

      play_restart on;
      record off;
    }
  }
}
```

I won't bother explaining all of what's happening here, but essentially it's saying to make an RTMP server available on port 1935 (the default port for the RTMP protocol), then to make it available for other people to load.

After adding the configuration, Nginx needs to reload to start using that configuration. For some reason, the `systemctl` service is not always registered, so directly reloading nginx is the safest bet:

```
sudo /usr/local/nginx/sbin/nginx -s reload
```

### Testing streams and URLs

Runners should stream to the server using the URL `rtmp://your.rtmp.server.com/live` and a stream key of your choosing. To test the stream, you can load it in VLC or any other network player at the same URL with their stream key appended to it. So, as an example:

```
Runner OBS Stream URL: rtmp://stream.example.com/live
Runner OBS Stream Key: my_dude
VLC Stream URL: rtmp://stream.example.com/live/my_dude
```

It's important to note that **the stream keys and URL are case sensitive**. For simplicity, it's easiest to tell runners to only use lowercase letters.

### Multi-server Setup

If you have runners streaming from around the world, some of them may experience lots of dropped frames or latency from the server being too far away and their connection not being strong enough. The best thing to do in this situation is make a second server for them and have that server push the stream on to your main server.

For example: In Spyrothon 4 we had people streaming from all over Europe, Eastern US, Western US, Saudi Arabia, and New Zealand. Our main server was set up in San Francisco, and the runners in Europs and Saudi Arabia were dropping a lot of frames trying to connect to it. The solution was to add new servers in London and New York, then have the San Francisco and London servers push streams to the New York server. Having the servers push the stream most of the distance lets us use their data center-tier internet connections that are lower latency and higher bandwidth, and the New York server was closer to our streaming location, so that was made to reduce latency on our end.

To configure multiple servers, run the same setup as above on each one, then all you have to do is add a single line on the other servers to push to your primary one. The full rtmp configuration for those would look like this:

```
rtmp {
  server {
    listen 1935;
    chunk_size 4096;

    application live {
      live on;

      play_restart on;
      record off;
      
      push rtmp://your.primary.rtmp.server.com/live;
    }
  }
}
```

The `push` line is the only thing added here. It works just like the OBS setup does, the stream key will get passed through automatically.

Using this setup, you can also consolidate what you load on your end to one server and know that all the streams will be available there. Similar to how Twitch has multiple ingest servers, but you can always access it from just `twitch.tv/username`.
