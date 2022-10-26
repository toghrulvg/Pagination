using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Models
{
    public class Social : BaseEntity
    {
        [Required(ErrorMessage = "Name can't be empty")]
        [StringLength(20, ErrorMessage = "The size of text max 20")]
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
