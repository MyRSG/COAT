using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COAT.Models;
using COAT.ViewModel.Shared;

namespace COAT.COATExtension.Search
{
    interface IDealSearch
    {
        IQueryable<Deal> Search(IQueryable<Deal> deals, DealSearchViewModel model);
    }
}
