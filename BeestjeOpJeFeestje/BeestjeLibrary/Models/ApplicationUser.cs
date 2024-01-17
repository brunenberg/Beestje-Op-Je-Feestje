using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeestjeLibrary.Models {
    public class ApplicationUser : IdentityUser {

        public int AccountId { get; set; }

        public Account? Account { get; set; }
    }
}
