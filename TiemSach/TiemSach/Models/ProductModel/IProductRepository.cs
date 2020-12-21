using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiemSach.ViewModel;

namespace TiemSach.Models.ProductModel
{
  public  interface IProductRepository
    {
        IEnumerable<Product> Get();

        ProductDetailViewModel Get(string id);

        Product Create(Product product);

        Product Edit(Product product);

        bool Remove(string id);
        List<Product> Search(string name);
    }
}
