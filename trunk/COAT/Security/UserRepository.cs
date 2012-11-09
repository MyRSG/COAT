using System.Web.Security;
using COAT.Database;
using COAT.Models;
using COAT.Util;
using COAT.Util.Mail;

namespace COAT.Security
{
    internal class UserRepository
    {
        private readonly UserEntityManager _userMgr = new UserEntityManager();

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!ValidateUser(username, oldPassword))
                return false;

            User user = _userMgr.FindUserByName(username);
            user.Password = newPassword;
            _userMgr.UpdateUser(user);

            return user.Password == newPassword;
        }

        public MembershipUser CreateUser(string username, string password, string email, string passwordQuestion,
                                         string passwordAnswer, bool isApproved, object providerUserKey,
                                         out MembershipCreateStatus status)
        {
            User user = _userMgr.CreateUser(username, password, email, 1, 1);
            var member = new COATMemebershipUser(user);
            status = MembershipCreateStatus.Success;
            return member;
        }

        public MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var collection = new MembershipUserCollection();
            foreach (User user in _userMgr.GetAllUsers())
            {
                collection.Add(new COATMemebershipUser(user));
            }

            totalRecords = collection.Count;
            return collection;
        }

        public string GetPassword(string username, string answer)
        {
            return _userMgr.FindUserByName(username).Password;
        }

        public MembershipUser GetUser(string username, bool userIsOnline)
        {
            User user = _userMgr.FindUserByName(username);
            user = user ?? _userMgr.FindUserByEmail(username);
            return new COATMemebershipUser(user);
        }

        public string GetUserNameByEmail(string email)
        {
            return _userMgr.FindUserByEmail(email).Name;
        }

        public string ResetPassword(string username, string answer)
        {
            User user = _userMgr.FindUserByName(username);
            user.Password = PasswordUtil.CreatePassword();
            _userMgr.UpdateUser(user);

            new COATMailHelper().SendMail(new[] {user.Email}, null,
                                          "Your password in COAT is reset!",
                                          user.Name + ": You password in COAT is reset! \r\nPlease use new password(" +
                                          user.Password +
                                          ")to login to COAT.\r\nYou can change the password after you login.");

            return user.Password;
        }

        public bool ValidateUser(string username, string password)
        {
            User user = _userMgr.FindUserByName(username);
            user = user ?? _userMgr.FindUserByEmail(username);

            if (user == null)
            {
                return false;
            }

            return user.Password == password;
        }
    }
}