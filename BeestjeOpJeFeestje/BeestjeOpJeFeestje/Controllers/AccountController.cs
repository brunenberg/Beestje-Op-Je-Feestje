using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace BeestjeOpJeFeestje.Controllers {
    public class AccountController : Controller {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<Account> userManager, SignInManager<Account> signInManager, ApplicationDbContext context) {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (ModelState.IsValid) {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded) {
                    return RedirectToAction("Index", "Home");
                } else {
                    ModelState.AddModelError(string.Empty, "Ongeldige login.");
                    return View(model);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateUser() {
            var cardTypes = _context.CustomerCards.ToList();
            ViewBag.CardTypes = cardTypes;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model) {
            if (ModelState.IsValid) {
                // Create a new Address instance
                var address = new Address {
                    Street = model.Street,
                    HouseNumber = model.HouseNumber,
                    City = model.City,
                    PostalCode = model.PostalCode,
                };

                // Save the Address to the database
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                // Create a new Account instance
                var user = new Account {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    AddressId = address.Id, // Associate the new Address with the user
                    CustomerCardId = model.CardTypeId
                };

                // Give user the role of Customer

                var password = GeneratePassword();
                var result = await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, "Customer");

                if (result.Succeeded) {
                    // User created successfully
                    var userCreatedViewModel = new UserCreatedViewModel {
                        Email = user.Email,
                        Password = password
                    };

                    return View("UserCreated", userCreatedViewModel);
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            var cardTypes = _context.CustomerCards.ToList();
            ViewBag.CardTypes = cardTypes;
            return View(model);
        }

        private string GeneratePassword() {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%&*()_+=-<>?/.,;:[]{}";
            const string alphanumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            // Ensure at least one alphanumeric character
            stringChars[0] = alphanumericChars[random.Next(alphanumericChars.Length)];

            for (int i = 1; i < stringChars.Length; i++) {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList() {
            var users = await _userManager.Users
            .Include(u => u.Address)
            .ToListAsync();
            var userViewModels = users.Select(u => new UserViewModel {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name,
                Street = u.Address.Street,
                HouseNumber = u.Address.HouseNumber,
                City = u.Address.City,
                PostalCode = u.Address.PostalCode,
                CustomerCardType = _context.CustomerCards.FirstOrDefault(c => c.Id == u.CustomerCardId)?.CardType
            });

            return View(userViewModels);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditUser(string id) {
            var user = await _userManager.Users.Include(u => u.Address)
                                               .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) {
                return NotFound();
            }

            var viewModel = new EditUserViewModel {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Street = user.Address.Street,
                HouseNumber = user.Address.HouseNumber,
                City = user.Address.City,
                PostalCode = user.Address.PostalCode,
                CardTypeId = user.CustomerCardId
            };

            ViewBag.CardTypes = _context.CustomerCards.ToList();

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model) {
            if (ModelState.IsValid) {
                var user = await _userManager.Users.Include(u => u.Address)
                                                   .FirstOrDefaultAsync(u => u.Id == model.Id);

                if (user == null) {
                    return NotFound();
                }

                user.Email = model.Email;
                user.Name = model.Name;
                user.Address.Street = model.Street;
                user.Address.HouseNumber = model.HouseNumber;
                user.Address.City = model.City;
                user.Address.PostalCode = model.PostalCode;
                user.CustomerCardId = model.CardTypeId;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded) {
                    return RedirectToAction(nameof(UserList));
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewBag.CardTypes = _context.CustomerCards.ToList();
            return View(model);
        }
    }
}