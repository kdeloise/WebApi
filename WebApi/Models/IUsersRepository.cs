using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi
{
    public interface IUsersRepository
    {
        IEnumerable<User> Get();
        User Get(int id);
        void Create(User item);
        void Update(User item);
        User Delete(int id);
    }
}
