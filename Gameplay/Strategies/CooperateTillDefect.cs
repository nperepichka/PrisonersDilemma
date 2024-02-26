using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    /// <summary>
    /// Friedman
    /// </summary>
    internal class CooperateTillDefect() : Strategy
    {
        public override bool Egotistical => true;

        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            return opponentActions.Any(_ => _.Action == GameAction.Defect) ? GameAction.Defect : GameAction.Cooperate;
        }
    }
}
