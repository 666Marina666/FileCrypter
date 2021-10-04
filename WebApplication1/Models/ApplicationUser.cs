using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<RsaParameters> RsaParameters { get; set; }

        public ApplicationUser()
        {
            RsaParameters = new List<RsaParameters>();
        }
    }
}
