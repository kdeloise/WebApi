using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.EF;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersRepository _usersRepository;
        private readonly ILogger _logger;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;


        public UsersController(
            IUsersRepository usersRepository,
            ILogger<UsersController> logger,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _usersRepository = usersRepository;
            _logger = logger;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _usersRepository.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                Token = tokenString
            });
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                _usersRepository.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet(Name = "GetAllItems")]
        public IActionResult Get()
        {
            var users = _usersRepository.Get();
            var model = _mapper.Map<IList<UserModel>>(users);            
            _logger.LogInformation("It's Get method");
            return Ok(model);
            //return _usersRepository.Get();
        }

        [HttpGet("{Id}", Name = "GetUser")]
        public IActionResult Get(int Id)
        {
            User user = _usersRepository.Get(Id);
            UserModel model = _mapper.Map<UserModel>(user);

            if (User == null)
            {
                return NotFound();
            }
            _logger.LogInformation($"It's Get by id={Id} method");
            return Ok(model);
        }

        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            _usersRepository.Create(user);
            _logger.LogInformation("It's Post method");
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int Id, [FromBody] User updatedUserItem)
        {
            if (updatedUserItem == null || updatedUserItem.Id != Id)
            {
                return BadRequest();
            }

            var user = _usersRepository.Get(Id);
            if (user == null)
            {
                return NotFound();
            }

            _usersRepository.Update(updatedUserItem);
            _logger.LogInformation($"It's Put id = {Id} method");
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int Id)
        {
            var deletedUserItem = _usersRepository.Delete(Id);

            if (deletedUserItem == null)
            {
                return BadRequest();
            }
            _logger.LogInformation($"It's Delete id = {Id} method");
            return Ok();
        }
    }
}
