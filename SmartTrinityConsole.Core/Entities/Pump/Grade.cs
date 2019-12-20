using System;
using System.Collections.Generic;

namespace SmartTrinityConsole.Core.Entities.Pump
{
    public class Grade
    {
        private string _rgb;

        public int Id { get; set; }
        public int Red { get; set; }
        public int Blue { get; set; }
        public int Green { get; set; }
        public string RGB { get { return _rgb; } set { SetRGBColor(value); } }
        public string Description { get; set; }
        public List<GradePrice> Prices { get; set; }

        public Grade()
        {
            Id = 0;
            Red = 0;
            Blue = 0;
            Green = 0;
            Description = "";
            Prices = new List<GradePrice>();
        }

        private void SetRGBColor(string rgb)
        {
            if (rgb == "")
                return;

            _rgb = rgb;
            Red = Convert.ToInt32(rgb.Substring(0, 2), fromBase: 16);
            Blue = Convert.ToInt32(rgb.Substring(4, 2), fromBase: 16);
            Green = Convert.ToInt32(rgb.Substring(2, 2), fromBase: 16);
        }

    }

    public class GradePrice
    {
        public double Price { get; set; }
        public int PriceLevel { get; set; }
    }
}