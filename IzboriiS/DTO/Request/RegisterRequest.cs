namespace IzboriiS.DTO.Request
{
    public class RegisterRequest
    {
        public string Sediste { get; set; }
        public string UserName { get; set; }
        public string Naziv { get; set; }
        public string PIB { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public DateTime ? datumOsnivanja { get; set; }
    }
}
