using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiemSach.Models.ProductModel;

namespace TiemSach.Models
{
    public class Cart
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

}
