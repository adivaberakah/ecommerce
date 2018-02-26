using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }

        public int CustomerID { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }// it is used to link the order items to the order
    }
}
