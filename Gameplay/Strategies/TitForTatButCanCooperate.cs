using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class TitForTatButCanCooperate() : Strategy
    {
        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            var lastOwnAction = ownActions.LastOrDefault();
            var lastOpponentAction = opponentActions.LastOrDefault();
            return lastOpponentAction?.Action == GameAction.Defect
                ? ((lastOwnAction?.Action == GameAction.Defect && Randomizer.Next(10) == 0) ? GameAction.Cooperate : GameAction.Defect)
                : GameAction.Cooperate;
        }
    }
}
