using AutoMapper;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Exceptions;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using RDMG.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDMG.Core.Services;

public class UserService(IMapper mapper, IUserRepository userRepository, ILogger<UserService> logger) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

    public async Task<int> CreateAsync(UserModel model)
    {
        ValidateModel(model);
        await CheckUserExist(model);
        try
        {
            model.Password = PasswordHelper.EncryptPassword(model.Password);
            var user = _mapper.Map<User>(model);
            user = await _userRepository.CreateAsync(user);
            return user.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Create User failed.");
            throw;
        }
    }

    private static void ValidateModel(UserModel model)
    {
        var errors = new List<ServiceException>();
        ArgumentNullException.ThrowIfNull(model);
        if (model.Password.Length < 8)
            errors.Add(new ServiceException(Resources.Error.PasswordLength));
        if (string.IsNullOrEmpty(model.Username))
            errors.Add(new ServiceException(string.Format(Resources.Error.RequiredValidation, model.Username)));
        if (string.IsNullOrEmpty(model.FirstName))
            errors.Add(new ServiceException(string.Format(Resources.Error.RequiredValidation, model.FirstName)));
        if (string.IsNullOrEmpty(model.LastName))
            errors.Add(new ServiceException(string.Format(Resources.Error.RequiredValidation, model.LastName)));
        if (string.IsNullOrEmpty(model.Email))
            errors.Add(new ServiceException(string.Format(Resources.Error.RequiredValidation, model.Email)));
        if (string.IsNullOrEmpty(model.Role))
            errors.Add(new ServiceException(string.Format(Resources.Error.RequiredValidation, model.Role)));
        if (errors.Count != 0)
            throw new ServiceAggregateException(errors);
    }

    private async Task CheckUserExist(UserModel model)
    {
        var user = await _userRepository.GetByUsernameAsync(model.Username, null);
        if (user is not null)
            throw new ServiceException(string.Format(Resources.Error.UserExist, model.Username));
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            return await _userRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete User failed.");
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
            _logger.LogError(ex, "Get User failed.");
            throw;
        }
    }

    public async Task<IEnumerable<UserModel>> ListAsync(bool? deleted = false)
    {
        try
        {
            var result = await _userRepository.ListAsync(deleted);

            return result.Select(_mapper.Map<UserModel>)
                .OrderBy(um => um.Username)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "List Users failed.");
            throw;
        }
    }

    public async Task<bool> RestoreAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetAsync(id);
            if (user is not null)
            {
                user.IsDeleted = false;
                await _userRepository.UpdateAsync(user);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Restore User #{userid} failed.", id);
            throw;
        }
    }

    public async Task UpdateAsync(UserModel model)
    {
        ValidateModelForEdit(model);
        try
        {
            var user = await _userRepository.GetAsync(model.Id);
            if (user is not null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Role = Enum.Parse<Role>(model.Role);
                await _userRepository.UpdateAsync(user);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update User failed.");
            throw;
        }
    }

    private static void ValidateModelForEdit(UserModel model)
    {
        var errors = new List<ServiceException>();
        ArgumentNullException.ThrowIfNull(model);
        if (string.IsNullOrEmpty(model.FirstName))
            errors.Add(new ServiceException(string.Format(Resources.Error.RequiredValidation, model.FirstName)));
        if (string.IsNullOrEmpty(model.LastName))
            errors.Add(new ServiceException(string.Format(Resources.Error.RequiredValidation, model.LastName)));
        if (string.IsNullOrEmpty(model.Email))
            errors.Add(new ServiceException(string.Format(Resources.Error.RequiredValidation, model.Email)));
        if (string.IsNullOrEmpty(model.Role))
            errors.Add(new ServiceException(string.Format(Resources.Error.RequiredValidation, model.Role)));
        if (errors.Count != 0)
            throw new ServiceAggregateException(errors);
    }

    public async Task ChangePasswordAsync(ChangePasswordModel model)
    {
        try
        {
            if (model.NewPassword.Length < 8)
                throw new ServiceException(Resources.Error.PasswordLength);

            var user = await _userRepository.GetAsync(model.Id) ?? throw new ServiceException(Resources.Error.NotFound);

            if (!PasswordHelper.CheckPassword(user.Password, model.CurrentPassword))
                throw new ServiceException(Resources.Error.PasswordMissMatch);

            user.Password = PasswordHelper.EncryptPassword(model.NewPassword);

            await _userRepository.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Change user password failed.");
            throw;
        }
    }
}