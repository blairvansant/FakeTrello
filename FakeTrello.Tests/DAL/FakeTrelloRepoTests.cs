using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeTrello.DAL;
using Moq;
using FakeTrello.Models;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

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

        [TestMethod]
        public void EnsureIcanAddBoard()
        {
            //arrange
            List<Board> fake_board_table = new List<Board>();


            //IQueryable<Board> / this is the type
            var query_boards = fake_board_table.AsQueryable();//re-express this list as something that understands queries

            Mock<FakeTrelloContext> fake_context = new Mock<FakeTrelloContext>();
            Mock<DbSet<Board>> mock_boards_set = new Mock<DbSet<Board>>();

            //hey LINQ, use the provider from our fake board/table list
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.Provider).Returns(query_boards.Provider);
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.Expression).Returns(query_boards.Expression);
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.ElementType).Returns(query_boards.ElementType);
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.GetEnumerator()).Returns(() => query_boards.GetEnumerator() );

            mock_boards_set.Setup(b => b.Add(It.IsAny<Board>())).Callback((Board board) => fake_board_table.Add(board));
            fake_context.Setup(c => c.Boards).Returns(mock_boards_set.Object);//context.boards returns fake_board_table (a list)




            FakeTrelloRepository repo = new FakeTrelloRepository(fake_context.Object);
            ApplicationUser a_user = new ApplicationUser {
                Id = "my-user-id",
                UserName = "Sammy",
                Email = "sammy@gmail.com"
            };

            //act
            repo.AddBoard("MyBoard", a_user/*Application goes here*/);

            //assert
            Assert.AreEqual(repo.Context.Boards.Count(), 1);
        }
    }
}
