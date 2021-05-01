using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public string ImageName { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool IsPublish { get; set; }
        public bool IsDelete { get; set; }

        public int CategoryId { get; set; }
        #region Navigation Properties
        public Category Category { get; set; }
        public List<ProductCard> ProductCards { get; set; }
        #endregion
    }
}
