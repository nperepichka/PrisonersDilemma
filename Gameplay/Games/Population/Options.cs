namespace Gameplay.Games.Population
{
    internal class Options(bool flexible) : Abstracts.Options
    {
        // Flexibility of interaction (f) - the author's idea of strategy research
        public override bool HumaneFlexible => f != 0;
        public override bool SelfishFlexible => f != 0;

        public override double f => flexible ? base.f : 0;

        public readonly int MaxPopulation = 200;
        //public const int PopulationTrimPercentage = 1;
    }
}
