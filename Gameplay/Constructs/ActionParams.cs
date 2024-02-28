using Gameplay.Games.Abstracts;

namespace Gameplay.Constructs
{
    internal class ActionParams(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step, Options options)
    {
        public List<HistoryItem> OwnActions => ownActions;

        public List<HistoryItem> OpponentActions => opponentActions;

        public Dictionary<string, object> Cache => cache;

        public int Step => step;

        public Options Options => options;

        public HistoryItem? GetOwnLastItem(int indexFromEnd)
        {
            return OwnActions.ElementAtOrDefault(OwnActions.Count - indexFromEnd);
        }

        public HistoryItem? GetOpponentLastItem(int indexFromEnd)
        {
            return OpponentActions.ElementAtOrDefault(OpponentActions.Count - indexFromEnd);
        }

        public T GetCacheValue<T>(string key, Func<T> initFunc)
        {
            if (Cache.TryGetValue(key, out var val) && val is T tval)
            {
                return tval;
            }
            var res = initFunc();
            Cache.Add(key, res);
            return res;
        }
    }
}
