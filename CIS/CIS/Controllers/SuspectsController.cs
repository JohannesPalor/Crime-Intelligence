using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIS.Controllers
{
    public class SuspectsController : Controller
    {
        // GET: Suspects
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Suspects()
        {
            return View();
        }
    }
}