using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using forum.Models;
using forum.Models.Modelviwe;
using System.Web.Security;
using Microsoft.Security.Application;
namespace forum.Controllers
{
    public class HomeController : Controller
    {
        ForumEntities db = new ForumEntities();
        
        public ActionResult Index(int id=1) //id==pagenumber
        {
            ViewBag.Message = "cats";
            var c = (from s in db.Cats where s.IsShow==true   select s).OrderByDescending(s=>s.Position).ThenBy(s=>s.ID);
            int count = c.Count();
            
           var cats = c.Skip((id - 1) *setting.perpage).Take(setting.perpage);
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
            ViewBag.Title = setting.sitename;
            ViewBag.key = setting.keywords;
            ViewBag.des = setting.descriptions;
            return View(cats.ToList());
        }

       
        public ActionResult topic(int id,int page=1) //show topics
        {
            var t = (from o in db.Topics where o.CatId == id && o.IsShow==true select o).OrderByDescending(o=>o.Position).ThenByDescending(o=>o.Position);
            int count = t.Count();
            
            var topic = t.Skip((page - 1) * setting.perpage).Take(setting.perpage);
            if (count % setting.perpage > 0)
            {
                count = count / setting.perpage;
                count = count + 1;
            }
            else
            {
                count = count/ setting.perpage;
            }
           var mycat = (from cat in db.Cats where cat.ID == id select cat).FirstOrDefault();
           ViewData["catname"] = mycat.Name;
           ViewBag.key = mycat.Keywords;
           ViewBag.des = mycat.Descriptin;
           ViewBag.Title = mycat.Name;
            if (count == 0) { count = 1; }
            ViewData["count"] = count;
            ViewData["id"] = id;
            return View(topic);
        }
        public ActionResult comment(int id,int page=1) //return comments for topics
        {
            Topic t=db.Topics.Find(id);
            if (t.IsShow == true &&t!=null)
            {
                int perpage = setting.perpage;
                IEnumerable<commentview> com = (from c in db.Comments
                                                from u in db.Users
                                                where c.TopcID == id && c.UserId == u.ID && c.IsShow == true
                                                select new commentview { id = c.ID, title = c.Title, Content = c.Content, username = u.UserName, image = u.Image, position = c.Position });
                int count = com.Count();
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
                ViewData["catname"] = (from topic in db.Topics
                                       from c in db.Cats
                                       where topic.ID == id && topic.CatId == c.ID
                                       select c.Name).FirstOrDefault();


                ViewData["Title"] = t.Title;
                ViewData["count"] = count;
                ViewData["topicid"] = id;
                ViewBag.key = t.Keywords;
                ViewBag.des = t.Descriptin;
                com = com.OrderByDescending(o => o.position).ThenBy(o => o.id).Skip((page - 1) * perpage).Take(perpage);
                return View(com);
            }
            else
            {
                return RedirectToAction("index");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        [NonAction]
        public ActionResult comment(int id, FormCollection f)  //insert comment
        {
            if (!string.IsNullOrEmpty(f["commet"]) )
            {
                Comment comment = new Comment();
                comment.TopcID = id;
                comment.Content =Sanitizer.GetSafeHtmlFragment( f["commet"]);
                comment.Title = f["mytitle"];
                comment.IsShow = true;
                comment.Date = DateTime.Now;
                comment.UserId = (from s in db.Users where s.UserName == User.Identity.Name select s.ID).SingleOrDefault();
                Topic topic = (from t in db.Topics where t.ID == id select t).SingleOrDefault();
                topic.LastUpdate = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
                //return RedirectToAction("comment", new { id = id });
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
            }
            else
            {
                ModelState.AddModelError("", "please fill all filed");
                return RedirectToAction("create","comment",new{id=id}); //Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
            }
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
