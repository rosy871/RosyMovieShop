using Microsoft.EntityFrameworkCore;
using RosyMovieShop.Models.Db;

namespace RosyMovieShop.Data
{
	public class RSDBContext : DbContext
	{
        public RSDBContext(DbContextOptions options):base(options)
        { }

        public DbSet<Customer> Customers { get; set; }
		public DbSet<Movie> Movies { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderRow> OrderRows { get; set; }
	}

}
