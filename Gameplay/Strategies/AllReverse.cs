using Gameplay.Constructs;
using Gameplay.Enums;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class AllReverse() : Strategy
    {
        public override bool Selfish => true;

        public override GameAction DoAction(ActionParams actionParams)
        {
            var lastOpponentAction = actionParams.OpponentActions.LastOrDefault();
            return lastOpponentAction?.Action == GameAction.Cooperate ? GameAction.Defect : GameAction.Cooperate;
        }
    }
}
