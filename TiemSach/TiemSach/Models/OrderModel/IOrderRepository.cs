using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiemSach.Models.OrderModel
{
  public  interface IOrderRepository
    {
        IEnumerable<Order> Get();

        Order Get(string id);

        Order Create(Order order);

        Order Edit(Order order);

        bool Remove(string id);
    }
}
