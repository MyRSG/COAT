using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;
using COAT.Models;

namespace COAT.Database
{
    public abstract class BaseEntityManager<TModel> : BaseEntityManager
        where TModel : EntityObject
    {
        public TModel AddEntityObjectFor(string setName, TModel model)
        {
            return base.AddEntityObject(setName, model) as TModel;
        }

        public TModel UpdateEntityObjectFor(TModel model)
        {
            return base.UpdateEntityObject(model) as TModel;
        }

        public TModel UpdateEntityObjectFor(TModel model, Action<COATEntities, EntityObject> attachRelated)
        {
            return base.UpdateEntityObject(model, attachRelated) as TModel;
        }

        public bool DeleteEntityObjectFor(TModel model)
        {
            return base.DeleteEntityObject(model);
        }

    }
}