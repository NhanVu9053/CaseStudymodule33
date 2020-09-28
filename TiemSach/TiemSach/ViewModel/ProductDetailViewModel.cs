using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiemSach.Models.ProductModel;

namespace TiemSach.ViewModel
{
    public class ProductDetailViewModel
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
      
        public int? Remain { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<Image> Images { get; set; }
        public string Description { get; set; }
        public string publisher { get; set; }
        public DateTime DoP { get; set; }
    }
}
