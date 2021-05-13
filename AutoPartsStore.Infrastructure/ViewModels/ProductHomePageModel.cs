using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Infrastructure.ViewModels
{
    public class ProductHomePageModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public long Price { get; set; }
    }
}
