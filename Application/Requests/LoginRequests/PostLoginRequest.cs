
using System.ComponentModel.DataAnnotations;

namespace Application.Requests.LoginRequests
{
    public class PostLoginRequest
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}
