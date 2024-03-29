﻿using Gameplay.Enums;

namespace Gameplay.Games.Tournament.Constructs
{
    internal class HistoryItem()
    {
        public GameAction Action { get; set; }

        public double Score { get; set; }

        public GameActionIntensive ActionIntensive { get; set; }

    }
}
