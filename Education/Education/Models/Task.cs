using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Models
{
    public class Task : BaseEntity
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public string Video { get; set; }

        //[Range(typeof(bool), "false", "true")] //Set Header to be required
        public bool Header { get; set; }

        public string Tag { get; set; }

        [Display(Name = "Project")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
