apt-get --yes --force-yes update
apt-get --yes --force-yes install build-essential libpcre3 libpcre3-dev libssl-dev
apt-get --yes --force-yes install --reinstall zlib1g zlib1g-dev

wget http://nginx.org/download/nginx-1.25.4.tar.gz
wget https://github.com/arut/nginx-rtmp-module/archive/master.zip
apt-get --yes --force-yes install unzip
unzip master.zip 
tar xvf nginx-1.25.4.tar.gz
cd nginx-1.25.4/

./configure --with-http_ssl_module --add-module=../nginx-rtmp-module-master
make
make install

sudo /usr/local/nginx/sbin/nginx
ps aux | grep nginx
