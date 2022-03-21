using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsoleGame.Player;
using ConsoleGame.World;

namespace ConsoleGame
{
    class Program
    {
        static bool gameRunning = true;
        static bool fight = false;
        static string location = "A";
        static Player.Player player = new();

        
        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Title = "Game";
            while (gameRunning)
            {
                Console.WriteLine("Current location: " + location);
                Console.WriteLine("Available options: " + String.Join(", ", Continents.GetOptions[location]) + " or Explore Area");
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
                    InitFight();
                    break;
                case "exit":
                    gameRunning = false;
                    break;
                case "stats":
                    CheckStats();
                    break;
                default:
                    location = move.ToUpper();
                    break;
            }
        }

        private static void CheckStats()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=',30));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Health: " + player.Health + " " + new string('=', player.Health/10));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Damage: " + player.Attack);
            Console.WriteLine(new string('=',30));
            Console.WriteLine("Armour: " + (player.PlayerArmour == null ? "" : player.PlayerArmour.Armour));
            Console.WriteLine("Weapon: " + (player.PlayerWeapon == null ? "" : player.PlayerWeapon.Weapon));
            Console.WriteLine(new string('=',30));
            Console.ForegroundColor = ConsoleColor.Yellow;

            Thread.Sleep(1500);
        }

        private static void InitFight()
        {
            int turn = 1;
            Enemy enemy = new Enemy();
            
            Console.WriteLine("Starting fight");
            ExploreResult();
            while (fight)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"===== Turn {turn} =======");
                if (turn % 2 == 0)
                {

                    Console.WriteLine("Your turn");
                    Console.WriteLine("Choices: Attack");
                    Attack(player, enemy);
                    Console.Clear();
                }
                else
                {
                    Defend(player, enemy);
                }
                
                turn++;
            }
        }

        private static void ExploreResult()
        {
            Random r = new Random();
            if (r.Next(0,2) == 1)
            {
                fight = true;
                return;
            }

            if (r.Next(0, 2) == 1)
            {
                PlayerArmour armour = new PlayerArmour();
                Pickup(armour);
            }
            else
            {
                PlayerWeapon weapon = new PlayerWeapon();
                Pickup(weapon);
            }
        }

        private static void Pickup(object item)
        {
            var reflectedType = item.GetType();
            if (reflectedType.FullName != null && reflectedType.FullName.Split('.').Contains("PlayerWeapon"))
            {
                Console.WriteLine("Picked up: " + (item as PlayerWeapon)?.Weapon);
                Thread.Sleep(1000);
                if (player.PlayerWeapon == null)
                {
                    player.Attack += ((PlayerWeapon) item).Damage;
                }
                player.PlayerWeapon = item as PlayerWeapon;
            }
            else
            {
                Console.WriteLine("Picked up: " + (item as PlayerArmour)?.Armour);
                Thread.Sleep(1000);
                if (player.PlayerArmour == null)
                {
                    player.Health += ((PlayerArmour) item).Health;
                }
                player.PlayerArmour = item as PlayerArmour;
            }
        }


        private static void Defend(Player.Player player, Enemy enemy)
        {
            Random random = new Random();
            int attack = random.Next(5, 13);

            Console.WriteLine($"Enemy attacks for - {attack}");
            player.Health = player.Health - attack;
            
            Console.Beep();
            
            Console.Write($"Player Health: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("|" + new string('=', player.Health / 10) + "|");

            if (enemy.Health <= 0 || player.Health <= 0)
            {
                fight = false;
                player.Health = 100;
                if (player.PlayerArmour != null)
                {
                    player.Health += player.PlayerArmour.Health;
                }
            }
        }

        private static void Attack(Player.Player player, Enemy enemy)
        {
            Console.Write($"Enemy Health: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("|" + new string('=', enemy.Health / 10) + "|");
            string choice = Console.ReadLine()?.ToLower();

            switch (choice)
            {
                case "attack":
                    Console.Beep();
                    enemy.Health = enemy.Health - player.Attack;
                    break;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Enemy health: {enemy.Health}");
            
            if (enemy.Health <= 0 || player.Health <= 0)
            {
                fight = false;
                player.Health = 100;
                if (player.PlayerArmour != null)
                {
                    player.Health += player.PlayerArmour.Health;
                }
            }
        }
    }
}