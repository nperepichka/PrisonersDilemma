using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    /// <summary>
    /// Kingmaker
    /// </summary>
    internal class Downing() : Strategy
    {
        public override bool Egotistical => true;

        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step)
        {
            if (step == 1)
            {
                return GameAction.Cooperate;
            }

            var lastOpponentAction1 = opponentActions.LastOrDefault();
            if (step == 2)
            {
                if (lastOpponentAction1?.Action == GameAction.Cooperate)
                {
                    cache["number_opponent_cooperations_in_response_to_C"] = 1;
                }
                return GameAction.Defect;
            }

            var lastOwnAction2 = GetLastItem(ownActions, 2);
            if (lastOwnAction2?.Action == GameAction.Cooperate && lastOpponentAction1?.Action == GameAction.Cooperate)
            {
                cache["number_opponent_cooperations_in_response_to_C"] = GetCacheValue(cache, "number_opponent_cooperations_in_response_to_C", () => 0) + 1;
            }
            if (lastOwnAction2?.Action == GameAction.Defect && lastOpponentAction1?.Action == GameAction.Cooperate)
            {
                cache["number_opponent_cooperations_in_response_to_D"] = GetCacheValue(cache, "number_opponent_cooperations_in_response_to_D", () => 0) + 1;
            }

            double number_opponent_cooperations_in_response_to_C = GetCacheValue(cache, "number_opponent_cooperations_in_response_to_C", () => 0);
            double number_opponent_cooperations_in_response_to_D = GetCacheValue(cache, "number_opponent_cooperations_in_response_to_D", () => 0);
            var ownCooperates = ownActions.Count(_ => _.Action == GameAction.Cooperate);
            //var ownDefects = ownActions.Count(_ => _.Action == GameAction.Defect);
            var ownDefects = ownActions.Count - ownCooperates;
            var alpha = number_opponent_cooperations_in_response_to_C / (ownCooperates + 1);
            var beta = number_opponent_cooperations_in_response_to_D / Math.Max(ownDefects, 2);

            var expected_value_of_cooperating = alpha * Options.C + (1 - alpha) * Options.c;
            var expected_value_of_defecting = beta * Options.D + (1 - beta) * Options.d;

            if (expected_value_of_cooperating > expected_value_of_defecting)
            {
                return GameAction.Cooperate;
            }
            if (expected_value_of_cooperating < expected_value_of_defecting)
            {
                return GameAction.Defect;
            }

            var lastOwnAction = ownActions.LastOrDefault();
            return lastOwnAction?.Action == GameAction.Cooperate ? GameAction.Defect : GameAction.Cooperate;
        }
    }
}
