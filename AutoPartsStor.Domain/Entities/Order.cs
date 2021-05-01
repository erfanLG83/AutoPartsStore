using AutoPartsStore.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public long TotalPrice { get; set; }
        #region Navigation Properties
        public AppUser User { get; set; }
        public List<ProductCard> ProductCards { get; set; }
        #endregion
    }
}
