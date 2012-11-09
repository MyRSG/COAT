namespace COAT.Data.Export
{
    public class ExportObject
    {
        [Export(Name = "Deal ID")]
        public string DealId { get; set; }

        [Export(Name = "End User Company")]
        public string EndUserCHS { get; set; }

        [Export(Name = "End User Company (EN)")]
        public string EndUserENG { get; set; }

        [Export(Name = "Country")]
        public string Country { get; set; }

        [Export(Name = "Province")]
        public string Province1 { get; set; }

        [Export(Name = "Industry-1")]
        public string Industry1 { get; set; }

        [Export(Name = "Industry-2")]
        public string Industry2 { get; set; }

        [Export(Name = "Province")]
        public string Province2 { get; set; }

        [Export(Name = "Opportunity Project Name")]
        public string ORPName { get; set; }

        [Export(Name = "Product Name")]
        public string ProductName { get; set; }

        [Export(Name = "Product Staus")]
        public string ProductStatus { get; set; }

        [Export(Name = "Partner Name")]
        public string PartnerName { get; set; }

        [Export(Name = "Partner Type")]
        public string PartnerType { get; set; }

        [Export(Name = "Total Price (USD)")]
        public string TotalPrice { get; set; }

        [Export(Name = "Soution ORP Deal Size")]
        public string DealSize { get; set; }

        //[Export(Name = "ORP Type")]
        //public string ORPType { get; set; }

        [Export(Name = "Sales Operation / ISO Admin")]
        public string Assigner { get; set; }

        [Export(Name = "Sales Operation / ISO Admin  Action Date")]
        public string AssignDate { get; set; }

        [Export(Name = "Insides Sales / Channel Manager")]
        public string Approver { get; set; }

        [Export(Name = "CAM/ISO Action Date")]
        public string ApproveDate { get; set; }

        [Export(Name = "Assigned Sales")]
        public string AssignedSalesName { get; set; }

        [Export(Name = "Channel Director")]
        public string ChannelDirector { get; set; }

        [Export(Name = "Channel Director  Action Date")]
        public string DirectorDate { get; set; }

        [Export(Name = "Deal Validation Result")]
        public string Status { get; set; }

        [Export(Name = "Contract Received")]
        public string ContractUploaded { get; set; }
    }
}