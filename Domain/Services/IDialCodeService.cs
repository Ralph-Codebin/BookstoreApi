using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IDialCodeService
    {
        Task<bool> IsValidDialCodeAsync(string dialCode);
    }
}
