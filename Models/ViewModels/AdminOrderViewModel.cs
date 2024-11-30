using RosyMovieShop.Models.Db;

namespace RosyMovieShop.Models.ViewModels
{
    public class AdminOrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderByCus { get; set; }
        public int CustomerId { get; set; }
        public Order Order { get; set; }
        public string DeliveryAddress { get; set; }

        public List<IndividualCartOrderViewModel> InMvovList { get; set; } = new List<IndividualCartOrderViewModel>();

    }
}
