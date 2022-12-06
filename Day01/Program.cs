using System.Linq;
using AOC2022.Core;

namespace AOC2022.Day01
{ 
    internal class Food
    {
        public int Calories { get; set; }
    }

    internal class Elf
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

    internal class ElfCollection
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

    public abstract class Day01Puzzle
    {
        public string Solve(IEnumerable<string> input, int chunkSize)
        {
            var elves = new ElfCollection();
            
            foreach (string line in input)
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
            return elves.GetTopCaloryCarryingElves(chunkSize).Sum(x => x.TotalFoodCalories).ToString();            
        }
    }
    
    public class Day01Puzzle01 : Day01Puzzle, IPuzzle
    {
        public string Solve(IEnumerable<string> input)
        {
            return Solve(input, 1);
        }
    }

    public class Day01Puzzle02 : Day01Puzzle, IPuzzle
    {
        public string Solve(IEnumerable<string> input)
        {
            return Solve(input, 3);
        }
    }
}