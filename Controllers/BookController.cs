using AutoMapper;
using BackendBookstore.DTOs;
using BackendBookstore.DTOs.CreateDTO;
using BackendBookstore.DTOs.ReadDTO;
using BackendBookstore.DTOs.UpdateDTO;
using BackendBookstore.Models;
using BackendBookstore.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;

namespace BackendBookstore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class BookController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookRepo _repository;
        private readonly IWebHostEnvironment _env;

        public BookController(IMapper mapper, IBookRepo repository, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _repository = repository;
            _env = env;
        }

        [AllowAnonymous]
        [HttpGet("{page}")]
        public ActionResult<IEnumerable<BookReadDto>> GetAll(int? categoryId, int page, string? search)
        {
            var books = _repository.GetBooks(categoryId, search);
            if (books == null || !books.Any())
            {
                return NoContent();
            }

            var pageResults = 9f;
            var pageCount = Math.Ceiling(books.Count() / pageResults);
            if (page > pageCount)
            {
                return BadRequest($"Invalid page number. The available pages are 1 to {pageCount}");
            }
            books = books.Skip((page - 1) * (int)pageResults).Take((int)pageResults);
            var bookDtos = _mapper.Map<IEnumerable<BookReadDto>>(books);

            foreach (var bookDto in bookDtos)
            {
                var book = _repository.FindBookById(bookDto.BookId);

                var orderDto = _mapper.Map<IEnumerable<OrderItemUpdateDto>>(book.Orderitems);
                bookDto.Orderitems = orderDto.ToList();
            }
            var response = new BookResponseDto
            {
                Books = bookDtos,
                CurrentPage = page,
                Pages = (int)pageCount
            };
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("id/{bookId}", Name = "GetBookById")]
        public ActionResult<BookReadDto> GetBookById(int bookId)
        {
            Book book = _repository.FindBookById(bookId);
            if (book != null)
            {
                var order = _mapper.Map<IEnumerable<OrderItemUpdateDto>>(book.Orderitems);
                var bookDto = _mapper.Map<BookReadDto>(book);
                bookDto.Orderitems = order.ToList();
                return Ok(bookDto);
            }
            return NotFound();

        }

        [AllowAnonymous]
        [HttpGet("ids/{bookIds}", Name = "GetProductsByIds")]
        public ActionResult<IEnumerable<BookReadDto>> GetBooksByIds(string bookIds)
        {
            var ids = bookIds.Split(',').Select(int.Parse).ToList();
            var books = _repository.GetBooksByIds(ids);

            if (books.Any())
            {
                var bookDtos = _mapper.Map<IEnumerable<BookReadDto>>(books);
                return Ok(bookDtos);
            }

            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public async Task<ActionResult<BookReadDto>> UploadAndCreateBook([FromForm] BookCreateDto book, IFormFile file)
        {
            string? filePath = null;

            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file was uploaded.");
                }

                var uploadsPath = Path.Combine(_env?.WebRootPath ?? string.Empty, "uploads");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                filePath = Path.Combine(uploadsPath, fileName); // Assign actual file path to filePath variable

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var bookModel = _mapper.Map<Book>(book);
                //bookModel.Image = $"/uploads/{fileName}";

                _repository.Create(bookModel);
                _repository.SaveChanges();
                var bookDto = _mapper.Map<BookUpdateDto>(bookModel);
                return CreatedAtRoute(nameof(GetBookById), new { bookId = bookDto.BookId }, bookDto);
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the data to the database.");
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult<BookReadDto> Update(BookUpdateDto book)
        {
            try
            {
                var oldBook = _repository.FindBookById(book.BookId);
                if (oldBook == null)
                {
                    return NotFound();
                }
                Book bookEntity = _mapper.Map<Book>(book);
                _mapper.Map(bookEntity, oldBook);
                _repository.SaveChanges();
                return Ok(_mapper.Map<BookReadDto>(oldBook));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{bookId}")]
        public IActionResult Delete(int bookId)
        {
            try
            {
                var book = _repository.FindBookById(bookId);
                if (book == null)
                {
                    return NotFound();
                }
                _repository.Delete(bookId);
                _repository.SaveChanges();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }
    }
}
