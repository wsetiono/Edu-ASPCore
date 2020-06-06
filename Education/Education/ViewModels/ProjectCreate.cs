using Education.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Education.ViewModels
{
    public class ProjectCreate : BaseEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Price { get; set; }
        public string Picture { get; set; }

        [Required(ErrorMessage = "Please choose image")]
        [Display(Name = "Picture")]
        public IFormFile Image { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}
