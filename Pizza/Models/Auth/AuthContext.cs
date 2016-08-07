using Pizza.Models.Auth.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizza.Models.Auth
{
    public class AuthContext
    {
        private int maxQueSize = 10;
        private Queue<Token> tokenQue;
        public AuthContext()
        {
            tokenQue = new Queue<Token>();
        }
        public void Add(DBContext dbContext, Token token)
        {
            AddToQue(token);
            dbContext.Tokens.Add(token);
            dbContext.SaveChanges();
        }

        public bool Check(DBContext dbContext, string tokenHash)
        {
            Token foundToken = null;
            foreach (var curToken in tokenQue)
            {
                if (curToken.hash.Equals(tokenHash))
                {
                    foundToken = curToken;
                    break;
                }
            }

            if (foundToken != null) //найден в очереди
            {
                if (foundToken.expdate < DateTime.Now) //недействителен
                {
                    RemoveFromDB(dbContext, foundToken);
                    return false;
                }
                else //действителен
                    return true;
            }
            else //в очереди не найден
            {
                var token = dbContext.Tokens.Find(tokenHash);
                if (token != null) //найден в БД
                {
                    if (token.expdate > DateTime.Now) //действителен
                    {
                        AddToQue(token);
                        return true;
                    }
                    else //недействителен
                    {
                        RemoveFromDB(dbContext, token);
                        return false;
                    }
                }
                else //в БД не ненайден
                    return false;
            }
        }
        public void Remove(DBContext dbContext, Token token)
        {
            if (token == null)
                return;

            if (dbContext.Tokens.Find(token.hash) != null)
                RemoveFromDB(dbContext, token);
            RemoveFromQue(token);
        }
        public void Remove(DBContext dbContext, int userID)
        {
            RemoveFromDB(dbContext, userID);
            RemoveFromQue(userID);
        }
        private void RemoveFromDB(DBContext dbContext, Token token)
        {
            var foundToken = dbContext.Tokens.FirstOrDefault(x => x.hash == token.hash);
            if (foundToken == null)
                return;
            dbContext.Tokens.Attach(foundToken);
            dbContext.Tokens.Remove(foundToken);
            dbContext.SaveChanges();
        }
        private void RemoveFromDB(DBContext dbContext, int userID)
        {
            dbContext.Tokens.RemoveRange(dbContext.Tokens.Where(t => t.user == userID));
            dbContext.SaveChanges();
        }
        private void RemoveFromQue(Token token)
        {
            List<Token> tempList = tokenQue.ToList();
            var tokenIndex = tempList.FindIndex(t => t.hash.Equals(token.hash, StringComparison.Ordinal));
            if (tokenIndex != -1)
                tempList.RemoveAt(tokenIndex);
            tokenQue = new Queue<Token>(tempList);
        }
        private void RemoveFromQue(int userID)
        {
            List<Token> tempList = tokenQue.ToList();
            tempList.RemoveAll(t => t.user == userID);
            tokenQue = new Queue<Token>(tempList);
        }
        private void AddToQue(Token token)
        {
            if (tokenQue.Count == this.maxQueSize)
                tokenQue.Dequeue();
            tokenQue.Enqueue(token);
        }
    }
}