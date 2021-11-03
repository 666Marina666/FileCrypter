using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RSACryptor;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services.Implemetations
{
    public class KeyServices : IKeyServices
    {
        private readonly AppDbContext _context;
        private readonly DbSet<RsaKeyPair> _keyContext;
        private readonly DbSet<ApplicationUser> _userContext;

        public KeyServices(AppDbContext context)
        {
            _context = context;
            _userContext = context.Users;
            _keyContext = context.RsaKeyPairs;
        }
        public RsaKeyPair Create(string userName, string keyPairName)
        {
            using (var provider = new RSACrypt())
            {
                var rsaKeyPair = new RsaKeyPair
                {
                    PairName = keyPairName,
                    PublicKey = provider.ExportKey(false).GetBuffer(),
                    PrivateKey = provider.ExportKey(true).GetBuffer(),
                    Creator = _userContext.First(u => u.UserName == userName)
                };

                _keyContext.Add(rsaKeyPair);
                _context.SaveChanges();

                return rsaKeyPair;
            }
        }

        public RsaKeyPair Get(int id)
        {
            return _keyContext.FirstOrDefault(key => key.Id == id);
        }

        public List<RsaKeyPair> GetAll(string userId)
        {
            return _keyContext.Where(key => key.Creator.Id == userId).ToList();
        }

        public RsaKeyPair Update(RsaKeyPair keyPair)
        {
            return new RsaKeyPair();
        }

        public RsaKeyPair Delete(RsaKeyPair keyPair)
        {
            return new RsaKeyPair();
        }
    }
}
