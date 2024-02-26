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
            Id = $"{Name}-{Guid.NewGuid()}";
        }

        public string Name { get; private set; }

        public string Id { get; private set; }

        public virtual bool Egotistical => false;

        public abstract GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step);

        protected readonly Random Randomizer = new();

        protected static HistoryItem? GetLastItem(List<HistoryItem> list, int indexFromEnd)
        {
            return list.ElementAtOrDefault(list.Count - indexFromEnd);
        }
    }
}
