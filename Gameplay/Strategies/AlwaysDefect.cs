using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class AlwaysDefect() : IStrategy
    {
        public string Name { get; private set; } = nameof(AlwaysDefect);

        public bool Egotistical { get; private set; } = true;

        public GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            return GameAction.Defect;
        }
    }
}
