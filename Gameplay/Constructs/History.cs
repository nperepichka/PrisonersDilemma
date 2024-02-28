using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Constructs
{
    internal class History(IStrategy strategy1, IStrategy strategy2)
    {
        public string Strategy1Name { get; private set; } = strategy1.Name;

        public string Strategy2Name { get; private set; } = strategy2.Name;

        public Guid Strategy1Id { get; private set; } = strategy1.Id;

        public Guid Strategy2Id { get; private set; } = strategy2.Id;

        private List<HistoryItem> Strategy1Actions { get; set; } = [];

        private List<HistoryItem> Strategy2Actions { get; set; } = [];

        private List<double> CooperationScores1 { get; set; } = [];

        private List<double> CooperationScores2 { get; set; } = [];

        private Dictionary<string, object> Strategy1Cache { get; set; } = [];

        private Dictionary<string, object> Strategy2Cache { get; set; } = [];

        public bool ContainsStrategy(string strategyName)
        {
            return Strategy1Name == strategyName || Strategy2Name == strategyName;
        }

        public bool ContainsStrategy(Guid strategyId)
        {
            return Strategy1Id == strategyId || Strategy2Id == strategyId;
        }

        public double GetScoresSum(string strategyName)
        {
            double score = 0;
            if (Strategy1Name == strategyName)
            {
                score = Strategy1Actions.Select(_ => _.Score).Sum();
                if (Strategy2Name == strategyName)
                {
                    score = (score + Strategy2Actions.Select(_ => _.Score).Sum()) * 0.5;
                }
            }
            else if (Strategy2Name == strategyName)
            {
                score = Strategy2Actions.Select(_ => _.Score).Sum();
            }
            return score;
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

        public double GetStrategyLastScore(Guid strategyId)
        {
            double score = 0;
            if (Strategy1Id == strategyId)
            {
                score = Strategy1Actions.Last().Score;
                if (Strategy2Id == strategyId)
                {
                    score = (score + Strategy2Actions.Last().Score) * 0.5;
                }
            }
            else if (Strategy2Id == strategyId)
            {
                score = Strategy2Actions.Last().Score;
            }
            return score;
        }

        public double GetScore(string strategyName)
        {
            return GetScoresSum(strategyName) * 100 / GetStepsCount();
        }

        public int GetDefectsCount(string strategyName)
        {
            int n = 0;
            if (Strategy1Name == strategyName)
            {
                n = Strategy1Actions.Count(_ => _.Action == GameAction.Defect);
                if (Strategy2Name == strategyName)
                {
                    n = (n + Strategy2Actions.Count(_ => _.Action == GameAction.Defect)) / 2;
                }
            }
            else if (Strategy2Name == strategyName)
            {
                n = Strategy2Actions.Count(_ => _.Action == GameAction.Defect);
            }
            return n;
        }

        public List<HistoryItem> GetStrategy1Actions()
        {
            return Strategy1Actions;
        }

        public List<HistoryItem> GetStrategy2Actions()
        {
            return Strategy2Actions;
        }

        public Dictionary<string, object> GetStrategy1Cache()
        {
            return Strategy1Cache;
        }

        public Dictionary<string, object> GetStrategy2Cache()
        {
            return Strategy2Cache;
        }

        public void AddAction(HistoryItem strategy1ActionItem, HistoryItem strategy2ActionItem)
        {
            Strategy1Actions.Add(strategy1ActionItem);
            Strategy2Actions.Add(strategy2ActionItem);
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

        public History Clone(IStrategy strategy1, IStrategy strategy2, Guid strategy1OldId, Guid strategy2OldId)
        {
            if (strategy1OldId != Strategy1Id && strategy1OldId != Strategy2Id || strategy2OldId != Strategy1Id && strategy2OldId != Strategy2Id)
            {
                throw new ArgumentException($"Invalid strategies: {strategy1OldId}, {strategy2OldId}");
            }
            var reverseActions = strategy1OldId != Strategy1Id;
            return new History(strategy1, strategy2)
            {
                Strategy1Actions = (reverseActions ? Strategy2Actions : Strategy1Actions).ToList(),
                Strategy2Actions = (reverseActions ? Strategy1Actions : Strategy2Actions).ToList(),
                CooperationScores1 = (reverseActions ? CooperationScores2 : CooperationScores1).ToList(),
                CooperationScores2 = (reverseActions ? CooperationScores1 : CooperationScores2).ToList(),
                Strategy1Cache = (reverseActions ? Strategy2Cache : Strategy1Cache).ToDictionary(k => k.Key, v => v.Value),
                Strategy2Cache = (reverseActions ? Strategy1Cache : Strategy2Cache).ToDictionary(k => k.Key, v => v.Value),
            };
        }
    }
}
