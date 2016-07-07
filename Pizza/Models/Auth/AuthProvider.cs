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

            //RemoveTokensByUserID(dbContext, userID);

            authContext.Add(dbContext, token);

            return token;
        }
        public void DeleteToken(DBContext dbContext, string tokenHash, bool deleteAllSessions = false)
        {
            if (!CheckToken(dbContext, tokenHash))
                return;

            var token = dbContext.Tokens.Find(tokenHash);
            if (!deleteAllSessions)
                authContext.Remove(dbContext, token);
            else
            {
                if (token != null)
                    authContext.Remove(dbContext, token.user);
            }
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
    }
}