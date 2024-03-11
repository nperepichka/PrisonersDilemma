namespace Gameplay.Games.Population
{
    internal class Options(bool flexible, int stabilizationSteps = 0) : Abstracts.Options
    {
        public override int D => 7;
        public override int C => 5;
        public override int d => 2;
        public override int c => 0;

        // Flexibility of interaction (f) - the author's idea of strategy research
        public override bool HumaneFlexible => f != 0;
        public override bool SelfishFlexible => f != 0;

        public override double f => flexible ? base.f : 0;

        public readonly int StabilizationSteps = stabilizationSteps;
    }
}
