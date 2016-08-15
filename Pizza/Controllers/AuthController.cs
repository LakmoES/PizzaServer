using Pizza.Filters;
using Pizza.Models;
using Pizza.Models.Auth;
using Pizza.Models.DBEntities;
using Pizza.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pizza.Controllers
{
    public class AuthController : Controller
    {
        private DBContext dbContext = new DBContext();
        private Random random = new Random();
        private int tokenLifetime = 60;
        private string guestPrefix = "Guest";
        // GET: Auth
        [ExceptionLogger]
        public JsonResult Login(string username, string password)
        {
            if (username == null || password == null)
                return Json("false", JsonRequestBehavior.AllowGet);

            var userList = dbContext.Users.Where(u => u.username.ToLower() == username.ToLower()).ToList();
            if (userList.Count == 0)
                return Json("user not found", JsonRequestBehavior.AllowGet);
            if (userList.First().password == password)
            {
                var token = AuthProvider.Instance.CreateToken(dbContext, userList.First().id, GetUserIP(), tokenLifetime);
                var jsonToken = new { token_hash = token.hash, lifetime = tokenLifetime };
                return Json(jsonToken, JsonRequestBehavior.AllowGet);
            }

            return Json("false", JsonRequestBehavior.AllowGet);
        }
        [ExceptionLogger]
        public JsonResult Logout(string token, int allSessions = 0)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);

            bool deleteAllSessions = allSessions == 1;
            AuthProvider.Instance.DeleteToken(dbContext, token, deleteAllSessions);

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        [ExceptionLogger]
        public JsonResult GetNewToken(string token)
        {
            if (token == null)
                return Json("false", JsonRequestBehavior.AllowGet);

            var newToken = AuthProvider.Instance.GetNewToken(dbContext, token, GetUserIP(), tokenLifetime);
            if (newToken == null)
                return Json("false", JsonRequestBehavior.AllowGet);

            var jsonToken = new { token_hash = newToken.hash, lifetime = tokenLifetime };
            return Json(jsonToken, JsonRequestBehavior.AllowGet);
        }
        [ExceptionLogger]
        public JsonResult RegisterUser(string username, string password, string email, string name, string surname)
        {
            User user = new User();
            user.username = username;
            user.password = password;
            user.email = email == null ? null : email.ToLower();
            user.name = name;
            user.surname = surname;
            user.guest = 0;
            user.registrationdate = DateTime.UtcNow;

            List<Error> errors;
            if (UserValidator.CheckAdditionUser(user, out errors))
            {
                var foundUser = dbContext.Users.Where(x => x.username.ToLower() == user.username.ToLower()).ToList();
                if (foundUser.Count > 0)
                {
                    errors.Add(new Error { error = "Пользователь с таким именем уже существует" });
                }
                else
                {
                    foundUser = dbContext.Users.Where(x => x.email == user.email).ToList();
                    if (foundUser.Count > 0)
                        errors.Add(new Error { error = "Пользователь с таким email адресом уже существует" });
                    else
                    {
                        try
                        {
                            dbContext.Users.Add(user);
                            dbContext.SaveChanges();
                        }
                        catch (System.Data.SqlClient.SqlException sEx) { errors.Add(new Error { error = "Ошибка БД. " + sEx.Message }); }
                        //catch (Exception) { errors.Add(new Error { error = "Неизвестная ошибка БД" }); }
                    }
                }
            }

            if (errors.Count > 0)
                return Json(errors, JsonRequestBehavior.AllowGet);
            else
                return Json("ok", JsonRequestBehavior.AllowGet);
        }
        [ExceptionLogger]
        public JsonResult NewGuest()
        {
            User guest = new User();
            guest.username = guestPrefix + dbContext.GetNextGuestIDValue();
            guest.password = RandomString(10);
            guest.email = null;
            guest.name = null;
            guest.surname = null;
            guest.guest = 1;
            guest.registrationdate = DateTime.UtcNow;

            List<Error> errors;
            if (UserValidator.CheckAdditionGuest(guest, out errors))
            {
                var foundUser = dbContext.Users.Where(x => x.username == guest.username).ToList();
                if (foundUser.Count > 0)
                {
                    errors.Add(new Error { error = "Гость с таким именем уже существует. Обратитесь к администратору. Имя:'" + guest.username + "'" });
                }
                else
                {
                    try
                    {
                        dbContext.Users.Add(guest);
                        dbContext.SaveChanges();
                    }
                    catch (System.Data.SqlClient.SqlException sEx) { errors.Add(new Error { error = "Ошибка БД. " + sEx.Message }); }
                    //catch (Exception) { errors.Add(new Error { error = "Неизвестная ошибка БД" }); }
                }
            }

            if (errors.Count > 0)
                return Json(errors, JsonRequestBehavior.AllowGet);
            else
                return Json(new { username = guest.username, password = guest.password }, JsonRequestBehavior.AllowGet);
        }
        [ExceptionLogger]
        public JsonResult NoMoreGuest(string token, string username, string password, string email, string name, string surname)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);

            int userID = dbContext.Tokens.Find(token).user;
            if (dbContext.Users.Find(userID).guest != 1)
                return Json("not a guest", JsonRequestBehavior.AllowGet);

            User updatedUser = new User();
            updatedUser.username = username;
            updatedUser.password = password;
            updatedUser.email = email == null ? null : email.ToLower();
            updatedUser.name = name;
            updatedUser.surname = surname;
            updatedUser.guest = 0;

            List<Error> errors;
            if (UserValidator.CheckAdditionUser(updatedUser, out errors))
            {
                var foundUser = dbContext.Users.Where(x => x.username.ToLower() == updatedUser.username.ToLower()).ToList();
                if (foundUser.Count > 0)
                {
                    errors.Add(new Error { error = "Пользователь с таким именем уже существует" });
                }
                foundUser = dbContext.Users.Where(x => x.email == updatedUser.email && x.id != userID).ToList();
                if (foundUser.Count > 0)
                    errors.Add(new Error { error = "Пользователь с таким email адресом уже существует" });
                else
                {
                    var user = dbContext.Users.Find(userID);
                    dbContext.Users.Attach(user);
                    var entry = dbContext.Entry(user);

                    entry.Property(e => e.username).IsModified = true;
                    entry.Entity.username = updatedUser.username;

                    entry.Property(e => e.password).IsModified = true;
                    entry.Entity.password = updatedUser.password;

                    entry.Property(e => e.email).IsModified = true;
                    entry.Entity.email = updatedUser.email;

                    entry.Property(e => e.name).IsModified = true;
                    entry.Entity.name = updatedUser.name;

                    entry.Property(e => e.surname).IsModified = true;
                    entry.Entity.surname = updatedUser.surname;

                    entry.Property(e => e.guest).IsModified = true;
                    entry.Entity.guest = updatedUser.guest;

                    dbContext.SaveChanges();
                }
            }

            if (errors.Count > 0)
                return Json(errors, JsonRequestBehavior.AllowGet);
            else
                return Json("ok", JsonRequestBehavior.AllowGet);
        }
        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private string GetUserIP()
        {
            return Request.UserHostAddress;
        }
    }
}