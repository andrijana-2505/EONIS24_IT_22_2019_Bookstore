using AutoMapper;
using BackendBookstore.DTOs;
using BackendBookstore.DTOs.CreateDTO;
using BackendBookstore.DTOs.ReadDTO;
using BackendBookstore.Models;
using BackendBookstore.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendBookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IUserRepo _repository;

        public AuthController(IMapper mapper, IConfiguration config, IUserRepo repository)
        {
            _mapper = mapper;
            _config = config;
            _repository = repository;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult<UserReadDto> Register(UserCreateDto users)
        {
            // Proveravanje da li korisnik već postoji
            if (_repository.FindByUserName(users.Username) != null)
                return BadRequest("User already registered!");

            // Mapiranje korisnika i hashovanje lozinke
            var userModel = _mapper.Map<User>(users);
            userModel.PasswordLogin = BCrypt.Net.BCrypt.HashPassword(users.PasswordLogin);

            try
            {
                // Kreiranje korisnika
                _repository.Create(userModel);
                _repository.SaveChanges();
                return Ok();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the data to the database.");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<UserReadDto> Login(UserLoginDto request)
        {
            //Pretraga korisnika po username-u
            var user = _repository.FindByUserName(request.Username);
            if (user == null || user.Username != request.Username)
                return BadRequest("User not found.");
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordLogin))
                return BadRequest("Wrong password!");

            //Kreiranje tokena i vracanje kao odgovor
            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            //Kreiranje liste claimova za token
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(UserRole), user.UserRole))
            };

            //Konfiguracija za kljuc
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Authentication:Schemas:Bearer:SigningKeys:0:Value").Value!));
            var cred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            //Kreiranje tokena
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }
    }
}
