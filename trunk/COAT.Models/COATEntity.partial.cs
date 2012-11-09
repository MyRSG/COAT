namespace COAT.Models
{
    public partial class COATEntities
    {
        //partial void OnContextCreated()
        //{
        //    // Register the handler for the SavingChanges event.
        //    this.SavingChanges
        //        += new EventHandler(context_SavingChanges);
        //}

        //private static void context_SavingChanges(object sender, EventArgs e)
        //{
        //    var mgr = ((ObjectContext)sender).ObjectStateManager;
        //    RecordAddedEntityObject(mgr);
        //    RecordModifiedEntityObject(mgr);

        //    foreach (ObjectStateEntry entry in ((ObjectContext)sender).ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified))
        //    {
        //        if (!entry.IsRelationship && entry.Entity.GetType() == typeof(Deal))
        //        {
        //            Deal obj = entry.Entity as Deal;


        //        }
        //        else if (!entry.IsRelationship && entry.Entity.GetType() == typeof(DealProduct))
        //        {

        //            DealProduct obj = entry.Entity as DealProduct;
        //        }
        //    }
        //}

        //private static void RecordAddedEntityObject(ObjectStateManager mgr)
        //{
        //    foreach (ObjectStateEntry entry in mgr.GetObjectStateEntries(EntityState.Added | EntityState.Modified))
        //    {
        //        if (!entry.IsRelationship && entry.Entity.GetType() == typeof(Deal))
        //        {
        //            Deal obj = entry.Entity as Deal;


        //        }
        //        else if (!entry.IsRelationship && entry.Entity.GetType() == typeof(DealProduct))
        //        {

        //            DealProduct obj = entry.Entity as DealProduct;
        //        }
        //    }
        //}

        //private static void RecordModifiedEntityObject(ObjectStateManager mgr)
        //{

        //}
    }
}