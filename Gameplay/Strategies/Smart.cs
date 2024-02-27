using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    /// <summary>
    /// The first 10 - cooperate.
    /// If opponent always cooperated - defect.
    /// If opponent always defected - defect.
    /// If opponent defected * 3 < steps - cooperate
    /// Otherwise, defect.
    /// </summary>
    internal class Smart() : Strategy
    {
        public override bool Selfish => true;

        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step)
        {
            var defectsCount = opponentActions.Count(_ => _.Action == GameAction.Defect);
            return step <= 10 || defectsCount > 0 && defectsCount * 3 < step
                ? GameAction.Cooperate
                : GameAction.Defect;
        }
    }
}
