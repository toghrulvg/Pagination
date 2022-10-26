using EntityFrameworkProject.Models;
using System.Collections.Generic;

namespace EntityFrameworkProject.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public SliderDetail SliderDetail { get; set; }
    }
}
