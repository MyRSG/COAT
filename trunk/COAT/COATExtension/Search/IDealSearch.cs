using System.Linq;
using COAT.Models;
using COAT.ViewModel.Shared;

namespace COAT.COATExtension.Search
{
    internal interface IDealSearch
    {
        IQueryable<Deal> Search(IQueryable<Deal> deals, DealSearchViewModel model);
    }
}