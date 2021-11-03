using System;

namespace WebApplication1.Models.File
{
    public abstract class FileModel
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public RsaKeyPair KeyPair { get; set; }
        public ApplicationUser UploadedBy { get; set; }
    }
}
