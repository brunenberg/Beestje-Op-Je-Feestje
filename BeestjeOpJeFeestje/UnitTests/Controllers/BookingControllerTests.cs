using BeestjeOpJeFeestje.Controllers;
using BeestjeOpJeFeestje.Models;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Controllers {
    public class BookingControllerTests {
        [Fact]
        public void Index_WhenCalled_ShouldReturnViewWithCorrectViewModel() {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") // Use in-memory database
                .Options;

            var context = new ApplicationDbContext(options);

            var account = new Account { Id = "TestAccountId", Email = "TestAccountId", Name = "TestAccountName" };
            context.Users.Add(account);

            var bookings = new List<Booking>
            {
                new Booking { Id = 1, AccountId = "TestAccountId", AnimalBookings = new List<BookingDetail>() },
                new Booking { Id = 2, AccountId = "TestAccountId", AnimalBookings = new List<BookingDetail>() }
            };

            foreach (var booking in bookings) {
                context.Bookings.Add(booking);
            }

            context.SaveChanges();

            var controller = new BookingController(context);

            // Mock User.Identity.Name
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "TestAccountId"),
            }));

            controller.ControllerContext = new ControllerContext() {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<BookingViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void Start_WithFutureDate_ShouldStoreDateInSessionAndRedirectToStep1() {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();
            var controller = new BookingController(null) {
                ControllerContext = new ControllerContext {
                    HttpContext = httpContext
                }
            };
            var futureDate = DateTime.Now.AddDays(1);

            // Act
            var result = controller.Start(futureDate);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Step1", redirectResult.ActionName);
            Assert.Equal(futureDate.ToString("dd-MM-yyyy"), httpContext.Session.GetString("SelectedDate"));
        }







    }
}
