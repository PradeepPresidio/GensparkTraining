using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamColabApp.Interfaces;
using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Misc;
using TeamColabApp.Contexts;
using System.IdentityModel.Tokens.Jwt;

namespace TeamColabApp.Services
{
    public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenService _tokenService;
    private readonly IEncryptionService _encryptionService;
    private readonly IRepository<long, User> _userRepository;
    private readonly IRepository<long, UserToken> _tokenRepository;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly TeamColabAppContext _context;

    public AuthenticationService(
        ITokenService tokenService,
        IEncryptionService encryptionService,
        IRepository<long, User> userRepository,
        IRepository<long, UserToken> tokenRepository,
        ILogger<AuthenticationService> logger,
        TeamColabAppContext context)
    {
        _tokenService = tokenService;
        _encryptionService = encryptionService;
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _logger = logger;
        _context = context;
    }

    public async Task<UserLoginResponseDto> Login(UserLoginRequestDto loginRequest)
    {
        long id = EntityNameToId.GetUserId(loginRequest.Username, _context);
        User? dbUser = await _userRepository.GetByIdAsync(id);

        if (dbUser == null)
            throw new Exception("Invalid username or password.");

        bool isPasswordValid = await _encryptionService.VerifyPassword(
            loginRequest.Password, dbUser.Password
        );

        if (!isPasswordValid)
            throw new Exception("Invalid username or password.");

        string token = await _tokenService.GenerateToken(dbUser);
        string refreshToken = _tokenService.GenerateRefreshToken();

        await _tokenRepository.AddAsync(new UserToken
        {
            TokenValue = refreshToken,
            TokenType = TokenType.Refresh,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        return new UserLoginResponseDto
        {
            Username = dbUser.Name,
            Token = token,
            RefreshToken = refreshToken
        };
    }

    public async Task<bool> Logout(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
            return false;

        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
        if (jwtToken == null)
            return false;

        await _tokenRepository.AddAsync(new UserToken
        {
            TokenValue = token,
            TokenType = TokenType.Blacklist,
            ExpiresAt = jwtToken.ValidTo
        });

        return true;
    }

    public async Task<UserLoginResponseDto?> RefreshToken(string refreshToken, string username)
    {
        UserToken? storedToken = (await _tokenRepository.GetAllAsync())
            .FirstOrDefault(t => t!=null && t.TokenValue == refreshToken && t.ExpiresAt > DateTime.UtcNow);

        if (storedToken == null)
            return null;

        long userId = EntityNameToId.GetUserId(username, _context);
        User? dbUser = await _userRepository.GetByIdAsync(userId);
        if (dbUser == null)
            return null;

        await _tokenRepository.DeleteAsync(storedToken.TokenId); 

        string newAccessToken = await _tokenService.GenerateToken(dbUser);
        string newRefreshToken = _tokenService.GenerateRefreshToken();

        await _tokenRepository.AddAsync(new UserToken
        {
            TokenValue = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        return new UserLoginResponseDto
        {
            Username = dbUser.Name,
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}

}
