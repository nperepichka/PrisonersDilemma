using Gameplay.Constructs.Enums;

namespace Gameplay.Constructs
{
    internal class ActionsHistoryItem()
    {
        public GameAction Action { get; set; }

        public double Score { get; set; }

        public GameActionIntensive ActionIntensive { get; set; }

    }
}
