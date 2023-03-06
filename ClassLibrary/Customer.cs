using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class Customer
    {
        public Customer()
        {
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Phone { get; set; }
        public string Email { get; set; } = null!;

    }
}
