OpLocks
========
A c# winforms program that makes it easy for a non-techincal user to make the registry changes required to disable opportunistic locking on Windows Vista, 7, Server 2008, 8, and Server 2012.

Opportunistic locking is a feature of SMB1.  It means that the server can give a client exclusive access to a file (which is called an oplock,) if no other processes need the file.  This allows a client to perform read-ahead, write-behind, and lock caching, which will increase performance for the client.  However, if a second process needs the file, the server will require the client to break the oplock.  At this point, the client has to flush all cached data and release the oplock.

Why can this be a problem?  According to Microsoft "If a multiuser or single user database application accesses a common data store on a Windows NT file server using opportunistic locks (or OPLOCKS), it is possible for a given user to cache partial transactions on the client systems hard drive. This is a performance enhancement to the Windows client redirector to reduce network file I/O between the client and server. The data being cached on the client redirector is later written back to the server. However, in some cases, a client system may stop responding (hang), do a hard reboot, lose its network connection to the server, or experience any number of other technical difficulties. In such cases, the content of the local cache that has not yet been written to the server can be lost. As a result, the transaction integrity of the database structures on the server is compromised and the data on the file server can become corrupted."

ISAM based databases are vulnerable to this kind of corruption.  Disabling oplocks on file servers for ISAM database servers removes this vulnerability.

