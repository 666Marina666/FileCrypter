﻿using System;
using System.IO;
using RSACryptor;
using System.Linq;
using WebApplication1.Data;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using WebApplication1.Models.RSA;
using WebApplication1.Models.File;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace WebApplication1.Controllers
{
    public class FileController : Controller
    {
        private readonly AppDbContext context;

        //public object DataProtectionScope { get; private set; }
        //public object ProtectedData { get; private set; }


        public IEnumerable<string> companies = new List<string>
        {
            "{1451D3BE-B302-4023-9A15-8CB18EFFA121}",
            "{A8B0A97C-7EC9-4146-97DA-2B801813FBBA}",
            "{D29C53AD-679D-452F-87B9-0DCF16657940}",
            "{835DBC2C-671D-4DF1-B5CD-FB19E67E9260}"
        };


        public FileController(AppDbContext context)
        {
            this.context = context;
        }

        [Authorize]

        public async Task<IActionResult> Index()
        {

            var fileuploadViewModel = await LoadAllFiles();
            ViewBag.Message = TempData["Message"];
            ViewBag.Compaties = companies;
            return View(fileuploadViewModel);
        }

        [Authorize]
        public async Task<IActionResult> RSAParametrs()
        {
            var fileuploadViewModel = new RsaViewModel
            {
                Name = "123",
                CreatedOn = DateTime.Now
            };
            ViewBag.Message = TempData["Message"];
            return View(fileuploadViewModel);
            //using (var rsa = new RSACryptoServiceProvider(512))
            //{
            //    var publicKey = rsa.ExportCspBlob(false);
            //    var privateKey = rsa.ExportCspBlob(true);
            //    File.WriteAllBytes(@"D:\public.txt", publicKey);
            //    File.WriteAllBytes(@"D:\private.txt", privateKey);
            //}
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description)
        {
            foreach (var file in files)
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
                bool basePathExists = Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var fileModel = new FileOnFileSystemModel
                    {
                        CreatedOn = DateTime.UtcNow,
                        FileType = file.ContentType,
                        Extension = extension,
                        Name = fileName,
                        Description = description,
                        FilePath = filePath
                    };
                    context.FilesOnFileSystem.Add(fileModel);
                    context.SaveChanges();
                }
            }

            TempData["Message"] = "File successfully uploaded to File System.";
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadToDatabase(List<IFormFile> files, string description, string rsaKey)
        {
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var fileModel = new FileOnDatabaseModel
                {
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    RsaPublicKey = rsaKey,
                    Description = description
                };

                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    fileModel.Data = dataStream.ToArray();
                }

                using (var provider = new RSACrypt())
                {
                    //TODO:read key from base
                //    RsaKeyPair rsaKeyPair = new RsaKeyPair
                //    {
                //        Id = new Guid(),
                //        PublicKey = provider.ExportKey(false).GetBuffer(),
                //        PrivateKey = provider.ExportKey(true).GetBuffer()
                //};

                //    var rsaKeyInstance = rsaKeyPair.PublicKey;
                    //provider.ImportKey(rsaKeyInstance.Length, rsaKeyInstance, true);

                    fileModel.Data = provider.Encrypt(fileModel.Data);

                    using (var key = provider.ExportKey(false))
                    {
                        var keyBuffer = key.GetBuffer();
                    }
                }

                context.FilesOnDatabase.Add(fileModel);
                context.SaveChanges();
            }

            TempData["Message"] = "File successfully uploaded to Database";
            return RedirectToAction("Index");
        }

        private async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel();
            viewModel.FilesOnDatabase = await context.FilesOnDatabase.ToListAsync();
            viewModel.FilesOnFileSystem = await context.FilesOnFileSystem.ToListAsync();
            return viewModel;
        }

        public async Task<IActionResult> DownloadFileFromDatabase(int id)
        {

            var file = await context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;

            var crypter = new CryptoHelper();
            crypter.GetPrivateKey_Method();
            crypter.WritePublicKey();
            crypter.CreateAsmKeys();

            file.Data = crypter.DecryptBytes(file.Data);

            return File(file.Data, file.FileType, file.Name + file.Extension);
        }

        public async Task<IActionResult> DownloadFileFromFileSystem(int id)
        {

            var file = await context.FilesOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            var memory = new MemoryStream();
            using (var stream = new FileStream(file.FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, file.FileType, file.Name + file.Extension);
        }

        public async Task<IActionResult> DeleteFileFromFileSystem(int id)
        {

            var file = await context.FilesOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            if (System.IO.File.Exists(file.FilePath))
            {
                System.IO.File.Delete(file.FilePath);
            }

            context.FilesOnFileSystem.Remove(file);
            context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from File System.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteFileFromDatabase(int id)
        {

            var file = await context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            context.FilesOnDatabase.Remove(file);
            context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
            return RedirectToAction("Index");
        }
    }
}
