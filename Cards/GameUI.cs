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
            };
            Game.InitUI();
            Controls.Add(Game);
        }
    }

    public class GameBox : GroupBox
    {
        public const int CARD_WIDTH = 100;
        public const int CARD_HEIGHT = 200;
        public const int CARD_SPACING = 5;

        private GameState Game;
        private PowerBox PowerRegion;
        private CardGroupBox EnemyHand;
        private CardGroupBox PlayerHand;
        private CardGroupBox EnemyBoard;
        private CardGroupBox PlayerBoard;

        public MoveProcessor Processor = new MoveProcessor();

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
            PowerRegion.UpdateUI();
        }

        // Initialise the UI by setting out ALL of the objec
        public void InitUI()
        {
            Game = new GameState();

            PowerRegion = new PowerBox(Game.CurrentPower);
            Controls.Add(PowerRegion);
            PowerRegion.Location = new Point(100, 100);
            PowerRegion.BringToFront();

            // Everything past here sets up the hand and board zones
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
            int BoxHeight = (this.Height - 5 * CARD_SPACING) / 4; // Can replace with BOX_SPACING
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
            }

            Game.PlayerTwo.Hand.Add(Cards.CardDB[0]);
            Game.PlayerTwo.Hand.Add(Cards.CardDB[1]);
            Game.PlayerTwo.Hand.Add(Cards.CardDB[0]);
            Game.PlayerTwo.Hand.Add(Cards.CardDB[1]);
            Game.PlayerOne.Hand.Add(Cards.CardDB[1]);
            Game.PlayerTwo.Board.Add(Cards.CardDB[0] as MinionCard);
            Game.PlayerOne.Board.Add(Cards.CardDB[0] as MinionCard);
            Game.CurrentPower = Cards.CardDB[1] as PowerCard;

            RenderState(Game);
        }
    }
}
