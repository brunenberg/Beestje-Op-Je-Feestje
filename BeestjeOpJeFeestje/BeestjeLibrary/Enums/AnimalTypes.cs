using System.ComponentModel.DataAnnotations;

namespace BeestjeLibrary.Enums {
    public class AnimalTypes {
        [Key]
        public string Type { get; set; } = null!;
    }
}
