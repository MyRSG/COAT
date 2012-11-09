using System.Linq;
using COAT.Models;

namespace COAT.Database
{
    public class UserEntityManager : BaseEntityManager<User>
    {
        public IQueryable<User> GetAllUsers()
        {
            return Entities.Users.Where(u => u.IsDelete == null || u.IsDelete == false);
        }

        public User FindUserByName(string name)
        {
            return GetAllUsers().FirstOrDefault(u => u.Name == name);
        }

        public User FindUserByEmail(string email)
        {
            return GetAllUsers().FirstOrDefault(u => u.Email == email);
        }

        public User CreateUser(string name, string password, string email, SystemRole sysRole, BusinessRole busRole)
        {
            return CreateUser(name, password, email, sysRole.Id, busRole.Id);
        }

        public User CreateUser(string name, string password, string email, int sysRoleId, int busRoleId)
        {
            User user = Entities.Users.CreateObject();
            user.Name = name;
            user.Password = password;
            user.Email = email;
            user.SystemRoleId = sysRoleId;
            user.BusinessRoleId = busRoleId;

            User rslt = AddEntityObjectFor(Entities.Users.EntitySet.Name, user);
            Synchronize();

            return rslt;
        }

        public User UpdateUser(User user)
        {
            User rslt = UpdateEntityObjectFor(user);
            Synchronize();

            return rslt;
        }

        public bool DeleteUser(User user)
        {
            bool rslt = DeleteEntityObjectFor(user);
            Synchronize();

            return rslt;
        }
    }
}