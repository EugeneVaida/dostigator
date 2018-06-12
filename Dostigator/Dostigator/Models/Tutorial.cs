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
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Текст")]
        public string Text { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Превью")]
        public string PreviewText { get; set; }

        [Display(Name = "Дата")]
        public string Date { get; set; }
        
    }
}