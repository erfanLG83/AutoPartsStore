using AutoPartsStore.Domain.Entities;
using AutoPartsStore.Infrastructure.ViewModels.Cart;
using AutoPartsStore.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AutoPartsStore.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartController> _logger;
        public CartController(ApplicationDbContext context,ILogger<CartController> logger)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var products = await _context.ProductCards.Include(n => n.Product).ThenInclude(n => n.Category).Where(n => n.UserId == userId && !n.OrderId.HasValue).ToListAsync();
            ViewBag.TotalPrice = products.Sum(n => n.Product.Price * n.Count).ToString("n0") + " ریال";
            var user = await _context.Users.FirstAsync(n => n.Id == userId);
            ViewBag.OrderConfirm = new OrderConfirmModel
            {
                Address=user.Address,
                Phone=user.PhoneNumber,
                PostalCode=user.PostalCode
            };
            return View(products.Select(n => new CartProductModel
            {
                Id = n.Id,
                Count = n.Count,
                Image = n.Product.ImageName,
                Price = n.Product.Price,
                Title = n.Product.Title,
                CategoryTitle = n.Product.Category.Title
            }));
        }
        public async Task<IActionResult> Add(int id)
        {
            bool success = false;
            if (await _context.Products.AnyAsync(x => x.Id == id))
            {
                var userId = User.Claims.First(n => n.Type == ClaimTypes.NameIdentifier).Value;
                var product = await _context.ProductCards.FirstOrDefaultAsync(x => x.UserId == userId && !x.OrderId.HasValue && x.ProductId == id);
                if (product == null)
                {
                    await _context.ProductCards.AddAsync(new Domain.Entities.ProductCard
                    {
                        Count = 1,
                        UserId = userId,
                        ProductId = id
                    });
                }
                else
                {
                    product.Count++;
                }
                await _context.SaveChangesAsync();
                success = true;
            }
            return Json(new { Success = success });
        }
        [HttpPost]
        public async Task<IActionResult> ChangeCounts([FromBody] ChangeCountModel[] changes)
        {
            var userId = User.Claims.First(n => n.Type == ClaimTypes.NameIdentifier).Value;
            if (changes != null && changes.Length > 0)
            {
                var products = await _context.ProductCards.Where(n => n.UserId == userId).ToListAsync();
                var updatedProducts = products.Where(n => changes.Any(x => x.Id == n.Id));
                int newCount = 0;
                foreach (var item in updatedProducts)
                {
                    newCount = changes.First(n => n.Id == item.Id).Count;
                    if (newCount > 0 && newCount != item.Count)
                        item.Count = newCount;
                }
                _context.UpdateRange(updatedProducts);
                await _context.SaveChangesAsync();
            }
            return Json(new { Success = true, data = changes });
        }
        [HttpPost]
        public async Task<IActionResult> ChangeRemoves([FromBody]int[] IDs)
        {
            var userId = User.Claims.First(n => n.Type == ClaimTypes.NameIdentifier).Value;
            if (IDs != null && IDs.Length > 0)
            {
                var products = await _context.ProductCards.Where(n => n.UserId == userId).ToListAsync();
                var removedProducts = products.Where(n => IDs.Any(x => x == n.Id));
                _context.RemoveRange(removedProducts);
                await _context.SaveChangesAsync();
            }
            return Json(new { Success = true });
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(OrderConfirmModel confirmModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Claims.First(n => n.Type == ClaimTypes.NameIdentifier).Value;
                var user = await _context.Users.FirstAsync(n => n.Id == userId);
                var cartProducts = await _context.ProductCards
                    .Where(x => x.UserId == user.Id && !x.OrderId.HasValue)
                    .Include(x=>x.Product)
                    .ToListAsync();
                Order order = new Order
                {
                    Date = DateTime.Now,
                    Address = user.Address,
                    UserId = user.Id,
                    StatusId = 1,
                    PostalCode = user.PostalCode,
                    TotalPrice = cartProducts.Sum(x => x.Product.Price)
                };
                var orderAdded = await _context.AddAsync(order);
                await _context.SaveChangesAsync();
                user.PhoneNumber = confirmModel.Phone;
                user.Address = confirmModel.Address;
                user.PostalCode = confirmModel.PostalCode;
               
                _context.UpdateRange(cartProducts.Select(x => {
                    x.SoldPrice = x.Product.Price;
                    x.OrderId = order.Id;
                    return x;
                }));
                _context.Update(user);
                await _context.SaveChangesAsync();
                return LocalRedirect("/Home/Index?msg=confirmorder");
            }
            return View("Index");
        }
    }
}