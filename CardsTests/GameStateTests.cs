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
        public void SwitchTurnsTestMana()
        {
            GameState Game = new GameState(DeckChoiceUI.COMBO_DECK, DeckChoiceUI.COMBO_DECK, true, 0);
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

        [TestMethod()]
        public void SwitchTurnsTest()
        {
            GameState G = new GameState(DeckChoiceUI.COMBO_DECK, DeckChoiceUI.COMBO_DECK, true, 0);
            for (int i = 0; i < 5; i++)
            {
                BasePlayer CorrectPlayer = i % 2 == 0 ? G.PlayerOne : G.PlayerTwo;
                Assert.ReferenceEquals(G.ActivePlayer, CorrectPlayer);
            }
        }

        [TestMethod()]
        public void BroadcastEffectTest()
        {
            GameState G = new GameState(DeckChoiceUI.COMBO_DECK, DeckChoiceUI.COMBO_DECK, true, 0);
            PowerCard TestCard = new PowerCard(G)
            {
                ManaCost = 0,
                Effects = new EffectData<PowerCard>()
                {
                    {
                        Effect.TurnEnd, (s, m) =>
                        {
                            m.ManaCost = 1;
                        }
                    },
                    {
                        Effect.CardDrawn, (s, m) =>
                        {
                            m.ManaCost = 2;
                        }
                    }
                }
            };
            G.PlayerTwo.Deck.Add(new SpellCard(G));
            G.CurrentPower = TestCard;
            Assert.AreEqual(0, G.CurrentPower.ManaCost);
            G.SwitchTurns();
            Assert.AreEqual(1, G.CurrentPower.ManaCost);
            G.ProcessMove(new Move(GameState.CARD_DRAW, 0));
            Assert.AreEqual(2, G.CurrentPower.ManaCost);
        }
    }
}