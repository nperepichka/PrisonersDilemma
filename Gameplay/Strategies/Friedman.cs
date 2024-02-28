using Gameplay.Constructs;
using Gameplay.Enums;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class Friedman() : Strategy
    {
        public override bool Nice => true;

        public override bool Selfish => true;

        public override GameAction DoAction(ActionParams actionParams)
        {
            return actionParams.OpponentActions.Any(_ => _.Action == GameAction.Defect) ? GameAction.Defect : GameAction.Cooperate;
        }
    }
}
