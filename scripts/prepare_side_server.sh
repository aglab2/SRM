tee -a /usr/local/nginx/conf/nginx.conf << END
rtmp {
  server {
    listen 1935;
    chunk_size 4096;

    application live {
      live on;

      play_restart on;
      record off;
      
      push rtmp://138.197.96.49/live;
    }
  }
}
END

/usr/local/nginx/sbin/nginx -s reload
