using Gameplay.Constructs;
using Gameplay.Enums;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class TwoTitsForTat() : Strategy
    {
        public override bool Nice => true;

        public override bool Selfish => true;

        public override GameAction DoAction(ActionParams actionParams)
        {
            var lastOpponentAction1 = actionParams.OpponentActions.LastOrDefault();
            var lastOpponentAction2 = actionParams.GetOpponentLastItem(2);
            return lastOpponentAction1?.Action == GameAction.Defect || lastOpponentAction2?.Action == GameAction.Defect
                ? GameAction.Defect
                : GameAction.Cooperate;
        }
    }
}
