using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using TeladocUserAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TeladocUserAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {

        // GET: api/<UserController>
        [HttpGet]
        public JsonNode Get()
        {
            UserController controller = new();
            return controller.GetAllUsers();
        }

        // GET api/<UserController>/5
        [HttpGet("{email}")]
        public JsonNode Get(string email)
        {
            UserController controller = new();
            return controller.GetUserByEmail(email);
        }

        // POST api/<UserController>
        [HttpPost]
        public JsonNode Post([FromBody] UserModel user)
        {
            UserController controller = new();
            return controller.AddUser(user);
        }


        // PUT api/<UserController>/5
        [HttpPut("{email}")]
        public JsonNode Put(string email, [FromBody] UserModel user)
        {
            UserController controller = new();
            return controller.UpdateUser(email, user);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{email}")]
        public JsonNode Delete(string email)
        {
            UserController controller = new();
            return controller.DeleteUser(email);
        }

    }
}

