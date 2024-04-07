using BusinessLogic;
using BusinessLogic.Rules.SelectionRules;
using Models;

namespace UnitTests {
    public class CustomerCardValidationRuleTests {
        private CustomerCardValidationRule _rule;

        public CustomerCardValidationRuleTests() {
            _rule = new CustomerCardValidationRule();
        }

        [Fact]
        public void Validate_NoCustomerCardAndMoreThanThreeAnimals_ReturnsFalse() {
            // Arrange
            var context = new ValidationContext {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Aap", Price = 20, ImagePath = "~/images/aap.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } },
                    new Animal { Name = "Olifant", Price = 50, ImagePath = "~/images/olifant.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } },
                    new Animal { Name = "Zebra", Price = 30, ImagePath = "~/images/zebra.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } },
                    new Animal { Name = "Leeuw", Price = 40, ImagePath = "~/images/leeuw.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } }
                },
                CustomerCard = null,
                BookingDate = DateTime.Now
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Klanten zonder klantenkaart mogen maximaal 3 dieren boeken.", result.errorMessage);
        }

        [Fact]
        public void Validate_SilverCustomerCardAndMoreThanFourAnimals_ReturnsFalse() {
            // Arrange
            var context = new ValidationContext {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Aap", Price = 20, ImagePath = "~/images/aap.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } },
                    new Animal { Name = "Olifant", Price = 50, ImagePath = "~/images/olifant.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } },
                    new Animal { Name = "Zebra", Price = 30, ImagePath = "~/images/zebra.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } },
                    new Animal { Name = "Leeuw", Price = 40, ImagePath = "~/images/leeuw.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } },
                    new Animal { Name = "Tijger", Price = 45, ImagePath = "~/images/tijger.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } }
                },
                CustomerCard = new CustomerCard { CardType = "Silver" },
                BookingDate = DateTime.Now
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Klanten met een zilveren klantenkaart mogen maximaal 4 dieren boeken.", result.errorMessage);
        }

        [Fact]
        public void Validate_GoldCustomerCardAndVIPAnimal_ReturnsFalse() {
            // Arrange
            var context = new ValidationContext {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Panda", Price = 100, ImagePath = "~/images/panda.jpg", AnimalType = new AnimalType { TypeName = "VIP" } }
                },
                CustomerCard = new CustomerCard { CardType = "Gold" },
                BookingDate = DateTime.Now
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Alleen klanten met een platina klantenkaart kunnen VIP dieren boeken.", result.errorMessage);
        }

        [Fact]
        public void Validate_PlatinumCustomerCardAndVIPAnimal_ReturnsTrue() {
            // Arrange
            var context = new ValidationContext {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Panda", Price = 100, ImagePath = "~/images/panda.jpg", AnimalType = new AnimalType { TypeName = "VIP" } }
                },
                CustomerCard = new CustomerCard { CardType = "Platinum" },
                BookingDate = DateTime.Now
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.True(result.isValid);
            Assert.Null(result.errorMessage);
        }

        [Fact]
        public void Validate_NoCustomerCardAndThreeAnimals_ReturnsTrue() {
            // Arrange
            var context = new ValidationContext {
                SelectedAnimals = new List<Animal>
                {
                new Animal { Name = "Aap", Price = 20, ImagePath = "~/images/aap.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } },
                new Animal { Name = "Olifant", Price = 50, ImagePath = "~/images/olifant.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } },
                new Animal { Name = "Zebra", Price = 30, ImagePath = "~/images/zebra.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } }
            },
                CustomerCard = null,
                BookingDate = DateTime.Now
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.True(result.isValid);
            Assert.Null(result.errorMessage);
        }

        [Fact]
        public void Validate_SilverCustomerCardAndFourAnimals_ReturnsTrue() {
            // Arrange
            var context = new ValidationContext {
                SelectedAnimals = new List<Animal>
                {
                new Animal { Name = "Aap", Price = 20, ImagePath = "~/images/aap.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } },
                new Animal { Name = "Olifant", Price = 50, ImagePath = "~/images/olifant.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } },
                new Animal { Name = "Zebra", Price = 30, ImagePath = "~/images/zebra.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } },
                new Animal { Name = "Leeuw", Price = 40, ImagePath = "~/images/leeuw.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } }
            },
                CustomerCard = new CustomerCard { CardType = "Silver" },
                BookingDate = DateTime.Now
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.True(result.isValid);
            Assert.Null(result.errorMessage);
        }

        [Fact]
        public void Validate_GoldCustomerCardAndNonVIPAnimal_ReturnsTrue() {
            // Arrange
            var context = new ValidationContext {
                SelectedAnimals = new List<Animal>
                {
                new Animal { Name = "Aap", Price = 20, ImagePath = "~/images/aap.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } }
            },
                CustomerCard = new CustomerCard { CardType = "Gold" },
                BookingDate = DateTime.Now
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.True(result.isValid);
            Assert.Null(result.errorMessage);
        }
    }
}
