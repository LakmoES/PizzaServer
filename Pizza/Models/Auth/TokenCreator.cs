using Pizza.Models.Auth.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizza.Models.Auth
{
    public class TokenCreator
    {
        public Token Create(int userID, string ip, int tokenLifetime)
        {
            Token token = new Token();
            token.ip = ip;
            token.createdate = DateTime.Now;
            token.expdate = token.createdate;
            token.expdate = token.expdate.AddMinutes(tokenLifetime);
            token.user = userID;

            Random rnd = new Random();
            token.hash = sha1(
                rnd.Next(100, 65535) + token.ip + token.createdate
                );

            return token;
        }

        private string sha1(string input)
        {
            byte[] hash;
            using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
                hash = sha1.ComputeHash(System.Text.Encoding.Unicode.GetBytes(input));
            var sb = new System.Text.StringBuilder();
            foreach (byte b in hash) sb.AppendFormat("{0:x2}", b);
            return sb.ToString();
        }
    }
}