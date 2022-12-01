using EquipmentManager.Models;
using EquipmentManager.Models.BusinessModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EquipmentManager.Controllers
{
    public class RegisterController : Controller
    {
        private readonly MyDbContext _db;

        public RegisterController(MyDbContext db)
        {
            _db = db;
        }

        // GET: RegisterController
        public ActionResult Index()
        {
            return View();
        }


        // POST: RegisterController/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Account account, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                var check = _db.Accounts.FirstOrDefault(s => s.Code == account.Code);
                if (check == null)
                {
                    if(account.Password != ConfirmPassword)
                    {
                        ViewBag.errorPass = "Mã xác nhận không khớp với mật khẩu đã nhập!";
                    }
                    account.Password = GetMD5(account.Password);
                    account.RoleId = 2;
                    account.Status = true;
                    account.CreatedDate = DateTime.Now;
                    account.ModiredDate = DateTime.Now;
                    _db.Accounts.Add(account);
                    _db.SaveChanges();
                    return RedirectToAction("Index","Login");
                }
                else
                {
                    ViewBag.error = "Tên tài khoản này đã tồn tại!";
                    return View();
                }


            }
            if(ConfirmPassword == null)
            {
                ViewBag.errorPass = "Mã xác nhận không được để trống!!";
            }
            return View();

        }
        //create a string MD5
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


        // GET: RegisterController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RegisterController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RegisterController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RegisterController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
