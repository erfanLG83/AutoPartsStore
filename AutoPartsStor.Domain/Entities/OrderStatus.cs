using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Domain.Entities
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public string Text { get; set; }
        #region Navigation Properties
        public List<Order> Orders { get; set; }
        #endregion
    }
}
