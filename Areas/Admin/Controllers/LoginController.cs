using EquipmentManager.Models;
using EquipmentManager.Models.BusinessModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System;
using System.Security.Cryptography;
using System.Text;

namespace EquipmentManager.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {

        private readonly MyDbContext _context;

        public LoginController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet] // GET Hiển thị form để nhập dữ liệu
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost] // POST khi submit form
        [ValidateAntiForgeryToken]
        public IActionResult Index(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // trả về trạng thái lỗi
            }
            else
            {
                var f_password = GetMD5(model.Password);
                // sẽ xử lý logic phần đăng nhập tại đây
                var data = _context.Accounts.Include(s => s.Role).Where(s => s.Code.Equals(model.Code) && s.Password.Equals(f_password) && s.Role.Name.Equals("Quản trị viên")).ToList();
                if (data.Count() > 0)
                {
                        HttpContext.Session.SetString("AdminLogin", data.FirstOrDefault().Name);
                        return RedirectToAction("Index", "Home");
                    
                }
                else
                {
                    ModelState.AddModelError("Lỗi", "Tài khoản Email hoặc mật khẩu không đúng");
                }

            }

            return View();

        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminLogin"); // Hủy session với key AdminLogin đã lưu trước đó
            return RedirectToAction("Index");
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
    }

}
