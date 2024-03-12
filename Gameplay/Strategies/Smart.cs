using Gameplay.Enums;
using Gameplay.Games.Tournament.Constructs;
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

        public override GameAction DoAction(ActionParams actionParams)
        {
            var defectsCount = actionParams.OpponentActions.Count(_ => _.Action == GameAction.Defect);
            return actionParams.Step <= 10 || defectsCount > 0 && defectsCount * 3 < actionParams.Step
                ? GameAction.Cooperate
                : GameAction.Defect;
        }
    }
}
