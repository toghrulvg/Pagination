using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.ViewModels.EmployeeVM
{
    public class EmployeeVM 
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }     
        public string Position { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
