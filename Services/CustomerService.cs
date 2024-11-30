using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RosyMovieShop.Data;
using RosyMovieShop.Models.Db;

namespace RosyMovieShop.Services
{
	public class CustomerService:ICustomerService
	{
		private readonly RSDBContext _context;

        public CustomerService(RSDBContext db)
        {
            _context=db;
        }

		public void EditCustomer(Customer customer)
		{

			_context.Customers.Update(customer);
			_context.SaveChanges();

		}
		public void CreateCustomer(Customer customer)
		{
			if (customer != null)
			{
				_context.Customers.Add(customer);
				_context.SaveChanges();
			}
		}
		public void DeleteCustomer(int id)
		{
			var customer = _context.Customers.Find(id);
			if (customer != null)
			{
				_context.Customers.Remove(customer);
				_context.SaveChanges();
			}
		}

		public bool CustomerExists(int id)
		{
			return _context.Customers.Any(x => x.Id == id);
		}
		public bool CustomerExist(string email)
		{
			return _context.Customers.Any(x => x.Email == email);
		}


		public List<Customer> GetAllCustomers()
		{
			return _context.Customers.ToList();
		}

		public Customer GetCustomer(int id)
		{
			var customer = _context.Customers.FirstOrDefault(x => x.Id == id);
			return customer;
		}

		public Customer GetCustomerFromEmail(string email)
		{
			var customer = _context.Customers.FirstOrDefault(x => x.Email == email);
			return customer;
		}

		public Customer TopCustomer()
		{
			var tcustomer=_context.Customers.Include(c=>c.Orders)
					.ThenInclude(c=>c.OrderRows).ToList()
					.Select(c => new
					{
						Customer=c,
						Spent=c.Orders.Sum(o =>o.OrderRows.Sum(or=> or.Price))
					}).OrderByDescending(c=>c.Spent).FirstOrDefault()?.Customer;
				return tcustomer;
		}

		//public List<Movie> GetAllMoviesWithOrder()
		//{
		//	var movies= _context.Movies.Include(mo => mo.OrderRows).ToList();
		//	return movies;
		//}
	}
}
