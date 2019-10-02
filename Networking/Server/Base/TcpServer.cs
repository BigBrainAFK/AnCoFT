namespace AnCoFT.Networking.Server.Base
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    using AnCoFT.Database;

    public abstract class TcpServer
    {
        protected bool Stopped = false;

        protected TcpServer(string ipAddress, int port, DatabaseContext databaseContext)
        {
            this.Listener = new TcpListener(IPAddress.Parse(ipAddress), port);
            this.DatabaseContext = databaseContext;
        }

        protected DatabaseContext DatabaseContext { get; set; }

        protected TcpListener Listener { get; set; }

        protected NetworkStream Stream { get; set; }

        public void Start()
        {
            Thread listenerThread = new Thread(this.ListenerThread);
            listenerThread.Start();
        }

        public abstract void ListenerThread();

        public abstract void ReceivingThread(object client);

        public void Stop()
        {
            this.Stopped = true;
            this.Listener.Stop();
        }
    }
}