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
            // Create a card and check that getting a card by its Id is the same card
            SpellCard First = new SpellCard("First Test", 1, "");
            Assert.AreSame(First, IdGenerator.GetById(First.Id));
        }
    }
}