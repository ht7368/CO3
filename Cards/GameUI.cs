using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

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

            Game = new GameBox()
            {
                Location = new Point(0, 0),
                Width = Screen.PrimaryScreen.Bounds.Width,
                Height = Screen.PrimaryScreen.Bounds.Height,
                //BackColor = Color.LightYellow,
            };
            Game.Visible = false;
            Game.SendToBack();
            Game.InitUI();
            Controls.Add(Game);
            Game.Visible = true;
        }
    }

    public class GameBox : Panel
    {
        public const int CARD_WIDTH = 100;
        public const int CARD_HEIGHT = 200;
        public const int CARD_SPACING = 10;
        public const int CONTROL_SPACING = 50;
        public Color BACK_COLOR = Color.FromArgb(114, 76, 61);

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
        private Label NotificationLabel;

        public GameState Game;
        public BaseCard SelectedCard;

        public static CustomFont CFont = new CustomFont();

        public GameBox()
        {
            Game = new GameState();
        }

        // Will update the visuals with the current board state.
        public void RenderState(GameState state)
        {
            EnemyHand.UpdateUI();
            EnemyBoard.UpdateUI();
            PlayerHand.UpdateUI();
            PlayerBoard.UpdateUI();
            PowerRegion.UpdateUI(Game.CurrentPower);
            EnemyHero.UpdatePlayerUI();
            PlayerHero.UpdatePlayerUI();

            PlayerMana.Text = $"{Game.PlayerOne.Mana} / {Game.PlayerOne.MaxMana}";
            EnemyMana.Text = $"{Game.PlayerTwo.Mana} / {Game.PlayerTwo.MaxMana}";
        }

        // Initialise the UI by setting out ALL of the objects
        public void InitUI()
        {
            BackgroundImage = Properties.Resources.BackArea;

            // Will initialize the player regions
            EnemyHero = new PlayerBox(Game.PlayerTwo.PlayerCard);
            PlayerHero = new PlayerBox(Game.PlayerOne.PlayerCard);
            Controls.Add(EnemyHero);
            Controls.Add(PlayerHero);
            EnemyHero.Left = 1700;
            PlayerHero.Left = 1700;
            PlayerHero.Top = 1000;

            // Power region - card area where the power is displayed
            Game.CurrentPower = Cards.CardDB[1](Game) as PowerCard;
            PowerRegion = new PowerBox(Game.CurrentPower)
            {
                Location = new Point(100, 100)
            };
            Controls.Add(PowerRegion);
            PowerRegion.BringToFront();

            // Everything past here sets up the hand and board zones
            EnemyHand = new CardGroupBox<BaseCard>(Game.PlayerTwo.Hand)
            {
                BackgroundImage = Properties.Resources.CardArea,
                BackgroundImageLayout = ImageLayout.Stretch,
            }; 
            Controls.Add(EnemyHand);
            PlayerHand = new CardGroupBox<BaseCard>(Game.PlayerOne.Hand)
            {
                BackgroundImage = Properties.Resources.CardArea,
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(PlayerHand);
            EnemyBoard = new CardGroupBox<MinionCard>(Game.PlayerTwo.Board)
            {
                BackgroundImage = Properties.Resources.CardArea,
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(EnemyBoard);
            PlayerBoard = new CardGroupBox<MinionCard>(Game.PlayerOne.Board)
            {
                BackgroundImage = Properties.Resources.CardArea,
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(PlayerBoard);

            EnemyBoard.InitUI();
            EnemyHand.InitUI();
            PlayerBoard.InitUI();
            PlayerHand.InitUI();

            // Position on the y-coordinate so that:
            // a. each box has an equal height
            // b. each box has the same spacing between them
            // c. the boxes are spaced evenly throughout the window
            int BoxHeight = (this.Height - (5 * CONTROL_SPACING )) / 4; // * 4; 
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
                TempArr[i].Top = (i + 1) * CONTROL_SPACING + i * BoxHeight;
                TempArr[i].Left = CONTROL_SPACING;
                TempArr[i].Width = Screen.PrimaryScreen.Bounds.Width - DesiredHeight - 3 * CONTROL_SPACING;
                TempArr[i].Height = BoxHeight;

                // Now change the height:
                // If the height of the box is CARD_HEIGHT with a CARD_SPACING on either side,
                // The new y-coordinate must be old-y - (height(first) - height(second)/2

                TempArr[i].Top = TempArr[i].Top + (BoxHeight - DesiredHeight) / 2;
                TempArr[i].Height = DesiredHeight;
            }
            BoxHeight = DesiredHeight;

            // Positioned relative to hand and board zones
            EnemyHero.Top = EnemyHand.Top;
            EnemyHero.Left = EnemyHand.Left + EnemyHand.Width + CONTROL_SPACING;
            PlayerHero.Top = PlayerHand.Top;
            PlayerHero.Left = PlayerHand.Left + PlayerHand.Width + CONTROL_SPACING;

            // Resize the hero boxes with the new height
            EnemyHero.Width = BoxHeight;
            PlayerHero.Width = BoxHeight;
            EnemyHero.Height = BoxHeight;
            PlayerHero.Height = BoxHeight;

            PlayerHero.InitUI();
            EnemyHero.InitUI();

            // Power region is also positioned relative to the new hand and board zones
            PowerRegion.Top = EnemyBoard.Top;
            PowerRegion.Width = BoxHeight / 2;
            PowerRegion.Height = BoxHeight;
            PowerRegion.Left = EnemyBoard.Left + EnemyBoard.Width + CONTROL_SPACING;

            // Buttons;
            ResetButton = new Button()
            {
                Left = PlayerBoard.Left + PlayerBoard.Width + CONTROL_SPACING,
                Top = PlayerBoard.Top,
                Width = DesiredHeight,
                Text = "RESET SELECTION",
                Image = Properties.Resources.ButtonBase,
                Font = CFont.GetFont(12),
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
                BackColor = BACK_COLOR,
            };
            ResetButton.FlatAppearance.BorderSize = 0;
            ResetButton.FlatAppearance.BorderColor = BACK_COLOR;
            ResetButton.Click += ResetButton_Click;
            Controls.Add(ResetButton);
            ResetButton.BringToFront();

            DrawButton = new Button()
            {
                Left = PlayerBoard.Left + PlayerBoard.Width + CONTROL_SPACING,
                Top = PlayerBoard.Top,
                Width = DesiredHeight,
                Text = "DRAW A CARD",
                Image = Properties.Resources.ButtonBase,
                Font = CFont.GetFont(12),
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
                BackColor = BACK_COLOR,
            };
            DrawButton.FlatAppearance.BorderSize = 0;
            DrawButton.FlatAppearance.BorderColor = BACK_COLOR;
            DrawButton.Click += DrawButton_Click;
            Controls.Add(DrawButton);
            DrawButton.BringToFront();

            PassButton = new Button()
            {
                Left = PlayerBoard.Left + PlayerBoard.Width + CONTROL_SPACING,
                Top = PlayerBoard.Top,
                Width = DesiredHeight,
                Text = "END YOUR TURN",
                Image = Properties.Resources.ButtonBase,
                Font = CFont.GetFont(12),
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
                ImageAlign = ContentAlignment.MiddleCenter,
                BackColor = BACK_COLOR,
            };
            PassButton.FlatAppearance.BorderSize = 0;
            PassButton.FlatAppearance.BorderColor = BACK_COLOR;
            PassButton.Click += PassButton_Click;
            Controls.Add(PassButton);
            PassButton.BringToFront();

            // Calculating the heights for all of the buttons
            int REDUCED_SPACING = CONTROL_SPACING / 3; // Different spacing for better aesthetic effect
            int ButtonHeight = (DesiredHeight - 2 * REDUCED_SPACING) / 3;
            DrawButton.Height = ButtonHeight;
            PassButton.Height = ButtonHeight;
            ResetButton.Height = ButtonHeight;
            PassButton.Top += ButtonHeight + REDUCED_SPACING;
            ResetButton.Top = PassButton.Top + ButtonHeight + REDUCED_SPACING;

            // Mana Labels
            int LabelWidth = (DesiredHeight - CONTROL_SPACING) / 2;
            PlayerMana = new Label()
            {
                Top = EnemyBoard.Top,
                Left = EnemyBoard.Left + EnemyBoard.Width + 2 * CONTROL_SPACING + LabelWidth,
                Width = LabelWidth,
                Height = LabelWidth,
                Image = Properties.Resources.ManaDisplay,
                Font = CFont.GetFont(15),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            EnemyMana = new Label()
            {
                Top = EnemyBoard.Top,
                Left = EnemyBoard.Left + EnemyBoard.Width + 2 * CONTROL_SPACING + LabelWidth,
                Width = LabelWidth,
                Height = LabelWidth,
                Image = Properties.Resources.ManaDisplayAlt,
                Font = CFont.GetFont(15),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            PlayerMana.Top += CONTROL_SPACING + LabelWidth;
            Controls.Add(PlayerMana);
            Controls.Add(EnemyMana);

            // Notification label
            NotificationLabel = new Label()
            {
                Text = "",
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false,
                Width = this.Width,
                Height = 22,
                Left = 0,
                BackColor = Color.Black,
                ForeColor = Color.White,
                Font = CFont.GetFont(12),
            };
            Controls.Add(NotificationLabel);

            // TESTING add cards to hand initially
            Game.PlayerTwo.Hand.Add(Cards.CardDB[0](Game));
            Game.PlayerTwo.Hand.Add(Cards.CardDB[1](Game));
            Game.PlayerTwo.Hand.Add(Cards.CardDB[0](Game));
            Game.PlayerTwo.Hand.Add(Cards.CardDB[0](Game));
            Game.PlayerOne.Hand.Add(Cards.CardDB[0](Game));
            Game.PlayerOne.Hand.Add(Cards.CardDB[3](Game));
            Game.PlayerTwo.Hand.Add(Cards.CardDB[1](Game));
            Game.PlayerOne.Hand.Add(Cards.CardDB[2](Game));
            Game.PlayerOne.Hand.Add(Cards.CardDB[0](Game));
            Game.PlayerOne.Hand.Add(Cards.CardDB[5](Game));
            Game.PlayerTwo.Board.Add(Cards.CardDB[4](Game) as MinionCard);
            Game.PlayerTwo.Board[0].OnBoard = true;
            Game.PlayerOne.Board.Add(Cards.CardDB[4](Game) as MinionCard);
            Game.PlayerOne.Board[0].OnBoard = true;

            Random Rand = new Random();
            for (int i = 0; i < 25; i++)
                Game.PlayerOne.Deck.Add(Cards.CardDB[Rand.Next(0, Cards.CardDB.Count)](Game));
            for (int i = 0; i < 25; i++)
                Game.PlayerTwo.Deck.Add(Cards.CardDB[Rand.Next(0, Cards.CardDB.Count)](Game));

            RenderState(Game);
        }

        public void HideNotif()
        {
            NotificationLabel.Visible = false;
        }

        public void DisplayNotification(string messageText)
        {
            NotificationLabel.Text = messageText;
            NotificationLabel.Visible = true;
            NotificationLabel.BringToFront();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            SelectedCard = null;
            RenderState(Game);
        }

        private void DrawButton_Click(object sender, EventArgs e)
        {
            Game.ProcessMove(new Move(GameState.CARD_DRAW, 0));
            RenderState(Game);
        }

        private void PassButton_Click(object sender, EventArgs e)
        {
            SelectedCard = null;
            Game.SwitchTurns();
            RenderState(Game);
            if (Game.ActivePlayer == Game.PlayerOne)
                DisplayNotification("IT IS YOUR TURN!");
        }
    }
}
