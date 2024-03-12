using Gameplay.Enums;
using Gameplay.Games.Tournament.Constructs;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class Tullock() : Strategy
    {
        public override bool Selfish => true;

        public override GameAction DoAction(ActionParams actionParams)
        {
            if (actionParams.Step <= 11)
            {
                return GameAction.Cooperate;
            }

            var opponentCooperated = actionParams.OpponentActions.TakeLast(10).Count(_ => _.Action == GameAction.Cooperate);
            return Randomizer.Next(10) < opponentCooperated - 1 ? GameAction.Cooperate : GameAction.Defect;
        }
    }
}
