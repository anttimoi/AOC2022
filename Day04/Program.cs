using System.Linq;
using AOC2022.Core;

namespace AOC2022.Day04
{
    internal class SectionIdRange
    {
        public SectionIdRange(int start, int stop)
        {
            if (start > stop)
            {
                throw new ArgumentException("Start has to be smaller than stop");
            }
            
            Start = start;
            Stop = stop;
        }

        public bool IsInside(SectionIdRange another)
        {
            return another.Start <= Start && another.Stop >= Stop;
        }

        private bool IsInside(int x) => x >= Start && x <= Stop;

        public bool Overlaps(SectionIdRange another)
        {
            return IsInside(another.Start) || IsInside(another.Stop);
        }
        
        public int Start { get; }
        public int Stop { get; }
    }

    internal static class Extensions
    {
        public static bool HasWrappingRange(this Tuple<SectionIdRange, SectionIdRange> x)
        {
            return x.Item1.IsInside(x.Item2) || x.Item2.IsInside(x.Item1);
        }

        public static bool HasOverlappingRanges(this Tuple<SectionIdRange, SectionIdRange> x)
        {
            return x.Item1.Overlaps(x.Item2) || x.Item2.Overlaps(x.Item1);
        }
    }
    
    internal static class InputParser
    {
        private static SectionIdRange ParseRange(string x)
        {
            var values = x.Split('-').Select(Int32.Parse).ToList();
            if (values.Count != 2) throw new ArgumentException($"Invalid range input {x}");
            return new SectionIdRange(values[0], values[1]);
        }
        
        public static Tuple<SectionIdRange, SectionIdRange> ParseLine(string x)
        {
            var values = x.Split(',').Select(ParseRange).ToList();
            if (values.Count != 2) throw new ArgumentException($"Invalid line input {x}");
            return new Tuple<SectionIdRange, SectionIdRange>(values[0], values[1]);
        }
    }
    
    public class Day04Puzzle01 : IPuzzle
    {
        public string Solve(IEnumerable<string> input)
        {
            var wrappingLines = input
                .Select(InputParser.ParseLine)
                .Count(x => x.HasWrappingRange());
            
            return wrappingLines.ToString();
        }
    }
    
    public class Day04Puzzle02 : IPuzzle
    {
        public string Solve(IEnumerable<string> input)
        {
            var overlappingLines = input
                .Select(InputParser.ParseLine)
                .Count(x => x.HasOverlappingRanges());
            
            return overlappingLines.ToString();
        }
    }    
}