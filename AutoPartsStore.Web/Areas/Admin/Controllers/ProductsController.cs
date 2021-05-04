using AutoPartsStore.Domain.Entities;
using AutoPartsStore.Infrastructure.Admin.Products;
using AutoPartsStore.Infrastructure.Repositories;
using AutoPartsStore.Persistence;
using AutoPartsStore.Services.Contract;
using AutoPartsStore.Services.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrgHome.DataLayer.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoPartsStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IFileWorker _fileWorker;
        private readonly IRepositoryBase<Product> _repository;
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context , IFileWorker fileWorker)
        {
            _fileWorker = fileWorker;
            _context = context;
            _repository = new RepositoryBase<Product>(context);
        }
        private void SetCategories()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Title");
        }
        public async Task<IActionResult> Index(int index = 1)
        {
            int count = 0;
            var products = Pagination.GetData<Product>(await _repository.FindByConditionAsync(n => !n.IsDelete), ref count, 5, index);
            count = (count % 5 == 0) ? count / 5 : (count / 5) + 1;
            int row = 5 * (index - 1);
            ViewBag.Pagination = new Pagination(count, index, 5, "/admin/products");
            products = await _repository.GetAllReferencePropertyAsync(products, n => n.Category);
            return View(
                    products.Select(n => new ProductsIndexViewModel
                    {
                        Row = ++row,
                        CategoryTitle = n.Category.Title,
                        Id = n.Id,
                        Stock = n.Stock,
                        Price = n.Price,
                        Name = n.Title,
                        ImageName = n.ImageName,
                        IsPublish = n.IsPublish
                    })
                );
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _repository.FindByIDAsync(id);
            if (product == null)
                return NotFound();
            product = await _repository.GetReferencePropertyAsync(product, m => m.Category);
            product = await _repository.GetCollectionPropertyAsync(product, n => n.ProductCards);
            int buyCount = 0;
            foreach (var item in product.ProductCards.Where(n => n.OrderId.HasValue))
            {
                buyCount += item.Count;
            }
            return View(
                new ProductDetailsViewModel
                {
                    Title = product.Title,
                    Description = product.Description,
                    Stock = product.Stock,
                    BuyCount = buyCount,
                    CategoryTitle = product.Category.Title,
                    Id = product.Id,
                    ImageName = product.ImageName,
                    IsPublish = product.IsPublish,
                    Price = product.Price,
                    PublishDate = product.PublishDate
                }
                );
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _repository.FindByIDAsync(id);
            if (product == null)
                return Json(new { Success = false });
            product.IsDelete = true;
            _repository.Update(product);
            await _context.SaveChangesAsync();
            if(product.ImageName!=null)
                _fileWorker.RemoveFileInPath("/img/" + product.ImageName);
            return Json(new { Success = true });
        }
        public IActionResult Create()
        {
            SetCategories();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDTO createDTO)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product
                {
                    CategoryId=createDTO.CategoryId,
                    Stock=createDTO.Stock,
                    Description=createDTO.Description,
                    IsPublish=createDTO.IsPublish,
                    Price=createDTO.Price,
                    Title=createDTO.Title
                };
                product.ImageName = await _fileWorker.AddFileToPath(createDTO.Image, "img");
                if (product.IsPublish)
                    product.PublishDate = DateTime.Now;
                await _repository.CreateAsync(product);
                await _context.SaveChangesAsync();
                return LocalRedirect("/admin/products/index");
            }
            SetCategories();
            return View(createDTO);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _repository.FindByIDAsync(id);
            if (product == null)
                return NotFound();
            SetCategories();
            return View(new ProductEditDTO
            {
                CategoryId=product.CategoryId,
                Title=product.Title,
                Stock=product.Stock,
                Description=product.Description,
                IsPublish=product.IsPublish,
                Id=product.Id,
                Price=product.Price
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditDTO editDTO)
        {
            if (ModelState.IsValid)
            {

                Product product = await _repository.FindByIDAsync(editDTO.Id);
                if (editDTO.EditImage != null)
                {
                    if (product.ImageName != null)
                        _fileWorker.RemoveFileInPath("/img/" + product.ImageName);
                    product.ImageName = await _fileWorker.AddFileToPath(editDTO.EditImage, "img");
                }
                if (!editDTO.IsPublish)
                    product.PublishDate = null;
                else if (!product.IsPublish)
                    product.PublishDate = DateTime.Now;
                product.IsPublish = editDTO.IsPublish;
                product.Price = editDTO.Price;
                product.Stock = editDTO.Stock;
                product.Title = editDTO.Title;
                product.Description = editDTO.Description;
                product.CategoryId = editDTO.CategoryId;
                _repository.Update(product);
                await _context.SaveChangesAsync();
                return LocalRedirect("/admin/products/index");
            }
            SetCategories();
            return View(editDTO);
        }
    }
}