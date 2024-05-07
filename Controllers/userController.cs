using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using forum.Models;
using forum.Models.Modelviwe;
using System.Web.Security;
namespace forum.Controllers
{
    public class userController : Controller
    {
        //
        // GET: /user/
        private ForumEntities db=new ForumEntities();
        [Authorize(Roles="2")]
        public ActionResult Index(int id=1)//id==pagenumber
        {
          var model=  db.Users.OrderByDescending(u => u.RegistDate).Skip((id-1)*setting.perpage).Take(setting.perpage);
          int count = db.Users.Count();
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
            return View(model.ToList());
        }
        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        
        public ActionResult create(userview u)
        {
            if (u.Image != null)
            {
                if (checkphoto(u.Image) == false)//check image
                {
                    ModelState.AddModelError("", ".jpg or .png and <100KB");
                    return View(u);
                }
                
            }
            
            if (ModelState.IsValid )
            {
                
                
                if (modeluser(u))//check and insert to database
                {
                    FormsAuthentication.SetAuthCookie(u.UserName, false);
                    return RedirectToAction("index","home");
                }
                ModelState.AddModelError("", "user exist");
                u.Password = u.RePassword = null;
               return View(u);
            }
            u.Password = u.RePassword = null;
            ModelState.AddModelError("", "try again");
            return View(u);
        }
        private bool modeluser(userview u)
        {

            forum.Models.User check = (from d in db.Users where d.UserName == u.UserName select d).SingleOrDefault();
            if (check==null)
            {
            User newuser=new User();
            newuser.Name = u.Name;
            newuser.LastName = u.LastName;
            newuser.UserName = u.UserName.Trim();
            newuser.Password = u.Password.Trim();
            newuser.Email = u.Email;
            newuser.IsActive = true;
            newuser.RoleID =1;//1-user 2-admin
            newuser.RegistDate = DateTime.Now;
            newuser.LastActivity = DateTime.Now;
             
            try
            {
                string s = Server.MapPath("~/Image") + "//";
                if (u.Image != null)
                {
                   

                    u.Image.SaveAs(s + u.UserName + System.IO.Path.GetExtension(u.Image.FileName));
                    newuser.Image = "~/Image/" + u.UserName + System.IO.Path.GetExtension(u.Image.FileName);
                }
                else
                {
                    newuser.Image = "~/Image/" + "no-image.png";
                }
                db.Users.Add(newuser);
                db.SaveChanges();
                    return true;
            }
            catch(Exception e)
            {
            return false;
            }
            }
            else 
                return false;
        }

       
       private bool checkphoto(HttpPostedFileBase Image)
        {
            string[] ext = { ".jpg", ".png" };
            if (Image.ContentLength / 1024 < 100)
            {
              string ch=  System.IO.Path.GetExtension(Image.FileName);
              foreach (string s in ext)
                  if (string.Compare(s,ch,true)==0)
                      return true;
              return false;

            }
            return false;
        }
        [Authorize]
       public ActionResult profile()
       {
           string username = User.Identity.Name;
           int id = (from u in db.Users where u.UserName == username select u.ID).SingleOrDefault();
         forum.Models.User U=(from d in db.Users where d.ID==id select  d).FirstOrDefault();
           return View(U);
       }
        [Authorize(Roles="2")]
        public ActionResult details(int id)
        {
            
           // int id = (from u in db.Users where u.UserName == username select u.ID).SingleOrDefault();
            forum.Models.User U = (from d in db.Users where d.ID == id select d).FirstOrDefault();
            return View("profile", U);
        }
        [Authorize]
       public ActionResult passwordchange()
       {
          
           
           return View();
       }
       [HttpPost]
        [Authorize]
       public ActionResult passwordchange(userpasseditview model)
       {
           
           forum.Models.User u = (from d in db.Users where d.UserName ==User.Identity.Name  select d).FirstOrDefault();
           if (u != null && String.Compare(model.newpassword, model.renewpassword, false) == 0 && String.Compare(model.password, u.Password, false) == 0)
           {
               u.Password = model.newpassword.Trim();
               db.SaveChanges();
               return RedirectToAction("index","home");
           }
           ModelState.AddModelError("", "try again");
           return View();
       }
       public ActionResult imagechange()
       {
           return View();
       }
        [HttpPost]
        [Authorize]
       public ActionResult imagechange(HttpPostedFileBase image)
       {
          
            forum.Models.User newuser = (from d in db.Users where d.UserName ==User.Identity.Name  select d).FirstOrDefault();
            if (newuser!=null)
            
            if (image!=null)
                if(checkphoto(image))//check image
                {
                    string s = Server.MapPath("~/Image") + "//";

                   image.SaveAs(s + User.Identity.Name + System.IO.Path.GetExtension(image.FileName));
                    newuser.Image = "~/Image/" + User.Identity.Name + System.IO.Path.GetExtension(image.FileName);
                    return RedirectToAction("profile");
                }
            ModelState.AddModelError("", "try again; .png or .jpg and <100KB ");
           return View();
       }
        [Authorize(Roles = "2")]
        public ActionResult edit(int id)
        {
            var model = db.Users.Find(id);
        ViewData["role"]= new SelectList(db.Roles, "ID", "RolrName", model.RoleID);
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles="2")]
        public ActionResult edit(forum.Models.User u)
        {
            try
            {
                forum.Models.User model = db.Users.Find(u.ID);
                model.Name = u.Name;
                model.Password = u.Password.Trim();
                model.IsActive = u.IsActive;
                model.UserName = u.UserName.Trim();
                model.LastName = u.LastName;
                model.RoleID = u.RoleID;
                model.Email = u.Email;
                db.SaveChanges();
                return RedirectToAction("index");
            }
            catch (Exception e)
            {
                //u.Password = "";
                return View(u);
            }
        }
        [Authorize(Roles= "2")]
        
        public ActionResult delete(int id)
        {
            User user = db.Users.Find(id);
            user.IsActive = false;
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
