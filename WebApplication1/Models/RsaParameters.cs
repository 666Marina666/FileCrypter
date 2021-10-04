using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class RsaParameters
    {
        [Key]
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string PairName { get; set; }
        public ICollection<ApplicationUser> OwnerUsers { get; set; }
    }
}
