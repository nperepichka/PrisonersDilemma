using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class AllRandom() : IStrategy
    {
        public string Name { get; private set; } = nameof(AllRandom);

        public bool Egotistical { get; private set; } = true;

        private readonly Random Randomizer = new();

        public GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            return (GameAction)Randomizer.Next(2);
        }
    }
}
