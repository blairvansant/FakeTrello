using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using FakeTrello.Models;

namespace FakeTrello.DAL
{
    public class FakeTrelloRepository : IRepository, IBoardManager, IBoardQuery
    {
        //private FakeTrelloContext context; // Data member
        SqlConnection _trelloConnection;

        public FakeTrelloRepository()
        {
            _trelloConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        public void AddBoard(string name, ApplicationUser owner)
        {
            //Board board = new Board { Name = name, Owner = owner };
            //Context.Boards.Add(board);
            //Context.SaveChanges();

            _trelloConnection.Open();

            try
            {
                var addBoardCommand = _trelloConnection.CreateCommand();
                addBoardCommand.CommandText = "Insert into Boards(Name,Owner_Id) values(@name,@ownerId)";
                var nameParameter = new SqlParameter("name", SqlDbType.VarChar);
                nameParameter.Value = name;
                addBoardCommand.Parameters.Add(nameParameter);
                var ownerParameter = new SqlParameter("owner", SqlDbType.Int);
                ownerParameter.Value = owner.Id;
                addBoardCommand.Parameters.Add(ownerParameter);

                addBoardCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                _trelloConnection.Close();
            }
        }

        public void AddCard(string name, int listId, string ownerId)
        {
            //throw new NotImplementedException();
        }

        public void AddCard(string name, List list, ApplicationUser owner)
        {
            //throw new NotImplementedException();
        }

        public void AddList(string name, int boardId)
        {
            throw new NotImplementedException();
        }

        public void AddList(string name, Board board)
        {
            throw new NotImplementedException();
        }

        public bool AttachUser(string userId, int cardId)
        {
            throw new NotImplementedException();
        }

        public bool CopyCard(int cardId, int newListId, string newOwnerId)
        {
            throw new NotImplementedException();
        }

        public Board GetBoard(int boardId)
        {
            _trelloConnection.Open();

            try
            {
                var getBoardCommand = _trelloConnection.CreateCommand();
                getBoardCommand.CommandText = @"
                    SELECT boardId, Name, Url, Owner_Id 
                    FROM Boards 
                    WHERE BoardId = @boardId";
                var boardIdParam = new SqlParameter("boardId", SqlDbType.Int);
                boardIdParam.Value = boardId;
                getBoardCommand.Parameters.Add(boardIdParam);

                var reader = getBoardCommand.ExecuteReader();

                if (reader.Read())
                {
                    var board = new Board
                    {
                        BoardId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        URL = reader.GetString(2),
                        Owner = new ApplicationUser {Id = reader.GetString(3)}
                    };
                    return board;
                }
            }
            catch(Exception ex) { }
            finally
            {
                _trelloConnection.Close();
            }

            return null;
            
        }

        public List<Board> GetBoardsFromUser(string userId)
        {
            //return Context.Boards.Where(b => b.Owner.Id == userId).ToList();

            _trelloConnection.Open();

            try
            {
                var getBoardCommand = _trelloConnection.CreateCommand();
                getBoardCommand.CommandText = @"
                    SELECT boardId, Name, Url, Owner_Id 
                    FROM Boards 
                    WHERE Owner_Id = @userId";
                var boardIdParam = new SqlParameter("userId", SqlDbType.VarChar);
                boardIdParam.Value = userId;
                getBoardCommand.Parameters.Add(boardIdParam);

                var reader = getBoardCommand.ExecuteReader();

                var boards = new List<Board>();
                while (reader.Read())
                {
                    var board = new Board
                    {
                        BoardId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        URL = reader.GetString(2),
                        Owner = new ApplicationUser { Id = reader.GetString(3) }
                    };

                    boards.Add(board);
                }

                return boards;
            }
            catch (Exception ex) { }
            finally
            {
                _trelloConnection.Close();
            }

            return new List<Board>();

        }

        public Card GetCard(int cardId)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> GetCardAttendees(int cardId)
        {
            throw new NotImplementedException();
        }

        public List<Card> GetCardsFromBoard(int boardId)
        {
            throw new NotImplementedException();
        }

        public List<Card> GetCardsFromList(int listId)
        {
            throw new NotImplementedException();
        }

        public List GetList(int listId)
        {
            throw new NotImplementedException();
        }

        public List<List> GetListsFromBoard(int boardId)
        {
            throw new NotImplementedException();
        }

        public bool MoveCard(int cardId, int oldListId, int newListId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveBoard(int boardId)
        {
            _trelloConnection.Open();

            try
            {
                var removeBoardCommand = _trelloConnection.CreateCommand();
                removeBoardCommand.CommandText = @"
                    Delete 
                    From Boards
                    Where BoardId = @boardId";

                var boardIdParameter = new SqlParameter("boardId", SqlDbType.Int);
                boardIdParameter.Value = boardId;
                removeBoardCommand.Parameters.Add(boardIdParameter);

                removeBoardCommand.ExecuteNonQuery();

                return true;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                _trelloConnection.Close();
            }

            return false;
            
        }

        public bool RemoveCard(int cardId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveList(int listId)
        {
            throw new NotImplementedException();
        }

        public void EditBoardName(int boardId, string newname)
        {
            //Board found_board = GetBoard(boardId);
            //if (found_board != null)
            //{
            //    found_board.Name = newname; // Akin to 'git add'
            //    Context.SaveChanges(); // Akin to 'git commit'
            //}
            // False Positive: SaveChanges is missing.

            _trelloConnection.Open();

            try
            {
                var updateBoardCommand = _trelloConnection.CreateCommand();
                updateBoardCommand.CommandText = @"
                    Update Boards 
                    Set Name = @NewName
                    Where boardid = @boardId";
                var nameParameter = new SqlParameter("NewName", SqlDbType.VarChar);
                nameParameter.Value = newname;
                updateBoardCommand.Parameters.Add(nameParameter);
                var boardIdParameter = new SqlParameter("boardId", SqlDbType.Int);
                boardIdParameter.Value = boardId;
                updateBoardCommand.Parameters.Add(boardIdParameter);

                updateBoardCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                _trelloConnection.Close();
            }

        }
    }
}