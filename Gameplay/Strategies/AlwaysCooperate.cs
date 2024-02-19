using Gameplay.Constructs;
using Gameplay.Constructs.Enums;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class AlwaysCooperate() : IStrategy
    {
        public string Name { get; private set; } = nameof(AlwaysCooperate);

        public bool Egotistical { get; private set; } = false;

        public GameAction DoAction(List<ActionsHistoryItem> ownActions, List<ActionsHistoryItem> opponentActions, int step)
        {
            return GameAction.Cooperate;
        }
    }
}
