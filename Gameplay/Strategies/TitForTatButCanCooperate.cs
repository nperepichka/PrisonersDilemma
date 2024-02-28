using Gameplay.Constructs;
using Gameplay.Enums;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class TitForTatButCanCooperate() : Strategy
    {
        public override bool Nice => true;

        public override GameAction DoAction(ActionParams actionParams)
        {
            var lastOwnAction = actionParams.OwnActions.LastOrDefault();
            var lastOpponentAction = actionParams.OpponentActions.LastOrDefault();
            return lastOpponentAction?.Action == GameAction.Defect
                ? ((lastOwnAction?.Action == GameAction.Defect && Randomizer.Next(10) == 0) ? GameAction.Cooperate : GameAction.Defect)
                : GameAction.Cooperate;
        }
    }
}
