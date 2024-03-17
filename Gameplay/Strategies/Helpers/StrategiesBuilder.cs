using Gameplay.Constructs;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies.Helpers
{
    internal static class StrategiesBuilder
    {
        public static IList<IStrategy> GetAllStrategies() => [
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
            new Smart(),
        ];

        public static IList<IStrategy> GetStrategies(Options options)
        {
            var allStrategies = GetAllStrategies();
            IList<IStrategy> strategies = [];

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

            if (!string.IsNullOrEmpty(options.DominationStrategy))
            {
                var strategy = allStrategies.FirstOrDefault(_ => _.Name == options.DominationStrategy)
                    ?? throw new ArgumentException($"Unknown strategy: {options.DominationStrategy}");
                while (strategies.Count(_ => _.Name == options.DominationStrategy) < strategies.Count(_ => _.Name != options.DominationStrategy))
                {
                    var clone = strategy.Clone();
                    strategies.Add(clone);
                }
            }

            return strategies;
        }
    }
}
