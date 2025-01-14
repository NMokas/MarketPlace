using AutoMapper;
using MarketPlace.CategoryAPI.Data;
using MarketPlace.CategoryAPI.Models;
using MarketPlace.CategoryAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.CategoryAPI.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryAPIController : Controller
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly ApplicationDbContext _db;
        public CategoryAPIController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDto();
            _mapper = mapper;
        }
        [Authorize]
        [HttpPost("CreateCategory")]
        public async Task<ResponseDto> CreateProduct([FromBody] CategoryDto categoryDto)
        {
            var auxCategory = _mapper.Map<Category>(categoryDto);
            await _db.AddAsync(auxCategory);
            _db.SaveChanges();
            _response.Result = auxCategory;
            return _response;
        }
        [Authorize]
        [HttpGet]
        public ResponseDto GetAllProducts()
        {
            try
            {
                IEnumerable<Category> auxCategories= _db.Categories.ToList();

                _response.Result = _mapper.Map<IEnumerable<CategoryDto>>(auxCategories);
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
        [Route("{id:int}")]
        public async Task<ResponseDto> GetCategoryById(int id)
        {
            try
            {
                var auxProduct = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);



                _response.Result = _mapper.Map<CategoryDto>(auxProduct);
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
        public async Task<ResponseDto> UpdateCategory([FromBody]CategoryDto categoryDto)
        {
            try
            {
                var auxProduct = await _db.Categories.FirstOrDefaultAsync(x => x.Id == categoryDto.Id);
                auxProduct = _mapper.Map<Category>(categoryDto);
                auxProduct.UpdateDate = DateTime.Now; 

                _db.Categories.Update(auxProduct);
                await _db.SaveChangesAsync();
                _response.Result = _mapper.Map<CategoryDto>(auxProduct);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }
        [HttpDelete]
        [Authorize]
        [Route("id:int")]

        public async Task<ResponseDto> DeleteCategory(int id)
        {

            var coupon = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (coupon != null)
            {
                _db.Categories.Remove(coupon);
                _db.SaveChanges();
                _response.Result = "Category Deleted with success";
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "This category does not exists";
            }


            return _response;
        }
    }
}
