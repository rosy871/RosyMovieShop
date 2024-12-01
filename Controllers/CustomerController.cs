using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using Newtonsoft.Json;
using RosyMovieShop.Helper;
using RosyMovieShop.Models.Db;
using RosyMovieShop.Models.ViewModels;
using RosyMovieShop.Services;

namespace RosyMovieShop.Controllers
{
	public class CustomerController : Controller
	{
		private readonly ICustomerService _ics;
		private readonly IMovieServices _ims;
		private readonly IOrderServices _ios;

        List<Movie> movielist = new List<Movie>();
       // List<IndividualCartOrderViewModel> cvm = new List<IndividualCartOrderViewModel>();
       // List<int> countCartItem = new List<int>();

        public CustomerController(ICustomerService ics, IMovieServices ims, IOrderServices ios)
        {
			_ics = ics;
			_ims = ims;
			_ios = ios;
			UserRole.URole = Role.Customer;
            
        }



        public IActionResult Index()
		{
			var movies = _ims.GetAllMoviesWithOrder().Where(m => m.Active = true);
            var custIndexVM = new CustomerIndexViewModel()
            {
                MostPopularMovie = movies.OrderByDescending(m => m.OrderRows.Count)
                                    .Take(5).ToList(),
                Top5NewestMovies = movies.OrderByDescending(m => m.ReleaseYear)
                                    .Take(5).ToList(),
                Top5CheapestMovies = movies.OrderBy(m => m.Price).Take(5).ToList(),
                Top5OldestMovies = movies.OrderBy(m => m.ReleaseYear).Take(5).ToList(),
                CustomerSpentMostMoney = _ics.TopCustomer()

            };
			return View(custIndexVM);
		}


		public IActionResult ListAllActiveMovies()
		{
			var movies = _ims.GetAllMovies().Where(m=>m.Active=true);
			return View(movies);
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


		//customer 

		[HttpGet]
		public IActionResult CreateCustomer()
		{
			TempData.Clear();
			return View();

		}
		[HttpPost]

		public IActionResult CreateCustomer(Customer customer)
		{
			if (ModelState.IsValid)
			{
				_ics.CreateCustomer(customer);
				string cusName = customer.FristName + " " + customer.LastName;
				int cusId = customer.Id;
				HttpContext.Session.SetString("SessionCustomerName", cusName);
				HttpContext.Session.SetInt32("SessionCusId", cusId);
				TempData["Message"] = $"{cusName} is registered succesfully";

			}

			if (TempData["formCart"] == null)
				return RedirectToAction("Index");
			else
				return RedirectToAction("CartDetails");
		}


		public IActionResult LoginCustomer()
		{
			//TempData["fromCart"] = null;
			TempData.Clear();
			return View();

		}
		[HttpPost]
		public IActionResult LoginCustomer(string email)
		{
			if (ModelState.IsValid)
			{
				var customer = _ics.GetCustomerFromEmail(email);
				if (customer != null)
				{
					string cusName = customer.FristName + " " + customer.LastName;
					int cusId = customer.Id;
					HttpContext.Session.SetString("SessionCustomerName", cusName);
					HttpContext.Session.SetInt32("SessionCusId", cusId);
					//HttpContext.Session.SetString("SessionCustomerEmail", email);

				}


			}

			if (TempData["formCart"] == null)
				return RedirectToAction("Index");
			else
				return RedirectToAction("CartDetails");
		}

		public IActionResult LogoutCustomer()
		{


			HttpContext.Session.Remove("SessionCustomerName");
			HttpContext.Session.Remove("SessionCusId");
			TempData.Clear();
			return RedirectToAction("Index", "Customer");


		}

		[HttpPost]
		public JsonResult ValidateEmail(string email)
		{
			bool emailExists = _ics.CustomerExist(email);
			if (!emailExists)
				return Json(new { success = false, message = "Email is not registered." });

			return Json(new { success = true });
		}

		[HttpPost]
		public JsonResult ValidateRegisterEmail(string email)
		{
			//if (!ModelState.IsValid) { return Json(new { success = false, message = "Invalid email format." }); }


			bool emailExists = _ics.CustomerExist(email);
			if (emailExists)
				return Json(new { success = false, message = "Email already exists." });

			return Json(new { success = true });
		}




        //Custome Cart
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            if (id == 0) return NotFound();

            var mov = _ims.GetMovie(id);
            if (HttpContext.Session.GetString("SessionMovieList") == null)
            {
                movielist.Add(mov);
                HttpContext.Session.SetString("SessionMovieList", JsonConvert.SerializeObject(movielist));
            }
            else
            {
                List<Movie> movielist1 = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
                movielist1.Add(mov);

                HttpContext.Session.SetString("SessionMovieList", JsonConvert.SerializeObject(movielist1));
            }

            var cartList = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
            var numberOfListItems = cartList.Count();

            HttpContext.Session.SetInt32("cartcountS", numberOfListItems);
            return Json(numberOfListItems);

        }

        [HttpPost]
        public IActionResult IncreaseItemInCart(int id)
        {
            if (id == null) return NotFound();

            var mov = _ims.GetMovie(id);

            List<Movie>? movielist1 = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
            movielist1.Add(mov);

            HttpContext.Session.SetString("SessionMovieList", JsonConvert.SerializeObject(movielist1));

            var cartList = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
            var numberOfListItems = cartList.Count();
            var price = cartList.FirstOrDefault(x => x.Id == id).Price;
            var movnum = cartList.FindAll(x => x.Id == id).Count();
            var tot = cartList.Sum(x => x.Price);

            HttpContext.Session.SetInt32("cartcountS", numberOfListItems);

            var data = new { pris = price, numMov = movnum, total = tot, totMovNum = numberOfListItems };

            return Json(data);

        }


        [HttpPost]
        public IActionResult DecreaseItemInCart(int id)
        {

            if (id==0) return NotFound();
            if (HttpContext.Session.GetString("SessionMovieList") == null) { return BadRequest("cart item cannot decrease than 0"); }
            
			List<Movie>? movielist1 = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));

            var mov = _ims.GetMovie(id);
            double price = 0; int numberOfListItems = 0; int movnum = 0; double tot = 0;

            foreach (var item in movielist1)
            {
                if (item.Id == mov.Id) { movielist1.Remove(item); break; }

            }
            HttpContext.Session.SetString("SessionMovieList", JsonConvert.SerializeObject(movielist1));

			List<Movie>? cartList = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
            if (cartList.Count > 0)
            {
                numberOfListItems = cartList.Count();
                movnum = cartList.FindAll(x => x.Id == id).Count();
                if (movnum > 0)
                {
                    price = cartList.FirstOrDefault(x => x.Id == id).Price;
                }
                tot = cartList.Sum(x => x.Price);
            }
            else
            {
                price = tot = 0;
                movnum = numberOfListItems = 0;
            }
            HttpContext.Session.SetInt32("cartcountS", numberOfListItems);

            var data = new { pris = price, numMov = movnum, total = tot, totMovNum = numberOfListItems };

            return Json(data);

        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            if (id == null) return NotFound();

            int numberOfListItems = 0; double tot = 0;
            List<Movie>? movielist1 = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
            if (movielist1.FindAll(x => x.Id == id).Count > 0)
            {

                movielist1.RemoveAll(m => m.Id == id);

            }
            HttpContext.Session.SetString("SessionMovieList", JsonConvert.SerializeObject(movielist1));
            var cartList = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));

            if (cartList.Count > 0)
            {

                numberOfListItems = cartList.Count();

                tot = cartList.Sum(x => x.Price);
            }
            else
            {
                tot = 0;
                numberOfListItems = 0;
            }
            HttpContext.Session.SetInt32("cartcountS", numberOfListItems);

            var data = new { total = tot, totMovNum = numberOfListItems };


            return Json(data);
        }

        public IActionResult CartDetails()
        {
            CartViewModel cvm1 = new CartViewModel();
            if (HttpContext.Session.GetString("SessionMovieList") == null)
            {
                return View();
            }
            else
            {
                var movielist1 = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
                foreach (var item in movielist1)
                {
                    IndividualCartOrderViewModel im = new IndividualCartOrderViewModel();
                    if (!cvm1.ListMovie.Any(x => x.Movie.Id == item.Id))
                    {
                        im.Movie = item;
                        im.NumberofCopies = 1;

                        cvm1.ListMovie.Add(im);
                    }
                    else
                    {
                        cvm1.ListMovie.FirstOrDefault(x => x.Movie.Id == item.Id).NumberofCopies++;

                    }
                }
               HttpContext.Session.SetString("SessionCartList", JsonConvert.SerializeObject(cvm1));
                return View(cvm1);
            }
        }

        public IActionResult DisplayLoginPartiaView()

        {
            return PartialView("_CustomerLoginPartial");
        }

        public IActionResult DisplayRegistraionPartiaView()
        {
            return PartialView("_CustomerRegisterPartial");
        }

        public IActionResult CheckoutfromCart()
        {

            var movielist = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
            if (movielist.Count > 0)
            {
                int cusid = (int)HttpContext.Session.GetInt32("SessionCusId");

                _ios.AddOrder(movielist, cusid);

            }
            return Json(new { success = true, redirectToUrl = Url.Action("OrderConfirmaion") });
        }


        public IActionResult OrderConfirmaion()
        {

            CartViewModel cvm1 = new CartViewModel();

            if (HttpContext.Session.GetString("SessionMovieList") == null)
            {
                return View();
            }
            else
            {
                var movielist1 = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
                foreach (var item in movielist1)
                {
                    IndividualCartOrderViewModel im = new IndividualCartOrderViewModel();
                    if (!cvm1.ListMovie.Any(x => x.Movie.Id == item.Id))
                    {
                        im.Movie = item;
                        im.NumberofCopies = 1;

                        cvm1.ListMovie.Add(im);
                    }
                    else
                    {
                        cvm1.ListMovie.FirstOrDefault(x => x.Movie.Id == item.Id).NumberofCopies++;

                    }
                }
            }
            HttpContext.Session.Remove("SessionMovieList");
            HttpContext.Session.Remove("cartcountS");
            return View(cvm1);
        }


        [HttpGet]
        public IActionResult Orders()
        {
            int cusid = (int)HttpContext.Session.GetInt32("SessionCusId");
            if (cusid == 0) return BadRequest("Customer id not found");

            var customer=_ics.GetCustomer(cusid);

            if (customer == null) return BadRequest("Customer not found");

            var orders=_ios.GetOrdersByCustomerId(cusid);

            if (orders == null) TempData["Message"] = "No previous Order is found.";

            
          
                var cvm = orders.Select(o => new CustomerOrderViewModel
                {
                    OrderDate = o.OrderDate,
                    OrderId = o.Id,
                    TotalCost = (double)o.OrderRows.Sum(or => or.Price),
                    Movies = o.OrderRows.GroupBy(or => or.Movie)
                   .Select(g => new IndividualCartOrderViewModel
                   {
                       Movie = g.Key,
                       NumberofCopies = g.Count()
                   }).ToList()
                }).ToList();                
            

            return View(cvm);
           
        }



            //[HttpGet]
            //public IActionResult Orders(string email)
            //{



            //	if (string.IsNullOrEmpty(email))
            //	{
            //		return BadRequest("Email is required.");

            //	}

            //	var customer = _ics.GetCustomerFromEmail(email);
            //	if (customer == null)
            //		return BadRequest("No Customer found.");

            //	var orders = _ios.GetOrdersByCustomerEmail(email)?.ToList() ?? new List<OrderViewModel>();

            //	var viewModel = new CustomerOrdersViewModel
            //	{
            //		CustomerName = $"{customer.FristName} {customer.LastName}",
            //		TotalOrders = orders.Count(),
            //		Orders = orders,
            //	};

            //	if (!orders.Any())
            //		ViewBag.Message = "No orders found for this customer.";

            //	return View(viewModel);
            //}

        }
}
