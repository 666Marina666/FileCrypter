using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models.File;

namespace WebApplication1.Services
{
public interface IFileService
{
    public FileOnDatabaseModel Upload(FileOnDatabaseModel fileModel);

    public Task<IQueryable<FileOnDatabaseModel>> GetAsync(Expression<Func<FileOnDatabaseModel, bool>> expression = null,
        params Expression<Func<FileOnDatabaseModel, object>>[] includes);

    public FileOnDatabaseModel Update(FileOnDatabaseModel File);

    public FileOnDatabaseModel Delete(FileOnDatabaseModel File);
}
}
