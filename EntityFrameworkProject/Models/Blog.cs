using System;

namespace EntityFrameworkProject.Models
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; }
    }
}
