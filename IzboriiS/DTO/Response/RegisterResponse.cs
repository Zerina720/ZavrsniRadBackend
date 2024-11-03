namespace IzboriiS.DTO.Response
{
    public class RegisterResponse
    {
        public string UserName { get; set; }
        public string Sediste { get; set; }
        public string Naziv { get; set; }
        public string PIB { get; set; }
        public DateTime? datumOsnivanja { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } 
    }
}
