using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Interfaces;
using TeamColabApp.Mappers;
using TeamColabApp.Misc;
using TeamColabApp.Contexts;

namespace TeamColabApp.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<long, User> _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly TeamColabAppContext _context;

        public UserService(IRepository<long, User> userRepository, IEncryptionService encryptionService, TeamColabAppContext context)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _context = context;
        }

        public override async Task<UserResponseDto> GetUserByName(string name)
        {
            IEnumerable<User?> allUsers = await _userRepository.GetAllAsync();
            User? user = allUsers.FirstOrDefault(u => u != null && u.Name == name && u.IsActive != false);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with name '{name}' not found.");
            }

            return UserMapper.EntityToResponseDto(user);
        }

        public override async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            IEnumerable<User?> users = await _userRepository.GetAllAsync();
            List<UserResponseDto> response = new List<UserResponseDto>();

            foreach (User? user in users)
            {
                if (user != null && user.IsActive==true)
                {
                    response.Add(UserMapper.EntityToResponseDto(user));
                }
            }

            return response;
        }

        public override async Task<UserResponseDto> AddUserAsync(UserRequestDto userRequestDto)
        {
            IEnumerable<User?> users = await _userRepository.GetAllAsync();
            User? existingUser = users.FirstOrDefault(u => u.Name == userRequestDto.Name);
            if (existingUser != null)
                throw new Exception("Duplicate User");

            User newUser = UserMapper.RequestDtoToEntity(userRequestDto);
            if (string.IsNullOrEmpty(userRequestDto.Name) || string.IsNullOrEmpty(userRequestDto.Password))
            {
                throw new ArgumentException("Name and Password cannot be empty.");
            }

            EncryptModel encrtyptedUser = await _encryptionService.EncryptData(new EncryptModel
            {
                Data = userRequestDto.Password
            });
            newUser.Password = encrtyptedUser.EncryptedString;
            newUser.CreatedAt = DateTime.UtcNow;
            newUser.UpdatedAt = DateTime.UtcNow;
            newUser.IsActive = true;
            newUser.Role = userRequestDto.Role ?? "User";

            User? createdUser = await _userRepository.AddAsync(newUser);

            if (createdUser == null)
            {
                throw new Exception("Failed to add user.");
            }

            return UserMapper.EntityToResponseDto(createdUser);
        }

        public override async Task<UserResponseDto> UpdateUserAsync(UserRequestDto userRequestDto, string name)
        {
            IEnumerable<User?> users = await _userRepository.GetAllAsync();
            User? existingUser = users.FirstOrDefault(u =>u!=null && u.Name == userRequestDto.Name);
            if (existingUser != null)
                throw new Exception("Duplicate User");
            long userId = EntityNameToId.GetUserId(name, _context);
            Console.WriteLine("UpdateUserStart");
            if (userId == 0)
            {
                throw new KeyNotFoundException($"User with name '{name}' not found.");
            }
            existingUser = await _userRepository.GetByIdAsync(userId);
            EncryptModel encryptedUser = await _encryptionService.EncryptData(new EncryptModel
            {
                Data = userRequestDto.Password,
            });
           
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID '{userId}' not found.");
            }

            existingUser.Name = userRequestDto.Name;
            existingUser.Password = encryptedUser.EncryptedString;
            existingUser.Role = userRequestDto.Role;
            // existingUser.IsActive = userRequestDto.IsActive;
            existingUser.UpdatedAt = DateTime.UtcNow;
             Console.WriteLine("UpdateUserend");
            User? updatedUser = await _userRepository.UpdateAsync(userId,existingUser);
            Console.WriteLine("number3");
            if (updatedUser == null)
            {
                throw new Exception("Failed to update user.");
            }

            return UserMapper.EntityToResponseDto(updatedUser);
        }

        public override async Task<bool> HardDeleteUserAsync(string name)
        {
            long userId = EntityNameToId.GetUserId(name, _context);
            if (userId == 0)
            {
                throw new KeyNotFoundException($"User with name '{name}' not found.");
            }
            User? userToDelete = await _userRepository.GetByIdAsync(userId);

            if (userToDelete == null)
            {
                throw new KeyNotFoundException($"User with ID '{userId}' not found.");
            }

            return await _userRepository.DeleteAsync(userId);
        }
        public override async Task<bool> SoftDeleteUserAsync(string name)
        {
            IEnumerable<User?> users = await _userRepository.GetAllAsync();
            User? user = users.FirstOrDefault(u => u!= null && u.Name == name, null);
            if (user != null)
            {
                user.IsActive = false;
                await _userRepository.UpdateAsync(user.UserId,user);
                return true;
            }
            else
            {
                return false;
                throw new Exception("User not found");
            }
        }
        public override async Task<UserResponseDto> RetrieveSoftDeleteUserAsync(string name)
        {
            long userId = EntityNameToId.GetUserId(name, _context);
            User? user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = true;
                User? updatedUser = await _userRepository.UpdateAsync(userId,user);
                UserResponseDto updatedUserDto = UserMapper.EntityToResponseDto(updatedUser);
                return updatedUserDto;
            }
            else
            {
                throw new Exception("User not found");
            }
        }
        public override async Task<IEnumerable<UserResponseDto>> GetUsersByRoleAsync(string role)
        {
            IEnumerable<User?> users = await _userRepository.GetAllAsync();
            List<UserResponseDto> filteredUsers = new List<UserResponseDto>();

            foreach (User? user in users)
            {
                if (user != null && user.IsActive != false && user.Role.Equals(role, StringComparison.OrdinalIgnoreCase))
                {
                    filteredUsers.Add(UserMapper.EntityToResponseDto(user));
                }
            }

            return filteredUsers;
        }
        public override async Task<IEnumerable<UserResponseDto>> GetUsersUnderProjectAsync(string name)
        {
            IEnumerable<User?> users = await _userRepository.GetAllAsync();
            List<UserResponseDto> result = new List<UserResponseDto>();

            foreach (User? user in users)
            {
                if (user != null && user.IsActive != false &&  user.Projects != null &&
                    user.Projects.Any(p => p.Title.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    result.Add(UserMapper.EntityToResponseDto(user));
                }
            }
            return result;
        }
        
    }
}
