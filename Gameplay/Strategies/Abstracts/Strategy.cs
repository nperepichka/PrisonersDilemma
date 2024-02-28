using Gameplay.Constructs;
using Gameplay.Enums;
using Gameplay.Games.Abstracts;
using Gameplay.Strategies.Interfaces;
using System.Diagnostics;

namespace Gameplay.Strategies.Abstracts
{
    internal abstract class Strategy : IStrategy
    {
        protected Strategy()
        {
            Name = new StackTrace()?.GetFrame(1)?.GetMethod()?.ReflectedType?.Name ?? "Unknown";
            Id = Guid.NewGuid();
        }

        public string Name { get; private set; }

        public Guid Id { get; private set; }

        public virtual bool Selfish => false;

        public virtual bool Nice => false;

        protected static readonly Random Randomizer = new();

        public GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step, Options options)
        {
            return DoAction(new ActionParams(ownActions, opponentActions, cache, step, options));
        }

        public abstract GameAction DoAction(ActionParams actionParams);
    }
}
