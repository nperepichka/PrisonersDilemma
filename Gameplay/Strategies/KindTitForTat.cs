using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class KindTitForTat() : Strategy
    {
        public override bool Nice => true;

        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step)
        {
            var lastOpponentAction = opponentActions.LastOrDefault();
            return lastOpponentAction?.Action != GameAction.Defect
                ? GameAction.Cooperate
                : (GameAction)Randomizer.Next(2);
        }
    }
}
