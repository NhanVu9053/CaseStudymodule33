using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiemSach.Models.OrderModel
{
  public  interface IOrderDetailRepository
    {
        IEnumerable<OrderDetail> Get();

        IEnumerable<OrderDetail> Get(string orderId);

        OrderDetail Create(OrderDetail orderDetail);

        OrderDetail Edit(OrderDetail orderDetail);

        bool Remove(string orderId);
    }
}
