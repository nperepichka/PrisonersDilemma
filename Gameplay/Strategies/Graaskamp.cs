using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    /// <summary>
    /// Kingmaker
    /// </summary>
    internal class Graaskamp() : Strategy
    {
        public override bool Egotistical => true;

        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step)
        {
            if (step == 51)
            {
                return GameAction.Defect;
            }

            if (step <= 56)
            {
                var lastOpponentAction = opponentActions.LastOrDefault();
                return lastOpponentAction?.Action ?? GameAction.Cooperate;
            }

            var isOponentRandom = IsOponentRandom(opponentActions);
            if (isOponentRandom)
            {
                return GameAction.Defect;
            }

            var isOponentTitForTat = IsOponentTitForTat(ownActions, opponentActions);
            if (isOponentTitForTat)
            {
                var lastOpponentAction = opponentActions.LastOrDefault();
                return lastOpponentAction?.Action ?? GameAction.Cooperate;
            }

            if (step == GetCacheValue(cache, "next_random_defection_turn", () =>
            {
                return step + Randomizer.Next(5,16);
            }))
            {
                cache["next_random_defection_turn"] = step + Randomizer.Next(5, 16);
                return GameAction.Defect;
            }

            return GameAction.Cooperate;
        }

        private static bool IsOponentRandom(List<HistoryItem> opponentActions)
        {
            var opponentCooperates = opponentActions.Select((_, i) => _.Action == GameAction.Cooperate ? 1 * i : 0).Sum();
            var opponentDefects = opponentActions.Select((_, i) => _.Action == GameAction.Defect ? 1 * i : 0).Sum();

            var v1 = Math.Max(opponentCooperates, opponentDefects) + 1.0;
            var v2 = Math.Min(opponentCooperates, opponentDefects) + 1.0;

            return v1 / v2 < 1.01;
        }

        private static bool IsOponentTitForTat(List<HistoryItem> ownActions, List<HistoryItem> opponentActions)
        {
            var isSame = true;
            var isTitForTat = true;
            for (var i = 0; i < ownActions.Count; i++ )
            {
                if (ownActions[i] != opponentActions[i])
                {
                    isSame = false;
                }
                if (i > 0 && opponentActions[i] != ownActions[i - 1])
                {
                    isTitForTat = false;
                }
                if (!isSame && !isTitForTat)
                {
                    break;
                }
            }
            return isSame || isTitForTat;
        }
    }
}
