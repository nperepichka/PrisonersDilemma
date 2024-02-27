using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class TwoTitsForTat() : Strategy
    {
        public override bool Nice => true;

        public override bool Selfish => true;

        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step)
        {
            var lastOpponentAction1 = opponentActions.LastOrDefault();
            var lastOpponentAction2 = GetLastItem(opponentActions, 2);
            return lastOpponentAction1?.Action == GameAction.Defect || lastOpponentAction2?.Action == GameAction.Defect
                ? GameAction.Defect
                : GameAction.Cooperate;
        }
    }
}
