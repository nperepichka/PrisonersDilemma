using Gameplay.Constructs;
using Gameplay.Strategies;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Helpers
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

                for (var i = 1; i <= options.BasePopulationMultiplicity; i++)
                {
                    strategies.AddRange(strategiesSet.Select(_ => _.Clone()));
                }

                if (!string.IsNullOrEmpty(options.DominantStrategy))
                {
                    var strategy = allStrategies.FirstOrDefault(_ => _.Name == options.DominantStrategy)
                        ?? throw new ArgumentException($"Unknown strategy: {options.DominantStrategy}");
                    for (var i = 1; i <= options.BasePopulationMultiplicity; i++)
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
