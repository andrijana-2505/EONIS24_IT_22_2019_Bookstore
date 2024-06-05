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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepo _repository;
        private readonly IMapper _mapper;
        public ReviewController(IReviewRepo repo, IMapper mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<ReviewReadDto>> GetAll(int? usersId)
        { 

            var reviewDtos = _mapper.Map<IEnumerable<ReviewReadDto>>(_repository.GetReviews(usersId));
            foreach (var reviewDto in reviewDtos)
            {
                reviewDto.Users = _mapper.Map<IEnumerable<UserUpdateDto>>
                     (_repository.GetUserForReview(reviewDto.ReviewId)).ToList();
                reviewDto.Books = _mapper.Map<IEnumerable<BookUpdateDto>>
                    (_repository.GetBookForReview(reviewDto.ReviewId)).ToList();
            }
            return Ok(reviewDtos);

        }
        [AllowAnonymous]
        [HttpGet("{reviewId}", Name = "GetReviewById")]
        public ActionResult<ReviewReadDto> GetReviewById(int reviewId)
        {
            Review review = _repository.FindReviewById(reviewId);
            if (review != null)
            {
                return Ok(_mapper.Map<ReviewReadDto>(review));
            }
            return NotFound();

        }
        [Authorize(Roles = "Admin, Customer")]
        [HttpPost]
        public ActionResult<ReviewReadDto> CreateReview(ReviewCreateDto review)
        {
            var reviewModel = _mapper.Map<Review>(review);
            try
            {
                _repository.Create(reviewModel);
                _repository.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the data to the database.");
            }
            var reviewDto = _mapper.Map<ReviewUpdateDto>(reviewModel);
            return CreatedAtRoute(nameof(GetReviewById), new { reviewId = reviewDto.ReviewId }, reviewDto);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult<ReviewReadDto> Update(ReviewUpdateDto man)
        {
            try
            {
                var oldReview = _repository.FindReviewById(man.ReviewId);
                if (oldReview == null)
                {
                    return NotFound();
                }
                Review reviewEntity = _mapper.Map<Review>(man);
                _mapper.Map(reviewEntity, oldReview);
                _repository.SaveChanges();
                return Ok(_mapper.Map<ReviewReadDto>(oldReview));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{reviewId}")]
        public IActionResult Delete(int reviewId)
        {
            try
            {
                var man = _repository.FindReviewById(reviewId);
                if (man == null)
                {
                    return NotFound();
                }
                _repository.Delete(reviewId);
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
