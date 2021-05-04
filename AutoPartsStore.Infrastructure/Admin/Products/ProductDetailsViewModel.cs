using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Infrastructure.Admin.Products
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public string ImageName { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool IsPublish { get; set; }
        public int BuyCount { get; set; }
        public string CategoryTitle { get; set; }
    }
}
