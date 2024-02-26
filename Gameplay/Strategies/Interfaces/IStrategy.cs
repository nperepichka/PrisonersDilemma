using Gameplay.Enums;
using Gameplay.Games.Tournament;

namespace Gameplay.Strategies.Interfaces
{
    internal interface IStrategy
    {
        string Name { get; }

        string Id { get; }

        bool Egotistical { get; }

        GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, int step);
    }
}
