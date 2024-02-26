using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class Tullock() : Strategy
    {
        public override bool Egotistical => true;

        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step)
        {
            if (step <= 11)
            {
                return GameAction.Cooperate;
            }

            var opponentCooperated = opponentActions.TakeLast(10).Count(_ => _.Action == GameAction.Cooperate);
            return Randomizer.Next(10) < opponentCooperated - 1 ? GameAction.Cooperate : GameAction.Defect;
        }
    }
}
