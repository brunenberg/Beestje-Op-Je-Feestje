using Models;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.Models {
    [ExcludeFromCodeCoverage]
    public class GuestTests {
        [Fact]
        public void TestGuestModel() {
            // Arrange
            var guest = new Guest {
                Id = 1,
                Name = "Test Guest",
                Email = "testguest@example.com",
                AddressId = 1,
                Address = new Address(),
                Bookings = new List<Booking>()
            };

            // Act
            var id = guest.Id;
            var name = guest.Name;
            var email = guest.Email;
            var addressId = guest.AddressId;
            var address = guest.Address;
            var bookings = guest.Bookings;

            // Assert
            Assert.Equal(1, id);
            Assert.Equal("Test Guest", name);
            Assert.Equal("testguest@example.com", email);
            Assert.Equal(1, addressId);
            Assert.NotNull(address);
            Assert.NotNull(bookings);
        }
    }
}
