using IzboriiS.Data.Models;
using IzboriiS.DTO.Request;
using IzboriiS.DTO.Response;

namespace IzboriiS.IService
{
    public interface IAuthService
    {
        public Task<LoginResponse> login(LoginRequest loginRequest);
       // public Task<User> Create(RegisterRequest request);
       public Task<RegisterResponse>Create(RegisterRequest registerRequest);
        public Task<bool> DeleteUser(string Id);
    }
}
