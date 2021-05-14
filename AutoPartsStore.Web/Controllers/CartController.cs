using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoPartsStore.Infrastructure.ViewModels.Cart;
using AutoPartsStore.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsStore.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var products = await _context.ProductCards.Include(n=>n.Product).ThenInclude(n=>n.Category).Where(n=>n.UserId==userId&&!n.OrderId.HasValue).ToListAsync();
            ViewBag.TotalPrice = products.Sum(n => n.Product.Price * n.Count).ToString("n0") +" ریال";
            return View(products.Select(n=>new CartProductModel { 
                Id = n.Id,
                Count = n.Count,
                Image = n.Product.ImageName,
                Price = n.Product.Price,
                Title = n.Product.Title,
                CategoryTitle = n.Product.Category.Title
            }));
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
                    if(newCount >0 && newCount != item.Count)
                        item.Count = newCount;
                }
                _context.UpdateRange(updatedProducts);
                await _context.SaveChangesAsync();
            }
            return Json(new { Success = true ,data=changes});
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
    }
}