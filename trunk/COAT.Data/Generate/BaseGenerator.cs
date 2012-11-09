using System.Data;
using COAT.Models;
using COAT.Util.Extension;

namespace COAT.Data.Generate
{
    public abstract class BaseGenerator<T>
    {
        protected BaseGenerator(DataRow row)
        {
            Row = row;
            Entity = new COATEntities();
        }

        protected abstract ColunmPropertyPair[] ColunmPropertyPairs { get; }
        protected DataRow Row { get; set; }
        protected COATEntities Entity { get; set; }
        protected abstract T GetInstance(DataRow row);
        protected abstract bool Validate(T obj);
        protected abstract T SychronizeDB(T obj);

        public virtual T Generate()
        {
            return Generate(Row);
        }

        protected T Generate(DataRow row)
        {
            T rslt = GetInstance(Row);
            row.FillObject(rslt, ColunmPropertyPairs);

            if (!Validate(rslt))
            {
                throw new InvalidExpressionException();
            }

            return SychronizeDB(rslt);
        }
    }
}