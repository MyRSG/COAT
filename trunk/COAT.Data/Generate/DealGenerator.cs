using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;
using COAT.Extension;
using System.Data;

namespace COAT.Data.Generate
{
    public class DealGenerator : BaseGenerator<Deal>
    {
        public DealGenerator(DataRow row)
            : base(row)
        { }

        protected override Extension.ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new ColunmPropertyPair[]{
                    new ColunmPropertyPair("Registration Status","RegistStatus"),
                    new ColunmPropertyPair("Created Date","CreateDate"),
                    new ColunmPropertyPair("Deal ID","Id"),
                    new ColunmPropertyPair("Opportunity Name","Name"),
                    new ColunmPropertyPair("Opportunity Owner","Owner")
                };
            }
        }

        protected override Deal GetInstance(DataRow row)
        {
            var customer = new CustomerGenerator(row).Generate();
            var industry = new IndustryGenerator(row).Generate();
            var partner = new PartnerGenerator(row).Generate();
            var sfdcSt = new SFDCStatusGenerator(row).Generate();
            var specialization = new SpecializationGenerator(row).Generate();

            var rslt = new Deal
            {
                CustomerId = customer.Id,
                IndustryId = industry.Id,
                PartnerId = partner.Id,
                RegistStatus = sfdcSt.Id,
                StatusId = Entity.Status.FirstOrDefault(a => a.ActionName == "PA").Id
            };

            if (specialization != null)
            {
                rslt.SpecializationId = specialization.Id;
            }

            return rslt;
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
            var dbDeal = Entity.Deals.FirstOrDefault(d => d.Id == obj.Id);
            if (dbDeal != null)
            {
                dbDeal.Update(obj, new string[] { "Id" });
                Entity.SaveChanges();
                return dbDeal;
            }

            Entity.Deals.AddObject(obj);
            Entity.SaveChanges();
            return obj;
        }
    }
}