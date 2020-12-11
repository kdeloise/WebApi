using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class EFUsersRepository : IUsersRepository
    {
        private EFUsersContext Context;
        public IEnumerable<User> Get()
        {
            return Context.Users;
        }
        public User Get(int Id)
        {
            return Context.Users.Find(Id);
        }
        public EFUsersRepository(EFUsersContext context)
        {
            Context = context;
        }
        public void Create(User item)
        {
            Context.Users.Add(item);
            Context.SaveChanges();
        }
        public void Update(User updatedUserItem)
        {
            User currentItem = Get(updatedUserItem.Id);
            currentItem.Name = updatedUserItem.Name;
            currentItem.Nickname = updatedUserItem.Nickname;

            Context.Users.Update(currentItem);
            Context.SaveChanges();
        }
        public User Delete(int id)
        {
            User user = Get(id);

            if (user != null)
            {
                Context.Users.Remove(user);
                Context.SaveChanges();
            }
            return user;
        }
    }
}
