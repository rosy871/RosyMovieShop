using RosyMovieShop.Models.Db;

namespace RosyMovieShop.Models.ViewModels
{
	public class CustomerIndexViewModel
	{
		public List<Movie> MostPopularMovie { get; set; } = new List<Movie>();
		public List<Movie> Top5NewestMovies { get; set; } = new List<Movie>();
		public List<Movie> Top5OldestMovies { get; set; } = new List<Movie>();
		public List<Movie> Top5CheapestMovies { get; set; } = new List<Movie>();
		public Customer CustomerSpentMostMoney { get; set; } = new Customer();

	}
}
