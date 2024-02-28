namespace Gameplay.Games.Population
{
    internal class Options(bool flexible) : Abstracts.Options
    {
        // D > C > d > c
        // 2C > D + c
        public override int D => 3;
        public override int C => 2;
        public override int d => 1;
        public override int c => 0;

        // Flexibility of interaction (f) - the author's idea of strategy research
        public override bool HumaneFlexible => f != 0;
        public override bool SelfishFlexible => f != 0;

        public override double f => flexible ? base.f : 0;

        public readonly int MaxPopulation = 1000;
        //public const int PopulationTrimPercentage = 1;
    }
}
