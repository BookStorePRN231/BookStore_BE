using AutoMapper;
using BookStoreAPI.Core.DTO;
using BookStoreAPI.Core.Interface;
using BookStoreAPI.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service.IService;

namespace BookStoreAPI.Controller
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        IBookService _book;
        IMapper _mapper;
        public BookController(IBookService book, IMapper mapper) 
        {
            _book = book;
            _mapper = mapper;
        }
        /// <summary>
        /// Search by book name or get all books
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBook(string? nameBook)
        {
            if (string.IsNullOrEmpty(nameBook))
            {
                var response = await _book.GetAllBook();
                if (response != null) return Ok(response);
            }
            else
            {
                var response = await _book.GetBookByName(nameBook);
                if (response != null) return Ok(response);
            }
            return BadRequest("Book don't exist in the system");
        }
        /// <summary>
        /// Get book by categoryId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetAllBookByCategory(int categoryId)
        {
            var respone = await _book.GetBookByCategory(categoryId);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest("null");
        }
        /// <summary>
        /// Get book by bookId
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookDetail(Guid bookId)
        {
            var respone = await _book.GetBookById(bookId);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest("Book don't exists");
        }
        /// <summary>
        /// Add book
        /// </summary>
        /// <param name="dTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateBook(BookDTO dTO)
        {
            if (dTO != null)
            {
                var book = _mapper.Map<Book>(dTO);
                var result = await _book.CreateBook(book, dTO.Image_URL, dTO.Request_Id);
                if (result) return Ok("Create Book Success");
            }
            return BadRequest("Create Book Fail");
        }
        /// <summary>
        /// Update book
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateBook(BookDetailDTO bookDTO)
        {
            if (bookDTO != null)
            {
                var book = _mapper.Map<Book>(bookDTO);
                var result = await _book.UpdateBook(book);
                if (result) return Ok("Update Book Success");
            }
            return BadRequest("Update Book Fail");
        }
        /// <summary>
        /// Delete or restore book by bookId
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="option">1.Delete, 2.Restore</param>
        /// <returns></returns>
        [HttpPatch("{bookId}")]
        public async Task<IActionResult> DeleteBook(Guid bookId,Enum.EnumClass.CommonStatusOption option)
        {
            bool result = false;
            switch ((int)option)
            {
                case 1:
                    result = await _book.DeleteBook(bookId);
                    if (result) return Ok("Delete Book Success");
                    break;
                case 2:
                    result = await _book.RestoreBook(bookId);
                    if (result) return Ok("Restore Book Success");
                    break;
            }
            return BadRequest("Delete/Restore Book Fail");
        }
       
        [HttpDelete]
        public async Task<IActionResult> RemoveBook(Guid bookId)
        {
            var result = await _book.RemoveBook(bookId);
            if (result) return Ok("remove Book Success");
            return BadRequest("remove Book Fail");
        }
    }
}
