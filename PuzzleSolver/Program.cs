using AOC2022.Core;
using AOC2022.Day01;
using AOC2022.Day02;
using AOC2022.Day03;
using AOC2022.Day04;

namespace AOC2022.PuzzleSolver
{
    using PuzzleWithInput = Tuple<IPuzzle, string>;
    
    static class PuzzleSolver
    {
        private static List<PuzzleWithInput> _puzzles = new List<PuzzleWithInput>()
        {
            new PuzzleWithInput(new Day01Puzzle01(), "1.txt"),
            new PuzzleWithInput(new Day01Puzzle02(), "1.txt"),
            new PuzzleWithInput(new Day02Puzzle01(), "2.txt"),
            new PuzzleWithInput(new Day02Puzzle02(), "2.txt"),
            new PuzzleWithInput(new Day03Puzzle01(), "3.txt"),
            new PuzzleWithInput(new Day03Puzzle02(), "3.txt"),
            new PuzzleWithInput(new Day04Puzzle01(), "4.txt"),
            new PuzzleWithInput(new Day04Puzzle02(), "4.txt"),   
        };
        
        private static IEnumerable<string> LoadInput(string filename)
        {
            return System.IO.File.ReadLines(filename);
        }

        public static void Solve()
        {
            foreach (var puzzle in _puzzles)
            {
                Console.WriteLine(puzzle.Item1.Solve(LoadInput(puzzle.Item2)));
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            PuzzleSolver.Solve();
        }
    }
}

