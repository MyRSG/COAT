using System.Web;
using System.Web.Security;
using COAT.Security;

namespace COAT.ViewModel.Shared
{
    public class NavBarViewModel
    {
        private COATMemebershipUser _user;

        public NavBarViewModel(HttpRequestBase request)
        {
            Request = request;
            SetCurrentMemberShipUser();
            SetAccessRight();
        }

        public bool IsShowChannelSection
        {
            get { return IsShowChannelAssign || IsShowChannelApprove || IsShowChannelDirector; }
        }

        public bool IsShowChannelAssign { get; set; }
        public bool IsShowChannelApprove { get; set; }
        public bool IsShowChannelDirector { get; set; }

        public bool IsShowSalesSection
        {
            get { return IsShowSalesAssign || IsShowSalesApprove; }
        }

        public bool IsShowSalesAssign { get; set; }
        public bool IsShowSalesApprove { get; set; }

        public bool IsShowContractSection { get; set; }
        public bool IsShowAdminSection { get; set; }
        public HttpRequestBase Request { get; set; }


        protected bool IsAdmin()
        {
            return _user.SystemRole.Name == "Admin";
        }

        protected void SetAccessRight()
        {
            if (IsAdmin())
            {
                IsShowChannelAssign = true;
                IsShowChannelApprove = true;
                IsShowChannelDirector = true;
                IsShowSalesAssign = true;
                IsShowSalesApprove = true;
                IsShowContractSection = true;
                IsShowAdminSection = true;
                return;
            }

            IsShowChannelAssign = _user.SystemRole.Name == "ChannelAssigner" || _user.SystemRole.Name == "SalesAssigner";
            IsShowChannelApprove = _user.SystemRole.Name == "ChannelApprover";
            IsShowChannelDirector = _user.SystemRole.Name == "ChannelDirector";
            IsShowSalesAssign = _user.SystemRole.Name == "SalesAssigner" || _user.SystemRole.Name == "ChannelAssigner";
            IsShowSalesApprove = _user.SystemRole.Name == "SalesApprover";
            IsShowContractSection = _user.SystemRole.Name == "ChannelApprover";
            IsShowAdminSection = false;
        }

        protected void SetCurrentMemberShipUser()
        {
            _user = Membership.GetUser(true) as COATMemebershipUser;
        }
    }
}