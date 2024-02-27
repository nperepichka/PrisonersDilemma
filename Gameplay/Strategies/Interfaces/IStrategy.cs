using Gameplay.Enums;
using Gameplay.Games.Tournament;

namespace Gameplay.Strategies.Interfaces
{
    internal interface IStrategy
    {
        string Name { get; }

        Guid Id { get; }

        bool Selfish { get; }

        bool Nice { get; }

        GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step);
    }
}
