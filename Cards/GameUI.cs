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
    public partial class GameUI : Form
    {
        const int CARD_WIDTH = 50;
        const int CARD_HEIGHT = 100;
        private List<CardBox> VisibleCards = new List<CardBox>();

        public GameUI()
        {
            InitializeComponent();
        }

        public void Update(GameState state)
        {
            VisibleCards.Clear();
            foreach (var c in state.PlayerTwo.Hand)
            {
                VisibleCards.Add(new CardBox(c));
            }
        }

        private void GameUI_Load(object sender, EventArgs e)
        {
            Update();
        }
    }
}
