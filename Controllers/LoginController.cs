using EquipmentManager.Models;
using EquipmentManager.Models.BusinessModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EquipmentManager.Controllers
{
    public class LoginController : Controller
    {
        private readonly MyDbContext _db;

        public LoginController(MyDbContext db)
        {
            _db = db;
        }

        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Login login)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(login.Password);
                var data = _db.Accounts.Include(s=>s.Role).Where(s => s.Code.Equals(login.Code) && s.Password.Equals(f_password) && s.Role.Name.Equals("Khách hàng")).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    HttpContext.Session.SetString("UserLogin", data.FirstOrDefault().Name);
                    HttpContext.Session.SetString("UserLogins", data.FirstOrDefault().Code);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Lỗi", "Tài khoản Email hoặc mật khẩu không đúng");
                }
            }
            return View();
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserLogin"); // Hủy session với key UserLogin đã lưu trước đó
            return RedirectToAction("Index");
        }
    }
}
