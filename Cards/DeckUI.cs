using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cards
{
    public partial class DeckUI : Form
    {
        public DeckBox Builder;

        public DeckUI()
        {
            InitializeComponent();
        }

        private void DeckUI_Load(object sender, EventArgs e)
        {
            //FormBorderStyle = FormBorderStyle.FixedSingle;
            Height = 600;
            Width = 1000;
            Builder = new DeckBox()
            {
                Location = new Point(0, 0),
                Width = Width,
                Height = Height,
            };
            Builder.InitUI();
            Controls.Add(Builder);
            // WinForms dimensions aren't accurate, so this helps
            Height += 39;
            Width += 16;
        }
    }

    public class DeckBox : Panel
    {
        private ListBox CardsPicked;
        private int CurrentPage = 0;
        public CardBuilderBox[] Boxes;
        private Button ScrollLeft;
        private Button ScrollRight;

        public DeckBox() : base()
        {

        }

        public void InitUI()
        {
            BackgroundImage = Properties.Resources.DeckArea;

            CardsPicked = new ListBox()
            {
                Width = GameBox.CARD_WIDTH,
                Height = Height - 2 * GameBox.CARD_SPACING,
            };
            CardsPicked.Left = Width - CardsPicked.Width - GameBox.CARD_SPACING;
            CardsPicked.Top = GameBox.CARD_SPACING;
            Controls.Add(CardsPicked);

            // We'll set up 8 CardBox-s that will be updated every time we scroll a page
            Boxes = new CardBuilderBox[8];
            // When laying out these, we need to calculate some values
            int VertSpacing = (Height - 2 * GameBox.CARD_HEIGHT - 50) / 4;
            int TopRowHeight = VertSpacing;
            int BotRowHeight = 2 * VertSpacing + GameBox.CARD_HEIGHT;
            int HorizSpacing = (Width - CardsPicked.Width - 4 * GameBox.CARD_WIDTH) / 5;
            for (int i = 0; i < 8; i++)
            {
                Boxes[i] = new CardBuilderBox();
                var Box = Boxes[i];
                Box.Top = i < 4 ? TopRowHeight : BotRowHeight;
                int j = i % 4;
                Box.Left = (j + 1) * HorizSpacing + j * GameBox.CARD_WIDTH;
                Controls.Add(Box);
            }

            ScrollLeft = new Button()
            {
                Image = Properties.Resources.ArrowLeft,
                Size = new Size(20, 20),
                Top = (BotRowHeight + GameBox.CARD_HEIGHT + TopRowHeight) / 2 - 10,
                Left = HorizSpacing - GameBox.CONTROL_SPACING - 20,
            };
            ScrollRight = new Button()
            {
                Image = Properties.Resources.ArrowRight,
                Size = new Size(20, 20),
                Top = (BotRowHeight + GameBox.CARD_HEIGHT + TopRowHeight) / 2 - 10,
                Left = Boxes[3].Left + GameBox.CARD_WIDTH + GameBox.CONTROL_SPACING,
            };
            ScrollLeft.Click += (_s, _e) =>
            {
                if (CurrentPage > 0)
                {
                    CurrentPage -= 1;
                    RenderPage();
                }
            };
            ScrollRight.Click += (_s, _e) =>
            {
                if ((CurrentPage + 1) * 8 < Cards.Collection.Count)
                {
                    CurrentPage += 1;
                    RenderPage();
                }
            };
            Controls.Add(ScrollLeft);
            Controls.Add(ScrollRight);

            RenderPage();
        }
        public void RenderPage()
        {
            int StartInd = 8 * CurrentPage;
            for (int i = 0; i < 8; i++)
            {
                // We won't render the last cards
                if (StartInd + i >= Cards.Collection.Count)
                {
                    Boxes[i].Visible = false;
                    break;
                }
                var Box = Cards.Collection[StartInd + i];
                Boxes[i].Visible = true;
                Boxes[i].RenderCard(Box);
            }
        }
    }
}
