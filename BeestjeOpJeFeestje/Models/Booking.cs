namespace Models {
    public class Booking {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string? AccountId { get; set; }
        public Account? Account { get; set; }
        public int? GuestId { get; set; }
        public Guest? Guest { get; set; }
        public ICollection<BookingDetail> AnimalBookings { get; set; }
        public int DiscountApplied { get; set; }

    }
}
