using AutoMapper;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Repository;
using MvcRDMG.Core.Abstractions.Services;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Core.Helpers;
using System;
using System.Threading.Tasks;

namespace MvcRDMG.Core.Services
{
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

                if (PasswordHelper.CheckPassword(user.Password, model.Password))
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
    }
}
