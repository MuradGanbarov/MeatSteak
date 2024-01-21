using System.ComponentModel.DataAnnotations;

namespace MeatSteak.Areas.Admin.ViewModels
{
    public class SettingUpdateVM
    {
        [Required(ErrorMessage = "Key is required")]
        [MinLength(3, ErrorMessage = "Key can contian minimum 3 characters")]
        [MaxLength(25, ErrorMessage = "Key can contain maximum 25 characters")]
        public string Key { get; set; }
        [Required(ErrorMessage = "Value is required")]
        [MinLength(3, ErrorMessage = "Value can contian minimum 3 characters")]
        [MaxLength(50, ErrorMessage = "Value can contain maximum 50 characters")]
        public string Value { get; set; }


    }
}
