using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class TitForTwoTats() : IStrategy
    {
        // Sample

        public string Name { get; private set; } = nameof(TitForTwoTats);

        public bool Egotistical { get; private set; } = false;

        public GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            var lastOpponentAction1 = opponentActions.LastOrDefault();
            var lastOpponentAction2 = opponentActions.ElementAtOrDefault(step - 3);
            return lastOpponentAction1?.Action == GameAction.Defect && lastOpponentAction2?.Action == GameAction.Defect ? GameAction.Defect : GameAction.Cooperate;
        }
    }
}
