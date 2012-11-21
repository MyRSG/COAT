using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using COAT.Models;
using COAT.Util.IDS;

namespace COAT.ViewModel.Shared
{
    public class DealSearchViewModel
    {
        private readonly COATEntities _db = new COATEntities();

        public string DealId { get; set; }
        public int ValidationTeamId { get; set; }
        public int ProvinceId { get; set; }
        public int Industry2Id { get; set; }
        public int ORPTypeId { get; set; }
        public string SpecializationsName { get; set; }
        public string Region { get; set; }
        public string COATStatusActionName { get; set; }
        public int SFDCStatusId { get; set; }
        public int ApproverId { get; set; }
        public string CustomerName { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? BeginActDate { get; set; }
        public DateTime? EndActDate { get; set; }
        

        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public IEnumerable<SelectListItem> ORPTypeList
        {
            get
            {
                return AddSelectListItem(
                    _db.ORPTypes.ToList().Select(
                        a => new SelectListItem {Text = a.Name, Value = a.Id.ToString(CultureInfo.InvariantCulture), Selected = a.Id == ORPTypeId}),
                    new SelectListItem {Text = "All Specialization ORP", Value = "0", Selected = ORPTypeId == 0});
            }
        }

        public IEnumerable<SelectListItem> RegionList
        {
            get
            {
                return AddSelectListItem(
                    _db.Regions.Select(
                        a =>
                        new SelectListItem
                            {
                                Text = a.Name,
                                Value = a.Name,
                                Selected = !string.IsNullOrEmpty(Region) && a.Name == Region
                            }),
                    new SelectListItem {Text = "All", Value = "", Selected = string.IsNullOrEmpty(Region)});
            }
        }

        public IEnumerable<SelectListItem> COATStatusList
        {
            get
            {
                return AddSelectListItem(
                    _db.Status.Select(
                        a =>
                        new SelectListItem
                            {
                                Text = a.Name,
                                Value = a.ActionName,
                                Selected =
                                    !string.IsNullOrEmpty(COATStatusActionName) && a.ActionName == COATStatusActionName
                            }),
                    new SelectListItem {Text = "All", Value = "", Selected = string.IsNullOrEmpty(COATStatusActionName)});
            }
        }

        public IEnumerable<SelectListItem> SFDCStatusList
        {
            get
            {
                return AddSelectListItem(
                    _db.SFDCStatus.ToList().Select(
                        a =>
                        new SelectListItem {Text = a.Name, Value = a.Id.ToString(CultureInfo.InvariantCulture), Selected = a.Id == SFDCStatusId}),
                    new SelectListItem {Text = "All", Value = "0", Selected = SFDCStatusId == 0});
            }
        }

        public IEnumerable<SelectListItem> ApproverList
        {
            get
            {
                return AddSelectListItem(
                    _db.Users.Where(u =>
                                   u.SystemRoleId == SystemRoleIds.ChannelApprover
                                   || u.SystemRoleId == SystemRoleIds.SalesApprover
                                   || u.SystemRoleId == SystemRoleIds.ChannelDirector)
                        .ToList().Select(
                            a =>
                            new SelectListItem {Text = a.Name, Value = a.Id.ToString(CultureInfo.InvariantCulture), Selected = a.Id == ApproverId})
                        .OrderBy(a => a.Text),
                    new SelectListItem {Text = "All", Value = "0", Selected = ApproverId == 0});
            }
        }

        public IEnumerable<SelectListItem> ValidationTeamList
        {
            get
            {
                var rslt = new List<SelectListItem>
                               {
                                   new SelectListItem {Text = "All", Value = "0", Selected = ValidationTeamId == 0},
                                   new SelectListItem
                                       {Text = "Volume Sales Team", Value = "1", Selected = ValidationTeamId == 1},
                                   new SelectListItem
                                       {Text = "Enterprise Channel Team", Value = "2", Selected = ValidationTeamId == 2}
                               };

                return rslt;
            }
        }

        public IEnumerable<SelectListItem> IndustryList
        {
            get
            {
                return AddSelectListItem(
                    _db.Industry2.ToList().Select(
                        a => new SelectListItem {Text = a.Name, Value = a.Id.ToString(CultureInfo.InvariantCulture), Selected = a.Id == Industry2Id}),
                    new SelectListItem {Text = "All", Value = "0", Selected = Industry2Id == 0});
            }
        }

        public IEnumerable<SelectListItem> ProvinceList
        {
            get
            {
                return AddSelectListItem(
                    _db.Provinces.OrderBy(a => a.Name)
                        .ToList().Select(
                            a =>
                            new SelectListItem {Text = a.Name, Value = a.Id.ToString(CultureInfo.InvariantCulture), Selected = a.Id == ProvinceId}),
                    new SelectListItem {Text = "All", Value = "0", Selected = ProvinceId == 0});
            }
        }

        public IEnumerable<SelectListItem> SpecializationList
        {
            get
            {
                return AddSelectListItem(
                    _db.Specializations.OrderBy(a=>a.FullName)
                    .ToList().Select(
                    a=>
                        new SelectListItem { Text = a.FullName, Value = a.FullName, Selected = a.FullName == SpecializationsName })
                    .Distinct(new SelectListItemEqualityComparer()),
              new SelectListItem { Text = "All", Value = "", Selected = string.IsNullOrEmpty(SpecializationsName) });
            }
        }

        private static IEnumerable<SelectListItem> AddSelectListItem(IEnumerable<SelectListItem> list,
                                                                     SelectListItem item)
        {
            List<SelectListItem> rslt = list.ToList();
            rslt.Add(item);
            return rslt;
        }

        class SelectListItemEqualityComparer:IEqualityComparer<SelectListItem>
        {
            public bool Equals(SelectListItem x, SelectListItem y)
            {
                return (x.Text == y.Text) && (x.Value == y.Value);
            }

            public int GetHashCode(SelectListItem obj)
            {
                // This hash code is not right, but I can't find another way is better than this.
                return string.Format("{0}{1}",obj.Text,obj.Value).GetHashCode();
            }
        }
    }
}