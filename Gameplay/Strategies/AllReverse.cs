using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class AllReverse() : Strategy
    {
        public override bool Selfish => true;

        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step)
        {
            var lastOpponentAction = opponentActions.LastOrDefault();
            return lastOpponentAction?.Action == GameAction.Cooperate ? GameAction.Defect : GameAction.Cooperate;
        }
    }
}
