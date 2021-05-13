using AutoPartsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Infrastructure.Admin.OrdersManager
{
    public class OrdersManagerIndexViewModel
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public string UserName { get; set; }
        public string ImageName { get; set; }
        public string Status { get; set; }
        public string TotalPrice { get; set; }
        public OrdersManagerIndexViewModel(Order order,int row)
        {
            Id = order.Id;
            Row = row;
            UserName = order.User.UserName;
            ImageName = order.User.ImageName;
            Status = order.OrderStatus?.Text;
            TotalPrice = order.TotalPrice.ToString("n0");
        }
    }
}
