using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class AlwaysCooperate() : IStrategy
    {
        public string Name { get; private set; } = nameof(AlwaysCooperate);

        public bool Egotistical { get; private set; } = false;

        public GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            return GameAction.Cooperate;
        }
    }
}
