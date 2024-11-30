using RosyMovieShop.Models.Db;

namespace RosyMovieShop.Models.ViewModels
{
	public class AdminIndexMovieCusomerList
	{
		public List<Movie> Movies { get; set; } = new List<Movie>();

		public List<Customer> Customers { get; set; } = new List<Customer>();


	}
}
