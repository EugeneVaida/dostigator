using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dostigator.Models
{
    public class Article
    {
        public string Name { get; set; }
        public string Preview { get; set; }
        public string Text { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

    }
}