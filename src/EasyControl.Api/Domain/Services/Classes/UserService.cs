using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using AutoMapper;
using EasyControl.Api.Contract.User;
using EasyControl.Api.Domain.Models;
using EasyControl.Api.Domain.Repository.Interfaces;
using EasyControl.Api.Domain.Services.Interfaces;

namespace EasyControl.Api.Domain.Services.Classes
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        private readonly TokenService _tokenService;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            TokenService tokenService
        )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<UserLoginResponseContract> Authentication(
            UserLoginRequestContract userContract
        )
        {
            User? user = await _userRepository.GetByEmail(userContract.Email);

            var passwordHash = CreatePasswordHash(userContract.Password);

            if (user is null || user.Password != passwordHash)
            {
                throw new AuthenticationException("Invalid credentials.");
            }

            var token = _tokenService.GenerateToken(user);

            var response = _mapper.Map<UserLoginResponseContract>(user);

            response.Token = token;

            return response;
        }

        public async Task<UserCreateResponseContract> Create(UserCreateRequestContract userContract)
        {
            ValidatesUserData(userContract);

            await ValidateEmailExists(userContract.Email);

            var user = _mapper.Map<User>(userContract);

            user.Password = CreatePasswordHash(user.Password);

            await _userRepository.Create(user);

            return _mapper.Map<UserCreateResponseContract>(user);
        }

        public async Task<UserCreateResponseContract> Update(
            Guid userId,
            UserCreateRequestContract userContract
        )
        {
            ValidatesUserData(userContract);

            User? user = await GetUserById(userId);

            if (user.Email != userContract.Email)
            {
                await ValidateEmailExists(userContract.Email);
            }

            _mapper.Map(userContract, user);

            user.Password = CreatePasswordHash(user.Password);

            user = await _userRepository.Update(user);

            return _mapper.Map<UserCreateResponseContract>(user);
        }

        public async Task<IEnumerable<UserCreateResponseContract>> GetAll()
        {
            var users = await _userRepository.GetAll();

            return _mapper.Map<IEnumerable<UserCreateResponseContract>>(users);
        }

        public async Task<UserCreateResponseContract> GetById(Guid userId)
        {
            var user = await GetUserById(userId);

            return _mapper.Map<UserCreateResponseContract>(user);
        }

        public async Task<UserCreateResponseContract> GetByEmail(string email)
        {
            var user =
                await _userRepository.GetByEmail(email) ?? throw new Exception("User not found.");

            return _mapper.Map<UserCreateResponseContract>(user);
        }

        private async Task<User> GetUserById(Guid id)
        {
            return await _userRepository.GetById(id) ?? throw new Exception("User not found.");
        }

        public async Task Delete(Guid id)
        {
            int response = await _userRepository.Delete(id);

            if (response == 0)
            {
                throw new Exception("Error when deleting user.");
            }
        }

        private static string CreatePasswordHash(string password)
        {
            byte[] bytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(password));

            string passwordHash = Convert.ToBase64String(bytes);

            return passwordHash;
        }

        public async Task ValidateEmailExists(string email)
        {
            var userByEmail = await _userRepository.GetByEmail(email);

            if (userByEmail != null)
            {
                throw new Exception("Email already exists.");
            }
        }

        public void ValidatesUserData(UserCreateRequestContract user)
        {
            IsStringValid(user.Name, "name");
            IsEmailValid(user.Email);
            IsStringValid(user.Password, "password");
        }

        public void IsEmailValid(string email)
        {
            string emailPattern = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            bool isEmailValid = Regex.IsMatch(email, emailPattern);

            if (!isEmailValid)
            {
                throw new Exception("Invalid email.");
            }
        }

        public void IsStringValid(string value, string title)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception($"Invalid {title}.");
            }
        }
    }
}
