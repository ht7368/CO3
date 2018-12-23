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

        public const int CARD_WIDTH = 100;
        public const int CARD_HEIGHT = 200;
        public const int CARD_SPACING = 5;

        protected BaseCard CardReferenced;

        // Visual control elements
        private PictureBox CardBase;
        private PictureBox CardArt;
        private Label CardName;
        private TextBox CardInfo;
        private Label CardAttack;
        private Label CardHealth;
        private Label CardCost;

        public CardBox(BaseCard card) : base()
        {
            CardReferenced = card;

            Size = new Size(100, 200);

            // Initializes all of these controls and adds them
            CardBase = new PictureBox()
            {
                Height = Height - 6,
                Width = Width - 6,
                Location = new Point(3, 3),
                // Image = ...,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Gray,
            };
            CardArt = new PictureBox()
            {
                Size = new Size(90, 90),
                Location = new Point(5, 5),
                Image = Properties.Resources.ArtPlaceholder,
                BackColor = Color.Transparent,
            };
            CardName = new Label()
            {
                Text = card.Name,
                Location = new Point(5, 100),
                Size = new Size(90, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.LightGray,
            };
            CardInfo = new TextBox()
            {
                TextAlign = HorizontalAlignment.Center,
                Multiline = true,
                Location = new Point(5, 130),
                Size = new Size(90, 65),
                Lines = new string[] { card.Description },
                Enabled = false,
            };
            CardCost = new Label()
            {
                Text = card.ManaCost.ToString(),
                Size = new Size(20, 20),
                Top = CARD_SPACING,
                Left = Width - 20 - CARD_SPACING,
            };

            Controls.Add(CardArt);
            Controls.Add(CardInfo);
            Controls.Add(CardName);
            Controls.Add(CardBase);
            Controls.Add(CardCost);

            CardCost.BringToFront();

            // Ensures clicking anywhere on the card counts as a click
            foreach (Control c  in Controls)
                c.Click += (_s, _e) =>
                {
                    CardClicked();
                };

            // Different visual effects for minion cards
            if (card is MinionCard)
            {
                CardAttack = new Label()
                {
                    Size = new Size(20, 20),
                    Location = new Point(0, Height - 20)
                };
                Controls.Add(CardAttack);
                CardAttack.BringToFront();
                CardHealth = new Label()
                {
                    Size = new Size(20, 20),
                    Location = new Point(Width - 20, Height - 20)
                };
                Controls.Add(CardHealth);
                CardHealth.BringToFront();

                CardAttack.Text = (CardReferenced as MinionCard).Attack.ToString();
                CardHealth.Text = (CardReferenced as MinionCard).Health.ToString();
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
                this.BackColor = Color.Red;
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

        public PowerBox(PowerCard power)
        {
            PowerCard = power;
            Height = CardBox.CARD_HEIGHT;
            Width = CardBox.CARD_WIDTH;
            Update();
        }

        public void UpdateUI(PowerCard p)
        {
            PowerCard = p;
            if (PowerCard == null)
            {
                // TODO: RENDER `No Power Played` 
                return;
            }
            Controls.Clear();
            CardBox Visual = new CardBox(PowerCard as BaseCard)
            {
                Location = new Point(0, 0),
            };
            Controls.Add(Visual);
        }
    }

    public class CardGroupBox<T> : Panel where T: BaseCard
    {
        // A List<BaseCard> (hand or board) is passed in and tracked
        // This is a reference to the same list, so that works fine
        private List<T> TrackedCards;

        public CardGroupBox(List<T> cards)
        {
            TrackedCards = cards;
        }

        public void UpdateCards()
        {
            // Firstly, create new CardBox -s from the tracked cards.
            List<CardBox> Cards = TrackedCards.Select(c => new CardBox(c)).ToList();

            //this.Height = 2 * CardBox.CARD_SPACING + CardBox.CARD_HEIGHT;

            // The y-coordinate of a card is set up so that it is centred
            int CoordY = this.Height / 2 - CardBox.CARD_HEIGHT / 2;

            this.Controls.Clear();
            // Can be refactored, mathematically
            int FirstX;
            if (Cards.Count % 2 == 0)
                // x = mid – (n / 2) * (g + w) + 0.5 * g
                FirstX = (this.Width / 2) - (Cards.Count / 2) * (CardBox.CARD_WIDTH + CardBox.CARD_SPACING) + (CardBox.CARD_SPACING / 2);
            else
                // mid – (n / 2) * (g + w) – 0.5 * w
                FirstX = (this.Width / 2) - (Cards.Count / 2) * (CardBox.CARD_WIDTH + CardBox.CARD_SPACING) - (CardBox.CARD_WIDTH / 2);

            int NextX = FirstX;
            foreach (CardBox c in Cards)
            {
                c.Left = NextX;
                c.Top = CoordY;
                NextX += (CardBox.CARD_WIDTH + CardBox.CARD_SPACING);
                Controls.Add(c);
            }
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
            this.UpdateUI();
            HealthLabel.Anchor = AnchorStyles.Right;
        }

        public void UpdateUI()
        {
            if (CardReferenced.Game.PlayerOne.PlayerCard == CardReferenced)
                HealthLabel.Text = CardReferenced.Game.PlayerOne.Health.ToString();
            else
                HealthLabel.Text = CardReferenced.Game.PlayerTwo.Health.ToString();
        }
    }
}
