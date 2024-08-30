using Olimpijada.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olimpijada.Managers
{
    public class MatchManager
    {
        public ObservableCollection<Country> Countries { get; set; }

        public MatchManager(ObservableCollection<Country> countries) 
        {
            this.Countries = countries;
        }

        public int[] SimulateMatch(Country team1, Country team2)
        {
            int[] result = new int[2];

            int teamForm1 = CalculateFormFactor(team1, team2);
            int teamForm2 = CalculateFormFactor(team2, team1);

            Random r = new Random();
            int score1 = (int)(r.Next(70, 100) + teamForm1);
            int score2 = (int)(r.Next(70, 100) + teamForm2);
            if(score1 == score2)
            {
                score2--;
            }
            result[0] = score1;
            result[1] = score2;
            return result;
        }

        private int CalculateFormFactor(Country team, Country opponent)
        {
            double winFormWeight = 0.4; 
            double scoreDifferenceWeight = 0.15;
            double rankingDifferenceWeight = 0.4;

            int winForm = team.TotalWins - team.TotalDefeats;

            int totalScoreDifference = team.TotalScoredPoints - team.TotalConcededPoints;

            int teamRankingDifference = opponent.FIBARanking - team.FIBARanking;

            int total = (int)(winForm * winFormWeight + totalScoreDifference * scoreDifferenceWeight + teamRankingDifference * rankingDifferenceWeight);
            Console.WriteLine(total + "\n" + "Score dif: " + teamRankingDifference + "\nTotal wins: " + team.TotalWins + "\nTotal defeats : " + team.TotalDefeats);
            return total;
        }
    }
}
