using System.ComponentModel.DataAnnotations;

namespace BeestjeLibrary.Enums {
    public class CustomerCard
    {
        [Key]
        public string CardType { get; set; }
    }
}
