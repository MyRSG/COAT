using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAT.COATExtension;
using COAT.Helper;
using COAT.Models;
using COAT.Security;
using COAT.Util.IDS;
using COAT.ViewModel.Shared;
using MvcContrib.UI.Grid;

namespace COAT.Controllers
{
    [Authorize(Roles = "Admin,ChannelApprover")]
    public class ContractController : COATController
    {
        private readonly FileHelper _fHelper = new FileHelper();

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

        //
        // GET: /Contract/

        public ViewResult Index(DealSearchViewModel search, GridSortOptions sort, int? page)
        {
            search.ControllerName = "Contract";
            search.ActionName = "Index";
            ViewBag.SearchModel = search;

            ViewBag.Sort = sort;
            COATMemebershipUser user = GetCurrentMemberShipUser();
            IQueryable<Deal> deals =
                Db.Deals.Where(a => a.ApproverId == user.Id || user.SystemRole.Id == SystemRoleIds.Admin).Search(search);
            ViewBag.SummaryDeals = deals;


            return View(SortAndPagingDeal(deals, sort, page));
        }

        public override ActionResult Details(string id)
        {
            Deal firstOrDefault = Db.Deals.FirstOrDefault(a => a.Id == id);
            if (firstOrDefault != null)
            {
                IEnumerable<int> ctrIds = firstOrDefault.DealContracts.Select(a => a.ContractId);
                IQueryable<int> ids = Db.Contracts.Where(a => ctrIds.Contains(a.Id)).Select(a => a.FileStoreId);
                ViewBag.Contract = Db.FileStores.Where(a => ids.Contains(a.Id));
            }
            return base.Details(id);
        }

        public ActionResult Upload(string id, HttpPostedFileBase[] contract)
        {
            foreach (HttpPostedFileBase c in contract)
            {
                if (c == null || c.ContentLength == 0)
                    continue;

                SaveContract(id, c);
            }

            return RedirectToAction("Details", new {id});
        }

        private void SaveContract(string id, HttpPostedFileBase c)
        {
            Deal deal = Db.Deals.FirstOrDefault(a => a.Id == id);
            if (deal == null)
                return;

            FileStore fs = _fHelper.SaveContractFile(c);
            var contract = new Contract {FileStoreId = fs.Id};
            Db.Contracts.AddObject(contract);
            Db.SaveChanges();

            var dealContract = new DealContract {DealId = deal.Id, ContractId = contract.Id};
            Db.DealContracts.AddObject(dealContract);
            Db.SaveChanges();
        }
    }
}