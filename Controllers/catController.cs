using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using forum.Models;
using System.Web.Security;

namespace forum.Controllers
{
    [Authorize(Roles="2")]
    public class catController : Controller
    {
        private ForumEntities db = new ForumEntities();

        //
        // GET: /cat/
       
    
        public ActionResult Index(int id=1)
        {
           // var cats = db.Cats;
            int count = db.Cats.Count();
            var cats = db.Cats.OrderBy(o => o.ID).Skip((id - 1) * setting.perpage).Take(setting.perpage);
            if (count % setting.perpage > 0)
            {
                count = count / setting.perpage;
                count = count + 1;
            }
            else
            {
                count = count / setting.perpage;
            }
            if (count == 0) { count = 1; }
            ViewData["count"] = count;
            return View(cats.ToList());
        }

        //
        // GET: /cat/Details/5

        public ActionResult Details(int id = 0)
        {
            Cat cat = db.Cats.Find(id);
            if (cat == null)
            {
                Response.Redirect("~/error");
                return HttpNotFound();
            }
            return View(cat);
        }

        //
        // GET: /cat/Create

        public ActionResult Create()
        {
           
            return View();
        }

        //
        // POST: /cat/Create

        [HttpPost]
        public ActionResult Create(Cat cat)
        {
            if (ModelState.IsValid)
            {
                string username=User.Identity.Name;
                int id = (from u in db.Users where u.UserName == username select u.ID).SingleOrDefault();
                cat.UserId = id;
                cat.Date = DateTime.Now;
                db.Cats.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

           
            return View(cat);
        }

        //
        // GET: /cat/Edit/5

        public ActionResult Edit(int id = 0)
        {
          
            Cat cat = db.Cats.Find(id);

            if (cat != null)
            {
                return View(cat);
            }
            else
            {
                return RedirectToAction("index");
            }
        }

        //
        // POST: /cat/Edit/5

        [HttpPost]
        public ActionResult Edit(Cat cat)
        {
            Cat temp = db.Cats.Find(cat.ID);
            if (temp != null)
            {
                try
                {
                    temp.Name = cat.Name;
                    temp.Keywords = cat.Keywords;
                    temp.Descriptin = cat.Descriptin;
                    temp.IsActive = cat.IsActive;
                    temp.IsShow = cat.IsShow;
                    temp.Position = cat.Position;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("","try again");
                    return View(cat);
                }
            }


            ModelState.AddModelError("", "not exist");
            return View(cat);    
            
            
            
        }

       

        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Cat cat = db.Cats.Find(id);
            cat.IsShow = false;
            cat.IsActive = false;
            db.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}