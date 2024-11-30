namespace RosyMovieShop.Models.Db
{
	public class OrderRow
	{
		public int Id { get; set; }
		public double Price { get; set; }

		//fks

		public int OrderId { get; set; }
		public int MovieId { get; set; }

		//nav
		public Movie Movie { get; set; }

		public Order Order { get; set; }


	}
}
