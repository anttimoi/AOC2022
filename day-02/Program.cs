// See https://aka.ms/new-console-template for more information
using System.Linq;

namespace day_02
{
    public enum Shape
    {
        Rock,
        Paper,
        Scissors,
    }

    public enum GameResult
    {
        PlayerWon,
        OpponentWon,
        Draw,
    }

    public class InputParser
    {
        public static Shape GetShapeFromString(string x) =>
            x switch
            {
                "A" => Shape.Rock,
                "B" => Shape.Paper,
                "C" => Shape.Scissors,
                "X" => Shape.Rock,
                "Y" => Shape.Paper,
                "Z" => Shape.Scissors,
                _ => throw new ArgumentException("Invalid input")
            };

        public static GameResult GetTargetResultFromString(string x) =>
            x switch
            {
                "X" => GameResult.OpponentWon,
                "Y" => GameResult.Draw,
                "Z" => GameResult.PlayerWon,
                _ => throw new ArgumentException("Invalid input")
            };

        public static Tuple<string, string> GetInputArgumentsFromString(string x)
        {
            var stringShapes = x.Split(" ");
            if (stringShapes.Length != 2)
            {
                throw new ArgumentException("Invalid input");
            }

            return new Tuple<string, string>(stringShapes[0], stringShapes[1]);
        }        
    }

    public class ShapePair
    {
        public Shape Player { get; set; }
        public Shape Opponent { get; set; }
    }

    public class ShapeAndTarget
    {
        public Shape Opponent { get; set; }
        public GameResult Target { get; set; }
    }

    public static class GameRules
    {
        public static GameResult GetResult(ShapePair x)
        {
            if (x.Player == x.Opponent)
            {
                return GameResult.Draw;
            }
            
            return Shapes.Exists(y => y.Item1 == x.Player && y.Item2 == x.Opponent)
                ? GameResult.PlayerWon
                : GameResult.OpponentWon;
        }

        public static Shape GetTargetShape(Shape opponent, GameResult target) =>
            target switch
            {
                GameResult.Draw => opponent,
                GameResult.PlayerWon => Shapes.Find(x => x.Item2 == opponent).Item1,
                GameResult.OpponentWon => Shapes.Find(x => x.Item1 == opponent).Item2,
                _ => throw new ArgumentException("Unexpected target")
            };
        
        // First item beats second
        private static readonly List<Tuple<Shape, Shape>> Shapes = new List<Tuple<Shape, Shape>>()
        {
            new Tuple<Shape, Shape>(Shape.Rock, Shape.Scissors),
            new Tuple<Shape, Shape>(Shape.Paper, Shape.Rock),
            new Tuple<Shape, Shape>(Shape.Scissors, Shape.Paper),
        };
    }
    
    public class Game
    {
        public Shape PlayerShape { get; }
        public Shape OpponentShape { get; }

        public Game(ShapePair x)
        {
            PlayerShape = x.Player;
            OpponentShape = x.Opponent;
        }
        
        public GameResult GetResult()
        {
            return GameRules.GetResult(new ShapePair() { Player = PlayerShape, Opponent = OpponentShape });
        }

        public int GetPlayerPoints()
        {
            return MapGameResultToPoints(GetResult()) + MapShapeToPoints(PlayerShape);
        }
        
        private static int MapGameResultToPoints(GameResult result) =>
            result switch
            {
                GameResult.PlayerWon => 6,
                GameResult.Draw => 3,
                GameResult.OpponentWon => 0,
            };

        private static int MapShapeToPoints(Shape shape) =>
            shape switch
            {
                Shape.Rock => 1,
                Shape.Paper => 2,
                Shape.Scissors => 3,
            };

    }
    
    class Tournament
    {
        public Tournament(ITournamentStrategy strategy)
        {
            _games = new List<Game>();
            _strategy = strategy;
        }

        public void PlayGame(string x)
        {
            _games.Add(new Game(_strategy.GetShapePairFromString(x)));
        }
        
        public int GetPlayerScore() {
            return _games.Sum(x => x.GetPlayerPoints());
        }
        
        private Shape GetRequiredShape(Shape opponent, GameResult targetResult)
        {
            return Shape.Paper;
        }
        
        private List<Game> _games;
        private ITournamentStrategy _strategy;
    }

    interface ITournamentStrategy
    {
        ShapePair GetShapePairFromString(string x);
    }
    
    class SelectedShapeStrategy : ITournamentStrategy
    {
        public ShapePair GetShapePairFromString(string x)
        {
            var (opponentShape, playerShape) = InputParser.GetInputArgumentsFromString(x);
            return new ShapePair()
            {
                Player = InputParser.GetShapeFromString(playerShape),
                Opponent = InputParser.GetShapeFromString(opponentShape),
            };
        }
    }

    class TargetResultStrategy : ITournamentStrategy
    {
        public ShapePair GetShapePairFromString(string x)
        {
            var (opponentString, targetResultString) = InputParser.GetInputArgumentsFromString(x);
            var opponent = InputParser.GetShapeFromString(opponentString);
            var player = GameRules.GetTargetShape(
                opponent,
                InputParser.GetTargetResultFromString(targetResultString)
            );
            return new ShapePair(){ Player = player, Opponent = opponent};
        }
    }
    
    class Day02
    {
        static void Main(string[] args)
        {
            var tournament = new Tournament(new SelectedShapeStrategy());
            var tournament2 = new Tournament(new TargetResultStrategy());
            
            foreach (string line in System.IO.File.ReadLines("input.txt"))
            {
                tournament.PlayGame(line);
                tournament2.PlayGame(line);
            }

            Console.WriteLine(tournament.GetPlayerScore());
            Console.WriteLine(tournament2.GetPlayerScore());
        }
    }
}