using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;
using Ecommerce.Core.IRepositories;
using Ecommerce.Core.IRepositories.IServices;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext dbContext;
        private readonly UserManager<LocalUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<LocalUser> signInManager;
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;

        public UserRepository(AppDbContext dbContext, 
                                UserManager<LocalUser> userManager, 
                                RoleManager<IdentityRole> roleManager, 
                                SignInManager<LocalUser> signInManager,
                                IMapper mapper,
                                ITokenService tokenService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }

        public bool IsUniqueUser(string Email)
        {
            var result = dbContext.LocalUsers.FirstOrDefault(x => x.Email == Email);
            return result == null;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDTO.Email);

            var checkPassword = await signInManager.CheckPasswordSignInAsync(user, loginRequestDTO.Password, false);
            if(!checkPassword.Succeeded)
            {
                return new LoginResponseDTO()
                {
                    User = null,
                    Token = "",
                };
            }

            var role = await userManager.GetRolesAsync(user);
            return new LoginResponseDTO()
            {
                User = mapper.Map<LocalUserDTO>(user),
                Token = await tokenService.CreateTokenAsync(user),
                Role = role.FirstOrDefault(),
            };
        }

        public async Task<LocalUserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            var user = new LocalUser
            {
                Email = registerationRequestDTO.Email,
                UserName = registerationRequestDTO.Email.Split('@')[0],
                FirstName = registerationRequestDTO.Fname,
                LastName = registerationRequestDTO.Lname,
                Address  = registerationRequestDTO.Address,

            };

            using(var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await userManager.CreateAsync(user, registerationRequestDTO.Password);
                    if(result.Succeeded)
                    {
                        //var roles = await roleManager.Roles.ToListAsync(); // fetch all roles
                        //foreach(var role in roles)
                        //{
                        //    if(!await roleManager.RoleExistsAsync(role.Name))
                        //    {
                        //        await roleManager.CreateAsync(new IdentityRole(role.Name));
                        //    }
                        //}

                        var roleExist = await roleManager.RoleExistsAsync(registerationRequestDTO.Role);
                        if (!roleExist)
                        {
                            throw new Exception($"The role {registerationRequestDTO.Role} doesn't exist!");
                        }

                        var userRoleResult = await userManager.AddToRoleAsync(user, registerationRequestDTO.Role);
                        if (userRoleResult.Succeeded)
                        {
                            await transaction.CommitAsync();
                            //var userReturn = dbContext.LocalUsers.FirstOrDefault(u => u.Email == registerationRequestDTO.Email);
                            return mapper.Map<LocalUserDTO>(user);
                        }
                        else
                        {
                            await transaction.RollbackAsync(); // RollBack transaction if adding to UsersRoles fails
                            throw new Exception("User Registrated failed");
                        }
                    }
                    else
                    {
                        //await transaction.RollbackAsync(); // RollBack transaction if add user fails
                        throw new Exception("User Registrated failed");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }
    }
}
