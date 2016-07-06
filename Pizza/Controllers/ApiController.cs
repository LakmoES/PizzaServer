using Pizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pizza.Controllers
{
    public class ApiController : Controller
    {
        private ApiContext apiContext = new ApiContext();
        // GET: Api
        public ActionResult Index()
        {
            var apis = apiContext.Apis;

            ViewBag.Apis = apis;

            return View();
        }
    }
}