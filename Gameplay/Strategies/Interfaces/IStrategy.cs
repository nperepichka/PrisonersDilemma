using Gameplay.Constructs;
using Gameplay.Constructs.Enums;

namespace Gameplay.Strategies.Interfaces
{
    internal interface IStrategy
    {
        string Name { get; }

        bool Egotistical { get; }

        GameAction DoAction(List<ActionsHistoryItem> ownActions, List<ActionsHistoryItem> opponentActions, int step);
    }
}
