using TeamColabApp.Models;
namespace TeamColabApp.Interfaces
{
    public interface IEncryptionService
    {
        public Task<EncryptModel> EncryptData(EncryptModel data);
        public Task<bool> VerifyPassword(string plainText, string hashedPassword);

    }
}