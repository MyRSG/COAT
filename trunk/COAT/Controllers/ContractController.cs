using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAT.ViewModel.Shared;
using MvcContrib.UI.Grid;
using COAT.COATExtension;
using COAT.IDS;
using COAT.Helper;
using COAT.Models;

namespace COAT.Controllers
{
    [Authorize(Roles = "Admin,ChannelApprover")]
    public class ContractController : COATController
    {
        FileHelper fHelper = new FileHelper();

        //
        // GET: /Contract/

        public ViewResult Index(DealSearchViewModel search, GridSortOptions sort, int? page)
        {
            search.ControllerName = "Contract";
            search.ActionName = "Index";
            ViewBag.SearchModel = search;

            ViewBag.Sort = sort;
            var user = GetCurrentMemberShipUser();
            var deals = db.Deals.Where(a => a.ApproverId == user.Id || user.SystemRole.Id == SystemRoleIds.Admin).Search(search);
            ViewBag.SummaryDeals = deals;



            return View(SortAndPagingDeal(deals, sort, page));
        }

        public override ActionResult Details(string id)
        {
            var ctrIds = db.Deals.FirstOrDefault(a => a.Id == id).DealContracts.Select(a => a.ContractId);
            var ids = db.Contracts.Where(a => ctrIds.Contains(a.Id)).Select(a => a.FileStoreId);
            ViewBag.Contract = db.FileStores.Where(a => ids.Contains(a.Id));
            return base.Details(id);
        }

        public ActionResult Upload(string id, HttpPostedFileBase[] contract)
        {
            foreach (var c in contract)
            {
                if (c == null || c.ContentLength == 0)
                    continue;

                SaveContract(id, c);
            }

            return RedirectToAction("Details", new { id = id });
        }

        private void SaveContract(string id, HttpPostedFileBase c)
        {
            var deal = db.Deals.FirstOrDefault(a => a.Id == id);
            if (deal == null)
                return;

            var fs = fHelper.SaveContractFile(c);
            var contract = new Contract { FileStoreId = fs.Id };
            db.Contracts.AddObject(contract);
            db.SaveChanges();

            var dealContract = new DealContract { DealId = deal.Id, ContractId = contract.Id };
            db.DealContracts.AddObject(dealContract);
            db.SaveChanges();

        }


        protected override int PreviousStatusId
        {
            get { throw new NotImplementedException(); }
        }

        protected override int NextStatusId
        {
            get { throw new NotImplementedException(); }
        }

        protected override IEnumerable<SelectListItem> AssignToList
        {
            get { return null; }
        }
    }
}
