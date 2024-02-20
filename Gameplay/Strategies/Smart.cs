using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class Smart() : IStrategy
    {
        // Potentially very interesting strategy
        // Aggressive (according to Axelrod)
        // Egotistical (author's term)
        // More forgiving than TitForTat
        // Tends to exploit too good opponents
        // More successful than TitForTat

        public string Name { get; private set; } = "! " + nameof(Smart);

        public bool Egotistical { get; private set; } = true;

        public GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            if (step <= 5)
            {
                return GameAction.Cooperate;
            }

            if (opponentActions.All(_ => _.Action == GameAction.Cooperate))
            {
                return GameAction.Defect;
            }

            if (opponentActions.All(_ => _.Action == GameAction.Defect))
            {
                return GameAction.Defect;
            }

            var defectsCount = opponentActions.Count(_ => _.Action == GameAction.Defect);
            if (defectsCount * 3 <= opponentActions.Count)
            {
                return GameAction.Cooperate;
            }

            return GameAction.Defect;
        }
    }
}
