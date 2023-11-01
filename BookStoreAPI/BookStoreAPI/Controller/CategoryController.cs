using BookStoreAPI.Core.DTO;
using BookStoreAPI.Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service.IService;

namespace BookStoreAPI.Controller
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        ICategoryService _cate;
        public CategoryController(ICategoryService cate) 
        {
            _cate = cate;
        }    
        [HttpGet()]
        public async Task<IActionResult> GetCategory()
        {
                var respone= await _cate.GetAllCategory();
                if(respone != null)
                {
                    return Ok(respone);
                }
            return BadRequest("null");
        }
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var response = await _cate.GetCategoryById(categoryId);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest("Category don't exists!");
        }
        

    }
}
