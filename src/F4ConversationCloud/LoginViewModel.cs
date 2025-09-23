public class LoginViewModel
{
    public string Email { get; set; }
    public string PassWord { get; set; }
    public int UserId { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public ClientFormStage Stage { get; set; }
    public string Password { get; set; }
}
