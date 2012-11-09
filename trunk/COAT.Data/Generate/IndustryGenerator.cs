using System.Data;
using System.Linq;
using COAT.Models;
using COAT.Util.Extension;

namespace COAT.Data.Generate
{
    public class IndustryGenerator : BaseGenerator<Industry>
    {
        public IndustryGenerator(DataRow row)
            : base(row)
        {
        }

        protected override ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new[]
                           {
                               new ColunmPropertyPair("Industry", "Name")
                           };
            }
        }

        protected override Industry GetInstance(DataRow row)
        {
            return new Industry();
        }

        protected override bool Validate(Industry obj)
        {
            return true;
        }

        protected override Industry SychronizeDB(Industry obj)
        {
            Industry dbIndustry = Entity.Industries.FirstOrDefault(i => i.Name == obj.Name);
            if (dbIndustry != null)
                return dbIndustry;

            Entity.Industries.AddObject(obj);
            Entity.SaveChanges();
            return obj;
        }
    }
}