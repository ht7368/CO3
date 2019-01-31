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
        public static NetworkStream Stream;

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

        public Network(string hostname)
        {
            Client = new TcpClient(hostname, Port);
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

        public async Task<Move> Recieve()
        {
            byte[] Buf = new byte[8]; // 4 bytes per uint, 2 uints
            await Stream.ReadAsync(Buf, 0, Buf.Length);

            uint Selected = BitConverter.ToUInt32(Buf, 0);
            uint Targeted = BitConverter.ToUInt32(Buf, 4);
            return new Move(Selected, Targeted);
        }

        public void SendDeck(byte[] deckcode)
        {
            Stream.Write(deckcode, 0, deckcode.Length);
        }

        public byte[] RecieveDeck()
        {
            // 25-card decks
            byte[] Buf = new byte[25];
            Stream.Read(Buf, 0, Buf.Length);
            return Buf;
        }

        public void SendRandomSeed(int seed)
        {
            byte[] Buf = BitConverter.GetBytes(seed);
            Stream.Write(Buf, 0, Buf.Length);
        }

        public int RecieveRandomSeed()
        {
            byte[] Buf = new byte[4];
            Stream.Read(Buf, 0, Buf.Length);
            return BitConverter.ToInt32(Buf, 0);
        }
    }

    

    public class EffectData<T> : Dictionary<Effect, Action<GameState, T>> where T: BaseCard
    {
        // Leaving this blank, it just acts as a more specific dictionary with a shorter name,
        // whilst giving us the option to add specific functionality later.
    }
}
