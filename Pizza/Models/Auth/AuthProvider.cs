using Pizza.Models.Auth.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizza.Models.Auth
{
    public class AuthProvider
    {
        private static AuthProvider instance = null;
        private AuthContext authContext;
        private TokenCreator tokenCreator;
        private AuthProvider()
        {
            authContext = new AuthContext();
            tokenCreator = new TokenCreator();
        }
        public static AuthProvider Instance
        {
            get
            {
                if (instance == null)
                    instance = new AuthProvider();
                return instance;
            }
        }
        public Token CreateToken(DBContext dbContext, int userID, string ip, int tokenLifetime)
        {
            var token = tokenCreator.Create(userID, ip, tokenLifetime);

            RemoveOldToken(dbContext, userID);

            authContext.Add(dbContext, token);

            return token;
        }

        public bool CheckToken(DBContext dbContext, string tokenHash)
        {
            if (tokenHash == null)
                return false;

            return authContext.Check(dbContext, tokenHash);
        }

        public Token GetNewToken(DBContext dbContext, string tokenHash, string ip, int tokenLifetime)
        {
            if (!CheckToken(dbContext, tokenHash))
                return null;

            var oldToken = dbContext.Tokens.Find(tokenHash);
            if (oldToken == null)
                return null;

            return CreateToken(dbContext, oldToken.user, ip, tokenLifetime);
        }

        private static void RemoveOldToken(DBContext dbContext, int userID)
        {
            dbContext.Tokens.RemoveRange(dbContext.Tokens.Where(x => x.user == userID));
            dbContext.SaveChanges();
        }
    }
}