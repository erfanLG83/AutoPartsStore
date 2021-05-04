using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AutoPartsStore.Infrastructure.Admin.Products
{
    public class ProductCreateDTO
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(75,ErrorMessage ="لطفا {0} را کمتر از 75 کرکتر وارد کنید")]
        public string Title { get; set; }
        [Display(Name = "توضیح خلاصه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "لطفا {0} را کمتر از 250 کرکتر وارد کنید")]
        public string Description { get; set; }
        [Display(Name = "تعداد موجود")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Stock { get; set; }
        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }
        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "لطفا {0} را انتخاب کنید")]
        public int CategoryId { get; set; }
        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را انتخاب کنید")]
        public IFormFile Image { get; set; }
        public bool IsPublish { get; set; }
    }
}
