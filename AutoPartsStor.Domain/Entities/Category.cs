using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        #region Navigation Properties
        public List<Product> Products { get; set; }
        #endregion
    }
}
