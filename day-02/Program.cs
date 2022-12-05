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

    public class GameRules
    {
        public static GameResult GetResult(ShapePair x)
        {
            if (x.Player == x.Opponent)
            {
                return GameResult.Draw;
            }

            // TODO ternary
            if (Shapes.Exists(y => y.Item1 == x.Player && y.Item2 == x.Opponent))
            {
                return GameResult.PlayerWon;
            }

            return GameResult.OpponentWon;
        }

        public static Shape GetTargetShape(Shape opponent, GameResult target)
        {
            if (target == GameResult.Draw)
            {
                return opponent;
            }

            if (target == GameResult.PlayerWon)
            {
                return Shapes.Find(x => x.Item2 == opponent).Item1;
            }

            return Shapes.Find(x => x.Item1 == opponent).Item2;
        }

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
        public Tournament()
        {
            _games = new List<Game>();
        }

        public void PlayGame(ShapePair x)
        {
            _games.Add(new Game(x));
        }

        public void PlayGame2(ShapeAndTarget x)
        {
            var playerShape = GameRules.GetTargetShape(x.Opponent, x.Target);
            _games.Add(new Game(new ShapePair(){ Player = playerShape, Opponent = x.Opponent}));
        }
        
        public int GetPlayerScore() {
            return _games.Sum(x => x.GetPlayerPoints());
        }
        
        private Shape GetRequiredShape(Shape opponent, GameResult targetResult)
        {
            return Shape.Paper;
        }
        
        private List<Game> _games;
    }

    class Day02
    {
        static void Main(string[] args)
        {
            var tournament = new Tournament();
            var tournament2 = new Tournament();
            
            foreach (string line in System.IO.File.ReadLines("input.txt"))
            {
                var (opponentShape, playerShape) = InputParser.GetInputArgumentsFromString(line);
                ShapePair x = new ShapePair()
                {
                    Player = InputParser.GetShapeFromString(playerShape),
                    Opponent = InputParser.GetShapeFromString(opponentShape),
                };

                tournament.PlayGame(x);
                
                var (_, targetResult) = InputParser.GetInputArgumentsFromString(line);
                ShapeAndTarget y = new ShapeAndTarget()
                {
                    Opponent = InputParser.GetShapeFromString(opponentShape),
                    Target = InputParser.GetTargetResultFromString(targetResult),
                };
                    
                tournament2.PlayGame2(y);
            }

            Console.WriteLine(tournament.GetPlayerScore());
            Console.WriteLine(tournament2.GetPlayerScore());
        }
    }
}