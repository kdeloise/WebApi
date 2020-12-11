using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public UsersController(IUsersRepository usersRepository)
        {
            UsersRepository = usersRepository;
        }

        [HttpGet(Name = "GetAllItems")]
        public IEnumerable<User> Get()
        {
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
            return new ObjectResult(deletedUserItem);
        }
    }
}
