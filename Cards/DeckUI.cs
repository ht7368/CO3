using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Cards
{
    public class CardLabel : Label
    {
        public CardBuilder Card;
    }

    public partial class DeckUI : Form
    {
        public DeckBox Builder;

        public DeckUI()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            WindowState = FormWindowState.Normal;
        }

        private void DeckUI_Load(object sender, EventArgs e)
        {
            // Initialize the directory for deck files
            if (!Directory.Exists("deckfiles"))
                Directory.CreateDirectory("deckfiles");
            if (!File.Exists("deckfiles/SwarmDeck.deck"))
                File.WriteAllBytes("deckfiles/SwarmDeck.deck", DeckChoiceUI.SWARM_DECK);
            if (!File.Exists("deckfiles/ComboDeck.deck"))
                File.WriteAllBytes("deckfiles/ComboDeck.deck", DeckChoiceUI.COMBO_DECK);

            FormBorderStyle = FormBorderStyle.FixedSingle;
            Height = 700;
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
        public const int ITEM_WIDTH = 150;
        public const int ITEM_HEIGHT = 23;
        public const int ITEM_SPACING = 0;
        public const int BUTTON_HEIGHT = 50;
        public Color EMPTY_COLOR = Color.FromArgb(65, 44, 34);
        public Color BACK_COLOR = Color.FromArgb(114, 78, 61);
        public Color FORE_COLOR = Color.FromArgb(107, 71, 71);

        private int CurrentPage = 0;
        private CardBuilderBox[] Boxes;
        private CardLabel[] SelectionBoxes;
        private Button ScrollLeft;
        private Button ScrollRight;
        private Button LoadDeck;
        private Button SaveDeck;
        private Button ClearDeck;
        private Button OtherDeck;

        public DeckBox() : base()
        {

        }

        public void InitUI()
        {
            SelectionBoxes = new CardLabel[25];
            BackgroundImage = Properties.Resources.DeckArea;
            // We'll set up 8 CardBox-s that will be updated every time we scroll a page
            Boxes = new CardBuilderBox[8];
            // When laying out these, we need to calculate some values
            
            int VertSpacing = (Height - 2 * GameBox.CARD_HEIGHT - BUTTON_HEIGHT) / 4;
            int TopRowHeight = VertSpacing;
            int BotRowHeight = 2 * VertSpacing + GameBox.CARD_HEIGHT;
            int HorizSpacing = (Width - 4 * GameBox.CARD_WIDTH - ITEM_WIDTH) / 6;

            for (int i = 0; i < 8; i++)
            {
                Boxes[i] = new CardBuilderBox(this);
                var Box = Boxes[i];
                Box.Top = i < 4 ? TopRowHeight : BotRowHeight;
                int j = i % 4;
                Box.Left = (j + 1) * HorizSpacing + j * GameBox.CARD_WIDTH;
                Controls.Add(Box);
            }

            for (int i = 0; i < 25; i++)
            {
                var Temp = new CardLabel()
                {
                    BackColor = EMPTY_COLOR,
                    Image = Properties.Resources.SelectionBase,
                    Height = ITEM_HEIGHT,
                    Width = ITEM_WIDTH,
                    Top = VertSpacing + i * (ITEM_HEIGHT + ITEM_SPACING),
                    Left = Width - ITEM_WIDTH - HorizSpacing,
                    Font = GameBox.CFont.GetFont(12),
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = "",
                };
                SelectionBoxes[i] = Temp;
                Temp.Click += (_s, _e) =>
                {
                if (Temp.Card != null)
                    {
                        Temp.BackColor = EMPTY_COLOR;
                        Temp.Image = Properties.Resources.SelectionBase;
                        Temp.Card = null;
                        Temp.Text = "";
                    }
                };
                Controls.Add(Temp);
            }

            ScrollLeft = new Button()
            {
                Image = Properties.Resources.ArrowLeft,
                Size = new Size(20, 20),
                Top = (BotRowHeight + GameBox.CARD_HEIGHT + TopRowHeight) / 2 - 10,
                Left = HorizSpacing - GameBox.CONTROL_SPACING - 20,
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
                BackColor = BACK_COLOR,
            };
            ScrollLeft.Click += (_s, _e) =>
            {
                if (CurrentPage > 0)
                {
                    CurrentPage -= 1;
                    RenderPage();
                }
            };
            ScrollLeft.FlatAppearance.BorderSize = 0;
            ScrollLeft.FlatAppearance.BorderColor = BACK_COLOR;
            Controls.Add(ScrollLeft);
            ScrollRight = new Button()
            {
                Image = Properties.Resources.ArrowRight,
                Size = new Size(20, 20),
                Top = (BotRowHeight + GameBox.CARD_HEIGHT + TopRowHeight) / 2 - 10,
                Left = Boxes[3].Left + GameBox.CARD_WIDTH + GameBox.CONTROL_SPACING,
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
                BackColor = BACK_COLOR,
            };
            ScrollRight.Click += (_s, _e) =>
            {
                if ((CurrentPage + 1) * 8 < Cards.Collection.Count)
                {
                    CurrentPage += 1;
                    RenderPage();
                }
            };
            ScrollRight.FlatAppearance.BorderSize = 0;
            ScrollRight.FlatAppearance.BorderColor = BACK_COLOR;
            Controls.Add(ScrollRight);
            LoadDeck = new Button()
            {
                Height = BUTTON_HEIGHT,
                Width = GameBox.CARD_WIDTH,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = GameBox.CFont.GetFont(12),
                Image = Properties.Resources.DeckButton,
                Top = 3 * VertSpacing + 2 * GameBox.CARD_HEIGHT,
                Left = HorizSpacing,
                Text = "LOAD DECK",
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
                BackColor = BACK_COLOR,
            };
            LoadDeck.FlatAppearance.BorderSize = 0;
            LoadDeck.FlatAppearance.BorderColor = BACK_COLOR;
            LoadDeck.Click += (_s, _e) =>
            {
                var Picker = new OpenFileDialog()
                {
                    Filter = "Deck Files|*.deck|All Files|*.*",
                    InitialDirectory = $"{Environment.CurrentDirectory}\\deckfiles",
                };
                if (Picker.ShowDialog() != DialogResult.OK)
                    return;
                byte[] Bytes = File.ReadAllBytes(Picker.FileName);
                CardBuilder[] Builders = Bytes
                    .Select(b => Cards.CardFromID(b))
                    .OrderBy(c => c.ManaCostData)
                    .ToArray();
                for (int i = 0; i < SelectionBoxes.Length; i++)
                {
                    var Card = Builders[i];
                    SelectionBoxes[i].Card = Card;
                    SelectionBoxes[i].Text = Card.NameData;
                    if (Card.TypeID == CardBuilder.CardType.Minion)
                        SelectionBoxes[i].Image = Properties.Resources.SelectionMinion;
                    else if (Card.TypeID == CardBuilder.CardType.Power)
                        SelectionBoxes[i].Image = Properties.Resources.SelectionPower;
                    else if (Card.TypeID == CardBuilder.CardType.Spell)
                        SelectionBoxes[i].Image = Properties.Resources.SelectionSpell;
                }

                if (!Cards.ValidateDeck(SelectionBoxes.Select(x => x.Card), out string why))
                {
                    MessageBox.Show(caption: "Deck Error!", text: why);
                    // Clear
                    foreach (CardLabel l in SelectionBoxes)
                    {
                        l.BackColor = EMPTY_COLOR;
                        l.Image = Properties.Resources.SelectionBase;
                        l.Card = null;
                        l.Text = "";
                    }
                    return;
                }
            };
            Controls.Add(LoadDeck);
            SaveDeck = new Button()
            {
                Height = BUTTON_HEIGHT,
                Width = GameBox.CARD_WIDTH,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = GameBox.CFont.GetFont(12),
                Image = Properties.Resources.DeckButton,
                Top = 3 * VertSpacing + 2 * GameBox.CARD_HEIGHT,
                Left = 2 * HorizSpacing + GameBox.CARD_WIDTH,
                Text = "SAVE DECK",
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
                BackColor = BACK_COLOR,
            };
            SaveDeck.FlatAppearance.BorderSize = 0;
            SaveDeck.FlatAppearance.BorderColor = BACK_COLOR;
            SaveDeck.Click += (_s, _e) =>
            {
                if (!Cards.ValidateDeck(SelectionBoxes.Select(x => x.Card), out string why))
                {
                    MessageBox.Show(caption: "Deck Error!", text: why);
                    return;
                }
                byte[] Bytes = new byte[25];
                for (int i = 0; i < 25; i++)
                {
                    var Card = SelectionBoxes[i];
                    if (Card.Card == null)
                    {
                        MessageBox.Show(text: "The deck is too small and must contain 25 cards.", caption: "Deck Error!");
                        return;
                    }
                    Bytes[i] = (byte) Card.Card.DeckID;
                }
                var Picker = new SaveFileDialog()
                {
                    Filter = "Deck Files|*.deck|All Files|*.*",
                    InitialDirectory = $"{Environment.CurrentDirectory}\\deckfiles",
                };
                Picker.ShowDialog();
                if (Picker.FileName == "")
                    return;
                File.WriteAllBytes(Picker.FileName, Bytes);
            };
            Controls.Add(SaveDeck);
            OtherDeck = new Button()
            {
                Height = BUTTON_HEIGHT,
                Width = GameBox.CARD_WIDTH,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = GameBox.CFont.GetFont(12),
                Image = Properties.Resources.DeckButton,
                Top = 3 * VertSpacing + 2 * GameBox.CARD_HEIGHT,
                Left = 3 * HorizSpacing + 2 * GameBox.CARD_WIDTH,
                Text = "EXIT WINDOW",
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
                BackColor = BACK_COLOR,
            };
            OtherDeck.FlatAppearance.BorderSize = 0;
            OtherDeck.FlatAppearance.BorderColor = BACK_COLOR;
            OtherDeck.Click += (_s, _e) =>
            {
                (Parent as Form).Close();
            };
            Controls.Add(OtherDeck);
            ClearDeck = new Button()
            {
                Height = BUTTON_HEIGHT,
                Width = GameBox.CARD_WIDTH,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = GameBox.CFont.GetFont(12),
                Image = Properties.Resources.DeckButton,
                Top = 3 * VertSpacing + 2 * GameBox.CARD_HEIGHT,
                Left = 4 * HorizSpacing + 3 * GameBox.CARD_WIDTH,
                Text = "CLEAR DECK",
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
                BackColor = BACK_COLOR,
            };
            ClearDeck.FlatAppearance.BorderSize = 0;
            ClearDeck.FlatAppearance.BorderColor = BACK_COLOR;
            ClearDeck.Click += (_s, _e) =>
            {
                foreach (CardLabel l in SelectionBoxes)
                {
                    l.BackColor = EMPTY_COLOR;
                    l.Image = Properties.Resources.SelectionBase;
                    l.Card = null;
                    l.Text = "";
                }
            };
            Controls.Add(ClearDeck);

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
                    for (int j = i; j < 8; j++)
                        Boxes[j].Visible = false;
                    break;
                }
                var Box = Cards.Collection[StartInd + i];
                Boxes[i].Visible = true;
                Boxes[i].RenderCard(Box);
            }
        }

        public void AddSelected(CardBuilder card)
        {
            // We want to add from the bottom-up
            // So we find the first empty space
            foreach (CardLabel l in SelectionBoxes)
            {
                if (l.Card == null)
                {
                    l.Text = card.NameData;
                    l.BackColor = Color.FromArgb(231, 89, 82);
                    l.Card = card;
                    if (card.TypeID == CardBuilder.CardType.Minion)
                        l.Image = Properties.Resources.SelectionMinion;
                    else if (card.TypeID == CardBuilder.CardType.Power)
                        l.Image = Properties.Resources.SelectionPower;
                    else if (card.TypeID == CardBuilder.CardType.Spell)
                        l.Image = Properties.Resources.SelectionSpell;
                    break;
                }
            }
        }
    }
}