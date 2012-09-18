using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;
using COAT.Extension;
using System.Data;

namespace COAT.Data.Generate
{
    public class PartnerGenerator : BaseGenerator<Partner>
    {
        public PartnerGenerator(DataRow row)
            : base(row)
        { }

        protected override Extension.ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new ColunmPropertyPair[]{
                   new ColunmPropertyPair("Partner Account","Name"),
                   new ColunmPropertyPair("Partner Qualification","Qualification")
                };
            }
        }

        protected override Partner GetInstance(System.Data.DataRow row)
        {
            return new Partner();
        }

        protected override bool Validate(Partner obj)
        {
            return true;
        }

        protected override Partner SychronizeDB(Partner obj)
        {
            var dbPartner = Entity.Partners.FirstOrDefault(p => p.Name == obj.Name);
            if (dbPartner != null)
                return dbPartner;

            Entity.Partners.AddObject(obj);
            Entity.SaveChanges();
            return obj;
        }
    }
}