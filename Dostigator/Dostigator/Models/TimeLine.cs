using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Dostigator.Models
{
    public class TimeLine
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
      
        [AllowHtml]
        public string Text { get; set; }    
        
        public string Position { get; set; }

        
        public string Date { get; set; }

        public int? AimId { get; set; }
        public Aim Aim { get; set; }
    }
}