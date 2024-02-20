using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class CooperateTillDefect() : IStrategy
    {
        // Friedman

        public string Name { get; private set; } = nameof(CooperateTillDefect);

        public bool Egotistical { get; private set; } = true;

        public GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            return opponentActions.Any(_ => _.Action == GameAction.Defect) ? GameAction.Defect : GameAction.Cooperate;
        }
    }
}
