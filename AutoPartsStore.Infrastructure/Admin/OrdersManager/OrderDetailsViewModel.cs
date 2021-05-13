using AutoPartsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoPartsStore.Infrastructure.Admin.OrdersManager
{
    public class OrderDetailsViewModel
    {
        public class ProductOrder
        {
            public int Count { get; set; }
            public string Price { get; set; }
            public string ImageName { get; set; }
            public int Row { get; set; }
            public string Title { get; set; }
            public ProductOrder(ProductCard product,int row)
            {
                Title = product.Product.Title;
                Row = row;
                Count = product.Count;
                Price = product.SoldPrice?.ToString("n0");
                ImageName = product.Product.ImageName;
            }
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ImageName { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public string TotalPrice { get; set;}
        public List<ProductOrder> Products { get; set; }
        public OrderDetailsViewModel(Order order)
        {
            Id = order.Id;
            Date = order.Date.ToString("yyyy/MM/dd");
            UserName = order.User.UserName;
            ImageName = order.User.ImageName;
            Status = order.OrderStatus?.Text;
            Products = new List<ProductOrder>();
            int row = 1;
            foreach (var item in order.ProductCards)
            {
                Products.Add(new ProductOrder(item,row));
                row++;
            }
            TotalPrice = order.TotalPrice.ToString("n0");
        }
    }

}
