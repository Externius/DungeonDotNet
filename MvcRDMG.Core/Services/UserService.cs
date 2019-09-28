using System;
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
            try
            {
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

        public bool Delete(int id)
        {
            return _userRepository.Delete(id);
        }

        public UserModel Get(int id)
        {
            var user = _userRepository.Get(id);

            return _mapper.Map<UserModel>(user);
        }

        public UserModel Login(UserModel model)
        {
            return _mapper.Map<UserModel>(_userRepository.GetByNameAndPass(model.UserName, model.Password));
        }

        public UserModel Update(UserModel model)
        {
            var user = _mapper.Map<Domain.User>(model);

            user = _userRepository.Update(user);

            return _mapper.Map<UserModel>(user);
        }
    }
}
