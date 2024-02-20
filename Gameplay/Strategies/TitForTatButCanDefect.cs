using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class TitForTatButCanDefect() : IStrategy
    {
        // Almost Joss

        public string Name { get; private set; } = nameof(TitForTatButCanDefect);

        public bool Egotistical { get; private set; } = true;

        private readonly Random Randomizer = new();

        public GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            var lastOwnAction = ownActions.LastOrDefault();
            var lastOpponentAction = opponentActions.LastOrDefault();
            return lastOpponentAction?.Action == GameAction.Defect
                ? GameAction.Defect
                : ((lastOwnAction?.Action == GameAction.Cooperate && Randomizer.Next(10) == 0) ? GameAction.Defect : GameAction.Cooperate);
        }
    }
}
