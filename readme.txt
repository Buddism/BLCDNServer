publish/  contains prebuilt executables
BLCacheParser/ and BLFileServer/ contain source code

BLFileServer: 
	hosts the file server binding on the ip and port in appsettings.json, 0.0.0.0 is ALL bindable ips
	need to port forward the port 5022 or the one you set in appsettings.json

BLCacheParser:
	converts a cache.db into a folder (blobs/) full of the compressed .bz2 format (the format bl uses for cdn) of each file in the cache
	1: drag and drop cache.db ontop of BLCacheParser
	2: it will create blobs/ where cache.db is located
	3: move it into the folder with BLFileServer

Requires clients and the server to have the add-on Support_CustomCDN by Queuenard
	https://blocklandglass.com/addons/addon/1580

at the top of [Support_CustomCDN/server.cs] add:
$CustomCDN::CDN_to_clients = "serversPublicIpAddress:port/blobs";