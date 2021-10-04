namespace WebApplication1.Models.File
{
    public class FileOnDatabaseModel : FileModel
    {
        public byte[] Data { get; set; }
        public string PublicKey { get; set; }
    }
}
