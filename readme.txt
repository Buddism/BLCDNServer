prebuilt executables are contained https://github.com/Buddism/BLCDNServer/releases
BLCacheParser/ and BLFileServer/ contain source code

BLFileServer: 
	hosts the file server binding on the ip and port in appsettings.json, 0.0.0.0 is ALL bindable ips
	need to port forward the port 28000 (OR THE ONE BL USES) (BOTH TCP+UDP) or the one you set in appsettings.json
	if players can join your server without you port forwarding you might not need to port forward (blocklands UPNP opens both TCP and UDP)

BLCacheParser:
	converts a cache.db into a folder (blobs/) full of the compressed .bz2 format (the format bl uses for cdn) of each file in the cache
	1: drag and drop cache.db ontop of BLCacheParser
	2: it will create blobs/ where cache.db is located
	3: move it into the folder with BLFileServer

Requires clients and the server to have the add-on Support_CustomCDN by Queuenard
	https://blocklandglass.com/addons/addon/1580

at the top of [Support_CustomCDN/server.cs] fill in your ip address and port, and add:
$CustomCDN::CDN_to_clients = "serversPublicIpAddress:port/blobs";

example:
	$CustomCDN::CDN_to_clients = "123.123.12.123:28000/blobs"

get your public ip address by google searching "what is my ip", and google should tell you (may have to scroll a little)
