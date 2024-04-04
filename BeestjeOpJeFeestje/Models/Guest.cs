namespace Models {
    public class Guest {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}