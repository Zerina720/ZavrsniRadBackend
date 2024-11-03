using AutoMapper;
using IzboriiS.Data.Models;
using IzboriiS.DTO.Request;
using IzboriiS.DTO.Response;
using IzboriiS.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MongoDbGenericRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IzboriiS.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<AppRole> _rolesCollection;
        private readonly IMongoCollection<User> _usersCollection;

        public AuthService(UserManager<User> userManager, RoleManager<AppRole> roleManager, IMapper mapper, IMongoDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _rolesCollection = dbContext.GetCollection<AppRole>("AppRoles");
            _usersCollection = dbContext.GetCollection<User>("Users");
        }

        public async Task<LoginResponse> login(LoginRequest loginRequest)
        {
            var user = await _usersCollection.Find(u => u.UserName == loginRequest.Username).FirstOrDefaultAsync();
            if (user == null)
                return new LoginResponse { user = null, poruka = "Ne postoji korisnik sa datom skraćenicom!" };

            if (!user.EmailConfirmed)
                return new LoginResponse { user = null, poruka = "Vaša registracija još uvek nije potvrđena!" };

            if (user.Blokiran)
                return new LoginResponse { user = null, poruka = "Blokirani ste od strane admina!" };

        
            var passwordHasher = new PasswordHasher<User>();
            var passwordCheckResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password);

            if (passwordCheckResult == PasswordVerificationResult.Success)
            {
                var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("78fUjkyzfLz56gTk"));
                var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, loginRequest.Username)
        };

                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256)
                );
                var toReturn = new JwtSecurityTokenHandler().WriteToken(token);

                // Vraćanje korisničkih uloga direktno iz MongoDB-a
                return new LoginResponse
                {
                    user = user,
                    poruka = "Uspešna prijava!",
                    expires = DateTime.Now.AddHours(1),
                    token = toReturn,
                    role = user.Roles // Koristi polje Roles iz korisničkog objekta
                };
            }
            else
            {
                return new LoginResponse { user = null, poruka = "Netačna lozinka!" };
            }
        }

        public async Task<RegisterResponse> Create(RegisterRequest request)
        {
            var userExist = await _usersCollection.Find(u => u.UserName == request.UserName).FirstOrDefaultAsync();
            if (userExist != null)
                return new RegisterResponse { Success = false, Message = "Korisnik sa tom skraćenicom već postoji!" };

            var user = _mapper.Map<User>(request);

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

            await _usersCollection.InsertOneAsync(user);
            if (request.Role == 1)
            {
               
                var roleExist = await _rolesCollection.Find(r => r.Name == "Admin").FirstOrDefaultAsync();
                if (roleExist == null)
                {
                    var newRole = new AppRole { Name = "Admin" };
                    await _rolesCollection.InsertOneAsync(newRole);
                }

                user.Roles = new List<string> { "Admin" };
            }
            else if (request.Role == 2)
            {
                var roleExist = await _rolesCollection.Find(r => r.Name == "Stranka").FirstOrDefaultAsync();
                if (roleExist == null)
                {
                    var newRole = new AppRole { Name = "Stranka" };
                    await _rolesCollection.InsertOneAsync(newRole);
                }

                user.Roles = new List<string> { "Stranka" };
            }
            else if (request.Role == 3)
            {
                var roleExist = await _rolesCollection.Find(r => r.Name == "GrupaGradjana").FirstOrDefaultAsync();
                if (roleExist == null)
                {
                    var newRole = new AppRole { Name = "GrupaGradjana" };
                    await _rolesCollection.InsertOneAsync(newRole);
                }

                user.Roles = new List<string> { "GrupaGradjana" };
            }

            await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);

            return new RegisterResponse { Success = true, Message = "Uspešna registracija!" };
        }




        public async Task<bool> DeleteUser(string id)
        {
            var user = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
                return false;

            var deleteResult = await _usersCollection.DeleteOneAsync(u => u.Id == id);

            return deleteResult.DeletedCount > 0;
        }

    }
}
