using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class FilterPrice
    {
        public string CategoryName { get; set; }
        public int FromPrice { get; set; }
        public int ToPrice { get; set; }
    }
}
