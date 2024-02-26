using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class AlwaysCooperate() : Strategy
    {
        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            return GameAction.Cooperate;
        }
    }
}
