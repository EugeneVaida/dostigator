using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace Dostigator.Models
{
    public class Aim
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

        public string StartDate { get; set; }

        [Required]
        public string FinishDate { get; set; }


        [DisplayName("Image")]
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }

        [Required]
        public string Group { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public ICollection<TimeLine> TimeLines { get; set; }

        public Aim()
        {            
            TimeLines = new List<TimeLine>();
            ImagePath = "~/AppFiles/Images/default.png";
        }
    }
}