using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Cards
{
    public class CardBox : Panel
    {
        public Color CARD_COLOR = Color.FromArgb(255, 107, 71, 71);
        public Color GREEN_IND = Color.FromArgb(255, 68, 197, 91);
        public Color RED_IND = Color.FromArgb(255, 231, 89, 82);

        public BaseCard CardReferenced;

        // Visual control elements
        private PictureBox CardArt;
        private Label CardName;
        private TextBox CardInfo;
        private Label CardAttack;
        private Label CardHealth;
        private Label CardCost;
        private Control CardPlayableIndicator;
        private PictureBox CardHidden;

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
                Font = GameBox.CFont.GetFont(9),
            };
            CardCost = new Label()
            {
                Text = CardReferenced.ManaCost.ToString(),
                Size = new Size(22, 22),
                Top = GameBox.CARD_HEIGHT - 22 - 4,
                Left = 4,
                Font = GameBox.CFont.GetFont(12),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Image = Properties.Resources.ManaBox,
            };
            CardHidden = new PictureBox()
            {
                Image = Properties.Resources.CardFlipped,
                Visible = CardReferenced.Owner == CardReferenced.Game.PlayerTwo
                    && (!(CardReferenced is MinionCard m) || !m.OnBoard)
                    && CardReferenced.Game.CurrentPower as BaseCard != CardReferenced,
                Left = 0,
                Top = 0,
                Height = GameBox.CARD_HEIGHT,
                Width = GameBox.CARD_WIDTH,
            };
            Controls.Add(CardHidden);
            CardPlayableIndicator = new Control()
            {
                Size = new Size(6, 6),
                Top = GameBox.CARD_HEIGHT - 6 - 12,
                // Centred between CardCost and CardHealth
                Left = 36,
                ForeColor = Color.White,
                BackColor = card.ManaCost <= card.Game.PlayerOne.Mana && card.Game.IsP1Turn ? GREEN_IND : RED_IND,
            };
            Controls.Add(CardPlayableIndicator);
            CardPlayableIndicator.BringToFront();

            Controls.Add(CardArt);
            Controls.Add(CardInfo);
            Controls.Add(CardName);
            Controls.Add(CardCost);

            CardCost.BringToFront();

            // Different visual effects for minion cards
            if (card is MinionCard minion)
            {
                CardAttack = new Label()
                {
                    Text = minion.Attack.ToString(),
                    Size = new Size(22, 22),
                    Top = GameBox.CARD_HEIGHT - 22 - 4,
                    Left = GameBox.CARD_WIDTH - 4 - 22 - 22,
                    Font = GameBox.CFont.GetFont(12),
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
                    Font = GameBox.CFont.GetFont(12),
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Image = Properties.Resources.HealthBox,
                };
                Controls.Add(CardHealth);
                CardHealth.BringToFront();
                if (minion.OnBoard)
                    CardPlayableIndicator.BackColor = minion.CanAttack ? GREEN_IND : RED_IND;
                if (minion.Owner == minion.Game.PlayerTwo)
                    CardPlayableIndicator.Visible = false;
                //CardAttack.Text = (CardReferenced as MinionCard).Attack.ToString();
            }

            CardHidden.BringToFront();

            // Ensures clicking anywhere on the card counts as a click
            foreach (Control c in Controls)
                c.Click += (_s, _e) =>
                {
                    CardClicked(this);
                };
            this.Click += (_s, _e) =>
            {
                CardClicked(this);
            };
        }

        public void SetPlayabilityVisibility(bool b)
        {
            CardPlayableIndicator.Visible = b;
        }

        public void SetHide(bool b)
        {
            CardHidden.Visible = b;
        }

        // onClick for card elements
        public void CardClicked(CardBox card)
        {
            GameState Game = card.CardReferenced.Game;

            if (card.CardReferenced is HeroCard || card.CardReferenced.Game.CurrentPower == card.CardReferenced || !card.CardReferenced.Game.IsP1Turn)
                return;
            // Get the game from the top-level gamebox by traversing up until we find the gamebox
            Control GameB = this;
            while (!(GameB is GameBox))
                GameB = GameB.Parent;
            GameBox GameUI = GameB as GameBox;

            GameUI.HideNotif();

            if (GameUI.SelectedCard == null && (!card.CardPlayableIndicator.Visible || card.CardPlayableIndicator.BackColor == card.RED_IND || card.CardReferenced.Owner == Game.PlayerTwo))
                return;

            // If nothing is selected yet, we set the selected card
            if (GameUI.SelectedCard == null)
            {
                GameUI.SelectedCard = CardReferenced;
                BackgroundImage = Properties.Resources.SelectedCardBody;
                return;
            }
            // Otherwise the move is completed and processed
            GameUI.Game.ProcessMove(new Move(GameUI.SelectedCard.Id, CardReferenced.Id));
            GameUI.SelectedCard = null;
            GameUI.RenderState(GameUI.Game);
        }

        public void UpdateUI()
        {
            // Reset the background image, in case it is highlighted
            BackgroundImage = Properties.Resources.CardBody;
            if (CardReferenced is MinionCard minion)
            {
                if (minion.OnBoard)
                    CardPlayableIndicator.BackColor = minion.CanAttack ? GREEN_IND : RED_IND;
                else
                    CardPlayableIndicator.BackColor = CardReferenced.ManaCost <= CardReferenced.Game.PlayerOne.Mana ? GREEN_IND : RED_IND;
                CardAttack.Text = minion.Attack.ToString();
                CardHealth.Text = minion.Health.ToString();
            }
            else
            {
                CardPlayableIndicator.BackColor = CardReferenced.ManaCost <= CardReferenced.Game.PlayerOne.Mana ? GREEN_IND : RED_IND;
            }
            // If it's not player one's turn (P2 has hidden indicators so we don't care what happens to them)
            if (CardReferenced.Game.InactivePlayer == CardReferenced.Game.PlayerOne)
                CardPlayableIndicator.BackColor = RED_IND;
            CardHidden.Visible = CardReferenced.Owner == CardReferenced.Game.PlayerTwo
                    && (!(CardReferenced is MinionCard m) || !m.OnBoard)
                    && CardReferenced.Game.CurrentPower != CardReferenced;
            CardHidden.BringToFront();
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
            Visual.SetPlayabilityVisibility(false);
            Controls.Remove(Visual);
            Controls.Add(Visual);
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
                Box.Game.ProcessMove(new Move(Box.SelectedCard.Id, card.Game.PlayerTwo.PlayerCard.Id));
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
            if (CardReferenced.Owner == CardReferenced.Game.PlayerOne)
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
            HealthLabel.Text = CardReferenced.Owner.Health.ToString();
        }
    }
}
