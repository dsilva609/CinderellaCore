using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinderellaCore.Model.Models;
using X.PagedList;

namespace CinderellaCore.Web.Models
{
    public class PopViewModel : BaseViewModel
    {
        public IPagedList<FunkoPop> Pops { get; set; }
    }
}