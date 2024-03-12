using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Abstracts
{
    internal abstract class GameField<TOptions> where TOptions : Options
    {
        public GameField(TOptions options, IList<IStrategy> strategies)
        {
            Options = options;
            Strategies = [];
            AddStrategies(strategies);
        }

        protected readonly Random Randomizer = new();

        protected TOptions Options { get; private set; }

        public List<IStrategy> Strategies { get; protected set; }

        protected abstract void AddStrategies(IList<IStrategy> strategies);
    }
}
