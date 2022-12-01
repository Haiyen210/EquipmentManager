using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EquipmentManager.Models;
using EquipmentManager.Models.BusinessModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;
using X.PagedList;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using NuGet.Protocol.Plugins;

namespace EquipmentManager.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private readonly MyDbContext _context;

        public AccountController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Account
        public async Task<IActionResult> Index(string name, int page = 1)
        {

            int limit = 5; // mỗi lần hiển thị 5 bản ghi
            // nếu không rỗng tham số name trên url
            var account = await _context.Accounts.Include(a => a.Role).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            if (!String.IsNullOrEmpty(name))
            {
                account = await _context.Accounts.Where(ac => ac.Name.Contains(name)).OrderBy(ac => ac.Id).ToPagedListAsync(page, limit);
            }
            return View(account);
        }
        // GET: Admin/Account/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Admin/Account/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        // POST: Admin/Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,Gender,Birthday,Phone,Email,Address,Status,Password,RoleId,CreatedDate,ModiredDate")] Account account)
        {
            var acc = await _context.Accounts.Where(x => x.Code == account.Code).ToListAsync();
            if (ModelState.IsValid)
            {
                if (acc.Count() > 0)
                {
                    ViewBag.error = "Mã danh mục " + account.Code + " đã tồn tại";
                    return View();
                }
                else
                {
                    account.Status = true;
                    account.CreatedDate = DateTime.Now;
                    account.ModiredDate = DateTime.Now;
                    account.Password = GetMD5(account.Password);
                    _context.Add(account);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", account.RoleId);
            return View(account);
        }

        // GET: Admin/Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", account.RoleId);
            return View(account);
        }

        // POST: Admin/Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Gender,Birthday,Phone,Email,Address,Status,Password,RoleId,CreatedDate,ModiredDate")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    account.Password = GetMD5(account.Password);
                    account.ModiredDate = DateTime.Now;
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", account.RoleId);
            return View(account);
        }

        public ActionResult Delete(IFormCollection foFormCollection, int id = 0)
        {
            try
            {
                Account account = _context.Accounts.Find(id);
                if (account == null)
                {
                    return HttpNotFound();
                }
                ViewData["msg"] = "Xóa thành công!!!";
                _context.Accounts.Remove(account);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }

        private bool AccountExists(int id)
        {
          return _context.Accounts.Any(e => e.Id == id);
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
