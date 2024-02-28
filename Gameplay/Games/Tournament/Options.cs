namespace Gameplay.Games.Tournament
{
    internal class Options(
        double flexibility,
        bool humaneFlexible,
        bool selfishFlexible
        ) : Abstracts.Options
    {
        public override int D => 7;
        public override int C => 5;
        public override int d => 2;
        public override int c => 0;

        // Flexibility of interaction (f) - the author's idea of strategy research
        public override bool HumaneFlexible => humaneFlexible;
        public override bool SelfishFlexible => selfishFlexible;

        public override double f => flexibility;

        public readonly int MinSteps = 100;
        public readonly int SameLastCooperationScores = 10;
        public readonly double ValuableCooperationScoreNumber = 0.01;
        public readonly int Repeats = 1;
    }
}
