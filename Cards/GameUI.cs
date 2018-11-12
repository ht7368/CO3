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
        CardGroupBox EnemyHand;

        public GameUI()
        {
            InitializeComponent();
        }

        // Will update the visuals with the current board state.
        public void RenderState(GameState state)
        {
            CardBox[] Cards = new CardBox[state.PlayerTwo.Hand.Count];
            for (int i = 0; i < state.PlayerTwo.Hand.Count; i++)
                Cards[i] = new CardBox(state.PlayerTwo.Hand[i]);

            EnemyHand.Update(Cards);
        }

        // Initialise the UI by setting out ALL of the objec
        private void GameUI_Load(object sender, EventArgs e)
        {
            EnemyHand = new CardGroupBox();
            EnemyHand.Location = new Point(0, 0);
            EnemyHand.Width = 1920;
            Controls.Add(EnemyHand);

            Game = new GameState();
            Game.PlayerTwo.Hand.Add(Cards.CardDB[0]);
            Game.PlayerTwo.Hand.Add(Cards.CardDB[1]);
            Game.PlayerTwo.Hand.Add(Cards.CardDB[1]);
            Game.PlayerTwo.Hand.Add(Cards.CardDB[1]);
            RenderState(Game);

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }
    }
}
