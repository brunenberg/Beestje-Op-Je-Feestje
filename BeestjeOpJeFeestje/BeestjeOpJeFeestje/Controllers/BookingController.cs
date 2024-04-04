using BeestjeOpJeFeestje.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;


namespace BeestjeOpJeFeestje.Controllers
{
    public class BookingController : Controller {

        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context) {
            _context = context;
        }
        public IActionResult Start(DateTime? selectedDate) {
            if(selectedDate.HasValue) {
                string selectedDateString = selectedDate.Value.ToString("yyyy-MM-dd");

                HttpContext.Session.SetString("SelectedDate", selectedDateString);
            }
            else {
                HttpContext.Session.Remove("SelectedDate");
            }

            BookingViewModel viewModel = new BookingViewModel {
                SelectedDate = HttpContext.Session.GetString("SelectedDate")
            };

            return RedirectToAction("Step1", viewModel);
        }

        public IActionResult Step1(BookingViewModel viewModel) {
            if(string.IsNullOrEmpty(viewModel.SelectedDate)) {
                return RedirectToAction("Index", "Home");
            }

            viewModel.Animals = _context.Animals.ToList();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Step2(List<int> selectedAnimals) {
            var byteArray = selectedAnimals.SelectMany(BitConverter.GetBytes).ToArray();

            HttpContext.Session.Set("SelectedAnimals", byteArray);

            return RedirectToAction("Step2");
        }

        [HttpGet]
        public IActionResult Step2() {
            var byteArray = HttpContext.Session.Get("SelectedAnimals");
            if(byteArray == null) {
                return RedirectToAction("Step1");
            }

            List<int> selectedAnimals = new List<int>();

            for(int i = 0; i < byteArray.Length; i += sizeof(int)) {
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

        public IActionResult Step3() {
            var byteArray = HttpContext.Session.Get("SelectedAnimals");
            if(byteArray == null) {
                return RedirectToAction("Step1");
            }
            List<int> selectedAnimals = new List<int>();
            for(int i = 0; i < byteArray.Length; i += sizeof(int)) {
                selectedAnimals.Add(BitConverter.ToInt32(byteArray, i));
            }
            List<Animal> animals = _context.Animals.Where(a => selectedAnimals.Contains(a.Id)).ToList();

            BookingViewModel viewModel = new BookingViewModel {
                SelectedDate = HttpContext.Session.GetString("SelectedDate"),
                SelectedAnimals = animals,
            };

            FillViewModel(viewModel);

            CalculateTotalPrice(viewModel);

            return View(viewModel);
        }

        private BookingViewModel FillViewModel(BookingViewModel viewModel) {
            if(User.Identity.IsAuthenticated) {
                ApplicationUser user = _context.Users
                .Include(u => u.Address)
                .FirstOrDefault(u => u.Email == User.Identity.Name);

                viewModel.Name = user.Name;
                viewModel.Email = user.Email;
                viewModel.Street = user.Address.Street;
                viewModel.HouseNumber = user.Address.HouseNumber;
                viewModel.PostalCode = user.Address.PostalCode;
                viewModel.City = user.Address.City;
            }

            return viewModel;
        }

        private void CalculateTotalPrice(BookingViewModel viewModel) {
            viewModel.TotalPrice = 0;
            viewModel.AppliedDiscounts = new List<string>();
            double basePrice = 0;
            double totalPrice = 0;
            bool hasEend = false;
            bool hasA = false, hasB = false, hasC = false;
            int animalCount = viewModel.SelectedAnimals.Count;

            foreach(Animal animal in viewModel.SelectedAnimals) {
                basePrice += animal.Price;
                totalPrice += animal.Name == "Eend" && new Random().Next(1, 7) == 1 ? animal.Price * 0.5 : animal.Price;
                hasEend |= animal.Name == "Eend" && new Random().Next(1, 7) == 1;
                hasA |= animal.Name.Contains("A");
                hasB |= animal.Name.Contains("B");
                hasC |= animal.Name.Contains("C");
            }

            double discountPercentage = 0;

            if(viewModel.SelectedAnimals.GroupBy(a => a.Name).Any(g => g.Count() >= 3)) {
                discountPercentage += 0.1;
                viewModel.AppliedDiscounts.Add("3 Types: 10%");
            }

            DateTime selectedDate = DateTime.Parse(viewModel.SelectedDate);
            if(selectedDate.DayOfWeek == DayOfWeek.Monday || selectedDate.DayOfWeek == DayOfWeek.Tuesday) {
                discountPercentage += 0.15;
                viewModel.AppliedDiscounts.Add("Maandag of Dinsdag: 15%");
            }

            discountPercentage += (hasA ? 0.02 : 0) + (hasB ? 0.02 : 0) + (hasC ? 0.02 : 0);
            viewModel.AppliedDiscounts.AddRange(new[] { hasA ? "Letter A: 2%" : null, hasB ? "Letter B: 2%" : null, hasC ? "Letter C: 2%" : null }.Where(s => s != null));

            if(HasCustomerCard()) {
                discountPercentage += 0.1;
                viewModel.AppliedDiscounts.Add("Klantenkaart: 10%");
            }

            discountPercentage = Math.Min(discountPercentage, 0.6);

            double discountedPrice = totalPrice * (1 - discountPercentage);
            viewModel.TotalPrice = discountedPrice;
        }


        public bool HasCustomerCard() {
            ApplicationUser user = _context.Users
            .Include(u => u.CustomerCard)
            .FirstOrDefault(u => u.Email == User.Identity.Name);

            return user?.CustomerCard != null;
        }
    }
}
