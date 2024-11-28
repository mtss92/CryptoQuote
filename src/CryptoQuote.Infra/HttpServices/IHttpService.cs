using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoQuote.Infra.HttpServices
{
    public interface IHttpService
    {
        Dictionary<string, string> Headers { get; set; }
        Task<T> GetAsync<T>(string path);
    }
}
