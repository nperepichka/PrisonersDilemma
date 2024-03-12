namespace Gameplay.Games.Tournament
{
    internal class Options(bool humaneFlexible, bool selfishFlexible, double flexibility, double valuableCooperationScoreNumber = 0.01) :
        Abstracts.Options(humaneFlexible, selfishFlexible, flexibility)
    {
        public readonly int SameLastCooperationScores = 10;
        public readonly double ValuableCooperationScoreNumber = valuableCooperationScoreNumber;
        public readonly int Repeats = 1;
    }
}
