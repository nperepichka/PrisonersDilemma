using Gameplay.Enums;
using Gameplay.Games.Tournament.Constructs;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class AllRandom() : Strategy
    {
        public override bool Selfish => true;

        public override GameAction DoAction(ActionParams actionParams)
        {
            return (GameAction)Randomizer.Next(2);
        }
    }
}
