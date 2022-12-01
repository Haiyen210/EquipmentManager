using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EquipmentManager.Models;
using EquipmentManager.Models.BusinessModels;
using System.Net.NetworkInformation;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;


namespace EquipmentManager.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly MyDbContext _context;

        public CategoryController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Category
        public async Task<IActionResult> Index(string name, int page = 1)
        {

            int limit = 5; // mỗi lần hiển thị 5 bản ghi
            var category = await _context.Categories.OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            // nếu không rỗng tham số name trên url
            if (!String.IsNullOrEmpty(name))
            {
                category = await _context.Categories.Where(c => c.Name.Contains(name)).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            }

            return View(category);
        }

        // GET: Admin/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,Status,CreatedDate,ModiredDate")] Category category)
        {
            var cate = await _context.Categories.Where(x => x.Code == category.Code).ToListAsync();
            if (ModelState.IsValid)
            {
                if (cate.Count() > 0)
                {
                    ViewBag.error = "Mã danh mục " + category.Code + " đã tồn tại";
                    return View();
                }
                else
                {
                    category.Status = true;
                    category.CreatedDate = DateTime.Now;
                    category.ModiredDate = DateTime.Now;
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
               
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Status,CreatedDate,ModiredDate")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //category.CreatedDate = DateTime.Now;
                    category.ModiredDate = DateTime.Now;
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }
        public ActionResult Delete(IFormCollection foFormCollection, int id = 0)
        {
            try
            {
                Category category = _context.Categories.Find(id);
                if (category == null)
                {
                    return HttpNotFound();
                }
                var product = _context.Products.Where(b => b.CategoryId == category.Id);
                if (product.Count() > 0)
                {
                    ViewData["msg"] = "Xóa thất bại,Danh mục có khóa ngoại không xóa được!!";
                }
                else
                {
                    ViewData["msg"] = "Xóa thành công!!!";
                    _context.Categories.Remove(category);
                    _context.SaveChanges();
                }
              
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
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
