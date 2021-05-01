using AutoPartsStore.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Domain.Entities
{
    public class ProductCard
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int? OrderId { get; set; }
        public int Count { get; set; }
        #region Navigation Properties
        public AppUser User { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
        #endregion
    }
}
