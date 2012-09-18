using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;
using COAT.Extension;
using System.Data;

namespace COAT.Data.Generate
{
    public class SFDCStatusGenerator : BaseGenerator<SFDCStatus>
    {
        public SFDCStatusGenerator(DataRow row)
            : base(row)
        { }

        protected override Extension.ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new ColunmPropertyPair[]{
                    new ColunmPropertyPair("Registration Status","Name")
                };
            }
        }

        protected override SFDCStatus GetInstance(System.Data.DataRow row)
        {
            return new SFDCStatus();
        }

        protected override bool Validate(SFDCStatus obj)
        {
            return obj.Name != null;
        }

        protected override SFDCStatus SychronizeDB(SFDCStatus obj)
        {
            var st = Entity.SFDCStatus.FirstOrDefault(a => a.Name == obj.Name);

            if (st != null)
            {
                st.Update(obj, new string[] { "Id" });
                Entity.SaveChanges();
                return st;
            }

            Entity.SFDCStatus.AddObject(obj);
            Entity.SaveChanges();
            return obj;
        }
    }
}