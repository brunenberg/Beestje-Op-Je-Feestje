using BeestjeLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Animal {
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Naam is verplicht.")]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Prijs is verplicht.")]
    public int Price { get; set; }

    [ForeignKey("AnimalType")]
    [Required(ErrorMessage = "Type is verplicht.")]
    public string Type { get; set; }

    public virtual AnimalType AnimalType { get; set; }
}
