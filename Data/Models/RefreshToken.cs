using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VirtualMenuAPI.Data.Models.Users;

namespace VirtualMenuAPI.Data.Models
{
  public class RefreshToken
  {
    private const string _errorMessage = "Out Of Range";
    [Key]
    public int Id { get; set; }
    [StringLength(600, MinimumLength = 5, ErrorMessage = _errorMessage)]
    public string Token { get; set; } = string.Empty;
    public bool IsRevoked { get; set; } = false;
    public DateTime DateAdded { get; set; }
    public DateTime DateExpire { get; set; }

    [StringLength(450, MinimumLength = 1, ErrorMessage = _errorMessage)]
    public string UserId { get; set; } = string.Empty;
    [ForeignKey(nameof(UserId))]
    public Account User { get; set; } = new();
  }
}
