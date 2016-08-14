using Pizza.Filters;
using Pizza.Models;
using Pizza.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pizza.Controllers
{
    public class OrderController : Controller
    {
        private DBContext dbContext = new DBContext();
        // GET: Order
        public ActionResult Index()
        {
            return null;
        }
        [ExceptionLogger]
        public JsonResult GetPage(string token, int page = -1, int pageSize = -1)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);
            if (pageSize < 1 || page < 1)
                return Json("bad argument", JsonRequestBehavior.AllowGet);

            int userID = dbContext.Tokens.Find(token).user;
            
            var query = dbContext.Bills.Where(b => b.client == userID);
            var model = query.OrderBy(b => b.id).Skip((page - 1) * pageSize).Take(pageSize)
                .Join(
                dbContext.BillStatuses,
                b => b.status,
                bs => bs.id,
                (b,bs) => new
                {
                    id = b.id,
                    status = bs.title,
                    promocode = b.promocode,
                    delivery = b.delivery,
                    cost = b.cost,
                    date = b.date
                }
                )
                .ToList()
                .Select(
                m => new { id = m.id, status = m.status, promocode = m.promocode, delivery = m.delivery, cost = m.cost, date = m.date.ToString("dd.MM.yyyy HH:mm:ss") }
                ).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [ExceptionLogger]
        public JsonResult Pages(string token, int pageSize = -1)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);
            if (pageSize < 1)
                return Json("bad argument", JsonRequestBehavior.AllowGet);

            int userID = dbContext.Tokens.Find(token).user;
            var query = dbContext.Bills.Where(b => b.client == userID);

            int totalPages = (int)(Math.Ceiling(query.Count() / (decimal)pageSize));

            return Json(totalPages, JsonRequestBehavior.AllowGet);
        }
        [ExceptionLogger]
        public JsonResult Products(string token, int orderNO = -1)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);
            int userID = dbContext.Tokens.Find(token).user;

            var foundBill = dbContext.Bills.Find(orderNO);
            if (foundBill == null)
                return Json("bad argument", JsonRequestBehavior.AllowGet);
            if(foundBill.client != userID)
                return Json("bad argument", JsonRequestBehavior.AllowGet);

            var orderedProducts = dbContext.OrderedProducts.Where(op => op.bill == foundBill.id)
                .Join(
                dbContext.Products,
                op => op.product,
                p => p.id,
                (op, p) => new
                {
                    id = p.id,
                    title = p.title,
                    measure = p.measure,
                    category = p.type,
                    cost = op.cost,
                    amount = op.amount,
                    available = p.available
                })
                .Join(
                dbContext.Measures,
                op => op.measure,
                m => m.id,
                (op, m) => new
                {
                    id = op.id,
                    title = op.title,
                    measure = m.title,
                    category = op.category,
                    cost = op.cost,
                    amount = op.amount,
                    available = op.available
                }).ToList();

            return Json(orderedProducts, JsonRequestBehavior.AllowGet);
        }
    }
}