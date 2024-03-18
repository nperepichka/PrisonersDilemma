using Gameplay.Enums;
using Gameplay.Games.Tournament.Constructs;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class AllReverse() : Strategy
    {
        public override bool Selfish => true;

        public override GameAction DoAction(ActionParams actionParams)
        {
            var lastOpponentAction = actionParams.OpponentActions.LastOrDefault();
            return lastOpponentAction?.Action == GameAction.Defect ? GameAction.Cooperate : GameAction.Defect;
        }
    }
}
