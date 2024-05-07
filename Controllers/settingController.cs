using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using forum.Models;
using forum.Models.Modelviwe;

namespace forum.Controllers
{
    [Authorize (Roles="2")]
    public class settingController : Controller
    {
        //
        // GET: /setting/

        public ActionResult Index()
        {
            forum.Models.Modelviwe.settingview setting = new Models.Modelviwe.settingview();

            return View(setting);
        }
        [HttpPost]
        public ActionResult index(settingview s)
        {
            if (ModelState.IsValid)
            {
                setting.perpage = s.perpage;
                setting.keywords = s.keywords;
                setting.descriptions = s.descriptions;
                setting.sitename = s.sitename;
                return RedirectToAction("index", "home");
            }
            return View(s);
        }
    }
}
