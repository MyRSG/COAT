using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;
using System.Data;
using COAT.Extension;

namespace COAT.Data.Generate
{
    public class NameAccountGenerator : BaseGenerator<NameAccount>
    {
        public NameAccountGenerator(DataRow row)
            : base(row)
        { }

        protected override Extension.ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new ColunmPropertyPair[]{
                    //new ColunmPropertyPair("Fiscal","Fiscal"),
                    //new ColunmPropertyPair("NAL ID","NALID"),
                    //new ColunmPropertyPair("Region","Region"),
                    //new ColunmPropertyPair("Account Country Location","AccountCountryLocation"),
                    //new ColunmPropertyPair("Sales Territory (Code)","SalesTerritoryCode"),
                    new ColunmPropertyPair("Full Account Name","FullAccountName"),
                    new ColunmPropertyPair("Full Account Name - Local","FullAccountNameLocal"),
                    new ColunmPropertyPair("Parent Account Name","ParentAccountName"),
                    new ColunmPropertyPair("Parent Account Name - Local","ParentAccountNameLocal"),
                    //new ColunmPropertyPair("DUNS Number (Optional)","DUNSNumber"),
                    //new ColunmPropertyPair("Global/NIA? (Optional)","Global_NIA"),
                    //new ColunmPropertyPair("HQ state (Optional)","HQState"),
                    //new ColunmPropertyPair("Local State (Optional)","LocalState"),
                    //new ColunmPropertyPair("Industry","Industry"),
                    //new ColunmPropertyPair("Effective Transfer Date","EffectiveDate"),
                    new ColunmPropertyPair("Primary Rep","PrimaryRep")
                    //new ColunmPropertyPair("Account Type (Named/Territory)","AccountType"),
                    //new ColunmPropertyPair("Sales Segment","SalesSegment"),
                    //new ColunmPropertyPair("SFDC Linkage","SFDCLinkage"),
                    //new ColunmPropertyPair("Item Type","ItemType"),
                    //new ColunmPropertyPair("Path","Path")
                };
            }
        }

        protected override NameAccount GetInstance(System.Data.DataRow row)
        {
            return new NameAccount();
        }

        protected override bool Validate(NameAccount obj)
        {
            return true;
        }

        protected override NameAccount SychronizeDB(NameAccount obj)
        {
            Entity.NameAccounts.AddObject(obj);
            Entity.SaveChanges();
            return obj;
        }
    }
}