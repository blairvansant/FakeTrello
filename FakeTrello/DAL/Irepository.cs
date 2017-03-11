using FakeTrello.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeTrello.DAL
{
    interface Irepository
    {
        // List of methods to help deliver features.

        //create cards list board
        void AddBoard(string name, ApplicationUser owner);
        void AddList(string name, Board board);
        void AddList(string name, int boardId);
        void AddCard(string name,List list, ApplicationUser owner);
        void AddCard(string name, int listId, string ownerId);

        //read or view data
        List<Card> GetCardsFromList(int listId);
        List<Card> GetCardsFromBoards(int boardId); //for searching boards
        Card GetCard(int cardId);
        List GetList(int listId);
        List<List> GetListsFromBoard(int boardId); //list of trello lists
        List<Board> GetBoardsFromUser(string UserId);
        Board GetBoard(int boardId);
        List<ApplicationUser> GetCardAttendees(int cardId);

        //update card list board (add user)
        bool AttachUser(int userId, int cardId); // true successful, false not
        bool MoveCard(int cardId, int oldListId, int newListId);
        bool CopyCar(int cardId, int newListId, string newOwnerId);

        //delete cards list board
        bool RemoveBoard(int boardId);// true successful, false not
        bool RemoveList(int listId);
        bool RemoveCard(int cardId);

    }
}
