using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Infrastructure.ViewModels
{
    public class ProductSingleModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CategoryTitle { get; set; }
        public int CategoryId { get; set; }
        public string Descrioption { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
    }
}
