using System;
using System.Collections.Generic;
using System.Linq;
using Education.Models;

namespace Education.ViewModels
{
    public class ProjectShow
    {
        public Project project { get; set; }
        public IEnumerable<Task> tasks { get; set; }
    }
}
