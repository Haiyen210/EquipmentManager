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
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly MyDbContext _context;

        public OrderController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Order
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 4;
            var order = await _context.Orders.Include(o => o.Account).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            // nếu không rỗng tham số name trên url
            if (!String.IsNullOrEmpty(name))
            {
                order = await _context.Orders.Include(o => o.Account).Where(c => c.Name.Contains(name)).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            }
            return View(order);
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        // GET: Admin/Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            
            if (order == null)
            {
                return NotFound();
            }
            var orderDetail = await _context.OrderDetails.Include(x => x.Product).Where(x => x.OrdersId == order.Id).ToListAsync();
            ViewBag.orderDetail = orderDetail;
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Address", order.AccountId);
           
            return View(order);
        }

        // POST: Admin/Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Phone,Address,Note,AccountId,TotalQuantity,TotalPrice,Status,CreatedDate,ModiredDate")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    order.ModiredDate = DateTime.Now;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Address", order.AccountId);
            return View(order);
        }

        public ActionResult Delete(IFormCollection foFormCollection, int id = 0)
        {
            Order order = _context.Orders.Find(id);
            IEnumerable<OrderDetail> orderDetail = _context.OrderDetails.Where(s => s.OrdersId == id);
            if (order == null && orderDetail == null)
            {
                return HttpNotFound();
            }
                foreach (var item in orderDetail)
                {
                    _context.OrderDetails.Remove(item);
                }
                _context.Orders.Remove(order);
                _context.SaveChanges();

            return RedirectToAction("Index");

        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }

        private bool OrderExists(int id)
        {
          return _context.Orders.Any(e => e.Id == id);
        }
    }
}
