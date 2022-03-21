using System;
using System.Linq;
using System.Threading.Tasks;
using ConsoleGame.Player;
using ConsoleGame.World;

namespace ConsoleGame
{
    class Program
    {
        static bool gameRunning = true;
        static string location = "A";
        static Player.Player player = new();
        private static Combat.Combat _combat = new Combat.Combat();

        static async Task Main(string[] args)
        {
            //Window Size
            Console.WindowHeight = 30;
            Console.WindowWidth = 120;

            //Init SKill
            player.Skills.Add("Jump Attack".ToLower(), new Skill { Name = "Jump Attack", Damage = 40, ResourceUsage = 40, Usage = SkillUsage.Stamina });
            player.Skills.Add("Shield Bash".ToLower(), new Skill { Name = "Shield Bash", Damage = 20, ResourceUsage = 50, Usage = SkillUsage.Stamina });

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Title = "Game";

            while (gameRunning)
            {
                Console.WriteLine("Current location: " + location);
                Console.WriteLine("Available options: " + String.Join(", ", Continents.GetOptions[location]) + " or Explore Area!");
                string move = Console.ReadLine()?.ToLower();

                if (move != null) ProcessOption(move);
                Console.Clear();
            }
        }

        private static void ProcessOption(string move)
        {
            switch (move)
            {
                case "explore":
                    _combat.InitFight(player);
                    break;
                case "exit":
                    gameRunning = false;
                    break;
                case "stats":
                    CheckStats();
                    break;
                default:
                    if (!Continents.GetOptions.ContainsKey(move.ToUpper())) return;
                    location = move.ToUpper();
                    break;
            }
        }

        private static void CheckStats()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=',30));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Health: " + player.Health + " " + new string('=', player.Health/10) + ">");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Damage: " + player.Attack);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Stamina: " + player.Stamina + " " + new string('=', player.Stamina / 10) + ">");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Experience: " + player.PlayerLevel.Experiance + " " + new string('=', (int)(player.PlayerLevel.Experiance / 10)) + ">");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Level: " + (player.PlayerLevel.Level + 1));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=',30));
            Console.WriteLine("Armour: " + (player.PlayerArmour == null ? "" : player.PlayerArmour.Armour));
            Console.WriteLine("Weapon: " + (player.PlayerWeapon == null ? "" : player.PlayerWeapon.Weapon));
            Console.WriteLine(new string('=',30));
            Console.WriteLine("Inventory Items: " + player.Inventory.Count());
            if (player.Inventory.Count() > 0)
            {
                int i = 0;
                foreach (var item in player.Inventory.Values)
                {
                    if (i == 5) break;
                    Console.Write(" - ");
                    Console.WriteLine(item.Name + " " + item.Stats + " " + item.Type);
                    i++;
                }
            }
            Console.WriteLine(new string('=', 30));
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Press 'R' to return to exploring.");
            while (true)
            {
                string key = Console.ReadLine().ToLower();
                if (key == "r")
                {
                    return;
                } else
                {
                    Console.WriteLine("Wrong input");
                }
            }
        }

        



    }
}