using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class KindTitForTat() : IStrategy
    {
        public string Name { get; private set; } = nameof(KindTitForTat);

        public bool Egotistical { get; private set; } = false;

        private readonly Random Randomizer = new();

        public GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            var lastOpponentAction = opponentActions.LastOrDefault();
            return lastOpponentAction?.Action != GameAction.Defect
                ? GameAction.Cooperate
                : (GameAction)Randomizer.Next(2);
        }
    }
}
