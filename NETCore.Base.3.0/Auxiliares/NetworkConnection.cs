using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;

namespace NETCore.Base._3._0.Auxiliares
{
    public class NetworkConnection : IDisposable
    {
        private readonly string _networkName;

        public NetworkConnection(string networkName, NetworkCredential credentials)
        {
            this._networkName = networkName;
            int error = NetworkConnection.WNetAddConnection2(new NetResource()
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Network,
                RemoteName = networkName.TrimEnd('\\')
            }, credentials.Password, credentials.UserName, 0);
            if (error != 0)
                throw new Win32Exception(error);
        }

        public event EventHandler<EventArgs> Disposed;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                EventHandler<EventArgs> disposed = this.Disposed;
                if (disposed != null)
                    disposed((object)this, EventArgs.Empty);
            }
            NetworkConnection.WNetCancelConnection2(this._networkName, 0, true);
        }

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(
          NetResource netResource,
          string password,
          string username,
          int flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags, bool force);

        ~NetworkConnection()
        {
            this.Dispose(false);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;
        public int Usage;
        public string LocalName;
        public string RemoteName;
        public string Comment;
        public string Provider;
    }
    public enum ResourceScope
    {
        Connected = 1,
        GlobalNetwork = 2,
        Remembered = 3,
        Recent = 4,
        Context = 5,
    }
    public enum ResourceType
    {
        Any = 0,
        Disk = 1,
        Print = 2,
        Reserved = 8,
    }
    public enum ResourceDisplaytype
    {
        Generic,
        Domain,
        Server,
        Share,
        File,
        Group,
        Network,
        Root,
        Shareadmin,
        Directory,
        Tree,
        Ndscontainer,
    }
}
