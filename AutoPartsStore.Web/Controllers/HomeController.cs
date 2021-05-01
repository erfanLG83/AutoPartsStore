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

namespace AutoPartsStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger , ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Product> Index()
        {
            return _context.Products.ToList();
        }
    }
}
