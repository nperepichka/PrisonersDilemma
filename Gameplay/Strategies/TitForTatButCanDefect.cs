using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    /// <summary>
    /// Almost Joss
    /// </summary>
    internal class TitForTatButCanDefect() : Strategy
    {
        public override bool Egotistical => true;

        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            var lastOwnAction = ownActions.LastOrDefault();
            var lastOpponentAction = opponentActions.LastOrDefault();
            return lastOpponentAction?.Action == GameAction.Defect
                ? GameAction.Defect
                : ((lastOwnAction?.Action == GameAction.Cooperate && Randomizer.Next(10) == 0) ? GameAction.Defect : GameAction.Cooperate);
        }
    }
}
