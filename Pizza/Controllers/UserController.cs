using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pizza.Models;
using Pizza.Models.DBEntities;
using Pizza.Models.Auth;

namespace Pizza.Controllers
{
    public class UserController : Controller
    {
        private DBContext dbContext = new DBContext();
        // GET: User
        public /*ActionResult*/JsonResult Index()
        {
            //return View();
            return Json("User controller. Index", JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowAll(string token)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);

            IEnumerable<User> users = dbContext.Users;

            return Json(users, JsonRequestBehavior.AllowGet);
        }
    }
}