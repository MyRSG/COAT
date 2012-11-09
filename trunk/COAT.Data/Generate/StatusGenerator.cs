using System;
using System.Data;
using COAT.Models;
using COAT.Util.Extension;

namespace COAT.Data.Generate
{
    public class StatusGenerator : BaseGenerator<Status>
    {
        public StatusGenerator(DataRow row)
            : base(row)
        {
        }


        protected override ColunmPropertyPair[] ColunmPropertyPairs
        {
            get { throw new NotImplementedException(); }
        }

        protected override Status GetInstance(DataRow row)
        {
            throw new NotImplementedException();
        }


        protected override bool Validate(Status obj)
        {
            throw new NotImplementedException();
        }

        protected override Status SychronizeDB(Status obj)
        {
            throw new NotImplementedException();
        }
    }
}