using Gameplay.Enums;
using Gameplay.Games.Tournament.Constructs;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Tournament
{
    internal class GameField(Options options, IList<IStrategy> strategies) :
        Abstracts.GameField<Options>(options, strategies)
    {
        public List<History> Actions { get; private set; }

        protected override void AddStrategies(IList<IStrategy> strategies)
        {
            Actions = [];

            foreach (var strategy in strategies)
            {
                if (Strategies.Any(_ => _.Id == strategy.Id))
                {
                    throw new ArgumentException("Strategy with same Id already exists");
                }

                Strategies.Add(strategy);

                foreach (var s in Strategies)
                {
                    Actions.Add(new History(s, strategy));
                }
            }
        }

        private GameActionIntensive CalculateActionIntensive(IStrategy strategy, GameAction action, List<HistoryItem> ownActions, List<HistoryItem> opponentActions)
        {
            if (!Options.SelfishFlexible && strategy.Selfish || !Options.HumaneFlexible && !strategy.Selfish)
            {
                return GameActionIntensive.Normal;
            }

            var lastOpponentAction = opponentActions.LastOrDefault();
            var lastOwnAction = ownActions.LastOrDefault();
            return lastOpponentAction?.Action != action || lastOwnAction?.Action != action ? GameActionIntensive.Low : GameActionIntensive.Normal;
        }

        private double CalculateScore(GameAction ownAction, GameActionIntensive ownActionIntensive, GameAction oppositeAction, GameActionIntensive oppositeActionIntensive)
        {
            double score = ownAction == GameAction.Cooperate
                ? oppositeAction == GameAction.Cooperate ? Options.C : Options.c
                : oppositeAction == GameAction.Cooperate ? Options.D : Options.d;

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

        private double GetScoreMod(GameActionIntensive actionIntensive)
        {
            return actionIntensive == GameActionIntensive.Low ? Options.f : 0;
        }

        private GameAction DoActionOrRandom(Func<GameAction> doAction)
        {
            return Options.Seed > 0 && Randomizer.Next(0, 10001) <= Options.Seed * 100
                ? (GameAction)Randomizer.Next(2)
                : doAction();
        }

        public void DoSteps()
        {
            Parallel.ForEach(Actions, actions =>
            {
                var step = 1;
                var s1 = Strategies.First(_ => _.Id == actions.Strategy1Id);
                var s2 = Strategies.First(_ => _.Id == actions.Strategy2Id);

                while (true)
                {
                    var strategy1Actions = actions.GetStrategy1Actions();
                    var strategy2Actions = actions.GetStrategy2Actions();
                    var strategy1Cache = actions.GetStrategy1Cache();
                    var strategy2Cache = actions.GetStrategy2Cache();

                    var action1 = DoActionOrRandom(() =>
                    {
                        return s1.DoAction(strategy1Actions, strategy2Actions, strategy1Cache, step, Options);
                    });
                    var action2 = DoActionOrRandom(() =>
                    {
                        return s2.DoAction(strategy2Actions, strategy1Actions, strategy2Cache, step, Options);
                    });

                    var action1Intensive = CalculateActionIntensive(s1, action1, strategy1Actions, strategy2Actions);
                    var action2Intensive = CalculateActionIntensive(s2, action2, strategy2Actions, strategy1Actions);

                    var action1Item = new HistoryItem()
                    {
                        Action = action1,
                        ActionIntensive = action1Intensive,
                        Score = CalculateScore(action1, action1Intensive, action2, action2Intensive)
                    };
                    var action2Item = new HistoryItem()
                    {
                        Action = action2,
                        ActionIntensive = action2Intensive,
                        Score = CalculateScore(action2, action2Intensive, action1, action1Intensive)
                    };

                    actions.AddAction(action1Item, action2Item);

                    if (actions.ShouldStopTournament(options))
                    {
                        break;
                    }
                    step++;
                }
            });
        }
    }
}
