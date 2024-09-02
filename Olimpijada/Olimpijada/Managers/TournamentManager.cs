using Olimpijada.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Match = Olimpijada.Models.Match;

namespace Olimpijada.Managers
{
    public class TournamentManager
    {

        private MatchManager matchManager =  new MatchManager();

        private ObservableCollection<Country> finalRanking = new ObservableCollection<Country>();
        private ObservableCollection<Country> Countries = new ObservableCollection<Country>();

        ObservableCollection<Country> PotD = new ObservableCollection<Country>();
        ObservableCollection<Country> PotE = new ObservableCollection<Country>();
        ObservableCollection<Country> PotF = new ObservableCollection<Country>();
        ObservableCollection<Country> PotG = new ObservableCollection<Country>();

        ObservableCollection<Country> QuarterFinalPair1 = new ObservableCollection<Country>();
        ObservableCollection<Country> QuarterFinalPair2 = new ObservableCollection<Country>();
        ObservableCollection<Country> QuarterFinalPair3 = new ObservableCollection<Country>();
        ObservableCollection<Country> QuarterFinalPair4 = new ObservableCollection<Country>();

        ObservableCollection<Country> SemiFinalPair1 = new ObservableCollection<Country>();
        ObservableCollection<Country> SemiFinalPair2 = new ObservableCollection<Country>();

        ObservableCollection<Country> FinalPair = new ObservableCollection<Country>();
        ObservableCollection<Country> ThirdPlacePair = new ObservableCollection<Country>();


        ObservableCollection<Country> Medals = new ObservableCollection<Country>();

        Random r = new Random();

        private bool potsMade = false;
        private bool drawMade = false;
        private bool simulateQuarterFinalFinished = false;
        private bool simulateSemiFinalFinished = false;
        private bool simulateFinalFinished = false;

        private static bool matchPrediction = true;

        private string Pots;

        private string QuarterFinalDraw;
        private string SemiFinalDraw;
        private string FinalDraw;
        private string AllResultsInKnockoutStage = "";

        public TournamentManager(ObservableCollection<Country> countries)
        {
            Countries = countries;
        }

        private void MakeThePairsOfQuarterFinal()
        {
            if (!drawMade)
            {
                QuarterFinalPair1.Add(PotD[0]);
                QuarterFinalPair2.Add(PotD[1]);

                int indexPotG = r.Next(0, 2);

                if (PotD[0].Group != PotG[indexPotG].Group && PotD[1].Group != PotG[1- indexPotG].Group)
                {
                    QuarterFinalPair1.Add(PotG[indexPotG]);
                    QuarterFinalPair2.Add(PotG[1 - indexPotG]);
                }
                else
                {
                    int newIndex = 1 - indexPotG;
                    QuarterFinalPair1.Add(PotG[newIndex]);
                    QuarterFinalPair2.Add(PotG[1 - newIndex]);
                }

                QuarterFinalPair3.Add(PotE[0]);
                QuarterFinalPair4.Add(PotE[1]);

                int indexPotF = r.Next(0, 2);

                if (PotE[0].Group != PotF[indexPotF].Group && PotE[1].Group != PotF[1 - indexPotG].Group)
                {
                    QuarterFinalPair3.Add(PotF[indexPotF]);
                    QuarterFinalPair4.Add(PotF[1 - indexPotF]);
                }
                else
                {
                    int newIndex = 1 - indexPotF;
                    QuarterFinalPair3.Add(PotF[newIndex]);
                    QuarterFinalPair4.Add(PotF[1 - newIndex]);
                }
                drawMade = true;
            }
        }

        public void SimulateQuearterFinal(bool canSimulate)
        {
            QuarterFinalDraw = "\n**************************************************************\n\n";
            if (canSimulate)
            {
                if (!drawMade)
                {
                    MakeThePairsOfQuarterFinal();
                }

                if (!simulateQuarterFinalFinished)
                {
                    QuarterFinalDraw += "Četvrtfinale:\n\n";
                    int[] result = new int[2];


                    result = matchManager.SimulateMatch(QuarterFinalPair1[0], QuarterFinalPair1[1]);
                    Match matchForFirstTeam = new Match(DateTime.Now, QuarterFinalPair1[1].ISOCode, result[0] + ":" + result[1]);
                    Match matchForSecondTeam = new Match(DateTime.Now, QuarterFinalPair1[0].ISOCode, result[1] + ":" + result[0]);
                    if (!matchPrediction)
                    {
                        QuarterFinalDraw += "\t" + QuarterFinalPair1[0].Team + " - " + QuarterFinalPair1[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    }
                    else
                    {
                        QuarterFinalDraw += MakeTheWinningChancesForTheMatchAndPredictResult(QuarterFinalPair1[0], QuarterFinalPair1[1], result[0], result[1]) + "\n";
                    }
                    SaveTheMatch(matchForFirstTeam, matchForSecondTeam, QuarterFinalPair1[0], QuarterFinalPair1[1], result[0], result[1]);

                    if (result[0] > result[1])
                    {
                        SemiFinalPair1.Add(QuarterFinalPair1[0]);
                    }
                    else
                    {
                        SemiFinalPair1.Add(QuarterFinalPair1[1]);
                    }

                    result = matchManager.SimulateMatch(QuarterFinalPair2[0], QuarterFinalPair2[1]);
                    matchForFirstTeam = new Match(DateTime.Now, QuarterFinalPair2[1].ISOCode, result[0] + ":" + result[1]);
                    matchForSecondTeam = new Match(DateTime.Now, QuarterFinalPair2[0].ISOCode, result[1] + ":" + result[0]);

                    if (!matchPrediction)
                    {
                        QuarterFinalDraw += "\t" + QuarterFinalPair2[0].Team + " - " + QuarterFinalPair2[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    }
                    else
                    {
                        QuarterFinalDraw += MakeTheWinningChancesForTheMatchAndPredictResult(QuarterFinalPair2[0], QuarterFinalPair2[1], result[0], result[1]) + "\n\n";
                    }
                    SaveTheMatch(matchForFirstTeam, matchForSecondTeam, QuarterFinalPair2[0], QuarterFinalPair2[1], result[0], result[1]);

                    if (result[0] > result[1])
                    {
                        SemiFinalPair2.Add(QuarterFinalPair2[0]);
                    }
                    else
                    {
                        SemiFinalPair2.Add(QuarterFinalPair2[1]);
                    }

                    result = matchManager.SimulateMatch(QuarterFinalPair3[0], QuarterFinalPair3[1]);
                    matchForFirstTeam = new Match(DateTime.Now, QuarterFinalPair3[1].ISOCode, result[0] + ":" + result[1]);
                    matchForSecondTeam = new Match(DateTime.Now, QuarterFinalPair3[0].ISOCode, result[1] + ":" + result[0]);

                    if (!matchPrediction)
                    {
                        QuarterFinalDraw += "\t" + QuarterFinalPair3[0].Team + " - " + QuarterFinalPair3[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    }
                    else
                    {
                        QuarterFinalDraw += MakeTheWinningChancesForTheMatchAndPredictResult(QuarterFinalPair3[0], QuarterFinalPair3[1], result[0], result[1]) + "\n";
                    }
                    SaveTheMatch(matchForFirstTeam, matchForSecondTeam, QuarterFinalPair3[0], QuarterFinalPair3[1], result[0], result[1]);

                    if (result[0] > result[1])
                    {
                        SemiFinalPair1.Add(QuarterFinalPair3[0]);
                    }
                    else
                    {
                        SemiFinalPair1.Add(QuarterFinalPair3[1]);
                    }

                    result = matchManager.SimulateMatch(QuarterFinalPair4[0], QuarterFinalPair4[1]);
                    matchForFirstTeam = new Match(DateTime.Now, QuarterFinalPair4[1].ISOCode, result[0] + ":" + result[1]);
                    matchForSecondTeam = new Match(DateTime.Now, QuarterFinalPair4[0].ISOCode, result[1] + ":" + result[0]);

                    if (!matchPrediction)
                    {
                        QuarterFinalDraw += "\t" + QuarterFinalPair4[0].Team + " - " + QuarterFinalPair4[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    }
                    else
                    {
                        QuarterFinalDraw += MakeTheWinningChancesForTheMatchAndPredictResult(QuarterFinalPair4[0], QuarterFinalPair4[1], result[0], result[1]) + "\n";
                    }
                    SaveTheMatch(matchForFirstTeam, matchForSecondTeam, QuarterFinalPair4[0], QuarterFinalPair4[1], result[0], result[1]);

                    if (result[0] > result[1])
                    {
                        SemiFinalPair2.Add(QuarterFinalPair4[0]);
                    }
                    else
                    {
                        SemiFinalPair2.Add(QuarterFinalPair4[1]);
                    }

                    AllResultsInKnockoutStage += QuarterFinalDraw;

                    QuarterFinalDraw += "\nPolufinalni mečevi:\n\n";

                    if (!matchPrediction)
                    {
                        QuarterFinalDraw += "\t" + SemiFinalPair1[0].Team + " - " + SemiFinalPair1[1].Team;
                        QuarterFinalDraw += "\n\t" + SemiFinalPair2[0].Team + " - " + SemiFinalPair2[1].Team + "\n";
                    }
                    else
                    {
                        QuarterFinalDraw += MakeTheWinningChancesForTheUpcomingMatchAndPredictResult(SemiFinalPair1[0], SemiFinalPair1[1]);
                        QuarterFinalDraw += MakeTheWinningChancesForTheUpcomingMatchAndPredictResult(SemiFinalPair2[0], SemiFinalPair2[1]);
                    }

                    simulateQuarterFinalFinished = true;
                }
                else
                {
                    QuarterFinalDraw += "\nČetvrtfinalne utakmice su već odigrane!\n\n";
                }
            }
            else
            {
                QuarterFinalDraw += "\nPrvo se moraju odigrati svi mečevi u grupama da bi se formirali parovi četvrtfinala!\n\n";
            }
            QuarterFinalDraw += "\n**************************************************************\n\n";
            Console.Write(QuarterFinalDraw);

        }

        public void SimulateSemiFinal()
        {
            SemiFinalDraw = "\n**************************************************************\n\n";

            if (simulateQuarterFinalFinished)
            {
                if (!simulateSemiFinalFinished)
                {
                    SemiFinalDraw += "Polufinale:\n\n";
                    int[] result = new int[2];


                    result = matchManager.SimulateMatch(SemiFinalPair1[0], SemiFinalPair1[1]);
                    Match matchForFirstTeam = new Match(DateTime.Now, SemiFinalPair1[1].ISOCode, result[0] + ":" + result[1]);
                    Match matchForSecondTeam = new Match(DateTime.Now, SemiFinalPair1[0].ISOCode, result[1] + ":" + result[0]);

                    if (!matchPrediction)
                    {
                        SemiFinalDraw += "\t" + SemiFinalPair1[0].Team + " - " + SemiFinalPair1[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    }
                    else
                    {
                        SemiFinalDraw += MakeTheWinningChancesForTheMatchAndPredictResult(SemiFinalPair1[0], SemiFinalPair1[1], result[0], result[1]) + "\n";
                    }
                    SaveTheMatch(matchForFirstTeam, matchForSecondTeam, SemiFinalPair1[0], SemiFinalPair1[1], result[0], result[1]);

                    if (result[0] > result[1])
                    {
                        FinalPair.Add(SemiFinalPair1[0]);
                        ThirdPlacePair.Add(SemiFinalPair1[1]);
                    }
                    else
                    {
                        FinalPair.Add(SemiFinalPair1[1]);
                        ThirdPlacePair.Add(SemiFinalPair1[0]);
                    }

                    result = matchManager.SimulateMatch(SemiFinalPair2[0], SemiFinalPair2[1]);
                    matchForFirstTeam = new Match(DateTime.Now, SemiFinalPair2[1].ISOCode, result[0] + ":" + result[1]);
                    matchForSecondTeam = new Match(DateTime.Now, SemiFinalPair2[0].ISOCode, result[1] + ":" + result[0]);

                    if (!matchPrediction)
                    {
                        SemiFinalDraw += "\t" + SemiFinalPair2[0].Team + " - " + SemiFinalPair2[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    }
                    else
                    {
                        SemiFinalDraw += MakeTheWinningChancesForTheMatchAndPredictResult(SemiFinalPair2[0], SemiFinalPair2[1], result[0], result[1]) + "\n";
                    }
                    SaveTheMatch(matchForFirstTeam, matchForSecondTeam, SemiFinalPair2[0], SemiFinalPair2[0], result[0], result[1]);

                    if (result[0] > result[1])
                    {
                        FinalPair.Add(SemiFinalPair2[0]);
                        ThirdPlacePair.Add(SemiFinalPair2[1]);
                    }
                    else
                    {
                        FinalPair.Add(SemiFinalPair2[1]);
                        ThirdPlacePair.Add(SemiFinalPair2[0]);
                    }

                    AllResultsInKnockoutStage += SemiFinalDraw;

                    SemiFinalDraw += "\nFinale:\n\n";

                    if (!matchPrediction)
                    {
                        SemiFinalDraw += "\t" + FinalPair[0].Team + " - " + FinalPair[1].Team + "\n";
                    }
                    else
                    {
                        SemiFinalDraw += MakeTheWinningChancesForTheUpcomingMatchAndPredictResult(FinalPair[0], FinalPair[1]) + "\n";
                    }

                    SemiFinalDraw += "\nUtakmica za treće mesto:\n\n";

                    if (!matchPrediction)
                    {
                        SemiFinalDraw += "\t" + ThirdPlacePair[0].Team + " - " + ThirdPlacePair[1].Team + "\n";
                    }
                    else
                    {
                        SemiFinalDraw += MakeTheWinningChancesForTheUpcomingMatchAndPredictResult(ThirdPlacePair[0], ThirdPlacePair[1]) + "\n";
                    }

                    simulateSemiFinalFinished = true;
                }
                else
                {
                    SemiFinalDraw += "\nPolufinalne utakmice su već odigrane!\n\n";
                }
            }
            else
            {
                SemiFinalDraw += "\nPrvo se moraju odigrati svi mečevi u grupama i četvrtfinalni mečevi da bi se formirali parovi polufinala!\n\n";
            }
            SemiFinalDraw += "\n**************************************************************\n\n";
            Console.Write(SemiFinalDraw);
        }

        public void SimulateFinalMatchAndThirdPlaceMatch()
        {
            FinalDraw = "\n**************************************************************\n\n";

            if (simulateQuarterFinalFinished && simulateSemiFinalFinished)
            {
                if (!simulateFinalFinished)
                {
                    FinalDraw += "Finale:\n\n";
                    int[] result = new int[2];


                    result = matchManager.SimulateMatch(FinalPair[0], FinalPair[1]);
                    Match matchForFirstTeam = new Match(DateTime.Now, FinalPair[1].ISOCode, result[0] + ":" + result[1]);
                    Match matchForSecondTeam = new Match(DateTime.Now, FinalPair[0].ISOCode, result[1] + ":" + result[0]);

                    if (!matchPrediction)
                    {
                        FinalDraw += "\t" + FinalPair[0].Team + " - " + FinalPair[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    }
                    else
                    {
                        FinalDraw += MakeTheWinningChancesForTheMatchAndPredictResult(FinalPair[0], FinalPair[1], result[0], result[1]) + "\n";
                    }
                    SaveTheMatch(matchForFirstTeam, matchForSecondTeam, FinalPair[0], FinalPair[1], result[0], result[1]);

                    if (result[0] > result[1])
                    {
                        Medals.Add(FinalPair[0]);
                        Medals.Add(FinalPair[1]);
                    }
                    else
                    {
                        Medals.Add(FinalPair[1]);
                        Medals.Add(FinalPair[0]);
                    }

                    FinalDraw += "\nUtakmica za treće mesto:\n\n";

                    result = matchManager.SimulateMatch(ThirdPlacePair[0], ThirdPlacePair[1]);
                    matchForFirstTeam = new Match(DateTime.Now, ThirdPlacePair[1].ISOCode, result[0] + ":" + result[1]);
                    matchForSecondTeam = new Match(DateTime.Now, ThirdPlacePair[0].ISOCode, result[1] + ":" + result[0]);

                    if (!matchPrediction)
                    {
                        FinalDraw += "\t" + ThirdPlacePair[0].Team + " - " + ThirdPlacePair[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    }
                    else
                    {
                        FinalDraw += MakeTheWinningChancesForTheMatchAndPredictResult(ThirdPlacePair[0], ThirdPlacePair[1], result[0], result[1]) + "\n";
                    }
                    SaveTheMatch(matchForFirstTeam, matchForSecondTeam, ThirdPlacePair[0], ThirdPlacePair[1], result[0], result[1]);

                    if (result[0] > result[1])
                    {
                        Medals.Add(ThirdPlacePair[0]);
                    }
                    else
                    {
                        Medals.Add(ThirdPlacePair[1]);
                    }

                    FinalDraw += RankTopThreeTeamsForMedals();

                    AllResultsInKnockoutStage += FinalDraw;

                    simulateFinalFinished = true;
                }
                else
                {
                    FinalDraw += "\nFinalna utakmica i utakmica za treće mesto su već odigrane!\n\n";
                }
            }
            else
            {
                FinalDraw += "\nPrvo se moraju odigrati svi mečevi u grupama, četvrtfinalni mečevi,\nkao i polufinalni mečevi da bi se formirao finalni par i par za treće mesto!\n\n";
            }
            FinalDraw += "\n**************************************************************\n\n";
            Console.Write(FinalDraw);
        }

        public void SaveTheMatch(Match matchForFirstTeam, Match matchForSecondTeam, Country team1, Country team2, int result1, int result2)
        {
            int firstIndex = Countries.IndexOf(team1);
            int secondIndex = Countries.IndexOf(team2);

            Countries[firstIndex].Matches.Add(matchForFirstTeam);
            Countries[secondIndex].Matches.Add(matchForSecondTeam);

            if (result1 > result2)
            {
                Countries[firstIndex].Points += 2;
                Countries[secondIndex].Points += 1;

                Countries[firstIndex].Wins++;
                Countries[secondIndex].Defeats++;
                Countries[firstIndex].TotalWins++;
                Countries[secondIndex].TotalDefeats++;
            }
            else
            {
                Countries[firstIndex].Points += 1;
                Countries[secondIndex].Points += 2;

                Countries[secondIndex].Wins++;
                Countries[firstIndex].Defeats++;
                Countries[secondIndex].TotalWins++;
                Countries[firstIndex].TotalDefeats++;
            }

            Countries[firstIndex].ScoredPoints += result1;
            Countries[secondIndex].ScoredPoints += result2;
            Countries[firstIndex].TotalScoredPoints += result1;
            Countries[secondIndex].TotalScoredPoints += result2;

            Countries[firstIndex].ConcededPoints += result2;
            Countries[secondIndex].ConcededPoints += result1;
            Countries[firstIndex].TotalConcededPoints += result2;
            Countries[secondIndex].TotalConcededPoints += result1;
        }

        private string RankTopThreeTeamsForMedals()
        {
            return "\nMedalje:\n\n\t" + "1." + Medals[0].Team + "\n\t" + "2." + Medals[1].Team + "\n\t3." + Medals[2].Team + "\n";
        }

        public void PrintTopThreeTeams()
        {
            string result = "\n**************************************************************\n";
            if (simulateFinalFinished)
            {
                result += RankTopThreeTeamsForMedals();
            }
            else
            {
                result += "\n\nPrvo se moraju odigrati svi mečevi turnira da bi se rangirali timovi koji su osvojili medalje!\n\n";
            }
            result += "\n**************************************************************\n";
            
            Console.WriteLine(result);
        }

        public void PrintCurrentResultsOfKnockoutStage()
        {
            if(!simulateQuarterFinalFinished || !simulateSemiFinalFinished || !simulateFinalFinished)
            {
                string ret = "\n**************************************************************\n\n\n";
                ret += "Prvo se moraju odigrati svi mečevi na turniru\n\n";
                ret += "\n**************************************************************\n\n";
                Console.WriteLine(ret);
            }
            else
            {
                AllResultsInKnockoutStage += "\n**************************************************************\n";
                Console.WriteLine(AllResultsInKnockoutStage);
            }
        }

        public void MakeThePots(ObservableCollection<Country> final)
        {
            finalRanking = final;
            if (!potsMade)
            {
                PotD.Add(finalRanking[0]);
                PotD.Add(finalRanking[1]);

                PotE.Add(finalRanking[2]);
                PotE.Add(finalRanking[3]);

                PotF.Add(finalRanking[4]);
                PotF.Add(finalRanking[5]);

                PotG.Add(finalRanking[6]);
                PotG.Add(finalRanking[7]);
            }
        }

        public void PrintThePotsAndQuarterFinalMatches(bool can)
        {
            Pots = "\n**************************************************************\n\n";
            if (can)
            {
                if (!drawMade)
                {
                    MakeThePairsOfQuarterFinal();
                }

                Pots += "Šeširi:\n\tŠešir D\n\t\t";
                Pots += PotD[0].Team + "\n\t\t";
                Pots += PotD[1].Team + "\n\t";

                Pots += "Šešir E\n\t\t";
                Pots += PotE[0].Team + "\n\t\t";
                Pots += PotE[1].Team + "\n\t";

                Pots += "Šešir F\n\t\t";
                Pots += PotF[0].Team + "\n\t\t";
                Pots += PotF[1].Team + "\n\t";

                Pots += "Šešir G\n\t\t";
                Pots += PotG[0].Team + "\n\t\t";
                Pots += PotG[1].Team;

                Pots += "\n\nEliminaciona faza:\n";
                if (!matchPrediction)
                {
                    Pots += "\n\t" + QuarterFinalPair1[0].Team + " - " + QuarterFinalPair1[1].Team;
                    Pots += "\n\t" + QuarterFinalPair2[0].Team + " - " + QuarterFinalPair2[1].Team + "\n";

                    Pots += "\n\t" + QuarterFinalPair3[0].Team + " - " + QuarterFinalPair3[1].Team;
                    Pots += "\n\t" + QuarterFinalPair4[0].Team + " - " + QuarterFinalPair4[1].Team;
                }
                else
                {
                    Pots += "\n";
                    Pots += MakeTheWinningChancesForTheUpcomingMatchAndPredictResult(QuarterFinalPair1[0], QuarterFinalPair1[1]);

                    Pots += MakeTheWinningChancesForTheUpcomingMatchAndPredictResult(QuarterFinalPair2[0], QuarterFinalPair2[1]) + "\n";

                    Pots += MakeTheWinningChancesForTheUpcomingMatchAndPredictResult(QuarterFinalPair3[0], QuarterFinalPair3[1]);

                    Pots += MakeTheWinningChancesForTheUpcomingMatchAndPredictResult(QuarterFinalPair4[0], QuarterFinalPair4[1]);
                }

                potsMade = true;

            }
            else
            {
                Pots += "\nPrvo se moraju odigrati svi mečevi u grupama da bi se formirali šeširi za narednu fazu takmičenja!\n";
            }
            Pots += "\n\n**************************************************************\n\n";
            Console.WriteLine(Pots);
        }

        private string MakeTheWinningChancesForTheMatchAndPredictResult(Country team1, Country team2, int score1, int score2)
        {
            string winningChances = matchManager.CalculateWinningChances(team1, team2);
            string prediction = matchManager.PredictMatch(team1, team2);
            string formattedResult = $"{team1.Team} - {team2.Team} ({score1}:{score2})";
            formattedResult = formattedResult.PadRight(40);

            return "\t\t" + formattedResult + winningChances + "\t" + prediction;
        }

        private string MakeTheWinningChancesForTheUpcomingMatchAndPredictResult(Country team1, Country team2)
        {
            string winningChances = matchManager.CalculateWinningChances(team1, team2);
            string prediction = matchManager.PredictMatch(team1, team2);
            string formattedResult = $"{team1.Team} - {team2.Team}";
            formattedResult = formattedResult.PadRight(40);

            return "\t\t" + formattedResult + winningChances + "\t" + prediction + "\n";
        }

        public void EnableMatchPredictions()
        {
            string ret = "\n**************************************************************\n\n\n";
            if (matchPrediction)
            {
                ret += "Prikazivanje šansi za pobedu i predikciju rezultata mečeva je već omogućeno!";
            }
            else
            {
                matchPrediction = true;
                ret += "Prikazivanje šansi za pobedu i predikciju rezultata mečeva je omogućeno!";
            }
            ret += "\n\n\n**************************************************************\n\n";
            Console.WriteLine(ret);
        }

        public void DisableMatchPredictions()
        {
            string ret = "\n**************************************************************\n\n\n";
            if (!matchPrediction)
            {
                ret += "Prikazivanje šansi za pobedu i predikciju rezultata mečeva je već onemogućeno!";
            }
            else
            {
                matchPrediction = false;
                ret += "Prikazivanje šansi za pobedu i predikciju rezultata mečeva je onemogućeno!";
            }
            ret += "\n\n\n**************************************************************\n\n";
            Console.WriteLine(ret);
        }

        
    }
}
