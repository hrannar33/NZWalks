using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository UserRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            this.UserRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            //validate incoming request

            //check if user is authenticated
            //chceck user and pass
           var user = await UserRepository.Authenticate(loginRequest.Username, loginRequest.Password);

            if (user != null)
            {
                //generate Jwt Token
                var token = await tokenHandler.CreateTokenAsync(user);
                return Ok(token);
            }


        return BadRequest("Username or Password is incorrect");


        }
    }
}
