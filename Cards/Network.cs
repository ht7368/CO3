using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Cards
{
    public class Network
    {
        public const int Port = 1420; // Arbitrarily chosen, will change again if a problem arises.

        // Static to avoid having multiple connections open at once
        static TcpClient Client;
        static NetworkStream Stream;

        // Construct an instance of Network that is a host, that is, it does not have to connect to an address.
        public Network()
        {
            // If hosting
            var Listener = new TcpListener(IPAddress.Any, Port);
            Listener.Start();
            // This will block the thread until a connection is achieved!
            Client = Listener.AcceptTcpClient();
            Stream = Client.GetStream();
        }

        public Network(string hostname, int port)
        {
            Client = new TcpClient(hostname, port);
            Stream = Client.GetStream();
        }

        public void Send(Move moveTaken)
        {
            // Convert the class into the binary representation of both IDs
            // First by splitting it into two byte arrays,
            byte[] First = BitConverter.GetBytes(moveTaken.Selected);
            byte[] Second = BitConverter.GetBytes(moveTaken.Targeted);
            // Next by copying these sequentially into a larger byte array,
            byte[] Package = new byte[First.Length + Second.Length];
            First.CopyTo(Package, 0);
            Second.CopyTo(Package, First.Length);
            // And then sending it
            Stream.Write(Package, 0, Package.Length);
        }

        public Move Recieve()
        {
            byte[] Buf = new byte[8]; // 4 bytes per uint, 2 uints
            Stream.Read(Buf, 0, Buf.Length);

            uint Selected = BitConverter.ToUInt32(Buf, 0);
            uint Targeted = BitConverter.ToUInt32(Buf, 4);
            return new Move(Selected, Targeted);
        }
    }

    

    public class EffectData<T> : Dictionary<Effect, Action<GameState>> where T: BaseCard
    {
        // Leaving this blank, it just acts as a more specific dictionary with a shorter name,
        // whilst giving us the option to add specific functionality later.
    }
}
