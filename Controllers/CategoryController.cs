using AutoMapper;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _repository;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<CategoryReadDto>> GetAll()
        {
            var categories = _repository.GetCategories();
            var catDtos = _mapper.Map<IEnumerable<CategoryReadDto>>(categories);

            foreach (var catDto in catDtos)
            {
                catDto.Books = _mapper.Map<IEnumerable<BookUpdateDto>>(_repository.GetBooksForCategory(catDto.CategoryId)).ToList();
            }
            return Ok(catDtos);
        }
        [AllowAnonymous]
        [HttpGet("{categoryId}", Name = "GetCategoryById")]
        public ActionResult<CategoryReadDto> GetCategoryById(int categoryId)
        {
            Category cat = _repository.FindCategoryById(categoryId);
            if (cat != null)
            {
                var catDto = _mapper.Map<CategoryReadDto>(cat);
                catDto.Books = _mapper.Map<IEnumerable<BookUpdateDto>>(_repository.GetBooksForCategory(catDto.CategoryId)).ToList();
                return Ok(catDto);
            }
            return NotFound();

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<CategoryReadDto> CreateCategory(CategoryCreateDto cat)
        {
            var catModel = _mapper.Map<Category>(cat);
            try
            {
                _repository.Create(catModel);
                _repository.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the data to the database.");
            }
            var catDto = _mapper.Map<CategoryUpdateDto>(catModel);
            return CreatedAtRoute(nameof(GetCategoryById), new { categoryId = catDto.CategoryId }, catDto);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut()]
        public ActionResult<CategoryReadDto> Update(CategoryUpdateDto cat)
        {
            try
            {
                var oldCategory = _repository.FindCategoryById(cat.CategoryId);
                if (oldCategory == null)
                {
                    return NotFound();
                }
                Category catEntity = _mapper.Map<Category>(cat);
                _mapper.Map(catEntity, oldCategory);
                _repository.SaveChanges();
                return Ok(_mapper.Map<CategoryReadDto>(oldCategory));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{categoryId}")]
        public IActionResult Delete(int categoryId)
        {
            try
            {
                var cat = _repository.FindCategoryById(categoryId);
                if (cat == null)
                {
                    return NotFound();
                }
                _repository.Delete(categoryId);
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
