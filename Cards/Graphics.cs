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
        private PictureBox CardBase;
        private PictureBox CardArt;
        private Label CardName;
        private TextBox CardInfo;

        public CardBox(BaseCard card)
        {
            BackColor = Color.Transparent;
            Size = new Size(50, 100);
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
                Size = new Size(40, 40),
                Location = new Point(5, 5),
                // Image = ...,
                BackColor = Color.Transparent,
            };
            CardName = new Label()
            {
                Text = card.Name,
                Location = new Point(5, 50),
                Size = new Size(40, 5),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
            };
            CardInfo = new TextBox()
            {
                TextAlign = HorizontalAlignment.Center,
                Multiline = true,
                Location = new Point(5, 60),
                Size = new Size(40, 40),
                Lines = new string[] { card.Description },
            };

            Controls.Add(CardBase);
            Controls.Add(CardArt);
            Controls.Add(CardName);
            Controls.Add(CardInfo);

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
