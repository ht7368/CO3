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

        private BaseCard CardReferenced;

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

            foreach (Control c in Controls)
                c.Click += (_s, _e) =>
                {
                    CardClicked();
                };

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

        public void CardClicked()
        {
            var Processor = (Parent.Parent as GameBox).Processor;

            if (Processor.NumberOfMoves() > 1)
            {
                Move Play = Processor.ProcessMoves();
                Processor.Clear();
                (Parent.Parent as GameBox).Game.ProcessMove(Play);
                return;
            }
                

            Processor.AddUserAction(CardReferenced);
            if (Processor.NumberOfMoves() == 1)
                this.BackColor = Color.Blue;
            else
                this.BackColor = Color.Red;
        }
    }

    /*
     * 
        private void PlayButton_Click(object sender, EventArgs e)
        {
            Move Play = Processor.ProcessMoves();
            Processor.Clear();
            Game.ProcessMove(Play);

            RenderState(Game);
        }*/

    public class PowerBox : Panel
    {
        PowerCard PowerCard;

        public PowerBox(PowerCard power)
        {
            PowerCard = power;
            Height = CardBox.CARD_HEIGHT;
            Width = CardBox.CARD_WIDTH;
            Update();
        }

        public void UpdateUI()
        {
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
}
