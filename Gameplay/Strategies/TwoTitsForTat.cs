using Gameplay.Constructs;
using Gameplay.Constructs.Enums;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class TwoTitsForTat() : IStrategy
    {
        public string Name { get; private set; } = nameof(TwoTitsForTat);

        public bool Egotistical { get; private set; } = true;

        public GameAction DoAction(List<ActionsHistoryItem> ownActions, List<ActionsHistoryItem> opponentActions, int step)
        {
            var lastOpponentAction1 = opponentActions.LastOrDefault();
            var lastOpponentAction2 = opponentActions.ElementAtOrDefault(step - 3);
            return lastOpponentAction1?.Action == GameAction.Defect || lastOpponentAction2?.Action == GameAction.Defect ? GameAction.Defect : GameAction.Cooperate;
        }
    }
}
