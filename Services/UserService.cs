using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_teamwork.EntityFramework;
using backend_teamwork1.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend_teamwork.Services
{
    public class UserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(AppDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            return user != null ? MapToDto(user) : null;
        }

     public async Task<List<UserDto>> GetAllUsersAsync(int pageNumber, int pageSize)
{
    try
    {
        var users = await _dbContext.Users
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return users.Select(MapToDto).ToList();
    }
    catch (Exception ex)
    {
        throw new Exception("Error occurred while fetching users.", ex);
    }
}

        public async Task<User> AddUserAsync(CreateUserDto createUserDto)
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Role = createUserDto.Role,
                Name = createUserDto.Name,
                Address = createUserDto.Address,
                Email = createUserDto.Email,
                Phone = createUserDto.Phone
            };

            user.Password = _passwordHasher.HashPassword(user, createUserDto.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<UserDto?> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto)
        {
            var existingUser = await _dbContext.Users.FindAsync(userId);
            if (existingUser == null)
            {
                throw new InvalidOperationException("User not found");
            }

            existingUser.Role = updateUserDto.Role;
            existingUser.Name = updateUserDto.Name;
            existingUser.Address = updateUserDto.Address;
            existingUser.Email = updateUserDto.Email;

            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                existingUser.Password = _passwordHasher.HashPassword(existingUser, updateUserDto.Password);
            }

            existingUser.Phone = updateUserDto.Phone;

            await _dbContext.SaveChangesAsync();

            return MapToDto(existingUser);
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var userToDelete = await _dbContext.Users.FindAsync(userId);
            if (userToDelete == null)
            {
                return false;
            }

            _dbContext.Users.Remove(userToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public bool VerifyPassword(User user, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, providedPassword);
            return result == PasswordVerificationResult.Success;
        }

        public async Task<UserDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !VerifyPassword(user, loginDto.Password))
            {
                return null; 
            }

            return MapToDto(user);
        }

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Role = user.Role,
                Name = user.Name,
                Address = user.Address,
                Email = user.Email,
                Phone = user.Phone
            };
        }
    }
}


