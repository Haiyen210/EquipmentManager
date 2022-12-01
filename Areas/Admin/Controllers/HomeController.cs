using EquipmentManager.Models;
using EquipmentManager.Models.BusinessModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using X.PagedList;

namespace EquipmentManager.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly MyDbContext _context;
        public HomeController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var product = _context.Products.Count();
            var category = _context.Categories.Count();
            var order = _context.Orders.Count();
            var account = _context.Accounts.Count();
            ViewData["product"] = product;
            ViewData["category"] = category;
            ViewData["order"] = order;
            ViewData["account"] = account;
            return View();
        }

    }
}
