using Microsoft.AspNetCore.Mvc;
using RosyMovieShop.Data;
using RosyMovieShop.Helper;
using RosyMovieShop.Models.Db;
using RosyMovieShop.Models.ViewModels;
using RosyMovieShop.Helper;
using RosyMovieShop.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace RosyMovieShop.Controllers
{
	public class AdminController : Controller
	{
		private readonly ICustomerService _ics;
		private readonly IMovieServices _ims;
		private readonly IOrderServices _ios;


		public AdminController(ICustomerService cid, IMovieServices mid, IOrderServices oid)
		{
			_ics = cid;
			_ims = mid;
			_ios = oid;
			UserRole.URole = Role.Admin;

		}


		public IActionResult LoginAdmin()
		{

			return View();

		}
		[HttpPost]
		public IActionResult LoginAdmin (Adminlogin log)
		{

			var admin = new Adminlog();

			if (admin.username == log.Username && admin.password == log.Password)
			{
				HttpContext.Session.SetString("SessionAdmin", log.Username);

				return RedirectToAction("Index");
			}
			else {
				TempData["ErrorMsg"] = "username/password doesnot match";
			return View();
			}					
			
		}

		public IActionResult Logout()
		{


			HttpContext.Session.Remove("SessionAdmin");
			TempData.Clear();
			return RedirectToAction("Index", "Customer");


		}
		public IActionResult Index()
		{
			AdminIndexMovieCusomerList obj = new AdminIndexMovieCusomerList();
			var cutomers = _ics.GetAllCustomers().OrderBy(c => c.FristName).ThenBy(c => c.LastName).ToList();
			obj.Customers = cutomers;

			List<Movie> movies = _ims.GetAllMovies().Where(m => m.Active == true).OrderByDescending(m => m.ReleaseYear).ThenBy(m => m.Title).ToList();
			obj.Movies = movies;

			return View(obj);
		}


        public IActionResult MovieDetails(int id)
        {
            if (id == 0)
                return NotFound();
            var movie = _ims.GetMovie(id);
            if (movie == null)
                return NotFound();


            return View(movie);
        }




        [HttpGet]
		public IActionResult CreateMovie()
		{
			return View();
		}
		[HttpPost]
		public IActionResult CreateMovie(Movie movie)
		{
			if (ModelState.IsValid)
			{
				_ims.CreateMovie(movie);
				TempData["Message"] = $"{movie.Title} is Added succesfully";
				return RedirectToAction("Index"); // INDEX FOR NOW, CHANGE LATER TO MOVIE LIST MAYBE
			}

			return View();
		}

		[HttpGet]
		public IActionResult EditMovie(int id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var movie = _ims.GetMovie(id);
			if (movie == null)
			{
				return NotFound();
			}
			return View(movie);
		}

		[HttpPost]
		public IActionResult EditMovie(int id, Movie movie)
		{
			if (id != movie.Id)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				_ims.UpdateMovie(movie);
				TempData["Message"] = $"{movie.Title} is Updated succesfully";

				return RedirectToAction("Index");
			}
			return View(movie);
		}

		public IActionResult CustomerDetail(int id)
		{
			if (id == null)
				return NotFound();
			var customer = _ics.GetCustomer(id);
			if (customer == null)
				return NotFound();
			return View(customer);
		}

		[HttpGet]
		public IActionResult EditCustomer(int id)
		{
			if (id == null)
				return NotFound();
			var customer = _ics.GetCustomer(id);
			if (customer == null)
				return NotFound();
			return View(customer);
		}
		[HttpPost]
		public IActionResult EditCustomer(int id, Customer customer)
		{
			if (ModelState.IsValid)
			{
				_ics.EditCustomer(customer);
				TempData["Message"] = $"{customer.FristName} is Updated succesfully";

				return RedirectToAction("Index"); // CHANGE LATER
			}
			return View(customer);

		}
		[HttpGet]
		public IActionResult DeleteCustomer(int id)
		{
			if (id == null)
				return NotFound();
			var customer = _ics.GetCustomer(id);
			if (customer == null)
				return NotFound();
			return View(customer);

		}
		[HttpPost]
		public IActionResult DeleteCustomerConfirmed(int id)
		{
			_ics.DeleteCustomer(id);
			TempData["Message"] = $"Customer_{id} is Removed succesfully";

			return RedirectToAction("Index");
		}
		[HttpGet]
		public IActionResult DeleteMovie(int id)
		{
			if (id == null)
				return NotFound();
			var movie = _ims.GetMovie(id);
			if (movie == null)
				return NotFound();
			return View(movie);
		}
		[HttpPost]
		public IActionResult DeleteMovieConfirmed(int id)
		{
			_ims.DeleteMovie(id);
			TempData["Message"] = $"Movie_{id} is Removed succesfully";

			return RedirectToAction("Index");
		}


		public IActionResult DisplayOrdersAdmin()
		{
			List<AdminOrderViewModel> aovm = new List<AdminOrderViewModel>();
			var list = _ios.GetAllOrdersListCOR().OrderByDescending(o => o.OrderDate);
			foreach (var item in list)
			{
				AdminOrderViewModel avm = new AdminOrderViewModel();
				avm.OrderDate = item.OrderDate;
				avm.OrderId = item.Id;
				avm.OrderByCus = item.Customer.FristName + " " + item.Customer.LastName;
				avm.CustomerId = item.Customer.Id;
				avm.DeliveryAddress = $"{item.Customer.DeliveryAddress} \n {item.Customer.DeliveryZip}, {item.Customer.DeliveryCity}";
				foreach (var movl in item.OrderRows)
				{
					IndividualCartOrderViewModel idc = new IndividualCartOrderViewModel();
					if (!avm.InMvovList.Any(x => x.Movie.Id == movl.MovieId))
					{
						idc.Movie = _ims.GetMovie(movl.MovieId);
						idc.NumberofCopies = 1;

						avm.InMvovList.Add(idc);
					}
					else
					{
						avm.InMvovList.FirstOrDefault(x => x.Movie.Id == movl.MovieId).NumberofCopies++;
					}
				}
				aovm.Add(avm);

			}

			return View(aovm);
		}

		public IActionResult DeleteOrder(int id)
		{
			if (id == null)
				return NotFound();
			var order = _ios.GetOrder(id);
			if (order == null)
				return NotFound();

			AdminOrderViewModel avm = new AdminOrderViewModel();
			avm.OrderDate = order.OrderDate;
			avm.OrderId = order.Id;
			avm.CustomerId = order.CustomerId;
			foreach (var movl in order.OrderRows)
			{
				IndividualCartOrderViewModel idc = new IndividualCartOrderViewModel();
				if (!avm.InMvovList.Any(x => x.Movie.Id == movl.MovieId))
				{
					idc.Movie = _ims.GetMovie(movl.MovieId);
					idc.NumberofCopies = 1;

					avm.InMvovList.Add(idc);
				}
				else
				{
					avm.InMvovList.FirstOrDefault(x => x.Movie.Id == movl.MovieId).NumberofCopies++;
				}
			}

			return View(avm);

		}

		public IActionResult DeleteOrderConfirmed(int id)
		{
			_ios.DeleteOrder(id);
			TempData["Message"] = $"Order_{id} is Removed succesfully";

			return Json(new { success = true, redirectToUrl = Url.Action("DisplayOrdersAdmin", "Order") });
		}


	}
}
