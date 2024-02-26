using Gameplay.Enums;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Tournament
{
    internal class GameField(params IStrategy[] strategies) : Abstracts.GameField(strategies)
    {
        public override void DoSteps()
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
                    var strategy1Cache = actions.GetStrategy1Cache();
                    var strategy2Cache = actions.GetStrategy2Cache();

                    var action1 = ShouldDoRandomAction()
                        ? (GameAction)Randomizer.Next(2)
                        : s1.DoAction(strategy1Actions, strategy2Actions, strategy1Cache, step);
                    var action2 = ShouldDoRandomAction()
                        ? (GameAction)Randomizer.Next(2)
                        : s2.DoAction(strategy2Actions, strategy1Actions, strategy2Cache, step);

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

                    if (actions.ShouldStopTournament())
                    {
                        break;
                    }
                    step++;
                }
            });
        }
    }
}
