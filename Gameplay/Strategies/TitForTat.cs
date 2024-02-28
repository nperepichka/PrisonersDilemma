using Gameplay.Constructs;
using Gameplay.Enums;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class TitForTat() : Strategy
    {
        public override bool Nice => true;

        public override GameAction DoAction(ActionParams actionParams)
        {
            var lastOpponentAction = actionParams.OpponentActions.LastOrDefault();
            return lastOpponentAction?.Action ?? GameAction.Cooperate;
        }
    }
}
