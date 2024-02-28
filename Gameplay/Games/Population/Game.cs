using Gameplay.Constructs;
using Gameplay.Games.Helpers;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Population
{
    internal class Game(bool flexible)
    {
        private const string TableFormat = "{0,10}{1,10}{2,8}{3,27}{4,7}";

        private Options Options { get; set; } = new Options(flexible);

        private IEnumerable<IStrategy> Strategies { get; set; }

        private int Step { get; set; } = 0;

        private int WriteScores(List<History> actions)
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
                Actions = actions.Where(_ => _.ContainsStrategy(s.Name)).ToList(),
            }).Select(s => new
            {
                s.Name,
                s.Selfish,
                s.Nice,
                Count = s.Strategies.Count(),
                Score = s.Strategies.Sum(ss => s.Actions
                    .Where(a => a.ContainsStrategy(ss))
                    .Sum(_ => _.GetStrategyLastScore(ss))
                ),
                Children = s.Strategies.Sum(ss => (int)s.Actions
                    .Where(a => a.ContainsStrategy(ss))
                    .Select(a => a.GetStrategyLastScore(ss))
                    .Average()
                ),
            }).OrderByDescending(_ => _.Children);

            var population = score.Sum(s => s.Children);

            Console.WriteLine($"Step: {Step}   Population: {population}");
            Console.WriteLine(string.Format(TableFormat, "Score", "Children", "Count", "Name", "Flags"));
            Console.WriteLine("---------------------------------------------------------------");

            foreach (var s in score)
            {
                var selfishFlag = s.Selfish ? "S" : "";
                var niceFlag = s.Nice ? "N" : "";
                var flagsStr = $"{niceFlag,2}{selfishFlag,2}";
                Console.WriteLine(string.Format(TableFormat, $"{s.Score:0.00}", s.Children, s.Count, s.Name, flagsStr));
            }

            Console.WriteLine();
            return population;
        }

        public void RunGame()
        {
            Console.WriteLine($"Flexible: {Options.f:0.00}   Seed: {Options.Seed:0.00}");
            Console.WriteLine();

            var gameStrategies = StrategiesBuilder.GetAllStrategies();
            Strategies = gameStrategies.DistinctBy(_ => _.Name);

            var population = Strategies.Count();
            List<History> actions = null;

            while (population < Options.MaxPopulation)
            {
                Step++;

                var gameField = actions == null
                    ? new GameField(Options, Step, gameStrategies)
                    : new GameField(Options, Step, gameStrategies, actions);
                gameField.DoStep();

                actions = gameField.Actions;
                population = WriteScores(actions);
            }
        }
    }
}
