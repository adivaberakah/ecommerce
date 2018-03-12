using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Models
{
    public class Basket
    {
        [Key]
        public Guid BasketId { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<BasketItem> BasketItems { get; set; }
    }
}
