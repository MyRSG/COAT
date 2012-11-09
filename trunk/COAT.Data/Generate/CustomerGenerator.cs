using System.Data;
using System.Linq;
using COAT.Models;
using COAT.Util.Extension;

namespace COAT.Data.Generate
{
    public class CustomerGenerator : BaseGenerator<Customer>
    {
        public CustomerGenerator(DataRow row)
            : base(row)
        {
        }

        protected override ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new[]
                           {
                               new ColunmPropertyPair("End User Company", "NameENG"),
                               new ColunmPropertyPair("End User Company(Local)", "NameCHS", true),
                               new ColunmPropertyPair("End User Company Street", "Street"),
                               new ColunmPropertyPair("End User Company City", "City"),
                               new ColunmPropertyPair("End User Company State/Province", "Province")
                           };
            }
        }

        protected override Customer GetInstance(DataRow row)
        {
            return new Customer();
        }

        protected override bool Validate(Customer obj)
        {
            return obj.NameENG != null || obj.NameCHS != null;
        }

        protected override Customer SychronizeDB(Customer obj)
        {
            Customer dbObj;
            if (obj.NameENG != null)
            {
                dbObj = Entity.Customers.FirstOrDefault(c => c.NameENG == obj.NameENG);
                return UpdateDB(dbObj, obj);
            }

            dbObj = Entity.Customers.FirstOrDefault(c => c.NameCHS == obj.NameCHS);
            return UpdateDB(dbObj, obj);
        }

        private Customer UpdateDB(Customer dbObj, Customer memoObj)
        {
            if (dbObj == null)
            {
                Entity.Customers.AddObject(memoObj);
                Entity.SaveChanges();
                return memoObj;
            }

            return dbObj;
        }
    }
}