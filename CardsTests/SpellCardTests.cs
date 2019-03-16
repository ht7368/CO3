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
    public class SpellCardTests
    {
        [TestMethod()]
        public void SpellPlayTest()
        {
            GameState G = new GameState(DeckChoiceUI.COMBO_DECK, DeckChoiceUI.COMBO_DECK, true, 0);

            SpellCard LesserBolt = Cards.CardFromID(13).Build(G, G.PlayerOne) as SpellCard;
            G.PlayerOne.Hand.Add(LesserBolt);
            G.PlayerOne.Mana = 13;
            G.ProcessMove(new Move(LesserBolt.Id, G.PlayerTwo.PlayerCard.Id));
            Assert.AreEqual(25 - 4, G.PlayerTwo.Health);


            SpellCard Innovation = Cards.CardFromID(29).Build(G, G.PlayerOne) as SpellCard;
            G.PlayerOne.Hand.Add(Innovation);
            G.PlayerOne.Mana = 13;
            G.ProcessMove(new Move(Innovation.Id, 0));
            Assert.AreEqual(13, G.PlayerOne.Mana);
        }
    }
}