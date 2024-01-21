namespace MeatSteak.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Dribbble { get; set; }
        public int? PositionId { get; set; }
        public Position? Positions  { get; set; }
    }
}
