using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Olimpijada.Models
{
    public class Country
    {
        public string Team { get; set; }
        public string ISOCode { get; set; }
        public int FIBARanking { get; set; }
        public string Group { get; set; }
        public int Points { get; set; }
        public int Wins { get; set; }
        public int Defeats { get; set; }
        public int ScoredPoints { get; set; }
        public int ConcededPoints { get; set; }
        public int TotalWins { get; set; }
        public int TotalDefeats { get; set; }
        public int TotalScoredPoints { get; set; }
        public int TotalConcededPoints { get; set; }


        public ObservableCollection<Match> Matches { get; set; } = new ObservableCollection<Match>();
        public ObservableCollection<Match> ExibitionMatches { get; set; } = new ObservableCollection<Match>();
    }
}
