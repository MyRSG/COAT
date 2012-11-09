using System.Data;
using System.Linq;
using COAT.Models;
using COAT.Util.Extension;

namespace COAT.Data.Generate
{
    public class DealGenerator : BaseGenerator<Deal>
    {
        public DealGenerator(DataRow row)
            : base(row)
        {
        }

        protected override ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new[]
                           {
                               new ColunmPropertyPair("Registration Status", "RegistStatus"),
                               new ColunmPropertyPair("Created Date", "CreateDate"),
                               new ColunmPropertyPair("Deal ID", "Id"),
                               new ColunmPropertyPair("Opportunity Name", "Name"),
                               new ColunmPropertyPair("Opportunity Owner", "Owner")
                           };
            }
        }

        protected override Deal GetInstance(DataRow row)
        {
            Customer customer = new CustomerGenerator(row).Generate();
            Industry industry = new IndustryGenerator(row).Generate();
            Partner partner = new PartnerGenerator(row).Generate();
            SFDCStatus sfdcSt = new SFDCStatusGenerator(row).Generate();
            Specialization specialization = new SpecializationGenerator(row).Generate();

            Status firstOrDefault = Entity.Status.FirstOrDefault(a => a.ActionName == "PA");
            if (firstOrDefault != null)
            {
                var rslt = new Deal
                               {
                                   CustomerId = customer.Id,
                                   IndustryId = industry.Id,
                                   PartnerId = partner.Id,
                                   RegistStatus = sfdcSt.Id,
                                   StatusId = firstOrDefault.Id
                               };

                if (specialization != null)
                {
                    rslt.SpecializationId = specialization.Id;
                }

                return rslt;
            }
            return null;
        }

        protected override bool Validate(Deal obj)
        {
            if (obj.Id == null)
            {
                return false;
            }

            return true;
        }

        protected override Deal SychronizeDB(Deal obj)
        {
            Deal dbDeal = Entity.Deals.FirstOrDefault(d => d.Id == obj.Id);
            if (dbDeal != null)
            {
                dbDeal.Update(obj, new[] {"Id"});
                Entity.SaveChanges();
                return dbDeal;
            }

            Entity.Deals.AddObject(obj);
            Entity.SaveChanges();
            return obj;
        }
    }
}