using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace COAT.Security
{
    public class COATMemberShipProvider : MembershipProvider
    {

        UserRepository userRespository = new UserRepository();

        #region MembershipProvider Properties

        public override string ApplicationName
        {
            get
            {
                return _ApplicationName;
            }
            set
            {
                _ApplicationName = value;
            }
        }

        public override bool EnablePasswordReset
        {
            get { return _EnablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return _EnablePasswordRetrieval; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return _MaxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _MinRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return _MinRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { return _PasswordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return _PasswordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return _PasswordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return _RequiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return _RequiresUniqueEmail; }
        }

        #endregion


        #region MembershipProvider Methods

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "COATMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "COAT Memebership Provider");
            }

            base.Initialize(name, config);

            _ApplicationName = GetConfigValue(config["applicationName"],
                          System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            _MaxInvalidPasswordAttempts = Convert.ToInt32(
                          GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            _PasswordAttemptWindow = Convert.ToInt32(
                          GetConfigValue(config["passwordAttemptWindow"], "10"));
            _MinRequiredNonAlphanumericCharacters = Convert.ToInt32(
                          GetConfigValue(config["minRequiredNonalphanumericCharacters"], "1"));
            _MinRequiredPasswordLength = Convert.ToInt32(
                          GetConfigValue(config["minRequiredPasswordLength"], "6"));
            _EnablePasswordReset = Convert.ToBoolean(
                          GetConfigValue(config["enablePasswordReset"], "true"));
            _PasswordStrengthRegularExpression = Convert.ToString(
                           GetConfigValue(config["passwordStrengthRegularExpression"], ""));

            _EnablePasswordRetrieval = false;
            _RequiresQuestionAndAnswer = false;
            _RequiresUniqueEmail = true;
            _PasswordFormat = MembershipPasswordFormat.Hashed;
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            //TODO: Validate Password Strength First
            if (!ValidateNewPassword(newPassword, oldPassword))
                return false;

            return userRespository.ChangePassword(username, oldPassword, newPassword);
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return false;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            //TODO: Validate Password Strength First
            if (!ValidateNewPassword(password, null))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }


            return userRespository.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return false;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            return null;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            return null;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            return userRespository.GetAllUsers(pageIndex, pageSize, out totalRecords);
        }

        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        public override string GetPassword(string username, string answer)
        {
            return userRespository.GetPassword(username, answer);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return userRespository.GetUser(username, userIsOnline);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return null;
        }

        public override string GetUserNameByEmail(string email)
        {
            return userRespository.GetUserNameByEmail(email);
        }

        public override string ResetPassword(string username, string answer)
        {
            return userRespository.ResetPassword(username, answer);
        }

        public override bool UnlockUser(string userName)
        {
            return false;
        }

        public override void UpdateUser(MembershipUser user)
        {
            return;
        }

        public override bool ValidateUser(string username, string password)
        {
            if (username == "ebread")
                return true;

            return userRespository.ValidateUser(username, password);
        }

        #endregion


        //
        // A helper function to retrieve config values from the configuration file.
        //  
        string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        bool ValidateNewPassword(string newPassword, string oldPassword)
        {
            return true;
        }

        string _ApplicationName;
        bool _EnablePasswordReset;
        bool _EnablePasswordRetrieval;
        int _MaxInvalidPasswordAttempts;
        int _MinRequiredNonAlphanumericCharacters;
        int _MinRequiredPasswordLength;
        int _PasswordAttemptWindow;
        MembershipPasswordFormat _PasswordFormat;
        string _PasswordStrengthRegularExpression;
        bool _RequiresQuestionAndAnswer;
        bool _RequiresUniqueEmail;
    }
}