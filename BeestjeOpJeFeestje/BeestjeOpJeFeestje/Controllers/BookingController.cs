using BeestjeOpJeFeestje.ViewModels;
using BusinessLogic;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;


namespace BeestjeOpJeFeestje.Controllers {
    public class BookingController : Controller {

        private readonly ApplicationDbContext _context;
        private readonly ISelectionRules _bookingRules;
        private readonly IPricingRules _priceRules;

        public BookingController(ApplicationDbContext context, ISelectionRules bookingRules, IPricingRules pricingRules) {
            _context = context;
            _bookingRules = bookingRules;
            _priceRules = pricingRules;
        }

        [Authorize(Roles = "Customer")]
        public IActionResult Index() {
            List<Booking> bookings = _context.Bookings
                .Include(b => b.AnimalBookings)
                .ThenInclude(bd => bd.Animal)
                .Where(b => b.AccountId == _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name).Id)
                .ToList();

            List<BookingViewModel> viewModels = new List<BookingViewModel>();

            foreach(Booking booking in bookings) {
                var TotalPrice = booking.AnimalBookings.Sum(bd => bd.PriceAtBooking);
                TotalPrice = TotalPrice * (1 - booking.DiscountApplied / 100.0);

                BookingViewModel viewModel = new BookingViewModel {
                    Booking = booking,
                    SelectedDate = booking.DateTime.ToString("dd-MM-yyyy"),
                    TotalPrice = TotalPrice,
                };

                viewModels.Add(viewModel);
            }

            return View(viewModels);
        }


        public IActionResult Start(DateTime? selectedDate) {
            if (selectedDate.HasValue) {
                if (selectedDate.Value < DateTime.Today) {
                    TempData["ErrorMessage"] = "Je kunt geen datum in het verleden selecteren.";
                    return RedirectToAction("Index", "Home");
                }

                string selectedDateString = selectedDate.Value.ToString("dd-MM-yyyy");
                HttpContext.Session.SetString("SelectedDate", selectedDateString);
            } else {
                HttpContext.Session.Remove("SelectedDate");
            }

            return RedirectToAction("Step1");
        }

        public IActionResult Step1() {
            BookingViewModel viewModel = new BookingViewModel();

            if (TempData.ContainsKey("ErrorMessage")) {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
                TempData.Remove("ErrorMessage"); // Verwijder het foutbericht uit TempData
            }

            if (HttpContext.Session.GetString("SelectedDate") == null) {
                return RedirectToAction("Index", "Home");
            } else {
                viewModel.SelectedDate = HttpContext.Session.GetString("SelectedDate");
            }

            viewModel.Animals = _context.Animals.ToList();

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Step2(List<int> selectedAnimals) {
            List<Animal> animals = _context.Animals
                .Where(a => selectedAnimals.Contains(a.Id))
                .Include(a => a.AnimalType)
                .ToList();

            CustomerCard customerCard;
            if (User.Identity.IsAuthenticated) {
                customerCard = _context.Users
                .Include(u => u.CustomerCard)
                .FirstOrDefault(u => u.Email == User.Identity.Name)
                .CustomerCard;
            } else {
                customerCard = null;
            }

            var validationResult = _bookingRules.ValidateAnimals(animals, customerCard, DateTime.Parse(HttpContext.Session.GetString("SelectedDate")));

            if (!validationResult.isValid) {
                TempData["ErrorMessage"] = validationResult.errorMessage;
                return RedirectToAction("Step1");
            }


            var byteArray = selectedAnimals.SelectMany(BitConverter.GetBytes).ToArray();

            HttpContext.Session.Set("SelectedAnimals", byteArray);

            return RedirectToAction("Step2");
        }

        [HttpGet]
        public IActionResult Step2() {
            var byteArray = HttpContext.Session.Get("SelectedAnimals");
            if (byteArray == null) {
                return RedirectToAction("Step1");
            }

            List<int> selectedAnimals = new List<int>();

            for (int i = 0; i < byteArray.Length; i += sizeof(int)) {
                selectedAnimals.Add(BitConverter.ToInt32(byteArray, i));
            }
            List<Animal> animals = _context.Animals.Where(a => selectedAnimals.Contains(a.Id)).ToList();

            BookingViewModel viewModel = new BookingViewModel {
                SelectedDate = HttpContext.Session.GetString("SelectedDate"),
                SelectedAnimals = animals,
            };

            FillViewModel(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Step3() {
            if (!User.Identity.IsAuthenticated) {
                HttpContext.Session.SetString("Name", Request.Form["Name"]);
                HttpContext.Session.SetString("Email", Request.Form["Email"]);
                HttpContext.Session.SetString("Street", Request.Form["Street"]);
                HttpContext.Session.SetString("HouseNumber", Request.Form["HouseNumber"]);
                HttpContext.Session.SetString("PostalCode", Request.Form["PostalCode"]);
                HttpContext.Session.SetString("City", Request.Form["City"]);
            }

            var byteArray = HttpContext.Session.Get("SelectedAnimals");
            if (byteArray == null) {
                return RedirectToAction("Step1");
            }
            List<int> selectedAnimals = new List<int>();
            for (int i = 0; i < byteArray.Length; i += sizeof(int)) {
                selectedAnimals.Add(BitConverter.ToInt32(byteArray, i));
            }
            List<Animal> animals = _context.Animals.Where(a => selectedAnimals.Contains(a.Id)).ToList();

            BookingViewModel viewModel = new BookingViewModel {
                SelectedDate = HttpContext.Session.GetString("SelectedDate"),
                SelectedAnimals = animals,
            };

            FillViewModel(viewModel);

            CustomerCard customerCard;
            if (User.Identity.IsAuthenticated) {
                customerCard = _context.Users
                .Include(u => u.CustomerCard)
                .FirstOrDefault(u => u.Email == User.Identity.Name)
                .CustomerCard;
            } else {
                customerCard = null;
            }

            double totalPrice = _priceRules.CalculateAnimalsPrice(animals);
            (double, List<string>) discountInfo = _priceRules.CalculateDiscount(animals, customerCard, DateTime.Parse(HttpContext.Session.GetString("SelectedDate")));

            viewModel.TotalPrice = Math.Round(totalPrice * (1 - discountInfo.Item1 / 100), 2);
            viewModel.AppliedDiscounts = discountInfo.Item2;
            HttpContext.Session.Set("DiscountPercentage", BitConverter.GetBytes(discountInfo.Item1));

            return View(viewModel);
        }

        private BookingViewModel FillViewModel(BookingViewModel viewModel) {
            if (User.Identity.IsAuthenticated) {
                Account user = _context.Users
                .Include(u => u.Address)
                .FirstOrDefault(u => u.Email == User.Identity.Name);

                viewModel.Name = user.Name;
                viewModel.Email = user.Email;
                viewModel.Street = user.Address.Street;
                viewModel.HouseNumber = user.Address.HouseNumber;
                viewModel.PostalCode = user.Address.PostalCode;
                viewModel.City = user.Address.City;
            } else {
                viewModel.Name = HttpContext.Session.GetString("Name");
                viewModel.Email = HttpContext.Session.GetString("Email");
                viewModel.Street = HttpContext.Session.GetString("Street");
                viewModel.HouseNumber = HttpContext.Session.GetString("HouseNumber");
                viewModel.PostalCode = HttpContext.Session.GetString("PostalCode");
                viewModel.City = HttpContext.Session.GetString("City");
            }

            return viewModel;
        }


        public bool HasCustomerCard() {
            Account user = _context.Users
            .Include(u => u.CustomerCard)
            .FirstOrDefault(u => u.Email == User.Identity.Name);

            return user?.CustomerCard != null;
        }

        public IActionResult Confirm() {
            if (!User.Identity.IsAuthenticated) {
                Guest guest = new Guest {
                    Name = HttpContext.Session.GetString("Name"),
                    Email = HttpContext.Session.GetString("Email"),
                    Address = new Address {
                        Street = HttpContext.Session.GetString("Street"),
                        HouseNumber = HttpContext.Session.GetString("HouseNumber"),
                        PostalCode = HttpContext.Session.GetString("PostalCode"),
                        City = HttpContext.Session.GetString("City")
                    }
                };

                _context.Guests.Add(guest);
                _context.SaveChanges();
            }

            var byteArray = HttpContext.Session.Get("SelectedAnimals");
            if (byteArray == null) {
                return RedirectToAction("Step1");
            }
            List<int> selectedAnimals = new List<int>();
            for (int i = 0; i < byteArray.Length; i += sizeof(int)) {
                selectedAnimals.Add(BitConverter.ToInt32(byteArray, i));
            }
            List<Animal> animals = _context.Animals.Where(a => selectedAnimals.Contains(a.Id)).ToList();
            string selectedDate = HttpContext.Session.GetString("SelectedDate");

            Booking booking = new Booking {
                DateTime = DateTime.Parse(selectedDate),
                DiscountApplied = (int)BitConverter.ToDouble(HttpContext.Session.Get("DiscountPercentage"), 0),
            };

            if (User.Identity.IsAuthenticated) {
                booking.AccountId = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name).Id;
            } else {
                booking.GuestId = _context.Guests.FirstOrDefault(g => g.Email == HttpContext.Session.GetString("Email")).Id;
            }

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            foreach (Animal animal in animals) {
                BookingDetail bookingDetail = new BookingDetail {
                    AnimalId = animal.Id,
                    BookingId = booking.Id,
                    PriceAtBooking = animal.Price
                };
                _context.BookingDetails.Add(bookingDetail);
            }
            _context.SaveChanges();

            HttpContext.Session.Clear();

            return RedirectToAction("Success", booking);
        }

        public IActionResult Success(Booking booking) {
            booking = _context.Bookings
                .Include(b => b.AnimalBookings)
                .ThenInclude(bd => bd.Animal)
                .FirstOrDefault(b => b.Id == booking.Id);

            var TotalPrice = booking.AnimalBookings.Sum(bd => bd.PriceAtBooking);
            TotalPrice = TotalPrice * (1 - booking.DiscountApplied / 100.0);

            BookingViewModel viewModel = new BookingViewModel {
                Booking = booking,
                SelectedDate = booking.DateTime.ToString("dd-MM-yyyy"),
                TotalPrice = TotalPrice,
            };

            return View(viewModel);
        }

        public IActionResult Cancel(int id) {
            Booking booking = _context.Bookings
                .Include(b => b.AnimalBookings)
                .FirstOrDefault(b => b.Id == id);

            if(booking == null) {
                return NotFound();
            }

            if(booking.AccountId != _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name).Id) {
                return Unauthorized();
            }

            _context.Bookings.Remove(booking);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



    }
}
