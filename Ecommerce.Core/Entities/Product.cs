using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class Product
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set;} = new HashSet<OrderDetails>();

    }
}
