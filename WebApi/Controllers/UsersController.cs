using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUsersRepository UsersRepository;
        private readonly ILogger _logger;

        public UsersController(IUsersRepository usersRepository, ILogger<UsersController> logger)
        {
            UsersRepository = usersRepository;
            _logger = logger;
        }

        [HttpGet(Name = "GetAllItems")]
        public IEnumerable<User> Get()
        {
            _logger.LogInformation("It's Get method");
            return UsersRepository.Get();
        }

        [HttpGet("{Id}", Name = "GetUser")]
        public IActionResult Get(int Id)
        {
            User user = UsersRepository.Get(Id);

            if (User == null)
            {
                return NotFound();
            }
            _logger.LogInformation(new ObjectResult(user).ToString());
            return new ObjectResult(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            UsersRepository.Create(user);
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

            var user = UsersRepository.Get(Id);
            if (user == null)
            {
                return NotFound();
            }

            UsersRepository.Update(updatedUserItem);
            _logger.LogInformation("It's Put<id> method");
            return RedirectToRoute("GetAllItems");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int Id)
        {
            var deletedUserItem = UsersRepository.Delete(Id);

            if (deletedUserItem == null)
            {
                return BadRequest();
            }
            _logger.LogInformation("It's Delete<id> method");
            return new ObjectResult(deletedUserItem);
        }
    }
}
