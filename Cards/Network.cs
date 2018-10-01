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
        public const int Port = 1420; // Arbitrarily chosen, will change again if a problem arises.

        // Static to avoid having multiple connections open at once (this won't work!)
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

    // A move is a class that can be used to represent a change from one gamestate to the next.
    // It consists of the ID of a selected card, which is the one being played. This cannot be null!
    // There is also the ID of a targeted card, for example if a spell is being cast onto a minion,
    // The spell is the selected card, and the minion is the targeted card.
    // The targeted card can be null, because some cards do not have targets, this is represented by 0.
    public class Move
    {
        public uint Selected;
        public uint Targeted;

        // Construct with no targeted card
        public Move(uint selected)
        {
            this._Move(selected, targeted: 0);
        }

        // Constructed with a target card
        public Move(uint selected, uint targeted)
        {
            this._Move(selected, targeted);
        }

        private void _Move(uint selected, uint targeted)
        {
            if (selected == 0)
                throw new ArgumentException();
            Selected = selected;
            Targeted = targeted;
        }
    }

}
