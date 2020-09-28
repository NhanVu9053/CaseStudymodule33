using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiemSach.Models.EF;

namespace TiemSach.Models.UserModel
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }
        public User Create(User user)
        {

            user.Id = Guid.NewGuid().ToString();
            user.IsDeleted = false;
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public User Edit(User user)
        {
            var editUser = context.Users.Attach(user);
            editUser.State = EntityState.Modified;
            context.SaveChanges();
            return user;
        }

        public IEnumerable<User> Get()
        {
            return from u in context.Users where u.IsDeleted == false select u;
        }

        public User Get(string id)
        {
            return (from u in context.Users where u.IsDeleted == false select u).FirstOrDefault();
        }

        public bool Remove(string id)
        {
            var userToRemove = context.Users.Find(id);
            if (userToRemove != null)
            {
                userToRemove.IsDeleted = true;
                return context.SaveChanges() > 0;
            }

            return false;
        }

    }
}
