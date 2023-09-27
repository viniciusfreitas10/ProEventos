using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProEventos.Application.Contratos;
using ProEventos.Persistence.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProEventos.Application
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserPersist _userPersist;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUserPersist userPersist)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userPersist = userPersist;
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager.Users
                    .SingleOrDefaultAsync(user 
                                            => user.UserName.ToLower() == userUpdateDto.UserName.ToLower());

                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch (Exception e)
            {
                throw new Exception($"erro ao tentar verificar o password. {e.Message}");
            }
        }

        public async Task<UserDto> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if(result.Succeeded)
                {
                    var userToReturn = _mapper.Map<UserDto>(user);

                    return userToReturn;
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception($"erro ao tentar criar uma conta. {e.Message}");
            }
        }

        public async Task<UserUpdateDto> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(userName);

                if (user == null) return null;

                var userReturn = _mapper.Map<UserUpdateDto>(user); 

                return userReturn;
            }
            catch (Exception e)
            {
                throw new Exception($"erro ao tentar recuperar um usuario pelo userName. {e.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(userUpdateDto.UserName);
                if(user == null) return null;

                _mapper.Map(userUpdateDto, user);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);

                _userPersist.Update<User>(user);

                if(await _userPersist.SaveChangesAsync())
                {
                    var userReturn = await _userPersist.GetUserByUserNameAsync(user.UserName);

                    return _mapper.Map<UserUpdateDto>(userReturn);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception($"erro ao tentar atualizar uma conta. {e.Message}");
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager.Users
                    .AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
            }
            catch (Exception e)
            {
                throw new Exception($"erro ao tentar verificar a existencia do usuario. {e.Message}");
            }
        }
    }
}
