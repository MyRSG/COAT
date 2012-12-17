using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web.Mvc;
using COAT.Models;
using COAT.Util.Extension;
using COAT.ViewModel.Shared;
using MvcContrib.Pagination;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;

namespace COAT.Controllers
{
    public abstract class COATController : BaseController
    {
        protected string ApproveActionName = "Approve";
        protected abstract int PreviousStatusId { get; }
        protected abstract int NextStatusId { get; }
        protected abstract IEnumerable<SelectListItem> AssignToList { get; }

        #region "Vitual Methods"

        #region "Http Methods"

        [Authorize]
        public virtual ActionResult Details(string id)
        {
            Deal deal = Db.Deals.Single(d => d.Id == id);
            ViewBag.AssignToList = AssignToList;
            return View(deal);
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult Approve(Deal deal, FormCollection collection)
        {
            OnApproving(deal, collection);

            Deal dbDeal = GetDeal(deal.Id);
            dbDeal.StatusId = NextStatusId;

            UpdateProducts(collection);
            SaveApproveHistory(dbDeal.Id, ApproveActionName, GetComment(collection));

            Db.SaveChanges();

            OnApproved(dbDeal, collection);
            return Redirect("../Index");
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult Reject(Deal deal, FormCollection collection)
        {
            Deal dbDeal = GetDeal(deal.Id);
            //var products = dbDeal.DealProducts;
            //var productActive = new Dictionary<int, bool>();
            //foreach (var p in products)
            //{
            //    productActive.Add(p.Id, false);
            //}
            //UpdateProducts(productActive);
            Status firstOrDefault = Db.Status.FirstOrDefault(a => a.ActionName == "R");
            if (firstOrDefault != null)
                dbDeal.StatusId = firstOrDefault.Id;
            SaveApproveHistory(dbDeal.Id, "Reject", GetComment(collection));
            Db.SaveChanges();

            return Redirect("../Index");
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult Wrong(Deal deal, FormCollection collection)
        {
            Deal dbDeal = GetDeal(deal.Id);
            EntityCollection<DealProduct> products = dbDeal.DealProducts;
            Dictionary<int, bool> productActive = products.ToDictionary(p => p.Id, p => true);
            UpdateProducts(productActive);
            dbDeal.StatusId = PreviousStatusId;
            dbDeal.ApproveDate = null;
            SaveApproveHistory(dbDeal.Id, "Assign Wrong", GetComment(collection));
            Db.SaveChanges();

            return Redirect("../Index");
        }

        #endregion
        
        #region "Protected Methods"
        protected virtual void OnApproving(Deal deal, FormCollection collection)
        {
        }

        protected virtual void OnApproved(Deal deal, FormCollection collection)
        {
        }

        protected virtual string GetComment(FormCollection collection)
        {
            try
            {
                return collection["comment"]??string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

      

        protected IPagination<Deal> SortAndPagingDeal(IQueryable<Deal> deals, GridSortOptions sort, int? page)
        {
            bool descending = sort.Direction == SortDirection.Descending;
            var orderMethodDic = new Dictionary<string, Func<IQueryable<Deal>, IOrderedQueryable<Deal>>>();

            InitialOrderMethodDic(descending, orderMethodDic);

            if (!string.IsNullOrWhiteSpace(sort.Column) && orderMethodDic.ContainsKey(sort.Column))
            {
                return orderMethodDic[sort.Column](deals).AsPagination(page ?? 1, 20);
            }

            return deals.OrderBy(a => a.Id).AsPagination(page ?? 1, 20);
        }

        protected void UpdateProducts(FormCollection collection)
        {
            Dictionary<int, bool> productActive = GetProductsActive(collection);
            UpdateProducts(productActive);
        }

        protected Dictionary<int, bool> GetProductsActive(FormCollection collection)
        {
            var rslt = new Dictionary<int, bool>();
            string header = new DealProductViewModel().ActionInputHead;
            foreach (string key in collection.AllKeys)
            {
                if (key == null || key.IndexOf(header, StringComparison.Ordinal) < 0)
                    continue;

                int id = int.Parse(key.Remove(0, header.Length));
                bool active = collection[key].ToLower().Contains("true");
                rslt.Add(id, active);
            }

            return rslt;
        }

        protected int GetAssignToId(FormCollection collection)
        {
            return int.Parse(collection["assignToId"]);
        }

        protected User GetAssignUser(FormCollection collection)
        {
            int userId = GetAssignToId(collection);
            return Db.Users.FirstOrDefault(a => a.Id == userId);
        }

        #endregion

        private static void InitialOrderMethodDic(bool descending,
                                                  Dictionary<string, Func<IQueryable<Deal>, IOrderedQueryable<Deal>>>
                                                      orderMethodDic)
        {
            orderMethodDic.Add("Name", d => d.OrderByDirection(dd => dd.Name));
            orderMethodDic.Add("Customer", d => d.OrderByDirection(dd => dd.Customer.NameENG, descending));
            orderMethodDic.Add("Partner", d => d.OrderByDirection(dd => dd.Partner.Name, descending));
            orderMethodDic.Add("Industry", d => d.OrderByDirection(dd => dd.Industry.Name, descending));
            orderMethodDic.Add("Owner", d => d.OrderByDirection(dd => dd.Owner, descending));
            orderMethodDic.Add("District", d => d.OrderByDirection(dd => dd.District, descending));
            orderMethodDic.Add("Status", d => d.OrderByDirection(dd => dd.Status.Name, descending));
            orderMethodDic.Add("CreateDate", d => d.OrderByDirection(dd => dd.CreateDate, descending));
            orderMethodDic.Add("Industry2", d => d.OrderByDirection(dd => dd.Industry2.Name, descending));
            orderMethodDic.Add("Province", d => d.OrderByDirection(dd => dd.Province.Name, descending));
            orderMethodDic.Add("Deal$",
                               d =>
                               d.OrderByDirection(dd => dd.DealProducts.Where(p => p.IsActive).Sum(p => p.Price),
                                                  descending));
            orderMethodDic.Add("City",
                               d => d.OrderByDirection(dd => dd.DirectorDate, descending).ThenBy(dd => dd.ApproveDate));
        }
    }
}