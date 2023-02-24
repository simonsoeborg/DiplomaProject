using System;
using System.Collections.Generic;

namespace ClassLibrary.EFModels
{
    public partial class Payment
    {
        public Payment()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public DateOnly? DatePaid { get; set; }
        public double? Amount { get; set; }
        public sbyte? Approved { get; set; }
        public string? Method { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
