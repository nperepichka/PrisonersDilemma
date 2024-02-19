using Gameplay.Constructs;
using Gameplay.Constructs.Enums;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class CooperateTillDefect() : IStrategy
    {
        // Friedman

        public string Name { get; private set; } = nameof(CooperateTillDefect);

        public bool Egotistical { get; private set; } = true;

        public GameAction DoAction(List<ActionsHistoryItem> ownActions, List<ActionsHistoryItem> opponentActions, int step)
        {
            return opponentActions.Any(_ => _.Action == GameAction.Defect) ? GameAction.Defect : GameAction.Cooperate;
        }
    }
}
