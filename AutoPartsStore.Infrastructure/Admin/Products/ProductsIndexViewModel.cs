using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Infrastructure.Admin.Products
{
    public class ProductsIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryTitle { get; set; }
        public int Stock { get; set; }
        public long Price { get; set; }
        public string ImageName { get; set; }
    }
}
