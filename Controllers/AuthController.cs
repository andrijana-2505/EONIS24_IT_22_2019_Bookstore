using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendBookstore.DTOs;
using BackendBookstore.DTOs.CreateDTO;
using BackendBookstore.DTOs.ReadDTO;
using BackendBookstore.DTOs.UpdateDTO;
using BackendBookstore.Models;
using BackendBookstore.Repositories.Interface;
using BackendBookstore.Repositories.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
            if (_repository.FindByEmail(users.Email) != null)
                return BadRequest("User already registered!");

            // Mapiranje korisnika i hashovanje lozinke
            var userModel = _mapper.Map<User>(users);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(users.Password);
            userModel.PasswordLogin = passwordHash;

            try
            {
                // Kreiranje korisnika
                _repository.Create(userModel);
                _repository.SaveChanges();
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the data to the database.");
                Console.WriteLine($"An error occurred while saving the data to the database: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the data to the database.");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<UserReadDto> Login(UserLoginDto request)
        {
            //Pretraga korisnika po username-u
            var user = _repository.FindByEmail(request.Email);
            if (user == null || user.Email != request.Email)
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
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(UserRole), user.UserRole))
            };

            //Konfiguracija za kljuc
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
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
