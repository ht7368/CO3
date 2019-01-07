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
    public class GameStateTests
    {
        [TestMethod()]
        public void SwitchTurnsTest()
        {
            GameState Game = new GameState();
            Assert.IsTrue(Game.IsP1Turn);
            Assert.AreEqual(Game.PlayerOne.Mana, 1);

            for (int i = 2; i < 6; i++)
            {
                // Reduce to zero to check amount being gained
                Game.PlayerOne.Mana = 0;
                // Pass twice, returning to player 1's turn
                Game.SwitchTurns();
                Game.SwitchTurns();
                Assert.AreEqual(Game.PlayerOne.Mana, i);
            }
        }
    }
}