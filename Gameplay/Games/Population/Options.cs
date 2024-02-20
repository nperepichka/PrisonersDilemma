namespace Gameplay.Games.Population
{
    internal static class Options
    {
        // D > C > d > c
        // 2C > D + c
        public const int D = 5;
        public const int C = 3;
        public const int d = 1;
        public const int c = 0;

        // Flexibility of interaction (f) - the author's idea of strategy research
        public static bool HumaneFlexible = true;
        public static bool EgotisticalFlexible = true;

        public const double f = 0.25;
    }
}
