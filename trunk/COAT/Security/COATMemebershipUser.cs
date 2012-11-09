using System;
using System.Web.Security;
using COAT.Models;

namespace COAT.Security
{
    public class COATMemebershipUser : MembershipUser
    {
        public COATMemebershipUser(User user)
            : base("COATMembershipProvider",
                   user.Name,
                   null,
                   user.Email,
                   null,
                   null,
                   user.IsDelete == null || (bool) user.IsDelete,
                   false,
                   DateTime.Now,
                   DateTime.Now,
                   DateTime.Now,
                   DateTime.Now,
                   DateTime.Now)
        {
            Id = user.Id;
            COATUser = user;
        }

        public User COATUser { get; set; }
        public int Id { get; set; }

        public SystemRole SystemRole
        {
            get { return COATUser.SystemRole; }
        }

        public BusinessRole BusinessRole
        {
            get { return COATUser.BusinessRole; }
        }
    }
}