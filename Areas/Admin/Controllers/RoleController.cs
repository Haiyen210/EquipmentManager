using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EquipmentManager.Models;
using EquipmentManager.Models.BusinessModels;

namespace EquipmentManager.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly MyDbContext _context;

        public RoleController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Role
        public async Task<IActionResult> Index()
        {
              return View(await _context.Roles.ToListAsync());
        }

        // GET: Admin/Role/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Admin/Role/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status")] Role role)
        {
            if (ModelState.IsValid)
            {
                role.Status = true;
                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Admin/Role/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Admin/Role/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status")] Role role)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.Id))
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
            return View(role);
        }

        public ActionResult Delete(IFormCollection foFormCollection, int id = 0)
        {
            try
            {
                Role role = _context.Roles.Find(id);
                if (role == null)
                {
                    return HttpNotFound();
                }
                ViewData["msg"] = "Xóa thành công!!!";
                _context.Roles.Remove(role);
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

        private bool RoleExists(int id)
        {
          return _context.Roles.Any(e => e.Id == id);
        }
    }
}
