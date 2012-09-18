using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using COAT.Extension;
using COAT.Models;

namespace COAT.Data.Generate
{
    public abstract class BaseGenerator<T>
    {

        protected abstract ColunmPropertyPair[] ColunmPropertyPairs { get; }
        protected abstract T GetInstance(DataRow row);
        protected abstract bool Validate(T obj);
        protected abstract T SychronizeDB(T obj);

        protected DataRow Row { get; set; }
        protected COATEntities Entity { get; set; }
        public BaseGenerator(DataRow row)
        {
            Row = row;
            Entity = new COATEntities();
        }

        public virtual T Generate()
        {
            return Generate(Row);
        }

        protected T Generate(DataRow row)
        {
            var rslt = GetInstance(Row);
            row.FillObject(rslt, ColunmPropertyPairs);

            if (!Validate(rslt))
            {
                throw new InvalidExpressionException();
            }

            return SychronizeDB(rslt);
        }
    }
}