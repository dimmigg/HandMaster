using Microsoft.AspNetCore.Identity;

namespace HandMaster_Models
{
    public class ApplicationUser :IdentityUser
    {
        public string FullName { get; set; }
    }
}
