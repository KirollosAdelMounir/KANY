﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kany.Models
{
    public class ActorsModel
    {
        public int ActorId { get; set; }
        public string ActorName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string City { get; set; }
        public string ContactNumber { get; set; }
        
    }
}