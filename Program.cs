using System;
using System.Collections.Generic;

namespace CSLight
{
    class Program
    {
        static void Main()
        {
            Arena arena = new Arena();

            arena.StartFight();
        }
    }

    class Arena
    {        
        public void StartFight()
        {
            Warrior warrior1 = ChooseWarrior();
            Warrior warrior2 = ChooseWarrior();

            while (warrior1.IsAlive && warrior2.IsAlive)
            {
                warrior1.TakeDamage(warrior2.GiveDamage());
                warrior1.ShowInfo();

                Console.WriteLine();

                warrior2.TakeDamage(warrior1.GiveDamage());
                warrior2.ShowInfo();

                Console.WriteLine("----------");
            }
        }         

        private Warrior ChooseWarrior()
        {
            Warrior warrior = null;
            List<Warrior> warriors = CreateWarriors();
            int fighterIndex;

            for (int i = 0; i < warriors.Count; i++)
            {
                Console.Write(i + " ");
                warriors[i].ShowInfo();
                Console.WriteLine();
            }

            while(warrior == null)
            {
                Console.Write("Выберите бойца: ");

                if (int.TryParse(Console.ReadLine(), out fighterIndex) && fighterIndex < warriors.Count && fighterIndex >= 0)
                {
                    warrior = warriors[fighterIndex];
                    Console.Clear();
                    
                }
            }
            return warrior;
        }

        private List<Warrior> CreateWarriors()
        {
            return new List<Warrior>()
            {
                new Knight("Рыцарь", 100, 15, 40),
                new Berserk("Берсерк", 100, 15, 25),
                new Tank("Танк", 100, 10, 25),
                new Assasin("Убийца", 101, 30, 20)
            };
        }
    }

    abstract class Warrior
    {
        public string Name { get; protected set; }
        public float Health { get; protected set; }
        public float Damage { get;  protected set; }
        public float Armor { get; protected set; }

        public Warrior(string name, float health, float damage, float armor)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public bool IsAlive => Health > 0;

        public virtual float GiveDamage()
        {
            return Damage;
        }


        public virtual void TakeDamage(float damage)
        {
            int armorPercentage = 100;
            Health -= damage - (damage * Armor / armorPercentage);
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Имя - {Name}\nЗдоровье - {Health}\nУрон - {Damage}\nБроня - {Armor}");
        }
    }

    class Knight : Warrior
    {
        public Knight(string name, float health, float damage, float armor) : base(name, health, damage, armor) { }
        
        public override void TakeDamage(float damage)
        {
            Random defendChance = new Random();

            int maxChance = 100 + 1;
            int minChance = 1;
            int desiredChance = 50;

            if (defendChance.Next(minChance, maxChance) <= desiredChance)
            {
                damage = CutDamage(damage);
            }
            base.TakeDamage(damage);
        }

        private float CutDamage(float takeDamage)
        {
            int armorPercentage = 100;
            takeDamage -= takeDamage * (Armor / armorPercentage);

            return takeDamage;
        }
    }

    class Berserk : Warrior
    {        
        public Berserk(string name, float health, float damage, float armor) : base(name, health, damage, armor) { }
        
        public override float GiveDamage()
        {
            Random critChance = new Random();
            int critDamage = 2;

            int maxChance = 100 + 1;
            int minChance = 1;
            int desiredChance = 30;

            if(critChance.Next(minChance, maxChance) <= desiredChance)
            {
                return base.GiveDamage() * critDamage;
            }
            return base.GiveDamage();
        }
    }

    class Tank : Warrior
    {
        public Tank(string name, float health, float damage, float armor) : base(name, health, damage, armor) { }        

        public override float GiveDamage()
        {
            Random healChance = new Random();
            float healPercent = 0.3f;

            int maxChance = 100 + 1;
            int minChance = 1;
            int desiredChance = 60;

            if (healChance.Next(minChance, maxChance) <= desiredChance)
            {
                Health += Damage * healPercent;
                return base.GiveDamage();
            }
            return base.GiveDamage();
        }
    }

    class Assasin : Warrior
    {
        public Assasin(string name, float health, float damage, float armor) : base(name, health, damage, armor) { }        

        public override void TakeDamage(float damage)
        {
            if (TryDodge() == false)
            {
                base.TakeDamage(damage);
            }
        }

        private bool TryDodge()
        {
            Random dodgeChance = new Random();

            int maxChance = 100 + 1;
            int minChance = 1;
            int desiredChance = 30;

            return dodgeChance.Next(minChance, maxChance) <= desiredChance;
        }
    }
}