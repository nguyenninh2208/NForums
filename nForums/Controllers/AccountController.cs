using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using nForums.Models;
using System.Data.Entity.Validation;

namespace nForums.Controllers
{
    public class AccountController : Controller
    {
        private NFORUMSEntities db = new NFORUMSEntities();
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(String username, string password)
        {
            var rs = "";
            var x = db.USERs.Where(model => model.USERNAME.Equals(username)).FirstOrDefault();
            if (x != null)
            {
                if (x.PASSWORD == password)
                {
                    rs = "success";
                    Session["UserID"] = x.ID;
                    Session["UserName"] = VietHoa(x.USERNAME);
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    rs = "!Password";
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                rs = "!Username";
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
        }
        public static string VietHoa(string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;

            string result = "";

            //lấy danh sách các từ  

            string[] words = s.Split(' ');

            foreach (string word in words)
            {
                // từ nào là các khoảng trắng thừa thì bỏ  
                if (word.Trim() != "")
                {
                    if (word.Length > 1)
                        result += word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower() + " ";
                    else
                        result += word.ToUpper() + " ";
                }

            }
            return result.Trim();
        }
        //
        // GET: /Account/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        public JsonResult IsAlreadyUsername(string username)
        {
            Boolean statuss = true;
            if (username == null)
            {
                statuss = false;
            }
            var lst = db.USERs.ToArray();

            if (Array.Exists(lst, x => x.USERNAME.ToUpper() == username.ToUpper()))
            {
                statuss = false;
            }
            return Json(statuss, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsAlreadyEmail(string email)
        {

            Boolean statuss = true;
            if (email == null)
            {
                statuss = false;
            }
            var lst = db.USERs.ToArray();

            if (Array.Exists(lst, x => x.Email.ToUpper() == email.ToUpper()))
            {
                statuss = false;
            }
            return Json(statuss, JsonRequestBehavior.AllowGet);
        }


        public USER CheckUserName(string username)
        {
            USER user = db.USERs.Where(x => x.USERNAME == username).SingleOrDefault();
            return user;
        }
        public USER CheckEmail(string email)
        {
            USER user = db.USERs.Where(x => x.Email == email).SingleOrDefault();
            return user;
        }
        public int CreateID() // Cơ chế sinh mã
        {
            int id = 0;
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                int a = rand.Next() % 10;
                a = a < 0 ? -1 * a : a;
                id += a;
            }
            USER user = db.USERs.Find(id);
            if (user != null)
                return CreateID();
            return id;
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(USER user)
        {
            var message = "";
            try
            {
                if (ModelState.IsValid)
                {
                    if (CheckUserName(user.USERNAME) != null)
                    {
                        message = "Tên đăng nhập đã được sử dụng !";
                    }
                    else if (CheckEmail(user.Email) != null)
                    {
                        message = "Email đã được sử dụng !";
                    }
                    else
                    {
                        var use = new USER();
                        use.ID = CreateID();
                        use.USERNAME = user.USERNAME;
                        use.PASSWORD = user.PASSWORD;
                        use.IS_MODERATOR = 0;
                        use.Email = user.Email;
                        use.STATUS = 0;
                        use.GENDER = user.GENDER;
                        use.CREATED = DateTime.Now;
                        db.USERs.Add(use);
                        db.SaveChanges();
                        Session["UserID"] = user.ID;
                        Session["UserName"] = user.USERNAME;
                        message = "Đăng ký thành công !";
                        //trả về trang đang đứng
                        //return Redirect(Request.UrlReferrer.ToString());
                    }
                }
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
            return Json(message, JsonRequestBehavior.AllowGet);
        }





        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            return RedirectToAction("Index", "Home");
        }





    }
}