using Gameplay.Constructs;
using Gameplay.Constructs.Enums;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class AlwaysDefect() : IStrategy
    {
        public string Name { get; private set; } = nameof(AlwaysDefect);

        public bool Egotistical { get; private set; } = true;

        public GameAction DoAction(List<ActionsHistoryItem> ownActions, List<ActionsHistoryItem> opponentActions, int step)
        {
            return GameAction.Defect;
        }
    }
}
