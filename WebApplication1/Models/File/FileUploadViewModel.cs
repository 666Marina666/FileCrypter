using System.Collections.Generic;

namespace WebApplication1.Models.File
{
    public class FileUploadViewModel
    {
        public List<FileOnDatabaseModel> FilesToDecrypt { get; set; }
        public List<FileOnDatabaseModel> FilesOnDatabase { get; set; }
        public List<FileOnFileSystemModel> FilesToEnrcrypt { get; set; }
        public List<FileOnFileSystemModel> FilesOnFileSystem { get; set; }
    }
}
