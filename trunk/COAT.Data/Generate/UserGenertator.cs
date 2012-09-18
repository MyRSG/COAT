using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;
using COAT.Extension;
using System.Data;
using System.Text;
using COAT.Util;
using System.Net.Mail;

namespace COAT.Data.Generate
{
    public class UserGenertator : BaseGenerator<User>
    {
        public int SystemRoleId { get; set; }
        public int BusinessRoleId { get; set; }
        public MailMessage MailMsg { get; set; }

        public UserGenertator(int systemRoleId, int bussinessRoleId, MailMessage mailMsg, DataRow row)
            : base(row)
        {
            SystemRoleId = systemRoleId;
            BusinessRoleId = bussinessRoleId;
            MailMsg = mailMsg;
        }

        protected override Extension.ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new ColunmPropertyPair[]{
                     new ColunmPropertyPair(0,"Name"),
                     new ColunmPropertyPair(1,"Email")
                };
            }
        }

        protected override User GetInstance(System.Data.DataRow row)
        {
            return new User { BusinessRoleId = BusinessRoleId, SystemRoleId = SystemRoleId, IsDelete = false };
        }

        protected override bool Validate(User obj)
        {
            return true;
        }

        protected override User SychronizeDB(User obj)
        {
            if (Entity.Users.Count(a => a.Email.ToLower() == obj.Email.ToLower() || a.Name.ToLower() == obj.Name.ToLower()) > 0)
            {
                var user = Entity.Users.FirstOrDefault(a => a.Email.ToLower() == obj.Email.ToLower() || a.Name.ToLower() == obj.Name.ToLower());
                //user.Password = PasswordUtil.CreatePassword();
                user.UpdateExcept(obj, new string[] { "Id", "Password" });
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
                mHelper.SendMail(new string[] { obj.Email },
                null,
                MailMsg.Subject,
                string.Format(MailMsg.Body, obj.Name, obj.Password));
            }
            catch { }

            return obj;
        }

        //private string GetNameFromEmail(string emall)
        //{
        //    string pattern = "[^@]*@";

        //}

        private COAT.Mail.COATMailHelper mHelper = new Mail.COATMailHelper();
    }
}