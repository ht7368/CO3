using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Cards
{
    public class CardBuilderBox : Panel
    {
        public Color CARD_COLOR = Color.FromArgb(255, 107, 71, 71);

        public CardBuilder CardReferenced;
        private DeckBox Form;
        private PictureBox CardArt;
        private Label CardName;
        private TextBox CardInfo;
        private Label CardAttack;
        private Label CardHealth;
        private Label CardCost;

        public CardBuilderBox(DeckBox form)
        {
            Form = form;

            Size = new Size(GameBox.CARD_WIDTH, GameBox.CARD_HEIGHT);
            BackgroundImage = Properties.Resources.CardBody;

            CardArt = new PictureBox()
            {
                Size = new Size(74, 74),
                Location = new Point(13, 13),
            };
            CardName = new Label()
            {
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
                BackColor = CARD_COLOR,
                BorderStyle = BorderStyle.None,
                ForeColor = Color.White,
                Cursor = Cursors.Arrow,
                Font = GameBox.CFont.GetFont(12),
            };
            CardCost = new Label()
            {
                Size = new Size(22, 22),
                Top = GameBox.CARD_HEIGHT - 22 - 4,
                Left = 4,
                Font = GameBox.CFont.GetFont(12),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Image = Properties.Resources.ManaBox,
            };
            CardAttack = new Label()
            {
                Size = new Size(22, 22),
                Top = GameBox.CARD_HEIGHT - 22 - 4,
                Left = GameBox.CARD_WIDTH - 4 - 22 - 22,
                Font = GameBox.CFont.GetFont(12),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Image = Properties.Resources.AttackBox,
            };

            CardHealth = new Label()
            {
                Size = new Size(22, 22),
                Top = GameBox.CARD_HEIGHT - 22 - 4,
                Left = GameBox.CARD_WIDTH - 4 - 22,
                Font = GameBox.CFont.GetFont(12),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Image = Properties.Resources.HealthBox,
            };
            Controls.Add(CardArt);
            Controls.Add(CardInfo);
            Controls.Add(CardName);
            Controls.Add(CardCost);
            CardCost.BringToFront();
            Controls.Add(CardHealth);
            CardHealth.BringToFront();
            Controls.Add(CardAttack);
            CardAttack.BringToFront();

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

        public void CardClicked(CardBuilderBox cardBox)
        {
            Form.AddSelected(cardBox.CardReferenced);
        }

        public void RenderCard(CardBuilder card)
        {
            CardReferenced = card;

            CardArt.Image = card.ArtData;
            CardInfo.Text = card.DescriptionData;
            CardName.Text = card.NameData;
            CardCost.Text = card.ManaCostData.ToString();
            if (card.TypeID == CardBuilder.CardType.Minion)
            {
                CardAttack.Text = card.AttackData.ToString();
                CardAttack.Visible = true;
                CardHealth.Text = card.HealthData.ToString();
                CardHealth.Visible = true;
            }
            else
            {
                CardAttack.Visible = false;
                CardHealth.Visible = false;
            }
        }
    }

}
