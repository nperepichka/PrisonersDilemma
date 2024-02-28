using Gameplay.Constructs;
using Gameplay.Enums;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    /// <summary>
    /// Kingmaker
    /// </summary>
    internal class Downing() : Strategy
    {
        public override bool Selfish => true;

        public override GameAction DoAction(ActionParams actionParams)
        {
            if (actionParams.Step == 1)
            {
                return GameAction.Cooperate;
            }

            var lastOpponentAction1 = actionParams.OpponentActions.LastOrDefault();
            if (actionParams.Step == 2)
            {
                if (lastOpponentAction1?.Action == GameAction.Cooperate)
                {
                    actionParams.Cache["number_opponent_cooperations_in_response_to_C"] = 1;
                }
                return GameAction.Defect;
            }

            var lastOwnAction2 = actionParams.GetOwnLastItem(2);
            if (lastOwnAction2?.Action == GameAction.Cooperate && lastOpponentAction1?.Action == GameAction.Cooperate)
            {
                actionParams.Cache["number_opponent_cooperations_in_response_to_C"] = actionParams.GetCacheValue("number_opponent_cooperations_in_response_to_C", () => 0) + 1;
            }
            if (lastOwnAction2?.Action == GameAction.Defect && lastOpponentAction1?.Action == GameAction.Cooperate)
            {
                actionParams.Cache["number_opponent_cooperations_in_response_to_D"] = actionParams.GetCacheValue("number_opponent_cooperations_in_response_to_D", () => 0) + 1;
            }

            double numberOpponentCooperationsInResponseToC = actionParams.GetCacheValue("number_opponent_cooperations_in_response_to_C", () => 0);
            double numberOpponentCooperationsInResponseToD = actionParams.GetCacheValue("number_opponent_cooperations_in_response_to_D", () => 0);
            var ownCooperates = actionParams.OwnActions.Count(_ => _.Action == GameAction.Cooperate);
            var ownDefects = actionParams.OwnActions.Count - ownCooperates;
            var alpha = numberOpponentCooperationsInResponseToC / (ownCooperates + 1);
            var beta = numberOpponentCooperationsInResponseToD / Math.Max(ownDefects, 2);

            var expectedValueOfCooperating = alpha * actionParams.Options.C + (1 - alpha) * actionParams.Options.c;
            var expectedValueOfDefecting = beta * actionParams.Options.D + (1 - beta) * actionParams.Options.d;

            if (expectedValueOfCooperating > expectedValueOfDefecting)
            {
                return GameAction.Cooperate;
            }
            if (expectedValueOfCooperating < expectedValueOfDefecting)
            {
                return GameAction.Defect;
            }

            var lastOwnAction = actionParams.OwnActions.LastOrDefault();
            return lastOwnAction?.Action == GameAction.Cooperate ? GameAction.Defect : GameAction.Cooperate;
        }
    }
}
