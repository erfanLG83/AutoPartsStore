using AutoPartsStore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AutoPartsStore.Infrastructure.Admin.Products
{
    public class ProductEditDTO : ProductCreateDTO
    {
        public int Id { get; set; }
        public new IFormFile Image { get; set; }
        [Display(Name = "تصویر محصول")]
        public IFormFile EditImage { get; set; }
    }
}
