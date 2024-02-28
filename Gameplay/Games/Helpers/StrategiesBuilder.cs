using Gameplay.Strategies;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Helpers
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
            new Smart()
        ];
    }
}
