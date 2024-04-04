using System.ComponentModel.DataAnnotations;

namespace Models {
    public class Animal {
        public int Id { get; set; }
        [Display(Name = "Naam")]
        public string Name { get; set; }
        [Display(Name = "Prijs")]
        public double Price { get; set; }
        [Display(Name = "Pad naar afbeelding")]
        public string ImagePath { get; set; }
        [Display(Name = "Type beestje")]
        public int AnimalTypeId { get; set; }
        [Display(Name = "Type beestje")]
        public AnimalType AnimalType { get; set; }
        public ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
