using Gameplay.Enums;

namespace Gameplay.Games.Tournament
{
    internal class History(string strategy1Name, string strategy2Name)
    {
        public string Strategy1Name { get; private set; } = strategy1Name;

        public string Strategy2Name { get; private set; } = strategy2Name;

        private List<HistoryItem> Strategy1Actions { get; set; } = [];

        private List<HistoryItem> Strategy2Actions { get; set; } = [];

        private List<double> CooperationScores1 { get; set; } = [];

        private List<double> CooperationScores2 { get; set; } = [];

        private double GetScoresSum(string strategyName)
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
        public bool ShouldStopTournament()
        {
            var step = GetStepsCount();
            var cooperationScore1 = Math.Round(GetScoresSum(Strategy1Name) * 100 / (step * Options.C), 2);
            var cooperationScore2 = Math.Round(GetScoresSum(Strategy2Name) * 100 / (step * Options.C), 2);
            CooperationScores1.Add(cooperationScore1);
            CooperationScores2.Add(cooperationScore2);

            if (step < Options.MinSteps)
            {
                return false;
            }

            for (var i = step - 2; i >= step - 10; i--)
            {
                if (cooperationScore1 != CooperationScores1.ElementAt(i) || cooperationScore2 != CooperationScores2.ElementAt(i))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
