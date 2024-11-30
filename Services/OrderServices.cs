using Microsoft.EntityFrameworkCore;
using RosyMovieShop.Data;
using RosyMovieShop.Models.Db;
using RosyMovieShop.Models.ViewModels;

namespace RosyMovieShop.Services
{
	public class OrderServices:IOrderServices
	{

		private readonly RSDBContext _db;

		public OrderServices(RSDBContext db)
		{
			_db = db;
		}

		public void AddOrder(List<Movie> movies, int cusId)
		{
			//if (order != null) _db.Orders.Add(order);
			//_db.SaveChanges();
			Order newOrder = new Order()
			{
				CustomerId = cusId,
				OrderDate = DateTime.Now
			};
			foreach (Movie movie in movies)
			{
				OrderRow row = new OrderRow()
				{
					MovieId = movie.Id,
					Price = movie.Price,
				};
				newOrder.OrderRows.Add(row);
			}
			_db.Orders.Add(newOrder);
			_db.SaveChanges();
		}


		public void DeleteOrder(int id)
		{
			var order = _db.Orders.Include(or => or.OrderRows).FirstOrDefault(o => o.Id == id);

			_db.Orders.Remove(order);
			_db.SaveChanges();

		}


		public Order GetOrder(int id)
		{
			var order = _db.Orders.Include(or => or.OrderRows).ThenInclude(m => m.Movie).FirstOrDefault(o => o.Id == id);
			return order;
		}

		public bool OrderExists(int id)
		{
			return _db.Orders.Any(m => m.Id == id);
		}


		public List<Order> GetAllOrdersListCOR()
		{
			var order = _db.Orders.Include(c => c.Customer).Include(or => or.OrderRows).ToList();
			return order;
		}



		public IEnumerable<Order> GetOrdersByCustomerId(int  id)
		{
			var orders = _db.Orders.Include(o => o.OrderRows).ThenInclude(or => or.Movie)
				.Where(o => o.CustomerId == id)
				.ToList();

			return orders;
		}

	}
}
