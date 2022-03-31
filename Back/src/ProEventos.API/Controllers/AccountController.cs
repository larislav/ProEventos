using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        public AccountController(IAccountService accountService,
                                ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpGet("GetUser")]
        // [AllowAnonymous]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsync(userName);
                return Ok(user);
            }
           catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar Usuário. Erro: {ex.Message}");
            }
        }


        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                if(await _accountService.UserExists(userDto.Username))
                    return BadRequest("Usuário já existe");
                
                var user = await _accountService.CreateAccountAsync(userDto);
                if (user != null)
                    return Ok(user);
                
                return BadRequest("Usuário não criado, tente novamente mais tarde!");
            }
           catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar criar o Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(userLogin.Username);
                if(user == null)
                    return Unauthorized("Usuário não encontrado");

                var result = await _accountService.CheckUserPasswordAsync(user, userLogin.Password);
                if(!result.Succeeded) return Unauthorized("Usuário ou Senha incorreto(s)");

                return Ok(new 
                {
                    userName = user.Username,
                    PrimeiroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result                
                });
            }
           catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar realizar login. Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        //[AllowAnonymous]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if(user == null)
                    return Unauthorized("Usuário inválido");
                
                var userReturn = await _accountService.UpdateAccount(userUpdateDto);
                if (userReturn != null)
                    return NoContent();
                
                return Ok(userReturn);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar o Usuário. Erro: {ex.Message}");
            }
        }
    }
}