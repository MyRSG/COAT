using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using COAT.Models;
using COAT.Util.Values;
using COAT.Util.Log;

namespace COAT.Data.Export
{
    public class ExportGenerator
    {
        private readonly Logger _logger;

        public ExportGenerator(IEnumerable<Deal> deals)
        {
            Deals = deals;
            _logger = new Logger();
        }

        public IEnumerable<Deal> Deals { get; protected set; }

        public ExportObject[] Generate()
        {
            var rslt = new List<ExportObject>();
            foreach (var d in Deals)
            {
                rslt.AddRange(GenerateExportObject(d));
            }

            return rslt.ToArray();
        }

        private IEnumerable<ExportObject> GenerateExportObject(Deal d)
        {
            var rslt = new List<ExportObject>();
            foreach (var pd in d.DealProducts)
            {
                var obj = new ExportObject();
                try
                {
                    obj.DealId = d.Id;
                    obj.EndUserCHS = d.Customer == null ? "" : d.Customer.NameCHS;
                    obj.EndUserENG = d.Customer == null ? "" : d.Customer.NameENG;
                    obj.Country = "China";
                    obj.Province1 = d.Customer == null ? "" : d.Customer.Province;
                    obj.Province2 = d.Province == null ? "" : d.Province.Name;
                    obj.Industry1 = d.Industry == null ? "" : d.Industry.Name;
                    obj.Industry2 = d.Industry2 == null ? "" : d.Industry2.Name;
                    obj.ORPName = d.Name;
                    obj.ProductName = pd.ProductName;
                    obj.ProductStatus = GetProductStatus(pd);
                    obj.PartnerName = d.Partner == null ? "" : d.Partner.Name;
                    obj.PartnerType = d.Partner == null ? "" : d.Partner.Qualification;
                    obj.TotalPrice = d.TotalPrice.ToString(CultureInfo.InvariantCulture);
                    obj.DealSize = d.DealSize;
                    obj.Assigner = d.Assigner == null ? "" : d.Assigner.Name;
                    obj.AssignDate = d.AssignDate.ToString();
                    obj.Approver = d.Approver == null ? "" : d.Approver.Name;
                    obj.ApproveDate = d.ApproveDate.ToString();
                    obj.ChannelDirector = d.Director == null ? "" : d.Director.Name;
                    obj.DirectorDate = d.DirectorDate.ToString();
                    obj.Status = d.Status == null ? "" : d.Status.Name;
                    obj.ContractUploaded = (d.DealContracts.Any()).ToString();
                    obj.AssignedSalesName = d.Notifier == null ? "" : d.Notifier.Name;
                    rslt.Add(obj);
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
                }
            }
            return rslt.ToArray();
        }


        private static string GetProductStatus(DealProduct dealProduct)
        {
            var deal = dealProduct.Deal;
            if (deal.Status.ActionName != COATStatusValue.Approved)
                return deal.Status.Name;

            return dealProduct.IsActive ? "Approve" : "Reject";
        }
    }
}