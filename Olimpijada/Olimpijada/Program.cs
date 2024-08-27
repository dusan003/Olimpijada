using System.Text.Json;
using Olimpijada.Models;
using System.IO;
using Olimpijada.Managers;
using System.ComponentModel.Design;

string groupsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "groups.json");

TournamentManager tournamentManager = new TournamentManager();
GroupManager groupManager = new GroupManager();
MatchManager matchManager = new MatchManager();

int result;

Console.WriteLine("-------------------------------Dobrodosli u Simulaciju Olimpijskih igara-------------------------------");


do
{
    Console.WriteLine("Izaberite opciju iz menija:");
    Console.WriteLine("\t1)Ispisi sve grupe");
    Console.WriteLine("\t2)Simuliraj prvo kolo u grupi");
    Console.WriteLine("\t3)Simuliraj drugo kolo u grupi");
    Console.WriteLine("\t4)Simuliraj trece kolo u grupi");
    Console.WriteLine("\t5)Ispisi sve rezultate u grupi");
    Console.WriteLine("\t6)Ispisi trenutno stanje na tabeli");
    Console.WriteLine("\t10)Izlaz iz simulacije");

    Int32.TryParse(Console.ReadLine(), out result);
    Menu(result);

} while (result != 10);

Console.WriteLine("Unesite 1 ako zelite da ispisete sve grupe:");

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
            groupManager.SimulateTheThirdRoundOfGroupMatches();
            break;
        case 5:
            groupManager.PrintAllResultsInTheGroup();
            break;
        case 6:
            groupManager.PrintCurrentGroupState();
            break;
        default:
            Console.WriteLine("Morate uneti validnu opciju!");
            break;
    }
}
