namespace ConsoleGame.Player
{
    public class Player
    {
        public string Name { get; set; } = "Player";

        public int Health { get; set; } = 100;

        public int Attack { get; set; } = 10;

        public int Stamina { get; set; } = 100;

        public PlayerArmour PlayerArmour { get; set; }

        public PlayerWeapon PlayerWeapon { get; set; }
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
}