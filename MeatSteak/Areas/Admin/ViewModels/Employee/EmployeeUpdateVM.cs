using MeatSteak.Models;
using System.ComponentModel.DataAnnotations;

namespace MeatSteak.Areas.Admin.ViewModels
{
    public class EmployeeUpdateVM
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3, ErrorMessage = "Name can contain minimum 3 characters")]
        [MaxLength(25, ErrorMessage = "Name can contain maximum 25 characters")]
        public string Name { get; set; }
        public string? ImageURL { get; set; }
        [Required(ErrorMessage = "Photo is required")]
        public IFormFile? Photo { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Dribbble { get; set; }
        public int? PositionId { get; set; }
        public List<Position>? Positions { get; set; }
    }
}
