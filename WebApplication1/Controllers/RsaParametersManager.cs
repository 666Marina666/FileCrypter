using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Text;

namespace WebApplication1.Controllers
{
    public class RsaParametersController : Controller
    {

        public RsaParameters GetNewRsaParameters()
        {
            return new RsaParameters()
            {
                PrivateKey = CreateRsaPrivateKey(),
                PublicKey = CreateRsaPublicKey()
            };
        }
       
        //public class MyCrypto
        //{
        //    RSACryptoServiceProvider rsa = null;
        //    string publicPrivateKeyXML;
        //    string publicOnlyKeyXML;
        //    public void AssignNewKey()
        //    {
        //        const int PROVIDER_RSA_FULL = 1;
        //        const string CONTAINER_NAME = "KeyContainer";
        //        CspParameters cspParams;
        //        cspParams = new CspParameters(PROVIDER_RSA_FULL);
        //        cspParams.KeyContainerName = CONTAINER_NAME;
        //        cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
        //        cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";
        //        rsa = new RSACryptoServiceProvider(cspParams);

        //        //Pair of public and private key as XML string.
        //        //Do not share this to other party
        //        publicPrivateKeyXML = rsa.ToXmlString(true);

        //        //Private key in xml file, this string should be share to other parties
        //        publicOnlyKeyXML = rsa.ToXmlString(false);

        //    }
        //}

        private string CreateRsaPublicKey()
        {
            //RSACryptoServiceProvider rsa;
            //RSA rsa = RSA.Create();
            
            return "newRsaPublicKey not implemented";
        }
        private string CreateRsaPrivateKey()
        {
            return "newRsaPublicKey not implemented";
        }

   
    }
}
   