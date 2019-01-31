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
            byte[] Bytes = File.ReadAllBytes(Picker.FileName);
            Deckcode = Bytes;
            Close();
        }
    }
}
