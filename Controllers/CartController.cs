using EquipmentManager.Models;
using EquipmentManager.Models.BusinessModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.WebSockets;

namespace EquipmentManager.Controllers
{
    public class CartController : Controller, IActionFilter
    {
        private readonly MyDbContext _db;

        private List<Cart> carts = new List<Cart>();
        public CartController(MyDbContext db)
        {
            _db = db;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cartInSession = HttpContext.Session.GetString("My-Cart");
            if (cartInSession != null)
            {
                carts = JsonConvert.DeserializeObject<List<Cart>>(cartInSession);
            }
            base.OnActionExecuting(context);
        }
        public IActionResult Add(int id)
        {
            var login = HttpContext.Session.GetString("UserLogin");
            if (login != null)
            {
                var cartitem = carts.Find(p => p.Id == id);
                if (cartitem != null)
                {
                    cartitem.Quantity++;
                    //carts.Where(c => c.Id == id).First().Quantity += 1;
                }
                else
                {
                    var pro = _db.Products.Find(id);
                    if (pro != null)
                    {
                        var cart = new Cart();
                        cart.Id = pro.Id;
                        cart.Name = pro.Name;
                        cart.Code = pro.Code;
                        cart.Quantity = 1;
                        if (pro.SalePrice == 0)
                        {
                            cart.Price = pro.Price;

                        }
                        if (pro.SalePrice > 0)
                        {
                            cart.Price = pro.SalePrice;

                        }
                        cart.Image = pro.Images;
                        cart.userName = login;
                        carts.Add(cart);
                    }

                }
                var session = HttpContext.Session;
                string jsoncart = JsonConvert.SerializeObject(carts);
                session.SetString("My-Cart", jsoncart);
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                ViewBag.error = "Hãy đăng nhập để thêm sản phẩm vào giỏ hàng!!!!";
                return RedirectToAction("Index", "Login");
            }


        }

        public IActionResult Remove(int id)
        {
            if (carts.Any(c => c.Id == id))
            {
                var item = carts.Where(c => c.Id == id).First();
                carts.Remove(item);
                var session = HttpContext.Session;
                string jsoncart = JsonConvert.SerializeObject(carts);
                session.SetString("My-Cart", jsoncart);

            }

            return RedirectToAction("Index", "Cart");
        }

        [Route("", Name = "Update")]
        [HttpPost]
        public IActionResult Update([FromForm] int Id, [FromForm] int Quantity)
        {
            if (carts != null)
            {
                if (carts.Any(c => c.Id == Id))
                {
                    if(Quantity > 0)
                    {
                        carts.FirstOrDefault(c => c.Id == Id).Quantity = Quantity;
                    }
                    else
                    {
                        carts.FirstOrDefault(c => c.Id == Id).Quantity = 1;
                    }
                   
                }
            }
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(carts);
            session.SetString("My-Cart", jsoncart);
            return Ok();
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("My-Cart");
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Index()
        {
            return View(carts);
        }
        public IActionResult CheckOut()
        {
            var login = HttpContext.Session.GetString("UserLogins");
            var account = _db.Accounts.FirstOrDefault(s => s.Code.Equals(login));
            var order = new Order();
            if (account != null)
            {
                order.Name = account.Name;
                order.Email = account.Email;
                order.Address = account.Address;
                order.Phone = account.Phone;
            }
            ViewBag.Cart = carts;
            return View(order);
        }

        // POST: CartController/CheckOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckOut(Order order)
        {
            if (ModelState.IsValid)
            {
                double total = 0;
                for (int i = 0; i < carts.Count; i++)
                {
                    var thanhtien = carts[i].Quantity * carts[i].Price;
                    total += (double)thanhtien;
                }
                var login = HttpContext.Session.GetString("UserLogins");
                var account = _db.Accounts.FirstOrDefault(s => s.Code.Equals(login));
                order.AccountId = account.Id;
                order.Status = 2;
                order.CreatedDate = DateTime.Now;
                order.ModiredDate = DateTime.Now;
                order.TotalQuantity = carts.Count();
                order.TotalPrice = total;
                _db.Orders.Add(order);
                _db.SaveChanges();
                if (order != null)
                {
                    for (int i = 0; i < carts.Count(); i++)
                    {
                        var orderDetail = new OrderDetail();
                        orderDetail.ProductId = carts[i].Id;
                        orderDetail.OrdersId = order.Id;
                        orderDetail.Quantity = carts[i].Quantity;
                        orderDetail.Price = carts[i].Price;
                        orderDetail.CreatedDate = DateTime.Now;
                        orderDetail.ModiredDate = DateTime.Now;
                        _db.OrderDetails.Add(orderDetail);
                        _db.SaveChanges();
                    }
                  
                }
                _db.SaveChanges();
                HttpContext.Session.Remove("My-Cart");
                return RedirectToAction("CheckOutSuccess", "Cart");
            }
            else
            {
                return View();
            }
           

        }
        public IActionResult CheckOutSuccess()
        {
            return View();
        }
    }
}
