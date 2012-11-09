using System;
using System.Data;
using System.Linq;
using COAT.Models;
using COAT.Util.Extension;

namespace COAT.Data.Generate
{
    public class SpecializationGenerator : BaseGenerator<Specialization>
    {
        public SpecializationGenerator(DataRow row)
            : base(row)
        {
        }

        protected override ColunmPropertyPair[] ColunmPropertyPairs
        {
            get { return new ColunmPropertyPair[0]; }
        }

        protected override Specialization GetInstance(DataRow row)
        {
            //string partnerTypeString = row["Partner Type"].ToString() + ",";

            string specializationString = string.Empty;
            if (IsNotEmpty(row))
            {
                specializationString = row["Partner Qualification"].ToString().Split(',')[0];
            }

            return Entity.Specializations.FirstOrDefault(a => a.ORPName == specializationString);
        }

        private static bool IsNotEmpty(DataRow row)
        {
            return !(row["Partner Qualification"] is DBNull) || row["Partner Qualification"] != null;
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