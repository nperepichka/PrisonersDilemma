using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class AfraidTitForTat() : Strategy
    {
        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            var lastOpponentAction1 = opponentActions.LastOrDefault();
            var lastOpponentAction2 = GetLastItem(opponentActions, 2);
            var lastOpponentAction3 = GetLastItem(opponentActions, 3);

            return lastOpponentAction1?.Action == GameAction.Defect
                ? (lastOpponentAction2?.Action == GameAction.Defect && lastOpponentAction3?.Action == GameAction.Defect ? GameAction.Cooperate : GameAction.Defect)
                : GameAction.Cooperate;
        }
    }
}
