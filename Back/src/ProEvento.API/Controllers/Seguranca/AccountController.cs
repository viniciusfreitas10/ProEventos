using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using ProEvento.API.Helpers;
using ProEventos.Application.Dtos;

namespace ProEvento.API.Controllers.Seguranca
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        
        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpGet("GetUser/{userName}")]
        public async Task<IActionResult> GetUser(string userName)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(userName);

                if(user == null) 
                    return NoContent();
                
                return Ok(user);
            }
            catch (Exception e)
            {
                Logger.Log("GetUser", $"User: {userName} - {e.Message}", "Error");
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar Usu�rio. Erro: {e.Message}");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                if (await _accountService.UserExists(userDto.UserName))
                    return BadRequest("User exists");

                var User = await _accountService.CreateAccountAsync(userDto);

                if (User != null)
                    return Ok(User);

                return BadRequest("User was not created. Try again");
            }
            catch (Exception e)
            {
                Logger.Log("Register", $"User: {userDto.UserName} - {e.Message}", "Error");
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar registrar um Usu�rio. Erro: {e.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(userLogin.UserName);

                if (user == null)
                    return Unauthorized("Usu�rio ou senha inv�lida");

                var result = await _accountService.CheckUserPasswordAsync(user, userLogin.Password);

                if (!result.Succeeded)
                    return Unauthorized();

                return Ok(new
                {
                    userName = user.UserName,
                    primeiroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result
                });
            }
            catch (Exception e)
            {
                Logger.Log("GetUser", $"User: {userLogin.UserName} - {e.Message}", "Error");
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar Usu�rio. Erro: {e.Message}");
            }
        }
    } 
}