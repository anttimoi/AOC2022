// See https://aka.ms/new-console-template for more information
using System.Linq;

namespace day_01
{ 
    public class Food
    {
        public int Calories { get; set; }
    }

    public class Elf
    {
        public Elf()
        {
            _food = new List<Food>();
        }

        public void AddFoodByCalories(int calories)
        {
            _food.Add(new Food { Calories = calories });
        }

        public int TotalFoodCalories
        {
            get => _food.Sum(x => x.Calories);
        }

        private List<Food> _food;
    }

    public class ElfCollection
    {
        public ElfCollection()
        {
            _elves = new List<Elf>() { new Elf() };
        }

        public Elf LatestElf
        {
            get => _elves.Last();
        }

        public void AddNewElf()
        {
            _elves.Add(new Elf());
        }

        public IEnumerable<Elf> GetTopCaloryCarryingElves(int elfCount)
        {
            return _elves.OrderByDescending(x => x.TotalFoodCalories).Take(elfCount);
        }

        private List<Elf> _elves;
    }

    class Day01
    {
        static void Main(string[] args)
        {
            var elves = new ElfCollection();


            foreach (string line in System.IO.File.ReadLines("input.txt"))
            {
                if (Int32.TryParse(line, out int calories))
                {
                    elves.LatestElf.AddFoodByCalories(calories);
                }
                else
                {
                    elves.AddNewElf();
                }
            }

            System.Console.WriteLine(elves.GetTopCaloryCarryingElves(1).Sum(x => x.TotalFoodCalories));
            System.Console.WriteLine(elves.GetTopCaloryCarryingElves(3).Sum(x => x.TotalFoodCalories));
        }
    }
}