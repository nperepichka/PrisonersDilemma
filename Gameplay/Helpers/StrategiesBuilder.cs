using Gameplay.Strategies;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Helpers
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

        public static IStrategy[] GetStrategies<TDominationStrategy>() where TDominationStrategy : IStrategy
        {
            var strategies = GetAllStrategies().ToList();
            while (strategies.Count(_ => _ is TDominationStrategy) < strategies.Count(_ => _ is not TDominationStrategy))
            {
                var clone = strategies.First(_ => _ is TDominationStrategy).Clone();
                strategies.Add(clone);
            }
            return [.. strategies];
        }
    }
}
