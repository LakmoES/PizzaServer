using Pizza.Filters;
using Pizza.Models;
using Pizza.Models.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pizza.Controllers
{
    public class ProductController : Controller
    {
        private DBContext dbContext = new DBContext();
        // GET: Product
        public ActionResult Index()
        {
            return null;
        }

        [ExceptionLogger]
        public JsonResult GetPage(char orderBy = '0', int desc = 0, int page = -1, int pageSize = -1, int category = -1)
        {
            if (pageSize < 1 || page < 1)
                return Json("bad argument", JsonRequestBehavior.AllowGet);
            // формируем запрос полного набора данных
            var query = category == -1 ? dbContext.Products : dbContext.Products.Where(p => p.type == category);

            // передаем нужное количество данных в модель
            // в LinqToEntities перед Skip() надо вызывать OrderBy()
            // ToList() надо вызывать именно здесь, чтобы материализовать
            // только нужные данные
            IOrderedQueryable<Product> ordered;
            switch(orderBy)
            {
                case 'c': ordered = desc == 0 ?  query.OrderBy(o => o.cost) : query.OrderByDescending(o => o.cost); break;
                case 't': ordered = desc == 0 ? query.OrderBy(o => o.type) : query.OrderByDescending(o => o.type); break;
                case 'a': ordered = desc == 0 ? query.OrderBy(o => o.advertising) : query.OrderByDescending(o => o.advertising); break;
                default: ordered = desc == 0 ? query.OrderBy(o => o.id) : query.OrderByDescending(o => o.id); break;
            }
            var model = ordered.Skip((page - 1) * pageSize).Take(pageSize).Join(
                dbContext.Measures,
                p => p.measure,
                m => m.id,
                (p, m) => new
                {
                    id = p.id,
                    title = p.title,
                    measure = m.title,
                    category = p.type,
                    cost = p.cost,
                    available = p.available,
                    advertising = p.advertising
                }
                ).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [ExceptionLogger]
        public JsonResult Pages(int pageSize = -1, int category = -1)
        {
            if (pageSize < 1)
                return Json("bad argument", JsonRequestBehavior.AllowGet);
            // формируем запрос полного набора данных
            // для определения количества страниц
            var query = category == -1 ? dbContext.Products : dbContext.Products.Where(p => p.type == category);

            int totalPages = (int)(Math.Ceiling(query.Count() / (decimal)pageSize));

            return Json(totalPages, JsonRequestBehavior.AllowGet);
        }
        [ExceptionLogger]
        public JsonResult GetCategoryName(int category = -1)
        {
            if (category < 1)
                return Json("bad argument", JsonRequestBehavior.AllowGet);

            var productType = dbContext.ProductTypes.Find(category);
            if (productType == null)
                return Json("false", JsonRequestBehavior.AllowGet);

            return Json(new { name = productType.title }, JsonRequestBehavior.AllowGet);
        }
        [ExceptionLogger]
        public JsonResult GetCategoryList()
        {
            IEnumerable<ProductType> productType = dbContext.ProductTypes.OrderBy(pt => pt.id);

            return Json(productType, JsonRequestBehavior.AllowGet);
        }
        [ExceptionLogger]
        public JsonResult GetByName(string name, int page = -1, int pageSize = -1)
        {
            if (name == null || page < 1 || pageSize < 1)
                return Json("bad argument", JsonRequestBehavior.AllowGet);

            var query = dbContext.Products.Where(p => p.title.Contains(name));

            var model = query.OrderBy(p => p.id).Skip((page - 1) * pageSize).Take(pageSize).Join(
                dbContext.Measures,
                p => p.measure,
                m => m.id,
                (p, m) => new
                {
                    id = p.id,
                    title = p.title,
                    measure = m.title,
                    category = p.type,
                    cost = p.cost,
                    available = p.available,
                    advertising = p.advertising
                }
                ).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [ExceptionLogger]
        public JsonResult PagesByName(string name, int pageSize = -1)
        {
            if (pageSize < 1 || name == null)
                return Json("bad argument", JsonRequestBehavior.AllowGet);

            var query = dbContext.Products.Where(p => p.title.Contains(name));

            int totalPages = (int)(Math.Ceiling(query.Count() / (decimal)pageSize));

            return Json(totalPages, JsonRequestBehavior.AllowGet);
        }

        [ExceptionLogger]
        public JsonResult GetImagesUrl(int productID = -1)
        {
            if (productID < 1)
                return Json("bad argument", JsonRequestBehavior.AllowGet);

            var foundProduct = dbContext.Products.Find(productID);
            if (foundProduct == null)
                return Json("bad product id", JsonRequestBehavior.AllowGet);
            if (foundProduct.image == null)
                return Json("images are not set", JsonRequestBehavior.AllowGet);

            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Host;

            return Json(
                dbContext.Images.Where(x => x.id == foundProduct.image)
                .Select(x => new { small = baseUrl + x.small, medium = baseUrl + x.medium, large = baseUrl + x.large }).First(), 
                JsonRequestBehavior.AllowGet);
        }
    }
}