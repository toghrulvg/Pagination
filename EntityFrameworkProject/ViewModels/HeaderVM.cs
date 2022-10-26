using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.ViewModels
{
    public class HeaderVM
    {
        public int Count { get; set; }
        public Dictionary<string,string> Settings { get; set; }
    }
}
