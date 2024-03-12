using Gameplay.Enums;
using Gameplay.Games.Tournament.Constructs;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    /// <summary>
    /// Kingmaker
    /// </summary>
    internal class Graaskamp() : Strategy
    {
        public override bool Selfish => true;

        public override GameAction DoAction(ActionParams actionParams)
        {
            if (actionParams.Step == 51)
            {
                return GameAction.Defect;
            }

            if (actionParams.Step <= 56)
            {
                var lastOpponentAction = actionParams.OpponentActions.LastOrDefault();
                return lastOpponentAction?.Action ?? GameAction.Cooperate;
            }

            var isOponentRandom = IsOponentRandom(actionParams.OpponentActions);
            if (isOponentRandom)
            {
                return GameAction.Defect;
            }

            var isOponentTitForTat = IsOponentTitForTat(actionParams.OwnActions, actionParams.OpponentActions);
            if (isOponentTitForTat)
            {
                var lastOpponentAction = actionParams.OpponentActions.LastOrDefault();
                return lastOpponentAction?.Action ?? GameAction.Cooperate;
            }

            if (actionParams.Step == actionParams.GetCacheValue("next_random_defection_turn", () =>
            {
                return actionParams.Step + Randomizer.Next(5, 16);
            }))
            {
                actionParams.Cache["next_random_defection_turn"] = actionParams.Step + Randomizer.Next(5, 16);
                return GameAction.Defect;
            }

            return GameAction.Cooperate;
        }

        private static bool IsOponentRandom(List<HistoryItem> opponentActions)
        {
            /*int[] observed = new int[opponentActions.Count];
            int[] expected = new int[opponentActions.Count];
            var observedSum = 0;
            var expectedSum = 0;

            for (var i = 0; i < opponentActions.Count; i++)
            {
                if (opponentActions[i].Action == GameAction.Cooperate)
                {
                    observedSum++;
                }
                observed[i] = observedSum;
                if (i % 2 == 0)
                {
                    expectedSum++;
                }
                expected[i] = expectedSum;
            }*/

            var opponentCooperates = opponentActions.Select((_, i) => _.Action == GameAction.Cooperate ? 1 * i : 0).Sum();
            var opponentDefects = opponentActions.Select((_, i) => _.Action == GameAction.Defect ? 1 * i : 0).Sum();

            var v1 = Math.Max(opponentCooperates, opponentDefects) + 1.0;
            var v2 = Math.Min(opponentCooperates, opponentDefects) + 1.0;

            return v1 / v2 < 1.001;
        }

        private static bool IsOponentTitForTat(List<HistoryItem> ownActions, List<HistoryItem> opponentActions)
        {
            var isSame = true;
            var isTitForTat = true;
            for (var i = 0; i < ownActions.Count; i++)
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
