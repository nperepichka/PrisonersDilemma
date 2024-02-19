using Gameplay.Constructs;
using Gameplay.Constructs.Enums;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class TitForTat() : IStrategy
    {
        public string Name { get; private set; } = "! " + nameof(TitForTat);

        public bool Egotistical { get; private set; } = false;

        public GameAction DoAction(List<ActionsHistoryItem> ownActions, List<ActionsHistoryItem> opponentActions, int step)
        {
            var lastOpponentAction = opponentActions.LastOrDefault();
            return lastOpponentAction?.Action ?? GameAction.Cooperate;
        }
    }
}
