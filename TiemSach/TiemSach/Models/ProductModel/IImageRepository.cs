using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiemSach.Models.ProductModel
{
  public  interface IImageRepository
    {
        Image Get(string id);

        Image Create(Image image);

        bool Remove(string id);
    }
}
