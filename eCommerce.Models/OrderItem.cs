using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Models
{
   public class OrderItem
    {
        public int OrderItemID { get; set; }
        public int ProductID { get; set; }//gotten from the product model, and it allows you to link to the product class
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }   

}
