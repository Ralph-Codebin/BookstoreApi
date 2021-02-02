using System.ComponentModel.DataAnnotations;

namespace Application.Factories
{
    public class ValueObjectFactoryConfig
    {
        [Range(1, int.MaxValue)] public int CacheMinutes { get; set; }
    }
}
