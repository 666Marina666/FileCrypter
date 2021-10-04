using System;

namespace WebApplication1.Models.RSA
{
    public class RsaKeyPair
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] PublicKey { get; set; }
        public byte[] PrivateKey { get; set; }
    }
}