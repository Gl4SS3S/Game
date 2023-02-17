using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGame.Player;
using ConsoleGame.World;
using Spectre.Console;
using System.Media;
using System.Threading;
using System.IO;

namespace ConsoleGame
{
    class Program
    {
        static bool gameRunning = true;
        static string location = "A";
        static Player.Player player = new();
        private static Combat.Combat _combat = new Combat.Combat();
        public static StringBuilder builder = new();

        static async Task Main(string[] args)
        {
            //Init SKill
            player.Skills.Add("Jump Attack".ToLower(), new Skill { Name = "Jump Attack", Damage = 40, ResourceUsage = 40, Usage = SkillUsage.Stamina });
            player.Skills.Add("Shield Bash".ToLower(), new Skill { Name = "Shield Bash", Damage = 20, ResourceUsage = 50, Usage = SkillUsage.Stamina });

            while (gameRunning)
            {
                builder.AppendLine("Current location: " + location)
                    .AppendLine("Available options: " + String.Join(", ", Continents.GetOptions[location]) + " or Explore Area!");

                var panel = new Panel(builder.ToString());
                panel.Header("Game", Justify.Center);
                panel.Expand();

                string[] extraChoices = new string[] { "explore", "stats"};
                extraChoices = extraChoices.Concat(Continents.GetOptions[location]).ToArray();

                // Render the table to the console
                AnsiConsole.Write(panel);
                var move = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("What's your [green]Choice[/]?")
                        .PageSize(10)
                        .AddChoices(extraChoices));

               

                if (move != null) ProcessOption(move);
                builder.Clear();
                Console.Clear();
            }
        }

        private static void PlayMusic(object obj)
        {
            if (OperatingSystem.IsWindows())
            {
                var path =  Path.Combine(Path.GetFullPath(@"..\..\..\"), @"Audio\BGMusic", "Future King of Heaven - Zachariah Hickman.wav");
                if (File.Exists(path))
                {
                    SoundPlayer player = new SoundPlayer(path);
                    player.Load();
                    player.PlayLooping();
                }
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