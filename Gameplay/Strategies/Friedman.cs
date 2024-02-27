using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class Friedman() : Strategy
    {
        public override bool Nice => true;

        public override bool Selfish => true;

        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step)
        {
            return opponentActions.Any(_ => _.Action == GameAction.Defect) ? GameAction.Defect : GameAction.Cooperate;
        }
    }
}
