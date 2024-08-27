using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olimpijada.Models
{
    public class Team
    {
        public string Name { get; set; }
        public string ISOCode { get; set; }
        public int FIBARanking { get; set; }
        public int Points {  get; set; }
        public int Wins { get; set; }
        public int ScoredPoints { get; set; }
        public int ConcededPoints { get; set; }

        public ObservableCollection<Match> Matches { get; set; } = new ObservableCollection<Match>();
    }
}
