using Gameplay.Constructs;
using Gameplay.Enums;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class AlwaysDefect() : Strategy
    {
        public override bool Selfish => true;

        public override GameAction DoAction(ActionParams actionParams)
        {
            return GameAction.Defect;
        }
    }
}
