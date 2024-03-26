using System.ComponentModel.DataAnnotations;

namespace BeestjeLibrary.Models {
    public class Role {
        [Key]
        public string Name { get; set; }
    }
}
