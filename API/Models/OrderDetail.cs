using System;
using System.Collections.Generic;

namespace DeliveryManagementSystemApi.Models
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int ExecutiveId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime TimeOfPickup { get; set; }
        public decimal WeightOfPackage { get; set; }
        public decimal Price { get; set; }

        public virtual Registration Customer { get; set; } = null!;
        public virtual Registration Executive { get; set; } = null!;
    }
}
