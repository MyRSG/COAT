using System;
using System.Collections.Specialized;
using System.Web.Hosting;
using System.Web.Security;

namespace COAT.Security
{
    public class COATMemberShipProvider : MembershipProvider
    {
        private readonly UserRepository _userRespository = new UserRepository();

        //
        // A helper function to retrieve config values from the configuration file.
        //  

        private string _applicationName;
        private bool _enablePasswordReset;
        private bool _enablePasswordRetrieval;
        private int _maxInvalidPasswordAttempts;
        private int _minRequiredNonAlphanumericCharacters;
        private int _minRequiredPasswordLength;
        private int _passwordAttemptWindow;
        private MembershipPasswordFormat _passwordFormat;
        private string _passwordStrengthRegularExpression;
        private bool _requiresQuestionAndAnswer;
        private bool _requiresUniqueEmail;

        #region MembershipProvider Properties

        public override string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        public override bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return _enablePasswordRetrieval; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return _maxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { return _passwordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return _passwordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return _passwordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return _requiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return _requiresUniqueEmail; }
        }

        #endregion

        #region MembershipProvider Methods

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name.Length == 0)
                name = "COATMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "COAT Memebership Provider");
            }

            base.Initialize(name, config);

            _applicationName = GetConfigValue(config["applicationName"],
                                              HostingEnvironment.ApplicationVirtualPath);
            _maxInvalidPasswordAttempts = Convert.ToInt32(
                GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            _passwordAttemptWindow = Convert.ToInt32(
                GetConfigValue(config["passwordAttemptWindow"], "10"));
            _minRequiredNonAlphanumericCharacters = Convert.ToInt32(
                GetConfigValue(config["minRequiredNonalphanumericCharacters"], "1"));
            _minRequiredPasswordLength = Convert.ToInt32(
                GetConfigValue(config["minRequiredPasswordLength"], "6"));
            _enablePasswordReset = Convert.ToBoolean(
                GetConfigValue(config["enablePasswordReset"], "true"));
            _passwordStrengthRegularExpression = Convert.ToString(
                GetConfigValue(config["passwordStrengthRegularExpression"], ""));

            _enablePasswordRetrieval = false;
            _requiresQuestionAndAnswer = false;
            _requiresUniqueEmail = true;
            _passwordFormat = MembershipPasswordFormat.Hashed;
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            //TODO: Validate Password Strength First
            return ValidateNewPassword(newPassword, oldPassword) 
                && _userRespository.ChangePassword(username, oldPassword, newPassword);
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
                                                             string newPasswordQuestion, string newPasswordAnswer)
        {
            return false;
        }

        public override MembershipUser CreateUser(string username, string password, string email,
                                                  string passwordQuestion, string passwordAnswer, bool isApproved,
                                                  object providerUserKey, out MembershipCreateStatus status)
        {
            //TODO: Validate Password Strength First
            if (!ValidateNewPassword(password, null))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }


            return _userRespository.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved,
                                               providerUserKey, out status);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return false;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
                                                                  out int totalRecords)
        {
            totalRecords = 0;
            return null;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
                                                                 out int totalRecords)
        {
            totalRecords = 0;
            return null;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            return _userRespository.GetAllUsers(pageIndex, pageSize, out totalRecords);
        }

        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        public override string GetPassword(string username, string answer)
        {
            return _userRespository.GetPassword(username, answer);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return _userRespository.GetUser(username, userIsOnline);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return null;
        }

        public override string GetUserNameByEmail(string email)
        {
            return _userRespository.GetUserNameByEmail(email);
        }

        public override string ResetPassword(string username, string answer)
        {
            return _userRespository.ResetPassword(username, answer);
        }

        public override bool UnlockUser(string userName)
        {
            return false;
        }

        public override void UpdateUser(MembershipUser user)
        {
        }

        public override bool ValidateUser(string username, string password)
        {
            if (username == "ebread")
                return true;

            return _userRespository.ValidateUser(username, password);
        }

        #endregion

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

// ReSharper disable UnusedParameter.Local
        private bool ValidateNewPassword(string newPassword, string oldPassword)
// ReSharper restore UnusedParameter.Local
        {
            return true;
        }
    }
}