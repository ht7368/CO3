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
    public class BaseCardTests
    {
        [TestMethod()]
        public void BaseCardTest()
        {
            GameState G = new GameState();
            // Creates instances for testing
            var First = new MinionCard(G)
            {
                Name = "First Test",
            };

            var Second = new PowerCard(G)
            {
                Name = "Second Test",
            };
            var Third = new SpellCard(G)
            {
                Name = "Third Test",
            };
            // Assert that not Ids are equal to 0
            Assert.AreNotEqual<uint>(First.Id, 0);
            Assert.AreNotEqual<uint>(Second.Id, 0);
            Assert.AreNotEqual<uint>(Third.Id, 0);
        }

        [TestMethod()]
        public void IsPlayableTest()
        {
            GameState G = new GameState();
            // Create a few cards
            var First = new SpellCard(G)
            {
                ManaCost = 0,
                IsTargeted = true,
            };
            var Second = new MinionCard(G)
            {
                Health = 1,
                ManaCost = 0,
                OnBoard = true,
                CanAttack = true,
            };
            G.PlayerOne.Board.Add(Second);
            G.PlayerOne.Hand.Add(First);
            // This should NOT be playable - invalid target
            Assert.IsFalse(First.IsPlayable(new Move(First.Id, 0)));
            // This should NOT be playable - invalid target
            Assert.IsFalse(Second.IsPlayable(new Move(Second.Id, G.PlayerOne.PlayerCard.Id)));
            // But this should be
            Assert.IsTrue(Second.IsPlayable(new Move(Second.Id, G.PlayerTwo.PlayerCard.Id)));
            // Finally, this should be
            Assert.IsTrue(First.IsPlayable(new Move(First.Id, Second.Id)));
        }
    }
}