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
    public class PowerCardTests
    {
        [TestMethod()]
        public void PowerPlayTest()
        {
            GameState G = new GameState(DeckChoiceUI.COMBO_DECK, DeckChoiceUI.COMBO_DECK, true, 0);

            PowerCard InsectHive = Cards.CardFromID(21).Build(G, G.PlayerOne) as PowerCard;
            G.CurrentPower = InsectHive;

            G.SwitchTurns();
            Assert.IsTrue(G.PlayerOne.Board.Count == 1);
            Assert.IsTrue(G.PlayerOne.Board[0].Name == "INSECT SWARM");
            Assert.IsTrue(G.PlayerTwo.Board.Count == 0);

            G.SwitchTurns();
            Assert.IsTrue(G.PlayerTwo.Board.Count > 0);
            Assert.IsTrue(G.PlayerTwo.Board[0].Name == "INSECT SWARM");

            PowerCard Arena = Cards.CardFromID(10).Build(G, G.PlayerOne) as PowerCard;
            G.CurrentPower = Arena;
            G.ProcessMove(new Move(G.PlayerOne.Board[0].Id, G.PlayerTwo.PlayerCard.Id));
            Assert.AreEqual(4, G.PlayerOne.Board[0].Attack);
        }
    }
}