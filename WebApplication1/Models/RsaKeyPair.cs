using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class RsaKeyPair
    {
        [Key]
        public int Id { get; set; }
        public string PairName { get; set; }
        public byte[] PublicKey { get; set; }
        public byte[] PrivateKey { get; set; }
        public ApplicationUser Creator { get; set; }
    }
}
