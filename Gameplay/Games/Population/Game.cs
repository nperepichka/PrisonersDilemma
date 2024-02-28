using Gameplay.Games.Helpers;
using Gameplay.Strategies;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Population
{
    internal class Game(bool flexible)
    {
        private Options Options { get; set; } = new Options(flexible);

        public void RunGame()
        {
            Console.WriteLine($"Flexible: {Options.f:0.00}   Seed: {Options.Seed:0.00}");

            var gameStrategies = StrategiesBuilder.GetAllStrategies();
            var gameField = new GameField(Options, gameStrategies);

            gameField.DoStep();
        }
    }
}
