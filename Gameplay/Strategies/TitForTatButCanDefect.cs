using Gameplay.Enums;
using Gameplay.Games.Tournament.Constructs;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    /// <summary>
    /// Almost Joss
    /// </summary>
    internal class TitForTatButCanDefect() : Strategy
    {
        public override bool Selfish => true;

        public override GameAction DoAction(ActionParams actionParams)
        {
            var lastOwnAction = actionParams.OwnActions.LastOrDefault();
            var lastOpponentAction = actionParams.OpponentActions.LastOrDefault();
            return lastOpponentAction?.Action == GameAction.Defect
                ? GameAction.Defect
                : ((lastOwnAction?.Action == GameAction.Cooperate && Randomizer.Next(10) == 0) ? GameAction.Defect : GameAction.Cooperate);
        }
    }
}
