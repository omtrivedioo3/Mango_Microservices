using Mango.Web.Models;
using Mongo.Web.Models;

namespace Mongo.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}
