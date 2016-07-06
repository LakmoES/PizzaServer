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
        private void RemoveFromDB(DBContext dbContext, Token token)
        {
            //dbContext.Tokens.Remove(token.hash);
            dbContext.Tokens.Attach(token);
            dbContext.Tokens.Remove(token);
            dbContext.SaveChanges();
        }
        private void AddToQue(Token token)
        {
            if (tokenQue.Count == this.maxQueSize)
                tokenQue.Dequeue();
            tokenQue.Enqueue(token);
        }
    }
}