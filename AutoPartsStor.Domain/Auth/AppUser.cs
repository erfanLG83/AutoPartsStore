using AutoPartsStore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Domain.Auth
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string ImageName { get; set; }
        public bool IsDeleted { get; set; }
        #region Navigation Properties
        public List<ProductCard> ProductCards { get; set; }
        public List<Order> Orders { get; set; }
        #endregion
    }
}
