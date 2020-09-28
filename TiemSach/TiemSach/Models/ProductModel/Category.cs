using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TiemSach.Models.ProductModel
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}

