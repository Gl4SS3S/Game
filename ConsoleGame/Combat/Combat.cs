using ConsoleGame.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleGame.Combat
{
    class Combat
    {
        private bool fight = false;

        public void InitFight(Player.Player player)
        {
            int turn = 1;
            Enemy enemy = new Enemy();

            Console.WriteLine("Starting fight");
            ExploreResult(player);
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

        private void Defend(Player.Player player, Enemy enemy)
        {
            Random random = new Random();
            int attack = random.Next(5, 13);

            if (enemy.Health > 30)
            {
                Console.WriteLine($"Enemy attacks for - {attack}");
                player.Health = player.Health - attack;
            }
            else
            {
                int probability = random.Next(0, 100);
                if (probability <= 20)
                {
                    Console.WriteLine("Enemy Ran Away....");
                    Thread.Sleep(1000);
                    fight = false;
                    return;
                }
                Console.WriteLine($"Enemy attacks for - {attack}");
                player.Health = player.Health - attack;
            }

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

        private void Attack(Player.Player player, Enemy enemy)
        {
            Console.Write($"Enemy Health: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("|" + new string('=', enemy.Health / 10) + "|");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Options: Attack, Skill, Retreat");
            string choice = Console.ReadLine()?.ToLower();


            switch (choice)
            {
                case "attack":
                    Console.Beep();
                    enemy.Health = enemy.Health - player.Attack;
                    break;
                case "skill":
                    useSkill(player.Skills, enemy, player);
                    break;
                case "retreat":
                    Console.WriteLine("You ran away....");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Thread.Sleep(1000);
                    fight = false;
                    return;
                default:
                    Console.Beep();
                    enemy.Health = enemy.Health - player.Attack;
                    break;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Enemy health: {enemy.Health}");

            if (enemy.Health <= 0 || player.Health <= 0)
            {
                player.PlayerLevel.Experiance += 20;
                if (player.PlayerLevel.Experiance >= player.PlayerLevel.ExperianceNeeded)
                {
                    player.PlayerLevel.Experiance = 0;
                    player.PlayerLevel.ExperianceNeeded = (int)(player.PlayerLevel.ExperianceNeeded + (player.PlayerLevel.ExperianceNeeded * player.PlayerLevel.ExprianceScaling));
                    player.PlayerLevel.Level++;
                }
                fight = false;
                player.Health = 100;
                if (player.PlayerArmour != null)
                {
                    player.Health += player.PlayerArmour.Health;
                }
            }
        }

        private void useSkill(Dictionary<string, Skill> skills, Enemy enemy, Player.Player player)
        {
            int i = 0;
            foreach (var (name, skill) in skills)
            {
                if (i == 0)
                {
                    Console.Write("   ");
                    Console.WriteLine("Name" + new string(' ', skill.Name.Length - 4) + " " + "Dmg" + " " + "Stam");
                    i++;
                }
                Console.Write(" - ");
                Console.WriteLine(skill.Name + " " + skill.Damage + " " + skill.ResourceUsage);
            }
            string chosenSkill = Console.ReadLine().ToLower();
            if (chosenSkill != null)
            {
                applySkill(chosenSkill, skills, enemy, player);
            }
        }

        private void applySkill(string chosenSkill, Dictionary<string, Skill> skills, Enemy enemy, Player.Player player)
        {
            skills.TryGetValue(chosenSkill, out Skill skill);
            if (skill != null)
            {
                if (player.Stamina < skill.ResourceUsage)
                {
                    Console.WriteLine("Not enough resources");
                    Thread.Sleep(1000);
                    return;
                }
                Console.Beep();
                enemy.Health -= skill.Damage;
                if (skill.Usage == SkillUsage.Stamina)
                {
                    player.Stamina -= skill.ResourceUsage;
                } 
            }
        }

        private void ExploreResult(Player.Player player)
        {
            Random r = new Random();
            if (r.Next(0, 2) == 1)
            {
                fight = true;
                return;
            }

            if (r.Next(0, 2) == 1)
            {
                PlayerArmour armour = new PlayerArmour();
                if (player.PlayerArmour != null)
                {
                    AddToInventory(armour, player);
                }
                Pickup(armour, player);
            }
            else
            {
                PlayerWeapon weapon = new PlayerWeapon();
                if (player.PlayerWeapon != null)
                {
                    AddToInventory(weapon, player);
                }
                Pickup(weapon, player);
            }
        }

        private void AddToInventory(object item, Player.Player player)
        {
            InventoryItem inventoryItem = new InventoryItem();
            var reflectedType = item.GetType();
            if (reflectedType.FullName != null && reflectedType.FullName.Split('.').Contains("PlayerWeapon"))
            {
                var weapon = item as PlayerWeapon;
                if (weapon != null)
                {
                    inventoryItem.Name = weapon.Weapon;
                    inventoryItem.Stats = weapon.Damage;
                    inventoryItem.Type = ItemType.Weapon;
                }
            }
            else
            {
                var armor = item as PlayerArmour;
                if (armor != null)
                {
                    inventoryItem.Name = armor.Armour;
                    inventoryItem.Stats = armor.Health;
                    inventoryItem.Type = ItemType.Armor;
                }
            }
            player.Inventory.Add(Guid.NewGuid(), inventoryItem);
        }

        private void Pickup(object item, Player.Player player)
        {
            var reflectedType = item.GetType();
            if (reflectedType.FullName != null && reflectedType.FullName.Split('.').Contains("PlayerWeapon"))
            {
                Console.WriteLine("Picked up: " + (item as PlayerWeapon)?.Weapon);
                Thread.Sleep(1000);
                if (player.PlayerWeapon == null)
                {
                    player.Attack += ((PlayerWeapon)item).Damage;
                }
                player.PlayerWeapon = item as PlayerWeapon;
            }
            else
            {
                Console.WriteLine("Picked up: " + (item as PlayerArmour)?.Armour);
                Thread.Sleep(1000);
                if (player.PlayerArmour == null)
                {
                    player.Health += ((PlayerArmour)item).Health;
                }
                player.PlayerArmour = item as PlayerArmour;
            }
        }
    }
}
