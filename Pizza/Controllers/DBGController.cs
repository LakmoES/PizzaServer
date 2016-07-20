using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pizza.Controllers
{
    public class DBGController : Controller
    {
        // GET: DBG
        public ActionResult Index()
        {
            return null;
        }
        public JsonResult Write(string message)
        {
            if (message != null)
                System.Diagnostics.Debug.WriteLine("New message: " + message);
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}