using System.Collections.Generic;

namespace WebApplication1.Models.File
{
    public class UploadedFilesViewModel
    {
        public int PairID { get; set; }
        public List<RsaKeyPair> KeyPairs { get; set; }
        public List<FileOnDatabaseModel> FilesOnDatabase { get; set; }
    }
}
