using Pizza.Models;
using Pizza.Models.Auth;
using Pizza.Models.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pizza.Controllers
{
    public class ShopCartController : Controller
    {
        private DBContext dbContext = new DBContext();
        // GET: ShopCart
        public ActionResult Index()
        {
            return null;
        }
        public JsonResult Show(string token)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);

            int userID = dbContext.Tokens.Find(token).user;
            var shoppingCarts = dbContext.ShoppingCarts.Where(sc => sc.user == userID).Select(sc => new { sc.product, sc.amount })
                .Join(
                dbContext.Products,
                sc => sc.product,
                p => p.id,
                (sc, p) => new
                {
                    productid = p.id,
                    title = p.title,
                    measure = p.measure,
                    category = p.type,
                    cost = p.cost,
                    available = p.available,
                    advertising = p.advertising,
                    amount = sc.amount
                }
                )
                .Join(
                dbContext.Measures,
                sc => sc.measure,
                m => m.id,
                (sc, m) => new
                {
                    productid = sc.productid,
                    title = sc.title,
                    measure = m.title,
                    category = sc.category,
                    cost = sc.cost,
                    available = sc.available,
                    advertising = sc.advertising,
                    amount = sc.amount
                }
                )
                .ToList();

            return Json(shoppingCarts, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddProduct(string token, int productID = -1, int amount = -1)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);
            if (productID == -1 || amount < 1)
                return Json("bad argument", JsonRequestBehavior.AllowGet);

            int userID = dbContext.Tokens.Find(token).user;

            if (dbContext.Products.Find(productID) != null)
            {
                var foundShoppingCart = dbContext.ShoppingCarts.FirstOrDefault(sc => sc.user == userID && sc.product == productID);
                if (foundShoppingCart != null)
                {
                    dbContext.ShoppingCarts.Attach(foundShoppingCart);
                    var entry = dbContext.Entry(foundShoppingCart);

                    entry.Property(e => e.amount).IsModified = true;
                    entry.Entity.amount = foundShoppingCart.amount + amount;
                }
                else
                    dbContext.ShoppingCarts.Add(new ShoppingCart { product = productID, amount = amount, user = userID });

                dbContext.SaveChanges();
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            else
                return Json("product not found", JsonRequestBehavior.AllowGet);
        }
        public JsonResult RemoveProduct(string token, int productID = -1)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);
            if (productID == -1)
                return Json("bad argument", JsonRequestBehavior.AllowGet);

            int userID = dbContext.Tokens.Find(token).user;
            dbContext.ShoppingCarts.RemoveRange(dbContext.ShoppingCarts.Where(sc => sc.product == productID && sc.user == userID));
            dbContext.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditProduct(string token, int productID = -1, int amount = -1)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);
            if (productID == -1 || amount < 1)
                return Json("bad argument", JsonRequestBehavior.AllowGet);

            int userID = dbContext.Tokens.Find(token).user;

            var foundShoppingCart = dbContext.ShoppingCarts.FirstOrDefault(sc => sc.user == userID && sc.product == productID);
            if (foundShoppingCart == null)
                return Json("product not found", JsonRequestBehavior.AllowGet);

            dbContext.ShoppingCarts.Attach(foundShoppingCart);
            var entry = dbContext.Entry(foundShoppingCart);

            entry.Property(e => e.amount).IsModified = true;
            entry.Entity.amount = amount;

            dbContext.SaveChanges();
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult Clear(string token)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);

            int userID = dbContext.Tokens.Find(token).user;
            dbContext.ShoppingCarts.RemoveRange(dbContext.ShoppingCarts.Where(sc => sc.user == userID));
            dbContext.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult MakeOrder(string token)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);

            int userID = dbContext.Tokens.Find(token).user;
            return Json("", JsonRequestBehavior.AllowGet); //todo: доделать метод. найти среди всех товаров валидные и заказать
        }
    }
}