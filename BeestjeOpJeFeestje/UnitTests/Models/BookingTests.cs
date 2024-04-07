using BeestjeOpJeFeestje.Controllers;
using BeestjeOpJeFeestje.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.Models {
    [ExcludeFromCodeCoverage]
    public class BookingTests {
        [Fact]
        public void TestBookingModel() {
            // Arrange
            var booking = new Booking {
                Id = 1,
                DateTime = DateTime.Now,
                AccountId = "TestAccountId",
                Account = new Account(),
                GuestId = 1,
                Guest = new Guest(),
                AnimalBookings = new List<BookingDetail>(),
                DiscountApplied = 10
            };

            // Act
            var id = booking.Id;
            var dateTime = booking.DateTime;
            var accountId = booking.AccountId;
            var account = booking.Account;
            var guestId = booking.GuestId;
            var guest = booking.Guest;
            var animalBookings = booking.AnimalBookings;
            var discountApplied = booking.DiscountApplied;

            // Assert
            Assert.Equal(1, id);
            Assert.Equal(DateTime.Now.Date, dateTime.Date);
            Assert.Equal("TestAccountId", accountId);
            Assert.NotNull(account);
            Assert.Equal(1, guestId);
            Assert.NotNull(guest);
            Assert.NotNull(animalBookings);
            Assert.Equal(10, discountApplied);
        }
    }
}
