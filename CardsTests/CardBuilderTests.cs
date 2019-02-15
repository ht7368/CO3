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
    public class CardBuilderTests
    {
        [TestMethod()]
        public void BuildTest()
        {
            GameState G = new GameState(true);
            // For a minion
            CardBuilder MinionBuilder = new CardBuilder
            {
                TypeID = CardBuilder.CardType.Minion,
                AttackData = 2,
                HealthData = 3,
                NameData = "MIN1",
                DescriptionData = "ABCXYZ",
                MinionOnPlayData = (s, m) =>
                {

                },
                MinionEffectData = new EffectData<MinionCard>()
                {{
                        Effect.CardDrawn, (s, m) =>
                        {

                        }
                }},
            };
            MinionCard Minion = MinionBuilder.Build(G, G.PlayerOne) as MinionCard;
            Assert.AreEqual(MinionBuilder.AttackData, Minion.Attack);
            Assert.AreEqual(MinionBuilder.HealthData, Minion.Health);
            Assert.AreEqual(MinionBuilder.NameData, Minion.Name);
            Assert.AreEqual(MinionBuilder.DescriptionData, Minion.Description);
            Assert.AreEqual(MinionBuilder.MinionOnPlayData, Minion.OnPlay);
            Assert.AreEqual(MinionBuilder.MinionEffectData, Minion.Effects);
        }
    }
}