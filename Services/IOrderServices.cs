using RosyMovieShop.Models.Db;
using RosyMovieShop.Models.ViewModels;

namespace RosyMovieShop.Services
{
	public interface IOrderServices
	{
		public void AddOrder(List<Movie> movies, int cusId);
		public void DeleteOrder(int id);
		public Order GetOrder(int id);
		public bool OrderExists(int id);
		public IEnumerable<Order> GetOrdersByCustomerId(int id);

		public List<Order> GetAllOrdersListCOR();

		//public IEnumerable<OrderViewModel> GetOrdersByCustomerEmail(string email);


	}
}
