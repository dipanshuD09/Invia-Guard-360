using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Models
{
    public class User
    {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public byte IsCustomer { get; set; }
            public byte IsAdmin { get; set; }
            public byte IsEmployee { get; set; }
            public int CustomerId { get; set; }
            public int TransactionId { get; set; }
            public byte IsActive { get; set; }
            public Customer Customer { get; set; } // Navigation property for the Customer
            public Transaction Transaction { get; set; } // Navigation property for the Transaction
        }

}
