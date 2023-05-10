using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NoteApi.Data.Dtos;
using NoteApi.Data.Repositories.UserRepository;
using NoteApi.Model;
using NoteApi.Model.Dtos;
using NoteApi.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoteApi.Controllers
{
    [ApiController]
    [Route("/api/users")]
    [Produces("application/json")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public IConfiguration _configuration;
        private readonly IMapper _mapper;
        
        public UserController(IUserRepository userRepository,IMapper mapper, IConfiguration config)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _configuration = config;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            IEnumerable<User> users = await _userRepository.GetAll();
            
            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        [HttpGet("{id}", Name = "GetUserById")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            if (id == 0) return BadRequest();

            User currentUser = GetCurrentUserService.GetCurrentUser(HttpContext);

            if (currentUser.Id == 0) return Unauthorized();

            if (currentUser.Id != id) return Forbid();
            
            User user = await _userRepository.GetById(id);

            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(int id, UserUpdateDto userUpdateDto)
        {
            if (id == 0) return BadRequest();

            if (userUpdateDto == null)
            {
                return BadRequest();
            }

            User currentUser = GetCurrentUserService.GetCurrentUser(HttpContext);

            if (currentUser.Id == 0) return Unauthorized();

            if (currentUser.Id != id) return Forbid();

            User user = await _userRepository.GetById(u => u.Id == id);

            User userUpdated = _mapper.Map<User>(userUpdateDto);
            userUpdated.Password = Encrypt.GetSHA256(userUpdated.Password);

            await _userRepository.Update(userUpdated);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if(id == 0) return BadRequest();

            User currentUser = GetCurrentUserService.GetCurrentUser(HttpContext);

            if (currentUser.Id == 0) return Unauthorized();

            if (currentUser.Id != id) return Forbid();

            User user = await _userRepository.GetById(id);

            await _userRepository.Delete(user);

            return NoContent();
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register([FromBody] UserCreateDto userCreateDto)
        {
            if (userCreateDto == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _mapper.Map<User>(userCreateDto);
            user.Password = Encrypt.GetSHA256(user.Password);

            await _userRepository.Insert(user);

            return Created("GetUserById", _mapper.Map<UserDto>(user));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Login([FromBody] UserLoginDto userLoginDto)
        {
            if (userLoginDto.Email != null && userLoginDto.Password != null)
            {
                UserLoginDto userLogin = new()
                {
                    Email = userLoginDto.Email,
                    Password = Encrypt.GetSHA256(userLoginDto.Password)
                };

                User? user = await _userRepository.Login(userLogin);

                if (user != null)
                {
                    var issuer = _configuration["Jwt:Issuer"];
                    var audience = _configuration["Jwt:Audience"];
                    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("Id", user.Id.ToString()),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name, user.Name)
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(60),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials
                        (new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var stringToken = tokenHandler.WriteToken(token);
                    return Ok(new TokenResponse { Token = stringToken });
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
