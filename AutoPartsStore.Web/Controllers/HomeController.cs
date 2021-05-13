using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoPartsStore.Web.Models;
using AutoPartsStore.Persistence;
using AutoPartsStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AutoPartsStore.Infrastructure.ViewModels;
using AutoPartsStore.Domain.Services;
using AutoPartsStore.Infrastructure.Repositories;

namespace AutoPartsStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepositoryBase<Product> _repository;

        public HomeController(ILogger<HomeController> logger , ApplicationDbContext context)
        {
            _context = context;
            _repository = new RepositoryBase<Product>(context);
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _repository.FindByConditionAsync(n => n.IsPublish && !n.IsDelete, n => n.OrderBy(m => m.PublishDate));
            return View(products.Select(
                n=>new ProductHomePageModel { 
                    Id = n.Id,
                    Price = n.Price,
                    Title =n.Title,
                    Image = n.ImageName
                }));
        }
    }
}
