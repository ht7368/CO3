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
    public partial class DeckChoiceUI : Form
    {
        public byte[] Deckcode;

        // Default decks as byte values
        // TODO: Move into the resources folder?

        public static byte[] SWARM_DECK = new byte[]
        {
             22, 22, 21, 21, 16, 16, 18, 18, 17, 17, 11, 11, 19, 19, 24, 24, 10, 10, 20, 20, 29, 31, 14, 14, 6
        };
        public static byte[] COMBO_DECK = new byte[]
        {
            29, 29, 32, 32, 28, 12, 27, 27, 9, 9, 15, 14, 14, 25, 25, 30, 30, 24, 24, 31, 31, 33, 33, 17, 13
        };

        public DeckChoiceUI()
        {
            InitializeComponent();
        }

        private void buttonCustom_Click(object sender, EventArgs e)
        {
            var Picker = new OpenFileDialog()
            {
                Filter = "Deck Files|*.deck|All Files|*.*",
                InitialDirectory = $"{Environment.CurrentDirectory}\\deckfiles",
            };
            if (Picker.ShowDialog() != DialogResult.OK)
                return;
            Pick(Picker.FileName);
        }

        private void buttonCombo_Click(object sender, EventArgs e)
        {
            Pick("deckfiles/ComboDeck.deck");
        }

        private void buttonSwarm_Click(object sender, EventArgs e)
        {
            Pick("deckfiles/SwarmDeck.deck");
        }

        private void Pick(string location)
        {
            byte[] Bytes = File.ReadAllBytes(location);
            Deckcode = Bytes;
            Close();
        }

        // To create the default decks in the deckfiles directory before choosing
        // So that they are not invalid - not found
        private void DeckChoiceUI_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("deckfiles"))
                Directory.CreateDirectory("deckfiles");
            if (!File.Exists("deckfiles/SwarmDeck.deck"))
                File.WriteAllBytes("deckfiles/SwarmDeck.deck", SWARM_DECK);
            if (!File.Exists("deckfiles/ComboDeck.deck"))
                File.WriteAllBytes("deckfiles/ComboDeck.deck", COMBO_DECK);
        }
    }
}
