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

        GameBox Game = new GameBox();

        public GameUI()
        {
            InitializeComponent();
        }

        public void GameUI_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Game = new GameBox
            {
                Location = new Point(0, 0),
                Width = Screen.PrimaryScreen.Bounds.Width,
                Height = Screen.PrimaryScreen.Bounds.Height,
                //BackColor = Color.LightYellow,
            };
            Game.InitUI();
            Controls.Add(Game);
            Update();
        }
    }

    public class GameBox : GroupBox
    {
        const int CARD_WIDTH = 100;
        const int CARD_HEIGHT = 200;
        const int CARD_SPACING = 5;

        GameState Game;
        CardGroupBox EnemyHand;
        CardGroupBox PlayerHand;
        CardGroupBox EnemyBoard;
        CardGroupBox PlayerBoard;

        public GameBox()
        {

        }

        // Will update the visuals with the current board state.
        public void RenderState(GameState state)
        {
            EnemyHand.UpdateCards();
            EnemyBoard.UpdateCards();
            PlayerHand.UpdateCards();
            PlayerBoard.UpdateCards();
        }

        // Initialise the UI by setting out ALL of the objec
        public void InitUI()
        {

            Game = new GameState();

            EnemyHand = new CardGroupBox(Game.PlayerTwo.Hand);
            Controls.Add(EnemyHand);
            PlayerHand = new CardGroupBox(Game.PlayerOne.Hand);
            Controls.Add(PlayerHand);
            // These two are List<MinionCard>s, but they need to be List<BaseCard>s, so cast
            EnemyBoard = new CardGroupBox(Game.PlayerTwo.Board.Cast<BaseCard>().ToList());
            Controls.Add(EnemyBoard);
            PlayerBoard = new CardGroupBox(Game.PlayerOne.Board.Cast<BaseCard>().ToList());
            Controls.Add(PlayerBoard);

            // Position on the y-coordinate so that:
            // a. each box has an equal height
            // b. each box has the same spacing between them
            // c. the boxes are spaced evenly throughout the window
            int BoxHeight = (this.Height - (5 * CARD_SPACING)) / 4; // * 4; // Can replace with BOX_SPACING
            EnemyHand.Height = BoxHeight;
            EnemyBoard.Height = BoxHeight;
            PlayerHand.Height = BoxHeight;
            PlayerBoard.Height = BoxHeight;

            var TempArr = new CardGroupBox[] { EnemyHand, EnemyBoard, PlayerBoard, PlayerHand };
            for (int i = 0; i < 4; i++)
            {
                TempArr[i].Top = (i + 1) * CARD_SPACING + i * BoxHeight;
                TempArr[i].Left = CARD_SPACING;
                TempArr[i].Width = 1000;
                TempArr[i].Height = BoxHeight;
            }
            //TempArr[1].Top = BoxHeight;
            /*
            TempArr[0].BackColor = Color.Red;
            TempArr[1].BackColor = Color.Blue;
            TempArr[2].BackColor = Color.Green;
            TempArr[3].BackColor = Color.Purple;
            */

            Game.PlayerTwo.Hand.Add(Cards.CardDB[0]);
            Game.PlayerTwo.Hand.Add(Cards.CardDB[1]);
            Game.PlayerTwo.Hand.Add(Cards.CardDB[0]);
            Game.PlayerTwo.Hand.Add(Cards.CardDB[1]);
            Game.PlayerOne.Hand.Add(Cards.CardDB[1]);
            Game.PlayerTwo.Board.Add(Cards.CardDB[0] as MinionCard);
            Game.PlayerOne.Board.Add(Cards.CardDB[0] as MinionCard);

            RenderState(Game);
        }
    }
}
