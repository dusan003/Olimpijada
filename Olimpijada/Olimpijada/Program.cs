using System.Text.Json;
using Olimpijada.Models;
using System.IO;
using Olimpijada.Managers;
using System.ComponentModel.Design;
using System.Collections.ObjectModel;

string groupsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "groups.json");

GroupManager groupManager = new GroupManager();

ObservableCollection<Country> finalRanking = new ObservableCollection<Country>();
ObservableCollection<Country> Countries = new ObservableCollection<Country>();

Countries = groupManager.GetAllCountries();
MatchManager matchManager = new MatchManager(Countries);
TournamentManager tournamentManager = new TournamentManager(Countries);

int result;
bool groupStageFinished = false;

Console.WriteLine("\n-------------------------------Dobrodosli u Simulaciju Olimpijskih igara-------------------------------\n\n");


do
{
    Console.WriteLine("Izaberite opciju iz menija:");
    Console.WriteLine("\t1)Ispiši sve grupe");
    Console.WriteLine("\t2)Simuliraj prvo kolo u grupi");
    Console.WriteLine("\t3)Simuliraj drugo kolo u grupi");
    Console.WriteLine("\t4)Simuliraj trece kolo u grupi");
    Console.WriteLine("\t5)Ispiši sve rezultate u grupi");
    Console.WriteLine("\t6)Ispiši trenutno stanje na tabeli");
    Console.WriteLine("\t7)Ispiši konačan rang svih timova u grupi");
    Console.WriteLine("\t8)Ispiši šešire");
    Console.WriteLine("\t9)Simuliraj četvrtfinale");
    Console.WriteLine("\t10)Simuliraj polufinale");
    Console.WriteLine("\t11)Simuliraj finale");
    Console.WriteLine("\t12)Ispiši timove koji su osvojili medalje");
    Console.WriteLine("\t13)Ispiši rezultate nokaut faze turnira");
    Console.WriteLine("\t15)Izlaz iz simulacije");

    Int32.TryParse(Console.ReadLine(), out result);
    Menu(result);

} while (result != 15);

void Menu(int result)
{
    switch (result)
    {
        case 1:
            groupManager.PrintAllGroups();
            break;
        case 2:
            groupManager.SimulateTheFirstRoundOfGroupMatches();
            break;
        case 3:
            groupManager.SimulateTheSecondRoundOfGroupMatches();
            break;
        case 4:
            groupStageFinished = groupManager.SimulateTheThirdRoundOfGroupMatches();
            if (groupStageFinished)
            {
                finalRanking = groupManager.GetFinalRanking();
                tournamentManager.MakeThePots(finalRanking);
            }
            break;
        case 5:
            groupManager.PrintAllResultsInTheGroup();
            break;
        case 6:
            groupManager.PrintCurrentGroupState();
            break;
        case 7:
            groupManager.PrintFinalRankingInTheGroups();
            break;
        case 8:
            tournamentManager.PrintThePotsAndQuarterFinalMatches(groupStageFinished);
            break;
        case 9:
            tournamentManager.SimulateQuearterFinal(groupStageFinished);
            break;
        case 10:
            tournamentManager.SimulateSemiFinal();
            break;
        case 11:
            tournamentManager.SimulateFinalMatchAndThirdPlaceMatch();
            break;
        case 12:
            tournamentManager.PrintTopThreeTeams();
            break;
        case 13:
            tournamentManager.PrintCurrentResultsOfKnockoutStage();
            break;
        case 14:
            groupManager.PrintAllMechesForUsa();
            break;
        case 15:
            break;
        default:
            Console.WriteLine("Morate uneti validnu opciju!");
            break;
    }
}
