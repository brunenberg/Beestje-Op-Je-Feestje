namespace Models {
    public class Animal {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public int AnimalTypeId { get; set; }
        public AnimalType AnimalType { get; set; }
        public ICollection<BookingDetail>? BookingDetails { get; set; }
    }
}
