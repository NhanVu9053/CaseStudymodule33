using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiemSach.Models.EF;

namespace TiemSach.Models.OrderModel
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext context;

        public CustomerRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Customer Create(Customer customer)
        {
            customer.CustomerId = Guid.NewGuid().ToString();
            context.Customers.Add(customer);
            context.SaveChanges();
            return customer;
        }

        public Customer Edit(Customer customer)
        {
            var editCustomer = context.Customers.Attach(customer);
            editCustomer.State = EntityState.Modified;
            context.SaveChanges();
            return customer;
        }

        public IEnumerable<Customer> Get()
        {
            return context.Customers;
        }

        public Customer Get(string id)
        {
            var customer = (from e in context.Customers
                            where e.CustomerId == id
                            select e).FirstOrDefault();
            return customer;
        }

        public bool Remove(string id)
        {
            var customerToRemove = context.Customers.Find(id);
            if (customerToRemove != null)
            {
                context.Customers.Remove(customerToRemove);
                return context.SaveChanges() > 0;
            }

            return false;
        }
    }
}
