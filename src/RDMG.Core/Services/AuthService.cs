using AutoMapper;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Helpers;
using System;
using System.Threading.Tasks;

namespace RDMG.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public AuthService(IMapper mapper, IUserRepository userRepository, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserModel> LoginAsync(UserModel model)
    {
        try
        {
            var user = await _userRepository.GetByUsernameAsync(model.Username);

            if (user == null)
                return null;

            return PasswordHelper.CheckPassword(user.Password, model.Password) ? _mapper.Map<UserModel>(user) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login User failed.");
            throw;
        }
    }
}