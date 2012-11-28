using System.Linq;
using COAT.Models;
using COAT.ViewModel.Shared;

namespace COAT.COATExtension
{
    public static class QueryableDealExtension
    {
        public static IQueryable<Deal> Search(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            return deals
                .SearchDealId(model)
                .SearchValidationTeam(model)
                .SearchORPType(model)
                .SearchRegion(model)
                .SearchProvince(model)
                .SearchIndustry2(model)
                .SearchCOATStatus(model)
                .SearchSFDCStatus(model)
                .SearchCustomer(model)
                .SearchApprover(model)
                .SearchBeginDate(model)
                .SearchEndDate(model)
                .SearchBeginActDate(model)
                .SearchEndActDate(model)
                .SearchSpecialization(model);
        }

        public static IQueryable<Deal> SearchBeginActDate(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (model.BeginActDate == null)
                return deals;

            return deals
                .Where(a => a.Status.ActionName == "A" || a.Status.ActionName == "R")
                .Where(
                    a =>
                    (a.ApproveDate >= model.BeginActDate && a.DirectorDate == null) ||
                    a.DirectorDate >= model.BeginActDate);
        }

        public static IQueryable<Deal> SearchEndActDate(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (model.EndActDate == null)
                return deals;

            return deals
                .Where(a => a.Status.ActionName == "A" || a.Status.ActionName == "R")
                .Where(
                    a =>
                    (a.ApproveDate <= model.EndActDate && a.DirectorDate == null) || a.DirectorDate <= model.EndActDate);
        }

        public static IQueryable<Deal> SearchDealId(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (string.IsNullOrEmpty(model.DealId))
                return deals;

            return deals.Where(a => model.DealId.Contains(a.Id));
        }

        public static IQueryable<Deal> SearchValidationTeam(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (model.ValidationTeamId == 0)
                return deals;

            string[] ids = new COATEntities().SalesDealsViews.Select(a => a.Id).ToArray();
            if (model.ValidationTeamId == 1)
            {
                return deals.Where(a => ids.Contains(a.Id));
            }

            return deals.Where(a => !ids.Contains(a.Id));
        }

        public static IQueryable<Deal> SearchORPType(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (model.ORPTypeId == 0)
                return deals;

            return deals;
        }

        public static IQueryable<Deal> SearchRegion(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (string.IsNullOrEmpty(model.Region))
                return deals;

            return deals.Where(d => d.ChinaRegion == model.Region);
        }

        public static IQueryable<Deal> SearchProvince(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (model.ProvinceId == 0)
                return deals;

            return deals.Where(a => a.ProvinceId == model.ProvinceId);
        }

        public static IQueryable<Deal> SearchIndustry2(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (model.Industry2Id == 0)
                return deals;

            return deals.Where(a => a.Industry2Id == model.Industry2Id);
        }

        public static IQueryable<Deal> SearchCOATStatus(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (string.IsNullOrEmpty(model.COATStatusActionName))
                return deals;

            return deals.Where(d => d.Status.ActionName == model.COATStatusActionName);
        }

        public static IQueryable<Deal> SearchSFDCStatus(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (model.SFDCStatusId == 0)
                return deals;

            return deals.Where(d => d.RegistStatus == model.SFDCStatusId);
        }

        public static IQueryable<Deal> SearchApprover(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (model.ApproverId == 0)
                return deals;
            return deals.Where(d => d.ApproverId == model.ApproverId);
        }

        public static IQueryable<Deal> SearchCustomer(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (string.IsNullOrEmpty(model.CustomerName))
                return deals;
            return
                deals.Where(
                    d =>
                    d.Customer.NameENG.Contains(model.CustomerName) || d.Customer.NameCHS.Contains(model.CustomerName));
        }

        public static IQueryable<Deal> SearchBeginDate(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (model.BeginDate == null)
                return deals;

            return deals.Where(d => d.CreateDate >= model.BeginDate);
        }

        public static IQueryable<Deal> SearchEndDate(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (model.EndDate == null)
                return deals;

            return deals.Where(d => d.CreateDate <= model.EndDate);
        }

        public static IQueryable<Deal> SearchSpecialization(this IQueryable<Deal> deals, DealSearchViewModel model)
        {
            if (string.IsNullOrEmpty(model.SpecializationsName))
                return deals;

            return deals.Where(d => d.Specialization.FullName == model.SpecializationsName);
        }
    }
}