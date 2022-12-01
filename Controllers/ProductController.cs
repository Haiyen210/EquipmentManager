using EquipmentManager.Models;
using EquipmentManager.Models.BusinessModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace EquipmentManager.Controllers
{
    public class ProductController : Controller
    {
        private MyDbContext _db;
        public ProductController(MyDbContext context)
        {
            _db = context;
        }
        // GET: ProductController
        [Route("san-pham",Name = "product")]
        public  async Task<ActionResult> Index(int id,int page =1 )
        {
            var categories = await _db.Categories.ToListAsync();
            ViewBag.category = categories;
            int limit = 12;
            var products = await _db.Products.ToListAsync();
            var product = await _db.Products.OrderBy(x=>x.Id).ToPagedListAsync(page, limit);
            if (id != 0)
            {
                product = await _db.Products.Where(x => x.CategoryId == id).OrderBy(x => x.Id).ToPagedListAsync(page, limit);

            }
            ViewBag.count = products.Count;
            return View(product);
        }

        // GET: ProductController/Details/5
        [Route("chi-tiet-san-pham", Name = "detail")]
        public IActionResult Details(int? id)
        {
            if (id == 0 || _db.Products == null)
            {
                return NotFound();
            }
            var product=  _db.Products.Include(p => p.Category).FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var productDetail = _db.Products.Include(p => p.Category).Where(x => x.CategoryId == product.CategoryId);
           
            if(productDetail != null)
            {
                ViewBag.productCate = productDetail;
            }

            return View("Details", product);
        }







    }
}
