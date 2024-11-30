using RosyMovieShop.Models.Db;

namespace RosyMovieShop.Models.ViewModels
{
    public class CartViewModel
    {
        public List<IndividualCartOrderViewModel> ListMovie { get; set; } = new List<IndividualCartOrderViewModel>();
        public Customer Customer { get; set; }

    }
}
