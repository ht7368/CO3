using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Cards
{
    public partial class MenuUI : Form
    {
        public MenuUI()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonDeck_Click(object sender, EventArgs e)
        {
            var Deck = new DeckUI();
            Deck.ShowDialog();
        }

        private void buttonHost_Click(object sender, EventArgs e)
        {
            /*Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
            Socket.Connect("8.8.8.8", 65530);
            string LocalIP = (Socket.LocalEndPoint as IPEndPoint).Address.ToString();

            MessageBox.Show(caption: "Information", text: $"Connect to: {LocalIP}");*/

            var Game = new GameUI();
            Game.ShowDialog();
        }

        private void buttonConn_Click(object sender, EventArgs e)
        {
            var IpWindow = new ConnectUI();
            IpWindow.ShowDialog();
            if (string.IsNullOrWhiteSpace(IpWindow.IP))
                return;
            var Game = new GameUI(IpWindow.IP);
            Game.ShowDialog();
        }
    }
}
