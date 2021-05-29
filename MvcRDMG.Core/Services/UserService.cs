using AutoMapper;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Repository;
using MvcRDMG.Core.Abstractions.Services;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Core.Domain;
using MvcRDMG.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcRDMG.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserService(IMapper mapper, IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> CreateAsync(UserModel model)
        {
            ValidateModel(model);
            await CheckUserExist(model);
            try
            {
                model.Password = PasswordHelper.EncryptPassword(model.Password);
                var user = _mapper.Map<Domain.User>(model);
                user = await _userRepository.CreateAsync(user);
                return user.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Create User failed.");
                throw;
            }
        }

        private static void ValidateModel(UserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (model.Password.Length < 8)
                throw new Exception(Resources.Error.PasswordLength);
            if (string.IsNullOrEmpty(model.Username))
                throw new Exception(string.Format(Resources.Error.RequiredValidation, model.Username));
            if (string.IsNullOrEmpty(model.FirstName))
                throw new Exception(string.Format(Resources.Error.RequiredValidation, model.FirstName));
            if (string.IsNullOrEmpty(model.LastName))
                throw new Exception(string.Format(Resources.Error.RequiredValidation, model.LastName));
            if (string.IsNullOrEmpty(model.Email))
                throw new Exception(string.Format(Resources.Error.RequiredValidation, model.Email));
            if (string.IsNullOrEmpty(model.Role))
                throw new Exception(string.Format(Resources.Error.RequiredValidation, model.Role));
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

        public async Task UpdateAsync(UserModel model)
        {
            ValidateModelForEdit(model);
            try
            {
                var user = await _userRepository.GetAsync(model.Id);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Role = (Role)Enum.Parse(typeof(Role), model.Role);
                await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update User failed.");
                throw;
            }
        }

        private static void ValidateModelForEdit(UserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrEmpty(model.FirstName))
                throw new Exception(string.Format(Resources.Error.RequiredValidation, model.FirstName));
            if (string.IsNullOrEmpty(model.LastName))
                throw new Exception(string.Format(Resources.Error.RequiredValidation, model.LastName));
            if (string.IsNullOrEmpty(model.Email))
                throw new Exception(string.Format(Resources.Error.RequiredValidation, model.Email));
            if (string.IsNullOrEmpty(model.Role))
                throw new Exception(string.Format(Resources.Error.RequiredValidation, model.Role));
        }

        public async Task ChangePasswordAsync(ChangePasswordModel model)
        {
            try
            {
                if (model.NewPassword.Length < 8)
                    throw new Exception(Resources.Error.PasswordLength);

                var user = await _userRepository.GetAsync(model.Id);

                if (user is null)
                    throw new Exception(Resources.Error.NotFound);
                if (!PasswordHelper.CheckPassword(user.Password, model.CurrentPassword))
                    throw new Exception(Resources.Error.PasswordMissMatch);

                user.Password = PasswordHelper.EncryptPassword(model.NewPassword);

                await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Change user password failed.");
                throw;
            }
        }
    }
}
