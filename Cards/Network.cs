using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Cards
{
    class Network
    {
        const int Port = 1420; // Arbitrarily chosen, will change again if a problem arises.

        // Static to avoid having multiple connections open at once (this won't work!)
        static TcpClient Client;
        static NetworkStream Stream;

        public Network()
        {
            if (!true)
            {
                // If connecting
                Client = new TcpClient();
            }
            else
            {
                // If hosting
                var Listener = new TcpListener(IPAddress.Any, Port);
                Listener.Start();
                Client = Listener.AcceptTcpClient();
            }
            Stream = Client.GetStream();
        }
    }
}
