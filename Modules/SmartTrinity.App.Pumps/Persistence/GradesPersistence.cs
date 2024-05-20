using SmartTrinity.App.Pumps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Pumps.Persistence
{
    public static class GradesPersistence
    {
        private static List<Grade> _grades = new List<Grade>();

        public static Grade GetGrade(int gradeId)
        {
            try { return _grades.Find(x => x.Id == gradeId); }
            catch { return null; }
        }

        public static Grade GetGrade(string gradeDescripcion)
        {
            try { return _grades.Find(x => x.Description == gradeDescripcion); }
            catch { return null; }
        }

        public static IEnumerable<Grade> GetGrades() => _grades;


        public static void AddGrade(Grade grade)
        {
            _grades.Add(grade);
        }

        public static void UpdateGrade(Grade grade)
        {
            bool exist = _grades.Exists(g => g.Id == grade.Id);
            if (exist)
            {
                int index = _grades.FindIndex(g => g.Id == grade.Id);
                _grades[index] = grade;
            }
        }

        public static double GetSalePrice(int priceLevel, Grade grade)
        {
            //TODO: Use price level 1 by default when others price levels are 0.
            double salePrice = 0.0;
            bool exist = grade.Prices.Exists(p => p.PriceLevel == priceLevel);

            if (exist)
                salePrice = grade.Prices.Find(p => p.PriceLevel == priceLevel).Price;

            if (salePrice == 0)
                salePrice = grade.Prices[0].Price;

            return salePrice;
        }

        public static void RemovePersistence()
        {
            _grades = new List<Grade>();
        }
    }
}
