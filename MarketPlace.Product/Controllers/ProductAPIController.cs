using AutoMapper;
using MarketPlace.ProductAPI.Data;
using MarketPlace.ProductAPI.Models;
using MarketPlace.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController :  Controller
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly ApplicationDbContext _db;
        public ProductAPIController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _response=new ResponseDto();
            _mapper = mapper;
        }
        [Authorize]
        [HttpPost("CreateProduct")]
        public async Task<ResponseDto> CreateProduct([FromBody]ProductDto product)
        {
            var auxProduct=_mapper.Map<Product>(product);
            await _db.AddAsync(auxProduct);
            _db.SaveChanges();

            return _response;
        }
        [Authorize]
        [HttpGet]
        public ResponseDto GetAllProducts()
        {
            try
            {
                IEnumerable<Product> auxProducts = _db.Products.ToList();

                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(auxProducts);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }
        [Authorize]
        [HttpGet]
        [Route("{pageNumber:int}")]
        public ResponseDto GetAllProductsByPage(int pageNumber)
        {
            try
            {
                int pageSize = 5;
                IEnumerable<Product> auxProducts = _db.Products.OrderBy(x => x.Name).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();

                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(auxProducts);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }
        [Authorize]
        [HttpGet]
        [Route("category/{categoryId:int}")]
        public ResponseDto GetProductsByCategory (int categoryId)
        {
            try
            {
                IEnumerable<Product> auxProducts = _db.Products.Where(x=>x.CategoryId==categoryId);

                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(auxProducts);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }
        [Authorize]
        [HttpGet]
        [Route("product/{id:int}")]
        public async Task<ResponseDto> GetProductById(int id)
        {
            try
            {
                var auxProduct = await _db.Products.FirstOrDefaultAsync(x=>x.Id==id);

               

                _response.Result = _mapper.Map<ProductDto>(auxProduct);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }
        [Authorize]
        [HttpPut("UpdateById")]
        public async Task<ResponseDto> UpdateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var auxProduct = await _db.Products.FirstOrDefaultAsync(x => x.Id == productDto.Id);
                auxProduct = _mapper.Map<Product>(productDto);
                auxProduct.UpdatedTime = DateTime.Now;

                _db.Products.Update(auxProduct);
                await _db.SaveChangesAsync();
                _response.Result = _mapper.Map<ProductDto>(auxProduct);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }
        [Authorize]
        [HttpDelete]
        [Route("id:int")]
        public async Task<ResponseDto> DeleteProduct(int id)
        {

            var coupon = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (coupon != null)
            {
                _db.Products.Remove(coupon);
                _db.SaveChanges();
                _response.Result = "Product Deleted with success";
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "This product does not exists";
            }


            return _response;
        }
    }
}
