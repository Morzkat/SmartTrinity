using System;
using System.Collections.Generic;

namespace SmartTrinity.App.Pumps.Core.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public Grade()
        {
            Id = 0;
            Description = "";
        }
    }

    public class GradePrice
    {
        public double Price { get; set; }
        public int PriceLevel { get; set; }
    }
}