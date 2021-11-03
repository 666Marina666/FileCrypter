using System;
using System.IO;
using RSACryptor;
using System.Linq;
using WebApplication1.Data;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using WebApplication1.Models.File;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace WebApplication1.Controllers
{
    public class FileController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;
        private readonly IKeyServices _keyServices;

        //public object DataProtectionScope { get; private set; }
        //public object ProtectedData { get; private set; }
        public FileController(AppDbContext context, IFileService fileService, IKeyServices keyServices)
        {
            _context = context;
            _fileService = fileService;
            _keyServices = keyServices;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            var fileuploadViewModel = await PreapreViewModel();
            ViewBag.Message = TempData["Message"];
            return View(fileuploadViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadToDatabase(List<IFormFile> files, string description, int pairId)
        {
            foreach (var file in files)
            {
                var fileModel = new FileOnDatabaseModel
                {
                    Description = description,
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    KeyPair = _keyServices.Get(pairId),
                    Extension = Path.GetExtension(file.FileName),
                    Name = Path.GetFileNameWithoutExtension(file.FileName)
                };

                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    fileModel.Data = dataStream.ToArray();
                }

                using (var provider = new RSACrypt())
                {
                    provider.ImportKey(fileModel.KeyPair.PublicKey.Length, fileModel.KeyPair.PublicKey, false);

                    fileModel.Data = provider.Encrypt(fileModel.Data);
                }

                _fileService.Upload(fileModel);
            }

            TempData["Message"] = "File successfully uploaded to Database";
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> DownloadFileFromDatabase(int id)
        {
            var fileModel = (await _fileService.GetAsync(file=>file.Id.Equals(id), file=>file.KeyPair)).First();
            if (fileModel == null) return null;

            var keyPair = _keyServices.Get(fileModel.KeyPair.Id);

            using (var provider = new RSACrypt())
            {
                provider.ImportKey(keyPair.PrivateKey.Length, keyPair.PublicKey, true);

                fileModel.Data = provider.Decrypt(fileModel.Data);
            }

            return File(fileModel.Data, fileModel.FileType, fileModel.Name + fileModel.Extension);
        }

        [Authorize]
        public async Task<IActionResult> DeleteFileFromDatabase(int id)
        {

            var file = await _context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.FilesOnDatabase.Remove(file);
            _context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
            return RedirectToAction("Index");
        }
        private async Task<UploadedFilesViewModel> PreapreViewModel()
        {
            //TODO:change upload files to upload only signatures
            var viewModel = new UploadedFilesViewModel();
            viewModel.FilesOnDatabase = await _context.FilesOnDatabase.ToListAsync();
            var userName = HttpContext.User.Identity.Name;
            var user = _context.Users.FirstOrDefault(usr => usr.UserName.Equals(userName));
            viewModel.KeyPairs = _keyServices.GetAll(user.Id);
            return viewModel;
        }
    }
}
