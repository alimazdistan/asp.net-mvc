using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using forum.Models;
using Microsoft.Security.Application;
using System.Web.Security;

namespace forum.Controllers
{
    public class commentController : Controller
    {
        private ForumEntities db = new ForumEntities();
        
        [Authorize]
        public ActionResult response(int id = 0)
        {
            Comment comment = db.Comments.Find(id);
            Topic t = db.Topics.Find(comment.TopcID);
            if (t.IsActive == false)
            {
                ModelState.AddModelError("", "not active");
                return RedirectToAction("notactive", "comment");
            }
           else if (comment == null)
            {
                Response.Redirect("~/error");
                return HttpNotFound();
            }
            comment.Title =string.Empty;
            comment.Content = string.Empty;
            ViewData["toppicid"] = comment.TopcID;
            return View(comment);
        }
        [HttpPost]
        [Authorize]
        public ActionResult response(int id,Comment comment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Comment c = (from s in db.Comments where s.ID == id select s).SingleOrDefault();
                    Comment newcomment = new Comment();
                    newcomment.TopcID = c.TopcID;
                    newcomment.Title = comment.Title;
                    newcomment.Content = "<div class='response'>" +Sanitizer.GetSafeHtmlFragment( c.Content) + "</div>" + comment.Content;
                    newcomment.Date = DateTime.Now;
                    newcomment.IsShow = true;

                    newcomment.UserId = (from s in db.Users where s.UserName == User.Identity.Name select s.ID).SingleOrDefault();
                    Topic topic = (from t in db.Topics where t.ID == c.TopcID select t).SingleOrDefault();
                    topic.LastUpdate = DateTime.Now;
                    db.Comments.Add(newcomment);
                    db.SaveChanges();

                    return RedirectToAction("comment", "home", new { id = c.TopcID });
                }
                catch (Exception e)
                {
                    return View();
                }
            }
            ModelState.AddModelError("", "try again");
            return View(comment);
        }

        //
        // GET: /comment/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            Topic t = db.Topics.Find(id);
           
            if (t.IsActive == false)
            {
                ModelState.AddModelError("", "not active");
                return RedirectToAction("notactive", "comment");
            }
            else  if (t == null) 
            {
                Redirect("~/error.html");
            }
            Comment c = new Comment();
            c.Content = string.Empty;
            c.Title = string.Empty;
            
            return View(c);
        }

        //
        // POST: /comment/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(int id,Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.TopcID = id;
                comment.Content = Sanitizer.GetSafeHtmlFragment(comment.Content);
                comment.IsShow = true;
                comment.Date = DateTime.Now;
                comment.UserId = (from s in db.Users where s.UserName == User.Identity.Name select s.ID).SingleOrDefault();
                Topic topic = (from t in db.Topics where t.ID == id select t).SingleOrDefault();
                topic.LastUpdate = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("comment", "home", new { id = id });
            }

            ModelState.AddModelError("", "please fill *");
            
            return View(comment);
        }

        //
        // GET: /comment/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Comment comment = db.Comments.Find(id);
            ViewData["topicid"] = comment.TopcID;
            int userid = (from u in db.Users where u.UserName == User.Identity.Name select u.ID).SingleOrDefault();
            myrole role = new myrole();
           bool admin= role.IsUserInRole(User.Identity.Name,"2");
            if (comment == null  )
            {
                Response.Redirect("~/error.html");
                return HttpNotFound();
            }
           
            if (admin || comment.UserId==userid)
            return View(comment);
            else
                return Redirect("~/account/login");
        }

        //
        // POST: /comment/Edit/5

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Comment comment)
        {
            var c = (from d in db.Comments where d.ID == comment.ID select d).SingleOrDefault() ;
            
            int userid = (from u in db.Users where u.UserName == User.Identity.Name select u.ID).SingleOrDefault();
            myrole role = new myrole();
            bool admin = role.IsUserInRole(User.Identity.Name, "2");
            if (ModelState.IsValid && c!=null &&(admin ||c.UserId==userid))
            {
                c.Content =Sanitizer.GetSafeHtmlFragment( comment.Content);
                c.Title = comment.Title;
            
                if (admin)//user is admin
                {
                    c.IsShow = comment.IsShow;
                    c.Position = comment.Position;
                }
                Topic topic = (from t in db.Topics where t.ID == c.TopcID select t).SingleOrDefault();
                topic.LastUpdate = DateTime.Now;
                
                db.SaveChanges();
                return RedirectToAction("comment", "home", new { id = c.TopcID });
            }

            return Redirect("~/account/login");
        }

        //
        

        [Authorize]
        public ActionResult delete(int id)
        {
            Comment comment = db.Comments.Find(id);
            int userid = (from u in db.Users where u.UserName == User.Identity.Name select u.ID).SingleOrDefault();
            myrole role = new myrole();
            bool admin = role.IsUserInRole(User.Identity.Name, "2");
            if (admin || userid == comment.UserId)
            {
                comment.IsShow = false;

                db.SaveChanges();

                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
            }
            return Redirect("~/account/login");
        }
        public ActionResult notactive()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}