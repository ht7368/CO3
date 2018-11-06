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
    public class MoveTests
    {
        [TestMethod()]
        public void MoveTest()
        {
            // Here we create moves that should be valid
            // If they are not, there will be an uncaught exception,
            // So the test will fail.
            var First = new Move(1, 0);
            var Second = new Move(1, 1);
            // Here we create moves that should be invalid
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var Third = new Move(0, 0);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var Fourth = new Move(0, 1);
            });
        }
    }
}