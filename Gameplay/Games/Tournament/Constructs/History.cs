using Gameplay.Constructs;
using Gameplay.Enums;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Tournament.Constructs
{
    internal class History(IStrategy strategy1, IStrategy strategy2)
    {
        public Guid Strategy1Id { get; private set; } = strategy1.Id;

        public Guid Strategy2Id { get; private set; } = strategy2.Id;

        public List<HistoryItem> Strategy1Actions { get; private set; } = [];

        public List<HistoryItem> Strategy2Actions { get; private set; } = [];

        private List<double> CooperationScores1 { get; set; } = [];

        private List<double> CooperationScores2 { get; set; } = [];

        public Dictionary<string, object> Strategy1Cache { get; private set; } = [];

        public Dictionary<string, object> Strategy2Cache { get; private set; } = [];

        public bool ContainsStrategy(Guid strategyId)
        {
            return Strategy1Id == strategyId || Strategy2Id == strategyId;
        }

        public double GetScoresSum(Guid strategyId)
        {
            double score = 0;
            if (Strategy1Id == strategyId)
            {
                score = Strategy1Actions.Select(_ => _.Score).Sum();
                if (Strategy2Id == strategyId)
                {
                    score = (score + Strategy2Actions.Select(_ => _.Score).Sum()) * 0.5;
                }
            }
            else if (Strategy2Id == strategyId)
            {
                score = Strategy2Actions.Select(_ => _.Score).Sum();
            }
            return score;
        }

        public double GetScore(Guid strategyId, int minSteps)
        {
            return GetScoresSum(strategyId) * minSteps / GetStepsCount();
        }

        public int GetDefectsCount(Guid strategyId)
        {
            int n = 0;
            if (Strategy1Id == strategyId)
            {
                n = Strategy1Actions.Count(_ => _.Action == GameAction.Defect);
                if (Strategy2Id == strategyId)
                {
                    n = (n + Strategy2Actions.Count(_ => _.Action == GameAction.Defect)) / 2;
                }
            }
            else if (Strategy2Id == strategyId)
            {
                n = Strategy2Actions.Count(_ => _.Action == GameAction.Defect);
            }
            return n;
        }

        public int GetStepsCount()
        {
            return Strategy1Actions.Count;
        }

        // Author's idea for determining the optimal number of iterations, based on the idea from the 2nd Axelrod tournament
        public bool ShouldStopTournament(Options options)
        {
            var step = GetStepsCount();

            var cooperationScore1 = GetScoresSum(Strategy1Id) * 100 / (step * options.C);
            var cooperationScore2 = GetScoresSum(Strategy2Id) * 100 / (step * options.C);
            CooperationScores1.Add(cooperationScore1);
            CooperationScores2.Add(cooperationScore2);

            if (step < options.MinSteps)
            {
                return false;
            }

            for (var i = step - 2; i >= step - options.SameLastCooperationScores; i--)
            {
                if (
                    Math.Abs(cooperationScore1 - CooperationScores1.ElementAt(i)) > options.ValuableCooperationScoreNumber
                    || Math.Abs(cooperationScore2 - CooperationScores2.ElementAt(i)) > options.ValuableCooperationScoreNumber
                    )
                {
                    return false;
                }
            }

            return true;
        }
    }
}
