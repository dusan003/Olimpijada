using Olimpijada.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olimpijada.Managers
{
    public class MatchManager
    {
        public int[] SimulateMatch(Country team1, Country team2)
        {
            int[] result = new int[2];

            Random r = new Random();
            int score1 = (int)(r.Next(70, 100) + (team2.FIBARanking - team1.FIBARanking)/2);
            int score2 = (int)(r.Next(70, 100) + (team1.FIBARanking - team2.FIBARanking)/2);
            if(score1 == score2)
            {
                score2--;
            }
            result[0] = score1;
            result[1] = score2;
            return (result);
        }
    }
}
