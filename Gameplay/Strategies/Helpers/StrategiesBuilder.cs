using Gameplay.Constructs;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies.Helpers
{
    internal static class StrategiesBuilder
    {
        public static IStrategy[] GetAllStrategies() => [
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

        public static IStrategy[] GetStrategies(Options options)
        {
            var strategies = GetAllStrategies().ToList();
            if (!string.IsNullOrEmpty(options.DominationStrategy))
            {
                while (strategies.Count(_ => _.Name == options.DominationStrategy) < strategies.Count(_ => _.Name != options.DominationStrategy))
                {
                    var dominationStrategy = strategies.FirstOrDefault(_ => _.Name == options.DominationStrategy)
                        ?? throw new ArgumentException($"Unknown strategy: {options.DominationStrategy}");
                    var clone = dominationStrategy.Clone();
                    strategies.Add(clone);
                }
            }
            return [.. strategies];
        }
    }
}
