using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using nForums.Models;
using System.Data.Entity.Validation;

namespace nForums.Controllers
{
    public class HomeController : Controller
    {
        private NFORUMSEntities db = new NFORUMSEntities();
         
        public ActionResult Index()
        {
            var lst = db.CATEGORies.ToList();

            return View(lst);
        }

        public ActionResult showDetailCate(int id)
        {
            if (id == null) return HttpNotFound();
            try
            {
                
                var cate = db.THREADs.Where(x => x.CATEGORY_ID == id).Select(x => new
                {
                    id = x.ID,
                    chude = x.SUBJECT,
                    nguoitao = x.USER_ID,
                    idCate = x.CATEGORY_ID
                }).OrderBy(x=>x.id).ToList();
                return Json(cate, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException ex)
            {
                var mystring = "";
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        mystring += validationError.PropertyName + " : " + validationError.ErrorMessage + "\n";
                    }
                }
                return Content(mystring);
            }
        }

        public ActionResult getModerator()
        {
            try
            {
                var mod = db.USERs.Where(x=>x.IS_MODERATOR == 1).Select(x => new
                {
                    id = x.ID,
                    username = x.USERNAME,
                }).OrderBy(x => x.id).ToList();
                return Json(mod, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException ex)
            {
                var mystring = "";
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        mystring += validationError.PropertyName + " : " + validationError.ErrorMessage + "\n";
                    }
                }
                return Content(mystring);
            }
        }

        public ActionResult getQuantityUser()
        {
            try
            {
                var user = db.USERs.Where(x => x.IS_MODERATOR == 0).Select(x => new
                {
                    id = x.ID,
                    username = x.USERNAME,
                }).OrderBy(x => x.id).ToList();
                return Json(user, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException ex)
            {
                var mystring = "";
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        mystring += validationError.PropertyName + " : " + validationError.ErrorMessage + "\n";
                    }
                }
                return Content(mystring);
            }
        }


        public ActionResult getUserRecent()
        {
            try
            {
                var userRecent = db.USERs.Where(x => x.IS_MODERATOR == 0).Select(x => new
                {
                    id = x.ID,
                    username = x.USERNAME,
                    date = x.CREATED
                }).OrderBy(x => x.date).First();
                return Json(userRecent, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException ex)
            {
                var mystring = "";
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        mystring += validationError.PropertyName + " : " + validationError.ErrorMessage + "\n";
                    }
                }
                return Content(mystring);
            }
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
       
    }
}