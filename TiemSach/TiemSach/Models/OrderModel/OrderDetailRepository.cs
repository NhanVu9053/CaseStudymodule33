using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiemSach.Models.EF;

namespace TiemSach.Models.OrderModel
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly AppDbContext context;

        public OrderDetailRepository(AppDbContext context)
        {
            this.context = context;
        }
        public OrderDetail Create(OrderDetail orderDetail)
        {
            orderDetail.OrderDetailId = Guid.NewGuid().ToString();
            context.OrderDetails.Add(orderDetail);
            context.SaveChanges();
            return orderDetail;
        }

        public OrderDetail Edit(OrderDetail orderDetail)
        {
            var editOrderDetail = context.OrderDetails.Attach(orderDetail);
            editOrderDetail.State = EntityState.Modified;
            context.SaveChanges();
            return orderDetail;
        }

        public IEnumerable<OrderDetail> Get()
        {
            return context.OrderDetails;
        }

        public IEnumerable<OrderDetail> Get(string orderId)
        {
            var orderDetails = (from o in context.OrderDetails where o.OrderId == orderId select o).ToList();
            return orderDetails;
        }

        public bool Remove(string orderId)
        {
            var orderDetailsToRemove = (from o in context.OrderDetails where o.OrderId == orderId select o).ToList();
            if (orderDetailsToRemove.Count > 0)
            {
                foreach (var o in orderDetailsToRemove)
                {
                    context.OrderDetails.Remove(o);
                    context.Products.Find(o.ProductId).Remain += o.Quantity;
                }

                context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
