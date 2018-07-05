using CinderellaCore.Model.Enums;

namespace CinderellaCore.Web.Models
{
    public class QueueItemViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int QueueRank { get; set; }
        public ItemType ItemType { get; set; }
    }
}