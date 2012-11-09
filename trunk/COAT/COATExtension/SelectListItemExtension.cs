using System.Web.Mvc;

namespace COAT.COATExtension
{
    public static class SelectListItemExtension
    {
        public static SelectList Select(this SelectList list, string val)
        {
            if (val == null)
                return list;

            object selVal = null;
            foreach (SelectListItem item in list)
            {
                if (item.Value == val)
                {
                    selVal = item.Value;
                }
            }

            return new SelectList(list.Items, list.DataValueField, list.DataTextField, selVal);
        }
    }
}