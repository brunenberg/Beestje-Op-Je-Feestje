using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeestjeLibrary.Models {
    public class Booking {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; } = null!;
        public List<Animal> Animals { get; set; } = null!;
    }
}
