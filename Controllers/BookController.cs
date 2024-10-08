﻿using AutoMapper;
using BackendBookstore.DTOs;
using BackendBookstore.DTOs.CreateDTO;
using BackendBookstore.DTOs.ReadDTO;
using BackendBookstore.DTOs.UpdateDTO;
using BackendBookstore.Models;
using BackendBookstore.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICategoryRepo _categoryRepo;
        private readonly IWebHostEnvironment _env;

        public BookController(IMapper mapper, IBookRepo repository, ICategoryRepo categoryRepo, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _repository = repository;
            _categoryRepo = categoryRepo;
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
                bookDto.Orderitems = _mapper.Map<IEnumerable<OrderItemUpdateDto>>(_repository.GetOrderItemsForBook(bookDto.BookId)).ToList();
                bookDto.Reviews = _mapper.Map<IEnumerable<ReviewUpdateDto>>(_repository.GetReviewsForBook(bookDto.BookId)).ToList();
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
                // Mapiranje Orderitems i Reviews
                var orderItems = _mapper.Map<IEnumerable<OrderItemUpdateDto>>(book.Orderitems);
                var reviews = _mapper.Map<IEnumerable<ReviewUpdateDto>>(book.Reviews);

                var bookDto = _mapper.Map<BookReadDto>(book);
                bookDto.Orderitems = orderItems.ToList();
                bookDto.Reviews = reviews.ToList();

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

        [AllowAnonymous]
        [HttpGet("search", Name = "SearchBooks")]
        public ActionResult<IEnumerable<BookReadDto>> SearchBooks(int? categoryId, string query)
        {
            Console.WriteLine($"Searching for books with query '{query}' in category '{categoryId}'");

            var books = _repository.GetBooks(categoryId, query);

            if (!books.Any())
            {
                return NotFound("No books found matching the criteria.");
            }

            var bookReadDtos = _mapper.Map<IEnumerable<BookReadDto>>(books);

            Console.WriteLine($"Found {bookReadDtos.Count()} books matching the search criteria");

            return Ok(bookReadDtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public async Task<ActionResult<BookReadDto>> CreateBook(BookCreateDto book)
        {
            var bookModel = _mapper.Map<Book>(book);
            try
            {
                _repository.Create(bookModel);
                _repository.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the data to the database: {ex.Message}");
            }
            var bookDto = _mapper.Map<BookUpdateDto>(bookModel);
            return CreatedAtRoute(nameof(GetBookById), new { bookId = bookDto.BookId }, bookDto);
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
