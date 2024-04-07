using Models;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.Models {
    [ExcludeFromCodeCoverage]
    public class AddressTests {
        [Fact]
        public void TestAddressModel() {
            // Arrange
            var address = new Address {
                Id = 1,
                Street = "Test Street",
                HouseNumber = "123",
                PostalCode = "45678",
                City = "Test City"
            };

            // Act
            var id = address.Id;
            var street = address.Street;
            var houseNumber = address.HouseNumber;
            var postalCode = address.PostalCode;
            var city = address.City;

            // Assert
            Assert.Equal(1, id);
            Assert.Equal("Test Street", street);
            Assert.Equal("123", houseNumber);
            Assert.Equal("45678", postalCode);
            Assert.Equal("Test City", city);
        }
    }
}
