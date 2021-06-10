using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoPartsStore.Domain.Entities;
using AutoPartsStore.Domain.Services;
using AutoPartsStore.Infrastructure.Repositories;
using AutoPartsStore.Infrastructure.ViewModels;
using AutoPartsStore.Persistence;
using AutoPartsStore.Services.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [Route("Products")]
        public async Task<IActionResult> Index(ProductsFilterModel filterModel)
        {
            int count = 0;
            IEnumerable<Product> products = await _context.Products
                .Where(x => !x.IsDelete && x.IsPublish && x.Stock >0)
                .ToListAsync(); ;
            if (filterModel == null)
            {
                filterModel = new ProductsFilterModel();
            }
            else
            {
                if (!string.IsNullOrEmpty(filterModel.Search))
                    products = products.Where(x => x.Title.Contains(filterModel.Search));
                if (filterModel.CategoriesId.Any())
                    products = products.Where(x => filterModel.CategoriesId.Any(n => n == x.CategoryId));
                switch ((OrderBy?) filterModel.FilterType)
                {
                    case null:
                        break;
                    case OrderBy.NoOrder:
                        break;
                    case OrderBy.RisingPrice:
                        products = products.OrderBy(x => x.Price).ToList();
                        break;
                    case OrderBy.DownwardPrice:
                        products = products.OrderByDescending(x => x.Price).ToList();
                        break;
                    case OrderBy.RisingDate:
                        products = products.OrderBy(x => x.PublishDate).ToList();
                        break;
                    case OrderBy.DownwarDate:
                        products = products.OrderByDescending(x => x.PublishDate).ToList();
                        break;
                    default:
                        break;
                }

            }
            var model = Pagination
                .GetData<Product>(products, ref count, 5, filterModel.Index)
                .Select(x => new ProductSingleModel
                {
                    Title = x.Title,
                    Price = x.Price.ToString("n0"),
                    Descrioption = x.Description,
                    Image = x.ImageName,
                    Id=x.Id
                }) ;
            count = (count % 5 == 0) ? count / 5 : (count / 5) + 1;
            string form = "";
            foreach (var item in filterModel.CategoriesId)
            {
                form += "categoriesId="+item+"&";
            }
            if (filterModel.FilterType.HasValue)
                form += "filtertype=" + filterModel.FilterType.Value;
            ViewBag.FilterModel = filterModel;
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Pagination = new Pagination(count, filterModel.Index, 5, $"/products?search={filterModel.Search}&{form}");
            return View(model);
        }
    }
}