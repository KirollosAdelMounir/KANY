using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kany.Models
{
    public class SearchModel
    {
        public int ActorId { get; set; }
        public string ActorName { get; set; }
        public int DirectorId   { get; set; }
        public string DirectorName { get; set; }
        public int MovieId  { get; set; }
        public string MovieName { get; set; }
    }
}