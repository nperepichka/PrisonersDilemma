using Gameplay.Games.Helpers;
using Gameplay.Strategies;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Population
{
    internal class Game(bool flexible)
    {
        private const string TableFormat = "{0,6}{1,27}{2,7}";

        private Options Options { get; set; } = new Options(flexible);

        private IEnumerable<IStrategy> Strategies { get; set; }

        private string StateSnapshot { get; set; } = null;

        private int SameStateSnapshot { get; set; } = 1;

        private bool WriteScores(IList<IStrategy> strategies, int step)
        {
            var score = Strategies.Select(s => new
            {
                s.Name,
                s.Selfish,
                s.Nice,
                Count = strategies.Count(_ => _.Name == s.Name),
            }).OrderByDescending(_ => _.Count)
            .ToArray();

            Console.WriteLine($"Step: {step}");
            Console.WriteLine(string.Format(TableFormat, "Count", "Name", "Flags"));
            Console.WriteLine("-----------------------------------------");

            foreach (var s in score)
            {
                var selfishFlag = s.Selfish ? "S" : "";
                var niceFlag = s.Nice ? "N" : "";
                var flagsStr = $"{niceFlag,2}{selfishFlag,2}";
                Console.WriteLine(string.Format(TableFormat, s.Count, s.Name, flagsStr));
            }

            Console.WriteLine();

            if (Options.SamePopulationStepsToStop > 0)
            {
                var stateSnapshot = string.Join("|", score.Where(_ => _.Count > 0).Select(_ => $"{_.Name}:{_.Count}"));
                if (stateSnapshot == StateSnapshot)
                {
                    SameStateSnapshot++;
                    if (SameStateSnapshot == Options.SamePopulationStepsToStop)
                    {
                        return true;
                    }
                }
                else
                {
                    StateSnapshot = stateSnapshot;
                    SameStateSnapshot = 1;
                }
            }
            else
            {
                var differentStrategiesCount = score.Count(_ => _.Count > 0);
                if (differentStrategiesCount == 1)
                {
                    return true;
                }
            }

            return false;
        }

        public void RunGame()
        {
            Console.WriteLine($"Flexible: {Options.f:0.00}   Seed: {Options.Seed:0.00}   Mutation: {Options.Mutation:0.00}");
            Console.WriteLine();

            var gameStrategies = StrategiesBuilder.GetStrategies<Smart>();
            Strategies = gameStrategies.DistinctBy(_ => _.Name);

            var step = 0;
            var shouldStop = WriteScores(gameStrategies, step);

            var gameField = new GameField(Options, gameStrategies);
            while (!shouldStop)
            {
                step++;
                gameField.DoStep();
                shouldStop = WriteScores(gameField.Strategies, step);
            }
        }
    }
}
