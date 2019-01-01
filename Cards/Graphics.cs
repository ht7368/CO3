using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Cards
{
    public class CardBox : Panel
    {
        public Color CARD_COLOR = Color.FromArgb(255, 107, 71, 71);

        public BaseCard CardReferenced;

        // Visual control elements
        private PictureBox CardArt;
        private Label CardName;
        private TextBox CardInfo;
        private Label CardAttack;
        private Label CardHealth;
        private Label CardCost;
        private PictureBox CardMinionIndicator;

        public CardBox(BaseCard card) : base()
        {
            CardReferenced = card;
            Size = new Size(GameBox.CARD_WIDTH, GameBox.CARD_HEIGHT);
            BackgroundImage = Properties.Resources.CardBody;

            CardArt = new PictureBox()
            {
                Size = new Size(74, 74),
                Location = new Point(13, 13),
            };
            if (CardReferenced.Art != null)
                CardArt.Image = CardReferenced.Art;
            CardName = new Label()
            {
                Text = card.Name,
                Location = new Point(5, 13 + 74 + 13),
                Size = new Size(90, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = CARD_COLOR,
                ForeColor = Color.White,
                Font = GameBox.CFont.GetFont(12),
            };
            CardInfo = new TextBox()
            {
                TextAlign = HorizontalAlignment.Center,
                Multiline = true,
                ReadOnly = true,
                Location = new Point(5, 13 + 74 + 13 + 25 + 7),
                Size = new Size(90, 65),
                Lines = new string[] { card.Description },
                BackColor = CARD_COLOR,
                BorderStyle = BorderStyle.None,
                ForeColor = Color.White,
                Cursor = Cursors.Arrow,
                Font = GameBox.CFont.GetFont(12),
            };
            CardCost = new Label()
            {
                Text = CardReferenced.ManaCost.ToString(),
                Size = new Size(22, 22),
                Top = GameBox.CARD_HEIGHT - 22 - 4,
                Left = 4,
                Font = GameBox.CFont.GetFont(13),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Image = Properties.Resources.ManaBox,
            };

            Controls.Add(CardArt);
            Controls.Add(CardInfo);
            Controls.Add(CardName);
            Controls.Add(CardCost);

            CardCost.BringToFront();

            // Different visual effects for minion cards
            if (card is MinionCard)
            {
                MinionCard minion = card as MinionCard;
                CardAttack = new Label()
                {
                    Text = minion.Attack.ToString(),
                    Size = new Size(22, 22),
                    Top = GameBox.CARD_HEIGHT - 22 - 4,
                    Left = GameBox.CARD_WIDTH - 4 - 22 - 22,
                    Font = GameBox.CFont.GetFont(13),
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Image = Properties.Resources.AttackBox,
                };
                Controls.Add(CardAttack);
                CardAttack.BringToFront();
                CardHealth = new Label()
                {
                    Text = minion.Health.ToString(),
                    Size = new Size(22, 22),
                    Top = GameBox.CARD_HEIGHT - 22 - 4,
                    Left = GameBox.CARD_WIDTH - 4 - 22,
                    Font = GameBox.CFont.GetFont(13),
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Image = Properties.Resources.HealthBox,
                };
                Controls.Add(CardHealth);
                CardHealth.BringToFront();
                CardMinionIndicator = new PictureBox()
                {
                    Size = new Size(22, 22),
                    Top = GameBox.CARD_HEIGHT - 22 - 4,
                    // Centred between CardCost and CardHealth
                    Left = (((CardCost.Left + 22) + CardHealth.Left) / 2) - 22,
                    Font = GameBox.CFont.GetFont(13),
                    ForeColor = Color.White,
                    Image = minion.CanAttack ? Properties.Resources.GreenBox : Properties.Resources.RedBox,
                };
                Controls.Add(CardMinionIndicator);
                CardMinionIndicator.BringToFront();

                //CardAttack.Text = (CardReferenced as MinionCard).Attack.ToString();
            }

            // Ensures clicking anywhere on the card counts as a click
            foreach (Control c in Controls)
                c.Click += (_s, _e) =>
                {
                    CardClicked();
                };
        }

        public void UpdateUI()
        {
            BackgroundImage = Properties.Resources.CardBody;
            if (CardReferenced is MinionCard)
            {
                MinionCard Minion = CardReferenced as MinionCard;
                CardMinionIndicator.Image = Minion.CanAttack ? Properties.Resources.GreenBox : Properties.Resources.RedBox;
                CardAttack.Text = Minion.Attack.ToString();
                CardHealth.Text = Minion.Health.ToString();
            }
        }

        // onClick for card elements
        public void CardClicked()
        {
            // Get the game from the top-level gamebox
            GameBox Game = (Parent.Parent as GameBox);
            // If nothing is selected yet, we set the selected card
            if (Game.SelectedCard == null)
            {
                Game.SelectedCard = CardReferenced;
                BackgroundImage = Properties.Resources.SelectedCardBody;
                return;
            }
            // Otherwise the move is completed and processed
            Game.Game.ProcessMove(new Move(Game.SelectedCard.Id, CardReferenced.Id));
            Game.SelectedCard = null;
            Game.RenderState(Game.Game);
        }
    }

    // Bespoke box for the power
    public class PowerBox : Panel
    {
        public PowerCard PowerCard;
        public uint LastPowerId;
        private Label PowerLabel;
        private CardBox Visual;
        private PictureBox NoPowerImage;

        public PowerBox(PowerCard power)
        {
            PowerLabel = new Label
            {
                Left = 2, // Aesthetic tweak - Nudge towards right
                BackColor = Color.FromArgb(114, 76, 61),
                Text = "CURRENT POWER",
                Font = GameBox.CFont.GetFont(12),
                Height = 10,
                Width = 150,
                ForeColor = Color.White,
            };
            NoPowerImage = new PictureBox()
            {
                Size = new Size(GameBox.CARD_WIDTH, GameBox.CARD_HEIGHT),
                Image = Properties.Resources.NoCardGraphic,
                Top = Height - GameBox.CARD_HEIGHT,
                Left = (this.Width - GameBox.CARD_WIDTH) / 2,
            };
            //Controls.Add(NoPowerImage);
            Controls.Add(PowerLabel);
            PowerLabel.BringToFront();
            BackColor = Color.FromArgb(114, 76, 61);
            UpdateUI(PowerCard);
        }

        public void UpdateUI(PowerCard p)
        {
            PowerCard = p;
            if (PowerCard == null)
            {
                NoPowerImage.Image = Properties.Resources.NoCardGraphic;
                NoPowerImage.BringToFront();
                return;
            }
            NoPowerImage.SendToBack();
            Controls.Remove(Visual);
            Visual = new CardBox(PowerCard as BaseCard)
            {
                Top = Height - GameBox.CARD_HEIGHT,
                Left = (this.Width - GameBox.CARD_WIDTH) / 2
            };
            Controls.Remove(Visual);
            Controls.Add(Visual);
        }
    }

    public class CardGroupBox<T> : Panel where T: BaseCard
    {
        // A List<BaseCard> (hand or board) is passed in and tracked
        // This is a reference to the same list, so that works fine
        private List<T> TrackedCards;
        private List<CardBox> DisplayedCards;

        public CardGroupBox(List<T> cards)
        {
            TrackedCards = cards;
        }

        public void InitUI()
        {
            // Firstly, create new CardBox -s from the tracked cards.
            DisplayedCards = TrackedCards.Select(c => new CardBox(c)).ToList();
        }

        public void UpdateUI()
        {

            SuspendLayout();
            // To update, we have to: 
            // UPDATE existing boxes with card info (they track their cards)
            // If their card no longer is in this zone, DELETE the box
            // CREATE new boxes for cards that aren't displayed
            // Whilst keeping order

            // Optimization trick:
            // track which cards aren't new by keeping track of their indexes
            // Defaults to false - true if the card will be rendered
            bool[] RenderedFlag = new bool[TrackedCards.Count];
            List<CardBox> PendingRemove = new List<CardBox>();

            foreach (CardBox box in DisplayedCards)
            {
                int Ind = TrackedCards.IndexOf(box.CardReferenced as T);
                if (Ind == -1)
                {
                    PendingRemove.Add(box);
                    Controls.Remove(box);
                }
                else
                {
                    box.UpdateUI();
                    RenderedFlag[Ind] = true;
                }
            }

            foreach (CardBox box in PendingRemove)
                DisplayedCards.Remove(box);
            
            for (int i = 0; i < RenderedFlag.Length; i++)
            {
                if (!RenderedFlag[i])
                {
                    CardBox Box = new CardBox(TrackedCards[i])
                    {
                        // Make it temporarily invisible
                        Top = 2000,
                    };
                    DisplayedCards.Add(Box);
                    Controls.Add(Box);
                }
            }

            // Now reposition
            int CoordY = (Height - GameBox.CARD_HEIGHT) / 2;
            int FirstX;
            if (DisplayedCards.Count % 2 == 0)
                // x = mid – (n / 2) * (g + w) + 0.5 * g
                FirstX = (this.Width / 2) - (DisplayedCards.Count / 2) * (GameBox.CARD_WIDTH + GameBox.CARD_SPACING) + (GameBox.CARD_SPACING / 2);
            else
                // mid – (n / 2) * (g + w) – 0.5 * w
                FirstX = (this.Width / 2) - (DisplayedCards.Count / 2) * (GameBox.CARD_WIDTH + GameBox.CARD_SPACING) - (GameBox.CARD_WIDTH / 2);

            int NextX = FirstX;
            foreach (CardBox c in DisplayedCards)
            {
                c.Left = NextX;
                c.Top = CoordY;
                NextX += (GameBox.CARD_WIDTH + GameBox.CARD_SPACING);
            }

            ResumeLayout();
        }
    }

    // A special box type for player heroes
    public class PlayerBox : CardBox
    {
        private Label HealthLabel;

        public PlayerBox(BaseCard card) : base(card)
        {
            Height = Width;
            Controls.Clear();
            Click += (_s, _e) =>
            {
                GameBox Box = (Parent as GameBox);
                if (Box.SelectedCard == null)
                    return;
                Box.Game.ProcessMove(new Move(Box.SelectedCard.Id, CardReferenced.Id));
                Box.SelectedCard = null;
                Box.RenderState(Box.Game);
            };

            HealthLabel = new Label();
            Controls.Add(HealthLabel);
        }

        public void InitUI()
        {
            HealthLabel.Size = new Size(42, 22);
            HealthLabel.ForeColor = Color.White;
            HealthLabel.Font = GameBox.CFont.GetFont(24);
            HealthLabel.BringToFront();
            if (CardReferenced.Game.PlayerOne.PlayerCard == CardReferenced)
            {
                BackgroundImage = Properties.Resources.HeroFramePlayer;
                HealthLabel.BackColor = Color.FromArgb(68, 197, 91);
                HealthLabel.Left = 7;
                HealthLabel.Top = Height - 22 - 9;
            }
            else
            {
                BackgroundImage = Properties.Resources.HeroFrameEnemy;
                HealthLabel.BackColor = Color.FromArgb(81, 81, 81);
                HealthLabel.Left = Width - 42 - 5;
                HealthLabel.Top = Height - 22 - 7;
            }
            UpdatePlayerUI();
        }

        public void UpdatePlayerUI()
        {
            if (CardReferenced.Game.PlayerOne.PlayerCard == CardReferenced)
                HealthLabel.Text = CardReferenced.Game.PlayerOne.Health.ToString();
            else
                HealthLabel.Text = CardReferenced.Game.PlayerTwo.Health.ToString();
        }
    }
}
