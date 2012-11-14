using System.Data;
using System.Linq;
using COAT.Models;
using COAT.Util.Extension;

namespace COAT.Data.Generate
{
    public class PartnerGenerator : BaseGenerator<Partner>
    {
        public PartnerGenerator(DataRow row)
            : base(row)
        {
        }

        protected override ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new[]
                           {
                               new ColunmPropertyPair("Partner Account", "Name"),
                               new ColunmPropertyPair("Partner Qualification", "Qualification",true)
                           };
            }
        }

        protected override Partner GetInstance(DataRow row)
        {
            return new Partner();
        }

        protected override bool Validate(Partner obj)
        {
            return true;
        }

        protected override Partner SychronizeDB(Partner obj)
        {
            Partner dbPartner = Entity.Partners.FirstOrDefault(p => p.Name == obj.Name);
            if (dbPartner != null)
                return dbPartner;

            Entity.Partners.AddObject(obj);
            Entity.SaveChanges();
            return obj;
        }
    }
}