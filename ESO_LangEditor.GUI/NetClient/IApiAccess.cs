using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.NetClient
{
    public interface IApiAccess
    {
        Task<HttpResponseMessage> Get(Guid userId);
        Task<HttpResponseMessage> Get(string url, string token);
        Task<HttpResponseMessage> Get(string url, string token, byte[] dto);
        Task<HttpResponseMessage> GetToken(byte[] loginUserDto);
        Task<HttpResponseMessage> GetToken(Guid userId, byte[] tokenDto);
        Task<HttpResponseMessage> Post(string url, byte[] data);
        Task<HttpResponseMessage> Post(string url, string token, byte[] data);
    }
}
