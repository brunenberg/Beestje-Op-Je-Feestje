using Models;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.Models {
    [ExcludeFromCodeCoverage]
    public class BookingDetailTests {
        [Fact]
        public void TestBookingDetailModel() {
            // Arrange
            var bookingDetail = new BookingDetail {
                Id = 1,
                BookingId = 1,
                Booking = new Booking(),
                AnimalId = 1,
                Animal = new Animal(),
                PriceAtBooking = 100.0
            };

            // Act
            var id = bookingDetail.Id;
            var bookingId = bookingDetail.BookingId;
            var booking = bookingDetail.Booking;
            var animalId = bookingDetail.AnimalId;
            var animal = bookingDetail.Animal;
            var priceAtBooking = bookingDetail.PriceAtBooking;

            // Assert
            Assert.Equal(1, id);
            Assert.Equal(1, bookingId);
            Assert.NotNull(booking);
            Assert.Equal(1, animalId);
            Assert.NotNull(animal);
            Assert.Equal(100.0, priceAtBooking);
        }
    }
}
