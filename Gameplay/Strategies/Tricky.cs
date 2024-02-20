using Gameplay.Constructs;
using Gameplay.Constructs.Enums;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class Tricky() : IStrategy
    {
        // The first 5 - cooperate. If defected just before - defect until the opponent defect. If cooperated last 3 times - defect with a probability. Otherwise, cooperate.

        public string Name { get; private set; } = nameof(Tricky);

        public bool Egotistical { get; private set; } = true;

        private readonly Random Randomizer = new();

        public GameAction DoAction(List<ActionsHistoryItem> ownActions, List<ActionsHistoryItem> opponentActions, int step)
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

            var lastOwnAction2 = ownActions.ElementAtOrDefault(step - 3);
            var lastOwnAction3 = ownActions.ElementAtOrDefault(step - 4);

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
