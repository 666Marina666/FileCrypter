using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.File;

namespace WebApplication1.Services.Implemetations
{
    public class FileService : IFileService
    {
        private readonly DbSet<FileOnDatabaseModel> _fileContext;
        private readonly AppDbContext _context;
        public FileService(AppDbContext context)
        {
            _context = context;
            _fileContext = context.FilesOnDatabase;
        }

        public FileOnDatabaseModel Upload(FileOnDatabaseModel fileModel)
        {
            _fileContext.Add(fileModel);
            _context.SaveChanges();
            return new FileOnDatabaseModel();
        }

        public async Task<IQueryable<FileOnDatabaseModel>> GetAsync(Expression<Func<FileOnDatabaseModel, bool>> expression = null, params Expression<Func<FileOnDatabaseModel, object>>[] includes)
        {
            return includes.Aggregate(await Task.Run(() => expression != null ? _fileContext.Where(expression) : _fileContext),
                (current, includesproperty) => current.Include(includesproperty));
        }

        public FileOnDatabaseModel Update(FileOnDatabaseModel File)
        {
            return new FileOnDatabaseModel();
        }

        public FileOnDatabaseModel Delete(FileOnDatabaseModel File)
        {
            return new FileOnDatabaseModel();
        }
    }
}
