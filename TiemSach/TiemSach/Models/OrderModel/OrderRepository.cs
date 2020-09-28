using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiemSach.Models.EF;

namespace TiemSach.Models.OrderModel
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext context;

        public OrderRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Order Create(Order order)
        {
            order.OrderId = Guid.NewGuid().ToString();
            context.Orders.Add(order);
            context.SaveChanges();
            return order;
        }

        public Order Edit(Order order)
        {
            var editOrder = context.Orders.Attach(order);
            editOrder.State = EntityState.Modified;
            context.SaveChanges();
            return order;
        }

        public IEnumerable<Order> Get()
        {
            return context.Orders; ;
        }

        public Order Get(string id)
        {
            var order = (from e in context.Orders
                         where e.OrderId == id
                         select e).FirstOrDefault();
            return order;
        }

        public bool Remove(string id)
        {
            var orderToRemove = context.Orders.Find(id);
            if (orderToRemove != null)
            {
                context.Orders.Remove(orderToRemove);
                return context.SaveChanges() > 0;
            }

            return false;
        }
    }
}
