using Gameplay.Constructs.Enums;
using Gameplay.Constructs.Values;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Constructs
{
    internal class GameField : IDisposable
    {
        public GameField(params IStrategy[] strategies)
        {
            Strategies = [];
            Actions = [];
            AddStrategies(strategies);
        }

        public List<IStrategy> Strategies { get; private set; }

        public List<ActionsHistory> Actions { get; private set; }

        private void AddStrategies(params IStrategy[] strategies)
        {
            foreach (var strategy in strategies)
            {
                if (Strategies.Any(_ => _.Name == strategy.Name))
                {
                    throw new ArgumentException("Strategy with same name already exists");
                }

                Strategies.Add(strategy);

                foreach (var s in Strategies)
                {
                    Actions.Add(new ActionsHistory(s.Name, strategy.Name));
                }
            }
        }

        public void DoSteps()
        {
            Parallel.ForEach(Actions, actions =>
            {
                var step = 1;
                var s1 = Strategies.First(_ => _.Name == actions.Strategy1Name);
                var s2 = Strategies.First(_ => _.Name == actions.Strategy2Name);

                while (true)
                {
                    var strategy1Actions = actions.GetStrategy1Actions();
                    var strategy2Actions = actions.GetStrategy2Actions();

                    var action1 = s1.DoAction(strategy1Actions, strategy2Actions, step);
                    var action2 = s2.DoAction(strategy2Actions, strategy1Actions, step);

                    var action1Intensive = CalculateActionIntensive(s1, action1, strategy1Actions, strategy2Actions);
                    var action2Intensive = CalculateActionIntensive(s2, action2, strategy2Actions, strategy1Actions);

                    var action1Item = new ActionsHistoryItem()
                    {
                        Action = action1,
                        ActionIntensive = action1Intensive,
                        Score = CalculateScore(action1, action1Intensive, action2, action2Intensive)
                    };
                    var action2Item = new ActionsHistoryItem()
                    {
                        Action = action2,
                        ActionIntensive = action2Intensive,
                        Score = CalculateScore(action2, action2Intensive, action1, action1Intensive)
                    };

                    actions.AddAction(action1Item, action2Item);

                    if (actions.ShouldStop())
                    {
                        break;
                    }
                    step++;
                }
            });
        }

        private static GameActionIntensive CalculateActionIntensive(IStrategy strategy, GameAction action, List<ActionsHistoryItem> ownActions, List<ActionsHistoryItem> opponentActions)
        {
            if (!Options.EgotisticalFlexible && strategy.Egotistical || !Options.HumaneFlexible && !strategy.Egotistical)
            {
                return GameActionIntensive.Normal;
            }

            var lastOpponentAction = opponentActions.LastOrDefault();
            var lastOwnAction = ownActions.LastOrDefault();
            return lastOpponentAction?.Action != action || lastOwnAction?.Action != action ? GameActionIntensive.Low : GameActionIntensive.Normal;
        }

        private static double CalculateScore(GameAction ownAction, GameActionIntensive ownActionIntensive, GameAction oppositeAction, GameActionIntensive oppositeActionIntensive)
        {
            double score = ownAction == GameAction.Cooperate
                ? (oppositeAction == GameAction.Cooperate ? Options.C : Options.c)
                : (oppositeAction == GameAction.Cooperate ? Options.D : Options.d);

            if (ownAction == oppositeAction)
            {
                var m = GetScoreMod(oppositeActionIntensive);
                if (ownAction == GameAction.Cooperate)
                {
                    if (m != 0)
                    {
                        score -= m;
                        score -= GetScoreMod(ownActionIntensive);
                    }
                    else
                    {
                        score += GetScoreMod(ownActionIntensive);
                    }
                }
                else
                {
                    if (m != 0)
                    {
                        score += m;
                        score += GetScoreMod(ownActionIntensive);
                    }
                    else
                    {
                        score -= GetScoreMod(ownActionIntensive);
                    }
                }
            }
            else
            {
                if (ownAction == GameAction.Cooperate)
                {
                    score += GetScoreMod(ownActionIntensive);
                    score += GetScoreMod(oppositeActionIntensive);
                }
                else
                {
                    score -= GetScoreMod(ownActionIntensive);
                    score -= GetScoreMod(oppositeActionIntensive);
                }
            }

            return score;
        }

        private static double GetScoreMod(GameActionIntensive actionIntensive)
        {
            return actionIntensive == GameActionIntensive.Low ? Options.f : 0;
        }

        public void Dispose()
        {
            Strategies = null;
            Actions = null;
            GC.Collect();
        }
    }
}
