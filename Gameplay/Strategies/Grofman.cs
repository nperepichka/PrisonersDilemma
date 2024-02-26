using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class Grofman() : Strategy
    {
        public override bool Nice => true;

        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step)
        {
            var lastOwnAction = ownActions.LastOrDefault();
            var lastOpponentAction = opponentActions.LastOrDefault();
            return step == 1 || lastOwnAction?.Action == lastOpponentAction?.Action
                ? GameAction.Cooperate
                : (Randomizer.Next(7) < 2 ? GameAction.Cooperate : GameAction.Defect);
        }
    }
}
