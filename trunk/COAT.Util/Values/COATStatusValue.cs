using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COAT.Util.Values
{
    public struct COATStatusValue
    {
        public const string PendingAssign = "PA";
        public const string PendingChannelApproval = "PCA";
        public const string PendingSalesApproval = "PSA";
        public const string PendingDirectorApproval = "PDA";
        public const string Approved = "A";
        public const string Rejected = "	R";
        public const string AwaitingSalesConfirmation = "PSC";
        public const string AwaitingVolumeSalesConfirmation = "PSCV";
    }
}
