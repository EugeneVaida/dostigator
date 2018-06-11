using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dostigator.Models
{
    public class Tutorial
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [AllowHtml]
        public string Text { get; set; }

        [Required]
        [AllowHtml]
        public string PreviewText { get; set; }

        public string Date { get; set; }
        
    }
}