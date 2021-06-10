using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoPartsStore.Domain.Entities;
using AutoPartsStore.Domain.Services;
using AutoPartsStore.Infrastructure.Admin.OrdersManager;
using AutoPartsStore.Infrastructure.Repositories;
using AutoPartsStore.Persistence;
using AutoPartsStore.Services.Contract;
using AutoPartsStore.Services.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class OrdersManagerController : Controller
    {
        private readonly IFileWorker _fileWorker;
        private readonly IRepositoryBase<Order> _repository;
        private readonly ApplicationDbContext _context;
        public OrdersManagerController(ApplicationDbContext context, IFileWorker fileWorker)
        {
            _fileWorker = fileWorker;
            _context = context;
            _repository = new RepositoryBase<Order>(context);
        }
        public async Task<IActionResult> Index(int index = 1)
        {
            int count = 0;
            var orders = Pagination.GetData<Order>(await _repository.FindAllAsync(false), ref count, 5, index);
            count = (count % 5 == 0) ? count / 5 : (count / 5) + 1;
            int row = (5 * (index - 1)) +1;
            ViewBag.Pagination = new Pagination(count, index, 5, "/admin/ordersmanager");
            Expression<Func<Order, object>>[] expressions = new Expression<Func<Order, object>>[]
            {
                n => n.OrderStatus,
                n => n.User
            };
            ViewBag.Statuses = await _context.Statuses.ToListAsync();
            orders = await _repository.GetAllReferencePropertyAsync(orders, expressions);
            //orders = await _repository.GetAllCollectionPropertyAsync(orders, n => n.ProductCards);
            return View(
                orders.Select(n=>new OrdersManagerIndexViewModel(n,row++))
                );
        }
        public async Task<IActionResult> ChangeStatus(int id , int statusId)
        {
            JsonResult failedResult = new JsonResult(new { Success = false });
            var order = await _repository.FindByIDAsync(id);
            if (order == null)
                return failedResult;
            var status = await _context.Statuses.FindAsync(statusId);
            if (status == null)
                return failedResult;
            order.StatusId = statusId;
            _repository.Update(order);
            await _context.SaveChangesAsync();
            return Json(new
            {
                Success = true,
                StatusText = status.Text
            });
        }
        public async Task<IActionResult> Details(int id)
        {
            var order = await _repository.FindByIDAsync(id);
            if (order == null)
                return NotFound();

            Expression<Func<Order, object>>[] expressions = new Expression<Func<Order, object>>[]
            {
                n => n.OrderStatus,
                n => n.User
            };
            order = await _repository.GetReferencePropertyAsync(order, expressions);
            order.ProductCards = await _context.ProductCards
                .Include(n => n.Product)
                .Where(n => n.OrderId.HasValue && n.OrderId == id).ToListAsync();
            return View(new OrderDetailsViewModel(order));
        }
    }
}