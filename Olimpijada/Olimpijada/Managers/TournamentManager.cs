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

        private MatchManager matchManager = new MatchManager();

        private ObservableCollection<Country> finalRanking = new ObservableCollection<Country>();

        ObservableCollection<Country> HatD = new ObservableCollection<Country>();
        ObservableCollection<Country> HatE = new ObservableCollection<Country>();
        ObservableCollection<Country> HatF = new ObservableCollection<Country>();
        ObservableCollection<Country> HatG = new ObservableCollection<Country>();

        ObservableCollection<Country> QuarterFinalPair1 = new ObservableCollection<Country>();
        ObservableCollection<Country> QuarterFinalPair2 = new ObservableCollection<Country>();
        ObservableCollection<Country> QuarterFinalPair3 = new ObservableCollection<Country>();
        ObservableCollection<Country> QuarterFinalPair4 = new ObservableCollection<Country>();

        ObservableCollection<Country> SemiFinalPair1 = new ObservableCollection<Country>();
        ObservableCollection<Country> SemiFinalPair2 = new ObservableCollection<Country>();

        ObservableCollection<Country> FinalPair = new ObservableCollection<Country>();

        Random r = new Random();

        private bool hatsMade = false;
        private bool drawMade = false;
        private bool simulateQuarterFinalFinished = false;
        private bool simulateSemiFinalFinished = false;
        private bool simulateFinalFinished = false;

        private string Hats;

        private string QuarterFinalDraw;
        private string SemiFinalDraw;
        private string FinalDraw;

        public TournamentManager() 
        {

        }

        private void MakeThePairsOfQuarterFinal()
        {
            if (!drawMade)
            {
                QuarterFinalPair1.Add(HatD[0]);
                QuarterFinalPair2.Add(HatD[1]);

                int indexHatG = r.Next(0, 2);

                if (HatD[0].Group != HatG[indexHatG].Group && HatD[1].Group != HatG[1-indexHatG].Group)
                {
                    QuarterFinalPair1.Add(HatG[indexHatG]);
                    QuarterFinalPair2.Add(HatG[1 - indexHatG]);
                }
                else
                {
                    int newIndex = 1 - indexHatG;
                    QuarterFinalPair1.Add(HatG[newIndex]);
                    QuarterFinalPair2.Add(HatG[1 - newIndex]);
                }

                QuarterFinalPair3.Add(HatE[0]);
                QuarterFinalPair4.Add(HatE[1]);

                int indexHatF = r.Next(0, 2);

                if (HatE[0].Group != HatF[indexHatF].Group && HatE[1].Group != HatF[1 - indexHatG].Group)
                {
                    QuarterFinalPair3.Add(HatF[indexHatF]);
                    QuarterFinalPair4.Add(HatF[1 - indexHatF]);
                }
                else
                {
                    int newIndex = 1 - indexHatF;
                    QuarterFinalPair3.Add(HatF[newIndex]);
                    QuarterFinalPair4.Add(HatF[1 - newIndex]);
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
                    QuarterFinalDraw += "Četvrtfinale:\n";
                    int[] result = new int[2];


                    result = matchManager.SimulateMatch(QuarterFinalPair1[0], QuarterFinalPair1[1]);
                    Match matchForFirstTeam = new Match(DateTime.Now, QuarterFinalPair1[1].ISOCode, result[0] + ":" + result[1]);
                    Match matchForSecondTeam = new Match(DateTime.Now, QuarterFinalPair1[0].ISOCode, result[1] + ":" + result[0]);

                    QuarterFinalDraw += "\t\t" + QuarterFinalPair1[0].Team + " - " + QuarterFinalPair1[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    //SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);

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

                    QuarterFinalDraw += "\t\t" + QuarterFinalPair2[0].Team + " - " + QuarterFinalPair2[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    //SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);

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

                    QuarterFinalDraw += "\t\t" + QuarterFinalPair3[0].Team + " - " + QuarterFinalPair3[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    //SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);

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

                    QuarterFinalDraw += "\t\t" + QuarterFinalPair4[0].Team + " - " + QuarterFinalPair4[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    //SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);

                    if (result[0] > result[1])
                    {
                        SemiFinalPair2.Add(QuarterFinalPair4[0]);
                    }
                    else
                    {
                        SemiFinalPair2.Add(QuarterFinalPair4[1]);
                    }

                    QuarterFinalDraw += "\nPolufinalni mečevi:\n\n" + "\t\t" + SemiFinalPair1[0].Team + " - " + SemiFinalPair1[1].Team;
                    QuarterFinalDraw += "\n\t\t" + SemiFinalPair2[0].Team + " - " + SemiFinalPair2[1].Team + "\n";

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
                    SemiFinalDraw += "Polufinale:\n";
                    int[] result = new int[2];


                    result = matchManager.SimulateMatch(SemiFinalPair1[0], SemiFinalPair1[1]);
                    Match matchForFirstTeam = new Match(DateTime.Now, SemiFinalPair1[1].ISOCode, result[0] + ":" + result[1]);
                    Match matchForSecondTeam = new Match(DateTime.Now, SemiFinalPair1[0].ISOCode, result[1] + ":" + result[0]);

                    SemiFinalDraw += "\t\t" + SemiFinalPair1[0].Team + " - " + SemiFinalPair1[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    //SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);

                    if (result[0] > result[1])
                    {
                        FinalPair.Add(SemiFinalPair1[0]);
                    }
                    else
                    {
                        FinalPair.Add(SemiFinalPair1[1]);
                    }

                    result = matchManager.SimulateMatch(SemiFinalPair2[0], SemiFinalPair2[1]);
                    matchForFirstTeam = new Match(DateTime.Now, SemiFinalPair2[1].ISOCode, result[0] + ":" + result[1]);
                    matchForSecondTeam = new Match(DateTime.Now, SemiFinalPair2[0].ISOCode, result[1] + ":" + result[0]);

                    SemiFinalDraw += "\t\t" + SemiFinalPair2[0].Team + " - " + SemiFinalPair2[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    //SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);

                    if (result[0] > result[1])
                    {
                        FinalPair.Add(SemiFinalPair2[0]);
                    }
                    else
                    {
                        FinalPair.Add(SemiFinalPair2[1]);
                    }

                    SemiFinalDraw += "\nFinale:\n\n" + "\t\t" + FinalPair[0].Team + " - " + FinalPair[1].Team + "\n";

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

        public void SimulateFinalMatch()
        {
            FinalDraw = "\n**************************************************************\n\n";

            if (simulateQuarterFinalFinished && simulateSemiFinalFinished)
            {
                if (!simulateFinalFinished)
                {
                    FinalDraw += "Finale:\n";
                    int[] result = new int[2];


                    result = matchManager.SimulateMatch(FinalPair[0], FinalPair[1]);
                    Match matchForFirstTeam = new Match(DateTime.Now, FinalPair[1].ISOCode, result[0] + ":" + result[1]);
                    Match matchForSecondTeam = new Match(DateTime.Now, FinalPair[0].ISOCode, result[1] + ":" + result[0]);

                    FinalDraw += "\t\t" + FinalPair[0].Team + " - " + FinalPair[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    //SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);



                    simulateFinalFinished = true;
                }
                else
                {
                    FinalDraw += "\nFinalna utakmica je već odigrana!\n\n";
                }
            }
            else
            {
                FinalDraw += "\nPrvo se moraju odigrati svi mečevi u grupama, četvrtfinalni mečevi,\nkao i polufinalni mečevi da bi se formirao finalni par!\n\n";
            }
            FinalDraw += "\n**************************************************************\n\n";
            Console.Write(FinalDraw);
        }

        public void MakeTheHats(ObservableCollection<Country> final)
        {
            finalRanking = final;
            if (!hatsMade)
            {
                HatD.Add(finalRanking[0]);
                HatD.Add(finalRanking[1]);

                HatE.Add(finalRanking[2]);
                HatE.Add(finalRanking[3]);

                HatF.Add(finalRanking[4]);
                HatF.Add(finalRanking[5]);

                HatG.Add(finalRanking[6]);
                HatG.Add(finalRanking[7]);
            }
        }

        public void PrintTheHats(bool can)
        {
            Hats = "\n**************************************************************\n\n";
            if (can)
            {

                Hats += "Šeširi:\n\tŠešir D\n\t\t";
                Hats += HatD[0].Team + "\n\t\t";
                Hats += HatD[1].Team + "\n\t";

                Hats += "Šešir E\n\t\t";
                Hats += HatE[0].Team + "\n\t\t";
                Hats += HatE[1].Team + "\n\t";

                Hats += "Šešir F\n\t\t";
                Hats += HatF[0].Team + "\n\t\t";
                Hats += HatF[1].Team + "\n\t";

                Hats += "Šešir G\n\t\t";
                Hats += HatG[0].Team + "\n\t\t";
                Hats += HatG[1].Team;

                hatsMade = true;
            }
            else
            {
                Hats += "Prvo se moraju odigrati svi mečevi u grupama da bi se formirali šeširi za narednu fazu takmičenja!";
            }
            Hats += "\n\n**************************************************************\n\n";
            Console.WriteLine(Hats);
        }
    }
}
