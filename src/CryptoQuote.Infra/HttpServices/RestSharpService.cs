using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoQuote.Infra.HttpServices
{
    public class RestSharpService : IHttpService
    {
        public Task<T> GetAsync<T>(string path)
        {
            throw new NotImplementedException();
        }
    }
}
