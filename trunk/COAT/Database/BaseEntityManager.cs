using System;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Transactions;
using COAT.Models;

namespace COAT.Database
{
    public abstract class BaseEntityManager
    {
        protected BaseEntityManager()
        {
            Entities = new COATEntities();
        }

        public COATEntities Entities { get; set; }

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
            if (obj.EntityState == EntityState.Detached)
            {
                Entities.Attach(obj);
                Entities.ObjectStateManager.ChangeObjectState(obj, EntityState.Modified);

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
            DoEntityTransaction(action);
            Entities.AcceptAllChanges();
        }

        private void DoEntityTransaction(Action<TransactionScope> action)
        {
            if (action == null)
                return;

            using (var transaction = new TransactionScope())
            {
                action(transaction);
                Entities.SaveChanges(SaveOptions.DetectChangesBeforeSave);
                transaction.Complete();
            }
        }
    }
}