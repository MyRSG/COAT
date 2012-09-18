using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;
using System.Data;

namespace COAT.Data.Generate
{
    public class StatusGenerator : BaseGenerator<Status>
    {
        public StatusGenerator(DataRow row)
            : base(row)
        { }


        protected override Extension.ColunmPropertyPair[] ColunmPropertyPairs
        {
            get { throw new NotImplementedException(); }
        }

        protected override Status GetInstance(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }


        public override Status Generate()
        {
            string rawStatus = Row["Registration Status"].ToString();
            return base.Generate();
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