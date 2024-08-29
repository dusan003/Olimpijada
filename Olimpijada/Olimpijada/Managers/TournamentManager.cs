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

        Random r = new Random();

        private bool potsMade = false;
        private bool drawMade = false;
        private bool simulateQuarterFinalFinished = false;
        private bool simulateSemiFinalFinished = false;
        private bool simulateFinalFinished = false;

        private string Pots;

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

                    SemiFinalDraw += "\t\t" + SemiFinalPair2[0].Team + " - " + SemiFinalPair2[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    //SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);

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

                    SemiFinalDraw += "\nFinale:\n\n" + "\t\t" + FinalPair[0].Team + " - " + FinalPair[1].Team + "\n";
                    SemiFinalDraw += "\nUtakmica za treće mesto:\n\n" + "\t\t" + ThirdPlacePair[0].Team + " - " + ThirdPlacePair[1].Team + "\n";

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

                    FinalDraw += "\t\t" + FinalPair[0].Team + " - " + FinalPair[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    //SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);

                    FinalDraw += "\nUtakmica za treće mesto:\n\n";

                    result = matchManager.SimulateMatch(ThirdPlacePair[0], ThirdPlacePair[1]);
                    matchForFirstTeam = new Match(DateTime.Now, ThirdPlacePair[1].ISOCode, result[0] + ":" + result[1]);
                    matchForSecondTeam = new Match(DateTime.Now, ThirdPlacePair[0].ISOCode, result[1] + ":" + result[0]);

                    FinalDraw += "\t\t" + ThirdPlacePair[0].Team + " - " + ThirdPlacePair[1].Team + " (" + result[0] + ":" + result[1] + ")\n";
                    //SaveTheMatch(matchForFirstTeam, matchForSecondTeam, firstIndex, secondIndex, result[0], result[1]);


                    simulateFinalFinished = true;
                }
                else
                {
                    FinalDraw += "\nFinalna utakmica i utakmica za treće messto su već odigrane!\n\n";
                }
            }
            else
            {
                FinalDraw += "\nPrvo se moraju odigrati svi mečevi u grupama, četvrtfinalni mečevi,\nkao i polufinalni mečevi da bi se formirao finalni par i par za treće mesto!\n\n";
            }
            FinalDraw += "\n**************************************************************\n\n";
            Console.Write(FinalDraw);
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

        public void PrintThePots(bool can)
        {
            Pots = "\n**************************************************************\n\n";
            if (can)
            {

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

                potsMade = true;
            }
            else
            {
                Pots += "Prvo se moraju odigrati svi mečevi u grupama da bi se formirali šeširi za narednu fazu takmičenja!";
            }
            Pots += "\n\n**************************************************************\n\n";
            Console.WriteLine(Pots);
        }
    }
}
