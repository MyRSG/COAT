using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using COAT.Models;
using System.Data.Objects.DataClasses;
using System.Data.Objects;

namespace COAT.Database
{
    public abstract class BaseEntityManager
    {
        public COATEntities Entities { get; set; }

        protected BaseEntityManager()
        {
            Entities = new COATEntities();
        }

        public EntityObject AddEntityObject(string setName, EntityObject obj)
        {
            Entities.AddObject(setName, obj);
            return obj;
        }

        public EntityObject UpdateEntityObject(EntityObject obj)
        {
            return UpdateEntityObject(obj, null);
        }

        public EntityObject UpdateEntityObject(EntityObject obj, Action<COATEntities, EntityObject> attachRelated)
        {
            if (obj.EntityState == System.Data.EntityState.Detached)
            {
                Entities.Attach(obj);
                Entities.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);

                if (attachRelated != null)
                {
                    attachRelated(Entities, obj);
                }
            }
            return obj;
        }

        public bool DeleteEntityObject(EntityObject obj)
        {
            Entities.DeleteObject(obj);
            return true;
        }

        public void Synchronize()
        {
            Entities.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);
        }

        protected void EntityTransaction(Action<TransactionScope> action)
        {
            try
            {
                DoEntityTransaction(action);
                Entities.AcceptAllChanges();
            }
            catch
            {
                throw;
            }
        }

        private void DoEntityTransaction(Action<TransactionScope> action)
        {
            if (action == null)
                return;

            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    action(transaction);
                    Entities.SaveChanges(SaveOptions.DetectChangesBeforeSave);
                    transaction.Complete();
                }
                catch
                {
                    throw;
                }
            }
        }

    }
}