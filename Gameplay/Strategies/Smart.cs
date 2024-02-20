using Gameplay.Constructs;
using Gameplay.Constructs.Enums;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class Smart() : IStrategy
    {
        // Потенційно дуже цікава стратегія
        // Автор: Перепічка Н.В.
        // Агресивна (за Аксельродом)
        // Егоїстична (авторський термін)
        // Більш схильна прощати, ніж TitForTat
        // Схильна експлуатувати надто добрих суперників
        // Успішніша за TitForTat

        public string Name { get; private set; } = "! " + nameof(Smart);

        public bool Egotistical { get; private set; } = true;

        public GameAction DoAction(List<ActionsHistoryItem> ownActions, List<ActionsHistoryItem> opponentActions, int step)
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
