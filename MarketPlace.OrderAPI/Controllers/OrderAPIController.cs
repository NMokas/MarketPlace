using AutoMapper;
using MarketPlace.OrderAPI.Data;
using MarketPlace.OrderAPI.Extensions;
using MarketPlace.OrderAPI.Models;
using MarketPlace.OrderAPI.Models.Dto;
using MarketPlace.OrderAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private IProductService _productService;

        public OrderAPIController(ApplicationDbContext db,IMapper mapper,IProductService productService)
        {
            this._response=new ResponseDto();
            _mapper = mapper;
            _productService = productService;
            _db = db;
        }

        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody]CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);

                OrderHeader orderCreated = _mapper.Map<OrderHeader>(orderHeaderDto);
                _db.OrderHeaders.Add(orderCreated);
                await _db.SaveChangesAsync();

                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;
                _response.Result = orderHeaderDto;
            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message= ex.Message;
            }
            return _response;
        }

    }
}
