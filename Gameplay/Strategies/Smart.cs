using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    /// <summary>
    /// The first 5 - cooperate.
    /// If opponent always cooperated - defect.
    /// If opponent always defected - defect.
    /// If opponent defected * 3 < steps - cooperate
    /// Otherwise, defect.
    /// </summary>
    internal class Smart() : Strategy
    {
        public override bool Egotistical => true;

        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            var defectsCount = opponentActions.Count(_ => _.Action == GameAction.Defect);
            return step <= 5 || defectsCount > 0 && defectsCount * 3 < step
                ? GameAction.Cooperate
                : GameAction.Defect;
        }
    }
}
