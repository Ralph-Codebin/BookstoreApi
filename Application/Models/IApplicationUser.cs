using System.Collections.Generic;

namespace Application.Models
{
    public interface IApplicationUser
    {
        string Username { get; }
        string DisplayName { get; }
        IReadOnlyCollection<string> Groups { get; }
    }
}
