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
        const int CARD_WIDTH = 100;
        const int CARD_HEIGHT = 200;
        const int CARD_SPACING = 5;

        GameState Game;

        public GameUI()
        {
            InitializeComponent();
        }

        public void RenderState(GameState state)
        {
            Controls.Clear();
            for (int i = 0; i < state.PlayerTwo.Hand.Count; i++)
            {
                Controls.Add(new CardBox(state.PlayerTwo.Hand[i]));
                Controls[Controls.Count - 1].Location = new Point(CARD_SPACING + i * CARD_WIDTH, CARD_SPACING);
            }
        }

        private void GameUI_Load(object sender, EventArgs e)
        {
            Game = new GameState();
            Game.PlayerTwo.Hand.Add(Cards.CardDB[0]);
            Game.PlayerTwo.Hand.Add(Cards.CardDB[1]);
            RenderState(Game);
        }
    }
}
