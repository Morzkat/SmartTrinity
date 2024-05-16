using SmartTrinity.App.Pumps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Pumps.Persistence
{
    public static class HosePersistence
    {
        private static List<Hose> _hoses = new List<Hose>();
        public static DateTime LastUpdate { get; set; }

        public static List<Hose> GetHoses()
        {
            return _hoses;
        }

        public static Hose GetHose(int hoseId)
        {
            bool exist = _hoses.Exists(x => x.HoseId == hoseId);
            if (exist)
                return _hoses.Find(x => x.HoseId == hoseId);

            AddHose(hoseId);
            return GetHose(hoseId);
        }

        public static void AddHose(int hoseId)
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

        public static void UpdateHose(Hose Hose)
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
