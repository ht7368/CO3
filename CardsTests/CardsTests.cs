using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Tests
{
    [TestClass()]
    public class CardsTests
    {
        [TestMethod()]
        public void ValidateDeckTest()
        {
            // Should fail: Null value
            CardBuilder[] Zeroth = DeckChoiceUI
                .SWARM_DECK
                .Select(n => Cards.CardFromID(n))
                .ToArray();
            Zeroth[0] = null;
            Assert.IsFalse(Cards.ValidateDeck(Zeroth, out string zero));
            // Should fail: Too many duplicates
            CardBuilder[] First = Enumerable
                .Repeat(1, 25)
                .Select(n => Cards.CardFromID((byte)n))
                .ToArray();
            Assert.IsFalse(Cards.ValidateDeck(First, out string one));
            // Should fail: Too few cards
            CardBuilder[] Second = new CardBuilder[]
            {
                Cards.CardFromID(1),
                Cards.CardFromID(2),
            };
            Assert.IsFalse(Cards.ValidateDeck(Second, out string two));
            // Should fail: Too many cards
            CardBuilder[] Third = Enumerable
                .Range(0, 30)
                .Select(n => Cards.CardFromID((byte)n))
                .ToArray();
            Assert.IsFalse(Cards.ValidateDeck(Third, out string three));
        }

        [TestMethod()]
        public void DefaultDeckValidationTest()
        {
            CardBuilder[] First = DeckChoiceUI
                .SWARM_DECK
                .Select(n => Cards.CardFromID(n))
                .ToArray();
            CardBuilder[] Second = DeckChoiceUI
                .COMBO_DECK
                .Select(n => Cards.CardFromID(n))
                .ToArray();
            Assert.IsTrue(Cards.ValidateDeck(First, out string one));
            Assert.IsTrue(Cards.ValidateDeck(Second, out string two));
        }

        [TestMethod()]
        public void UniqueIDTest()
        {
            Dictionary<int, int> Count = new Dictionary<int, int>();
            foreach (CardBuilder cb in Cards.Collection)
                if (Count.ContainsKey(cb.DeckID))
                    Assert.Fail();
                else
                    Count.Add(cb.DeckID, 0);
        }

        [TestMethod()]
        public void UniqueArtTest()
        {
            foreach (CardBuilder cb in Cards.Collection)
                Assert.IsNotNull(cb.ArtData);
        }

        [TestMethod()]
        public void MinimumLengthTest()
        {
            Assert.IsTrue(Cards.Collection.Count >= 30);
        }
    }
}