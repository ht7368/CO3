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
        const int ITEM_WIDTH = 150;
        const int ITEM_HEIGHT = 40;
        const int ITEM_SPACING = 5;

        private int CurrentPage = 0;
        private CardBuilderBox[] Boxes;
        private List<Label> SelectedCards;
        private Button ScrollLeft;
        private Button ScrollRight;

        public DeckBox() : base()
        {

        }

        public void InitUI()
        {
            SelectedCards = new List<Label>();
            BackgroundImage = Properties.Resources.DeckArea;
            // We'll set up 8 CardBox-s that will be updated every time we scroll a page
            Boxes = new CardBuilderBox[8];
            // When laying out these, we need to calculate some values
            int VertSpacing = (Height - 2 * GameBox.CARD_HEIGHT - 50) / 4;
            int TopRowHeight = VertSpacing;
            int BotRowHeight = 2 * VertSpacing + GameBox.CARD_HEIGHT;
            int HorizSpacing = (Width - 5 * GameBox.CARD_WIDTH) / 5;
            for (int i = 0; i < 8; i++)
            {
                Boxes[i] = new CardBuilderBox(this);
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
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
            };
            ScrollRight = new Button()
            {
                Image = Properties.Resources.ArrowRight,
                Size = new Size(20, 20),
                Top = (BotRowHeight + GameBox.CARD_HEIGHT + TopRowHeight) / 2 - 10,
                Left = Boxes[3].Left + GameBox.CARD_WIDTH + GameBox.CONTROL_SPACING,
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
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

        public void AddSelected(CardBuilder form)
        {
            int VertPosition;
            if (SelectedCards.Count == 0)
                VertPosition = GameBox.CONTROL_SPACING;
            else
                VertPosition = SelectedCards[SelectedCards.Count - 1].Bottom + ITEM_SPACING;

            var Temp = new Label()
            {
                Top = VertPosition,
                Left = Width - GameBox.CONTROL_SPACING - ITEM_WIDTH,
                Text = form.NameData,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = ITEM_HEIGHT,
                Width = ITEM_WIDTH,
            };
            SelectedCards.Add(Temp);
            Controls.Add(Temp);

            Temp.Click += (_s, _e) =>
            {
                // Need to shift cards downward that are above this one to avoid gaps
                int Ind = SelectedCards.IndexOf(Temp);
                // We can do i+1 since we don't need to modify what we are removing
                for (int i = Ind + 1; i < SelectedCards.Count; i++)
                    SelectedCards[i].Top -= ITEM_HEIGHT + ITEM_SPACING;
                SelectedCards.Remove(Temp);
                Controls.Remove(Temp);
            };
        }
    }
}
