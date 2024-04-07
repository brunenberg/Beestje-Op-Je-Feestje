using Models;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.Models {
    [ExcludeFromCodeCoverage]
    public class AccountTests {
        [Fact]
        public void TestAccountModel() {
            // Arrange
            var account = new Account {
                Name = "Test User",
                AddressId = 1,
                Address = new Address(),
                CustomerCardId = 1,
                CustomerCard = new CustomerCard(),
                Bookings = new List<Booking>()
            };

            // Act
            var name = account.Name;
            var addressId = account.AddressId;
            var address = account.Address;
            var customerCardId = account.CustomerCardId;
            var customerCard = account.CustomerCard;
            var bookings = account.Bookings;

            // Assert
            Assert.Equal("Test User", name);
            Assert.Equal(1, addressId);
            Assert.NotNull(address);
            Assert.Equal(1, customerCardId);
            Assert.NotNull(customerCard);
            Assert.NotNull(bookings);
        }
    }
}
