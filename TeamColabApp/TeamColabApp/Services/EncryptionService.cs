using BCrypt.Net;
using TeamColabApp.Interfaces;
using TeamColabApp.Models;
using BCrypt.Net;

public class EncryptionService : IEncryptionService
{
    public Task<EncryptModel> EncryptData(EncryptModel data)
    {
        string hash = BCrypt.Net.BCrypt.HashPassword(data.Data);
        data.EncryptedString = hash;
        return Task.FromResult(data);
    }

    public Task<bool> VerifyPassword(string plainText, string hashedPassword)
    {
        bool isValid = BCrypt.Net.BCrypt.Verify(plainText, hashedPassword);
        return Task.FromResult(isValid);
    }
}
