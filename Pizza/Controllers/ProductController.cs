using Pizza.Models;
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
        public JsonResult GetPage(int page = -1, int pageSize = -1, int category = -1)
        {
            if (pageSize < 1 || page < 1)
                return Json("bad argument", JsonRequestBehavior.AllowGet);
            // формируем запрос полного набора данных
            var query = category == -1 ? dbContext.Products : dbContext.Products.Where(p => p.type == category);

            // передаем нужное количество данных в модель
            // в LinqToEntities перед Skip() надо вызывать OrderBy()
            // ToList() надо вызывать именно здесь, чтобы материализовать
            // только нужные данные
            var model = query.OrderBy(o => o.id).Skip((page - 1) * pageSize).Take(pageSize).Join(
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
    }
}