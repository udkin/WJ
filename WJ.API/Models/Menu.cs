using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WJ.API.Models
{
    public class Menu
    {
        public string Name { set; get; }

        public string Title { set; get; }

        public string Icon { set; get; }

        public string Jump { set; get; }

        [Newtonsoft.Json.JsonIgnore]
        public List<Menu> list { set; get; }
    }
}