using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace kany.Models
{
    public class MovieModel
    {
        public int MovieID { get; set; }
        public string MovieName { get; set; }

        [FileExtensions(Extensions = "jpg,jpeg,png")]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Student Image")]
        public string Image { get; set; }

        public DateTime DateOfRelease { get; set; }
        public string MPAA { get; set; }
        public int Duration { get; set; }
        public string Category { get; set; }
        public int Rating { get; set; }
        public int ActorID { get; set; }
        public int DirectorID { get; set; }
    }
}