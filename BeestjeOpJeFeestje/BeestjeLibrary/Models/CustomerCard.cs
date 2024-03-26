using System.ComponentModel.DataAnnotations;

namespace BeestjeLibrary.Models {
    public class CustomerCard {
        [Key]
        public string Type { get; set; }
    }
}
