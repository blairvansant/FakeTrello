using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeTrello.DAL;

namespace FakeTrello.Tests.DAL
{
    [TestClass]
    public class FakeTrelloRepositoryTests
    {
        public object Context { get; private set; }

        [TestMethod]
        public void EnsureIHaveNotNullContext()
        {
            FakeTrelloRepository repo = new FakeTrelloRepository();
            Assert.IsNotNull(repo);
        }

        [TestMethod]
        public void EnsureICanInjectContextInstance() 
        {
            FakeTrelloContext context = new FakeTrelloContext();
            FakeTrelloRepository repo = new FakeTrelloRepository(context);
            Assert.IsNotNull(repo.Context);
        }
    }
}
