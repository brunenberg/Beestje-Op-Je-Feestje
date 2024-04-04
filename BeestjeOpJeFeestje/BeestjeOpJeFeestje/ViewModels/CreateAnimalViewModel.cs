using System.ComponentModel.DataAnnotations;

public class CreateAnimalViewModel {
    public int Id { get; set; }
    [Display(Name = "Naam")]
    public string Name { get; set; }
    [Display(Name = "Prijs")]
    public double Price { get; set; }
    [Display(Name = "Pad naar afbeelding")]
    public string ImagePath { get; set; }
    [Display(Name = "Type beestje")]
    public int AnimalTypeId { get; set; }
}