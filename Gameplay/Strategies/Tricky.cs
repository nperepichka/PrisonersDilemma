using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    /// <summary>
    /// The first 5 - cooperate.
    /// If defected just before - defect until the opponent defect.
    /// If cooperated last 3 times - defect with a probability.
    /// Otherwise, cooperate.
    /// </summary>
    internal class Tricky() : Strategy
    {
        public override bool Egotistical => true;

        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            if (step <= 5)
            {
                return GameAction.Cooperate;
            }

            var lastOwnAction1 = ownActions.LastOrDefault();
            var lastOpponentAction = opponentActions.LastOrDefault();

            if (lastOwnAction1?.Action == GameAction.Defect)
            {
                return lastOpponentAction?.Action == GameAction.Defect ? GameAction.Cooperate : GameAction.Defect;
            }

            var lastOwnAction2 = GetLastItem(ownActions, 2);
            var lastOwnAction3 = GetLastItem(ownActions, 3);

            if (
                lastOwnAction1?.Action == GameAction.Cooperate
                && lastOwnAction2?.Action == GameAction.Cooperate
                && lastOwnAction3?.Action == GameAction.Cooperate
                )
            {
                return (GameAction)Randomizer.Next(2);
            }

            return GameAction.Cooperate;
        }
    }
}
