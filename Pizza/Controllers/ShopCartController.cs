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
        public JsonResult Show(string token, string promocode)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);

            var promo = dbContext.PromoCodes.Find(promocode);
            if (promocode != null && promo == null)
                return Json("bad promocode", JsonRequestBehavior.AllowGet);
            if (promo != null)
            {
                if (promo.active == 0)
                    return Json("inactive promocode", JsonRequestBehavior.AllowGet);
            }
            else
                promo = new PromoCode { discount = 0 };

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
                    amount = sc.amount,
                    resultPrice = sc.advertising == 0 ? (sc.cost * sc.amount) / (decimal)100.0 * (100 - promo.discount) : sc.cost * sc.amount
                }
                ).ToList();

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
        public JsonResult MakeOrder(string token, string promocode, int addressID = -1)
        {
            if (!AuthProvider.Instance.CheckToken(dbContext, token))
                return Json("wrong token", JsonRequestBehavior.AllowGet);

            var promo = dbContext.PromoCodes.Find(promocode);
            if (promocode != null && promo == null)
                return Json("bad promocode", JsonRequestBehavior.AllowGet);
            if (promo != null)
                if (promo.active == 0)
                    return Json("inactive promocode", JsonRequestBehavior.AllowGet);

            int userID = dbContext.Tokens.Find(token).user;

            var address = dbContext.UserAddress.FirstOrDefault(a => a.id == addressID && a.user == userID);
            if (addressID != -1 && address == null)
                return Json("bad address", JsonRequestBehavior.AllowGet);

            var products = dbContext.ShoppingCarts.Where(sc => sc.user == userID)
                .Join(
                dbContext.Products,
                sc => sc.product,
                p => p.id,
                (sc, p) => new
                {
                    p.id,
                    p.title,
                    p.cost,
                    p.available,
                    p.advertising,
                    sc.amount
                }
                );
            if (products.Count() <= 0)
                return Json("empty shopping cart order", JsonRequestBehavior.AllowGet);
            if (products.Count(p => p.available != 1) > 0)
                return Json("attempt to order unavailable", JsonRequestBehavior.AllowGet);

            Delivery delivery = null;
            if (address != null)
            {
                delivery = new Delivery { address = address.id };
                dbContext.Deliveries.Add(delivery);
                dbContext.SaveChanges();
                dbContext.Entry(delivery).GetDatabaseValues();
            }

            Bill bill = new Bill
            {
                client = userID,
                status = 1,
                promocode = promo == null ? null : promo.code,
                delivery = delivery == null ? null : (int?)delivery.id,
                staff = null,
                date = DateTime.Now
            };
            dbContext.Bills.Add(bill);
            dbContext.SaveChanges();
            dbContext.Entry(bill).GetDatabaseValues();

            var productList = products.ToList();
            Decimal billCost = 0;
            int promoDiscount = promo == null ? 0 : promo.discount;
            var orderedProducts = new List<OrderedProduct>();
            foreach (var p in productList)
            {
                decimal productCost = p.advertising == 1 ? p.cost * p.amount : (p.cost * p.amount) / (decimal)100.0 * (100 - promoDiscount);
                orderedProducts.Add(new OrderedProduct { bill = bill.id, product = p.id, amount = p.amount, cost = productCost });
                billCost += productCost;
            }
            dbContext.Bills.Attach(bill);
            var entry = dbContext.Entry(bill);

            entry.Property(e => e.cost).IsModified = true;
            entry.Entity.cost = billCost;

            dbContext.OrderedProducts.AddRange(orderedProducts);
            dbContext.ShoppingCarts.RemoveRange(dbContext.ShoppingCarts.Where(sc => sc.user == userID));
            dbContext.SaveChanges();

            return Json(new { orderNO = bill.id }, JsonRequestBehavior.AllowGet);
        }
    }
}