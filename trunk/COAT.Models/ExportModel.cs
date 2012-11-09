namespace COAT.Models
{
    public class ExportDataModel
    {
        public ExportDataModel(Deal deal)
        {
            Deal = deal;
        }

        public Deal Deal { get; set; }
    }
}