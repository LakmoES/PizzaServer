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
        public ActionResult Index()
        {
            //return View();
            return null;
        }
        public JsonResult ShowAll(string token)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);

            IEnumerable<User> users = dbContext.Users;

            return Json(users, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddTel(string token, string tel)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);
            if (tel == null)
                return Json("null tel", JsonRequestBehavior.AllowGet);

            var t = dbContext.Tokens.Find(token);
            dbContext.UserTelephones.Add(new UserTelephone { number = tel, user = t.user });
            dbContext.SaveChanges();
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddAddress(string token, string address)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);
            if (address == null)
                return Json("null address", JsonRequestBehavior.AllowGet);

            var t = dbContext.Tokens.Find(token);
            dbContext.UserAddress.Add(new UserAddress { address = address, user = t.user });
            dbContext.SaveChanges();
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTelList(string token)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);

            var t = dbContext.Tokens.Find(token);
            var telList = dbContext.UserTelephones.Where(ut => ut.user == t.user).Select(ut => new { id = ut.id, number = ut.number }).ToList();

            return Json(telList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAddressList(string token)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);

            var t = dbContext.Tokens.Find(token);
            var addressList = dbContext.UserAddress.Where(ua => ua.user == t.user).Select(ua => new { id = ua.id, address = ua.address }).ToList();

            return Json(addressList, JsonRequestBehavior.AllowGet);
        }
    }
}