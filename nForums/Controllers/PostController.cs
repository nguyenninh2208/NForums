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
    public class PostController : Controller
    {
        //
        // GET: /Post/
        private NFORUMSEntities db = new NFORUMSEntities();
        public ActionResult DetailPost(int id)
        {
            var lst = db.POSTs.Where(x => x.THREAD_ID == id).FirstOrDefault();
            return View();
        }
        public ActionResult showPostDetail(int? id)
        {
            var showPost = db.POSTs.Where(x => x.ID == id).FirstOrDefault();
            //var lst = showPost.SUBJECT.ToString();
            //ViewBag.name = lst;
            return View("showPostDetail", showPost);
        }

        [HttpPost]
        public ActionResult deletePost(int id)
        {
            if (id == null) return HttpNotFound();

            var mesDel = "";
            var postDel = db.POSTs.Where(x => x.ID == id).FirstOrDefault();
            db.POSTs.Remove(postDel);
            int row = db.SaveChanges();
            if (row >= 1)
            {
                mesDel = "Xóa thành công";
                return Json(mesDel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                mesDel = "Có lỗi xảy ra";
                return Json(mesDel, JsonRequestBehavior.AllowGet);
            }
            //return Redirect(Request.UrlReferrer.ToString());
        }



        public ActionResult showComment(int id)
        {
            if (id == null) return HttpNotFound();
            try
            {
                var cmt = db.COMMENTs.Where(x => x.POST_ID == id).Select(x => new
                {
                    maCmt = x.ID,
                    tieude = x.POST.SUBJECT,
                    noidungCmt = x.CONTEN,
                    maPost = x.POST_ID,
                    tenNguoiCmt = x.USER.USERNAME,
                    maNguoiCmt = x.USER_ID,
                    thoigianCmt = x.CREATED,
                    ngaythamGianNguoiCmt = x.USER.CREATED,
                }).OrderBy(x => x.thoigianCmt).ToList();
                return Json(cmt, JsonRequestBehavior.AllowGet);
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



        public ActionResult showLikePost(int id)
        {
            if (id == null) return HttpNotFound();
            try
            {
                var like = db.LIKE_POST.Where(x => x.POST_ID == id).Select(x => new
                {
                    idLikePost = x.ID,
                    username = x.USER.USERNAME,
                    idPost = x.POST_ID,
                    maPost = x.POST_ID,
                    time = x.CREATED
                }).OrderBy(x => x.idLikePost).ToList();
                return Json(like, JsonRequestBehavior.AllowGet);
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




        public ActionResult EditPost(int id)
        {
            var post = db.POSTs.Find(id);
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //else if ((int)Session["UserID"] != post.USER_ID)
            //{
            //    return Content("<script>alert(\"Bạn không thể sửa !\")</script>");
            //}
            else
            {
                return PartialView("_EditPost", post);
            }

        }

        [HttpPost]
        public ActionResult EditPost()
        {
            var mes = "";
            try
            {
                var post = db.POSTs.Find(int.Parse(Request["POST_ID"]));
                post.SUBJECT = Request["subject"];
                post.CONTEN = Request["comment-content"];
                post.CREATED = DateTime.Now;
                int row = db.SaveChanges();
                
                if (row >= 1)
                {
                    mes = "Bạn đã sửa bài đăng thành công";
                    return Json(mes, JsonRequestBehavior.AllowGet);
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
            mes = "Đã có lỗi xảy ra ";
            return Json(mes, JsonRequestBehavior.AllowGet);
        }

        public int CreateIDLikePost()
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (db.LIKE_POST.Find(id) != null)
            {
                return CreateIDLikePost();
            }
            if (id < 1)
                return CreateIDLikePost();
            return id;
        }



        public ActionResult LikePost(int id)
        {
            var mes = "";
            try
            {
                if (Session["UserID"] == null)
                {
                    mes = "Vui lòng đăng nhập để thích bài đăng này";
                    return Json(mes, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int idUser = (int)Session["UserID"];
                    if (db.LIKE_POST.FirstOrDefault(x => x.POST_ID == id) == null)
                    {
                        var like = new LIKE_POST();
                        like.ID = CreateIDLikePost();
                        like.USER_ID = idUser;
                        like.POST_ID = id;
                        like.CREATED = DateTime.Now;
                        db.LIKE_POST.Add(like);
                        int row = db.SaveChanges();
                        if (row >= 1)
                        {
                            mes = "Thích bài đăng này thành công";
                            return Json(mes, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            mes = "Có lỗi xảy ra";
                            return Json(mes, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        mes = "Bạn đã thích bài đăng này";
                        return Json(mes, JsonRequestBehavior.AllowGet);
                    }
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
            mes = "Đã có lỗi xảy ra ";
            return Json(mes, JsonRequestBehavior.AllowGet);
        }


    }
}