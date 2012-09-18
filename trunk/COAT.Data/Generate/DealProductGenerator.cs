using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;
using COAT.Extension;
using System.Data;
using System.Diagnostics;

namespace COAT.Data.Generate
{
    public class DealProductGenerator : BaseGenerator<DealProduct>
    {

        Dictionary<string, double> _NamePricePair = new Dictionary<string, double>();

        public DealProductGenerator(DataRow row)
            : base(row)
        { }
        protected override Extension.ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new ColunmPropertyPair[]{
                   new ColunmPropertyPair("Deal ID","DealId"),
                   new ColunmPropertyPair("Product Name","ProductName"),
                   new ColunmPropertyPair("Total Price (converted)","Price")
                 };
            }
        }

        protected override DealProduct GetInstance(System.Data.DataRow row)
        {
            var deal = new DealGenerator(row).Generate();
            return new DealProduct { DealId = deal.Id, IsActive = true };
        }

        protected override bool Validate(DealProduct obj)
        {
            return true;
        }

        protected override DealProduct SychronizeDB(DealProduct obj)
        {
            var dbDP = Entity.DealProducts.FirstOrDefault(dp => dp.DealId == obj.DealId && dp.ProductName == obj.ProductName);
            if (dbDP != null)
            {
                dbDP.Update(obj, new string[] { "Id", "DealId", "ProductName" });
                Entity.SaveChanges();
                return dbDP;
            }

            Entity.DealProducts.AddObject(obj);
            Entity.SaveChanges();
            return obj;
        }

        public new DealProduct[] Generate()
        {
            List<DealProduct> dpList = new List<DealProduct>();
            var spliter = '\n';
            var productName = Row["Product Name"].ToString();
            var priceString = Row["Total Price (converted)"].ToString();

            var nameArray = productName.Split(spliter);
            var priceArray = priceString.Split(spliter);


            for (var index = 0; index < nameArray.Length; index++)
            {
                try
                {
                    var dp = GetInstance(Row);
                    dp.ProductName = nameArray[index];
                    dp.Price = double.Parse(priceArray[index]);
                    dpList.Add(SychronizeDB(dp));
                }
                catch (Exception ex)
                {

                }
            }

            return dpList.ToArray();

        }
    }
}