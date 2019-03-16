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
    public class BasePlayerTests
    {
        [TestMethod()]
        public void DrawCardTest()
        {
            GameState G = new GameState(DeckChoiceUI.COMBO_DECK, DeckChoiceUI.COMBO_DECK, true, 0);
            // 5 cards are drew at the start of the game
            // The deck can be emptied by drawing 20 more
            for (int i = 0; i < 20; i++)
                G.PlayerOne.DrawCard();
            Assert.IsTrue(G.PlayerOne.Health > 0);
            // Deck is now empty
            G.PlayerOne.DrawCard();
            Assert.IsFalse(G.PlayerOne.Health > 0);
        }

        [TestMethod()]
        public void DrawCardHandTest()
        {
            GameState G = new GameState(DeckChoiceUI.COMBO_DECK, DeckChoiceUI.COMBO_DECK, true, 0);
            Assert.AreEqual(20, G.PlayerOne.Deck.Count);
            Assert.AreEqual(20, G.PlayerTwo.Deck.Count);
            for (int i = 0; i < 10; i++)
                G.PlayerOne.DrawCard();
            Assert.IsTrue(G.PlayerOne.Hand.Count == 10);
        }
    }
}