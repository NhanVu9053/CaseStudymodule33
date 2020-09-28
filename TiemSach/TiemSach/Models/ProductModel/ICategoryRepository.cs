using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiemSach.Models.ProductModel
{
  public  interface ICategoryRepository
    {
        IEnumerable<Category> Get();

        Category Get(int id);

        Category Create(Category category);

        Category Edit(Category category);

        bool Remove(int id);
    }
}
