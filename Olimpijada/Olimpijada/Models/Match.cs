using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olimpijada.Models
{
    public class Match
    {
        public DateTime Date { get; set; }
        public string Opponent { get; set; }
        public string Result { get; set; }

        public Match(DateTime date, string opponent, string result)
        {
            Date = date;
            Opponent = opponent;
            Result = result;
        }
    }
}
