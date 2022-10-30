using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DMSystemMvc.Models
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
        [DisplayName("Phone No.")]
        public string? PhoneNo { get; set; }
        [DisplayName("Registration Type")]
        public string? RegistrationType { get; set; }
        public string? Address { get; set; }
        public string Password { get; set; } = null!;

        public virtual ICollection<OrderDetail> OrderDetailCustomers { get; set; }
        public virtual ICollection<OrderDetail> OrderDetailExecutives { get; set; }
    }
}
