using RosyMovieShop.Models.Db;

namespace RosyMovieShop.Services
{
	public interface ICustomerService
	{
		public void EditCustomer(Customer customer);
		public void CreateCustomer(Customer customer);
		public void DeleteCustomer(int id);
		public List<Customer> GetAllCustomers();
		public Customer TopCustomer();

		public Customer GetCustomer(int id);
		public bool CustomerExists(int id);
		public bool CustomerExist(string email);
		public Customer GetCustomerFromEmail(string email);

	}
}
