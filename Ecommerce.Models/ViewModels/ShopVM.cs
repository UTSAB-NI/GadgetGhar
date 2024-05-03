using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class ShopVM
    {
        public IEnumerable<Product> Products { get; set; }
        public FilterPrice FilterPrice { get; set; }
    }
}
