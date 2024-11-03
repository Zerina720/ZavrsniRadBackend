using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace IzboriiS.Data.Models
{
    [CollectionName("Roles")] // Naziv kolekcije u MongoDB-u
    public class AppRole : MongoIdentityRole<string>
    {
        public AppRole() : base()
        {
        }

        public AppRole(string roleName) : base(roleName)
        {
        }
    }
}
