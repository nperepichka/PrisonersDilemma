using Gameplay.Constructs;
using Gameplay.Enums;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class Grofman() : Strategy
    {
        public override bool Nice => true;

        public override GameAction DoAction(ActionParams actionParams)
        {
            var lastOwnAction = actionParams.OwnActions.LastOrDefault();
            var lastOpponentAction = actionParams.OpponentActions.LastOrDefault();
            return actionParams.Step == 1 || lastOwnAction?.Action == lastOpponentAction?.Action
                ? GameAction.Cooperate
                : (Randomizer.Next(7) < 2 ? GameAction.Cooperate : GameAction.Defect);
        }
    }
}
