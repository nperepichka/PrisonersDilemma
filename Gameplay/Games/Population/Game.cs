using Gameplay.Constructs;
using Gameplay.Games.Helpers;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Population
{
    internal class Game(bool flexible)
    {
        private const string TableFormat = "{0,10}{1,8}{2,8}{3,27}{4,7}";

        private Options Options { get; set; } = new Options(flexible);

        private IEnumerable<IStrategy> Strategies { get; set; }

        private int Step { get; set; } = 0;

        private string StateSnapshot { get; set; } = null;

        private int SameStateSnapshot { get; set; } = 1;

        private bool WriteScores(List<History> actions)
        {
            var score = Strategies.Select(s => new
            {
                s.Name,
                s.Selfish,
                s.Nice,
                Strategies = actions
                    .SelectMany(_ => new[] {
                        _.Strategy1Name == s.Name ? _.Strategy1Id : Guid.Empty,
                        _.Strategy2Name == s.Name ? _.Strategy2Id : Guid.Empty
                    })
                    .Where(_ => _ != Guid.Empty)
                    .Distinct(),
                Actions = actions.Where(_ => _.ContainsStrategy(s.Name)).ToArray(),
            }).Select(s => new
            {
                s.Name,
                s.Selfish,
                s.Nice,
                Count = s.Strategies.Count(),
                Score = s.Strategies.Sum(ss => s.Actions
                    .Where(a => a.ContainsStrategy(ss))
                    .Select(a => a.GetStrategyLastScore(ss))
                    .Sum()),
                Total = s.Strategies.Sum(ss => s.Actions
                    .Where(a => a.ContainsStrategy(ss))
                    .Select(a => a.GetScoresSum(ss))
                    .Sum()),
            }).OrderByDescending(_ => _.Score)
            .ToArray();

            Console.WriteLine($"Step: {Step}");
            Console.WriteLine(string.Format(TableFormat, "Total", "Score", "Count", "Name", "Flags"));
            Console.WriteLine("-------------------------------------------------------------");

            foreach (var s in score)
            {
                var selfishFlag = s.Selfish ? "S" : "";
                var niceFlag = s.Nice ? "N" : "";
                var flagsStr = $"{niceFlag,2}{selfishFlag,2}";
                Console.WriteLine(string.Format(TableFormat, $"{s.Total:0.00}", $"{s.Score:0.00}", s.Count, s.Name, flagsStr));
            }

            Console.WriteLine();

            if (Step > Options.StabilizationSteps)
            {
                var stateSnapshot = string.Join("|", score.Where(_ => _.Score > 0).Select(_ => $"{_.Name}:{_.Score:0.00}"));
                if (stateSnapshot == StateSnapshot)
                {
                    SameStateSnapshot++;
                    if (SameStateSnapshot >= Options.SamePopulationStepsToStop && Step >= Options.MinSteps)
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

            return false;

            // TODO: why score of ESS is count*C ?
        }

        public void RunGame()
        {
            Console.WriteLine($"Flexible: {Options.f:0.00}   Seed: {Options.Seed:0.00}");
            Console.WriteLine();

            var gameStrategies = StrategiesBuilder.GetAllStrategies();
            Strategies = gameStrategies.DistinctBy(_ => _.Name);

            List<History> actions = null;

            while (true)
            {
                Step++;

                // TODO: rewrite this logic - on each step Tournament should be playead for each pair

                var gameField = actions == null
                    ? new GameField(Options, Step, gameStrategies)
                    : new GameField(Options, Step, gameStrategies, actions);
                gameField.DoStep();

                actions = gameField.Actions;
                var shouldStop = WriteScores(actions);

                if (shouldStop)
                {
                    break;
                }
            }
        }
    }
}
