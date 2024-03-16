using Gameplay.Constructs;
using Gameplay.Enums;
using Gameplay.Games.Tournament.Constructs;
using Gameplay.Strategies.Interfaces;
using System.Diagnostics;

namespace Gameplay.Strategies.Abstracts
{
    internal abstract class Strategy : IStrategy
    {
        protected Strategy()
        {
            Type = (new StackTrace()?.GetFrame(1)?.GetMethod()?.ReflectedType)
                ?? throw new TypeLoadException("Unknow stratedy type");
            Name = Type.Name;
            Id = Guid.NewGuid();
        }

        public string Name { get; private set; }

        public Guid Id { get; private set; }

        public virtual bool Selfish => false;

        public virtual bool Nice => false;

        protected static readonly Random Randomizer = new();

        private Type Type { get; set; }

        public GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step, Options options)
        {
            return DoAction(new ActionParams(ownActions, opponentActions, cache, step, options));
        }

        public abstract GameAction DoAction(ActionParams actionParams);

        public IStrategy Clone()
        {
            return Activator.CreateInstance(Type) is IStrategy strategy
                ? strategy
                : throw new TypeLoadException("Unknow stratedy type");
        }
    }
}
