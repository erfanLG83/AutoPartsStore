using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoPartsStore.Domain.Entities;
using AutoPartsStore.Infrastructure.Admin.Products;
using AutoPartsStore.Infrastructure.Repositories;
using AutoPartsStore.Persistence;
using AutoPartsStore.Services.Features;
using Microsoft.AspNetCore.Mvc;
using PrgHome.DataLayer.Repository;

namespace AutoPartsStore.Web.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IRepositoryBase<Product> _repository;
        public ProductsController(ApplicationDbContext context)
        {
            _repository = new RepositoryBase<Product>(context);
        }
        public async Task<IActionResult> Index(int index=1)
        {
            int count = 0;
            var products = Pagination.GetData<Product>(await _repository.FindAllAsync(false),ref count,5,index);
            count = (count % 5 == 0) ? count / 5 : (count / 5) + 1;
            ViewBag.Pagination = new Pagination(count,index,5,"/admin/products");
            products = await _repository.GetAllReferencePropertyAsync(products, n => n.Category);
            return View(
                    products.Select(n=>new ProductsIndexViewModel
                    {
                        CategoryTitle=n.Category.Title,
                        Id=n.Id,
                        Stock=n.Stock,
                        Price=n.Price,
                        Name=n.Title
                    })
                );
        }
    }
}