using System;
using System.Data;
using System.Linq;
using System.Net.Mail;
using COAT.Models;
using COAT.Util;
using COAT.Util.Extension;
using COAT.Util.Mail;

namespace COAT.Data.Generate
{
    public class UserGenertator : BaseGenerator<User>
    {
        private readonly COATMailHelper _mHelper = new COATMailHelper();

        public UserGenertator(int systemRoleId, int bussinessRoleId, MailMessage mailMsg, DataRow row)
            : base(row)
        {
            SystemRoleId = systemRoleId;
            BusinessRoleId = bussinessRoleId;
            MailMsg = mailMsg;
        }

        public int SystemRoleId { get; set; }
        public int BusinessRoleId { get; set; }
        public MailMessage MailMsg { get; set; }

        protected override ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new[]
                           {
                               new ColunmPropertyPair(0, "Name"),
                               new ColunmPropertyPair(1, "Email")
                           };
            }
        }

        protected override User GetInstance(DataRow row)
        {
            return new User {BusinessRoleId = BusinessRoleId, SystemRoleId = SystemRoleId, IsDelete = false};
        }

        protected override bool Validate(User obj)
        {
            return true;
        }

        protected override User SychronizeDB(User obj)
        {
            if (
                Entity.Users.Count(
                    a => a.Email.ToLower() == obj.Email.ToLower() || a.Name.ToLower() == obj.Name.ToLower()) > 0)
            {
                var user =
                    Entity.Users.FirstOrDefault(
                        a => a.Email.ToLower() == obj.Email.ToLower() || a.Name.ToLower() == obj.Name.ToLower());
                //user.Password = PasswordUtil.CreatePassword();
                user.UpdateExcept(obj, new[] {"Id", "Password"});
                Entity.SaveChanges();
                //try
                //{
                //    mHelper.SendMail(new string[] { obj.Email },
                //    null,
                //    "Your COAT Account's Password is Reset",
                //    string.Format("{0}, Your new password is {1}", user.Name, user.Password));
                //}
                //catch { }
                return obj;
            }

            //Generate Password
            obj.Password = PasswordUtil.CreatePassword();

            Entity.Users.AddObject(obj);
            Entity.SaveChanges();
            try
            {
                _mHelper.SendMail(new[] {obj.Email},
                                 null,
                                 MailMsg.Subject,
                                 string.Format(MailMsg.Body, obj.Name, obj.Password));
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }

            return obj;
        }

        //private string GetNameFromEmail(string emall)
        //{
        //    string pattern = "[^@]*@";

        //}
    }
}