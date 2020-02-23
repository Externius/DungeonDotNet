using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Repository;
using MvcRDMG.Core.Abstractions.Services;
using MvcRDMG.Core.Abstractions.Services.Models;

namespace MvcRDMG.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserService (IMapper mapper, IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserModel> CreateAsync(UserModel model)
        {
            await CheckUserExist(model);
            try
            {
                model.Password = GetSavedPasswordHash(model.Password);
                var user = _mapper.Map<Domain.User>(model);
                user = await _userRepository.CreateAsync(user);
                return _mapper.Map<UserModel>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Create User failed.");
                throw;
            }
        }

        private async Task CheckUserExist(UserModel model)
        {
            var user = await _userRepository.GetByUsernameAsync(model.Username, null);
            if (user != null)
                throw new Exception(string.Format(Resources.Error.UserExist, model.Username));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _userRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Delete User failed.");
                throw;
            }
        }

        public async Task<UserModel> GetAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetAsync(id);

                return _mapper.Map<UserModel>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Get User failed.");
                throw;
            }
        }

        public async Task<IEnumerable<UserModel>> ListAsync(bool? deleted = false)
        {
            try
            {
                var result = await _userRepository.ListAsync(deleted);

                return result.Select(p => _mapper.Map<UserModel>(p))
                        .OrderBy(um => um.Username)
                        .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"List Users failed.");
                throw;
            }
        }

        public async Task<UserModel> LoginAsync(UserModel model)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(model.Username);

                if (user == null)
                    return null;

                if (CheckPassword(user.Password, model.Password))
                    return _mapper.Map<UserModel>(user);
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Login User failed.");
                throw;
            }
        }

        public async Task<bool> RestoreAsync(int id)
        {
            try
            {
                var model = await _userRepository.GetAsync(id);
                model.Deleted = false;
                await _userRepository.UpdateAsync(model);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Restore User #{id} failed.");
                throw;
            }
        }

        public async Task<UserModel> UpdateAsync(UserModel model)
        {
            try
            {
                model.Password = GetSavedPasswordHash(model.Password);
                var user = _mapper.Map<Domain.User>(model);

                user = await _userRepository.UpdateAsync(user);

                return _mapper.Map<UserModel>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update User failed.");
                throw;
            }
        }

        private string GetSavedPasswordHash(string password)
        {
            var passwordHash = "";

            using (var rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var salt = new byte[16];
                rNGCryptoServiceProvider.GetBytes(salt);
                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
                var hash = pbkdf2.GetBytes(20);
                var hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
                passwordHash = Convert.ToBase64String(hashBytes);
            }

            return passwordHash;
        }

        private bool CheckPassword(string savedPasswordHash, string password)
        {
            var result = true;
            var hashBytes = Convert.FromBase64String(savedPasswordHash);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}
