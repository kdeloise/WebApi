using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.EF;

namespace WebApi
{
    public interface IUsersRepository
    {
        User Authenticate(string username, string password);
        IEnumerable<User> Get();
        User Get(int id);
        void Create(User item, string password);
        void Create(User item);
        void Update(User item);
        User Delete(int id);
    }
}
