using BeestjeLibrary.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeestjeLibrary.Models {
    public class Account {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public int AdressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; } = null!;
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("CustomerCard")]
        public string? CustomerCard { get; set; }


    }
}
