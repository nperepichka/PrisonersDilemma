using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Abstracts
{
    internal abstract class GameField : IDisposable
    {
        public GameField(params IStrategy[] strategies)
        {
            Strategies = [];
            Actions = [];
            AddStrategies(strategies);
        }

        protected readonly Random Randomizer = new();

        public List<IStrategy> Strategies { get; private set; }

        public List<History> Actions { get; private set; }

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
                    Actions.Add(new History(s.Name, strategy.Name));
                }
            }
        }

        public abstract void DoSteps();

        protected static GameActionIntensive CalculateActionIntensive(IStrategy strategy, GameAction action, List<HistoryItem> ownActions, List<HistoryItem> opponentActions)
        {
            if (!Options.EgotisticalFlexible && strategy.Egotistical || !Options.HumaneFlexible && !strategy.Egotistical)
            {
                return GameActionIntensive.Normal;
            }

            var lastOpponentAction = opponentActions.LastOrDefault();
            var lastOwnAction = ownActions.LastOrDefault();
            return lastOpponentAction?.Action != action || lastOwnAction?.Action != action ? GameActionIntensive.Low : GameActionIntensive.Normal;
        }

        protected static double CalculateScore(GameAction ownAction, GameActionIntensive ownActionIntensive, GameAction oppositeAction, GameActionIntensive oppositeActionIntensive)
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

        private static double GetScoreMod(GameActionIntensive actionIntensive)
        {
            return actionIntensive == GameActionIntensive.Low ? Options.f : 0;
        }

        protected bool ShouldDoRandomAction()
        {
            return Options.Seed > 0 && Randomizer.Next(0, 10001) <= Options.Seed * 100;
        }

        public void Dispose()
        {
            Strategies = null;
            Actions = null;
            GC.Collect();
        }
    }
}
