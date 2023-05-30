本项目基于nginx实现负载均衡与CDN，通过两个tomcat进行实验。

Nginx 是高性能的 HTTP 和反向代理的web服务器，处理高并发能力是十分强大的，能经受高负载的考验,有报告表明能支持高达 50,000 个并发连接数。其特点是占有内存少，并发能力强，事实上nginx的并发能力确实在同类型的网页服务器中表现较好，中国大陆使用nginx网站用户有：百度、京东、新浪、网易、腾讯、淘宝等

### 负载均衡

准备两个tomcat，分别部署在8080和8082端口。通过修改/conf/server.xml文件中的server默认端口、http协议默认端口以及ajp协议默认端口即可实现部署。同时在两个tomcat的/webapp/下新建edu目录，分别写入html文件，用于指示当前运行的tomcat。

修改nginx的配置文件，监听8080和8082端口，并将两个tomcat反向代理。通过访问www.puhua datamining.com/edu/a.html进入tomcat网页中。默认采用轮询的方式显示负载均衡，每次访问该网址，都会在两个tomcat网页中轮流访问。此外可以通过修改配置文件中的参数来实现Weight法、IP Hash等方法，用于负载均衡。

### CDN

使用 Nginx 处理静态页面，Tomcat 处理动态页面。在linux中准备实验用的静态资源，在/data/目录下新建两个子目录/data/image/和/data/www/分别用于存放图片和网站资源。其中image目录下存放了一张图片，而www目录下存放了.html文件。

修改nginx配置文件，新增location /www/和location /image/。通过 location 指定不同的后缀名实现不同的请求转发。访问www.puhua datamining.com/image/JNU.png即可访问图片静态资源，而访问www.puhua datamining.com/www/a.html即可访问网页静态资源