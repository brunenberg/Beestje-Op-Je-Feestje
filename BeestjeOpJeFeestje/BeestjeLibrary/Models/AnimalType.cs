using System.ComponentModel.DataAnnotations;

namespace BeestjeLibrary.Models {
    public class AnimalType {
        [Key]
        public string Type { get; set; }
    }
}
