using BeestjeOpJeFeestje.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Data;


namespace BeestjeOpJeFeestje.Controllers {
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

            // Fill the ViewModel with the selected animals
            BookingViewModel viewModel = new BookingViewModel {
                SelectedDate = HttpContext.Session.GetString("SelectedDate"),
                SelectedAnimals = animals
            };

            return View(viewModel);
        }

        public IActionResult Step3() {
            var byteArray = HttpContext.Session.Get("SelectedAnimals");
            List<int> selectedAnimals = new List<int>();

            for(int i = 0; i < byteArray.Length; i += sizeof(int)) {
                selectedAnimals.Add(BitConverter.ToInt32(byteArray, i));
            }
            List<Animal> animals = _context.Animals.Where(a => selectedAnimals.Contains(a.Id)).ToList();

            

            return View();
        }
    }
}
