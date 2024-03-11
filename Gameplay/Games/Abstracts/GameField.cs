using Gameplay.Constructs;
using Gameplay.Enums;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Abstracts
{
    internal abstract class GameField
    {
        public GameField(Options options, params IStrategy[] strategies)
        {
            Options = options;
            Strategies = [];
            Actions = [];
            AddStrategies(strategies);
        }

        protected readonly Random Randomizer = new();

        protected Options Options { get; private set; }

        public List<IStrategy> Strategies { get; private set; }

        public List<History> Actions { get; private set; }

        private void AddStrategies(IStrategy[] strategies)
        {
            if (strategies == null)
            {
                return;
            }

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

        protected GameActionIntensive CalculateActionIntensive(IStrategy strategy, GameAction action, List<HistoryItem> ownActions, List<HistoryItem> opponentActions)
        {
            if (!Options.SelfishFlexible && strategy.Selfish || !Options.HumaneFlexible && !strategy.Selfish)
            {
                return GameActionIntensive.Normal;
            }

            var lastOpponentAction = opponentActions.LastOrDefault();
            var lastOwnAction = ownActions.LastOrDefault();
            return lastOpponentAction?.Action != action || lastOwnAction?.Action != action ? GameActionIntensive.Low : GameActionIntensive.Normal;
        }

        protected double CalculateScore(GameAction ownAction, GameActionIntensive ownActionIntensive, GameAction oppositeAction, GameActionIntensive oppositeActionIntensive)
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

        protected GameAction DoDoActionOrRandom(Func<GameAction> doAction)
        {
            return Options.Seed > 0 && Randomizer.Next(0, 10001) <= Options.Seed * 100
                ? (GameAction)Randomizer.Next(2)
                : doAction();
        }
    }
}
