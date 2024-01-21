﻿using System.ComponentModel.DataAnnotations;

namespace MeatSteak.Areas.Admin.ViewModels
{
    public class PositionCreateVM
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3, ErrorMessage = "Name can contain minimum 3 characters")]
        [MaxLength(25, ErrorMessage = "Name can contain maximum 25 characters")]
        public string Name { get; set; }
    }
}
