using SmartTrinity.App.Pumps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Pumps.Persistence
{
    public static class HosesPersistence
    {
        private static List<Hose> _hoses = new List<Hose>();
        public static DateTime LastUpdate { get; set; }

        public static List<Hose> Get()
        {
            return _hoses;
        }

        public static Hose Get(int hoseId)
        {
            bool exist = _hoses.Exists(x => x.HoseId == hoseId);
            if (exist)
                return _hoses.Find(x => x.HoseId == hoseId);

            Add(hoseId);
            return Get(hoseId);
        }

        public static void Add(int hoseId)
        {
            bool exist = _hoses.Exists(x => x.HoseId == hoseId);
            if (!exist)
            {
                _hoses.Add(new Hose
                {
                    HoseId = hoseId,
                    TotalizerMoney = 0,
                    TotalizerVolume = 0,
                    Grades = new List<Grade>()
                });
            }
        }

        public static void Update(Hose Hose)
        {
            bool exist = _hoses.Exists(x => x.HoseId == Hose.HoseId);
            if (exist)
            {
                int index = _hoses.FindIndex(p => p.HoseId == Hose.HoseId);
                _hoses[index] = Hose;
            }
        }

        public static void RemovePersistence()
        {
            _hoses = new List<Hose>();
        }
    }
}
