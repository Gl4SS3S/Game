using System;
using System.Collections.Generic;

namespace ConsoleGame.Player
{
    public class Player
    {
        public string Name { get; set; } = "Player";

        public int Health { get; set; } = 100;

        public int Attack { get; set; } = 10;

        public int Stamina { get; set; } = 100;

        public PlayerLevel PlayerLevel { get; set; } = new();

        public PlayerArmour PlayerArmour { get; set; }

        public PlayerWeapon PlayerWeapon { get; set; }

        public Dictionary<Guid, InventoryItem> Inventory { get; set; } = new();

        public Dictionary<string, Skill> Skills { get; set; } = new();
    }

    public class PlayerLevel
    {

        public float ExprianceScaling { get; set; } = 0.1F;

        public int ExperianceNeeded { get; set; } = 100;

        public int Experiance { get; set; } = 0;

        public int Level { get; set; } = 1;
    }

    public class PlayerArmour
    {
        public string Armour { get; set; } = "Leather Hat ";

        public int Health { get; set; } = 20;
    }
    
    public class PlayerWeapon
    {
        public string Weapon { get; set; } = "Bronze Sword";

        public int Damage { get; set; } = 10;
    }

    public class InventoryItem
    {
        public string Name { get; set; }

        public ItemType Type { get; set; }

        public int Stats { get; set; }
    }

    public class Skill
    {
        public int Damage { get; set; }

        public string Name { get; set; }

        public int ResourceUsage { get; set; }

        public SkillUsage Usage { get; set; }
    }

    public enum ItemType
    {
        Weapon,
        Armor
    }

    public enum SkillUsage
    {
        Stamina,
        Mana
    }
}