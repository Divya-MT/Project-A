using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DMSystemMvc.Models
{
    public partial class OrderDetail
    {
        [DisplayName("Order Id")]
        public int OrderId { get; set; }

        [DisplayName("Customer")]
        public int CustomerId { get; set; }

        [DisplayName("Executive")]
        public int ExecutiveId { get; set; }

        [DisplayName("Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DeliveryDate { get; set; }

        [DisplayName("Time Of Pickup")]
        public DateTime? TimeOfPickup { get; set; }

        [DisplayName("Package Weight in Grams")]
        public decimal? WeightOfPackage { get; set; }

        public decimal? Price { get; set; }

        public virtual Registration Customer { get; set; } = null!;
        public virtual Registration Executive { get; set; } = null!;
    }
}
