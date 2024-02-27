using Gameplay.Enums;
using Gameplay.Games.Tournament;
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

        public abstract GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step);

        protected readonly Random Randomizer = new();

        protected static HistoryItem? GetLastItem(List<HistoryItem> list, int indexFromEnd)
        {
            return list.ElementAtOrDefault(list.Count - indexFromEnd);
        }

        protected static T GetCacheValue<T>(Dictionary<string, object> cache, string key, Func<T> initFunc)
        {
            if (cache.TryGetValue(key, out var val) && val is T tval)
            {
                return tval;
            }
            var res = initFunc();
            cache.Add(key, res);
            return res;
        }
    }
}
