using XMLConverter.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace XMLConverterWA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Convert(string stringDocument)
        {
            ViewBag.Message = "This code is what you want... Enjoy :)";
            var manager = new XmlConverterManager();
            try
            {
                // -TODO -oFBE: Remove TmpFunc
                var result = manager.CreaClasseSerializzataString(XDocument.Parse(stringDocument.Trim()), out var a);
                ViewBag.ResultRows = 10;
                ViewBag.Result = result;
            }
            catch
            {

            }

            return View();
        }
    }
}