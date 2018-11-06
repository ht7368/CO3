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
            // Creates instances for testing
            var First = new MinionCard("First Test", 1, 1, 1, "");
            var Second = new PowerCard("Second Test", 1, "");
            var Third = new SpellCard("Third test", 1, "");
            // Assert that not Ids are equal to 0
            Assert.AreNotEqual<uint>(First.Id, 0);
            Assert.AreNotEqual<uint>(Second.Id, 0);
            Assert.AreNotEqual<uint>(Third.Id, 0);
        }
    }
}