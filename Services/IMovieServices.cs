using RosyMovieShop.Models.Db;

namespace RosyMovieShop.Services
{
	public interface IMovieServices
	{
		public void UpdateMovie(Movie movie);

		//public CustomerViewModel TopMovies();
		public void CreateMovie(Movie movie);
		public void DeleteMovie(int id);
		public List<Movie> GetAllMovies();
		public List<Movie> GetAllMoviesWithOrder();


		public Movie GetMovie(int id);

		public bool MovieExists(int id);

	}
}
