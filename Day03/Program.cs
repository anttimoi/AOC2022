using System.Linq;
using System.IO;
using AOC2022.Core;

namespace AOC2022.Day03
{
    internal static class ItemPriority
    {
        public static int Get(char x)
        {
            if (x >= _firstLowercase && x <= _lastLowercase)
            {
                return x - _firstLowercase + 1;
            }

            if (x >= _firstUppercase && x <= _lastLowercase)
            {
                return (x - _firstUppercase) + (_lastLowercase - _firstLowercase) + 2;
            }
            
            throw new ArgumentException($"Invalid input {x}");
        }

        private static char _firstLowercase = 'a';
        private static char _firstUppercase = 'A';
        
        private static char _lastLowercase = 'z';
        private static char _lastUppercase = 'Z';        
    }

    internal class Rucksack
    {
        private string _contents;

        public Rucksack(string contents)
        {
            if (contents.Length % 2 == 1)
            {
                throw new ArgumentException("Rucksack compartments aren't equal size.");
            }
            _contents = contents;
        }

        public List<char> FirstCompartment
        {
            get => new List<char>(_contents.Substring(0, _contents.Length / 2));
        }
        
        public List<char> SecondCompartment
        {
            get => new List<char>(_contents.Substring(_contents.Length / 2, _contents.Length / 2));
        }

        public char FindCommonItemInCompartments()
        {
            var index = FirstCompartment
                .Select(x => SecondCompartment.IndexOf(x))
                .Aggregate(0, (acc, x) => x > acc ? x : acc);

            return SecondCompartment[index];
        }
    }

    internal class CharCounter
    {
        public static char GetCharWithCount(List<char> chars, int count)
        {
            var charCounts = chars.Select(x => chars.Count(y => y == x)).ToList();
            var index = charCounts.FindIndex(x => x == count);
            return chars[index];
        }
    }

    internal static class Helpers
    {
        public static List<char> GetDistinctItems(Rucksack x)
        {
            return x.FirstCompartment
                .Concat(x.SecondCompartment)
                .Distinct()
                .ToList();
        }

        public static List<char> ConcatItemsForCounting(List<char> acc, Rucksack y)
        {
            return acc.Concat(GetDistinctItems(y)).ToList();
        }
    }

    public class Day03Puzzle01 : IPuzzle
    {
        public string Solve(IEnumerable<string> input)
        {
            var rucksacks = input.Select(x => new Rucksack(x));

            var prioritySum = rucksacks
                .Select(x => x.FindCommonItemInCompartments())
                .Select(ItemPriority.Get)
                .Sum();

            return prioritySum.ToString();
        }
    }

    public class Day03Puzzle02 : IPuzzle
    {
        public string Solve(IEnumerable<string> input)
        {
            var rucksacks = input.Select(x => new Rucksack(x));

            const int chunkSize = 3;
            var rucksackCharacters = rucksacks
                .Chunk(chunkSize)
                .Select(x => x.Aggregate(new List<char>(), Helpers.ConcatItemsForCounting));

            var prioritySum2 = rucksackCharacters
                .Select(x => CharCounter.GetCharWithCount(x, chunkSize))
                .Select(ItemPriority.Get)
                .Sum();

            return prioritySum2.ToString();
        }
    }
}
