using Gameplay.Constructs;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies.Helpers
{
    internal static class StrategiesBuilder
    {
        private static List<IStrategy> GetAllStrategies() => [
            new Smart(),
            new AlwaysCooperate(),
            new AlwaysDefect(),
            new TitForTat(),
            new AllRandom(),
            new AllReverse(),
            new Friedman(),
            new TitForTwoTats(),
            new TwoTitsForTat(),
            new TitForTatButCanDefect(),
            new TitForTatButCanCooperate(),
            new KindTitForTat(),
            new Grofman(),
            new Tullock(),
            new Graaskamp(),
            new Downing(),
        ];

        public static List<IStrategy> GetStrategies(Options options)
        {
            var allStrategies = GetAllStrategies();
            List<IStrategy> strategies = [];

            if (options.Strategies != null && options.Strategies.Length != 0)
            {
                foreach (var strategyName in options.Strategies)
                {
                    var strategy = allStrategies.FirstOrDefault(_ => _.Name == strategyName)
                        ?? throw new ArgumentException($"Unknown strategy: {strategyName}");
                    var clone = strategy.Clone();
                    strategies.Add(clone);
                }
            }
            else
            {
                strategies = allStrategies;
            }

            if (options.GameType == Enums.GameType.Population)
            {
                var strategiesSet = strategies;
                strategies = [];

                for (var i = 1; i <= options.BasePopulation; i++)
                {
                    strategies.AddRange(strategiesSet.Select(_ => _.Clone()));
                }

                if (!string.IsNullOrEmpty(options.DominationStrategy))
                {
                    var strategy = allStrategies.FirstOrDefault(_ => _.Name == options.DominationStrategy)
                        ?? throw new ArgumentException($"Unknown strategy: {options.DominationStrategy}");
                    while (strategies.Count(_ => _.Name == options.DominationStrategy) * options.DominationStrategyCoef < strategies.Count(_ => _.Name != options.DominationStrategy))
                    {
                        var clone = strategy.Clone();
                        strategies.Add(clone);
                    }
                }
            }

            return strategies;
        }
    }
}
