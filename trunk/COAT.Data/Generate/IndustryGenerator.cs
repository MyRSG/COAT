using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;
using COAT.Extension;
using System.Data;

namespace COAT.Data.Generate
{
    public class IndustryGenerator : BaseGenerator<Industry>
    {
        public IndustryGenerator(DataRow row)
            : base(row)
        { }

        protected override Extension.ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new ColunmPropertyPair[]{
                   new ColunmPropertyPair("Industry","Name")};
            }
        }

        protected override Industry GetInstance(System.Data.DataRow row)
        {
            return new Industry();
        }

        protected override bool Validate(Industry obj)
        {
            return true;
        }

        protected override Industry SychronizeDB(Industry obj)
        {
            var dbIndustry = Entity.Industries.FirstOrDefault(i => i.Name == obj.Name);
            if (dbIndustry != null)
                return dbIndustry;

            Entity.Industries.AddObject(obj);
            Entity.SaveChanges();
            return obj;
        }
    }
}