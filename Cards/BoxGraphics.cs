using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Cards
{
    

    
   
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
