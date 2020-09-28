using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiemSach.Models.UserModel
{
   public interface IUserRepository
    {
        IEnumerable<User> Get();

        User Get(string id);

        User Create(User product);

        User Edit(User product);

        bool Remove(string id);
    }
}
