using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;
using System.Web.Security;
using COAT.Database;
using COAT.Util;

namespace COAT.Security
{
    internal class UserRepository
    {
        UserEntityManager userMgr = new UserEntityManager();

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!ValidateUser(username, oldPassword))
                return false;

            var user = userMgr.FindUserByName(username);
            user.Password = newPassword;
            userMgr.UpdateUser(user);

            return user.Password == newPassword;

        }

        public MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            var user = userMgr.CreateUser(username, password, email, 1, 1);
            var member = new COATMemebershipUser(user);
            status = MembershipCreateStatus.Success;
            return member;
        }

        public MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var collection = new MembershipUserCollection();
            foreach (var user in userMgr.GetAllUsers())
            {
                collection.Add(new COATMemebershipUser(user));
            }

            totalRecords = collection.Count;
            return collection;
        }

        public string GetPassword(string username, string answer)
        {
            return userMgr.FindUserByName(username).Password;
        }

        public MembershipUser GetUser(string username, bool userIsOnline)
        {
            var user = userMgr.FindUserByName(username);
            user = user ?? userMgr.FindUserByEmail(username);
            return new COATMemebershipUser(user);
        }

        public string GetUserNameByEmail(string email)
        {
            return userMgr.FindUserByEmail(email).Name;
        }

        public string ResetPassword(string username, string answer)
        {
            var user = userMgr.FindUserByName(username);
            user.Password = PasswordUtil.CreatePassword();
            userMgr.UpdateUser(user);

            new Mail.COATMailHelper().SendMail(new string[] { user.Email }, null,
                "Your password in COAT is reset!",
                user.Name + ": You password in COAT is reset! \r\nPlease use new password(" + user.Password + ")to login to COAT.\r\nYou can change the password after you login.");

            return user.Password;
        }

        public bool ValidateUser(string username, string password)
        {
            var user = userMgr.FindUserByName(username);
            user = user == null ? userMgr.FindUserByEmail(username) : user;

            if (user == null)
            {
                return false;
            }

            return user.Password == password;
        }

    }
}