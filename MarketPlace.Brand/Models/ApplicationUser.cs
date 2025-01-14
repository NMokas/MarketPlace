using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.AuthAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public bool IsBrand { get; set; }= false;
        public bool IsApproved { get; set; }=false;
    }
}
