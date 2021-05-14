using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Infrastructure.ViewModels.Cart
{
    public class CartProductModel:ProductHomePageModel
    {
        public int Count { get; set; }
        public string CategoryTitle { get; set; }
    }
}
