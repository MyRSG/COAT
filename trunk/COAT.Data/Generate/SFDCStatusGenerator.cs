using System.Data;
using System.Linq;
using COAT.Models;
using COAT.Util.Extension;

namespace COAT.Data.Generate
{
    public class SFDCStatusGenerator : BaseGenerator<SFDCStatus>
    {
        public SFDCStatusGenerator(DataRow row)
            : base(row)
        {
        }

        protected override ColunmPropertyPair[] ColunmPropertyPairs
        {
            get
            {
                return new[]
                           {
                               new ColunmPropertyPair("Registration Status", "Name")
                           };
            }
        }

        protected override SFDCStatus GetInstance(DataRow row)
        {
            return new SFDCStatus();
        }

        protected override bool Validate(SFDCStatus obj)
        {
            return obj.Name != null;
        }

        protected override SFDCStatus SychronizeDB(SFDCStatus obj)
        {
            SFDCStatus st = Entity.SFDCStatus.FirstOrDefault(a => a.Name == obj.Name);

            if (st != null)
            {
                st.Update(obj, new[] {"Id"});
                Entity.SaveChanges();
                return st;
            }

            Entity.SFDCStatus.AddObject(obj);
            Entity.SaveChanges();
            return obj;
        }
    }
}