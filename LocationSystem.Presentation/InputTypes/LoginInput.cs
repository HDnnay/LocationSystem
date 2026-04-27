namespace LocationSystem.Presentation.InputTypes
{
    public class LoginInput
    {
        public string UserName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
