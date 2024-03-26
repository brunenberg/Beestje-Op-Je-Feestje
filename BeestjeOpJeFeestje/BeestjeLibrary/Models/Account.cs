using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeestjeLibrary.Models {
    public class Account {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public int AddressId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; } = null!;
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string CustomerCardId { get; set; }

        [ForeignKey("CustomerCardId")]
        public virtual CustomerCard CustomerCard { get; set; }
    }
}