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
        public void MinionPlayTest()
        {
            GameState G = new GameState(DeckChoiceUI.COMBO_DECK, DeckChoiceUI.COMBO_DECK, true, 0);
            G.CurrentPower = new PowerCard(G);
            // Create two minions, 1/4 and 2/5
            // They will end up 1/2 and 2/4 after battle
            MinionCard First = new MinionCard(G)
            {
                OnBoard = true,
                CanAttack = true,
                Attack = 1,
                Health = 4,
                Owner = G.PlayerOne,
            };
            MinionCard Second = new MinionCard(G)
            {
                OnBoard = true,
                CanAttack = true,
                Attack = 2,
                Health = 5,
                Owner = G.PlayerTwo,
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

        [TestMethod()]
        public void MinionEffectTest()
        {
            GameState G = new GameState(DeckChoiceUI.COMBO_DECK, DeckChoiceUI.COMBO_DECK, true, 0);

            // Test: Can place a minion on board
            G.PlayerOne.Mana = 10;
            MinionCard NewCard = Cards.CardFromID(18).Build(G, G.PlayerOne) as MinionCard;
            G.PlayerOne.Hand.Add(NewCard);
            G.ProcessMove(new Move(NewCard.Id, 0));
            Assert.IsTrue(NewCard.OnBoard);

            // Test: Ensure "Insect Swarm" played was correct
            Assert.IsTrue(G.PlayerOne.Board.Count == 3);
        }
    }
}