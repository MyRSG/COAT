using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;
using System.Data;
using COAT.Extension;

namespace COAT.Data.Generate
{
    public class SpecializationGenerator : BaseGenerator<Specialization>
    {
        public SpecializationGenerator(DataRow row)
            : base(row)
        { }

        protected override Extension.ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new ColunmPropertyPair[0];
            }
        }

        protected override Specialization GetInstance(System.Data.DataRow row)
        {
            string partnerTypeString = row["Partner Type"].ToString() + ",";
            string text = row["Partner Qualification"].ToString();
            string specializationString = text.Split(',')[0];

            return Entity.Specializations.FirstOrDefault(a => a.ORPName == specializationString);
        }

        protected override bool Validate(Specialization obj)
        {
            return true;
        }

        protected override Specialization SychronizeDB(Specialization obj)
        {
            return obj;
        }
    }
}