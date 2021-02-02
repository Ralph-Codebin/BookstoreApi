namespace Domain.Model.Entities
{
    public class UpdateProductDataRequest
    {
        public int id { get; set; }
        public string imagePath { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string price { get; set; }
    }
}
