using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Cards
{
    public class CardBox : System.Windows.Forms.GroupBox
    {
        private PictureBox CardBase;
        private PictureBox CardArt;
        private Label CardName;
        private TextBox CardInfo;

        public CardBox(BaseCard card) : base()
        {
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
                    ToggleClick();
                };
        }

        public void ToggleClick()
        {
            if (CardBase.BackColor == Color.Transparent)
                CardBase.BackColor = Color.Red;
            else
                CardBase.BackColor = Color.Transparent;
        }
    }
}
