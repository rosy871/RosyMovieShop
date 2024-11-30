using System.ComponentModel.DataAnnotations;

namespace RosyMovieShop.Models.Db
{
	public class Customer
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "First Name is required")]
		[StringLength(20, ErrorMessage = "CustomerName cannot be longer than 20 characters")]
		public string FristName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Last Name is required")]
		[StringLength(20, ErrorMessage = "Last Name cannot be longer than 20 characters")]
		public string LastName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress]
		[StringLength(200)]
		public string Email { get; set; } = string.Empty;

		[StringLength(25)]
		public string BillingAddress { get; set; } = string.Empty;

		[StringLength(10)]
		public string BillingZip { get; set; } = string.Empty;

		[StringLength(25)]
		public string BillingCity { get; set; } = string.Empty;

		[StringLength(25)]
		[Required(ErrorMessage = "Delivery Address is required")]

		public string DeliveryAddress { get; set; } = string.Empty;

		[StringLength(10)]
		[Required(ErrorMessage = "Delivery Zip is required")]

		public string DeliveryZip { get; set; } = string.Empty;

		[Required(ErrorMessage = "Delivery City is required")]

		[StringLength(25)]
		public string DeliveryCity { get; set; } = string.Empty;

		[StringLength(25)]
		public string PhoneNo { get; set; } = string.Empty;

		public List<Order> Orders { get; set; } = new List<Order>();


	}
}
