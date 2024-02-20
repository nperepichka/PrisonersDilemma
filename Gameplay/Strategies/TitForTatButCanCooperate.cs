using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Strategies
{
    internal class TitForTatButCanCooperate() : IStrategy
    {
        public string Name { get; private set; } = nameof(TitForTatButCanCooperate);

        public bool Egotistical { get; private set; } = false;

        private readonly Random Randomizer = new();

        public GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step)
        {
            var lastOwnAction = ownActions.LastOrDefault();
            var lastOpponentAction = opponentActions.LastOrDefault();
            return lastOpponentAction?.Action == GameAction.Defect
                ? ((lastOwnAction?.Action == GameAction.Defect && Randomizer.Next(10) == 0) ? GameAction.Cooperate : GameAction.Defect)
                : GameAction.Cooperate;
        }
    }
}
