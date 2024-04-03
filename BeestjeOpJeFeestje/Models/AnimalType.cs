namespace Models {
    public class AnimalType {
        public int Id { get; set; }
        public string TypeName { get; set; } // Jungle, Farm, Snow, Desert, VIP
        public ICollection<Animal> Animals { get; set; }
    }
}
