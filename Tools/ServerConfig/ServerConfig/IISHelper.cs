//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.DirectoryServices;

public class IISObject 
{
    protected DirectoryEntry d;

    protected IISObject(DirectoryEntry d) 
    {
        this.d = d;
    }

    public string KeyType 
    {
        get 
        {
            return (string) d.Properties["KeyType"][0];
        }
    }

    public IISObject this[string path] 
    {
        get 
        {
            foreach(DirectoryEntry de in d.Children) 
            {
                if ( de.Path == path ) 
                {
                    return new IISObject(de);
                }
            }

            return null;
        }
    }
}

public class IISWebServer : IISObject 
{
    public IISWebServer(DirectoryEntry d) : base(d) 
    {
    }

    public int ServerState 
    {
        get 
        {
            return (int) d.Properties["ServerState"][0];
        }
    }

    public string DefaultDoc 
    {
        get 
        {
            return (string) d.Properties["DefaultDoc"][0];
        }
    }
}

public class IISWebVirtualDir : IISObject 
{
    public IISWebVirtualDir(DirectoryEntry d) : base(d) 
    {
    }

    public string AppRoot 
    {
        get 
        {
            return (string) d.Properties["AppRoot"][0];
        }
        set 
        {
            d.Properties["AppRoot"][0] = value; d.CommitChanges();
        }
    }

    public string AppFriendlyName 
    {
        get 
        {
            return (string) d.Properties["AppFriendlyName"][0];
        }
    }

    public int AppIsolated 
    {
        get 
        {
            return (int) d.Properties["AppRoot"][0];
        }
    }

    public bool AccessRead 
    {
        get 
        {
            return (bool) d.Properties["AccessRead"][0];
        }
        set 
        {
            d.Properties["AccessRead"][0] = value; d.CommitChanges();
        }
    }

    public bool AccessWrite 
    {
        get 
        {
            return (bool) d.Properties["AccessWrite"][0];
        }
        set 
        {
            d.Properties["AccessWrite"][0] = value; d.CommitChanges();
        }
    }

    public bool AccessExecute 
    {
        get 
        {
            return (bool) d.Properties["AccessExecute"][0];
        }
        set 
        {
            d.Properties["AccessExecute"][0] = value; d.CommitChanges();
        }
    }

    public bool AccessScript 
    {
        get 
        {
            return (bool) d.Properties["AccessScript"][0];
        }
        set 
        {
            d.Properties["AccessScript"][0] = value; d.CommitChanges();
        }
    }

    public bool AccessSource 
    {
        get 
        {
            return (bool) d.Properties["AccessSource"][0];
        }
    }

    public bool AccessNoRemoteRead 
    {
        get 
        {
            return (bool) d.Properties["AccessNoRemoteRead"][0];
        }
    }

    public bool EnableDefaultDoc 
    {
        get 
        {
            return (bool) d.Properties["EnableDefaultDoc"][0];
        }
        set 
        {
            d.Properties["EnableDefaultDoc"][0] = value; d.CommitChanges();
        }
    }

    public bool EnableDirBrowsing 
    {
        get 
        {
            return (bool) d.Properties["EnableDirBrowsing"][0];
        }
        set 
        {
            d.Properties["EnableDirBrowsing"][0] = value; d.CommitChanges();
        }
    }

    public string DefaultDoc 
    {
        get 
        {
            return (string) d.Properties["DefaultDoc"][0];
        }
        set 
        {
            d.Properties["DefaultDoc"][0] = value; d.CommitChanges();
        }
    }

    public string Path 
    {
        get 
        {
            return (string) d.Properties["Path"][0];
        }
        set 
        {
            d.Properties["Path"][0] = value; d.CommitChanges();
        }
    }

    public IISWebVirtualDir AddWebDir(string name)
    {
        DirectoryEntry de = d.Children.Add(name, "IISWebVirtualDir");
        de.CommitChanges();

        IISWebVirtualDir iDir = new IISWebVirtualDir(de);
        return iDir;
    }

    public IISWebVirtualDir AddWebVirtualDir(string name, string path) 
    {
        DirectoryEntry de = d.Children.Add(name, d.SchemaClassName);
        de.CommitChanges();

        IISWebVirtualDir iDir = new IISWebVirtualDir(de);
        iDir.AppRoot = d.Properties["AppRoot"][0] + "/" + name;
        iDir.Path = path;

        return iDir;
    }
}
