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
        public GameBox Game = new GameBox();

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
            Controls.Add(Game);
            Game.InitUI();
        }
    }

    public class GameBox : Panel
    {
        public const int CARD_WIDTH = 100;
        public const int CARD_HEIGHT = 200;
        public const int CARD_SPACING = 5;
        public const int CONTROL_SPACING = 10;

        private PowerBox PowerRegion;
        private CardGroupBox<BaseCard> EnemyHand;
        private CardGroupBox<BaseCard> PlayerHand;
        private CardGroupBox<MinionCard> EnemyBoard;
        private CardGroupBox<MinionCard> PlayerBoard;
        private Button PassButton;
        private Button ResetButton;
        private Button DrawButton;
        private Label PlayerMana;
        private Label EnemyMana;
        private PlayerBox EnemyHero;
        private PlayerBox PlayerHero;

        public GameState Game;
        public BaseCard SelectedCard;

        public GameBox()
        {
            Game = new GameState();
        }

        // Will update the visuals with the current board state.
        public void RenderState(GameState state)
        {
            EnemyHand.UpdateCards();
            EnemyBoard.UpdateCards();
            PlayerHand.UpdateCards();
            PlayerBoard.UpdateCards();
            PowerRegion.UpdateUI(Game.CurrentPower);

            PlayerMana.Text = $"P {Game.PlayerOne.Mana} out of {Game.PlayerOne.MaxMana}";
            EnemyMana.Text = $"E {Game.PlayerTwo.Mana} out of {Game.PlayerTwo.MaxMana}";
        }

        // Initialise the UI by setting out ALL of the objects
        public void InitUI()
        {
            PlayerMana = new Label();
            EnemyMana = new Label();
            Controls.Add(PlayerMana);
            Controls.Add(EnemyMana);
            PlayerMana.Top = 100;
            EnemyMana.Top = 200;

            // Will initialize the player regions
            EnemyHero = new PlayerBox(Game.PlayerTwo.PlayerCard);
            PlayerHero = new PlayerBox(Game.PlayerOne.PlayerCard);
            Controls.Add(EnemyHero);
            Controls.Add(PlayerHero);
            EnemyHero.Left = 1600;
            PlayerHero.Left = 1600;
            PlayerHero.Top = 1000;

            // Power region - card area where the power is displayed
            Game.CurrentPower = Cards.CardDB[1](Game) as PowerCard;
            PowerRegion = new PowerBox(Game.CurrentPower)
            {
                Location = new Point(100, 100)
            };
            Controls.Add(PowerRegion);
            PowerRegion.BringToFront();

            // Buttons;
            ResetButton = new Button()
            {
                Location = new Point(100, 130),
                Text = "Reset",
            };
            ResetButton.Click += ResetButton_Click;
            Controls.Add(ResetButton);
            ResetButton.BringToFront();

            DrawButton = new Button()
            {
                Location = new Point(100, 160),
                Text = "Draw",
            };
            DrawButton.Click += DrawButton_Click;
            Controls.Add(DrawButton);
            DrawButton.BringToFront();

            PassButton = new Button()
            {
                Location = new Point(100, 190),
                Text = "End Turn",
            };
            PassButton.Click += PassButton_Click;
            Controls.Add(PassButton);
            PassButton.BringToFront();

            // Everything past here sets up the hand and board zones
            EnemyHand = new CardGroupBox<BaseCard>(Game.PlayerTwo.Hand)
            {

            };
            Controls.Add(EnemyHand);
            PlayerHand = new CardGroupBox<BaseCard>(Game.PlayerOne.Hand)
            {

            };
            Controls.Add(PlayerHand);
            EnemyBoard = new CardGroupBox<MinionCard>(Game.PlayerTwo.Board)
            {
                //BackgroundImage = Properties.Resources.ScrollBody,
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(EnemyBoard);
            PlayerBoard = new CardGroupBox<MinionCard>(Game.PlayerOne.Board)
            {
                //BackgroundImage = Properties.Resources.ScrollBody,
            };
            Controls.Add(PlayerBoard);

            BackgroundImage = Properties.Resources.Wood;
            BackgroundImageLayout = ImageLayout.Tile;

            // Position on the y-coordinate so that:
            // a. each box has an equal height
            // b. each box has the same spacing between them
            // c. the boxes are spaced evenly throughout the window
            int BoxHeight = (this.Height - (5 * CARD_SPACING)) / 4; // * 4; 
            EnemyHand.Height = BoxHeight;
            EnemyBoard.Height = BoxHeight;
            PlayerHand.Height = BoxHeight;
            PlayerBoard.Height = BoxHeight;

            // The zone for player targets, buttons and other displayed information...
            // ... should have width equal to the height of the boxes, because it must
            // contain two squares (player targets) with that width (plus spacing)
            int DesiredHeight = CARD_HEIGHT + 2 * CARD_SPACING;

            var TempArr = new Control[] { EnemyHand, EnemyBoard, PlayerBoard, PlayerHand };
            for (int i = 0; i < 4; i++)
            {
                TempArr[i].Top = (i + 1) * CARD_SPACING + i * BoxHeight;
                TempArr[i].Left = CONTROL_SPACING;
                TempArr[i].Width = Screen.PrimaryScreen.Bounds.Width - BoxHeight - 3 * CONTROL_SPACING;
                TempArr[i].Height = BoxHeight;

                // Now change the height:
                // If the height of the box is CARD_HEIGHT with a CARD_SPACING on either side,
                // The new y-coordinate must be old-y - (height(first) - height(second)/2

                TempArr[i].Top = TempArr[i].Top + (BoxHeight - DesiredHeight) / 2;
                TempArr[i].Height = DesiredHeight;
            }
            BoxHeight = DesiredHeight;

            PowerRegion.Top = EnemyBoard.Top;
            PowerRegion.Width = BoxHeight / 2;
            PowerRegion.Height = BoxHeight;
            PowerRegion.Left = EnemyBoard.Left + EnemyBoard.Width + CONTROL_SPACING;

            Game.PlayerTwo.Hand.Add(Cards.CardDB[0](Game));
            Game.PlayerTwo.Hand.Add(Cards.CardDB[1](Game));
            Game.PlayerTwo.Hand.Add(Cards.CardDB[0](Game));
            Game.PlayerTwo.Hand.Add(Cards.CardDB[0](Game));
            Game.PlayerOne.Hand.Add(Cards.CardDB[0](Game));
            Game.PlayerTwo.Hand.Add(Cards.CardDB[1](Game));
            Game.PlayerOne.Hand.Add(Cards.CardDB[2](Game));
            Game.PlayerOne.Hand.Add(Cards.CardDB[0](Game));
            Game.PlayerOne.Hand.Add(Cards.CardDB[5](Game));
            Game.PlayerTwo.Board.Add(Cards.CardDB[4](Game) as MinionCard);
            Game.PlayerOne.Board.Add(Cards.CardDB[4](Game) as MinionCard);

            RenderState(Game);
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            SelectedCard = null;
            RenderState(Game);
        }

        private void DrawButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PassButton_Click(object sender, EventArgs e)
        {
            Game.SwitchTurns();
            PlayerMana.Text = $"P {Game.PlayerOne.Mana} out of {Game.PlayerOne.MaxMana}";
            EnemyMana.Text = $"E {Game.PlayerTwo.Mana} out of {Game.PlayerTwo.MaxMana}";
        }
    }
}
