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
    public class IdGeneratorTests
    {
        [TestMethod()]
        public void GetByIdTest()
        {
            GameState G = new GameState();
            // Create a card and check that getting a card by its Id is the same card
            SpellCard First = new SpellCard(G)
            {
                Name = "First Test",
                ManaCost = 1,
                Description = "",
            };
            Assert.AreSame(First, IdGenerator.GetById(First.Id));
        }
    }
}