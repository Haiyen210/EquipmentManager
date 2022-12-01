using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EquipmentManager.Models;
using EquipmentManager.Models.BusinessModels;
using X.PagedList;

namespace EquipmentManager.Areas.Admin.Controllers
{
    public class BannerController : BaseController
    {
        private readonly MyDbContext _context;

        public BannerController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Banner
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 5; // mỗi lần hiển thị 5 bản ghi
            var banner = await _context.Banners.OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            // nếu không rỗng tham số name trên url
            if (!String.IsNullOrEmpty(name))
            {
                banner = await _context.Banners.Where(c => c.Name.Contains(name)).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            }
            return View(banner);
        }

        // GET: Admin/Banner/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Banners == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // GET: Admin/Banner/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Banner/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,Images,Description,Status,CreatedDate,ModiredDate")] Banner banner)
        {
            var ban = await _context.Banners.Where(x => x.Code == banner.Code).ToListAsync();
           
            if (ModelState.IsValid)
            {
                if (ban.Count() > 0)
                {
                    ViewBag.error = "Mã danh mục " + banner.Code + " đã tồn tại";
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
                            banner.Images = FileName; // gán tên ảnh cho thuộc tinh Images
                        }

                    }

                    banner.Status = true;
                    banner.CreatedDate = DateTime.Now;
                    banner.ModiredDate = DateTime.Now;
                    _context.Add(banner);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
              
            }
            return View(banner);
        }

        // GET: Admin/Banner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Banners == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        // POST: Admin/Banner/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Images,Description,Status,CreatedDate,ModiredDate")] Banner banner)
        {
            if (id != banner.Id)
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
                            banner.Images = FileName; // gán tên ảnh cho thuộc tinh Image
                        }

                    }
                    else
                    {
                        var imagePath = await _context.Banners.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                        if (imagePath != null)
                        {
                            banner.Images = imagePath.Images;
                        }

                    }

                    banner.ModiredDate = DateTime.Now;
                    _context.Update(banner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerExists(banner.Id))
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
            return View(banner);
        }

        public ActionResult Delete(IFormCollection foFormCollection, int id = 0)
        {
            try
            {
                Banner banner = _context.Banners.Find(id);
                if (banner == null)
                {
                    return HttpNotFound();
                }
                ViewData["msg"] = "Xóa thành công!!!";
                _context.Banners.Remove(banner);
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

        private bool BannerExists(int id)
        {
            return _context.Banners.Any(e => e.Id == id);
        }
    }
}
