using BusinessLogic.Rules.PricingRules;
using BusinessLogic;
using Models;

namespace UnitTests {
    public class TypeGroupDiscountRuleTests {
        private TypeGroupDiscountRule _rule;

        public TypeGroupDiscountRuleTests() {
            _rule = new TypeGroupDiscountRule();
        }

        [Fact]
        public void GetDiscount_NoTypeGroup_ReturnsZero() {
            // Arrange
            var context = new DiscountContext {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Aap", Price = 20, AnimalTypeId = 1 },
                    new Animal { Name = "Olifant", Price = 50, AnimalTypeId = 2 },
                    new Animal { Name = "Zebra", Price = 30, AnimalTypeId = 3 }
                }
            };

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(0, result.discountPercentage);
            Assert.Null(result.discountMessage);
        }

        [Fact]
        public void GetDiscount_TypeGroupExists_ReturnsDiscount() {
            // Arrange
            var context = new DiscountContext {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Aap", Price = 20, AnimalTypeId = 1 },
                    new Animal { Name = "Olifant", Price = 50, AnimalTypeId = 1 },
                    new Animal { Name = "Zebra", Price = 30, AnimalTypeId = 1 }
                }
            };

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(10, result.discountPercentage);
            Assert.Single(result.discountMessage);
            Assert.Contains("3 beestjes van hetzelfde type: 10%", result.discountMessage);
        }

        [Fact]
        public void GetDiscount_MultipleTypeGroupsExists_ReturnsSingleDiscount() {
            // Arrange
            var context = new DiscountContext {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Aap", Price = 20, AnimalTypeId = 1 },
                    new Animal { Name = "Olifant", Price = 50, AnimalTypeId = 1 },
                    new Animal { Name = "Zebra", Price = 30, AnimalTypeId = 1 },
                    new Animal { Name = "Hond", Price = 20, AnimalTypeId = 2 },
                    new Animal { Name = "Ezel", Price = 30, AnimalTypeId = 2 },
                    new Animal { Name = "Koe", Price = 30, AnimalTypeId = 2 }
        }
            };

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(10, result.discountPercentage);
            Assert.Single(result.discountMessage);
            Assert.Contains("3 beestjes van hetzelfde type: 10%", result.discountMessage);
        }
    }
}