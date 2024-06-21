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

        public static Grade Get(int gradeId)
        {
            try { return _grades.Find(x => x.Id == gradeId); }
            catch { return null; }
        }

        public static Grade Get(string gradeDescripcion)
        {
            try { return _grades.Find(x => x.Description == gradeDescripcion); }
            catch { return null; }
        }

        public static IEnumerable<Grade> Get() => _grades;


        public static void Add(Grade grade)
        {
            _grades.Add(grade);
        }

        public static void Update(Grade grade)
        {
            bool exist = _grades.Exists(g => g.Id == grade.Id);
            if (exist)
            {
                int index = _grades.FindIndex(g => g.Id == grade.Id);
                _grades[index] = grade;
            }
        }

        public static void RemovePersistence()
        {
            _grades = new List<Grade>();
        }
    }
}
