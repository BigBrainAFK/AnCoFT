using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AnCoFT.Networking.Server.Base
{
    public class UdpServer
    {
        private UdpClient udpClient = null;
        private IPEndPoint groupEP = null;
        public UdpServer(int port)
        {
            this.udpClient = new UdpClient(port);
            this.groupEP = new IPEndPoint(IPAddress.Any, port);
        }

        public void StartListening()
        {
            Thread t = new Thread(ListenerThread);
            t.Start();
        }

        public void ListenerThread()
        {
            while (true)
            {
                byte[] bytes = this.udpClient.Receive(ref groupEP);
                //this.udpClient.Send(bytes, bytes.Length);

                Console.WriteLine($"Received broadcast from {groupEP} :");
                //byte[] reversed = bytes.Reverse().ToArray();
                Console.WriteLine($"UDP-RECV {BitConverter.ToString(bytes, 0, bytes.Length)}");

                byte[] decrypted = this.Decrypt(bytes, bytes.Length);
                Console.WriteLine($"UDP-RECV-DECRYPTED {BitConverter.ToString(decrypted, 0, bytes.Length)}");
            }
        }

        public byte[] Decrypt(byte[] buffer, int size)
        {
            byte[] decrypted = new byte[size];
            Array.Copy(buffer, decrypted, size);

            for (int i = 4; i < size; ++i)
            {
                decrypted[i] ^= (byte)0xB7u;
            }

            return decrypted;
        }
    }
}
