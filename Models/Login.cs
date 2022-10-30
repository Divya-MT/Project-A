namespace DMSystemMvc.Models
{
    public class Login
    {
        public string Name { get; set; } = null!;

        public string? RegistrationType { get; set; }
        public string Password { get; set; } = null!;
    }
}
