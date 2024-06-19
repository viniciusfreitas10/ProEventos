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
using System.Security.Claims;
using ProEvento.API.Extensions;

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

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();

                var user = await _accountService.GetUserByUserNameAsync(userName.ToString());

                if(user == null) 
                    return NoContent();
                
                return Ok(user);
            }
            catch (Exception e)
            {
                Logger.Log("GetUser", $"User:  - {e.Message}", "Error");
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar Usuário. Erro: {e.Message}");
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
                    $"Erro ao tentar registrar um Usuário. Erro: {e.Message}");
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
                    return Unauthorized("Usuário ou senha inválida");

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
                    $"Erro ao tentar fazer login do Usuário. Erro: {e.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName()); 

                if (user == null)
                    return Unauthorized("Usuário ou senha inválida");


                var userReturn = await _accountService.UpdateAccount(userUpdateDto);
                if(userReturn == null) return NoContent();

                return Ok(userReturn);
            }
            catch (Exception e)
            {
                Logger.Log("UpdateUser", $"User: {userUpdateDto.UserName} - {e.Message}", "Error");
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar um Usuário. Erro: {e.Message}");
            }
        }
    } 
}