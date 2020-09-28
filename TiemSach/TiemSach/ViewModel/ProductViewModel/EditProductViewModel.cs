using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiemSach.Models.ProductModel;

namespace TiemSach.ViewModel.ProductViewModel
{
    public class EditProductViewModel : CreateProductViewModel
    {
        public string ProductId { get; set; }
        public IEnumerable<Image> ImagesFileName { get; set; }
    }
}
