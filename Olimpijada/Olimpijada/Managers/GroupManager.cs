using Olimpijada.Models;
using Olimpijada.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;

namespace Olimpijada.Managers
{
    public class GroupManager
    {
        private Dictionary<string, ObservableCollection<Country>> Groups;
        private Dictionary<string, ObservableCollection<Match>> ExibitionMatches;
        private ObservableCollection<Country> Countries = new ObservableCollection<Country>();
        string groupsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "groups.json");
        string exibitionsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "exibitions.json");
        private MatchManager matchManager;

        private ObservableCollection<Country> finalRanking = new ObservableCollection<Country>();

        private string ResultOfFirstRoundOfGroupStage;
        private string ResultOfSecondRoundOfGroupStage;
        private string ResultOfThirdRoundOfGroupStage;

        private string AllResultsInTheGroup;

        private string GroupStageStandings;

        private static bool firstFinished = false;
        private static bool secondFinished = false;
        private static bool thirdFinished = false;

        public GroupManager()
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new CustomDateTimeConverter() }
            };
            string groupsJson = File.ReadAllText(groupsFilePath);
            Groups = JsonSerializer.Deserialize<Dictionary<string, ObservableCollection<Country>>>(groupsJson);
            string exibitionsJson = File.ReadAllText(exibitionsFilePath);
            ExibitionMatches = JsonSerializer.Deserialize<Dictionary<string, ObservableCollection<Match>>>(exibitionsJson, options);
            LoadAllCountries();
            LoadExibitionMatches();
            matchManager = new MatchManager(Countries);
        }

        private void LoadAllCountries()
        {
            foreach (var group in Groups)
            {
                foreach (var country in group.Value)
                {
                    country.Group = group.Key;
                    Countries.Add(country);
                }
            }
        }

        public ObservableCollection<Country> GetAllCountries()
        {
            return Countries;
        }

        private void LoadExibitionMatches()
        {
            foreach (var country in Countries)
            {
                if (ExibitionMatches.ContainsKey(country.ISOCode))
                {
                    foreach (var match in ExibitionMatches[country.ISOCode])
                    {
                        country.ExibitionMatches.Add(match);
                    }
                    ProcessExibitionsMatchesForCountry(country);
                }
            }
        }

        private void ProcessExibitionsMatchesForCountry(Country country)
        {
            foreach(Match match in country.ExibitionMatches)
            {
                int[] scores = match.Result.Split('-').Select(int.Parse).ToArray();
                if (scores[0] > scores[1])
                {
                    country.TotalWins++;
                }
                else
                {
                    country.TotalDefeats++;
                }
                country.TotalScoredPoints += scores[0];
                country.TotalConcededPoints += scores[1];
            }
        }

        public void PrintAllGroups()
        {
            Console.WriteLine("**************************************************************\n");
            foreach (var group in Groups)
            {
                Console.WriteLine($"Group {group.Key}:");
                Console.WriteLine(new string('-', 20));

                foreach (var team in group.Value)
                {
                    Console.WriteLine($"Team Name  : {team.Team}");
                    Console.WriteLine($"ISO Code   : {team.ISOCode}");
                    Console.WriteLine($"FIBA Rank  : {team.FIBARanking}");
                    Console.WriteLine(new string('-', 20));
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n**************************************************************");
        }

        public void SimulateTheFirstRoundOfGroupMatches()
        {
            ResultOfFirstRoundOfGroupStage = "\n**************************************************************\n\n";
            if (!firstFinished)
            {
                ResultOfFirstRoundOfGroupStage += "Grupna faza - I kolo:\n";
                Country[] Group = new Country[50];
                int[] result = new int[2];
                int counter = 0;
                foreach (var group in Groups)
                {
                    ResultOfFirstRoundOfGroupStage += "\t Grupa " + group.Key + "\n";
                    foreach (Country team in group.Value)
                    {

                        Group[counter] = team;
                        counter++;
                        if (counter % 2 == 0)
                        {
                            int firstIndex = counter - 2;
                            int secondIndex = counter - 1;

                            result = matchManager.SimulateMatch(Group[firstIndex], Group[secondIndex]);
                            Match matchForFirstTeam = new Match(DateTime.Now, Group[secondIndex].ISOCode, result[0] + ":" + result[1]);
                            Match matchForSecondTeam = new Match(DateTime.Now, Group[firstIndex].ISOCode, result[1] + ":" + result[0]);

                            ResultOfFirstRoundOfGroupStage += "\t\t" + Group[firstIndex].Team + " - " + Group[secondIndex].Team + " (" + result[0] + ":" + result[1] + ")\n";
                            SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);
                        }
                    }
                }
                firstFinished = true;
                AllResultsInTheGroup += ResultOfFirstRoundOfGroupStage;
            }
            else
            {
                ResultOfFirstRoundOfGroupStage += "\nPrvo kolo grupne faze je već odigrano!\n\n";
            }
            ResultOfFirstRoundOfGroupStage += "\n**************************************************************\n\n";
            Console.Write(ResultOfFirstRoundOfGroupStage);
        }

        public void SimulateTheSecondRoundOfGroupMatches()
        {
            ResultOfSecondRoundOfGroupStage = "\n**************************************************************\n\n";
            if (!secondFinished)
            {
                if (firstFinished)
                {
                    ResultOfSecondRoundOfGroupStage += "Grupna faza - II kolo:\n";
                    Country[] Group = new Country[50];
                    int[] result = new int[2];
                    int CountryIndex = 0;
                    foreach (var group in Groups)
                    {
                        int counter = 0;
                        ResultOfSecondRoundOfGroupStage += "\t Grupa " + group.Key + "\n";
                        foreach (Country team in group.Value)
                        {
                            Group[counter] = team;
                            counter++;
                            CountryIndex++;

                            if (counter == 4)
                            {
                                int firstIndex = CountryIndex - 4;
                                int secondIndex = CountryIndex - 2;

                                result = matchManager.SimulateMatch(Group[0], Group[2]);
                                Match matchForFirstTeam = new Match(DateTime.Now, Group[2].ISOCode, result[0] + ":" + result[1]);
                                Match matchForSecondTeam = new Match(DateTime.Now, Group[0].ISOCode, result[1] + ":" + result[0]);

                                ResultOfSecondRoundOfGroupStage += "\t\t" + Group[0].Team + " - " + Group[2].Team + " (" + result[0] + ":" + result[1] + ")\n";
                                SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);

                                firstIndex++; // sada su 2 i 4 ekipa
                                secondIndex++;

                                result = matchManager.SimulateMatch(Group[1], Group[3]);
                                matchForFirstTeam = new Match(DateTime.Now, Group[3].ISOCode, result[0] + ":" + result[1]);
                                matchForSecondTeam = new Match(DateTime.Now, Group[1].ISOCode, result[1] + ":" + result[0]);

                                ResultOfSecondRoundOfGroupStage += "\t\t" + Group[1].Team + " - " + Group[3].Team + " (" + result[0] + ":" + result[1] + ")\n";
                                SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);
                            }
                        }
                    }
                    secondFinished = true;
                    AllResultsInTheGroup += ResultOfSecondRoundOfGroupStage;
                }
                else
                {
                    ResultOfSecondRoundOfGroupStage += "\nPrvo se mora odigrati prvo kolo grupne faze!\n\n";
                }
            }
            else
            {
                ResultOfSecondRoundOfGroupStage += "\nDrugo kolo grupne faze je već odigrano!\n\n";
            }
            ResultOfSecondRoundOfGroupStage += "\n**************************************************************\n\n";
            Console.Write(ResultOfSecondRoundOfGroupStage);
        }

        public void PrintAllMechesForUsa()
        {
            string ret = "";
            foreach(Country team in Countries)
            {
                if (team.ISOCode.Equals("SRB"))
                {
                    ret += "Primljeni: " + team.TotalConcededPoints + "\n";
                }
            }
            Console.Write(ret);
        }

        public bool SimulateTheThirdRoundOfGroupMatches()
        {
            ResultOfThirdRoundOfGroupStage = "\n**************************************************************\n\n";
            if (!thirdFinished)
            {
                if (firstFinished)
                {
                    if (secondFinished)
                    {
                        ResultOfThirdRoundOfGroupStage += "Grupna faza - III kolo:\n";
                        Country[] Group = new Country[50];
                        int[] result = new int[2];
                        int CountryIndex = 0;
                        foreach (var group in Groups)
                        {
                            int counter = 0;
                            ResultOfThirdRoundOfGroupStage += "\t Grupa " + group.Key + "\n";
                            foreach (Country team in group.Value)
                            {
                                Group[counter] = team;
                                counter++;
                                CountryIndex++;

                                if (counter == 4)
                                {
                                    int firstIndex = CountryIndex - 4;
                                    int secondIndex = CountryIndex - 1;

                                    result = matchManager.SimulateMatch(Group[0], Group[3]);
                                    Match matchForFirstTeam = new Match(DateTime.Now, Group[3].ISOCode, result[0] + ":" + result[1]);
                                    Match matchForSecondTeam = new Match(DateTime.Now, Group[0].ISOCode, result[1] + ":" + result[0]);

                                    ResultOfThirdRoundOfGroupStage += "\t\t" + Group[0].Team + " - " + Group[3].Team + " (" + result[0] + ":" + result[1] + ")\n";
                                    SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);

                                    firstIndex++; // sada su 2 i 3 ekipa
                                    secondIndex--;

                                    result = matchManager.SimulateMatch(Group[1], Group[2]);
                                    matchForFirstTeam = new Match(DateTime.Now, Group[2].ISOCode, result[0] + ":" + result[1]);
                                    matchForSecondTeam = new Match(DateTime.Now, Group[1].ISOCode, result[1] + ":" + result[0]);

                                    ResultOfThirdRoundOfGroupStage += "\t\t" + Group[1].Team + " - " + Group[2].Team + " (" + result[0] + ":" + result[1] + ")\n";
                                    SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);

                                }
                            }
                        }
                        thirdFinished = true;
                        AllResultsInTheGroup += ResultOfThirdRoundOfGroupStage;
                        finalRanking = RankTopTeamsAcrossGroups();
                    }
                    else
                    {
                        ResultOfThirdRoundOfGroupStage += "\nPrvo se mora odigrati drugo kolo grupne faze!\n\n";
                    }
                }
                else
                {
                    ResultOfThirdRoundOfGroupStage += "\nPrvo se mora odigrati prvo kolo grupne faze!\n\n";
                }
            }
            else
            {
                ResultOfThirdRoundOfGroupStage += "\nTreće kolo grupne faze je već odigrano!\n\n";
            }
            ResultOfThirdRoundOfGroupStage += "\n**************************************************************\n\n";
            Console.Write(ResultOfThirdRoundOfGroupStage);
            return thirdFinished;
        }

        public void SaveTheMatch(Match matchForFirstTeam, Match matchForSecondTeam, int firstIndex, int secondIndex, int result1, int result2)
        {
            Countries[firstIndex].Matches.Add(matchForFirstTeam);
            Countries[secondIndex].Matches.Add(matchForSecondTeam);

            if (result1 > result2) {
                Countries[firstIndex].Points += 2;
                Countries[secondIndex].Points += 1;

                Countries[firstIndex].Wins ++;
                Countries[secondIndex].Defeats ++;
            }
            else
            {
                Countries[firstIndex].Points += 1;
                Countries[secondIndex].Points += 2;

                Countries[secondIndex].Wins++;
                Countries[firstIndex].Defeats++;
            }

            Countries[firstIndex].ScoredPoints += result1;
            Countries[secondIndex].ScoredPoints += result2;

            Countries[firstIndex].ConcededPoints += result2;
            Countries[secondIndex].ConcededPoints += result1;
        }

        public void PrintCurrentGroupState()
        {
            string result = "**************************************************************\n\n";
            result += "Konačan plasman u grupama:\n";

            ObservableCollection<Country> ranked = new ObservableCollection<Country>();
            ObservableCollection<Country> temp = new ObservableCollection<Country>();

            foreach (var group in Groups)
            {
                result += $"\nGrupa {group.Key} (Ime - pobede/porazi/bodovi/postignuti koševi/primljeni koševi/koš razlika)::\n";

                foreach (var team in group.Value)
                {
                    temp.Add(team);
                }
                ranked = RankTeamsInGroups(temp);
                int counter = 1;
                foreach (var team in ranked)
                {
                    string pointDifference = (team.ScoredPoints - team.ConcededPoints) >= 0
                                             ? $"+{team.ScoredPoints - team.ConcededPoints}"
                                             : $"{team.ScoredPoints - team.ConcededPoints}";

                    if (team.Team.Equals("Sjedinjene Države")) // samo zbog lepseg izgleda
                    {
                        result += $"\t{counter}. {team.Team,-12}\t {team.Wins} / {team.Defeats} / {team.Points} / {team.ScoredPoints} / {team.ConcededPoints} / {pointDifference}\n";
                        counter++;
                    }
                    else
                    {
                        result += $"\t{counter}. {team.Team,-12}\t\t {team.Wins} / {team.Defeats} / {team.Points} / {team.ScoredPoints} / {team.ConcededPoints} / {pointDifference}\n";
                        counter++;
                    }
                }

                temp.Clear();
            }
            result += "\n**************************************************************\n";

            Console.WriteLine(result);

        }

        public ObservableCollection<Country> RankTeamsInGroups(ObservableCollection<Country> teams)
        {

            var rankedTeams = teams.OrderByDescending(t => t.Points).ToList();

            for (int i = 0; i < rankedTeams.Count - 1; i++)
            {
                var currentTeam = rankedTeams[i];
                var nextTeam = rankedTeams[i + 1];

                if (currentTeam.Points == nextTeam.Points)
                {
                    var match = currentTeam.Matches.FirstOrDefault(m => m.Opponent == nextTeam.ISOCode);
                    if (match != null)
                    {
                        int[] scores = match.Result.Split(':').Select(int.Parse).ToArray();
                        int pointDifference = scores[0] - scores[1];

                        if (pointDifference > 0)
                        {
                            continue;
                        }
                        else
                        {
                            rankedTeams[i] = nextTeam;
                            rankedTeams[i + 1] = currentTeam;
                        }
                    }
                }
            }

            var circleGroups = rankedTeams
                .GroupBy(t => t.Points)
                .Where(g => g.Count() > 2) // Grupa se formira ako ima više od dva tima sa istim brojem bodova
                .Select(g => g.ToList())
                .ToList();


            foreach (var group in circleGroups)
            {
                var circleRanked = group.OrderByDescending(t => CalculatePointDifferenceInCircle(group, t)).ToList();

                int insertIndex = rankedTeams.FindIndex(t => group.Contains(t));

                if (insertIndex != -1)
                {
                    foreach (var team in group)
                    {
                        rankedTeams.Remove(team);
                    }

                    rankedTeams.InsertRange(insertIndex, circleRanked);
                }
            }

            return new ObservableCollection<Country>(rankedTeams);
        }


        private int CalculatePointDifferenceInCircle(List<Country> circleTeams, Country team)
        {
            int pointDifference = 0;
            foreach (var match in team.Matches)
            {
                var opponent = circleTeams.FirstOrDefault(t => t.ISOCode == match.Opponent);
                if (opponent != null)
                {
                    var scores = match.Result.Split(':').Select(int.Parse).ToArray();
                    pointDifference += scores[0] - scores[1];
                }
            }
            return pointDifference;
        }

        public void PrintAllResultsInTheGroup()
        {
            if (firstFinished && secondFinished && thirdFinished)
            {
                AllResultsInTheGroup += "\n**************************************************************\n\n";
                Console.WriteLine(AllResultsInTheGroup);
            }
            else
            {
                string ret = "\n**************************************************************\n\n\n";
                ret += "Nisu odigrani svi mečevi u grupi!";
                ret += "\n\n\n**************************************************************\n";
                Console.WriteLine(ret);
            }
        }

        public ObservableCollection<Country> RankTopTeamsAcrossGroups()
        {

            ObservableCollection<Country> firstPlaceTeams = new ObservableCollection<Country>();
            ObservableCollection<Country> secondPlaceTeams = new ObservableCollection<Country>();
            ObservableCollection<Country> thirdPlaceTeams = new ObservableCollection<Country>();
            ObservableCollection<Country> fourthPlaceTeams = new ObservableCollection<Country>();

            foreach (var group in Groups)
            {
                ObservableCollection<Country> rankedTeams = RankTeamsInGroups(group.Value);
                firstPlaceTeams.Add(rankedTeams[0]);
                secondPlaceTeams.Add(rankedTeams[1]);
                thirdPlaceTeams.Add(rankedTeams[2]);
                fourthPlaceTeams.Add(rankedTeams[3]);
            }

            ObservableCollection<Country> rankedFirstPlaceTeams = RankTeamsForFinalGroupStageStanding(firstPlaceTeams);
            AddTeamsToFinalRanking(finalRanking, rankedFirstPlaceTeams);

            ObservableCollection<Country> rankedSecondPlaceTeams = RankTeamsForFinalGroupStageStanding(secondPlaceTeams);
            AddTeamsToFinalRanking(finalRanking, rankedSecondPlaceTeams);

            ObservableCollection<Country> rankedThirdPlaceTeams = RankTeamsForFinalGroupStageStanding(thirdPlaceTeams);
            AddTeamsToFinalRanking(finalRanking, rankedThirdPlaceTeams);

            ObservableCollection<Country> rankedFourthPlaceTeams = RankTeamsForFinalGroupStageStanding(fourthPlaceTeams);
            AddTeamsToFinalRanking(finalRanking, rankedFourthPlaceTeams);


            return finalRanking;
        }

        public ObservableCollection<Country> GetFinalRanking()
        {
            return finalRanking;
        }

        private ObservableCollection<Country> RankTeamsForFinalGroupStageStanding(ObservableCollection<Country> teams)
        {
            return new ObservableCollection<Country>(
                teams.OrderByDescending(t => t.Points)
                     .ThenByDescending(t => t.ScoredPoints - t.ConcededPoints)
                     .ThenByDescending(t => t.ScoredPoints));
        }

        private void AddTeamsToFinalRanking(ObservableCollection<Country> finalRanking, ObservableCollection<Country> rankedTeams)
        {
            for (int i = 0; i < rankedTeams.Count; i++)
            {
                finalRanking.Add(rankedTeams[i]);
            }
        }

        public void PrintFinalRankingInTheGroups()
        {
            GroupStageStandings = "**************************************************************\n\n";
            if (firstFinished && secondFinished && thirdFinished)
            {
                GroupStageStandings += "Konačni plasman timova nakon grupne faze:\n";

                int position = 1;
                foreach (Country team in finalRanking)
                {
                    string pointDifference = (team.ScoredPoints - team.ConcededPoints) >= 0
                                                    ? $"+{team.ScoredPoints - team.ConcededPoints}"
                                                    : $"{team.ScoredPoints - team.ConcededPoints}";

                    if (team.Team.Equals("Sjedinjene Države") || position >= 10) // samo zbog lepseg izgleda
                    {
                        GroupStageStandings += $"\t{position}. {team.Team,-12}\t {team.Wins} / {team.Defeats} / {team.Points} / {team.ScoredPoints} / {team.ConcededPoints} / {pointDifference}\n";
                        position++;
                    }
                    else
                    {
                        GroupStageStandings += $"\t{position}. {team.Team,-12}\t\t {team.Wins} / {team.Defeats} / {team.Points} / {team.ScoredPoints} / {team.ConcededPoints} / {pointDifference}\n";
                        position++;
                    }
                    if (position == 9)
                    {
                        GroupStageStandings += "----------------------------------------------------------------------\n";
                    }
                }
            }
            else
            {
                GroupStageStandings += "\nPrvo se moraju odigrati svi mečevi u grupama da bi se timovi rangirali za prolazak u narednu fazu takmičenja!\n";
            }

            GroupStageStandings += "\n\n**************************************************************\n\n";
            Console.WriteLine(GroupStageStandings);
        }
    }
}
