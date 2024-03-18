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

        private List<double> Strategy1Scores { get; set; } = [];

        private List<double> Strategy2Scores { get; set; } = [];

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

        public double GetScore(Guid strategyId, Options options)
        {
            return GetScoresSum(strategyId) * options.MinSteps / (Strategy1Actions.Count * options.C);
        }

        public int GetAggressiveValue(Guid strategyId)
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
            return n * 100 / Strategy1Actions.Count;
        }

        public bool ShouldStopTournament(Options options)
        {
            var step = Strategy1Actions.Count;

            var score1 = GetScore(Strategy1Id, options);
            var score2 = GetScore(Strategy2Id, options);
            Strategy1Scores.Add(score1);
            Strategy2Scores.Add(score2);

            if (step < options.MinSteps)
            {
                return false;
            }

            for (var i = step - 2; i >= step - options.SameLastCooperationScores; i--)
            {
                if (
                    Math.Abs(score1 - Strategy1Scores.ElementAt(i)) > options.ValuableCooperationScoreNumber
                    || Math.Abs(score2 - Strategy2Scores.ElementAt(i)) > options.ValuableCooperationScoreNumber
                    )
                {
                    return false;
                }
            }

            return true;
        }
    }
}
