using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using COAT.Security;

namespace COAT.ViewModel.Shared
{
    public class NavBarViewModel
    {
        public bool IsShowChannelSection { get { return IsShowChannelAssign || IsShowChannelApprove || IsShowChannelDirector; } }
        public bool IsShowChannelAssign { get; set; }
        public bool IsShowChannelApprove { get; set; }
        public bool IsShowChannelDirector { get; set; }

        public bool IsShowSalesSection { get { return IsShowSalesAssign || IsShowSalesApprove; } }
        public bool IsShowSalesAssign { get; set; }
        public bool IsShowSalesApprove { get; set; }

        public bool IsShowContractSection { get; set; }
        public bool IsShowAdminSection { get; set; }


        public NavBarViewModel(HttpRequestBase request)
        {
            _Request = request;
            SetCurrentMemberShipUser();
            SetAccessRight();
        }

        protected bool IsAdmin()
        {
            return _User.SystemRole.Name == "Admin";
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

            IsShowChannelAssign = _User.SystemRole.Name == "ChannelAssigner" || _User.SystemRole.Name == "SalesAssigner";
            IsShowChannelApprove = _User.SystemRole.Name == "ChannelApprover";
            IsShowChannelDirector = _User.SystemRole.Name == "ChannelDirector";
            IsShowSalesAssign = _User.SystemRole.Name == "SalesAssigner" || _User.SystemRole.Name == "ChannelAssigner";
            IsShowSalesApprove = _User.SystemRole.Name == "SalesApprover";
            IsShowContractSection = _User.SystemRole.Name == "ChannelApprover";
            IsShowAdminSection = false;

        }

        protected void SetCurrentMemberShipUser()
        {
            _User = Membership.GetUser(true) as COATMemebershipUser;
        }

        private HttpRequestBase _Request;
        private COATMemebershipUser _User;
    }
}