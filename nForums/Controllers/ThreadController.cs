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
    public class ThreadController : Controller
    {
        //
        // GET: /Thread/
        private NFORUMSEntities db = new NFORUMSEntities();
        public ActionResult DetailThread(int? id)
        {
            var thread = db.POSTs.Where(x => x.THREAD_ID == id).ToList();
            var name = thread.Select(x => x.THREAD.SUBJECT).FirstOrDefault();
            var idThread = db.THREADs.Where(z => z.ID == id).ToList();
            var IdThread = idThread.Select(x => x.ID).FirstOrDefault();
            ViewBag.idThread = idThread.Select(x=>x.ID).FirstOrDefault();
            TempData["idThread"] = IdThread;
            ViewBag.name = name;
            ViewBag.count = thread.Count;


            return View("DetailThread", thread);
        }
        public ActionResult NewThread()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View("NewThread");
        }

        public int CreateIDPost()
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (db.POSTs.Find(id) != null)
            {
                return CreateIDPost();
            }
            if (id < 1)
                return CreateIDPost();
            return id;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Newthread(POST post)
        {
            var mess = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session["UserID"] != null)
                    {
                        int idThread = (int)TempData["idThread"];
                        int id = (int)Session["UserID"];
                        post.ID = CreateIDPost();
                        post.USER_ID = id;
                        post.THREAD_ID = idThread;
                        post.CREATED = DateTime.Now;
                        post.STATUS = 0;
                        db.POSTs.Add(post);
                        db.SaveChanges();
                        mess = "Đăng bài viết thành công";
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                }
            }
            ViewBag.mess = mess;
            //return RedirectToAction("DetailThread", "Thread", new { id = post.THREAD_ID });
            //return Redirect(Request.UrlReferrer.ToString());
            return Json(mess, JsonRequestBehavior.AllowGet);
        }



        }
    }
