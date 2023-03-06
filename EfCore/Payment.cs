using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class Payment
    {
        public Payment()
        {
        }

        public int Id { get; set; }
        public DateOnly? DatePaid { get; set; }
        public double? Amount { get; set; }
        public sbyte? Approved { get; set; }
        public string? Method { get; set; }

    }
}
