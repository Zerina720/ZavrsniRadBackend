using IzboriiS.Data.Models;

namespace IzboriiS.DTO.Response
{
    public class LoginResponse
    {
        public User user { get; set; }

        public string poruka { get; set; }
        public IList<string> role { get; set; }
        public DateTime expires { get; set; }
        public string token { get; set; }
    }
}
