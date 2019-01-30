using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            var Game = new GameUI();
            Game.ShowDialog();
        }

        private void buttonConn_Click(object sender, EventArgs e)
        {
            var Game = new GameUI("192.168.1.68");
            //var Game = new GameUI("81.129.225.127");
            Game.ShowDialog();
        }
    }
}
