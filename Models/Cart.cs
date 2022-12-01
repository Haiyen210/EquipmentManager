namespace EquipmentManager.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int Quantity { get; set; }
        public double? Price { get; set; }
        public string? userName { get; set; }
      
    }
}
