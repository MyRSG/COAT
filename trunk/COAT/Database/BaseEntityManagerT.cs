using System;
using System.Data.Objects.DataClasses;
using COAT.Models;

namespace COAT.Database
{
    public abstract class BaseEntityManager<TModel> : BaseEntityManager
        where TModel : EntityObject
    {
        public TModel AddEntityObjectFor(string setName, TModel model)
        {
            return AddEntityObject(setName, model) as TModel;
        }

        public TModel UpdateEntityObjectFor(TModel model)
        {
            return UpdateEntityObject(model) as TModel;
        }

        public TModel UpdateEntityObjectFor(TModel model, Action<COATEntities, EntityObject> attachRelated)
        {
            return UpdateEntityObject(model, attachRelated) as TModel;
        }

        public bool DeleteEntityObjectFor(TModel model)
        {
            return DeleteEntityObject(model);
        }
    }
}