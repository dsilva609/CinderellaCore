using CinderellaCore.Model.Enums;
using CinderellaCore.Model.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace CinderellaCore.Web.Models
{
    public class BookViewModel : BaseViewModel
    {
        public IPagedList<Book> Books { get; set; }

        public BookInfoModel BookInfo { get; set; }
        public BookMediaInfoModel MediaInfo { get; set; }
        public PurchaseInfoViewModel PurchaseInfo { get; set; }
        public ItemStatusViewModel StatusInfo { get; set; }
    }

    public class BookInfoModel : ItemInfoViewModel
    {
        [Required]
        public string Author { get; set; }

        [Required]
        public string Publisher { get; set; }

        public string GoogleBookID { get; set; }
    }

    public class BookMediaInfoModel : MediaInfoViewModel
    {
        [Required]
        public BookTypeEnum Type { get; set; }

        public bool Hardcover { get; set; }

        [Display(Name = "First Edition?")]
        public bool IsFirstEdition { get; set; }

        [DisplayName("Page Count")]
        public int PageCount { get; set; }

        [DisplayName("ISBN 10")]
        public string ISBN10 { get; set; }

        [DisplayName("ISBN 13")]
        public string ISBN13 { get; set; }

        [Display(Name = "Reissue?")]
        public bool IsReissue { get; set; }
    }
}