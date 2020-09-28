using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TiemSach.Models.ProductModel
{
    public class Image
    {
        [Required] public string ImageId { get; set; }

        public string ImageName { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
