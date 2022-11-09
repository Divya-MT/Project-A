using System;
using System.Collections.Generic;

namespace DeliveryManagementSystemApi.Models
{
    public partial class Registration
    {
        public Registration()
        {
            OrderDetailCustomers = new HashSet<OrderDetail>();
            OrderDetailExecutives = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? Age { get; set; }
        public string? PhoneNo { get; set; }
        public string? RegistrationType { get; set; }
        public string? Address { get; set; }
        public string Password { get; set; } = null!;

        public virtual ICollection<OrderDetail> OrderDetailCustomers { get; set; }
        public virtual ICollection<OrderDetail> OrderDetailExecutives { get; set; }
    }
}
