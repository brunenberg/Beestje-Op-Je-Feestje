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
            if(TempData.ContainsKey("ErrorMessage")) {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
                TempData.Remove("ErrorMessage"); // Remove the error message from TempData
            }

            if(string.IsNullOrEmpty(viewModel.SelectedDate)) 
            {
                if(HttpContext.Session.GetString("SelectedDate") == null) 
                {
                    return RedirectToAction("Index", "Home");
                }
                else 
                {
                    viewModel.SelectedDate = HttpContext.Session.GetString("SelectedDate");
                }
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
            if(User.Identity.IsAuthenticated) {
                customerCard = _context.Users
                .Include(u => u.CustomerCard)
                .FirstOrDefault(u => u.Email == User.Identity.Name)
                .CustomerCard;
            } else {
                customerCard = null;
            }

            if(customerCard == null) {
                customerCard = new CustomerCard { CardType = "None" };
            }

            var validationResult = ValidateAnimals(animals, customerCard, DateTime.Parse(HttpContext.Session.GetString("SelectedDate")));
            if(!validationResult.isValid) {
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

        public (bool isValid, string errorMessage) ValidateAnimals(List<Animal> selectedAnimals, CustomerCard customerCard, DateTime bookingDate) {
            // Regel: Je mag geen beestje boeken met het type ‘Leeuw’ of ‘IJsbeer’ als je ook een beestje boekt van het type ‘Boerderijdier’
            bool hasBoerderijdier = selectedAnimals.Any(a => a.AnimalType.TypeName == "Boerderijdier");
            if(hasBoerderijdier && (selectedAnimals.Any(a => a.AnimalType.TypeName == "Leeuw") || selectedAnimals.Any(a => a.AnimalType.TypeName == "IJsbeer"))) {
                return (false, "Je mag geen beestje boeken van het type 'Leeuw' of 'IJsbeer' als je ook een beestje boekt van het type 'Boerderijdier'.");
            }

            // Regel: Je mag geen beestje boeken met de naam ‘Pinguïn’ in het weekend
            if(selectedAnimals.Any(a => a.Name == "Pinguïn" && (bookingDate.DayOfWeek == DayOfWeek.Saturday || bookingDate.DayOfWeek == DayOfWeek.Sunday))) {
                return (false, "Je mag geen beestje boeken met de naam 'Pinguïn' in het weekend.");
            }

            // Regel: Je mag geen beestje boeken van het type ‘Woestijn’ in de maanden oktober t/m februari
            if(selectedAnimals.Any(a => a.AnimalType.TypeName == "Woestijn" && (bookingDate.Month >= 10 || bookingDate.Month <= 2))) {
                return (false, "Je mag geen beestje boeken van het type 'Woestijn' in de maanden oktober t/m februari.");
            }

            // Regel: Je mag geen beestje boeken van het type ‘Sneeuw’ in de maanden juni t/m augustus
            if(selectedAnimals.Any(a => a.AnimalType.TypeName == "Sneeuw" && (bookingDate.Month >= 6 && bookingDate.Month <= 8))) {
                return (false, "Je mag geen beestje boeken van het type 'Sneeuw' in de maanden juni t/m augustus.");
            }

            // Regel: Klanten zonder klantenkaart mogen maximaal 3 dieren boeken
            if(customerCard.CardType.Equals("None") && selectedAnimals.Count > 3) {
                return (false, "Klanten zonder klantenkaart mogen maximaal 3 dieren boeken.");
            }

            // Regel: Klanten met een zilveren klantenkaart mogen 1 dier extra boeken
            if(customerCard.CardType.Equals("Silver") && selectedAnimals.Count > 4) {
                return (false, "Klanten met een zilveren klantenkaart mogen maximaal 4 dieren boeken.");
            }

            // Regel: Klanten met een platina kaart mogen daarnaast ook nog de VIP dieren boeken
            if(!customerCard.CardType.Equals("Platinum")) {
                if(selectedAnimals.Any(a => a.AnimalType.TypeName == "VIP")) {
                    return (false, "Alleen klanten met een platina klantenkaart kunnen VIP dieren boeken.");
                }
            }

            // Regel: Klanten met een gouden kaart mogen zoveel dieren boeken als ze willen
            if(customerCard.CardType.Equals("Gold")) {
                return (true, null); // Return true zonder foutmelding
            }

            return (true, null); // Als alle validatieregels zijn doorstaan, return true zonder foutmelding
        }


    }
}
