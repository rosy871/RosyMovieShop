using RosyMovieShop.Models.Db;

namespace RosyMovieShop.Models.ViewModels
{
	public class CustomerOrderViewModel
	{
		public int OrderId { get; set; }
		public DateTime OrderDate { get; set; }
		public double TotalCost { get; set; }
		public List<IndividualCartOrderViewModel> Movies { get; set; }
	}
}
