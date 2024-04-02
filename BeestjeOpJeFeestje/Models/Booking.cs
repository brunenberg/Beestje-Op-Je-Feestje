namespace Models {
    public class Booking {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public ICollection<BookingDetail> AnimalBookings { get; set; }
    }
}
