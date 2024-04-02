namespace Models {
    public class AnimalType {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public ICollection<Animal> Animals { get; set; }
    }
}
