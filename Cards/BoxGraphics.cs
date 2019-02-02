using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Cards
{




    public class CardGroupBox<T> : Panel where T : BaseCard
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
            DisplayedCards = new List<CardBox>();
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

        public IEnumerable<CardBox> GetCards()
        {
            foreach (CardBox cb in DisplayedCards)
                yield return cb;
            yield break;
        }
    }
}

  