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
    public class topicController : Controller
    {
        private ForumEntities db = new ForumEntities();

       
        public ActionResult Index(int id=1)
        {
           // var topics = db.Topics.Include(t => t.Cat).Include(t => t.User);

            int count = db.Topics.Count();
          var topics = db.Topics.OrderBy(o=>o.ID).Skip((id - 1) * setting.perpage).Take(setting.perpage);
          if (count % setting.perpage > 0)
          {
              count = count / setting.perpage;
              count = count + 1;
          }
          else
          {
              count = count / setting.perpage;
          }
            if (count == 0) { count = 1;  }
            ViewData["count"] = count;
            return View(topics.ToList());
        }

        //
        // GET: /topic/Details/5

        public ActionResult Details(int id = 0)
        {
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                Response.Redirect("~/error");
               
            }
            return View(topic);
        }

        //
        // GET: /topic/Create

        public ActionResult Create()
        {
            IEnumerable<Cat> cat = db.Cats.Where(c => c.IsActive == true).ToList();
            ViewBag.CatId = new SelectList(cat, "ID", "Name");
         
            return View();
        }

        //
        // POST: /topic/Create

        [HttpPost]
        public ActionResult Create(Topic topic)
        {
            if (ModelState.IsValid)
            {
                topic.LastUpdate = DateTime.Now;
                topic.UserID = (from c in db.Users where c.UserName == User.Identity.Name select c.ID).SingleOrDefault();
                db.Topics.Add(topic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatId = new SelectList(db.Cats, "ID", "Name", topic.CatId);
           // ViewBag.UserID = new SelectList(db.Users, "ID", "Name", topic.UserID);
            return View(topic);
        }

        //
        // GET: /topic/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                Response.Redirect("~/error");
                return HttpNotFound();
            }
            ViewBag.CatId = new SelectList(db.Cats, "ID", "Name", topic.CatId);
            //ViewBag.UserID = new SelectList(db.Users, "ID", "Name", topic.UserID);
            return View(topic);
        }

        //
        // POST: /topic/Edit/5

        [HttpPost]
        public ActionResult Edit(Topic topic)
        {
            Topic t = db.Topics.Where(o => o.ID == topic.ID).SingleOrDefault();
            if (ModelState.IsValid && t!=null)
            {
                t.LastUpdate = DateTime.Now;
                t.UserID = (from u in db.Users where u.UserName == User.Identity.Name select u.ID).SingleOrDefault();
                t.CatId = topic.CatId;
                t.Title = topic.Title;
                t.Descriptin = topic.Descriptin;
                t.IsActive = topic.IsActive;
                t.IsShow = topic.IsShow;
                t.Position = topic.Position;
                t.Keywords = topic.Keywords;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatId = new SelectList(db.Cats, "ID", "Name", topic.CatId);
           // ViewBag.UserID = new SelectList(db.Users, "ID", "Name", topic.UserID);
            ModelState.AddModelError("", "try aghain");
            return View(topic);
        }

        public ActionResult delete(int id)
        {
            Topic topic = db.Topics.Find(id);
            topic.IsShow = false;
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