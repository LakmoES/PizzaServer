using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pizza.Models;
using Pizza.Models.DBEntities;
using Pizza.Models.Auth;
using Pizza.Validators;

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
        public JsonResult Edit(string token, string password, string email, string name, string surname)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);
            if (password == null && email == null && name == null && surname == null)
                return Json("nothing to edit", JsonRequestBehavior.AllowGet);

            var user = dbContext.Users.Find(dbContext.Tokens.Find(token).user);

            var updatedUser = new User { password = password, email = email, name = name, surname = surname };
            List<Error> errors;
            if (!UserValidator.CheckUpdate(updatedUser, out errors))
                return Json(errors, JsonRequestBehavior.AllowGet);

 
            dbContext.Users.Attach(user);
            var entry = dbContext.Entry(user);
            if (updatedUser.password != null)
            {
                entry.Property(e => e.password).IsModified = true;
                entry.Entity.password = updatedUser.password;
            }
            if (updatedUser.email != null)
            {
                entry.Property(e => e.email).IsModified = true;
                entry.Entity.email = updatedUser.email;
            }
            if (updatedUser.name != null)
            {
                entry.Property(e => e.name).IsModified = true;
                entry.Entity.name = updatedUser.name;
            }
            if (updatedUser.surname != null)
            {
                entry.Property(e => e.surname).IsModified = true;
                entry.Entity.surname = updatedUser.surname;
            }
            dbContext.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
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
        public JsonResult RemoveTel(string token, int? telID)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);
            if (telID == null)
                return Json("bad argument", JsonRequestBehavior.AllowGet);

            var t = dbContext.Tokens.Find(token);
            dbContext.UserTelephones.RemoveRange(dbContext.UserTelephones.Where(tel => tel.user == t.user && tel.id == telID));
            dbContext.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult RemoveAddress(string token, int? addressID)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);
            if (addressID == null)
                return Json("bad argument", JsonRequestBehavior.AllowGet);

            var t = dbContext.Tokens.Find(token);
            dbContext.UserAddress.RemoveRange(dbContext.UserAddress.Where(address => address.user == t.user && address.id == addressID));
            dbContext.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditTel(string token, int? telID, string tel)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);
            if (telID == null || tel == null)
                return Json("bad argument", JsonRequestBehavior.AllowGet);
            if(tel.Length < 1)
                return Json("bad argument", JsonRequestBehavior.AllowGet);

            var userID = dbContext.Tokens.Find(token).user;
            var telephone = dbContext.UserTelephones.FirstOrDefault(t => t.id == telID && t.user == userID);
            if (telephone == null)
                return Json("telephone not found", JsonRequestBehavior.AllowGet);

            var updatedTel = new UserTelephone { number = tel };

            dbContext.UserTelephones.Attach(telephone);
            var entry = dbContext.Entry(telephone);

            entry.Property(e => e.number).IsModified = true;
            entry.Entity.number = updatedTel.number;
            
            dbContext.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditAddress(string token, int? addressID, string address)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);
            if (addressID == null || address == null)
                return Json("bad argument", JsonRequestBehavior.AllowGet);
            if (address.Length < 1)
                return Json("bad argument", JsonRequestBehavior.AllowGet);

            var userID = dbContext.Tokens.Find(token).user;
            var addr = dbContext.UserAddress.FirstOrDefault(a => a.id == addressID && a.user == userID);
            if (addr == null)
                return Json("address not found", JsonRequestBehavior.AllowGet);

            var updatedAddress = new UserAddress { address = address };

            dbContext.UserAddress.Attach(addr);
            var entry = dbContext.Entry(addr);

            entry.Property(e => e.address).IsModified = true;
            entry.Entity.address = updatedAddress.address;

            dbContext.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInfo(string token)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);

            var t = dbContext.Tokens.Find(token);
            var user = dbContext.Users.Where(u => u.id == t.user).Select(u => new { username = u.username, name = u.name, surname = u.surname, email = u.email, guest = u.guest }).FirstOrDefault();

            return Json(user, JsonRequestBehavior.AllowGet);
        }
    }
}
 