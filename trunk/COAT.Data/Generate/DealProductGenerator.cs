using System.Collections.Generic;
using System.Data;
using System.Linq;
using COAT.Models;
using COAT.Util.Extension;

namespace COAT.Data.Generate
{
    public class DealProductGenerator : BaseGenerator<DealProduct>
    {
        public DealProductGenerator(DataRow row)
            : base(row)
        {
        }

        protected override ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new[]
                           {
                               new ColunmPropertyPair("Deal ID", "DealId"),
                               new ColunmPropertyPair("Product Name", "ProductName"),
                               new ColunmPropertyPair("Total Price (converted)", "Price")
                           };
            }
        }

        protected override DealProduct GetInstance(DataRow row)
        {
            Deal deal = new DealGenerator(row).Generate();
            return new DealProduct {DealId = deal.Id, IsActive = true};
        }

        protected override bool Validate(DealProduct obj)
        {
            return true;
        }

        protected override DealProduct SychronizeDB(DealProduct obj)
        {
            DealProduct dbDp =
                Entity.DealProducts.FirstOrDefault(dp => dp.DealId == obj.DealId && dp.ProductName == obj.ProductName);
            if (dbDp != null)
            {
                dbDp.Update(obj, new[] {"Id", "DealId", "ProductName"});
                Entity.SaveChanges();
                return dbDp;
            }

            Entity.DealProducts.AddObject(obj);
            Entity.SaveChanges();
            return obj;
        }

        public new DealProduct[] Generate()
        {
            var dpList = new List<DealProduct>();
            const char spliter = '\n';
            string productName = Row["Product Name"].ToString();
            string priceString = Row["Total Price (converted)"].ToString();

            string[] nameArray = productName.Split(spliter);
            string[] priceArray = priceString.Split(spliter);


            for (int index = 0; index < nameArray.Length; index++)
            {
                try
                {
                    DealProduct dp = GetInstance(Row);
                    dp.ProductName = nameArray[index];
                    dp.Price = double.Parse(priceArray[index]);
                    dpList.Add(SychronizeDB(dp));
                }
                catch
                {
                }
            }

            return dpList.ToArray();
        }
    }
}