using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EquipmentManager.Models;
using EquipmentManager.Models.BusinessModels;
using System.Xml.Linq;
using System.IO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace EquipmentManager.Areas.Admin.Controllers
{    
    public class ProductController : BaseController
    {
        private readonly MyDbContext _context;
        public ProductController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Product
        [Route("Admin/Product", Name = "products")]
        public async Task<IActionResult> Index(string name,int cate, int page = 1)
        {
            var category = await _context.Categories.ToListAsync();
            ViewBag.category = category;
            int limit = 4; // mỗi lần hiển thị 5 bản ghi
            var product = await _context.Products.Include(p => p.Category).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            // nếu không rỗng tham số name trên url
            if (!String.IsNullOrEmpty(name))
            {
                product = await _context.Products.Where(c => c.Name.Contains(name)).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            }
            ViewBag.cate = cate;
            if (cate > 0)
            {
                product = await _context.Products.Where(c => c.CategoryId == cate).OrderBy(c => c.Id).ToPagedListAsync(page, limit);

            }
           
            return View(product);
        }
        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,Images,Description,Price,SalePrice,Status,CategoryId,CreatedDate,ModiredDate")] Product product)
        {
            var pro = await _context.Products.Where(x => x.Code == product.Code).ToListAsync();
            if (ModelState.IsValid)
            {
                if (pro.Count() > 0)
                {
                    ViewBag.error = "Mã danh mục " + product.Code + " đã tồn tại";
                    return View();
                }
                else
                {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var FileName = file.FileName;
                    // upload ảnh vào thư mục wwwroot\\images
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        product.Images = FileName; // gán tên ảnh cho thuộc tinh Images
                    }

                }
                product.CreatedDate = DateTime.Now;
                product.ModiredDate = DateTime.Now;
                product.Status = true;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId.ToString());
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Images,Description,Price,SalePrice,Status,CategoryId,CreatedDate,ModiredDate")] Product product)
        {

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count() > 0 && files[0].Length > 0)
                    {
                        var file = files[0];
                        var FileName = file.FileName;
                        // upload ảnh vào thư mục wwwroot\\Product
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            product.Images = FileName; // gán tên ảnh cho thuộc tinh Image
                        }

                    }
                    else
                    {
                        var imagePath = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                        if (imagePath != null)
                        {
                            product.Images = imagePath.Images;
                        }
                       
                    }

                    product.ModiredDate = DateTime.Now;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        public ActionResult Delete(IFormCollection foFormCollection, int id = 0)
        {

            Product product = _context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }



        // GET: Admin/Product/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Products == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Category)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //// POST: Admin/Product/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Products == null)
        //    {
        //        return Problem("Entity set 'MyDbContext.Products'  is null.");
        //    }
        //    var product = await _context.Products.FindAsync(id);
        //    if (product != null)
        //    {
        //        _context.Products.Remove(product);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
