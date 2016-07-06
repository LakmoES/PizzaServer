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
        private int tokenLifetime = 60;
        // GET: Auth
        public JsonResult Login(string username, string password)
        {
            if (username == null || password == null)
                return Json("false", JsonRequestBehavior.AllowGet);

            var userList = dbContext.Users.Where(u => u.username == username.ToLower()).ToList();
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

        //[HttpPost]
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
        public JsonResult RegisterUser(string username, string password, string email, string name, string surname)
        {
            User user = new User();
            user.username = username == null ? null : username.ToLower();
            user.password = password;
            user.email = email == null ? null : email.ToLower();
            user.name = name;
            user.surname = surname;
            user.guest = 0;

            List<Error> errors;
            if (UserValidator.CheckAdditionUser(user, out errors))
            {
                var foundUser = dbContext.Users.Where(x => x.username == user.username).ToList();
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
                        catch (Exception) { errors.Add(new Error { error = "Неизвестная ошибка БД" }); }
                    }
                }
            }

            if (errors.Count > 0)
                return Json(errors, JsonRequestBehavior.AllowGet);
            else
                return Json("ok", JsonRequestBehavior.AllowGet);
        }
        private string GetUserIP()
        {
            return Request.UserHostAddress;
        }
    }
}