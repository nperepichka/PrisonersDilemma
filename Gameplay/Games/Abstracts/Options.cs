namespace Gameplay.Games.Abstracts
{
    internal abstract class Options(
        bool humaneFlexible,
        bool selfishFlexible,
        double flexibility
        )
    {
        // D > C > d > c
        // 2C > D + c
        public readonly int D = 7;
        public readonly int C = 5;
        public readonly int d = 2;
        public readonly int c = 0;

        // Flexibility of interaction (f) - the author's idea of strategy research
        public bool HumaneFlexible => humaneFlexible;
        public bool SelfishFlexible => selfishFlexible;

        protected const double DefaultFlexibility = 0.25;
        public double f => flexibility;

        // Chance of strategy to play randomly (0.00% - 100.00%)
        public readonly double Seed = 0;

        public readonly int MinSteps = 100;
    }
}
