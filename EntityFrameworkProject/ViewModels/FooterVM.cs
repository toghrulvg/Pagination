using EntityFrameworkProject.Models;
using System.Collections.Generic;

namespace EntityFrameworkProject.ViewModels
{
    public class FooterVM
    {
        public string Email { get; set; }
        public IEnumerable<Social> Socials { get; set; }
    }
}
