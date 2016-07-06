using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizza.Models
{
    public class API
    {
        public string url { set; get; }
        public string type { set; get; }
        public string parameters { set; get; }
        public string description { set; get; }
        public string returns { set; get; }
        public bool requireToken { set; get; }
    }
}