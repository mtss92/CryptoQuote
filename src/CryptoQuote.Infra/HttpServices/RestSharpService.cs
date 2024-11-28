using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoQuote.Infra.HttpServices
{
    public class RestSharpService : IHttpService
    {
        public Dictionary<string, string> Headers { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task<T> GetAsync<T>(string path)
        {
            throw new NotImplementedException();
        }
    }
}
