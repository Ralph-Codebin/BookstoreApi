namespace Domain.Model.ValueObjects.Abstractions
{
    public class LanguageResource<T>
    {
        public string LanguageCode { get; set; }
        public T Value { get; set; }
    }
}
