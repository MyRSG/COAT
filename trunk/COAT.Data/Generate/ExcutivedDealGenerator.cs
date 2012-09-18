using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;
using System.Data;
using COAT.Extension;


namespace COAT.Data.Generate
{
    public class ExcutivedDealGenerator : BaseGenerator<ExcutivedDeal>
    {
        public ExcutivedDealGenerator(DataRow row) : base(row) { }

        protected override Extension.ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new ColunmPropertyPair[] { 
                     new ColunmPropertyPair("Deal ID","DealId"),
                     //new ColunmPropertyPair("End User Name","EndUserName"),
                     new ColunmPropertyPair<ExcutivedDeal>("End User Company",a=>a.EndUserName),
                     new ColunmPropertyPair<ExcutivedDeal>("End User Company(Local)",a=>a.EndUserName2,true),
                     new ColunmPropertyPair("PAM","PAMName"),
                     new ColunmPropertyPair("Fiscal Period","CreateDateAndQuater"),
                     //new ColunmPropertyPair("Expiration Date Year and Qtr - Fiscal","ExpireDateAndQuater",true),
                     //new ColunmPropertyPair("Closure Date - Fiscal Year Qtr","ClosureDateAndQuater",true),
                     new ColunmPropertyPair("Partner Account","PartnerOwner"),
                     //new ColunmPropertyPair("Partner Type","PartnerType",true),
                     //new ColunmPropertyPair("Line Registration Status","LineRegStatus",true),
                     new ColunmPropertyPair("Registration Status","Status"),
                     new ColunmPropertyPair("Product Name","ProductName"),
                     //new ColunmPropertyPair("Opp Industry","OppIndustry",true),
                     //new ColunmPropertyPair("Approved Amount USD","ApprovedAmountUSD",true),
                     //new ColunmPropertyPair("Partner Country","PartnerCountry",true),
                     //new ColunmPropertyPair("Partner Region","PartnerRegion",true),
                     //new ColunmPropertyPair("Opportunity Creation Date","CreateDate",true),
                     //new ColunmPropertyPair("Close Date","CloseDate",true),
                     //new ColunmPropertyPair("Expiration Date","ExpireDate",true),
                     //new ColunmPropertyPair("Specialization","Specialization",true),
                     //new ColunmPropertyPair("Effective Create Date FY","CreateFY",true),
                     //new ColunmPropertyPair("Effective Closure Date FY","CloseFY",true),
                     //new ColunmPropertyPair("Effective Create and Close Date in same FY",""),
                     //new ColunmPropertyPair("Expired in FY","ExpireFY",true),
                     //new ColunmPropertyPair("PARTNER_ROLLUP","PartnerRollUp",true),
                     //new ColunmPropertyPair("Stage","Stage",true),
                     //new ColunmPropertyPair("PNET ID","PNETID",true),
                     //new ColunmPropertyPair("Partner Email","PartnerEmail",true),
                     //new ColunmPropertyPair("Opportunity Name","OppName",true)
                     new ColunmPropertyPair("Opportunity Name","OppName")
            };
            }
        }

        protected override ExcutivedDeal GetInstance(System.Data.DataRow row)
        {
            return new ExcutivedDeal();
        }

        protected override bool Validate(ExcutivedDeal obj)
        {
            return true;
        }

        protected override ExcutivedDeal SychronizeDB(ExcutivedDeal obj)
        {
            var edeal = Entity.ExcutivedDeals.FirstOrDefault(a => a.DealId == obj.DealId);

            if (edeal != null)
            {
                edeal.Update(obj, new string[] { "DealId" });
                Entity.SaveChanges();
                return edeal;
            }

            Entity.ExcutivedDeals.AddObject(obj);
            Entity.SaveChanges();
            return obj;
        }
    }
}