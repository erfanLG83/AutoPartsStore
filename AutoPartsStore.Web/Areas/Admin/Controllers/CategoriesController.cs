using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoPartsStore.Domain.Entities;
using AutoPartsStore.Infrastructure.Repositories;
using AutoPartsStore.Persistence;
using Microsoft.AspNetCore.Mvc;
using AutoPartsStore.Domain.Services;
using AutoPartsStore.Services.Features;
using AutoPartsStore.Infrastructure.Admin.Categories;
using Microsoft.AspNetCore.Authorization;

namespace AutoPartsStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="مدیر")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepositoryBase<Category> _categoryRep;
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
            _categoryRep = new RepositoryBase<Category>(context);
        }
        public async Task<IActionResult> Index(int index=1)
        {
            var m = new Category();
            int count = 0;
            var categories = Pagination.GetData<Category>(await _categoryRep.FindAllAsync(false),ref count,5,index);
            count = (count % 5 == 0) ? count / 5 : (count / 5) + 1;
            int row = 5 * (index - 1);
            ViewBag.Pagination = new Pagination(count, index, 5, "/admin/categories");
            return View(
                    categories.Select(n => new CategoriesIndexViewModel
                    {
                        Row = ++row,
                        Title = n.Title,
                        Id = n.Id,
                    })
                );
        }
        public async Task<IActionResult> Create(string name)
        {
            if (String.IsNullOrEmpty(name))
                return Json(new { 
                    Success=false
                });
            var category = new Category { Title = name };
            await _categoryRep.CreateAsync(category);
            await _context.SaveChangesAsync();
            return Json(new { 
                Success=true,
                Id=category.Id
            });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRep.FindByIDAsync(id);
            if (category == null)
                return Json(new { Success = false });
            _categoryRep.Delete(category);
            await _context.SaveChangesAsync();
            return Json(new
            {
                Success = true
            }) ;
        }
        public async Task<IActionResult> Edit(int id,string name)
        {
            if (String.IsNullOrEmpty(name))
                return Json(new
                {
                    Success = false
                });
            var category = await _categoryRep.FindByIDAsync(id);
            if(category == null)
                return Json(new
                {
                    Success = false
                });
            category.Title = name;
            await _context.SaveChangesAsync();
            return Json(new
            {
                Success=true
            });
        }
    }
}