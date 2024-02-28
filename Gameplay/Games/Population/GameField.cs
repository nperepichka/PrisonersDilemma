using Gameplay.Enums;
using Gameplay.Strategies.Interfaces;

namespace Gameplay.Games.Population
{
    internal class GameField(Options options, params IStrategy[] strategies) : Abstracts.GameField(options, strategies)
    {
        public void DoStep()
        {
            // TODO: implement
        }
    }
}
