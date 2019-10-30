using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public UserModel Create(UserModel model)
        {
            CheckUserExist(model);
            try
            {
                model.Password = GetSavedPasswordHash(model.Password);
                var user = _mapper.Map<Domain.User>(model);
                user = _userRepository.Create(user);
                return _mapper.Map<UserModel>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Create User failed.");
                throw;
            }
        }

        private void CheckUserExist(UserModel model)
        {
            var user = _userRepository.GetByUsername(model.Username, null);
            if (user != null)
                throw new Exception(string.Format(Resources.Error.UserExist, model.Username));
        }

        public bool Delete(int id)
        {
            return _userRepository.Delete(id);
        }

        public UserModel Get(int id)
        {
            var user = _userRepository.Get(id);

            return _mapper.Map<UserModel>(user);
        }

        public IEnumerable<UserModel> List(bool? deleted = false)
        {
            return _userRepository.List(deleted)
                                    .Select(p => _mapper.Map<UserModel>(p))
                                    .OrderBy(um => um.Username)
                                    .ToList();
        }

        public UserModel Login(UserModel model)
        {
            var user = _userRepository.GetByUsername(model.Username);

            if (user == null)
                return null;

            if (CheckPassword(user.Password, model.Password))
                return _mapper.Map<UserModel>(user);
            else
                return null;
        }

        public bool Restore(int id)
        {
            try
            {
                var model = _userRepository.Get(id);
                model.Deleted = false;
                _userRepository.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Restore User #{id} failed.");
                throw;
            }
        }

        public UserModel Update(UserModel model)
        {
            model.Password = GetSavedPasswordHash(model.Password);
            var user = _mapper.Map<Domain.User>(model);

            user = _userRepository.Update(user);

            return _mapper.Map<UserModel>(user);
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
