tee -a /usr/local/nginx/conf/nginx.conf << END
rtmp {
  server {
    listen 1935;
    chunk_size 4096;

    application live {
      live on;

      play_restart on;
      record off;
      
      push rtmp://157.245.113.176/live;
    }
  }
}
END

/usr/local/nginx/sbin/nginx -s reload
