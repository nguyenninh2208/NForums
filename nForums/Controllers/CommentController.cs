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
    public class CommentController : Controller
    {
        //
        // GET: /Comment/
        private NFORUMSEntities db = new NFORUMSEntities();

        
        public ActionResult PostComment(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var model = db.POSTs.Find(id);
            return View("_PostComment", model);
        }

        public int CreateIDComment()
        {
            Random rand = new Random();
            int id = rand.Next(1, 1000);
            if (db.COMMENTs.Find(id) != null)
            {
                return CreateIDComment();
            }
            if (id < 1)
                return CreateIDComment();
            return id;
        }

        [HttpPost]
        public ActionResult PostComment()
        {
            var mes = "";
            try
            {
               
                int id = (int)Session["UserID"];
                //var cmt = db.COMMENTs.Find(int.Parse(Request["POST_ID"]));
                var cmt = new COMMENT();
                cmt.ID = CreateIDComment();
                cmt.CONTEN = Request["comment"];
                cmt.POST_ID = int.Parse(Request["POST_ID"]);
                cmt.CREATED = DateTime.Now;
                cmt.USER_ID = id;
                
                db.COMMENTs.Add(cmt);
                db.SaveChanges();
                mes = "Bình luận thành công";
                return Json(mes, JsonRequestBehavior.AllowGet);
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


        public ActionResult showReplyComment(int? id)
        {
            try
            {
                var replyCmt = db.REPLY_COMMENT.Where(x=>x.COMMENT_ID == id).Select(x => new
                {
                    maReplyCmt = x.ID,
                    noidungCmt = x.CONTEN,
                    tieude = x.COMMENT.POST.SUBJECT,
                    noidungCmtBanDau = x.COMMENT.CONTEN,
                    maCmt = x.COMMENT_ID + 1,
                    tenNguoiCmt = x.USER.USERNAME,
                    maNguoiCmt = x.USER_ID,
                    thoigianCmt = x.CREATED,
                    ngaythamGianNguoiCmt = x.USER.CREATED
                }).OrderBy(x => x.thoigianCmt).ToList();
                return Json(replyCmt, JsonRequestBehavior.AllowGet);
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

       




	}
}