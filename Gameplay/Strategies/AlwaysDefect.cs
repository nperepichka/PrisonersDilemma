﻿using Gameplay.Enums;
using Gameplay.Games.Tournament;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class AlwaysDefect() : Strategy
    {
        public override bool Egotistical => true;

        public override GameAction DoAction(List<HistoryItem> ownActions, List<HistoryItem> opponentActions, Dictionary<string, object> cache, int step)
        {
            return GameAction.Defect;
        }
    }
}
