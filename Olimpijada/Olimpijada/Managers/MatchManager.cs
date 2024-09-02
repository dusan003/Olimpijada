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

        public MatchManager() 
        {

        }

        public int[] SimulateMatch(Country team1, Country team2)
        {
            int[] result = new int[2];

            int teamForm1 = CalculateFormFactor(team1, team2);
            int teamForm2 = CalculateFormFactor(team2, team1);

            Random r = new Random();
            int score1 = (int)(r.Next(70, 90) + teamForm1);
            int score2 = (int)(r.Next(70, 90) + teamForm2);
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
            return total;
        }

        public string CalculateWinningChances(Country team1, Country team2)
        {
            int teamForm1 = CalculateFormFactor(team1, team2);
            int teamForm2 = CalculateFormFactor(team2, team1);
            int formDifference;
            double team1WinningChance;
            double team2WinningChance;

            if (teamForm1 < 0 && teamForm2 > 0)
            {
                formDifference = teamForm2 - teamForm1;
                team1WinningChance = 50 - formDifference;
                team2WinningChance = 50 + formDifference;
            }
            else if(teamForm1 > 0 && teamForm2 < 0)
            {
                formDifference = teamForm1 - teamForm2;
                team1WinningChance = 50 + formDifference;
                team2WinningChance = 50 - formDifference;
            }
            else if(teamForm1 > teamForm2)
            {
                formDifference = teamForm1 - teamForm2;
                team1WinningChance = 50 + formDifference;
                team2WinningChance = 50 - formDifference;
            }
            else
            {
                formDifference = teamForm2 - teamForm1;
                team1WinningChance = 50 - formDifference;
                team2WinningChance = 50 + formDifference;
            }

            return $"Šanse za pobedu: ({team1WinningChance}% - {team2WinningChance}%)";
        }

        public string PredictMatch(Country team1, Country team2)
        {
            int teamForm1 = CalculateFormFactor(team1, team2);
            int teamForm2 = CalculateFormFactor(team2, team1);

            int score1 = 80 + teamForm1;
            int score2 = 80 + teamForm2;
            if (score1 == score2)
            {
                score2--;
            }

            return $"Predvidjen rezultat: ({score1}:{score2})";
        }

    }
}
