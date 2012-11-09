using System.Linq;
using COAT.Models;

namespace COAT.Database
{
    public class RoleEntityManager : BaseEntityManager<SystemRole>
    {
        public IQueryable<SystemRole> GetAllRoles()
        {
            return Entities.SystemRoles;
        }

        public SystemRole GetRole(int id)
        {
            return Entities.SystemRoles.FirstOrDefault(r => r.Id == id);
        }

        public SystemRole GetRole(string roleName)
        {
            return Entities.SystemRoles.FirstOrDefault(r => r.Name == roleName);
        }
    }
}