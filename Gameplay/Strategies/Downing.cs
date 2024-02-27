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
        public override bool Selfish => true;

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

            double numberOpponentCooperationsInResponseToC = GetCacheValue(cache, "number_opponent_cooperations_in_response_to_C", () => 0);
            double numberOpponentCooperationsInResponseToD = GetCacheValue(cache, "number_opponent_cooperations_in_response_to_D", () => 0);
            var ownCooperates = ownActions.Count(_ => _.Action == GameAction.Cooperate);
            var ownDefects = ownActions.Count - ownCooperates;
            var alpha = numberOpponentCooperationsInResponseToC / (ownCooperates + 1);
            var beta = numberOpponentCooperationsInResponseToD / Math.Max(ownDefects, 2);

            var expectedValueOfCooperating = alpha * Options.C + (1 - alpha) * Options.c;
            var expectedValueOfDefecting = beta * Options.D + (1 - beta) * Options.d;

            if (expectedValueOfCooperating > expectedValueOfDefecting)
            {
                return GameAction.Cooperate;
            }
            if (expectedValueOfCooperating < expectedValueOfDefecting)
            {
                return GameAction.Defect;
            }

            var lastOwnAction = ownActions.LastOrDefault();
            return lastOwnAction?.Action == GameAction.Cooperate ? GameAction.Defect : GameAction.Cooperate;
        }
    }
}
