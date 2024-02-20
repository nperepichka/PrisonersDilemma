using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class TitForTat() : IStrategy
    {
        public string Name { get; private set; } = "! " + nameof(TitForTat);

        public bool Egotistical { get; private set; } = false;

        public GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            var lastOpponentAction = opponentActions.LastOrDefault();
            return lastOpponentAction?.Action ?? GameAction.Cooperate;
        }
    }
}
