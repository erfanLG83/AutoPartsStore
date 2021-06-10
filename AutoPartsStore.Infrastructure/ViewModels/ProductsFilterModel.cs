using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Infrastructure.ViewModels
{
    public enum OrderBy
    {
        NoOrder,
        RisingPrice,
        DownwardPrice,
        RisingDate,
        DownwarDate
    }
    public class ProductsFilterModel
    {
        public int Index { get; set; } = 1;
        public string Search { get; set; }
        public int? FilterType { get; set; }
        public List<int> CategoriesId { get; set; } = new List<int>();
    }
}
