namespace RosyMovieShop.Models.Db
{
	public class Order
	{
		public int Id { get; set; }
		public DateTime OrderDate { get; set; }

		public int CustomerId { get; set; }
		public Customer Customer { get; set; } = null!;

		public List<OrderRow> OrderRows { get; set; } = new List<OrderRow>();



	}
}
