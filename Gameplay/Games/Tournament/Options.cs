namespace Gameplay.Games.Tournament
{
    internal static class Options
    {
        // D > C > d > c
        // 2C > D + c
        public const int D = 7;
        public const int C = 5;
        public const int d = 2;
        public const int c = 0;

        // Flexibility of interaction (f) - the author's idea of strategy research
        public static bool HumaneFlexible = true;
        public static bool SelfishFlexible = true;

        public static double f = 0.5;

        public static int? Steps = null;
        public const int MinSteps = 100;
        public const int SameLastCooperationScores = 10;
        public const double ValuableCooperationScoreNumber = 0.01;
        public const int Repeats = 1;

        // Chance of strategy to play randomly (0.00% - 100.00%)
        public static double Seed = 0;
    }
}
