using MarketPlace.Models;

namespace MarketPlace.Services.IServices
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}
