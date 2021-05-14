using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoPartsStore.Domain.Entities;
using AutoPartsStore.Domain.Services;
using AutoPartsStore.Infrastructure.Repositories;
using AutoPartsStore.Infrastructure.ViewModels;
using AutoPartsStore.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace AutoPartsStore.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepositoryBase<Product> _repository;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
            _repository = new RepositoryBase<Product>(context);
        }
        private bool AnyCommonWord(string str, IEnumerable<string> secoend)
            => str.Split(' ').Intersect(secoend).Any();
        public async Task<IActionResult> Details(int id)
        {
            var product = await _repository.FindByIDAsync(id);
            if (product == null)
                return NotFound();
            product = await _repository.GetReferencePropertyAsync(product, n=>n.Category);
            var wordsInTitle = product.Title.Split(' ');
            var products = await _repository.FindByConditionAsync(n=>!n.IsDelete&&n.IsPublish&&n.Id!=id);
            ViewBag.OtherProducts = products.Where(n =>
                n.CategoryId==product.CategoryId||
                AnyCommonWord(n.Title,wordsInTitle)||
                AnyCommonWord(n.Description,wordsInTitle)
            ).Take(6).Select(p=>new ProductHomePageModel { 
                Id=p.Id,
                Image=p.ImageName,
                Price=p.Price,
                Title=p.Title
            });
            return View(new ProductSingleModel { 
                Id=product.Id,
                CategoryId = product.CategoryId,
                CategoryTitle = product.Category.Title,
                Descrioption = product.Description,
                Image = product.ImageName,
                Price = product.Price.ToString("n0"),
                Title = product.Title,
                Stock = product.Stock
            });
        }
    }
}