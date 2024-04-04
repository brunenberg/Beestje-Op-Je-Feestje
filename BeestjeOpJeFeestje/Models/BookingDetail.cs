namespace Models {
    public class BookingDetail {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        public int AnimalId { get; set; }
        public Animal Animal { get; set; }
        public double PriceAtBooking { get; set; }
    }

}
