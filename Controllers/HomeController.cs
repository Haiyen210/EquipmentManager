using EquipmentManager.Models;
using EquipmentManager.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace EquipmentManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyDbContext _context;
        public HomeController(ILogger<HomeController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }
       

        
        [HttpGet]
        public  IActionResult Index(int? id)
        {
            IEnumerable<Banner> banner = _context.Banners;
            IEnumerable<Product> productSale = _context.Products.Where(x => x.SalePrice > 0);
            ViewBag.productSale = productSale;
            ViewBag.banner = banner;
            IEnumerable<Product> product = _context.Products.OrderByDescending(s=>s.Id).Take(8);
            ViewBag.product = product;
           
                return View () ;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}