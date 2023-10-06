namespace VirtualMenuAPI.Data.Dtos
{
  public class AuthResultDto
  {
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
  }
}
