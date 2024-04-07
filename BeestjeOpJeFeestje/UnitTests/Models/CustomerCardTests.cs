using Models;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.Models {
    [ExcludeFromCodeCoverage]
    public class CustomerCardTests {
        [Fact]
        public void TestCustomerCardModel() {
            // Arrange
            var customerCard = new CustomerCard {
                Id = 1,
                CardType = "Gold"
            };

            // Act
            var id = customerCard.Id;
            var cardType = customerCard.CardType;

            // Assert
            Assert.Equal(1, id);
            Assert.Equal("Gold", cardType);
        }
    }
}
