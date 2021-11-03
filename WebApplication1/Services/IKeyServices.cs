using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IKeyServices
    {
        public RsaKeyPair Create(string userName, string keyPairName);

        public RsaKeyPair Get(int id);

        public List<RsaKeyPair> GetAll(string userId);

        public RsaKeyPair Update(RsaKeyPair keyPair);

        public RsaKeyPair Delete(RsaKeyPair keyPair);
    }
}
