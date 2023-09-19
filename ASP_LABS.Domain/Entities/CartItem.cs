using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_LABS.Domain.Entities
{
    public class CartItem
    {
        public Book? Item { get; set; }
        public int Count { get; set; }
    }
}
