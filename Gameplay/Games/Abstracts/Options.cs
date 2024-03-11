namespace Gameplay.Games.Abstracts
{
    internal abstract class Options
    {
        // D > C > d > c
        // 2C > D + c
        public virtual int D => 5;
        public virtual int C => 3;
        public virtual int d => 1;
        public virtual int c => 0;

        // Flexibility of interaction (f) - the author's idea of strategy research
        public abstract bool HumaneFlexible { get; }
        public abstract bool SelfishFlexible { get; }

        public virtual double f => 0.25;

        // Chance of strategy to play randomly (0.00% - 100.00%)
        public readonly double Seed = 0;

        public readonly int MinSteps = 100;
    }
}
