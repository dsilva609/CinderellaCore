using CinderellaCore.Model.Models;
using X.PagedList;

namespace CinderellaCore.Web.Models
{
    public class PopViewModel : BaseViewModel
    {
        public IPagedList<FunkoPop> Pops { get; set; }
    }
}