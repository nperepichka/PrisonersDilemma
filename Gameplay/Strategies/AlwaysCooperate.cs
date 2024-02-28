﻿using Gameplay.Constructs;
using Gameplay.Enums;
using Gameplay.Strategies.Abstracts;

namespace Gameplay.Strategies
{
    internal class AlwaysCooperate() : Strategy
    {
        public override bool Nice => true;

        public override GameAction DoAction(ActionParams actionParams)
        {
            return GameAction.Cooperate;
        }
    }
}
