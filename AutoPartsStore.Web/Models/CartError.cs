using AutoPartsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoPartsStore.Web.Models
{
    public class CartError
    {
        public CartError()
        {

        }
        public string ProductTitle { get; set; }
        public int ProductStock { get; set; }
        public int OrderCount { get; set; }
    }
}
