using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<RsaKeyPair> RsaParameters { get; set; }

        public ApplicationUser()
        {
            RsaParameters = new List<RsaKeyPair>();
        }
    }
}
