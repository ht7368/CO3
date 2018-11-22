using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Cards
{
    public class CardBox : GroupBox
    {
        public const int CARD_WIDTH = 100;
        public const int CARD_HEIGHT = 200;
        public const int CARD_SPACING = 5;

        private BaseCard CardReferenced;

        private PictureBox CardBase;
        private PictureBox CardArt;
        private Label CardName;
        private TextBox CardInfo;

        public CardBox(BaseCard card) : base()
        {
            CardReferenced = card;

            BackColor = Color.Transparent;
            Size = new Size(100, 200);
            FlatStyle = FlatStyle.Flat;

            CardBase = new PictureBox()
            {
                Size = Size,
                Location = new Point(0, 0),
                // Image = ...,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
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
                BackColor = Color.Transparent,
            };
            CardInfo = new TextBox()
            {
                TextAlign = HorizontalAlignment.Center,
                Multiline = true,
                Location = new Point(5, 130),
                Size = new Size(90, 65),
                Lines = new string[] { card.Description },
            };

            Controls.Add(CardArt);
            Controls.Add(CardInfo);
            Controls.Add(CardName);
            Controls.Add(CardBase);

            foreach (Control c in Controls)
                c.Click += (_s, _e) =>
                {
                    CardClicked();
                };
        }

        public void CardClicked()
        {
            switch (Parent)
            {
                case CardGroupBox g:
                    (g.Parent as GameBox).Processor.AddUserAction(CardReferenced, MoveProcessor.PlayArea.Board);
                    break;

                default:
                    throw new Exception();
            }
        }
    }

    public class PowerBox : GroupBox
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

    public class CardGroupBox : GroupBox
    {
        // A List<BaseCard> (hand or board) is passed in and tracked
        // This is a reference to the same list, so that works fine
        private List<BaseCard> TrackedCards;

        public CardGroupBox(List<BaseCard> cards)
        {
            TrackedCards = cards;

            this.FlatStyle = FlatStyle.Popup;
        }

        public void UpdateCards()
        {
            // Firstly, create new CardBox -s from the tracked cards.
            List<CardBox> Cards = TrackedCards.Select(c => new CardBox(c)).ToList();

            this.Height = 2 * CardBox.CARD_SPACING + CardBox.CARD_HEIGHT;

            this.Controls.Clear();
            // Can be refactored, mathematically
            int FirstX;
            if (Cards.Count % 2 == 0)
                // x = mid – (n / 2) * (g + w) + 0.5 * g
                FirstX = (this.Width / 2) - (Cards.Count / 2) * (CardBox.CARD_WIDTH + CardBox.CARD_SPACING) + (CardBox.CARD_SPACING / 2);
            else
                // mid – 0.5 * w – (floor(n / 2) * (g + w)  
                FirstX = (this.Width / 2) - (Cards.Count / 2) * (CardBox.CARD_WIDTH + CardBox.CARD_SPACING) - (CardBox.CARD_SPACING / 2);

            int NextX = FirstX;
            foreach (CardBox c in Cards)
            {
                c.Left = NextX;
                c.Top = CardBox.CARD_SPACING;
                NextX += (CardBox.CARD_WIDTH + CardBox.CARD_SPACING);
                Controls.Add(c);
            }
        }
    }
}
