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
    public class MinionCardTests
    {
        [TestMethod()]
        public void PlayTest()
        {
            GameState G = new GameState();
            G.CurrentPower = new PowerCard(G);
            // Create two minions, 1/4 and 2/5
            // They will end up 1/2 and 2/4 after battle
            MinionCard First = new MinionCard(G)
            {
                OnBoard = true,
                CanAttack = true,
                Attack = 1,
                Health = 4,
            };
            MinionCard Second = new MinionCard(G)
            {
                OnBoard = true,
                CanAttack = true,
                Attack = 2,
                Health = 5,
            };
            G.PlayerOne.Board.Add(First);
            G.PlayerTwo.Board.Add(Second);

            // Tests
            Assert.IsTrue(First.IsPlayable(new Move(First.Id, Second.Id)));
            G.ProcessMove(new Move(First.Id, Second.Id));
            Assert.AreEqual(First.Attack, 1);
            Assert.AreEqual(First.Health, 2);
            Assert.AreEqual(Second.Attack, 2);
            Assert.AreEqual(Second.Health, 4);
        }
    }
}