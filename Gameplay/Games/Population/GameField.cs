using Gameplay.Constructs;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Population
{
    internal class GameField(Options options, int step, params IStrategy[] strategies) : Abstracts.GameField(options, strategies)
    {
        public GameField(Options options, int step, IStrategy[] strategies, List<History> actions) : this(options, step)
        {
            AddStrategies(strategies, actions);
        }

        protected override bool CanCommunicateWithItself => false;

        private void AddStrategies(IStrategy[] strategies, List<History> actions)
        {
            // Moran process will be used to get next generation
            var allStrategies = actions.SelectMany(_ => new[] {
                    new {
                        Name = _.Strategy1Name,
                        Id = _.Strategy1Id,
                    },
                    new {
                        Name = _.Strategy2Name,
                        Id = _.Strategy2Id,
                    }
                })
                .DistinctBy(_ => _.Id)
                .Select(_ => new Generation()
                {
                    Id = _.Id,
                    Name = _.Name,
                    Score = actions
                        .Where(a => a.ContainsStrategy(_.Id))
                        .Select(a => a.GetStrategyLastScore(_.Id))
                        .Sum(),
                    Children = 1,
                })
                .OrderByDescending(_ => _.Score)
                .ThenBy(_ => Randomizer.Next())
                .ToArray();

            var birth = allStrategies.First();
            var death = allStrategies.Last();

            if (birth.Score > death.Score)
            {
                birth.Children = 2;
                death.Children = 0;
            }

            // TODO: fix history clone

            var hash = new Dictionary<Guid, Guid>();

            foreach (var strategyInfo in allStrategies)
            {
                var masterStrategy = strategies.First(_ => _.Name == strategyInfo.Name);
                for (var i = 0; i < strategyInfo.Children; i++)
                {
                    var strategy = masterStrategy.Clone();
                    hash.Add(strategy.Id, strategyInfo.Id);

                    if (Strategies.Any(_ => _.Id == strategy.Id))
                    {
                        throw new ArgumentException("Strategy with same Id already exists");
                    }

                    Strategies.Add(strategy);

                    foreach (var s in Strategies)
                    {
                        if (s.Id != strategy.Id)
                        {
                            var sOldId = hash[s.Id];
                            var history = actions
                                .First(_ => _.ContainsStrategy(strategyInfo.Id) && _.ContainsStrategy(sOldId))
                                .Clone(s, strategy, sOldId, strategyInfo.Id);
                            Actions.Add(history);
                        }
                    }
                }
            }
        }

        private readonly int Step = step;

        public void DoStep()
        {
            Parallel.ForEach(Actions, actions =>
            {
                var s1 = Strategies.First(_ => _.Id == actions.Strategy1Id);
                var s2 = Strategies.First(_ => _.Id == actions.Strategy2Id);

                var strategy1Actions = actions.GetStrategy1Actions();
                var strategy2Actions = actions.GetStrategy2Actions();
                var strategy1Cache = actions.GetStrategy1Cache();
                var strategy2Cache = actions.GetStrategy2Cache();

                var action1 = DoDoActionOrRandom(() =>
                {
                    return s1.DoAction(strategy1Actions, strategy2Actions, strategy1Cache, Step, Options);
                });
                var action2 = DoDoActionOrRandom(() =>
                {
                    return s2.DoAction(strategy2Actions, strategy1Actions, strategy2Cache, Step, Options);
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

            });
        }
    }
}
